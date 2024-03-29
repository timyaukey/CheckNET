﻿Option Strict On
Option Explicit On


Public Class AdjustPersonalBusinessForm
    Private mobjHostUI As IHostUI
    Private mobjBusinessAccount As Account
    Private mobjCompany As Company
    Private mobjPersonalAccount As Account
    Private mobjLoanToAccount As Account
    Private mstrPersonalPaymentKey As String
    Private mstrPersonalExpenseKey As String
    Private mstrLoanToPersonKey As String
    Private mblnFakeTrxFound As Boolean

    Private ReadOnly mstrDivideMarker As String = ":DIVIDE"
    Private ReadOnly mstrPaymentMarker As String = ":PERSONAL-PAY"
    Private ReadOnly mstrExpenseMarker As String = ":PERSONAL-EXP"

    Public Sub ShowModal(ByVal objHostUI As IHostUI, ByVal objBusinessAccount As Account)
        mobjHostUI = objHostUI
        mobjCompany = mobjHostUI.Company
        mobjBusinessAccount = objBusinessAccount

        If objBusinessAccount.AcctType <> Account.AccountType.Liability Then
            mobjHostUI.ErrorMessageBox("Only liability accounts may be adjusted for personal/business use")
            Exit Sub
        End If

        If objBusinessAccount.RelatedAcct1 Is Nothing Then
            mobjHostUI.ErrorMessageBox("Related account #1 is not set (this is the related personal account)")
            Exit Sub
        End If
        mobjPersonalAccount = objBusinessAccount.RelatedAcct1
        txtPersonalAccount.Text = mobjPersonalAccount.Title

        If objBusinessAccount.RelatedAcct2 Is Nothing Then
            mobjHostUI.ErrorMessageBox("Related account #2 is not set (this is the loan to person account)")
            Exit Sub
        End If
        mobjLoanToAccount = objBusinessAccount.RelatedAcct2
        mstrLoanToPersonKey = mobjLoanToAccount.Registers(0).CatKey
        txtLoanToAccount.Text = mobjLoanToAccount.Title

        For intCatIndex As Integer = 1 To mobjCompany.Categories.ElementCount
            Dim strCatName As String = mobjCompany.Categories.GetValue1(intCatIndex)
            If strCatName.Contains("Personal") Then
                If strCatName.Contains("Expense") Then
                    mstrPersonalExpenseKey = mobjCompany.Categories.GetKey(intCatIndex)
                    txtPersonalExpenses.Text = mobjCompany.Categories.GetValue1(intCatIndex)
                ElseIf strCatName.Contains("Payment") Then
                    mstrPersonalPaymentKey = mobjCompany.Categories.GetKey(intCatIndex)
                    txtPersonalPayments.Text = mobjCompany.Categories.GetValue1(intCatIndex)
                End If
            End If
        Next
        If mstrPersonalPaymentKey Is Nothing Then
            mobjHostUI.ErrorMessageBox("Cannot find a category with ""Personal"" and ""Payment"" in the name")
            Exit Sub
        End If
        If mstrPersonalExpenseKey Is Nothing Then
            mobjHostUI.ErrorMessageBox("Cannot find a category with ""Personal"" and ""Expense"" in the name")
            Exit Sub
        End If
        Me.ShowDialog()
    End Sub

    Private Sub btnDeleteAdjustments_Click(sender As Object, e As EventArgs) Handles btnDeleteAdjustments.Click
        Try
            If MsgBox("Are you sure you want to delete adjustment transactions for this date range?", MsgBoxStyle.OkCancel, "Confirm") <> MsgBoxResult.Ok Then
                Exit Sub
            End If
            If Not blnValidDates() Then
                Exit Sub
            End If
            Dim intDeleteCount As Integer =
                intDeleteAdjustments(mobjBusinessAccount) +
                intDeleteAdjustments(mobjPersonalAccount)
            mobjHostUI.InfoMessageBox(intDeleteCount.ToString() + " adjustment transactions deleted")
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub btnRecreateAdjustments_Click(sender As Object, e As EventArgs) Handles btnRecreateAdjustments.Click
        Try
            If MsgBox("Are you sure you want to (re)create adjustment transactions for this date range?", MsgBoxStyle.OkCancel, "Confirm") <> MsgBoxResult.Ok Then
                Exit Sub
            End If
            If Not blnValidDates() Then
                Exit Sub
            End If
            Dim intDeleteCount As Integer =
                intDeleteAdjustments(mobjBusinessAccount) +
                intDeleteAdjustments(mobjPersonalAccount)
            Dim intAdjustCount As Integer = intCreateAdjustments()
            mobjHostUI.InfoMessageBox(intDeleteCount.ToString() + " adjustments transactions deleted, " + intAdjustCount.ToString() + " adjustment transactions created")
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Function blnValidDates() As Boolean
        If ctlStartDate.Value > ctlEndDate.Value Then
            mobjHostUI.InfoMessageBox("Start date must be before end date")
            Return False
        End If
        Return True
    End Function

    Private Function intDeleteAdjustments(ByVal objAccount As Account) As Integer
        Dim colToDelete As List(Of BaseTrx) = New List(Of BaseTrx)
        Dim objTrx As BaseTrx
        Dim intDeleteCount As Integer = 0
        For Each objReg As Register In objAccount.Registers
            ShowProgress("Scanning " + objReg.Title)
            For Each objTrx In objReg.GetDateRange(Of BaseTrx)(ctlStartDate.Value, ctlEndDate.Value)
                If blnIsInvalid(objTrx) Then
                    mobjHostUI.InfoMessageBox("Invalid transaction found: " + objTrx.ToString())
                End If
                If blnIsAdjustment(objTrx) Then
                    colToDelete.Add(objTrx)
                End If
            Next
        Next
        For Each objTrx In colToDelete
            ShowProgress("Delete " + objTrx.ToString())
            intDeleteCount += 1
            objTrx.Delete(New LogDelete(), "PersonalBusinessForm.DeleteAdjustment")
        Next
        Return intDeleteCount
    End Function

    Private Function blnIsAdjustment(ByVal objTrx As BaseTrx) As Boolean
        Return objTrx.Description.EndsWith(mstrExpenseMarker) Or
            objTrx.Description.EndsWith(mstrPaymentMarker)
    End Function

    Private Function blnIsInvalid(ByVal objTrx As BaseTrx) As Boolean
        If objTrx.Description.Contains(":") Then
            If objTrx.Description.EndsWith(mstrDivideMarker) Then
                Return False
            End If
            If objTrx.Description.EndsWith(mstrPaymentMarker) Then
                Return False
            End If
            If objTrx.Description.EndsWith(mstrExpenseMarker) Then
                Return False
            End If
            Return True
        End If
    End Function

    Private Function intCreateAdjustments() As Integer
        Dim objPersonalReg As Register
        Dim intAdjustCount As Integer = 0
        mblnFakeTrxFound = False
        objPersonalReg = mobjPersonalAccount.Registers(0)
        For Each objBusinessReg As Register In mobjBusinessAccount.Registers
            intAdjustCount += intCreateAdjustments(objBusinessReg, objPersonalReg)
        Next
        If mblnFakeTrxFound Then
            mobjHostUI.InfoMessageBox("Note: There were fake transactions in the business account in this date range.")
        End If
        Return intAdjustCount
    End Function

    Private Function intCreateAdjustments(ByVal objBusinessReg As Register, ByVal objPersonalReg As Register) As Integer
        Dim objBusinessTrx As BaseTrx
        Dim colToDivide As List(Of BaseTrx) = New List(Of BaseTrx)
        Dim intAdjustCount As Integer = 0
        ShowProgress("Scanning " + objBusinessReg.Title)
        For Each objBusinessTrx In objBusinessReg.GetDateRange(Of BaseTrx)(ctlStartDate.Value, ctlEndDate.Value)
            If blnTrxNeedsDividing(objBusinessTrx) Then
                colToDivide.Add(objBusinessTrx)
            End If
        Next
        For Each objBusinessTrx In colToDivide
            ShowProgress("Adjusting " + objBusinessTrx.ToString())
            intAdjustCount += intDivideTrx(objBusinessTrx, objPersonalReg)
        Next
        Return intAdjustCount
    End Function

    Private Function blnTrxNeedsDividing(ByVal objBusinessTrx As BaseTrx) As Boolean
        Return (blnIsMarkedToDivide(objBusinessTrx) Or blnIsPayment(objBusinessTrx))
    End Function

    Private Function blnIsMarkedToDivide(ByVal objBusinessTrx As BaseTrx) As Boolean
        'Has to be a BankTrx because the adjustment in the business register
        'is a BankTrx and it has to use the same category as the first split
        'of objBusinessTrx.
        If TypeOf objBusinessTrx Is BankTrx Then
            Return objBusinessTrx.Description.EndsWith(mstrDivideMarker)
        End If
        Return False
    End Function

    Private Function blnIsPayment(ByVal objBusinessTrx As BaseTrx) As Boolean
        '"Is a payment" means "is a transfer from another balance sheet account".
        'We don't need any categories from this one so it doesn't have to be a BankTrx,
        'because the adjustment is a transfer to the "loan to person" account.
        Dim objNormalTrx As BankTrx
        If objBusinessTrx.Amount > 0 Then
            If TypeOf objBusinessTrx Is ReplicaTrx Then
                Return True
            End If
            objNormalTrx = TryCast(objBusinessTrx, BankTrx)
            If Not objNormalTrx Is Nothing Then
                Return objNormalTrx.Splits(0).HasReplicaTrx
            End If
        End If
        Return False
    End Function

    Private Function intDivideTrx(ByVal objBusinessTrx As BaseTrx, ByVal objPersonalReg As Register) As Integer
        Dim curBusinessBalance As Decimal = curGetAccountBalance(mobjBusinessAccount, objBusinessTrx.TrxDate)
        Dim curPersonalBalance As Decimal = curGetAccountBalance(mobjPersonalAccount, objBusinessTrx.TrxDate)
        Dim curBusinessPart As Decimal
        Dim curPersonalPart As Decimal
        Dim curTotalBalance As Decimal
        Dim strOrigCatKey As String
        Dim intMarkerOffset As Integer
        Dim strNewDescription As String
        Dim datAdjustmentDate As DateTime

        curTotalBalance = curBusinessBalance + curPersonalBalance
        If curTotalBalance = 0D Then
            curBusinessPart = objBusinessTrx.Amount
        Else
            curBusinessPart = (curBusinessBalance / curTotalBalance) * objBusinessTrx.Amount
            curBusinessPart = System.Math.Round(curBusinessPart, 2)
        End If
        curPersonalPart = objBusinessTrx.Amount - curBusinessPart
        datAdjustmentDate = objBusinessTrx.TrxDate

        'add two new trx to move part to the personal register
        If blnIsPayment(objBusinessTrx) Then
            'Is a loan payment.
            strNewDescription = objBusinessTrx.Description + mstrPaymentMarker
            CreateAdjustmentTrx(objBusinessTrx.Register, datAdjustmentDate, strNewDescription, mstrLoanToPersonKey, -curPersonalPart)
            CreateAdjustmentTrx(objPersonalReg, datAdjustmentDate, strNewDescription, mstrPersonalPaymentKey, curPersonalPart)
        ElseIf blnIsMarkedToDivide(objBusinessTrx) Then
            'Is a charge of some kind.
            intMarkerOffset = objBusinessTrx.Description.IndexOf(mstrDivideMarker)
            If intMarkerOffset <= 0 Then
                Throw New Exception("Dividable expense trx has no divide marker")
            End If
            strNewDescription = objBusinessTrx.Description.Substring(0, intMarkerOffset) + mstrExpenseMarker
            strOrigCatKey = DirectCast(objBusinessTrx, BankTrx).Splits(0).CategoryKey
            CreateAdjustmentTrx(objBusinessTrx.Register, datAdjustmentDate, strNewDescription, strOrigCatKey, -curPersonalPart)
            CreateAdjustmentTrx(objPersonalReg, datAdjustmentDate, strNewDescription, mstrPersonalExpenseKey, curPersonalPart)
        Else
            'This should never happen, because blnTrxNeedsDividing() only returns trx
            'that match the two conditions above.
            Throw New Exception("Do not know how to divide transaction")
        End If
        Return 2
    End Function

    Private Sub CreateAdjustmentTrx(ByVal objRegister As Register, ByVal datDate As DateTime, ByVal strDescription As String,
                                    ByVal strCatKey As String, ByVal curAmount As Decimal)
        Dim objTrx As BankTrx = New BankTrx(objRegister)
        objTrx.NewStartNormal(True, "Pmt", datDate, strDescription, "", BaseTrx.TrxStatus.Unreconciled, False, 0D, False, False, 0, "", "")
        objTrx.AddSplit("", strCatKey, "", "", Utilities.EmptyDate, Utilities.EmptyDate, "", "", curAmount)
        objRegister.NewAddEnd(objTrx, New LogAdd(), "AdjustPersonalBusiness.AddTrx")
    End Sub

    Private Function curGetAccountBalance(ByVal objAccount As Account, ByVal datAfterEndDate As DateTime) As Decimal
        Dim curResult As Decimal = 0D
        Dim datEndDate As DateTime = ctlEndDate.Value
        For Each objReg As Register In objAccount.Registers
            For Each objTrx As BaseTrx In objReg.GetAllTrx(Of BaseTrx)()
                If objTrx.TrxDate >= datAfterEndDate Then
                    Exit For
                End If
                'Don't use GetTrx.curBalance, because we want to use only "real" trx in computing the balance.
                If objTrx.IsFake Then
                    mblnFakeTrxFound = True
                Else
                    curResult = curResult + objTrx.Amount
                End If
            Next
        Next
        Return curResult
    End Function

    Private Sub ShowProgress(ByVal strMessage As String)
        lblProgress.Text = strMessage
        lblProgress.Refresh()
    End Sub

    Private Sub btnHelp_Click(sender As Object, e As EventArgs) Handles btnHelp.Click
        mobjHostUI.InfoMessageBox("Divide payment, interest and fee transactions in the current " +
            "account into personal " +
            "and business parts. May only be used on a liability account, e.g. a credit card used " +
            "for both business and personal purchases. Finds all expense transactions ending in """ +
            mstrDivideMarker + """ (normally only interest And fees), and all payments made to the account, " +
            "and divides them into business and personal amounts in proportion to the balances " +
            "on that date of the liability account and its related personal account. Business and " +
            "personal purchases are recorded directly in their respective accounts without any " +
            "special handling.")
        mobjHostUI.InfoMessageBox("Divides the transactions by creating " +
            "additional transactions to move the personal amount to its related personal account " +
            "and an account recording loans to that person. This way personal use becomes a loan " +
            "to the person. The new transactions always end in """ + mstrPaymentMarker + """ and """ +
            mstrExpenseMarker + """ to indicate to the system what they are for.")
        mobjHostUI.InfoMessageBox("Requires two related accounts to be indicated in the definition for this account: " +
            "#1 is the related account representing the personal portion of the balance " +
            "(account type ""personal""), and #2 is the loan to person account (account type ""asset""). " +
            "Also requires two personal categories to be set up with " +
            "specific text in their names to use in the related personal account to record " +
            "the personal parts of purchases and payments that have been divided. " +
            "Try to use this tool without setting these things " +
            "up to see error messages with specific guidance.")
    End Sub
End Class