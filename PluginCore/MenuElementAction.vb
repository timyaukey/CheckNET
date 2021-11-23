Option Strict On
Option Explicit On

Imports System.Windows.Forms

Public Class MenuElementAction
    Inherits MenuElementBase

    Private ReadOnly ClickHandler As EventHandler
    Public MenuItemControl As ToolStripMenuItem

    Public Sub New(ByVal title_ As String, ByVal sortCode_ As Integer, ByVal clickHandler_ As EventHandler)
        MyBase.New(title_, sortCode_)
        ClickHandler = clickHandler_
    End Sub

    Public Overrides Sub CreateUIElement(ByVal mnuParent As ToolStripMenuItem)
        Dim mnuNewItem As ToolStripMenuItem = New ToolStripMenuItem()
        mnuNewItem.Text = Me.Title
        AddHandler mnuNewItem.Click, Me.ClickHandler
        mnuParent.DropDownItems.Add(mnuNewItem)
        MenuItemControl = mnuNewItem
    End Sub
End Class
