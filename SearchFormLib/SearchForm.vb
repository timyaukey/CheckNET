Option Strict On
Option Explicit On


Public Class SearchForm
    Inherits System.Windows.Forms.Form
    Implements ISearchForm
    Implements IHostSearchUI
    Implements IHostSearchToolUI

    Private mobjHostUI As IHostUI
    Private WithEvents mobjReg As Register
    Private mobjAccount As Account
    Private mobjCompany As Company
    Private mdatDefaultDate As Date
    Private mobjSelectedSearchHandler As ISearchHandler
    Private mobjSelectedSearchFilter As ISearchFilter

    Private Class SearchMatch
        Public objTrx As BaseTrx
    End Class

    Private mintMatchCount As Integer
    Private mcurAmountMatched As Decimal
    Private mcurAmountTotal As Decimal
    Private mblnSkipRemember As Boolean
    Private colCheckedTrx As ICollection(Of BaseTrx)
    Private objSelectedTrx As BaseTrx

    'Parameters of most recent successful search
    Private mdatLastStart As Date
    Private mdatLastEnd As Date
    Private mobjLastSearchFilter As ISearchFilter
    Private mobjLastSearchHandler As ISearchHandler

    Public Sub ShowMe(ByVal objHostUI_ As IHostUI, ByVal objReg_ As Register) Implements ISearchForm.ShowMe

        mobjHostUI = objHostUI_
        mobjReg = objReg_
        mobjAccount = mobjReg.Account
        mobjCompany = mobjHostUI.Company
        Me.MdiParent = mobjHostUI.GetMainForm()
        colCheckedTrx = New List(Of BaseTrx)
        mcurAmountMatched = 0
        mcurAmountTotal = 0
        mdatDefaultDate = Today
        Me.Text = "Search " & mobjReg.Title
        txtStartDate.Text = Utilities.FormatDate(DateAdd(Microsoft.VisualBasic.DateInterval.Month, -2, Today))
        txtEndDate.Text = Utilities.FormatDate(DateAdd(Microsoft.VisualBasic.DateInterval.Month, 6, Today))
        LoadSearchIn()
        LoadSearchType()
        LoadSearchFilter()
        cboSearchIn.SelectedIndex = 0
        cboSearchType.SelectedIndex = 0
        cboFilterType.SelectedIndex = 0
        txtSearchFor.Focus()
        LoadTools()

        Me.Show()

    End Sub

    Public Sub ShowMeAgain() Implements ISearchForm.ShowMeAgain
        Me.Show()
        Me.Activate()
    End Sub

    Private Sub SearchForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Me.Width = 1011
        Me.Height = 547
    End Sub

    Private Sub SearchForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        mobjReg = Nothing
        RaiseEvent SearchFormClosed(eventSender, eventArgs)
    End Sub

    Public Event SearchFormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Implements ISearchForm.SearchFormClosed

    Private Sub LoadSearchIn()
        cboSearchIn.Items.Clear()
        For Each objHandler As ISearchHandler In mobjHostUI.GetSearchHandlers
            cboSearchIn.Items.Add(objHandler)
        Next
        mobjLastSearchHandler = Nothing
    End Sub

    Private Sub LoadSearchType()
        cboSearchType.Items.Clear()
        cboSearchType.Items.Add(New SearchComparerEqualTo())
        cboSearchType.Items.Add(New SearchComparerStartsWith())
        cboSearchType.Items.Add(New SearchComparerContains())
    End Sub

    Private Sub LoadSearchFilter()
        cboFilterType.Items.Clear()
        For Each objFilter As ISearchFilter In mobjHostUI.GetSearchFilters
            cboFilterType.Items.Add(objFilter)
        Next
    End Sub

    Private Sub LoadTools()
        cboTools.Items.Clear()
        For Each objTool As ISearchTool In mobjHostUI.GetSearchTools
            cboTools.Items.Add(objTool)
        Next
    End Sub

    Private Sub cboSearchIn_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSearchIn.SelectedIndexChanged
        mobjSelectedSearchHandler = DirectCast(cboSearchIn.SelectedItem, ISearchHandler)
        If Not mobjSelectedSearchHandler Is Nothing Then
            mobjSelectedSearchHandler.HandlerSelected(Me)
        End If
    End Sub

    Private Sub cboFilterType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFilterType.SelectedIndexChanged
        mobjSelectedSearchFilter = DirectCast(cboFilterType.SelectedItem, ISearchFilter)
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        Try
            If mobjSelectedSearchHandler Is Nothing Then
                Exit Sub
            End If

            If Not Utilities.IsValidDate(txtStartDate.Text) Then
                mobjHostUI.ErrorMessageBox("Invalid starting date.")
                Exit Sub
            End If

            If Not Utilities.IsValidDate(txtEndDate.Text) Then
                mobjHostUI.ErrorMessageBox("Invalid ending date.")
                Exit Sub
            End If

            If Not mobjSelectedSearchHandler.PrepareSearch(Me) Then
                Exit Sub
            End If

            ClearResults()

            mobjLastSearchHandler = mobjSelectedSearchHandler
            mobjLastSearchFilter = mobjSelectedSearchFilter
            mdatLastStart = CDate(txtStartDate.Text)
            mdatLastEnd = CDate(txtEndDate.Text)

            SearchInternal()
            RememberSelectedTrx()
            RememberCheckedTrx()

            mcurAmountTotal = mcurAmountTotal + mcurAmountMatched
            ShowTotals()
            mobjHostUI.InfoMessageBox(mintMatchCount & " matches found.")

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub SearchInternal()
        Dim dlgTrx As AddSearchMatchTrxDelegate

        Try
            mblnSkipRemember = True
            If Not mobjLastSearchHandler Is Nothing Then
                For Each objTrx As BaseTrx In mobjReg.GetDateRange(Of BaseTrx)(mdatLastStart, mdatLastEnd)
                    If mobjLastSearchFilter.IsIncluded(objTrx) Then
                        If chkShowAllSplits.Checked And objTrx.GetType() Is GetType(BankTrx) Then
                            dlgTrx = AddressOf AddSearchMatchAllSplits
                        Else
                            dlgTrx = AddressOf AddSearchMatchTrx
                        End If
                        mobjLastSearchHandler.ProcessTrx(objTrx, dlgTrx, AddressOf AddSearchMatchSplit)
                    End If
                Next
            End If
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
            mobjHostUI.ErrorMessageBox("Add/Mult amount is required.")
            Exit Function
        End If
        If Not IsNumeric(txtAddMultAmount.Text) Then
            mobjHostUI.ErrorMessageBox("Invalid Add/Mult amount.")
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
        My.Computer.Clipboard.SetText(Utilities.FormatCurrency(mcurAmountTotal))
    End Sub

    Private Sub ShowTotals()
        lblTotalDollars.Text = "Matched $" & Utilities.FormatCurrency(mcurAmountMatched) & "    Total $" & Utilities.FormatCurrency(mcurAmountTotal)
    End Sub

    Private Sub AddSearchMatchTrx(ByVal objTrx As BaseTrx)

        Dim objItem As ListViewItem
        Dim strCategory As String = ""
        Dim strPONumber As String = ""
        Dim strInvoiceNum As String = ""
        Dim strInvoiceDate As String = ""
        Dim strDueDate As String = ""
        Dim strTerms As String = ""
        Dim strBudget As String = ""
        Dim curAvailable As Decimal

        objItem = objAddNewMatch(objTrx, objTrx.Amount)
        If objTrx.GetType() Is GetType(BankTrx) Then
            DirectCast(objTrx, BankTrx).SummarizeSplits(mobjCompany, strCategory, strPONumber, strInvoiceNum, strInvoiceDate, strDueDate, strTerms, strBudget, curAvailable)
            UITools.AddListSubItem(objItem, 4, Utilities.FormatCurrency(curAvailable))
            UITools.AddListSubItem(objItem, 5, strCategory)
            UITools.AddListSubItem(objItem, 6, strPONumber)
            UITools.AddListSubItem(objItem, 7, strInvoiceNum)
            UITools.AddListSubItem(objItem, 8, strInvoiceDate)
            UITools.AddListSubItem(objItem, 9, strDueDate)
            UITools.AddListSubItem(objItem, 10, strTerms)
        Else
            UITools.AddListSubItem(objItem, 4, "")
            UITools.AddListSubItem(objItem, 5, "")
            UITools.AddListSubItem(objItem, 6, objTrx.PONumber)
            UITools.AddListSubItem(objItem, 7, objTrx.InvoiceNum)
            UITools.AddListSubItem(objItem, 8, "")
            UITools.AddListSubItem(objItem, 9, "")
            UITools.AddListSubItem(objItem, 10, "")
        End If
        UITools.AddListSubItem(objItem, 11, objTrx.FakeStatusLabel)
        mcurAmountMatched = mcurAmountMatched + objTrx.Amount

    End Sub

    Private Sub AddSearchMatchAllSplits(ByVal objTrx As BaseTrx)

        Dim objSplit As TrxSplit
        For Each objSplit In DirectCast(objTrx, BankTrx).Splits
            AddSearchMatchSplit(objTrx, objSplit)
        Next

    End Sub

    Private Sub AddSearchMatchSplit(ByVal objTrx As BaseTrx, ByVal objSplit As TrxSplit)

        Dim objItem As ListViewItem
        Dim strInvoiceDate As String
        Dim strDueDate As String
        Dim curAvailable As Decimal

        objItem = objAddNewMatch(objTrx, objSplit.Amount)
        If objSplit.BudgetKey = mobjCompany.PlaceholderBudgetKey Then
            curAvailable = objSplit.Amount
        Else
            curAvailable = 0
        End If
        If objSplit.InvoiceDate = Utilities.EmptyDate Then
            strInvoiceDate = ""
        Else
            strInvoiceDate = Utilities.FormatDate(objSplit.InvoiceDate)
        End If
        If objSplit.DueDate = Utilities.EmptyDate Then
            strDueDate = ""
        Else
            strDueDate = Utilities.FormatDate(objSplit.DueDate)
        End If
        UITools.AddListSubItem(objItem, 4, Utilities.FormatCurrency(curAvailable))
        UITools.AddListSubItem(objItem, 5, mobjCompany.Categories.TranslateKey(objSplit.CategoryKey))
        UITools.AddListSubItem(objItem, 6, objSplit.PONumber)
        UITools.AddListSubItem(objItem, 7, objSplit.InvoiceNum)
        UITools.AddListSubItem(objItem, 8, strInvoiceDate)
        UITools.AddListSubItem(objItem, 9, strDueDate)
        UITools.AddListSubItem(objItem, 10, objSplit.Terms)
        UITools.AddListSubItem(objItem, 11, objTrx.FakeStatusLabel)
        mcurAmountMatched = mcurAmountMatched + objSplit.Amount

    End Sub

    Private Function objAddNewMatch(ByVal objTrx As BaseTrx, ByVal curMatchAmount As Decimal) As ListViewItem
        Dim objItem As ListViewItem = UITools.ListViewAdd(lvwMatches)
        objItem.Text = Utilities.FormatDate(objTrx.TrxDate)
        UITools.AddListSubItem(objItem, 1, objTrx.Number)
        UITools.AddListSubItem(objItem, 2, objTrx.Description)
        UITools.AddListSubItem(objItem, 3, Utilities.FormatCurrency(curMatchAmount))
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
            mobjReg.SetCurrent(objMatch.objTrx)
            mobjReg.FireShowCurrent()
            RememberSelectedTrx()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
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
            TopException(ex)
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
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdEditTrx_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEditTrx.Click
        Try
            Dim objTrx As BaseTrx
            Dim objMatch As SearchMatch
            If lvwMatches.FocusedItem Is Nothing Then
                Exit Sub
            End If
            objMatch = DirectCast(lvwMatches.FocusedItem.Tag, SearchMatch)
            objTrx = objMatch.objTrx
            If objTrx.GetType() Is GetType(ReplicaTrx) Then
                mobjHostUI.ErrorMessageBox("You may not edit a replica transaction directly. Instead edit the split it was created from in another transaction.")
                Exit Sub
            End If
            If mobjHostUI.UpdateTrx(objTrx, mdatDefaultDate, "SearchForm.Edit") Then
                Exit Sub
            End If
            mobjReg.ValidateRegister()

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdNewNormal_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNewNormal.Click
        Try
            Dim objTrx As BankTrx = New BankTrx(mobjReg)
            objTrx.NewEmptyNormal(mdatDefaultDate)
            If mobjHostUI.AddNormalTrx(objTrx, mdatDefaultDate, True, "SearchForm.NewNormal") Then
                mobjHostUI.InfoMessageBox("Canceled.")
            End If
            mobjReg.ValidateRegister()
            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Enumerate all BaseTrx objects for selected list items in the search results.
    ''' If multiple splits for the same BaseTrx are included in the search results
    ''' will only return that BaseTrx once. Assumes all splits for the same BaseTrx
    ''' are adjacent in the search results.
    ''' </summary>
    ''' <returns></returns>
    Private Iterator Function colGetCheckedTrx() As IEnumerable(Of BaseTrx)
        Dim objItem As ListViewItem
        Dim objTrx As BaseTrx
        Dim objLastTrx As BaseTrx = Nothing
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

    Private Sub btnRunTool_Click(sender As Object, e As EventArgs) Handles btnRunTool.Click
        Try
            Dim objTool As ISearchTool = DirectCast(cboTools.SelectedItem, ISearchTool)
            If objTool Is Nothing Then
                mobjHostUI.ErrorMessageBox("Select a tool from the drop down list first")
                Return
            End If
            objTool.Run(Me)
            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try

    End Sub

    Private Function IsValidTrxForBulkOperation(ByVal objTrx As BaseTrx, ByVal strOperation As String) As Boolean _
        Implements IHostSearchToolUI.IsValidTrxForBulkOperation

        IsValidTrxForBulkOperation = False
        If objTrx.GetType() IsNot GetType(BankTrx) Then
            mobjHostUI.ErrorMessageBox("Budgets and transfers may not be " & strOperation & ".")
            Exit Function
        End If
        If Not objTrx.IsFake Then
            mobjHostUI.ErrorMessageBox("Only fake transactions may be " & strOperation & ".")
            Exit Function
        End If
        If objTrx.IsAutoGenerated Then
            mobjHostUI.ErrorMessageBox("Generated transactions may not be " & strOperation & ".")
            Exit Function
        End If
        If objTrx.RepeatKey <> "" Then
            mobjHostUI.ErrorMessageBox("Transactions in a repeat sequence may not be " & strOperation & ".")
            Exit Function
        End If
        IsValidTrxForBulkOperation = True

    End Function

    Private Sub mobjReg_EndRegenerating() Handles mobjReg.EndRegenerating
        RedoSearch()
    End Sub

    Private Sub mobjReg_ManyTrxChanged() Handles mobjReg.ManyTrxChanged
        RedoSearch()
    End Sub

    Private Sub mobjReg_TrxAdded(ByVal objTrx As BaseTrx) Handles mobjReg.TrxAdded
        RedoSearch()
    End Sub

    Private Sub mobjReg_TrxDeleted(ByVal objTrx As BaseTrx) Handles mobjReg.TrxDeleted
        RedoSearch()
    End Sub

    Private Sub mobjReg_TrxUpdated(ByVal blnPositionChanged As Boolean, ByVal objTrx As BaseTrx) Handles mobjReg.TrxUpdated
        RedoSearch()
    End Sub

    Private Sub RedoSearch()
        ClearResults()
        SearchInternal()
        RestoreCheckedAndSelected()
    End Sub

    Private Sub RememberCheckedTrx()
        Dim objCheckedTrx As BaseTrx
        If Not mblnSkipRemember Then
            'Remember the results that are checked.
            colCheckedTrx = New List(Of BaseTrx)
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
        Dim objTrx As BaseTrx
        Dim objCheckedTrx As BaseTrx

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

    Public Sub UseTextCriteria() Implements IHostSearchUI.UseTextCriteria
        txtSearchFor.Visible = True
        cboSearchCats.Visible = False
    End Sub

    Public Sub UseComboBoxCriteria(ByVal objChoices As IEnumerable(Of Object)) Implements IHostSearchUI.UseComboBoxCriteria
        txtSearchFor.Visible = False
        cboSearchCats.Visible = True
        cboSearchCats.Items.Clear()
        Dim objChoice As Object
        For Each objChoice In objChoices
            cboSearchCats.Items.Add(objChoice)
        Next
        With cboSearchCats
            .Left = txtSearchFor.Left
            .Top = txtSearchFor.Top
        End With
    End Sub

    Public Function GetTextSearchFor() As String Implements IHostSearchUI.GetTextSearchFor
        Return txtSearchFor.Text
    End Function

    Public Function GetComboBoxSearchFor() As Object Implements IHostSearchUI.GetComboBoxSearchFor
        Return cboSearchCats.SelectedItem
    End Function

    Public Function GetSearchType() As Object Implements IHostSearchUI.GetSearchType
        Return cboSearchType.SelectedItem
    End Function

    Public Iterator Function GetAllSelectedTrx() As IEnumerable(Of BaseTrx) Implements IHostSearchToolUI.GetAllSelectedTrx
        'Make a copy to iterate, in case the caller modifies a trx which causes the search to refresh.
        Dim objSelected As List(Of BaseTrx) = New List(Of BaseTrx)(colGetCheckedTrx())
        For Each objTrx As BaseTrx In objSelected
            Yield objTrx
        Next
    End Function

    Public ReadOnly Property Reg() As Register Implements IHostSearchToolUI.Reg
        Get
            Return mobjReg
        End Get
    End Property
End Class