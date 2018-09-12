Option Strict On
Option Explicit On

Imports CheckBookLib

Friend Class SearchForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private mfrmReg As RegisterForm
    Private WithEvents mobjReg As Register
    Private mobjAccount As Account
    Private mobjCompany As Company
    Private mdatDefaultDate As Date

    Private Class SearchMatch
        Public objTrx As Trx
    End Class

    Private mintMatchCount As Integer
    Private mcurAmountMatched As Decimal
    Private mcurAmountTotal As Decimal
    Private mblnIgnoreTrxUpdates As Boolean
    Private mblnSkipRemember As Boolean
    Private colCheckedTrx As ICollection(Of Trx)
    Private objSelectedTrx As Trx

    'Parameters of most recent successful search
    Private mlngLastSearchType As Trx.TrxSearchType
    Private mlngLastSearchField As Trx.TrxSearchField
    Private mstrLastSearchFor As String
    Private mdatLastStart As Date
    Private mdatLastEnd As Date
    Private mblnLastIncludeGenerated As Boolean

    Public Sub ShowMe(ByVal objReg_ As Register, ByVal frmReg_ As RegisterForm)

        mobjReg = objReg_
        mobjAccount = mobjReg.objAccount
        mobjCompany = mobjAccount.objCompany
        mfrmReg = frmReg_
        colCheckedTrx = New List(Of Trx)
        mcurAmountMatched = 0
        mcurAmountTotal = 0
        mdatDefaultDate = Today
        Me.Text = "Search " & mobjReg.strTitle
        txtStartDate.Text = Utilities.strFormatDate(DateAdd(Microsoft.VisualBasic.DateInterval.Month, -2, Today))
        txtEndDate.Text = Utilities.strFormatDate(DateAdd(Microsoft.VisualBasic.DateInterval.Month, 6, Today))
        LoadSearchIn()
        LoadSearchType()
        LoadComboFromStringTranslator(cboSearchCats, mobjCompany.objCategories)
        cboSearchIn.SelectedIndex = 0
        cboSearchType.SelectedIndex = 2
        txtSearchFor.Focus()
        Me.Show()

    End Sub

    Private Sub SearchForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Me.Width = 1011
        Me.Height = 547
    End Sub

    Private Sub SearchForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'UPGRADE_NOTE: Object mobjReg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        mobjReg = Nothing
    End Sub

    Private Sub LoadSearchIn()
        cboSearchIn.Items.Clear()
        LoadSearchInOne("Description", Trx.TrxSearchField.Description)
        LoadSearchInOne("Memo", Trx.TrxSearchField.Memo)
        LoadSearchInOne("Category", Trx.TrxSearchField.Category)
        LoadSearchInOne("Number", Trx.TrxSearchField.Number)
        LoadSearchInOne("Amount", Trx.TrxSearchField.Amount)
        LoadSearchInOne("Invoice #", Trx.TrxSearchField.InvoiceNumber)
        LoadSearchInOne("PO #", Trx.TrxSearchField.PONumber)
    End Sub

    Private Sub LoadSearchInOne(ByVal strTitle As String, ByVal lngFieldID As Trx.TrxSearchField)
        With cboSearchIn
            .Items.Add(UITools.CreateListBoxItem(strTitle, lngFieldID))
        End With
    End Sub

    Private Sub LoadSearchType()
        cboSearchType.Items.Clear()
        LoadSearchTypeOne("Equal To", Trx.TrxSearchType.EqualTo)
        LoadSearchTypeOne("Starts With", Trx.TrxSearchType.StartsWith)
        LoadSearchTypeOne("Contains", Trx.TrxSearchType.Contains)
    End Sub

    Private Sub LoadSearchTypeOne(ByVal strTitle As String, ByVal lngTypeID As Trx.TrxSearchType)
        With cboSearchType
            .Items.Add(UITools.CreateListBoxItem(strTitle, lngTypeID))
        End With
    End Sub

    Private Sub LoadComboFromStringTranslator(ByVal cbo As System.Windows.Forms.ComboBox, ByVal objList As IStringTranslator)

        gLoadComboFromStringTranslator(cbo, objList, False)
        With cbo
            .Left = txtSearchFor.Left
            .Top = txtSearchFor.Top
        End With
    End Sub

    Private Sub cboSearchIn_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cboSearchIn.SelectedIndexChanged
        Select Case UITools.GetItemData(cboSearchIn, cboSearchIn.SelectedIndex)
            Case Trx.TrxSearchField.Category
                txtSearchFor.Visible = False
                cboSearchCats.Visible = True
            Case Else
                txtSearchFor.Visible = True
                cboSearchCats.Visible = False
        End Select
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        Dim lngSearchType As Trx.TrxSearchType
        Dim lngSearchField As Trx.TrxSearchField
        Dim strSearchFor As String
        Dim lngItemData As Integer

        Try

            lngSearchType = CType(UITools.GetItemData(cboSearchType, cboSearchType.SelectedIndex), Trx.TrxSearchType)
            lngSearchField = CType(UITools.GetItemData(cboSearchIn, cboSearchIn.SelectedIndex), Trx.TrxSearchField)
            Select Case lngSearchField
                Case Trx.TrxSearchField.Category
                    If lngSearchType <> Trx.TrxSearchType.EqualTo And lngSearchType <> Trx.TrxSearchType.StartsWith Then
                        MsgBox("Category searches must be ""equal to"" or ""starts with"".", MsgBoxStyle.Critical)
                        Exit Sub
                    End If
                    If cboSearchCats.SelectedIndex = -1 Then
                        MsgBox("Please select a category to search for.", MsgBoxStyle.Critical)
                        Exit Sub
                    Else
                        lngItemData = UITools.GetItemData(cboSearchCats, cboSearchCats.SelectedIndex)
                        strSearchFor = mobjCompany.objCategories.strKey(lngItemData)
                        If lngSearchType = Trx.TrxSearchType.StartsWith Then
                            strSearchFor = mobjCompany.objCategories.strKeyToValue1(strSearchFor)
                        End If
                    End If
                Case Else
                    'Allow "search for nothing" so you can find all trx in a date range.
                    'If txtSearchFor = "" Then
                    '    MsgBox "Please enter something to search for.", vbCritical
                    '    Exit Sub
                    'End If
                    strSearchFor = txtSearchFor.Text
            End Select

            If Not Utilities.blnIsValidDate(txtStartDate.Text) Then
                MsgBox("Invalid starting date.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            If Not Utilities.blnIsValidDate(txtEndDate.Text) Then
                MsgBox("Invalid ending date.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            ClearResults()

            mlngLastSearchField = lngSearchField
            mstrLastSearchFor = strSearchFor
            mlngLastSearchType = lngSearchType
            mblnLastIncludeGenerated = (chkIncludeGenerated.CheckState = System.Windows.Forms.CheckState.Checked)
            mdatLastStart = CDate(txtStartDate.Text)
            mdatLastEnd = CDate(txtEndDate.Text)

            SearchInternal()
            RememberSelectedTrx()
            RememberCheckedTrx()

            mcurAmountTotal = mcurAmountTotal + mcurAmountMatched
            ShowTotals()
            MsgBox(mintMatchCount & " matches found.", MsgBoxStyle.Information)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub SearchInternal()
        Dim dlgTrx As Trx.AddSearchMaxTrxDelegate

        Try
            mblnSkipRemember = True
            For Each objTrx As Trx In mobjReg.colDateRange(mdatLastStart, mdatLastEnd)
                If mblnLastIncludeGenerated Or Not objTrx.blnAutoGenerated Then
                    If chkShowAllSplits.Checked And objTrx.lngType = Trx.TrxType.Normal Then
                        dlgTrx = AddressOf AddSearchMatchAllSplits
                    Else
                        dlgTrx = AddressOf AddSearchMatchTrx
                    End If
                    objTrx.CheckSearchMatch(mobjCompany, mlngLastSearchField, mstrLastSearchFor, mlngLastSearchType,
                       dlgTrx, AddressOf AddSearchMatchSplit)
                End If
            Next
        Finally
            mblnSkipRemember = False
        End Try
    End Sub

    Private Sub cmdAddToTotal_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAddToTotal.Click
        Dim dblAmount As Double
        If blnValidAmount(dblAmount) Then
            mcurAmountTotal = mcurAmountTotal + CDec(dblAmount)
            ShowTotals()
        End If
    End Sub

    Private Sub cmdMultTotalBy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMultTotalBy.Click
        Dim dblAmount As Double
        If blnValidAmount(dblAmount) Then
            mcurAmountTotal = mcurAmountTotal * CDec(dblAmount)
            ShowTotals()
        End If
    End Sub

    Private Function blnValidAmount(ByRef dblAmount As Double) As Boolean
        If Len(Trim(txtAddMultAmount.Text)) = 0 Then
            MsgBox("Add/Mult amount is required.", MsgBoxStyle.Critical)
            Exit Function
        End If
        If Not IsNumeric(txtAddMultAmount.Text) Then
            MsgBox("Invalid Add/Mult amount.", MsgBoxStyle.Critical)
            Exit Function
        End If
        dblAmount = CDbl(txtAddMultAmount.Text)
        blnValidAmount = True
    End Function

    Private Sub cmdClearTotal_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdClearTotal.Click
        mcurAmountMatched = 0
        mcurAmountTotal = 0
        ShowTotals()
    End Sub

    Private Sub cmdTotalToClipboard_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdTotalToClipboard.Click
        My.Computer.Clipboard.SetText(Utilities.strFormatCurrency(mcurAmountTotal))
    End Sub

    Private Sub ShowTotals()
        lblTotalDollars.Text = "Matched $" & Utilities.strFormatCurrency(mcurAmountMatched) & "    Total $" & Utilities.strFormatCurrency(mcurAmountTotal)
    End Sub

    Private Sub AddSearchMatchTrx(ByVal objTrx As Trx)

        Dim objItem As ListViewItem
        Dim strCategory As String = ""
        Dim strPONumber As String = ""
        Dim strInvoiceNum As String = ""
        Dim strInvoiceDate As String = ""
        Dim strDueDate As String = ""
        Dim strTerms As String = ""
        Dim strBudget As String = ""
        Dim curAvailable As Decimal

        objItem = objAddNewMatch(objTrx, objTrx.curAmount)
        If objTrx.lngType = Trx.TrxType.Normal Then
            DirectCast(objTrx, NormalTrx).SummarizeSplits(mobjCompany, strCategory, strPONumber, strInvoiceNum, strInvoiceDate, strDueDate, strTerms, strBudget, curAvailable)
            gAddListSubItem(objItem, 4, Utilities.strFormatCurrency(curAvailable))
            gAddListSubItem(objItem, 5, strCategory)
            gAddListSubItem(objItem, 6, strPONumber)
            gAddListSubItem(objItem, 7, strInvoiceNum)
            gAddListSubItem(objItem, 8, strInvoiceDate)
            gAddListSubItem(objItem, 9, strDueDate)
            gAddListSubItem(objItem, 10, strTerms)
            gAddListSubItem(objItem, 11, objTrx.strFakeStatus)
        Else
            gAddListSubItem(objItem, 4, "")
            gAddListSubItem(objItem, 5, "")
            gAddListSubItem(objItem, 6, "")
            gAddListSubItem(objItem, 7, "")
            gAddListSubItem(objItem, 8, "")
            gAddListSubItem(objItem, 9, "")
            gAddListSubItem(objItem, 10, "")
            gAddListSubItem(objItem, 11, "")
        End If
        mcurAmountMatched = mcurAmountMatched + objTrx.curAmount

    End Sub

    Private Sub AddSearchMatchAllSplits(ByVal objTrx As Trx)

        Dim objSplit As TrxSplit
        For Each objSplit In DirectCast(objTrx, NormalTrx).colSplits
            AddSearchMatchSplit(objTrx, objSplit)
        Next

    End Sub

    Private Sub AddSearchMatchSplit(ByVal objTrx As Trx, ByVal objSplit As TrxSplit)

        Dim objItem As ListViewItem
        Dim strInvoiceDate As String
        Dim strDueDate As String
        Dim curAvailable As Decimal

        objItem = objAddNewMatch(objTrx, objSplit.curAmount)
        If objSplit.strBudgetKey = mobjCompany.strPlaceholderBudgetKey Then
            curAvailable = objSplit.curAmount
        Else
            curAvailable = 0
        End If
        If objSplit.datInvoiceDate = System.DateTime.FromOADate(0) Then
            strInvoiceDate = ""
        Else
            strInvoiceDate = Utilities.strFormatDate(objSplit.datInvoiceDate)
        End If
        If objSplit.datDueDate = System.DateTime.FromOADate(0) Then
            strDueDate = ""
        Else
            strDueDate = Utilities.strFormatDate(objSplit.datDueDate)
        End If
        gAddListSubItem(objItem, 4, Utilities.strFormatCurrency(curAvailable))
        gAddListSubItem(objItem, 5, mobjCompany.objCategories.strTranslateKey(objSplit.strCategoryKey))
        gAddListSubItem(objItem, 6, objSplit.strPONumber)
        gAddListSubItem(objItem, 7, objSplit.strInvoiceNum)
        gAddListSubItem(objItem, 8, strInvoiceDate)
        gAddListSubItem(objItem, 9, strDueDate)
        gAddListSubItem(objItem, 10, objSplit.strTerms)
        gAddListSubItem(objItem, 11, objTrx.strFakeStatus)
        mcurAmountMatched = mcurAmountMatched + objSplit.curAmount

    End Sub

    Private Function objAddNewMatch(ByVal objTrx As Trx, ByVal curMatchAmount As Decimal) As ListViewItem
        Dim objItem As ListViewItem = gobjListViewAdd(lvwMatches)
        objItem.Text = Utilities.strFormatDate(objTrx.datDate)
        gAddListSubItem(objItem, 1, objTrx.strNumber)
        gAddListSubItem(objItem, 2, objTrx.strDescription)
        gAddListSubItem(objItem, 3, Utilities.strFormatCurrency(curMatchAmount))
        Dim objMatch As SearchMatch = New SearchMatch()
        objMatch.objTrx = objTrx
        objItem.Tag = objMatch
        mintMatchCount = mintMatchCount + 1
        Return objItem
    End Function

    Private Sub ClearResults()
        lvwMatches.Items.Clear()
        mintMatchCount = 0
        mcurAmountMatched = 0
    End Sub

    Private Sub lvwMatches_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lvwMatches.Click
        Try

            If lvwMatches.FocusedItem Is Nothing Then
                Exit Sub
            End If
            Dim objMatch As SearchMatch = DirectCast(lvwMatches.FocusedItem.Tag, SearchMatch)
            mobjReg.SetCurrent(objMatch.objTrx.lngIndex)
            mobjReg.RaiseShowCurrent()
            RememberSelectedTrx()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub lvwMatches_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lvwMatches.DoubleClick
        cmdEditTrx_Click(cmdEditTrx, New System.EventArgs())
    End Sub

    Private Sub lvwMatches_ItemChecked(sender As Object, eventArgs As ItemCheckedEventArgs) Handles lvwMatches.ItemChecked
        Try
            RememberCheckedTrx()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelect.Click
        Try

            Dim objItem As System.Windows.Forms.ListViewItem

            For Each objItem In lvwMatches.Items
                objItem.Checked = Not objItem.Checked
            Next objItem
            RememberCheckedTrx()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdEditTrx_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEditTrx.Click
        Try
            Dim objTrx As Trx
            Dim objMatch As SearchMatch
            If lvwMatches.FocusedItem Is Nothing Then
                Exit Sub
            End If
            objMatch = DirectCast(lvwMatches.FocusedItem.Tag, SearchMatch)
            objTrx = objMatch.objTrx
            If objTrx.lngType = Trx.TrxType.Replica Then
                MsgBox("You may not edit a replica transaction directly. Instead edit the split it was created from in another transaction.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            Using frmEdit As TrxForm = frmCreateTrxForm()
                If frmEdit.blnUpdate(objTrx.lngIndex, mobjReg, mdatDefaultDate, "SearchForm.Edit") Then
                    Exit Sub
                End If
            End Using
            mobjReg.ValidateRegister()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdNewNormal_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNewNormal.Click
        Try
            Dim objTrx As NormalTrx = New NormalTrx(mobjReg)
            objTrx.NewEmptyNormal(mdatDefaultDate)
            Using frm As TrxForm = frmCreateTrxForm()
                If frm.blnAddNormal(mobjReg, objTrx, mdatDefaultDate, True, "SearchForm.NewNormal") Then
                    MsgBox("Canceled.")
                End If
            End Using
            mobjReg.ValidateRegister()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function frmCreateTrxForm() As TrxForm
        Return New TrxForm()
    End Function

    ''' <summary>
    ''' Enumerate all Trx objects for selected list items in the search results.
    ''' If multiple splits for the same Trx are included in the search results
    ''' will only return that Trx once. Assumes all splits for the same Trx
    ''' are adjacent in the search results.
    ''' </summary>
    ''' <returns></returns>
    Private Iterator Function colGetCheckedTrx() As IEnumerable(Of Trx)
        Dim objItem As ListViewItem
        Dim objTrx As Trx
        Dim objLastTrx As Trx = Nothing
        For Each objItem In lvwMatches.Items
            If objItem.Checked Then
                objTrx = DirectCast(objItem.Tag, SearchMatch).objTrx
                If Not objTrx Is objLastTrx Then
                    objLastTrx = objTrx
                    Yield objTrx
                End If
            End If
        Next
    End Function

    Private Sub cmdExport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExport.Click
        Dim frmExport As ExportForm
        frmExport = New ExportForm
        Try

            Dim objTrx As Trx
            Dim colSplits As IEnumerable(Of TrxSplit)
            Dim objSplit As TrxSplit
            Dim lngExportCount As Integer

            If Not frmExport.blnGetSettings(mobjCompany) Then
                MsgBox("Export canceled.")
                Exit Sub
            End If

            frmExport.OpenOutput()

            lngExportCount = 0
            For Each objTrx In colGetCheckedTrx()
                'Ignore budgets and transfers instead of showing an error, because
                'it is common to export all trx in a date range except these.
                If objTrx.lngType = Trx.TrxType.Normal Then
                    colSplits = DirectCast(objTrx, NormalTrx).colSplits
                    For Each objSplit In colSplits
                        frmExport.WriteSplit(objTrx, objSplit)
                        lngExportCount = lngExportCount + 1
                    Next objSplit
                End If
            Next

            frmExport.CloseOutput()
            frmExport.Close()

            MsgBox(lngExportCount & " splits exported.", MsgBoxStyle.Information)

            Exit Sub
        Catch ex As Exception
            If Not frmExport Is Nothing Then
                frmExport.Close()
            End If
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdRecategorize_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRecategorize.Click
        Try

            Dim frmArgs As ChangeCategoryForm
            Dim strOldCatKey As String = ""
            Dim strNewCatKey As String = ""
            Dim colTrx As ICollection(Of NormalTrx)
            Dim objCheckedTrx As Trx
            Dim objNormalTrx As NormalTrx
            Dim objTrxManager As NormalTrxManager
            Dim colSplits As IEnumerable(Of TrxSplit)
            Dim objSplit As TrxSplit
            Dim strCatKey As String
            Dim lngChgCount As Integer
            Dim objStartLogger As ILogGroupStart

            frmArgs = New ChangeCategoryForm
            If Not frmArgs.blnGetCategories(mobjCompany, strOldCatKey, strNewCatKey) Then
                Exit Sub
            End If

            colTrx = New List(Of NormalTrx)
            For Each objCheckedTrx In colGetCheckedTrx()
                If objCheckedTrx.lngType <> Trx.TrxType.Normal Then
                    MsgBox("Budgets and transfers may not be recategorized.", MsgBoxStyle.Critical)
                    Exit Sub
                End If
                If objCheckedTrx.blnAutoGenerated Then
                    MsgBox("Generated transactions may not be recategorized.", MsgBoxStyle.Critical)
                    Exit Sub
                End If
                colSplits = DirectCast(objCheckedTrx, NormalTrx).colSplits
                For Each objSplit In colSplits
                    With objSplit
                        If objSplit.strCategoryKey = strOldCatKey Then
                            colTrx.Add(DirectCast(objCheckedTrx, NormalTrx))
                            Exit For
                        End If
                    End With
                Next objSplit
            Next
            If colTrx.Count() = 0 Then
                MsgBox("No transactions selected, or none that use the old category.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            objStartLogger = mobjReg.objLogGroupStart("SearchForm.Recategorize")
            For Each objNormalTrx In colTrx
                objTrxManager = objNormalTrx.objGetTrxManager()
                colSplits = objNormalTrx.colSplits
                objTrxManager.UpdateStart()
                objNormalTrx.ClearSplits()
                For Each objSplit In colSplits
                    With objSplit
                        strCatKey = objSplit.strCategoryKey
                        If strCatKey = strOldCatKey Then
                            strCatKey = strNewCatKey
                        End If
                        objNormalTrx.AddSplit(.strMemo, strCatKey, .strPONumber, .strInvoiceNum, .datInvoiceDate, .datDueDate, .strTerms, .strBudgetKey, .curAmount)
                    End With
                Next objSplit
                objTrxManager.UpdateEnd(New LogChange, "SearchForm.Recategorize")
                lngChgCount = lngChgCount + 1
            Next
            mobjReg.LogGroupEnd(objStartLogger)

            MsgBox("Changed category of " & lngChgCount & " transactions.")

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdCombine_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCombine.Click
        Try

            Dim objNewTrx As NormalTrx = Nothing
            Dim objOldTrx As NormalTrx
            Dim colOldTrx As ICollection(Of Trx)
            Dim objOldSplit As TrxSplit
            Dim objStartLogger As ILogGroupStart
            Dim datToday As Date
            Dim datResult As Date

            'Build the new Trx from selected (checked) Trx.
            'Use the first Trx for Trx level data, and clone the splits of all Trx.
            'Keep a collection of the chosen Trx, to delete them at the end.
            colOldTrx = New List(Of Trx)
            For Each objOldTrx In colGetCheckedTrx()
                If Not blnValidTrxForBulkOperation(objOldTrx, "combined") Then
                    Exit Sub
                End If
                'If we do not yet have a new trx, create it.
                If objNewTrx Is Nothing Then
                    objNewTrx = New NormalTrx(mobjReg)
                    datToday = Today
                    objNewTrx.NewStartNormal(True, "", datToday, objOldTrx.strDescription, objOldTrx.strMemo, Trx.TrxStatus.Unreconciled, New TrxGenImportData())
                End If
                'Remember the old Trx to delete later if the new Trx is saved.
                'Remember the Trx object instead of its index because the index may change
                'as the result of saving the new Trx or deleting other old ones.
                colOldTrx.Add(objOldTrx)
                'Clone all the splits in old trx and add them to new trx.
                For Each objOldSplit In objOldTrx.colSplits
                    With objOldSplit
                        objNewTrx.AddSplit(.strMemo, .strCategoryKey, .strPONumber, .strInvoiceNum, .datInvoiceDate, .datDueDate, .strTerms, .strBudgetKey, .curAmount)
                    End With
                Next objOldSplit
            Next
            If colOldTrx.Count() = 0 Then
                MsgBox("No transactions selected.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            'Now let them edit it and possibly save it.
            Using frmTrx As TrxForm = frmCreateTrxForm()
                If frmTrx.blnAddNormal(mobjReg, objNewTrx, datResult, False, "SearchForm.CombineNew") Then
                    'They did not save it.
                    Exit Sub
                End If
            End Using
            objNewTrx = mobjReg.objNormalTrx(mobjReg.lngCurrentTrxIndex())

            'Now delete old trx.
            'Because we start from the Trx object instead of its index, we don't need
            'to worry if saving the new trx or a prior delete changed the index of a Trx.
            objStartLogger = mobjReg.objLogGroupStart("SearchForm.CombineDelete")
            For Each objOldTrx In colOldTrx
                objOldTrx.Delete(New LogDelete, "SearchForm.CombineDeleteTrx")
            Next
            mobjReg.LogGroupEnd(objStartLogger)

            mobjReg.SetCurrent(objNewTrx.lngIndex)
            mobjReg.RaiseShowCurrent()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdMove_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMove.Click
        Try

            Dim objTrxSrc As NormalTrx
            Dim objTrxFirst As NormalTrx = Nothing
            Dim colTrx As ICollection(Of NormalTrx)
            Dim strNewDate As String = ""
            Dim objNewReg As Register = Nothing
            Dim datExplicitDate As Date
            Dim blnUseDayOffset As Boolean
            Dim intDayOffset As Integer
            Dim datNewDate As Date
            Dim frmMoveTo As MoveDstForm

            colTrx = New List(Of NormalTrx)
            For Each objTrxSrc In colGetCheckedTrx()
                If Not blnValidTrxForBulkOperation(objTrxSrc, "moved") Then
                    Exit Sub
                End If
                colTrx.Add(objTrxSrc)
            Next
            If colTrx.Count() = 0 Then
                MsgBox("No transactions selected.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            frmMoveTo = New MoveDstForm
            If Not frmMoveTo.blnShowModal(mobjAccount.colRegisters, mobjReg, strNewDate, objNewReg) Then
                Exit Sub
            End If
            If Utilities.blnIsValidDate(strNewDate) Then
                datExplicitDate = CDate(strNewDate)
                blnUseDayOffset = False
            ElseIf IsNumeric(strNewDate) Or strNewDate = "" Then
                intDayOffset = CInt(Val(strNewDate))
                blnUseDayOffset = True
            Else
                'Should never get here.
                MsgBox("Invalid date or number of days.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            mblnIgnoreTrxUpdates = True

            Dim objStartLogger As ILogGroupStart
            objStartLogger = mobjReg.objLogGroupStart("SearchForm.Move")
            For Each objTrxSrc In colTrx
                If blnUseDayOffset Then
                    datNewDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDayOffset, objTrxSrc.datDate)
                Else
                    datNewDate = datExplicitDate
                End If
                With objTrxSrc
                    If objNewReg Is Nothing Then
                        'Changing date, not register.
                        Dim objTrxManager As NormalTrxManager = objTrxSrc.objGetTrxManager()
                        objTrxManager.UpdateStart()
                        objTrxManager.objTrx.datDate = datNewDate
                        objTrxManager.UpdateEnd(New LogMove, "SearchForm.MoveUpdate")
                        If objTrxFirst Is Nothing Then
                            objTrxFirst = objTrxSrc
                        End If
                    Else
                        'Changing register, and possibly date.
                        Dim objTrxNew As NormalTrx = New NormalTrx(objNewReg)
                        objTrxNew.NewStartNormal(True, objTrxSrc)
                        objTrxNew.datDate = datNewDate
                        .CopySplits(objTrxNew)
                        objNewReg.NewAddEnd(objTrxNew, New LogAdd, "SearchForm.MoveAdd")
                        If objTrxFirst Is Nothing Then
                            objTrxFirst = objTrxNew
                        End If
                        objTrxSrc.Delete(New LogDelete, "SearchForm.MoveDelete")
                    End If
                End With
            Next objTrxSrc
            mobjReg.LogGroupEnd(objStartLogger)

            mblnIgnoreTrxUpdates = False

            If Not objTrxFirst Is Nothing Then
                If objNewReg Is Nothing Then
                    mobjReg.SetCurrent(objTrxFirst.lngIndex)
                    mobjReg.RaiseShowCurrent()
                Else
                    objNewReg.SetCurrent(objTrxFirst.lngIndex)
                    objNewReg.RaiseShowCurrent()
                End If
            End If

            RedoSearch()

            Exit Sub
        Catch ex As Exception
            mblnIgnoreTrxUpdates = False
            gTopException(ex)
        End Try
    End Sub

    Private Function blnValidTrxForBulkOperation(ByVal objTrx As Trx, ByVal strOperation As String) As Boolean

        blnValidTrxForBulkOperation = False
        If objTrx.lngType <> Trx.TrxType.Normal Then
            MsgBox("Budgets and transfers may not be " & strOperation & ".", MsgBoxStyle.Critical)
            Exit Function
        End If
        If Not objTrx.blnFake Then
            MsgBox("Only fake transactions may be " & strOperation & ".", MsgBoxStyle.Critical)
            Exit Function
        End If
        If objTrx.blnAutoGenerated Then
            MsgBox("Generated transactions may not be " & strOperation & ".", MsgBoxStyle.Critical)
            Exit Function
        End If
        If objTrx.strRepeatKey <> "" Then
            MsgBox("Transactions in a repeat sequence may not be " & strOperation & ".", MsgBoxStyle.Critical)
            Exit Function
        End If
        blnValidTrxForBulkOperation = True

    End Function

    Private Sub mobjReg_RedisplayTrx() Handles mobjReg.RedisplayTrx
        RedoSearch()
    End Sub

    Private Sub mobjReg_TrxAdded(ByVal lngIndex As Integer, ByVal objTrx As Trx) Handles mobjReg.TrxAdded
        RedoSearch()
    End Sub

    Private Sub mobjReg_TrxDeleted(ByVal lngIndex As Integer) Handles mobjReg.TrxDeleted
        RedoSearch()
    End Sub

    Private Sub mobjReg_TrxUpdated(ByVal lngOldIndex As Integer, ByVal lngNewIndex As Integer, ByVal objTrx As Trx) Handles mobjReg.TrxUpdated
        If Not mblnIgnoreTrxUpdates Then
            RedoSearch()
        End If
    End Sub

    Private Sub RedoSearch()
        ClearResults()
        SearchInternal()
        RestoreCheckedAndSelected()
    End Sub

    Private Sub RememberCheckedTrx()
        Dim objCheckedTrx As Trx
        If Not mblnSkipRemember Then
            'Remember the results that are checked.
            colCheckedTrx = New List(Of Trx)
            For Each objCheckedTrx In colGetCheckedTrx()
                colCheckedTrx.Add(objCheckedTrx)
            Next
        End If
    End Sub

    Private Sub RememberSelectedTrx()
        If Not mblnSkipRemember Then
            objSelectedTrx = Nothing
            If Not lvwMatches.FocusedItem Is Nothing Then
                objSelectedTrx = DirectCast(lvwMatches.FocusedItem.Tag, SearchMatch).objTrx
            End If
        End If
    End Sub

    Private Sub RestoreCheckedAndSelected()

        Dim objItem As System.Windows.Forms.ListViewItem
        Dim objTrx As Trx
        Dim objCheckedTrx As Trx

        Try
            mblnSkipRemember = True
            For Each objItem In lvwMatches.Items

                objTrx = DirectCast(objItem.Tag, SearchMatch).objTrx
                For Each objCheckedTrx In colCheckedTrx
                    If objCheckedTrx Is objTrx Then
                        objItem.Checked = True
                        Exit For
                    End If
                Next objCheckedTrx
                If objTrx Is objSelectedTrx Then
                    objItem.Focused = True
                    objItem.Selected = True
                    objItem.EnsureVisible()
                End If
            Next objItem
        Finally
            mblnSkipRemember = False
        End Try
    End Sub
End Class