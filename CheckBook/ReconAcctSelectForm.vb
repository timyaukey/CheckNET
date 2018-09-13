Option Strict On
Option Explicit On

Imports CheckBookLib

Friend Class ReconAcctSelectForm
    Inherits System.Windows.Forms.Form

    Private mobjCompany As Company

    Public Sub Init(ByVal objCompany As Company)
        mobjCompany = objCompany
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        Dim objAccount As Account
        Dim frm As ReconcileForm

        Try

            objAccount = UITools.objGetSelectedAccountAndUnload(lstAccounts, Me, mobjCompany)
            If objAccount Is Nothing Then
                MsgBox("Please select the account to reconcile.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            frm = New ReconcileForm
            frm.ShowMe(objAccount)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub ReconAcctSelectForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Try

            UITools.LoadAccountListBox(lstAccounts, mobjCompany)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub lstAccounts_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lstAccounts.DoubleClick
        cmdOkay_Click(cmdOkay, New System.EventArgs())
    End Sub
End Class