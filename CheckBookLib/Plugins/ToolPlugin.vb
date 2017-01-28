Option Strict On
Option Explicit On

''' <summary>
''' All plugins that are added to the main program user interface
''' must subclass this class. There is a complex inheritance tree
''' based on this root class, and the main program user interface
''' decides which menu to add a plugin too based on which class
''' in the heirarchy the plugin inherits from. For example, all
''' check importers must inherit from CheckImportPlugin.
''' </summary>

Public MustInherit Class ToolPlugin
    Protected ReadOnly HostUI As IHostUI
    Public Sub New(ByVal hostUI_ As IHostUI)
        HostUI = hostUI_
        SortCode = 10
    End Sub
    Public MustOverride Function GetMenuTitle() As String
    Public MustOverride Sub ClickHandler(ByVal sender As Object, ByVal e As EventArgs)
    Public SortCode As Integer
End Class
