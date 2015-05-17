Option Strict Off
Option Explicit On

Imports CheckBookLib

Friend Class ReconAcctSelectForm
    Inherits System.Windows.Forms.Form

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        Dim objAccount As Account
        Dim frm As ReconcileForm

        On Error GoTo ErrorHandler

        objAccount = gobjGetSelectedAccountAndUnload(lstAccounts, Me)
        If objAccount Is Nothing Then
            MsgBox("Please select the account to reconcile.", MsgBoxStyle.Critical)
            Exit Sub
        End If

        frm = New ReconcileForm
        frm.ShowMe(objAccount)

        Exit Sub
ErrorHandler:
        TopError("cmdOkay_Click")
    End Sub

    Private Sub ReconAcctSelectForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        On Error GoTo ErrorHandler

        gLoadAccountListBox(lstAccounts)

        Exit Sub
ErrorHandler:
        TopError("Form_Load")
    End Sub

    Private Sub TopError(ByVal strRoutine As String)
        gTopErrorTrap("ReconAcctSelectForm." & strRoutine)
    End Sub

    Private Sub lstAccounts_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lstAccounts.DoubleClick
        cmdOkay_Click(cmdOkay, New System.EventArgs())
    End Sub
End Class