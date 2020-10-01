Option Strict On
Option Explicit On


Public Class FindLiveBudgetsPlugin
    Inherits PluginBase

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register(ByVal setup As IHostSetup)
        setup.objReportMenu.Add(New MenuElementAction("Find Live Budgets", 100, AddressOf ClickHandler, GetPluginPath()))
    End Sub

    Private Sub ClickHandler(sender As Object, e As EventArgs)
        Try
            Dim objReg As Register = HostUI.objGetCurrentRegister()
            If objReg Is Nothing Then
                HostUI.ErrorMessageBox("Please click on the register window you wish to search.")
                Exit Sub
            End If
            Dim frmFind As LiveBudgetListForm = New LiveBudgetListForm
            frmFind.ShowModal(HostUI, objReg, HostUI.objCompany.objBudgets)
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub
End Class
