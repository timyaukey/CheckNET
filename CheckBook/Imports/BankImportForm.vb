Option Strict Off
Option Explicit On

Imports VB = Microsoft.VisualBasic
Imports CheckBookLib

Friend Class BankImportForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private WithEvents mobjAccount As Account
    Private mobjTrxImport As ITrxImport
    Private mlngStatusSearchType As CBMain.ImportStatusSearch
    Private mlngUpdateSearchType As CBMain.ImportBatchUpdateSearch
    Private mlngNewSearchType As CBMain.ImportBatchNewSearch
    Private mlngBatchUpdateType As CBMain.ImportBatchUpdateType
    Private mlngIndividualSearchType As CBMain.ImportIndividualSearchType
    Private mlngIndividualUpdateType As CBMain.ImportIndividualUpdateType
    Private mblnFake As Boolean

    Private Enum ImportStatus
        'Have not decided what to do with item yet.
        mlngIMPSTS_UNRESOLVED = 0
        'Item was matched to a prior import.
        mlngIMPSTS_PRIOR = 1
        'Item was used to create a new Trx.
        mlngIMPSTS_NEW = 2
        'Item was used to update an existing trx.
        mlngIMPSTS_UPDATE = 3
    End Enum

    'Item obtained from ITrxImport.
    Private Structure ImportItem
        'ImportedTrx created by ITrxImport.
        Dim objImportedTrx As ImportedTrx
        'Register it was added to or updated in.
        Dim objReg As Register
        Dim lngStatus As ImportStatus
        'If set, identifies an exact match to the imported
        'trx in some register.
        Dim objMatchedReg As Register
        Dim objMatchedTrx As Trx
    End Structure

    'Column number in item list with index into maudtItem().
    Private Const mintITMCOL_INDEX As Short = 7
    'Column number in match list with index into maudtMatch().
    Private Const mintMCHCOL_INDEX As Short = 6

    'Imported transaction information.
    Private maudtItem() As ImportItem '1 to mintItems
    Private mintItems As Short

    'Match to an item in maudtItem().
    Private Structure MatchItem
        Dim objTrx As Trx
        Dim objReg As Register
        Dim lngRegIndex As Integer
    End Structure

    'Matches to currently selected ImportItem.
    Private maudtMatch() As MatchItem '1 to mintMatches
    Private mintMatches As Short

    'Register selected in cboRegister.
    Private mobjSelectedRegister As Register

    'String for matching import(s).
    Private mstrImportSearchText As String
    Private mintNextImportToSearch As Short

    Public Sub ShowMe(ByVal strTitle As String, ByVal objAccount As Account, _
                      ByVal objTrxImport As ITrxImport, _
                      ByVal lngStatusSearchType As CBMain.ImportStatusSearch, _
                      ByVal lngUpdateSearchType As CBMain.ImportBatchUpdateSearch, _
                      ByVal lngNewSearchType As CBMain.ImportBatchNewSearch, _
                      ByVal lngIndividualUpdateType As CBMain.ImportIndividualUpdateType, _
                      ByVal lngIndividualSearchType As CBMain.ImportIndividualSearchType, _
                      ByVal lngBatchUpdateType As CBMain.ImportBatchUpdateType, _
                      ByVal blnFake As Boolean)

        Try

            mobjAccount = objAccount
            mobjTrxImport = objTrxImport
            mlngStatusSearchType = lngStatusSearchType
            mlngUpdateSearchType = lngUpdateSearchType
            mlngNewSearchType = lngNewSearchType
            mlngIndividualUpdateType = lngIndividualUpdateType
            mlngIndividualSearchType = lngIndividualSearchType
            mlngBatchUpdateType = lngBatchUpdateType
            mblnFake = blnFake

            mstrImportSearchText = ""
            mintNextImportToSearch = 0
            ShowSearchFor()
            If Not blnLoadImports() Then
                Exit Sub
            End If
            DisplayImportItems()
            LoadRegisterList()
            gLoadComboFromStringTranslator(cboDefaultCategory, gobjCategories, True)

            Me.Text = strTitle
            ConfigureButtons()
            Me.Show()

            Exit Sub
        Catch ex As Exception
            Me.Close()
            gNestedException(ex)
        End Try
    End Sub

    Private Sub ConfigureButtons()
        Select Case mlngNewSearchType
            Case CBMain.ImportBatchNewSearch.None
                cmdBatchNew.Enabled = False
                cmdFindNew.Enabled = False
                cmdCreateNew.Enabled = False
            Case CBMain.ImportBatchNewSearch.Bank
        End Select

        Select Case mlngUpdateSearchType
            Case CBMain.ImportBatchUpdateSearch.None
                cmdBatchUpdates.Enabled = False
                cmdFindUpdates.Enabled = False
        End Select

        Select Case mlngIndividualUpdateType
            Case CBMain.ImportIndividualUpdateType.None
                cmdUpdateExisting.Enabled = False
        End Select
    End Sub

    'UPGRADE_WARNING: Event chkHideCompleted.CheckStateChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub chkHideCompleted_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkHideCompleted.CheckStateChanged
        Try

            DisplayImportItems()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdFindUpdates_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFindUpdates.Click
        Try

            Dim objItem As System.Windows.Forms.ListViewItem
            Dim intItemIndex As Short
            Dim intFoundCount As Short
            Dim strFailReason As String = ""

            ClearUpdateMatches()
            intFoundCount = 0
            For Each objItem In lvwTrx.Items
                objItem.Checked = False

                intItemIndex = CShort(objItem.SubItems(mintITMCOL_INDEX).Text)
                If blnValidForAutoUpdate(intItemIndex, False, strFailReason) Then
                    objItem.Checked = True
                    intFoundCount = intFoundCount + 1
                    With maudtItem(intItemIndex).objMatchedTrx
                        objItem.ToolTipText = gstrFormatDate(.datDate) + " " + .strDescription + " " + gstrFormatCurrency(.curAmount)
                    End With
                Else
                    'UPGRADE_ISSUE: MSComctlLib.ListItem property objItem.ToolTipText was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                    objItem.ToolTipText = strFailReason
                End If
            Next objItem

            MsgBox("Found " & intFoundCount & " imported transactions with exact matches.")

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdBatchUpdates_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBatchUpdates.Click
        Try

            Dim objItem As System.Windows.Forms.ListViewItem
            Dim intItemIndex As Short
            Dim strFailReason As String = ""
            Dim intUpdateCount As Short
            Dim strSummaryExplanation As String = ""
            Dim lngMatchedRegIndex As Integer

            ClearUpdateMatches()
            For Each objItem In lvwTrx.Items
                If objItem.Checked Then

                    intItemIndex = CShort(objItem.SubItems(mintITMCOL_INDEX).Text)
                    If Not blnValidForAutoUpdate(intItemIndex, True, strFailReason) Then
                        objItem.Checked = False
                        MsgBox("Skipping and unchecking " & strDescribeItem(intItemIndex) & " because: " & strFailReason & ".")
                    End If
                End If
            Next objItem

            BeginProgress()
            intUpdateCount = 0
            For Each objItem In lvwTrx.Items
                If objItem.Checked Then

                    intItemIndex = CShort(objItem.SubItems(mintITMCOL_INDEX).Text)
                    With maudtItem(intItemIndex)
                        lngMatchedRegIndex = .objMatchedReg.lngFindTrx(.objMatchedTrx)
                        If lngMatchedRegIndex = 0 Then
                            gRaiseError("Could not find matched Trx")
                        End If
                        Select Case mlngBatchUpdateType

                            Case CBMain.ImportBatchUpdateType.Bank
                                .objMatchedReg.ImportUpdateBank(lngMatchedRegIndex, .objImportedTrx.datDate, .objMatchedTrx.strNumber, mblnFake, .objImportedTrx.curAmount, .objImportedTrx.strImportKey)

                            Case CBMain.ImportBatchUpdateType.Amount
                                .objMatchedReg.ImportUpdateAmount(lngMatchedRegIndex, mblnFake, .objImportedTrx.curAmount)

                            Case CBMain.ImportBatchUpdateType.NumberAmount
                                .objMatchedReg.ImportUpdateNumAmt(lngMatchedRegIndex, .objImportedTrx.strNumber, mblnFake, .objImportedTrx.curAmount)

                            Case Else
                                'Should not be possible.
                                gRaiseError("Invalid batch update type")

                        End Select

                        .lngStatus = ImportStatus.mlngIMPSTS_UPDATE
                        .objReg = .objMatchedReg
                        DisplayOneImportItem(objItem, intItemIndex)
                        intUpdateCount = intUpdateCount + 1
                        UpdateProgress(.objReg)
                    End With
                End If
            Next objItem
            EndProgress()

            Select Case mlngBatchUpdateType
                Case CBMain.ImportBatchUpdateType.Bank
                    strSummaryExplanation = "without changing transaction numbers, or transaction dates."
                Case CBMain.ImportBatchUpdateType.Amount
                    strSummaryExplanation = "updating transaction amounts only."
                Case CBMain.ImportBatchUpdateType.NumberAmount
                    strSummaryExplanation = "updating transaction numbers and amounts."
                Case Else
                    'Should not be possible.
                    gRaiseError("Invalid batch update type")
            End Select

            MsgBox("Marked " & intUpdateCount & " transactions as imported, " & strSummaryExplanation)

            Exit Sub
        Catch ex As Exception
            EndProgress()
            gTopException(ex)
        End Try
    End Sub

    Private Sub ClearUpdateMatches()
        Dim intIndex As Integer
        For intIndex = 1 To mintItems
            maudtItem(intIndex).objMatchedReg = Nothing
            maudtItem(intIndex).objMatchedTrx = Nothing
        Next
    End Sub

    Private Function blnValidForAutoUpdate(ByRef intItemIndex As Short, ByVal blnAllowNonExact As Boolean, ByRef strFailReason As String) As Boolean

        Dim objImportedTrx As ImportedTrx
        Dim objReg As Register
        Dim colMatches As Collection = Nothing
        Dim colExactMatches As Collection = Nothing
        Dim colUnusedMatches As Collection = Nothing
        Dim blnExactMatch As Boolean
        Dim intExactCount As Short
        Dim lngPossibleIndex As Integer
        Dim objPossibleMatchTrx As Trx
        Dim lngNumber As Integer
        Dim blnNonExactConfirmed As Boolean
        Dim blnCheckWithoutAmount As Boolean

        blnValidForAutoUpdate = False
        strFailReason = "Unspecified"

        With maudtItem(intItemIndex)
            If .lngStatus <> ImportStatus.mlngIMPSTS_UNRESOLVED Then
                strFailReason = "Transaction already imported"
                Exit Function
            End If

            objImportedTrx = .objImportedTrx
            'Verify that all the checked imported trx really do have a
            'single exact match, because the user may have checked additional
            'imported trx.
            intExactCount = 0
            lngNumber = Val(objImportedTrx.strNumber)
            For Each objReg In mobjAccount.colRegisters

                Select Case mlngUpdateSearchType
                    Case CBMain.ImportBatchUpdateSearch.Bank
                        objReg.MatchCore(lngNumber, objImportedTrx.datDate, 120, objImportedTrx.strDescription, objImportedTrx.curAmount,
                                         objImportedTrx.curMatchMin, objImportedTrx.curMatchMax, False, colMatches, colExactMatches, blnExactMatch)
                        objReg.PruneToExactMatches(colExactMatches, objImportedTrx.datDate, colMatches, blnExactMatch)
                        colUnusedMatches = colRemoveAlreadyMatched(objReg, colMatches)
                        colUnusedMatches = colApplyNarrowMethodForBank(objReg, objImportedTrx, colMatches, blnExactMatch)
                    Case CBMain.ImportBatchUpdateSearch.Payee
                        objReg.MatchPayee(objImportedTrx.datDate, 7, objImportedTrx.strDescription, False, colMatches, blnExactMatch)
                        colUnusedMatches = colRemoveAlreadyMatched(objReg, colMatches)
                    Case Else
                        'Should not be possible
                        gRaiseError("Invalid batch update search type")
                End Select
                'If we have one match that wasn't matched by a previous import item.
                If colUnusedMatches.Count() = 1 Then
                    blnNonExactConfirmed = False
                    If blnAllowNonExact And Not blnExactMatch Then
                        If MsgBox(strDescribeItem(intItemIndex) & " is only a partial match. " & "Update it anyway?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton1, "Confirm") = MsgBoxResult.Yes Then
                            blnNonExactConfirmed = True
                        End If
                    End If
                    lngPossibleIndex = colUnusedMatches.Item(1)
                    objPossibleMatchTrx = objReg.objTrx(lngPossibleIndex)
                    blnCheckWithoutAmount = False
                    'A check in the register with a zero amount means we didn't know the amount when we entered it, or imported it.
                    If Val(objPossibleMatchTrx.strNumber) > 0 And objPossibleMatchTrx.curAmount = 0.0# Then
                        blnCheckWithoutAmount = True
                    End If
                    If (blnExactMatch Or blnNonExactConfirmed Or blnCheckWithoutAmount) Then
                        If objPossibleMatchTrx.strImportKey = "" Then
                            .objMatchedTrx = objPossibleMatchTrx
                            .objMatchedReg = objReg
                            intExactCount = intExactCount + 1
                        End If
                    End If
                End If
            Next objReg

            If intExactCount = 0 Then
                strFailReason = "Could not find an identical or very similar transaction to update"
                Exit Function
            End If
            If intExactCount > 1 Then
                strFailReason = "Cannot decide between multiple identical or very similar transactions to update"
                Exit Function
            End If

        End With
        blnValidForAutoUpdate = True

    End Function

    'Filter out trx that are already matched to something in maudtItem().
    Private Function colRemoveAlreadyMatched(ByVal objReg As Register, ByVal colMatches As Collection) As Collection
        Dim colUnusedMatches As Collection
        Dim intCheckIndex As Integer
        Dim blnAlreadyMatched As Boolean
        Dim objPossibleMatchTrx As Trx
        Dim vlngPossibleIndex As Object

        colUnusedMatches = New Collection
        For Each vlngPossibleIndex In colMatches
            objPossibleMatchTrx = objReg.objTrx(vlngPossibleIndex)
            blnAlreadyMatched = False
            For intCheckIndex = 1 To mintItems
                If maudtItem(intCheckIndex).objMatchedTrx Is objPossibleMatchTrx Then
                    blnAlreadyMatched = True
                    Exit For
                End If
            Next
            If Not blnAlreadyMatched Then
                colUnusedMatches.Add(vlngPossibleIndex)
            End If
        Next
        Return colUnusedMatches
    End Function

    Private Function colApplyNarrowMethodForBank(ByVal objReg As Register, ByVal objTrx As ImportedTrx, ByVal colUnusedMatches As Collection,
                                                 ByRef blnExactMatch As Boolean) As Collection
        Dim colResult As Collection
        Dim objPossibleMatchTrx As Trx
        Dim vlngPossibleIndex As Object
        Dim datTargetDate As Date
        Dim dblBestDistance As Double
        Dim dblCurrentDistance As Double
        Dim lngBestMatch As Integer
        Dim blnHaveFirstMatch As Boolean

        If colUnusedMatches.Count = 0 Then
            Return colUnusedMatches
        End If

        Select Case objTrx.lngNarrowMethod
            Case ImportMatchNarrowMethod.EarliestDate
                datTargetDate = #1/1/1980#
            Case ImportMatchNarrowMethod.ClosestDate
                datTargetDate = objTrx.datDate
            Case ImportMatchNarrowMethod.None
                Return colUnusedMatches
            Case Else
                gRaiseError("Unrecognized narrowing method")
        End Select

        blnHaveFirstMatch = False
        For Each vlngPossibleIndex In colUnusedMatches
            objPossibleMatchTrx = objReg.objTrx(vlngPossibleIndex)
            If String.IsNullOrEmpty(objPossibleMatchTrx.strImportKey) And (objPossibleMatchTrx.lngStatus <> Trx.TrxStatus.glngTRXSTS_RECON) Then
                dblCurrentDistance = Math.Abs(objPossibleMatchTrx.datDate.Subtract(datTargetDate).TotalDays)
                If (Not blnHaveFirstMatch) Or (dblCurrentDistance < dblBestDistance) Then
                    dblBestDistance = dblCurrentDistance
                    lngBestMatch = vlngPossibleIndex
                    blnHaveFirstMatch = True
                End If
            End If
        Next
        blnExactMatch = True
        colResult = New Collection
        colResult.Add(lngBestMatch)
        Return colResult

    End Function

    Private Sub cmdFindNew_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFindNew.Click
        Try

            Dim objItem As System.Windows.Forms.ListViewItem
            Dim intItemIndex As Short
            Dim intFoundCount As Short
            Dim strFailReason As String = ""

            intFoundCount = 0
            For Each objItem In lvwTrx.Items
                objItem.Checked = False

                intItemIndex = CShort(objItem.SubItems(mintITMCOL_INDEX).Text)
                If blnValidForAutoNew(intItemIndex, False, False, strFailReason) Then
                    objItem.Checked = True
                    intFoundCount = intFoundCount + 1
                    'UPGRADE_ISSUE: MSComctlLib.ListItem property objItem.ToolTipText was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                    objItem.ToolTipText = "Selected"
                Else
                    'UPGRADE_ISSUE: MSComctlLib.ListItem property objItem.ToolTipText was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                    objItem.ToolTipText = strFailReason
                End If
            Next objItem

            MsgBox("Found " & intFoundCount & " imported transactions to turn into new transactions.")

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdBatchNew_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBatchNew.Click
        Try

            Dim objItem As System.Windows.Forms.ListViewItem
            Dim intItemIndex As Short
            Dim intCreateCount As Short
            Dim frm As TrxForm
            Dim datDummy As Date
            Dim objImportedTrx As Trx
            Dim objImportedSplit As TrxSplit
            Dim colPOMatches As Collection = Nothing
            Dim blnItemImported As Boolean
            Dim vlngMatchedTrxIndex As Object
            Dim objMatchedTrx As Trx
            Dim objMatchedSplit As TrxSplit
            Dim strPONumber As String
            Dim blnAllowBankNonCard As Boolean
            Dim strFailReason As String = ""

            If mobjSelectedRegister Is Nothing Then
                MsgBox("First select the register to create the transactions in.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            If MsgBox("Do you really want to create new transactions for everything" & " you have checked?", MsgBoxStyle.Question + MsgBoxStyle.OkCancel + MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Ok Then
                Exit Sub
            End If

            blnAllowBankNonCard = (chkAllowManualBatchNew.CheckState = System.Windows.Forms.CheckState.Checked)

            For Each objItem In lvwTrx.Items
                If objItem.Checked Then

                    intItemIndex = CShort(objItem.SubItems(mintITMCOL_INDEX).Text)
                    If Not blnValidForAutoNew(intItemIndex, blnAllowBankNonCard, True, strFailReason) Then
                        MsgBox("Skipping and unchecking " & strDescribeItem(intItemIndex) & " because: " & strFailReason & ".")
                        objItem.Checked = False
                    End If
                End If
            Next objItem

            BeginProgress()
            intCreateCount = 0
            For Each objItem In lvwTrx.Items
                If objItem.Checked Then

                    intItemIndex = CShort(objItem.SubItems(mintITMCOL_INDEX).Text)
                    blnItemImported = False
                    objImportedTrx = maudtItem(intItemIndex).objImportedTrx
                    'Check if we are importing an invoice that can be matched to a purchase order.
                    'If this happens we update an existing Trx by adding a split rather than creating
                    'a new Trx as would normally be the case in this method.
                    If mlngNewSearchType = CBMain.ImportBatchNewSearch.VendorInvoice Then
                        If objImportedTrx.lngSplits > 0 Then
                            objImportedSplit = objImportedTrx.objFirstSplit
                            strPONumber = objImportedSplit.strPONumber
                            If LCase(strPONumber) = "none" Then
                                strPONumber = ""
                            End If
                            If strPONumber <> "" Then
                                mobjSelectedRegister.MatchPONumber(objImportedTrx.datDate, 14, objImportedTrx.strDescription, strPONumber, colPOMatches)
                                'There should be only one matching Trx, but we'll check all matches
                                'and use the first one with a split with no invoice number. That split
                                'represents the uninvoiced part of the purchase order due on that date.
                                For Each vlngMatchedTrxIndex In colPOMatches
                                    'UPGRADE_WARNING: Couldn't resolve default property of object vlngMatchedTrxIndex. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                    objMatchedTrx = mobjSelectedRegister.objTrx(vlngMatchedTrxIndex)
                                    For Each objMatchedSplit In objMatchedTrx.colSplits
                                        If objMatchedSplit.strPONumber = strPONumber And objMatchedSplit.strInvoiceNum = "" Then
                                            'Add the imported Trx as a new split in objMatchedTrx,
                                            'and reduce the amount of objMatchedSplit by the same amount
                                            'so the total amount of objMatchedTrx does not change.
                                            'UPGRADE_WARNING: Couldn't resolve default property of object vlngMatchedTrxIndex. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                                            mobjSelectedRegister.ImportUpdatePurchaseOrder(vlngMatchedTrxIndex, objMatchedSplit, objImportedSplit)
                                            blnItemImported = True
                                        End If
                                    Next objMatchedSplit
                                Next vlngMatchedTrxIndex
                            End If
                        End If
                    End If
                    'If we did not match the import to a purchase order.
                    If Not blnItemImported Then
                        frm = New TrxForm
                        If Not frm.blnAddNormalSilent(mobjAccount, mobjSelectedRegister, objImportedTrx, datDummy, True, "ImportNewBatch") Then
                            'Either the Trx was silently added, or TrxForm was displayed because
                            'of a validation error and the user successfully fixed the problem
                            'and saved the Trx.
                            blnItemImported = True
                        End If
                    End If
                    'Now update the UI on the import form and any open register forms.
                    If blnItemImported Then
                        With maudtItem(intItemIndex)
                            .lngStatus = ImportStatus.mlngIMPSTS_NEW
                            .objReg = mobjSelectedRegister
                        End With
                        DisplayOneImportItem(objItem, intItemIndex)
                        intCreateCount = intCreateCount + 1
                        UpdateProgress(mobjSelectedRegister)
                    End If
                End If
            Next objItem
            EndProgress()

            MsgBox("Imported " & intCreateCount & " transactions into the selected register.")

            Exit Sub
        Catch ex As Exception
            EndProgress()
            gTopException(ex)
        End Try
    End Sub

    Private Function blnValidForAutoNew(ByRef intItemIndex As Short, ByVal blnAllowBankNonCard As Boolean, ByVal blnSetMissingCategory As Boolean, ByRef strFailReason As String) As Boolean

        Dim objReg As Register
        Dim objImportedTrx As ImportedTrx
        Dim strTrxNum As String
        Dim objSplit As TrxSplit
        Dim lngNumber As Integer
        Dim colMatches As Collection = Nothing
        Dim colExactMatches As Collection = Nothing
        Dim blnExactMatch As Boolean
        Dim lngCatIdx As Integer
        Dim strDefaultCatKey As String

        blnValidForAutoNew = False
        strFailReason = "Unspecified"

        lngNumber = 0
        If maudtItem(intItemIndex).lngStatus <> ImportStatus.mlngIMPSTS_UNRESOLVED Then
            strFailReason = "Transaction already imported"
            Exit Function
        End If

        objImportedTrx = maudtItem(intItemIndex).objImportedTrx
        If objImportedTrx.lngSplits = 0 Then
            strFailReason = "Transaction has no splits"
            Exit Function
        End If
        If objImportedTrx.lngNarrowMethod <> ImportMatchNarrowMethod.None Then
            strFailReason = "Memorized transaction has a narrowing method"
            Exit Function
        End If

        objSplit = objImportedTrx.objFirstSplit
        If objSplit.strCategoryKey = "" And blnSetMissingCategory Then
            If cboDefaultCategory.SelectedIndex <> -1 Then
                lngCatIdx = gintVB6GetItemData(cboDefaultCategory, cboDefaultCategory.SelectedIndex)
                If lngCatIdx > 0 Then
                    strDefaultCatKey = gobjCategories.strKey(lngCatIdx)
                    objSplit.strCategoryKey = strDefaultCatKey
                End If
            End If
        End If
        If objSplit.strCategoryKey = "" Then
            strFailReason = "Transaction has no category"
            Exit Function
        End If

        strTrxNum = LCase(objImportedTrx.strNumber)
        Select Case mlngNewSearchType
            Case CBMain.ImportBatchNewSearch.Bank
                If (strTrxNum <> "card") And Not blnAllowBankNonCard Then
                    strFailReason = "Transaction is not a credit or debit card use"
                    Exit Function
                End If
            Case CBMain.ImportBatchNewSearch.VendorInvoice
                If strTrxNum <> "inv" And strTrxNum <> "crm" Then
                    strFailReason = "Transaction is not an invoice or credit memo"
                    Exit Function
                End If
            Case Else
                'Should not be possible
                gRaiseError("Invalid batch new search type")
        End Select

        For Each objReg In mobjAccount.colRegisters
            colMatches = New Collection
            blnExactMatch = False

            Select Case mlngNewSearchType
                Case CBMain.ImportBatchNewSearch.Bank
                    objReg.MatchCore(lngNumber, objImportedTrx.datDate, 60, objImportedTrx.strDescription, objImportedTrx.curAmount,
                                     objImportedTrx.curMatchMin, objImportedTrx.curMatchMax, False, colMatches, colExactMatches, blnExactMatch)
                    objReg.PruneToExactMatches(colExactMatches, objImportedTrx.datDate, colMatches, blnExactMatch)
                Case CBMain.ImportBatchNewSearch.VendorInvoice
                    objReg.MatchInvoice(objImportedTrx.datDate, 120, objImportedTrx.strDescription, objSplit.strInvoiceNum, colMatches)
                    blnExactMatch = True
                Case Else
                    'Should not be possible
                    gRaiseError("Invalid batch new search type")
            End Select

            If colMatches.Count() > 0 And blnExactMatch Then
                strFailReason = "An identical or very similar transaction already exists"
                Exit Function
            End If
        Next objReg

        blnValidForAutoNew = True

    End Function

    Private Sub BeginProgress()
        'UPGRADE_WARNING: Screen property Screen.MousePointer has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
    End Sub

    Private Sub UpdateProgress(ByVal objReg As Register)
        System.Windows.Forms.Application.DoEvents()
        objReg.RaiseShowCurrent()
    End Sub

    Private Sub EndProgress()
        'UPGRADE_WARNING: Screen property Screen.MousePointer has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
    End Sub

    Private Sub cmdClose_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub cmdRefreshItems_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRefreshItems.Click
        Try

            DisplayImportItems()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function blnLoadImports() As Boolean
        Dim objImportedTrx As ImportedTrx

        Try

            If Not mobjTrxImport.blnOpenSource(mobjAccount) Then
                Exit Function
            End If
            lblReadFrom.Text = "Items read from " & mobjTrxImport.strSource

            blnLoadImports = True
            mintItems = 0
            Erase maudtItem

            Do
                objImportedTrx = mobjTrxImport.objNextTrx()
                If objImportedTrx Is Nothing Then
                    Exit Do
                End If
                mintItems = mintItems + 1
                'UPGRADE_WARNING: Lower bound of array maudtItem was changed from gintLBOUND1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
                ReDim Preserve maudtItem(mintItems)
                With maudtItem(mintItems)
                    'UPGRADE_NOTE: Object maudtItem().objReg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    .objReg = Nothing
                    .objImportedTrx = objImportedTrx
                    .lngStatus = ImportStatus.mlngIMPSTS_UNRESOLVED
                End With
            Loop

            mobjTrxImport.CloseSource()

            Exit Function
        Catch ex As Exception
            mobjTrxImport.CloseSource()
            gNestedException(ex)
        End Try
    End Function

    '$Description Display import items.

    Private Sub DisplayImportItems()
        Dim intIndex As Short
        Dim blnShowCompleted As Boolean
        Dim intOldSelectedIndex As Short
        Dim objNewItem As System.Windows.Forms.ListViewItem = Nothing
        Dim objNewSelectedItem As System.Windows.Forms.ListViewItem = Nothing

        Try

            ClearCurrentItemMatches()
            blnShowCompleted = (chkHideCompleted.CheckState <> System.Windows.Forms.CheckState.Checked)
            If Not lvwTrx.FocusedItem Is Nothing Then
                intOldSelectedIndex = intSelectedItemIndex()
            End If
            lvwTrx.Items.Clear()
            For intIndex = 1 To mintItems
                If maudtItem(intIndex).lngStatus = ImportStatus.mlngIMPSTS_UNRESOLVED Or blnShowCompleted Then
                    blnMatchImport(intIndex)
                    objNewItem = objAddToImportList(intIndex)
                    If intIndex = intOldSelectedIndex Then
                        objNewSelectedItem = objNewItem
                    End If
                End If
            Next
            ClearLvwSelection(lvwTrx)
            If Not objNewSelectedItem Is Nothing Then
                lvwTrx.FocusedItem = objNewSelectedItem
                'UPGRADE_WARNING: MSComctlLib.ListItem method objNewSelectedItem.EnsureVisible has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
                objNewSelectedItem.EnsureVisible()
            End If

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Function objAddToImportList(ByVal intIndex As Short) As System.Windows.Forms.ListViewItem
        Dim objItem As System.Windows.Forms.ListViewItem

        objAddToImportList = Nothing
        Try

            objItem = gobjListViewAdd(lvwTrx)
            DisplayOneImportItem(objItem, intIndex)
            objAddToImportList = objItem

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Sub DisplayOneImportItem(ByVal objItem As System.Windows.Forms.ListViewItem, ByVal intIndex As Short)

        Dim objTrx As ImportedTrx
        Dim strStatus As String = ""

        Try

            objTrx = maudtItem(intIndex).objImportedTrx
            SetTrxSubItems(objTrx, objItem, maudtItem(intIndex).objReg, 6)
            With objItem
                Select Case maudtItem(intIndex).lngStatus
                    Case ImportStatus.mlngIMPSTS_PRIOR
                        strStatus = "Prior"
                    Case ImportStatus.mlngIMPSTS_NEW
                        strStatus = "New"
                    Case ImportStatus.mlngIMPSTS_UPDATE
                        strStatus = "Update"
                End Select

                If objItem.SubItems.Count > 5 Then
                    objItem.SubItems(5).Text = strStatus
                Else
                    objItem.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strStatus))
                End If

                If objItem.SubItems.Count > mintITMCOL_INDEX Then
                    objItem.SubItems(mintITMCOL_INDEX).Text = CStr(intIndex)
                Else
                    objItem.SubItems.Insert(mintITMCOL_INDEX, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(intIndex)))
                End If
            End With

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub LoadRegisterList()
        Dim objReg As Register
        Dim intIndex As Short

        With cboRegister
            .Items.Clear()
            intIndex = 0
            For Each objReg In mobjAccount.colRegisters
                .Items.Add(gobjCreateListBoxItem(objReg.strTitle, intIndex))
                intIndex = intIndex + 1
            Next objReg
        End With
    End Sub

    Private Sub BankImportForm_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Try
            If KeyAscii >= 32 And KeyAscii <= 126 Then
                mstrImportSearchText = mstrImportSearchText & Chr(KeyAscii)
                ShowSearchFor()
                CancelKeyPress(eventArgs)
                Return
            ElseIf KeyAscii = 3 Then  '^C (clear search string)
                mstrImportSearchText = ""
                ShowSearchFor()
                CancelKeyPress(eventArgs)
                Return
            ElseIf KeyAscii = 8 Then  'Backspace (delete last char from search string)
                If Len(mstrImportSearchText) > 0 Then
                    mstrImportSearchText = VB.Left(mstrImportSearchText, Len(mstrImportSearchText) - 1)
                End If
                ShowSearchFor()
                CancelKeyPress(eventArgs)
                Return
            ElseIf KeyAscii = 19 Then  '^S (search for search string)
                FindMatchingImport()
                CancelKeyPress(eventArgs)
                Return
            End If

        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub CancelKeyPress(eventArgs As KeyPressEventArgs)
        eventArgs.KeyChar = Chr(0)
        eventArgs.Handled = True
    End Sub

    Private Sub FindMatchingImport()
        Dim intListItemIndex As Short
        Dim intItemArrayIndex As Short

        If mstrImportSearchText = "" Then
            MsgBox("Type something to search for before pressing ^S to find it.")
            Exit Sub
        End If
        intListItemIndex = mintNextImportToSearch
        Do
            If intListItemIndex >= lvwTrx.Items.Count Then
                MsgBox("Could not find an import item matching """ & mstrImportSearchText & """.")
                Exit Sub
            End If
            intItemArrayIndex = CShort(lvwTrx.Items.Item(intListItemIndex).SubItems(mintITMCOL_INDEX).Text)
            With maudtItem(intItemArrayIndex).objImportedTrx
                If StrComp(.strNumber, mstrImportSearchText, CompareMethod.Text) = 0 Or gstrFormatCurrency(.curAmount) = mstrImportSearchText Or InStr(1, .strDescription, mstrImportSearchText, CompareMethod.Text) > 0 Then
                    lvwTrx.SelectedItems.Clear()
                    lvwTrx.FocusedItem = lvwTrx.Items.Item(intListItemIndex)
                    lvwTrx.FocusedItem.Selected = True
                    lvwTrx.FocusedItem.EnsureVisible()
                    mintNextImportToSearch = intListItemIndex + 1
                    Exit Sub
                End If
            End With
            intListItemIndex = intListItemIndex + 1
        Loop
    End Sub

    Private Sub ShowSearchFor()
        If mstrImportSearchText = "" Then
            lblSearchFor.Text = "Type something and press ^S to search for that text"
        Else
            lblSearchFor.Text = "^S to search for """ & mstrImportSearchText & """, backspace to edit, ^C to start over)"
        End If
        mintNextImportToSearch = 0
    End Sub

    Private Sub BankImportForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Me.Width = 849
        Me.Height = 551
    End Sub

    Private Sub lvwTrx_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lvwTrx.Click
        Try

            If Not lvwTrx.FocusedItem Is Nothing Then
                mintNextImportToSearch = lvwTrx.FocusedItem.Index
                SearchForMatches()
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdRepeatSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRepeatSearch.Click
        Try

            'Did they select anything?
            If lvwTrx.FocusedItem Is Nothing Then
                MsgBox("Select an item in the top list first.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            SearchForMatches()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    'Search for matches to the import item currently selected in the top list.
    'Redisplays the import item with current status if that import item has already been imported.

    Private Sub SearchForMatches()
        Dim objImportedTrx As ImportedTrx
        Dim objReg As Register
        Dim intItemIndex As Short
        Dim lngNumber As Integer
        Dim colMatches As Collection = Nothing
        Dim colExactMatches As Collection = Nothing
        Dim blnExactMatch As Boolean
        Dim vlngRegIndex As Object
        Dim objMatchedTrx As Trx

        Try

            ClearCurrentItemMatches()

            'Has the selected item already been processed?
            intItemIndex = intSelectedItemIndex()
            If maudtItem(intItemIndex).lngStatus <> ImportStatus.mlngIMPSTS_UNRESOLVED Then
                Exit Sub
            End If

            'This is the import item they selected.
            objImportedTrx = maudtItem(intItemIndex).objImportedTrx

            'Not usually possible to match here, because the item would have been matched
            'when loaded and detected a few lines above where it checks the import status.
            'Probably means the matching trx was changed since the imports were loaded,
            'and it only matches now.
            If blnMatchImport(intItemIndex) Then
                RedisplaySelectedItem()
                Exit Sub
            End If

            'Look for possible matches in ALL registers, not just the selected register.
            If IsNumeric(objImportedTrx.strNumber) Then
                lngNumber = CInt(objImportedTrx.strNumber)
            Else
                lngNumber = 0
            End If
            For Each objReg In mobjAccount.colRegisters

                Select Case mlngIndividualSearchType
                    Case CBMain.ImportIndividualSearchType.Bank
                        objReg.MatchCore(lngNumber, objImportedTrx.datDate, 120, objImportedTrx.strDescription, objImportedTrx.curAmount,
                                         objImportedTrx.curMatchMin, objImportedTrx.curMatchMax,
                                         chkLooseMatch.CheckState = System.Windows.Forms.CheckState.Checked, colMatches, colExactMatches, blnExactMatch)
                        objReg.PruneToNonImportedExactMatches(colExactMatches, objImportedTrx.datDate, colMatches, blnExactMatch)
                    Case CBMain.ImportIndividualSearchType.Payee
                        objReg.MatchPayee(objImportedTrx.datDate, 7, objImportedTrx.strDescription, False, colMatches, blnExactMatch)
                    Case CBMain.ImportIndividualSearchType.VendorInvoice
                        objReg.MatchInvoice(objImportedTrx.datDate, 120, objImportedTrx.strDescription, objImportedTrx.objFirstSplit.strInvoiceNum, colMatches)
                        blnExactMatch = True
                    Case Else
                        'Should not be possible
                        gRaiseError("Invalid individual search type")
                End Select
                For Each vlngRegIndex In colMatches
                    objMatchedTrx = objReg.objTrx(vlngRegIndex)
                    'Show the match if it hasn't been imported before,
                    'or we're importing a fake trx. We allow fake trx to be imported
                    'so we can import document information for them - we don't save
                    'their amount or trx number if matched to a real trx.
                    If (Len(objMatchedTrx.strImportKey) = 0 Or objImportedTrx.blnFake) And objMatchedTrx.lngStatus <> Trx.TrxStatus.glngTRXSTS_RECON Then
                        mintMatches = mintMatches + 1
                        ReDim Preserve maudtMatch(mintMatches)
                        With maudtMatch(mintMatches)
                            .objReg = objReg
                            .objTrx = objMatchedTrx
                            .lngRegIndex = vlngRegIndex
                        End With
                        DisplayMatch(objMatchedTrx, mintMatches)
                    End If
                Next vlngRegIndex
            Next objReg
            'Deselect everything in list (the first item is selected by default).
            ClearLvwSelection(lvwMatches)

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    'Look for an existing transaction that matches the specified import item.
    'This sets the import status of the import item.

    Private Function blnMatchImport(ByVal intItemIndex As Short) As Boolean
        Dim objImportedTrx As ImportedTrx
        Dim objReg As Register
        Dim colMatches As Collection = Nothing
        Dim blnExactMatch As Boolean
        Dim lngIndex As Integer
        Dim lngImportMatch As Integer
        Dim lngNumber As Integer

        Try

            'This is the import item they selected.
            objImportedTrx = maudtItem(intItemIndex).objImportedTrx
            If IsNumeric(objImportedTrx.strNumber) Then
                lngNumber = CInt(objImportedTrx.strNumber)
            Else
                lngNumber = 0
            End If

            'Look for an import match in ALL registers, not just the selected register.
            'If found, update maudtItem() and redisplay it with the match info.
            For Each objReg In mobjAccount.colRegisters
                lngImportMatch = 0
                Select Case mlngStatusSearchType
                    Case CBMain.ImportStatusSearch.Bank
                        If objImportedTrx.strImportKey <> "" Then
                            lngImportMatch = objReg.lngMatchImportKey(objImportedTrx.strImportKey)
                        End If
                    Case ImportStatusSearch.BillPayment
                        lngImportMatch = objReg.lngMatchPaymentDetails(objImportedTrx.strNumber, objImportedTrx.datDate, 10, objImportedTrx.strDescription, objImportedTrx.curAmount)
                    Case CBMain.ImportStatusSearch.PayeeNonGenerated
                        objReg.MatchPayee(objImportedTrx.datDate, 7, objImportedTrx.strDescription, True, colMatches, blnExactMatch)
                        If colMatches.Count > 0 Then
                            'UPGRADE_WARNING: Couldn't resolve default property of object colMatches(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            lngIndex = colMatches.Item(1)
                            If Not objReg.objTrx(lngIndex).blnAutoGenerated Then
                                lngImportMatch = lngIndex
                            End If
                        End If
                    Case CBMain.ImportStatusSearch.VendorInvoice
                        'UPGRADE_WARNING: Couldn't resolve default property of object objTrx.colSplits().strInvoiceNum. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                        objReg.MatchInvoice(objImportedTrx.datDate, 120, objImportedTrx.strDescription, objImportedTrx.objFirstSplit.strInvoiceNum, colMatches)
                        If colMatches.Count() > 0 Then
                            'UPGRADE_WARNING: Couldn't resolve default property of object colMatches(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                            lngImportMatch = colMatches.Item(1)
                        End If
                    Case Else
                        'Should not be possible
                        gRaiseError("Invalid import prior match type")
                End Select
                If lngImportMatch > 0 Then
                    maudtItem(intItemIndex).lngStatus = ImportStatus.mlngIMPSTS_PRIOR
                    maudtItem(intItemIndex).objReg = objReg
                    blnMatchImport = True
                    Exit Function
                End If
            Next objReg

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Sub ClearLvwSelection(ByRef lvw As System.Windows.Forms.ListView)
        Dim objItem As System.Windows.Forms.ListViewItem

        'UPGRADE_NOTE: Object lvw.SelectedItem may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        lvw.FocusedItem = Nothing
        For Each objItem In lvw.Items
            If objItem.Selected Then
                objItem.Selected = False
            End If
        Next objItem
    End Sub

    Private Sub ClearCurrentItemMatches()
        lvwMatches.Items.Clear()
        mintMatches = 0
        Erase maudtMatch
    End Sub

    Private Sub DisplayMatch(ByVal objTrx As Trx, ByVal intIndex As Short)
        Dim objItem As System.Windows.Forms.ListViewItem
        objItem = gobjListViewAdd(lvwMatches)
        SetTrxSubItems(objTrx, objItem, maudtMatch(intIndex).objReg, 5)

        If objItem.SubItems.Count > mintMCHCOL_INDEX Then
            objItem.SubItems(mintMCHCOL_INDEX).Text = CStr(intIndex)
        Else
            objItem.SubItems.Insert(mintMCHCOL_INDEX, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(intIndex)))
        End If
    End Sub

    Private Sub SetTrxSubItems(ByVal objTrx As Trx, ByVal objItem As System.Windows.Forms.ListViewItem, ByVal objReg As Register, ByVal intRegColumn As Short)

        Dim strRegTitle As String
        With objItem
            .Text = gstrFormatDate(objTrx.datDate)

            If objItem.SubItems.Count > 1 Then
                objItem.SubItems(1).Text = objTrx.strNumber
            Else
                objItem.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, objTrx.strNumber))
            End If

            If objItem.SubItems.Count > 2 Then
                objItem.SubItems(2).Text = objTrx.strDescription
            Else
                objItem.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, objTrx.strDescription))
            End If

            If objItem.SubItems.Count > 3 Then
                objItem.SubItems(3).Text = gstrFormatCurrency(objTrx.curAmount)
            Else
                objItem.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, gstrFormatCurrency(objTrx.curAmount)))
            End If

            If objItem.SubItems.Count > 4 Then
                objItem.SubItems(4).Text = strSummarizeTrxCat(objTrx)
            Else
                objItem.SubItems.Insert(4, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strSummarizeTrxCat(objTrx)))
            End If
            'For .NET compatibility: You can only set an index immediately after
            'the last existing index, and intRegColumn can be either 5 or 6.

            If objItem.SubItems.Count > 5 Then
                objItem.SubItems(5).Text = ""
            Else
                objItem.SubItems.Insert(5, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, ""))
            End If
            If objReg Is Nothing Then
                strRegTitle = ""
            Else
                strRegTitle = objReg.strTitle
            End If

            If objItem.SubItems.Count > intRegColumn Then
                objItem.SubItems(intRegColumn).Text = strRegTitle
            Else
                objItem.SubItems.Insert(intRegColumn, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strRegTitle))
            End If
        End With
    End Sub

    Private Function strSummarizeTrxCat(ByVal objTrx As Trx) As String

        If objTrx.lngSplits = 1 Then
            strSummarizeTrxCat = gobjCategories.strKeyToValue1(objTrx.objFirstSplit.strCategoryKey)
        Else
            strSummarizeTrxCat = "(split)"
        End If

    End Function

    Private Sub cmdCreateNew_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCreateNew.Click
        Dim frm As TrxForm
        Dim datDummy As Date

        Try

            If Not blnValidImportItemSelected() Then
                Exit Sub
            End If
            If mobjSelectedRegister Is Nothing Then
                MsgBox("First select the register to create the transaction in.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            frm = New TrxForm
            'If chkBypassDateConfirm.value = vbChecked Then
            '    frm.blnBypassConfirmation = True
            'End If
            With maudtItem(intSelectedItemIndex())
                If frm.blnAddNormal(mobjAccount, mobjSelectedRegister, .objImportedTrx, datDummy, True, "Import.CreateNew") Then
                    Exit Sub
                End If
                .lngStatus = ImportStatus.mlngIMPSTS_NEW
                .objReg = mobjSelectedRegister
            End With
            RedisplaySelectedItem()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub lvwTrx_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lvwTrx.DoubleClick
        Dim frm As TrxForm
        Dim datDummy As Date

        Try

            If Not blnValidImportItemSelected() Then
                Exit Sub
            End If
            If mobjSelectedRegister Is Nothing Then
                MsgBox("First select the register to create the transaction in.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            frm = New TrxForm
            'If chkBypassDateConfirm.value = vbChecked Then
            '    frm.blnBypassConfirmation = True
            'End If
            With maudtItem(intSelectedItemIndex())
                If MsgBox("Create transaction " & strDescribeTrx(.objImportedTrx) & "?", MsgBoxStyle.OkCancel + MsgBoxStyle.DefaultButton1, "Create Transaction") <> MsgBoxResult.Ok Then
                    Exit Sub
                End If
                If frm.blnAddNormalSilent(mobjAccount, mobjSelectedRegister, .objImportedTrx, datDummy, True, "ImportNewSilent") Then
                    Exit Sub
                End If
                .lngStatus = ImportStatus.mlngIMPSTS_NEW
                .objReg = mobjSelectedRegister
            End With
            RedisplaySelectedItem()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdUpdateExisting_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdUpdateExisting.Click
        Dim objMatchedReg As Register
        Dim lngMatchedRegIndex As Integer
        Dim objMatchedTrx As Trx
        Dim strNewNumber As String
        Dim curNewAmount As Decimal
        Dim blnPreserveNumAmt As Boolean

        Try

            If Not blnValidImportItemSelected() Then
                Exit Sub
            End If
            If lvwMatches.FocusedItem Is Nothing Then
                MsgBox("First select the matched transaction to update.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            With maudtMatch(intSelectedMatchIndex())
                objMatchedReg = .objReg
                lngMatchedRegIndex = .lngRegIndex
                objMatchedTrx = .objTrx
            End With
            With maudtItem(intSelectedItemIndex())
                With .objImportedTrx

                    Select Case mlngIndividualUpdateType

                        Case CBMain.ImportIndividualUpdateType.Bank
                            blnPreserveNumAmt = (Not objMatchedTrx.blnFake) And .blnFake
                            If (.curAmount <> objMatchedTrx.curAmount) And Not blnPreserveNumAmt Then
                                If MsgBox("NOTE: The amount of the imported transaction is " & "different than the amount of the match you selected. " & "Updating the matched transaction will change its amount to " & "equal the amount of the import." & vbCrLf & vbCrLf & "Do you really want to do this?", MsgBoxStyle.OkCancel + MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Ok Then
                                    MsgBox("Update cancelled.", MsgBoxStyle.Information)
                                    Exit Sub
                                End If
                            End If
                            strNewNumber = .strNumber
                            curNewAmount = .curAmount
                            If blnPreserveNumAmt Then
                                strNewNumber = objMatchedTrx.strNumber
                                curNewAmount = objMatchedTrx.curAmount
                            End If
                            objMatchedReg.ImportUpdateBank(lngMatchedRegIndex, .datDate, strNewNumber, mblnFake, curNewAmount, .strImportKey)

                        Case CBMain.ImportIndividualUpdateType.Amount
                            objMatchedReg.ImportUpdateAmount(lngMatchedRegIndex, mblnFake, .curAmount)

                        Case CBMain.ImportIndividualUpdateType.NumberAmount
                            objMatchedReg.ImportUpdateNumAmt(lngMatchedRegIndex, .strNumber, mblnFake, .curAmount)

                        Case Else
                            'Should not be possible
                            gRaiseError("Invalid individual update type")

                    End Select
                End With
                .lngStatus = ImportStatus.mlngIMPSTS_UPDATE
                .objReg = objMatchedReg
            End With
            RedisplaySelectedItem()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function blnValidImportItemSelected() As Boolean
        If lvwTrx.FocusedItem Is Nothing Then
            MsgBox("First select the import item in the top list.", MsgBoxStyle.Critical)
            Exit Function
        End If
        Select Case maudtItem(intSelectedItemIndex()).lngStatus
            Case ImportStatus.mlngIMPSTS_UNRESOLVED
            Case ImportStatus.mlngIMPSTS_NEW, ImportStatus.mlngIMPSTS_UPDATE
                MsgBox("This import item has already been processed.", MsgBoxStyle.Critical)
                Exit Function
            Case ImportStatus.mlngIMPSTS_PRIOR
                If MsgBox("This item exactly matches an item imported in " & "a previous import session. Do you wish to import it anyway?", MsgBoxStyle.Question + MsgBoxStyle.OkCancel + MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Ok Then
                    Exit Function
                End If
            Case Else
                gRaiseError("Unexpected import status in blnValidImportItemSelected")
        End Select
        blnValidImportItemSelected = True
    End Function

    Private Sub RedisplaySelectedItem()
        DisplayOneImportItem(lvwTrx.FocusedItem, intSelectedItemIndex())
    End Sub

    Private Function intSelectedItemIndex() As Short
        'UPGRADE_WARNING: Lower bound of collection lvwTrx.SelectedItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        intSelectedItemIndex = CShort(lvwTrx.FocusedItem.SubItems(mintITMCOL_INDEX).Text)
    End Function

    Private Function intSelectedMatchIndex() As Short
        'UPGRADE_WARNING: Lower bound of collection lvwMatches.SelectedItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        intSelectedMatchIndex = CShort(lvwMatches.FocusedItem.SubItems(mintMCHCOL_INDEX).Text)
    End Function

    'UPGRADE_WARNING: Event cboRegister.SelectedIndexChanged may fire when form is initialized. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="88B12AE1-6DE0-48A0-86F1-60C0686C026A"'
    Private Sub cboRegister_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboRegister.SelectedIndexChanged
        Try

            With cboRegister
                If .SelectedIndex < 0 Then
                    'UPGRADE_NOTE: Object mobjSelectedRegister may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    mobjSelectedRegister = Nothing
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object mobjAccount.colLoadedRegisters.Item().objReg. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    mobjSelectedRegister = mobjAccount.colRegisters.Item(gintVB6GetItemData(cboRegister, .SelectedIndex))
                End If
            End With

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mobjAccount_ChangeMade() Handles mobjAccount.ChangeMade
        Try
            'Because MatchItem.lngRegIndex may have changed for any matches.
            'Also, this clears the list after "Create New" or "Update Existing".
            ClearCurrentItemMatches()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function strDescribeItem(ByRef intItemIndex As Short) As String
        strDescribeItem = strDescribeTrx(maudtItem(intItemIndex).objImportedTrx)
    End Function

    Private Function strDescribeTrx(ByRef objTrx As Trx) As String
        strDescribeTrx = "[ " & gstrFormatDate(objTrx.datDate) & " " & objTrx.strDescription & " $" & gstrFormatCurrency(objTrx.curAmount) & " ]"
    End Function
End Class