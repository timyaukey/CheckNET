Option Strict On
Option Explicit On


Friend Class ReconAcctSelectForm
    Inherits System.Windows.Forms.Form

    Private mobjHostUI As IHostUI
    Private mobjCompany As Company

    Public Sub Init(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
        mobjCompany = mobjHostUI.Company
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        Dim objAccount As Account
        Dim frm As ReconcileForm

        Try

            objAccount = UITools.GetSelectedAccountAndUnload(lstAccounts, Me, mobjCompany)
            If objAccount Is Nothing Then
                mobjHostUI.ErrorMessageBox("Please select the account to reconcile.")
                Exit Sub
            End If

            frm = New ReconcileForm
            frm.ShowMe(mobjHostUI, objAccount)

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub ReconAcctSelectForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Try

            UITools.LoadAccountListBox(lstAccounts, mobjCompany)

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub lstAccounts_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lstAccounts.DoubleClick
        cmdOkay_Click(cmdOkay, New System.EventArgs())
    End Sub
End Class