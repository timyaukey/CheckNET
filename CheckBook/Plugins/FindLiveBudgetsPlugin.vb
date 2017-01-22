Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class FindLiveBudgetsPlugin
    Inherits ToolPlugin

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub ClickHandler(sender As Object, e As EventArgs)
        Try
            Dim objReg As Register = HostUI.objGetCurrentRegister()
            If objReg Is Nothing Then
                MsgBox("Please click on the register window you wish to search.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            Dim frmFind As LiveBudgetListForm = New LiveBudgetListForm
            frmFind.ShowModal(objReg, gobjBudgets)
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Overrides Function GetMenuTitle() As String
        Return "Find Live Budgets"
    End Function
End Class
