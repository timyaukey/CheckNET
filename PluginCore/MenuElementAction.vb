Option Strict On
Option Explicit On

Imports CheckBookLib
Imports System.Windows.Forms

Public Class MenuElementAction
    Inherits MenuElementBase

    Private ReadOnly ClickHandler As EventHandler

    Public Sub New(ByVal title_ As String, ByVal sortCode_ As Integer, ByVal clickHandler_ As EventHandler, ByVal pluginPath_ As String)
        MyBase.New(title_, sortCode_, pluginPath_)
        ClickHandler = clickHandler_
    End Sub

    Public Overrides Sub CreateUIElement(ByVal mnuParent As ToolStripMenuItem)
        Dim mnuNewItem As ToolStripMenuItem = New ToolStripMenuItem()
        mnuNewItem.Text = Me.Title
        AddHandler mnuNewItem.Click, Me.ClickHandler
        mnuParent.DropDownItems.Add(mnuNewItem)
    End Sub
End Class
