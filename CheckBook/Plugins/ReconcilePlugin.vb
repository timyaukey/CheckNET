Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class ReconcilePlugin
    Inherits ToolPlugin

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
        SortCode = 1
    End Sub

    Public Overrides Sub ClickHandler(sender As Object, e As EventArgs)
        Try
            Dim frm As ReconAcctSelectForm = New ReconAcctSelectForm
            frm.ShowDialog()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Overrides Function GetMenuTitle() As String
        Return "Reconcile"
    End Function
End Class
