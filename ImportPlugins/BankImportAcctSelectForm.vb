Option Strict Off
Option Explicit On

Imports CheckBookLib
Imports PluginCore

Public Class BankImportAcctSelectForm
    Inherits System.Windows.Forms.Form

    Private mobjHostUI As IHostUI
    Private mobjCompany As Company
    Private mobjAccount As Account

    Public Sub ShowMe(ByVal objHostUI As IHostUI, ByVal strTitle As String, ByVal objImportHandler As IImportHandler, ByVal objTrxReader As ITrxReader)

        Dim frm As BankImportForm
        Try
            mobjHostUI = objHostUI
            mobjCompany = mobjHostUI.objCompany
            mobjAccount = Nothing
            UITools.LoadAccountListBox(lstAccounts, mobjCompany)
            Me.ShowDialog()
            Application.DoEvents()
            If Not mobjAccount Is Nothing Then
                frm = New BankImportForm
                frm.ShowMe(strTitle, mobjAccount, objImportHandler, objTrxReader, objHostUI)
            End If
            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        Try

            mobjAccount = UITools.objGetSelectedAccountAndUnload(lstAccounts, Me, mobjCompany)
            If mobjAccount Is Nothing Then
                mobjHostUI.ErrorMessageBox("Please select the account to import into.")
                Exit Sub
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub lstAccounts_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lstAccounts.DoubleClick
        cmdOkay_Click(cmdOkay, New System.EventArgs())
    End Sub
End Class