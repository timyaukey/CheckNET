Option Strict On
Option Explicit On


Public Class RegisterForm
    Inherits System.Windows.Forms.Form
    Implements IRegisterForm

    Private mobjHostUI As IHostUI
    Private mobjCompany As Company
    Private mobjAccount As Account
    Private WithEvents mobjReg As Register
    Private mblnLoadComplete As Boolean
    Private mdatDefaultNewDate As Date
    Private WithEvents mfrmSearch As ISearchForm
    Private mblnOldVisible As Boolean
    Private mblnShowValidationErrors As Boolean

    'Column numbers.
    Private mintColDate As Integer
    Private mintColNumber As Integer
    Private mintColDescr As Integer
    Private mintColAmount As Integer
    Private mintColBalance As Integer
    Private mintColCategory As Integer
    Private mintColPONumber As Integer
    Private mintColInvoiceNum As Integer
    Private mintColInvoiceDate As Integer
    Private mintColDueDate As Integer
    Private mintColTerms As Integer
    Private mintColStatus As Integer
    Private mintColFake As Integer
    Private mintColAutoGenerated As Integer
    Private mintColImportKey As Integer

    Private mstrColValueFuncs(50) As strColumnValue

    Private mlngCOLOR_BUDGET As System.Drawing.Color = System.Drawing.Color.FromArgb(0, 0, &HA0)
    Private mlngCOLOR_CREDIT As System.Drawing.Color = System.Drawing.Color.FromArgb(0, &HA0, 0)
    Private mlngCOLOR_FAKE As System.Drawing.Color = System.Drawing.Color.FromArgb(&HFA, &HF0, &HF8) '&HF0, &HE0, &HF0
    Private mlngCOLOR_REAL As System.Drawing.Color = System.Drawing.Color.FromArgb(255, 255, 250) '&H80000005
    Private mlngCOLOR_REPLICA As System.Drawing.Color = System.Drawing.Color.FromArgb(&H80, &H40, &H0)

    Public Sub ShowMe(ByVal objHostUI_ As IHostUI, ByVal objReg_ As Register) Implements IRegisterForm.ShowMe

        mobjHostUI = objHostUI_
        mobjAccount = objReg_.objAccount
        mobjReg = objReg_
        mobjCompany = mobjHostUI.objCompany
        mdatDefaultNewDate = Today
        Me.MdiParent = mobjHostUI.objGetMainForm()
        Me.Show()

    End Sub

    Public Sub ShowMeAgain() Implements IRegisterForm.ShowMeAgain
        Me.Show()
        Me.Activate()
    End Sub

    Public ReadOnly Property objReg() As Register Implements IRegisterForm.objReg
        Get
            objReg = mobjReg
        End Get
    End Property

    Public ReadOnly Property objAccount() As Account
        Get
            objAccount = mobjAccount
        End Get
    End Property

    Private ReadOnly Property objCurrentTrx() As Trx
        Get
            If grdReg.CurrentRow Is Nothing Then
                Return Nothing
            End If
            Return mobjReg.objTrx(grdReg.CurrentRow.Index + 1)
        End Get
    End Property

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click
        Dim objTrx As Trx
        Dim objXfer As TransferManager
        Dim objOtherReg As Register

        Try
            objTrx = objCurrentTrx()
            If objTrx Is Nothing Then
                mobjHostUI.ErrorMessageBox("You must first select a transaction.")
                Return
            End If
            With objTrx
                If .GetType() Is GetType(ReplicaTrx) Then
                    mobjHostUI.ErrorMessageBox("You may not delete a replica transaction. Instead delete the split it was created from in another transaction.")
                    Exit Sub
                End If
                If .blnAutoGenerated Then
                    mobjHostUI.ErrorMessageBox("You may not delete a generated transaction.")
                    Exit Sub
                End If
                If .strRepeatKey <> "" Then
                    MsgBox("You can delete this transaction, but it has a repeat key " & "so the software will probably just recreate it." & vbCrLf &
                           "If you don't want to use this transaction it is much better " & "to change the amount to zero than to delete it.", MsgBoxStyle.Critical)
                End If
                If mobjHostUI.OkCancelMessageBox("Do you really want to delete the transaction dated " & Utilities.strFormatDate(.datDate) & " for $" & Utilities.strFormatCurrency(.curAmount) & " made out to " & .strDescription & "?") <> DialogResult.OK Then
                    Exit Sub
                End If
                If .lngStatus = Trx.TrxStatus.Reconciled Then
                    If mobjHostUI.OkCancelMessageBox("This transaction has been reconciled to a bank statement. " & "Are you sure you want to delete it?") <> DialogResult.OK Then
                        Exit Sub
                    End If
                End If
            End With
            'This will fire events which cause this form to redisplay all affected
            'parts of the grid, with one exception: it will not update the "unmatched"
            'column of any affected normal Trx when deleting a budget Trx. This seems like
            'an unimportant case, and is difficult to fix because the process of applying
            'splits does not let you find the register index of the normal Trx to which
            'a budget is applied.
            If objTrx.GetType() Is GetType(TransferTrx) Then
                objXfer = New TransferManager
                objOtherReg = mobjAccount.objFindReg(DirectCast(objTrx, TransferTrx).strTransferKey)
                objXfer.DeleteTransfer(mobjReg, objTrx, objOtherReg)
            Else
                objTrx.Delete(New LogDelete, "RegisterForm.Delete")
            End If
            DiagnosticValidate()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEdit.Click
        Try
            EditTrx()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub EditTrx()
        Dim objTrx As Trx = objCurrentTrx()
        If objTrx Is Nothing Then
            mobjHostUI.ErrorMessageBox("You must first select a register row.")
            Exit Sub
        End If
        If TypeOf objTrx Is ReplicaTrx Then
            mobjHostUI.ErrorMessageBox("You may not edit a replica transaction directly. Instead edit the split it was created from in another transaction.")
            Exit Sub
        End If
        If mobjHostUI.blnUpdateTrx(objTrx, mdatDefaultNewDate, "RegForm.Edit") Then
            Exit Sub
        End If
        DiagnosticValidate()
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        Try
            If mfrmSearch Is Nothing Then
                mfrmSearch = mobjHostUI.objMakeSearchForm()
                mfrmSearch.ShowMe(mobjHostUI, mobjReg)
            Else
                mfrmSearch.ShowMeAgain()
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub RegisterForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        'If we don't clear these here, the form continues to receive events
        'through mobjReg. Something about our processing for these events
        'makes the form reappear! Unloading destroys the form, but not the
        'underlying object.
        'UPGRADE_NOTE: Object mobjReg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        mobjReg = Nothing
        'UPGRADE_NOTE: Object mobjAccount may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        mobjAccount = Nothing
    End Sub

    Private Sub grdReg_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdReg.DoubleClick
        Try
            EditTrx()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    'Populate grid columns with data for the rows that are currently visible,
    'or may become visible before RefreshPage() is called again.

    Private Sub RefreshPage()
        grdReg.Invalidate()
    End Sub

    Private Sub cmdNewNormal_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNewNormal.Click
        Try
            Dim objTrx As NormalTrx = New NormalTrx(mobjReg)
            objTrx.NewEmptyNormal(mdatDefaultNewDate)
            If mobjHostUI.blnAddNormalTrx(objTrx, mdatDefaultNewDate, True, "RegForm.NewNormal") Then
                mobjHostUI.ErrorMessageBox("Canceled.")
            End If
            DiagnosticValidate()
            Exit Sub
        Catch ex As Exception
            gTopException(ex) : End Try
    End Sub

    Private Sub cmdNewBudget_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNewBudget.Click
        Try
            If mobjHostUI.blnAddBudgetTrx(mobjReg, mdatDefaultNewDate, "RegForm.NewBudget") Then
                mobjHostUI.ErrorMessageBox("Canceled.")
            End If
            DiagnosticValidate()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub cmdNewXfer_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdNewXfer.Click
        Try
            If mobjHostUI.blnAddTransferTrx(mobjReg, mdatDefaultNewDate, "RegForm.NewXfer") Then
                mobjHostUI.ErrorMessageBox("Canceled.")
            End If
            DiagnosticValidate()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub RegisterForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Try

            mblnLoadComplete = True
            Me.Text = mobjReg.strTitle
            ConfigGrid()
            LoadGrid()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Delegate Function strColumnValue(ByVal objTrx As Trx) As String

    Private Function strColumnEmpty(ByVal objTrx As Trx) As String
        strColumnEmpty = ""
    End Function

    Private Sub ConfigGrid()
        Dim intCol As Integer

        ConfigGridCol(intCol, mintColDate, "Date", 700,
            Function(objTrx As Trx) objTrx.datDate.ToString(Utilities.strDateWithTwoDigitYear))
        ConfigGridCol(intCol, mintColNumber, "Number", 700,
            Function(objTrx As Trx) objTrx.strNumber)
        ConfigGridCol(intCol, mintColDescr, "Description", 3000,
            Function(objTrx As Trx) objTrx.strDescription)
        ConfigGridCol(intCol, mintColAmount, "Amount", 900,
            Function(objTrx As Trx) Utilities.strFormatCurrency(objTrx.curAmount), True)
        ConfigGridCol(intCol, mintColBalance, "Balance", 900,
            Function(objTrx As Trx) Utilities.strFormatCurrency(objTrx.curBalance), True)
        ConfigGridCol(intCol, mintColCategory, "Category", 1800,
            Function(objTrx As Trx) objTrx.strCategory)
        ConfigGridCol(intCol, mintColPONumber, "PO#", 900,
            Function(objTrx As Trx) objTrx.strPONumber)
        ConfigGridCol(intCol, mintColInvoiceNum, "Invoice#", 900,
            Function(objTrx As Trx) objTrx.strInvoiceNum)
        ConfigGridCol(intCol, mintColInvoiceDate, "Inv. Date", 700,
            Function(objTrx As Trx) If(objTrx.GetType() Is GetType(NormalTrx), DirectCast(objTrx, NormalTrx).strSummarizeInvoiceDate(), ""))
        ConfigGridCol(intCol, mintColDueDate, "Due Date", 700,
            Function(objTrx As Trx) If(objTrx.GetType() Is GetType(NormalTrx), DirectCast(objTrx, NormalTrx).strSummarizeDueDate(), ""))
        ConfigGridCol(intCol, mintColTerms, "Terms", 800,
            Function(objTrx As Trx) If(objTrx.GetType() Is GetType(NormalTrx), DirectCast(objTrx, NormalTrx).strSummarizeTerms(), ""))
        ConfigGridCol(intCol, mintColFake, "Fake", 500,
            Function(objTrx As Trx) If(objTrx.GetType() Is GetType(NormalTrx), If(objTrx.blnFake, "Y", ""), "-"))
        ConfigGridCol(intCol, mintColAutoGenerated, "Gen", 500,
            Function(objTrx As Trx) If(objTrx.blnAutoGenerated, "Y", ""))
        ConfigGridCol(intCol, mintColImportKey, "Imported", 800,
            Function(objTrx As Trx) If(objTrx.GetType() Is GetType(NormalTrx),
                If(String.IsNullOrEmpty(DirectCast(objTrx, NormalTrx).strImportKey), "", "Y"), ""))
        ConfigGridCol(intCol, mintColStatus, "Status", 600,
            Function(objTrx As Trx) If(objTrx.GetType() Is GetType(NormalTrx),
                "? RNS".Substring(objTrx.lngStatus, 1), ""))
        grdReg.Columns(mintColDescr).Frozen = True

        grdReg.ColumnCount = intCol
    End Sub

    Private Sub ConfigGridCol(ByRef intColCounter As Integer, ByRef intColSave As Integer, ByVal strLabel As String,
        ByVal intWidth As Integer, ByVal strValueFunc As strColumnValue, Optional ByVal blnRightAlign As Boolean = False)

        Dim col As DataGridViewTextBoxColumn
        col = New DataGridViewTextBoxColumn()
        col.HeaderText = strLabel
        col.Width = CInt(intWidth / 12)
        If blnRightAlign Then
            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight
        End If
        grdReg.Columns.Add(col)
        intColSave = intColCounter
        mstrColValueFuncs(intColSave) = strValueFunc
        intColCounter = intColCounter + 1
    End Sub

    Private Sub LoadGrid()
        grdReg.RowCount = mobjReg.lngTrxCount
        Dim objSelectedTrx As Trx = mobjReg.objFirstOnOrAfter(Today)
        If Not (objSelectedTrx Is Nothing) Then
            mobjReg.SetCurrent(objSelectedTrx)
            mobjReg.FireShowCurrent()
        End If
        RefreshPage()
        DiagnosticValidate()
    End Sub

    Private Sub grdReg_CellValueNeeded(ByVal sender As System.Object,
        ByVal e As System.Windows.Forms.DataGridViewCellValueEventArgs) Handles grdReg.CellValueNeeded
        If Not mobjReg Is Nothing Then
            Dim objTrx As Trx
            If e.RowIndex < mobjReg.lngTrxCount Then
                objTrx = mobjReg.objTrx(e.RowIndex + 1)
                e.Value = mstrColValueFuncs(e.ColumnIndex)(objTrx)
            End If
        End If
    End Sub

    Private Sub grdReg_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles grdReg.CellFormatting
        Dim objTrx As Trx
        If Not mobjReg Is Nothing Then
            If e.RowIndex < mobjReg.lngTrxCount Then
                objTrx = mobjReg.objTrx(e.RowIndex + 1)
                If objTrx.blnFake Then
                    e.CellStyle.BackColor = mlngCOLOR_FAKE
                Else
                    e.CellStyle.BackColor = mlngCOLOR_REAL
                End If
                If objTrx.GetType() Is GetType(ReplicaTrx) Then
                    e.CellStyle.ForeColor = mlngCOLOR_REPLICA
                ElseIf objTrx.curAmount > 0 Then
                    e.CellStyle.ForeColor = mlngCOLOR_CREDIT
                ElseIf objTrx.GetType() Is GetType(BudgetTrx) Then
                    e.CellStyle.ForeColor = mlngCOLOR_BUDGET
                Else
                    e.CellStyle.ForeColor = Color.Black
                End If
            End If
        End If
    End Sub

    Private Sub grdReg_CurrentCellChanged(sender As Object, e As EventArgs) Handles grdReg.CurrentCellChanged
        Dim objTrx As Trx = objCurrentTrx()
        If Not (objTrx Is Nothing) Then
            mobjReg.SetCurrent(objTrx)
        End If
    End Sub

    Private Function strRepeatUnit(ByVal lngRepeatUnit As Trx.RepeatUnit) As String
        Select Case lngRepeatUnit
            Case Trx.RepeatUnit.Day
                strRepeatUnit = "Days"
            Case Trx.RepeatUnit.Week
                strRepeatUnit = "Weeks"
            Case Trx.RepeatUnit.Month
                strRepeatUnit = "Months"
            Case Else
                strRepeatUnit = ""
        End Select
    End Function

    Private Function lngIndexToGridRow(ByVal lngIndex As Integer) As Integer
        Return lngIndex - 1
    End Function

    Private Sub mfrmSearch_FormClosed(ByVal eventSender As System.Object,
        ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles mfrmSearch.SearchFormClosed

        mfrmSearch = Nothing
    End Sub

    Private Sub mobjReg_ManyTrxChanged() Handles mobjReg.ManyTrxChanged
        Try
            grdReg.Invalidate()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mobjReg_BudgetChanged(ByVal objBudget As Trx) Handles mobjReg.BudgetChanged
        Try
            grdReg.Invalidate()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mobjReg_BeginRegenerating() Handles mobjReg.BeginRegenerating
        Try

            mblnOldVisible = Me.Visible
            Me.Visible = False
            System.Windows.Forms.Application.DoEvents()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mobjReg_EndRegenerating() Handles mobjReg.EndRegenerating
        Try

            'Cannot just invalidate grid, because number of rows may have changed.
            LoadGrid()
            Me.Visible = mblnOldVisible

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mobjReg_ShowCurrent(ByVal objTrx As Trx) Handles mobjReg.ShowCurrent
        Try

            Dim lngGridRow As Integer = lngIndexToGridRow(objTrx.lngIndex)
            If lngGridRow < grdReg.Rows.Count Then
                grdReg.CurrentCell = grdReg.Rows(lngGridRow).Cells(0)
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mobjReg_TrxAdded(ByVal objTrx As Trx) Handles mobjReg.TrxAdded
        Try

            grdReg.RowCount = grdReg.RowCount + 1
            grdReg.Invalidate()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mobjReg_TrxDeleted(ByVal objTrx As Trx) Handles mobjReg.TrxDeleted
        Try

            grdReg.RowCount = grdReg.RowCount - 1
            grdReg.Invalidate()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mobjReg_TrxUpdated(ByVal blnPositionChanged As Boolean, ByVal objTrx As Trx) Handles mobjReg.TrxUpdated
        Try

            If blnPositionChanged Then
                grdReg.Invalidate()
            Else
                InvalidateGridRow(objTrx)
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub InvalidateGridRow(ByVal objTrx As Trx)
        grdReg.InvalidateRow(lngIndexToGridRow(objTrx.lngIndex))
    End Sub

    Private Sub mobjReg_ValidationError(ByVal objTrx As Trx, ByVal strMsg As String) Handles mobjReg.ValidationError
        Dim strTrxSummary As String = "(none)"

        Try

            If mblnShowValidationErrors Then
                If Not (objTrx Is Nothing) Then
                    strTrxSummary = strTrxSummaryForMsg(objTrx)
                End If
                Dim result As MsgBoxResult = MsgBox("Validation error on register trx " & strTrxSummary & ":" & vbCrLf & strMsg & vbCrLf & "Show more errors?", MsgBoxStyle.YesNo)
                If result = MsgBoxResult.No Then
                    mblnShowValidationErrors = False
                End If
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function strTrxSummaryForMsg(ByVal objTrx As Trx) As String
        strTrxSummaryForMsg = Utilities.strFormatDate(objTrx.datDate) & " " & objTrx.strDescription & " $" & Utilities.strFormatCurrency(objTrx.curAmount)
    End Function

    Private Sub DiagnosticValidate()
        mblnShowValidationErrors = True
        mobjReg.ValidateRegister()
    End Sub
End Class