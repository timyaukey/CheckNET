Option Strict On
Option Explicit On

Imports CheckBookLib

Friend Class StartupForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Public WithEvents mobjAccount As Account

    Public Sub Configure(ByVal objAccount As Account)
        mobjAccount = objAccount
    End Sub

    Private Sub mobjAccount_LoadStatus(ByVal strMessage As String) Handles mobjAccount.LoadStatus
        ShowStatus(strMessage)
    End Sub

    Public Sub ShowStatus(ByVal strMessage As String)
        lblMessage.Text = strMessage
        System.Windows.Forms.Application.DoEvents()
    End Sub
End Class