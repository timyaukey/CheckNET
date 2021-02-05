Option Strict On
Option Explicit On


Public Class FindLiveBudgetsPlugin
    Inherits PluginBase

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register(ByVal setup As IHostSetup)
        setup.objReportMenu.Add(New MenuElementRegister(HostUI, "Find Live Budgets", 100, AddressOf ClickHandler, GetPluginPath()))
    End Sub

    Private Sub ClickHandler(sender As Object, e As RegisterEventArgs)
        Try
            Dim frmFind As LiveBudgetListForm = New LiveBudgetListForm
            frmFind.ShowModal(HostUI, e.objReg, HostUI.objCompany.objBudgets)
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub
End Class
