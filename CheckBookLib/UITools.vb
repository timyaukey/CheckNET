Option Strict On
Option Explicit On

Imports System.Windows.Forms

Public Class UITools

    Public Shared Function CreateListBoxItem(ByVal strName As String, ByVal intValue As Integer) As CBListBoxItem
        Return New CBListBoxItem(strName, intValue)
    End Function

    Public Shared Function GetItemString(ctl As ComboBox, intIndex As Integer) As String
        Return DirectCast(ctl.Items(intIndex), CBListBoxItem).strName
    End Function

    Public Shared Function GetItemData(ctl As ComboBox, intIndex As Integer) As Integer
        Return DirectCast(ctl.Items(intIndex), CBListBoxItem).intValue
    End Function

    Public Shared Function ListViewAdd(ByVal lvw As ListView) As ListViewItem
        Return lvw.Items.Add("")
    End Function

    Public Shared Sub AddListSubItem(ByVal objItem As ListViewItem, ByVal intColIndex As Integer, ByVal strValue As String)
        If objItem.SubItems.Count > intColIndex Then
            objItem.SubItems(intColIndex).Text = strValue
        Else
            objItem.SubItems.Insert(intColIndex, New ListViewItem.ListViewSubItem(Nothing, strValue))
        End If
    End Sub

    Public Shared Sub SetListViewSortColumn(ByVal lvw As ListView, ByVal intColumn As Short)
        If intColumn = 0 Then
            lvw.ListViewItemSorter = Nothing
        Else
            lvw.ListViewItemSorter = New ListViewSorter(intColumn)
        End If
    End Sub

End Class
