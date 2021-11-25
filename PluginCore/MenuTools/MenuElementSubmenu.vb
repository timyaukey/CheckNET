Option Strict On
Option Explicit On

Imports System.Windows.Forms

Public Class MenuElementSubmenu
    Inherits MenuElementBase

    Public MenuBuilder As MenuBuilder

    Public Sub New(ByVal title_ As String, ByVal sortCode_ As Integer, ByVal builder_ As MenuBuilder)
        MyBase.New(title_, sortCode_)
        MenuBuilder = builder_
    End Sub

    Public Overrides Sub CreateUIElement(ByVal mnuParent As ToolStripMenuItem)
        MyBase.CreateUIElement(mnuParent)
        MenuBuilder.MnuParent = Me.MenuItemControl
        MenuBuilder.AddElementsToMenu()
    End Sub

End Class
