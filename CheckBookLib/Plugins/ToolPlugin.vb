Option Strict On
Option Explicit On

Imports CheckBookLib

''' <summary>
''' A convenience implementation of IToolPlugin.
''' </summary>

Public MustInherit Class ToolPlugin
    Implements IToolPlugin
    Protected ReadOnly HostUI As IHostUI

    Public Sub New(ByVal hostUI_ As IHostUI)
        HostUI = hostUI_
    End Sub

    Public MustOverride Function GetMenuTitle() As String Implements IToolPlugin.GetMenuTitle

    Public MustOverride Sub ClickHandler(ByVal sender As Object, ByVal e As EventArgs) Implements IToolPlugin.ClickHandler

    Public Overridable Function SortCode() As Integer Implements IToolPlugin.SortCode
        Return 10
    End Function
End Class
