Option Strict On
Option Explicit On

Imports System.Windows.Forms

Public Class ListViewSorter
    Implements IComparer

    Private col As Integer

    Public Sub New()
        col = 0
    End Sub

    Public Sub New(ByVal column As Integer)
        col = column
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
       Implements IComparer.Compare
        Dim item1 As ListViewItem
        Dim item2 As ListViewItem
        item1 = CType(x, ListViewItem)
        item2 = CType(y, ListViewItem)
        Return [String].Compare(item1.SubItems(col).Text, item2.SubItems(col).Text)
    End Function
End Class

