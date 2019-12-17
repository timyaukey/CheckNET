Option Strict On
Option Explicit On

Imports System.Windows.Forms

Public MustInherit Class MenuElementBase
    Public ReadOnly Property Title As String
    Public ReadOnly Property SortCode As Integer
    Public ReadOnly Property PluginPath As String

    Public Sub New(ByVal title_ As String, ByVal sortCode_ As Integer, ByVal pluginPath_ As String)
        Title = title_
        SortCode = sortCode_
        PluginPath = pluginPath_
    End Sub

    Public MustOverride Sub CreateUIElement(ByVal mnuParent_ As ToolStripMenuItem)
End Class
