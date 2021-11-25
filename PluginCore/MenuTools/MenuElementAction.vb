Option Strict On
Option Explicit On

Imports System.Windows.Forms

Public Class MenuElementAction
    Inherits MenuElementBase

    Private ReadOnly ClickHandler As EventHandler

    Public Sub New(ByVal title_ As String, ByVal sortCode_ As Integer, ByVal clickHandler_ As EventHandler)
        MyBase.New(title_, sortCode_)
        ClickHandler = clickHandler_
    End Sub

    Public Overrides Sub CreateUIElement(ByVal mnuParent As ToolStripMenuItem)
        MyBase.CreateUIElement(mnuParent)
        AddHandler MenuItemControl.Click, Me.ClickHandler
    End Sub
End Class
