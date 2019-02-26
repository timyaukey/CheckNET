Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class TrialBalancePlugIn
    Inherits ToolPlugin

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register()
        HostUI.objReportMenu.Add(New MenuElementAction("Financial Statements", 2, AddressOf ClickHandler, GetPluginPath()))
    End Sub

    Private Sub ClickHandler(sender As Object, e As EventArgs)
        Dim frm As TrialBalanceForm = New TrialBalanceForm()
        frm.ShowWindow(Me.HostUI)
    End Sub
End Class
