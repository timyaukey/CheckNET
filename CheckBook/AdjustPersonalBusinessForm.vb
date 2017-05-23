Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class AdjustPersonalBusinessForm
    Private mobjBusinessAccount As Account
    Private mobjCompany As Company
    Private mobjPersonalAccount As Account
    Private mstrPersonalPaymentKey As String
    Private mstrPersonalExpenseKey As String

    Public Sub ShowModal(ByVal objBusinessAccount As Account)
        mobjBusinessAccount = objBusinessAccount
        mobjCompany = mobjBusinessAccount.objCompany
        Dim strPersonalAccountName As String = mobjBusinessAccount.strTitle + ":Personal"
        For Each objAccount As Account In mobjCompany.colAccounts
            If objAccount.strTitle = strPersonalAccountName Then
                mobjPersonalAccount = objAccount
                Exit For
            End If
        Next
        If mobjPersonalAccount Is Nothing Then
            MsgBox("Cannot find an account named """ + strPersonalAccountName + """", MsgBoxStyle.Critical)
            Exit Sub
        End If
        For intCatIndex As Integer = 1 To mobjCompany.objCategories.intElements
            Dim strCatName As String = mobjCompany.objCategories.strValue1(intCatIndex)
            If strCatName.Contains("Personal") Then
                If strCatName.Contains("Expense") Then
                    mstrPersonalExpenseKey = mobjCompany.objCategories.strKey(intCatIndex)
                ElseIf strCatName.Contains("Payment") Then
                    mstrPersonalPaymentKey = mobjCompany.objCategories.strKey(intCatIndex)
                End If
            End If
        Next
        If mstrPersonalPaymentKey Is Nothing Then
            MsgBox("Cannot find a category with ""Personal"" and ""Payment"" in the name")
            Exit Sub
        End If
        If mstrPersonalExpenseKey Is Nothing Then
            MsgBox("Cannot find a category with ""Personal"" and ""Expense"" in the name")
            Exit Sub
        End If
        Me.ShowDialog()
    End Sub

    Private Sub btnDeleteAdjustments_Click(sender As Object, e As EventArgs) Handles btnDeleteAdjustments.Click
        If MsgBox("Are you sure you want to delete adjustment transactions for this date range?", MsgBoxStyle.OkCancel, "Confirm") <> MsgBoxResult.Ok Then
            Exit Sub
        End If
        If Not blnValidDates() Then
            Exit Sub
        End If
        DeleteAdjustments(mobjBusinessAccount)
        DeleteAdjustments(mobjPersonalAccount)
        MsgBox("Adjustment transactions deleted")
    End Sub

    Private Function blnValidDates() As Boolean
        If ctlStartDate.Value > ctlEndDate.Value Then
            MsgBox("Start date must be before end date")
            Return False
        End If
        Return True
    End Function

    Private Sub DeleteAdjustments(ByVal objAccount As Account)
        For Each objReg As Register In objAccount.colRegisters
            For Each objTrx As Trx In objReg.colDateRange(ctlStartDate.Value, ctlEndDate.Value)

            Next
        Next
    End Sub
End Class