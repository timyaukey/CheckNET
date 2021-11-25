Option Strict On
Option Explicit On

Imports System.Windows.Forms

Public MustInherit Class MenuElementBase
    Public ReadOnly Property Title As String
    Public ReadOnly Property SortCode As Integer
    Public MenuItemControl As ToolStripMenuItem

    Public Sub New(ByVal title_ As String, ByVal sortCode_ As Integer)
        Title = title_
        SortCode = sortCode_
    End Sub

    Public Overridable Sub CreateUIElement(ByVal mnuParent As ToolStripMenuItem)
        Dim mnuNewItem As ToolStripMenuItem = New ToolStripMenuItem()
        mnuNewItem.Text = Me.Title
        mnuParent.DropDownItems.Add(mnuNewItem)
        MenuItemControl = mnuNewItem
    End Sub

    Public Property Enabled() As Boolean
        Get
            Return MenuItemControl.Enabled
        End Get
        Set(value As Boolean)
            MenuItemControl.Enabled = value
        End Set
    End Property
End Class
