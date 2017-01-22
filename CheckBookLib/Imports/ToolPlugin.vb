Option Explicit On
Option Strict On

Public MustInherit Class ToolPlugin
    Protected ReadOnly HostUI As IHostUI
    Public Sub New(ByVal hostUI_ As IHostUI)
        HostUI = hostUI_
    End Sub
    Public MustOverride Function GetMenuTitle() As String
    Public MustOverride Sub ClickHandler(ByVal sender As Object, ByVal e As EventArgs)
End Class
