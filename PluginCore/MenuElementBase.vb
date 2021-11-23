Option Strict On
Option Explicit On

Imports System.Windows.Forms

Public MustInherit Class MenuElementBase
    Public ReadOnly Property Title As String
    Public ReadOnly Property SortCode As Integer

    Public Sub New(ByVal title_ As String, ByVal sortCode_ As Integer)
        Title = title_
        SortCode = sortCode_
    End Sub

    Public MustOverride Sub CreateUIElement(ByVal mnuParent_ As ToolStripMenuItem)
End Class
