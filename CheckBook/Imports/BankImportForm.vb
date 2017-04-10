Option Strict On
Option Explicit On

Imports VB = Microsoft.VisualBasic
Imports CheckBookLib

Friend Class BankImportForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private WithEvents mobjAccount As Account
    Private mobjCompany As Company
    Private mobjImportHandler As IImportHandler
    Private mobjTrxReader As ITrxReader

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

    'Item obtained from ITrxReader.
    Private Structure ImportItem
        'ImportedTrx created by ITrxReader.
        Dim objImportedTrx As ImportedTrx
        'Register it was added to or updated in.
        Dim objReg As Register
        Dim lngStatus As ImportStatus
        'If set, identifies an exact match to the imported
        'trx in some register.
        Dim objMatchedReg As Register
        Dim objMatchedTrx As NormalTrx
    End Structure

    'Column number in item list with index into maudtItem().
    Private Const mintITMCOL_INDEX As Integer = 7
    'Column number in match list with index into maudtMatch().
    Private Const mintMCHCOL_INDEX As Integer = 10

    'Imported transaction information.
    Private maudtItem() As ImportItem '1 to mintItems
    Private mintItems As Integer

    Private ReadOnly Iterator Property colAllMatchedTrx() As IEnumerable(Of NormalTrx)
        Get
            Dim item As ImportItem
            For Each item In maudtItem
                If Not item.objMatchedTrx Is Nothing Then
                    Yield item.objMatchedTrx
                End If
            Next
        End Get
    End Property

    'Matches to currently selected ImportItem.
    Private maudtMatch() As NormalTrx '1 to mintMatches
    Private mintMatches As Integer

    'Register selected in cboRegister.
    Private mobjSelectedRegister As Register

    'String for matching import(s).
    Private mstrImportSearchText As String
    Private mintNextImportToSearch As Integer

    Public Sub ShowMe(ByVal strTitle As String, ByVal objAccount As Account,
                      ByVal objImportHandler As IImportHandler,
                      ByVal objTrxReader As ITrxReader,
                      ByVal objHostUI As IHostUI)

        Try

            mobjAccount = objAccount
            mobjCompany = mobjAccount.objCompany
            mobjImportHandler = objImportHandler
            mobjTrxReader = objTrxReader

            'This form is an MDI child.
            'This code simulates the VB6 
            ' functionality of automatically
            ' loading and showing an MDI
            ' child's parent.
            Me.MdiParent = objHostUI.objGetMainForm()
            objHostUI.objGetMainForm().Show()

            mstrImportSearchText = ""
            mintNextImportToSearch = 0
            ShowSearchFor()
            If Not blnLoadImports() Then
                Exit Sub
            End If
            DisplayImportItems()
            LoadRegisterList()
            gLoadComboFromStringTranslator(cboDefaultCategory, mobjCompany.objCategories, True)

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
        cmdBatchNew.Enabled = mobjImportHandler.blnAllowNew
        cmdFindNew.Enabled = mobjImportHandler.blnAllowNew
        cmdCreateNew.Enabled = mobjImportHandler.blnAllowNew
        cmdBatchUpdates.Enabled = mobjImportHandler.blnAllowBatchUpdates
        cmdFindUpdates.Enabled = mobjImportHandler.blnAllowBatchUpdates
        cmdUpdateExisting.Enabled = mobjImportHandler.blnAllowIndividualUpdates
    End Sub

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
            Dim intItemIndex As Integer
            Dim intFoundCount As Integer
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
            Dim intItemIndex As Integer
            Dim strFailReason As String = ""
            Dim intUpdateCount As Integer

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
                        mobjImportHandler.BatchUpdate(.objImportedTrx, .objMatchedTrx)
                        .lngStatus = ImportStatus.mlngIMPSTS_UPDATE
                        .objReg = .objMatchedReg
                        DisplayOneImportItem(objItem, intItemIndex)
                        intUpdateCount = intUpdateCount + 1
                        UpdateProgress(.objReg)
                    End With
                End If
            Next objItem
            EndProgress()
            MsgBox("Marked " & intUpdateCount & " transactions as imported, " & mobjImportHandler.strBatchUpdateFields + ".")

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

    Private Function blnValidForAutoUpdate(ByRef intItemIndex As Integer, ByVal blnAllowNonExact As Boolean, ByRef strFailReason As String) As Boolean

        Dim objImportedTrx As ImportedTrx
        Dim objReg As Register
        Dim colUnusedMatches As ICollection(Of NormalTrx) = Nothing
        Dim blnExactMatch As Boolean
        Dim intExactCount As Integer
        Dim objPossibleMatchTrx As NormalTrx
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
            For Each objReg In mobjAccount.colRegisters

                mobjImportHandler.BatchUpdateSearch(objReg, objImportedTrx, colAllMatchedTrx, colUnusedMatches, blnExactMatch)
                'If we have one match that wasn't matched by a previous import item.
                If colUnusedMatches.Count() = 1 Then
                    blnNonExactConfirmed = False
                    If blnAllowNonExact And Not blnExactMatch Then
                        If MsgBox(strDescribeItem(intItemIndex) & " is only a partial match. " & "Update it anyway?",
                                  MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton1, "Confirm") = MsgBoxResult.Yes Then
                            blnNonExactConfirmed = True
                        End If
                    End If
                    objPossibleMatchTrx = gdatFirstElement(colUnusedMatches)
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

    Private Sub cmdFindNew_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdFindNew.Click
        Try

            Dim objItem As System.Windows.Forms.ListViewItem
            Dim intItemIndex As Integer
            Dim intFoundCount As Integer
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
            Dim intItemIndex As Integer
            Dim intCreateCount As Integer
            Dim frm As TrxForm
            Dim datDummy As Date
            Dim objImportedTrx As ImportedTrx
            Dim blnItemImported As Boolean
            Dim blnManualSelectionAllowed As Boolean
            Dim strFailReason As String = ""

            If mobjSelectedRegister Is Nothing Then
                MsgBox("First select the register to create the transactions in.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            If MsgBox("Do you really want to create new transactions for everything" & " you have checked?",
                      MsgBoxStyle.Question Or MsgBoxStyle.OkCancel Or MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Ok Then
                Exit Sub
            End If

            blnManualSelectionAllowed = (chkAllowManualBatchNew.CheckState = System.Windows.Forms.CheckState.Checked)

            For Each objItem In lvwTrx.Items
                If objItem.Checked Then

                    intItemIndex = CShort(objItem.SubItems(mintITMCOL_INDEX).Text)
                    If Not blnValidForAutoNew(intItemIndex, blnManualSelectionAllowed, True, strFailReason) Then
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
                    objImportedTrx = maudtItem(intItemIndex).objImportedTrx
                    blnItemImported = mobjImportHandler.blnAlternateAutoNewHandling(objImportedTrx, mobjSelectedRegister)
                    'If we did not use alternate handling.
                    If Not blnItemImported Then
                        frm = New TrxForm
                        If Not frm.blnAddNormalSilent(mobjSelectedRegister, objImportedTrx, datDummy, True, "ImportNewBatch") Then
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

    Private Function blnValidForAutoNew(ByRef intItemIndex As Integer, ByVal blnManualSelectionAllowed As Boolean, ByVal blnSetMissingCategory As Boolean, ByRef strFailReason As String) As Boolean

        Dim objReg As Register
        Dim objImportedTrx As ImportedTrx
        Dim objSplit As TrxSplit
        Dim colMatches As ICollection(Of NormalTrx) = Nothing
        Dim blnExactMatch As Boolean
        Dim lngCatIdx As Integer
        Dim strDefaultCatKey As String

        blnValidForAutoNew = False
        strFailReason = "Unspecified"

        If maudtItem(intItemIndex).lngStatus <> ImportStatus.mlngIMPSTS_UNRESOLVED Then
            strFailReason = "Transaction already imported"
            Return False
        End If

        objImportedTrx = maudtItem(intItemIndex).objImportedTrx
        If objImportedTrx.lngSplits = 0 Then
            strFailReason = "Transaction has no splits"
            Return False
        End If
        If objImportedTrx.lngNarrowMethod <> ImportMatchNarrowMethod.None Then
            strFailReason = "Memorized transaction has a narrowing method"
            Return False
        End If

        objSplit = objImportedTrx.objFirstSplit
        If objSplit.strCategoryKey = "" And blnSetMissingCategory Then
            If cboDefaultCategory.SelectedIndex <> -1 Then
                lngCatIdx = gintVB6GetItemData(cboDefaultCategory, cboDefaultCategory.SelectedIndex)
                If lngCatIdx > 0 Then
                    strDefaultCatKey = mobjCompany.objCategories.strKey(lngCatIdx)
                    objSplit.strCategoryKey = strDefaultCatKey
                End If
            End If
        End If
        If objSplit.strCategoryKey = "" Then
            strFailReason = "Transaction has no category"
            Return False
        End If

        strFailReason = mobjImportHandler.strAutoNewValidationError(objImportedTrx, mobjAccount, blnManualSelectionAllowed)
        If Not String.IsNullOrEmpty(strFailReason) Then
            Return False
        End If

        For Each objReg In mobjAccount.colRegisters
            colMatches = New List(Of NormalTrx)
            blnExactMatch = False
            mobjImportHandler.AutoNewSearch(objImportedTrx, objReg, colMatches, blnExactMatch)
            If colMatches.Count() > 0 And blnExactMatch Then
                strFailReason = "An identical or very similar transaction already exists"
                Return False
            End If
        Next objReg

        Return True

    End Function

    Private Sub BeginProgress()
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
    End Sub

    Private Sub UpdateProgress(ByVal objReg As Register)
        System.Windows.Forms.Application.DoEvents()
        objReg.RaiseShowCurrent()
    End Sub

    Private Sub EndProgress()
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

            If Not mobjTrxReader.blnOpenSource(mobjAccount) Then
                Exit Function
            End If
            lblReadFrom.Text = "Items read from " & mobjTrxReader.strSource

            blnLoadImports = True
            mintItems = 0
            Erase maudtItem

            Do
                objImportedTrx = mobjTrxReader.objNextTrx()
                If objImportedTrx Is Nothing Then
                    Exit Do
                End If
                mintItems = mintItems + 1
                ReDim Preserve maudtItem(mintItems)
                With maudtItem(mintItems)
                    'UPGRADE_NOTE: Object maudtItem().objReg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    .objReg = Nothing
                    .objImportedTrx = objImportedTrx
                    .lngStatus = ImportStatus.mlngIMPSTS_UNRESOLVED
                End With
            Loop

            Array.Sort(Of ImportItem)(maudtItem, AddressOf ImportItemComparer)

            mobjTrxReader.CloseSource()

            Exit Function
        Catch ex As Exception
            mobjTrxReader.CloseSource()
            gNestedException(ex)
        End Try
    End Function

    Private Function ImportItemComparer(ByVal objItem1 As ImportItem, ByVal objItem2 As ImportItem) As Integer
        Dim intResult As Integer
        If objItem1.objImportedTrx Is Nothing Then
            If objItem2.objImportedTrx Is Nothing Then
                Return 0
            End If
            Return -1
        End If
        If objItem2.objImportedTrx Is Nothing Then
            Return 1
        End If
        intResult = objItem1.objImportedTrx.datDate.CompareTo(objItem2.objImportedTrx.datDate)
        If intResult <> 0 Then
            Return intResult
        End If
        intResult = objItem1.objImportedTrx.strNumber.CompareTo(objItem2.objImportedTrx.strNumber)
        If intResult <> 0 Then
            Return intResult
        End If
        Return objItem1.objImportedTrx.strDescription.CompareTo(objItem2.objImportedTrx.strDescription)
    End Function

    '$Description Display import items.

    Private Sub DisplayImportItems()
        Dim intIndex As Integer
        Dim blnShowCompleted As Boolean
        Dim intOldSelectedIndex As Integer
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
                objNewSelectedItem.EnsureVisible()
            End If

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Function objAddToImportList(ByVal intIndex As Integer) As System.Windows.Forms.ListViewItem
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

    Private Sub DisplayOneImportItem(ByVal objItem As System.Windows.Forms.ListViewItem, ByVal intIndex As Integer)

        Dim objTrx As ImportedTrx = maudtItem(intIndex).objImportedTrx
        Dim objReg As Register = maudtItem(intIndex).objReg
        Dim strStatus As String = ""
        Dim strRegTitle As String = ""

        Try

            Select Case maudtItem(intIndex).lngStatus
                Case ImportStatus.mlngIMPSTS_PRIOR
                    strStatus = "Prior"
                Case ImportStatus.mlngIMPSTS_NEW
                    strStatus = "New"
                Case ImportStatus.mlngIMPSTS_UPDATE
                    strStatus = "Update"
            End Select
            With objItem
                .Text = gstrFormatDate(objTrx.datDate)
                .SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, objTrx.strNumber))
                .SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, objTrx.strDescription))
                .SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, gstrFormatCurrency(objTrx.curAmount)))
                .SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, strSummarizeTrxCat(objTrx)))
                .SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, strStatus))
                If Not objReg Is Nothing Then
                    strRegTitle = objReg.strTitle
                End If
                .SubItems.Insert(6, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strRegTitle))
                .SubItems.Insert(mintITMCOL_INDEX, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(intIndex)))
            End With

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub LoadRegisterList()
        Dim objReg As Register
        Dim intIndex As Integer

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
        Dim KeyAscii As Integer = Asc(eventArgs.KeyChar)
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
        Dim intListItemIndex As Integer
        Dim intItemArrayIndex As Integer

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
        Me.Width = 1115
        Me.Height = 587
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
        Dim intItemIndex As Integer
        Dim colMatches As ICollection(Of NormalTrx) = Nothing
        Dim blnExactMatch As Boolean
        Dim objMatchedTrx As NormalTrx

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
            For Each objReg In mobjAccount.colRegisters
                mobjImportHandler.IndividualSearch(objReg, objImportedTrx,
                    chkLooseMatch.CheckState = System.Windows.Forms.CheckState.Checked,
                    colMatches, blnExactMatch)
                For Each objMatchedTrx In colMatches
                    mintMatches = mintMatches + 1
                    ReDim Preserve maudtMatch(mintMatches)
                    maudtMatch(mintMatches) = objMatchedTrx
                    DisplayMatch(objMatchedTrx, mintMatches)
                Next
            Next objReg

            'Deselect everything in list (the first item is selected by default).
            ClearLvwSelection(lvwMatches)
            'Select the first matched trx which is not already imported,
            'then make a trx a few rows below visible to scroll the first
            'non-imported to the middle of the list control.
            Dim objMatchItem As ListViewItem
            Dim objMatchTrx As NormalTrx
            Dim blnFoundNonImported As Boolean = False
            Dim intNumberVisible As Integer = 0
            For Each objMatchItem In lvwMatches.Items
                objMatchTrx = maudtMatch(CInt(objMatchItem.SubItems(mintMCHCOL_INDEX).Text))
                If objMatchTrx.strImportKey = "" Then
                    If Not blnFoundNonImported Then
                        objMatchItem.Selected = True
                        blnFoundNonImported = True
                    End If
                End If
                If blnFoundNonImported Then
                    objMatchItem.EnsureVisible()
                    intNumberVisible = intNumberVisible + 1
                    If intNumberVisible >= 4 Then
                        Exit For
                    End If
                End If
            Next

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    'Look for an existing transaction that matches the specified import item.
    'This sets the import status of the import item.

    Private Function blnMatchImport(ByVal intItemIndex As Integer) As Boolean
        Dim objImportedTrx As ImportedTrx
        Dim objReg As Register
        Dim objImportMatch As NormalTrx
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
                objImportMatch = mobjImportHandler.objStatusSearch(objImportedTrx, objReg)
                If Not objImportMatch Is Nothing Then
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

    Private Sub DisplayMatch(ByVal objTrx As NormalTrx, ByVal intIndex As Integer)

        Dim objItem As ListViewItem = gobjListViewAdd(lvwMatches)
        Dim strFake As String = ""
        Dim strGen As String = ""
        Dim strImport As String = ""
        If objTrx.blnFake Then
            strFake = "Y"
        End If
        If objTrx.blnAutoGenerated Then
            strGen = "Y"
        End If
        If objTrx.strImportKey <> "" Then
            strImport = "Y"
        End If
        With objItem
            .Text = gstrFormatDate(objTrx.datDate)
            .SubItems.Insert(1, New ListViewItem.ListViewSubItem(Nothing, objTrx.strNumber))
            .SubItems.Insert(2, New ListViewItem.ListViewSubItem(Nothing, objTrx.strDescription))
            .SubItems.Insert(3, New ListViewItem.ListViewSubItem(Nothing, gstrFormatCurrency(objTrx.curAmount)))
            .SubItems.Insert(4, New ListViewItem.ListViewSubItem(Nothing, strSummarizeTrxCat(objTrx)))
            .SubItems.Insert(5, New ListViewItem.ListViewSubItem(Nothing, gstrSummarizeTrxDueDate(objTrx)))
            .SubItems.Insert(6, New ListViewItem.ListViewSubItem(Nothing, strFake))
            .SubItems.Insert(7, New ListViewItem.ListViewSubItem(Nothing, strGen))
            .SubItems.Insert(8, New ListViewItem.ListViewSubItem(Nothing, strImport))
            .SubItems.Insert(9, New ListViewItem.ListViewSubItem(Nothing, objTrx.objReg.strTitle))
            .SubItems.Insert(mintMCHCOL_INDEX, New ListViewItem.ListViewSubItem(Nothing, CStr(intIndex)))
        End With

    End Sub

    Private Function strSummarizeTrxCat(ByVal objTrx As NormalTrx) As String

        If objTrx.lngSplits = 1 Then
            strSummarizeTrxCat = mobjCompany.objCategories.strKeyToValue1(objTrx.objFirstSplit.strCategoryKey)
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
                If frm.blnAddNormal(mobjSelectedRegister, .objImportedTrx, datDummy, True, "Import.CreateNew") Then
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
                If MsgBox("Create transaction " & strDescribeTrx(.objImportedTrx) & "?", MsgBoxStyle.OkCancel Or MsgBoxStyle.DefaultButton1, "Create Transaction") <> MsgBoxResult.Ok Then
                    Exit Sub
                End If
                If frm.blnAddNormalSilent(mobjSelectedRegister, .objImportedTrx, datDummy, True, "ImportNewSilent") Then
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
        Dim objMatchedTrx As NormalTrx

        Try
            If Not blnValidImportItemSelected() Then
                Exit Sub
            End If
            If lvwMatches.FocusedItem Is Nothing Then
                MsgBox("First select the matched transaction to update.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            objMatchedTrx = maudtMatch(intSelectedMatchIndex())
            If objMatchedTrx.strImportKey <> "" Then
                MsgBox("You may not update a transaction that has already been imported.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            With maudtItem(intSelectedItemIndex())
                If mobjImportHandler.blnIndividualUpdate(.objImportedTrx, objMatchedTrx) Then
                    .lngStatus = ImportStatus.mlngIMPSTS_UPDATE
                    .objReg = objMatchedTrx.objReg
                End If
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
                If MsgBox("This item exactly matches an item imported in " & "a previous import session. Do you wish to import it anyway?",
                          MsgBoxStyle.Question Or MsgBoxStyle.OkCancel Or MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Ok Then
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

    Private Function intSelectedItemIndex() As Integer
        intSelectedItemIndex = CShort(lvwTrx.FocusedItem.SubItems(mintITMCOL_INDEX).Text)
    End Function

    Private Function intSelectedMatchIndex() As Integer
        intSelectedMatchIndex = CInt(lvwMatches.FocusedItem.SubItems(mintMCHCOL_INDEX).Text)
    End Function

    Private Sub cboRegister_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboRegister.SelectedIndexChanged
        Try

            With cboRegister
                If .SelectedIndex < 0 Then
                    'UPGRADE_NOTE: Object mobjSelectedRegister may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    mobjSelectedRegister = Nothing
                Else
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

    Private Function strDescribeItem(ByRef intItemIndex As Integer) As String
        strDescribeItem = strDescribeTrx(maudtItem(intItemIndex).objImportedTrx)
    End Function

    Private Function strDescribeTrx(ByRef objTrx As ImportedTrx) As String
        strDescribeTrx = "[ " & gstrFormatDate(objTrx.datDate) & " " & objTrx.strDescription & " $" & gstrFormatCurrency(objTrx.curAmount) & " ]"
    End Function
End Class