Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class TrialBalancePlugIn
    Inherits ReportPlugin

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub ClickHandler(sender As Object, e As EventArgs)
        Dim frm As TrialBalanceForm = New TrialBalanceForm()
        frm.ShowWindow(Me.HostUI)
    End Sub

    Public Overrides Function GetMenuTitle() As String
        Return "Financial Statements"
    End Function
End Class
