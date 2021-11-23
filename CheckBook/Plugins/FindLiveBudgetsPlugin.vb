Option Strict On
Option Explicit On


Public Class FindLiveBudgetsPlugin
    Inherits PluginBase

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register(ByVal setup As IHostSetup)
        setup.ReportMenu.Add(New MenuElementRegister(HostUI, "Find Live Budgets", 100, AddressOf ClickHandler))
        MetadataInternal = New PluginMetadata("Find Live Budgets", "Willow Creek Software",
            Reflection.Assembly.GetExecutingAssembly(), Nothing, "", Nothing)
    End Sub

    Private Sub ClickHandler(sender As Object, e As RegisterEventArgs)
        Try
            Dim frmFind As LiveBudgetListForm = New LiveBudgetListForm
            frmFind.ShowModal(HostUI, e.Reg, HostUI.Company.Budgets)
            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub
End Class
