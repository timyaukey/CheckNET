Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class ReconcilePlugin
    Inherits ToolPlugin

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub ClickHandler(sender As Object, e As EventArgs)
        Try
            Dim frm As ReconAcctSelectForm = New ReconAcctSelectForm
            frm.Init(HostUI.objCompany)
            frm.ShowDialog()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Overrides Function GetMenuTitle() As String
        Return "Reconcile"
    End Function

    Public Overrides Function SortCode() As Integer
        Return 1
    End Function
End Class
