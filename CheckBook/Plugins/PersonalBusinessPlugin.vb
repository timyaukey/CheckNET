Option Strict On
Option Explicit On


Public Class PersonalBusinessPlugin
    Inherits PluginBase

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register(ByVal setup As IHostSetup)
        setup.ToolMenu.Add(New MenuElementRegister(HostUI, "Adjust Account For Personal Use", 110, AddressOf ClickHandler, GetPluginPath()))
    End Sub

    Private Sub ClickHandler(sender As Object, e As RegisterEventArgs)
        Try
            Dim frmAdjust As AdjustPersonalBusinessForm = New AdjustPersonalBusinessForm
            frmAdjust.ShowModal(HostUI, e.Reg.Account)
            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub
End Class
