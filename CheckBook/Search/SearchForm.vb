Option Strict On
Option Explicit On

Imports CheckBookLib

Friend Class SearchForm
    Inherits System.Windows.Forms.Form
    Implements IHostSearchUI
    Implements IHostSearchToolUI

    Private mobjHostUI As IHostUI
    Private WithEvents mobjReg As Register
    Private mobjAccount As Account
    Private mobjCompany As Company
    Private mdatDefaultDate As Date
    Private mobjSelectedSearchHandler As ISearchHandler

    Private Class SearchMatch
        Public objTrx As Trx
    End Class

    Private mintMatchCount As Integer
    Private mcurAmountMatched As Decimal
    Private mcurAmountTotal As Decimal
    Private mblnSkipRemember As Boolean
    Private colCheckedTrx As ICollection(Of Trx)
    Private objSelectedTrx As Trx

    'Parameters of most recent successful search
    Private mdatLastStart As Date
    Private mdatLastEnd As Date
    Private mblnLastIncludeGenerated As Boolean
    Private mobjLastSearchHandler As ISearchHandler

    Public Sub ShowMe(ByVal objHostUI_ As IHostUI, ByVal objReg_ As Register)

        mobjHostUI = objHostUI_
        mobjReg = objReg_
        mobjAccount = mobjReg.objAccount
        mobjCompany = mobjHostUI.objCompany
        colCheckedTrx = New List(Of Trx)
        mcurAmountMatched = 0
        mcurAmountTotal = 0
        mdatDefaultDate = Today
        Me.Text = "Search " & mobjReg.strTitle
        txtStartDate.Text = Utilities.strFormatDate(DateAdd(Microsoft.VisualBasic.DateInterval.Month, -2, Today))
        txtEndDate.Text = Utilities.strFormatDate(DateAdd(Microsoft.VisualBasic.DateInterval.Month, 6, Today))
        LoadSearchIn()
        LoadSearchType()
        cboSearchIn.SelectedIndex = 0
        cboSearchType.SelectedIndex = 0
        txtSearchFor.Focus()
        LoadTools()

        Me.Show()

    End Sub

    Private Sub SearchForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Me.Width = 1011
        Me.Height = 547
    End Sub

    Private Sub SearchForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        mobjReg = Nothing
    End Sub

    Private Sub LoadSearchIn()
        cboSearchIn.Items.Clear()
        For Each objHandler As ISearchHandler In mobjHostUI.objSearchHandlers
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

    Private Sub LoadTools()
        cboTools.Items.Clear()
        For Each objTool As ISearchTool In mobjHostUI.objSearchTools
            cboTools.Items.Add(objTool)
        Next
    End Sub

    Private Sub cboSearchIn_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSearchIn.SelectedIndexChanged
        mobjSelectedSearchHandler = DirectCast(cboSearchIn.SelectedItem, ISearchHandler)
        If Not mobjSelectedSearchHandler Is Nothing Then
            mobjSelectedSearchHandler.HandlerSelected(Me)
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        Try
            If mobjSelectedSearchHandler Is Nothing Then
                Exit Sub
            End If

            If Not Utilities.blnIsValidDate(txtStartDate.Text) Then
                mobjHostUI.ErrorMessageBox("Invalid starting date.")
                Exit Sub
            End If

            If Not Utilities.blnIsValidDate(txtEndDate.Text) Then
                mobjHostUI.ErrorMessageBox("Invalid ending date.")
                Exit Sub
            End If

            If Not mobjSelectedSearchHandler.blnPrepareSearch(Me) Then
                Exit Sub
            End If

            ClearResults()

            mobjLastSearchHandler = mobjSelectedSearchHandler
            mblnLastIncludeGenerated = (chkIncludeGenerated.CheckState = System.Windows.Forms.CheckState.Checked)
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
            gTopException(ex)
        End Try
    End Sub

    Private Sub SearchInternal()
        Dim dlgTrx As AddSearchMatchTrxDelegate

        Try
            mblnSkipRemember = True
            If Not mobjLastSearchHandler Is Nothing Then
                For Each objTrx As Trx In mobjReg.colDateRange(mdatLastStart, mdatLastEnd)
                    If mblnLastIncludeGenerated Or Not objTrx.blnAutoGenerated Then
                        If chkShowAllSplits.Checked And objTrx.lngType = Trx.TrxType.Normal Then
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
            UITools.AddListSubItem(objItem, 4, Utilities.strFormatCurrency(curAvailable))
            UITools.AddListSubItem(objItem, 5, strCategory)
            UITools.AddListSubItem(objItem, 6, strPONumber)
            UITools.AddListSubItem(objItem, 7, strInvoiceNum)
            UITools.AddListSubItem(objItem, 8, strInvoiceDate)
            UITools.AddListSubItem(objItem, 9, strDueDate)
            UITools.AddListSubItem(objItem, 10, strTerms)
            UITools.AddListSubItem(objItem, 11, objTrx.strFakeStatus)
        Else
            UITools.AddListSubItem(objItem, 4, "")
            UITools.AddListSubItem(objItem, 5, "")
            UITools.AddListSubItem(objItem, 6, "")
            UITools.AddListSubItem(objItem, 7, "")
            UITools.AddListSubItem(objItem, 8, "")
            UITools.AddListSubItem(objItem, 9, "")
            UITools.AddListSubItem(objItem, 10, "")
            UITools.AddListSubItem(objItem, 11, "")
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
        UITools.AddListSubItem(objItem, 4, Utilities.strFormatCurrency(curAvailable))
        UITools.AddListSubItem(objItem, 5, mobjCompany.objCategories.strTranslateKey(objSplit.strCategoryKey))
        UITools.AddListSubItem(objItem, 6, objSplit.strPONumber)
        UITools.AddListSubItem(objItem, 7, objSplit.strInvoiceNum)
        UITools.AddListSubItem(objItem, 8, strInvoiceDate)
        UITools.AddListSubItem(objItem, 9, strDueDate)
        UITools.AddListSubItem(objItem, 10, objSplit.strTerms)
        UITools.AddListSubItem(objItem, 11, objTrx.strFakeStatus)
        mcurAmountMatched = mcurAmountMatched + objSplit.curAmount

    End Sub

    Private Function objAddNewMatch(ByVal objTrx As Trx, ByVal curMatchAmount As Decimal) As ListViewItem
        Dim objItem As ListViewItem = UITools.ListViewAdd(lvwMatches)
        objItem.Text = Utilities.strFormatDate(objTrx.datDate)
        UITools.AddListSubItem(objItem, 1, objTrx.strNumber)
        UITools.AddListSubItem(objItem, 2, objTrx.strDescription)
        UITools.AddListSubItem(objItem, 3, Utilities.strFormatCurrency(curMatchAmount))
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
                mobjHostUI.ErrorMessageBox("You may not edit a replica transaction directly. Instead edit the split it was created from in another transaction.")
                Exit Sub
            End If
            If mobjHostUI.blnUpdateTrx(objTrx, mdatDefaultDate, "SearchForm.Edit") Then
                Exit Sub
            End If
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
            If mobjHostUI.blnAddNormalTrx(objTrx, mdatDefaultDate, True, "SearchForm.NewNormal") Then
                mobjHostUI.InfoMessageBox("Canceled.")
            End If
            mobjReg.ValidateRegister()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

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
            gTopException(ex)
        End Try

    End Sub

    Private Function blnValidTrxForBulkOperation(ByVal objTrx As Trx, ByVal strOperation As String) As Boolean _
        Implements IHostSearchToolUI.blnValidTrxForBulkOperation

        blnValidTrxForBulkOperation = False
        If objTrx.lngType <> Trx.TrxType.Normal Then
            mobjHostUI.ErrorMessageBox("Budgets and transfers may not be " & strOperation & ".")
            Exit Function
        End If
        If Not objTrx.blnFake Then
            mobjHostUI.ErrorMessageBox("Only fake transactions may be " & strOperation & ".")
            Exit Function
        End If
        If objTrx.blnAutoGenerated Then
            mobjHostUI.ErrorMessageBox("Generated transactions may not be " & strOperation & ".")
            Exit Function
        End If
        If objTrx.strRepeatKey <> "" Then
            mobjHostUI.ErrorMessageBox("Transactions in a repeat sequence may not be " & strOperation & ".")
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
        RedoSearch()
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

    Public Function strGetTextSearchFor() As String Implements IHostSearchUI.strGetTextSearchFor
        Return txtSearchFor.Text
    End Function

    Public Function objGetComboBoxSearchFor() As Object Implements IHostSearchUI.objGetComboBoxSearchFor
        Return cboSearchCats.SelectedItem
    End Function

    Public Function objGetSearchType() As Object Implements IHostSearchUI.objGetSearchType
        Return cboSearchType.SelectedItem
    End Function

    Public Iterator Function objAllSelectedTrx() As IEnumerable(Of Trx) Implements IHostSearchToolUI.objAllSelectedTrx
        'Make a copy to iterate, in case the caller modifies a trx which causes the search to refresh.
        Dim objSelected As List(Of Trx) = New List(Of Trx)(colGetCheckedTrx())
        For Each objTrx As Trx In objSelected
            Yield objTrx
        Next
    End Function

    Public ReadOnly Property objReg() As Register Implements IHostSearchToolUI.objReg
        Get
            Return mobjReg
        End Get
    End Property
End Class