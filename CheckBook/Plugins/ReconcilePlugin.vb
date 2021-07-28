Option Strict On
Option Explicit On


Public Class ReconcilePlugin
    Inherits PluginBase

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register(ByVal setup As IHostSetup)
        setup.objToolMenu.Add(New MenuElementAction("Reconcile", 1, AddressOf ClickHandler, GetPluginPath()))
    End Sub

    Private Sub ClickHandler(sender As Object, e As EventArgs)
        Try
            Dim frm As ReconAcctSelectForm = New ReconAcctSelectForm
            frm.Init(HostUI)
            frm.ShowDialog()
            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub
End Class
