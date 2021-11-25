Option Strict On
Option Explicit On

Imports System.Windows.Forms

Public Class MenuBuilder
    Public Elements As List(Of MenuElementBase) = New List(Of MenuElementBase)
    Protected Friend MnuParent As ToolStripMenuItem

    Public Sub New()
        Me.New(Nothing)
    End Sub

    Public Sub New(ByVal mnuParent_ As ToolStripMenuItem)
        MnuParent = mnuParent_
    End Sub

    Public Sub Add(ByVal element As MenuElementBase)
        Elements.Add(element)
    End Sub

    Public Sub AddElementsToMenu()
        Elements.Sort(AddressOf ElementComparer)
        For Each element As MenuElementBase In Elements
            element.CreateUIElement(MnuParent)
        Next
    End Sub

    Private Function ElementComparer(ByVal element1 As MenuElementBase, ByVal element2 As MenuElementBase) As Integer
        Dim intCompare As Integer = element1.SortCode().CompareTo(element2.SortCode())
        If intCompare = 0 Then
            intCompare = element1.Title().CompareTo(element2.Title())
        End If
        Return intCompare
    End Function
End Class
