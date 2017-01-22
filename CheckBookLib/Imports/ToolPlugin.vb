Option Explicit On
Option Strict On

Public MustInherit Class ToolPlugin
    Protected HostUI As IHostUI
    Public MustOverride ReadOnly Property Title() As String
    Public MustOverride Sub Handler(ByVal sender As Object, ByVal e As EventArgs)
    Public Sub Init(ByVal hostUI_ As IHostUI)
        HostUI = hostUI_
    End Sub
End Class
