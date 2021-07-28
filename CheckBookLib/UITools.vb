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

    Public Shared Sub LoadAccountListBox(ByVal lst As ListBox, ByVal objCompany As Company)
        Dim objAccount As Account

        With lst
            .Items.Clear()
            For Each objAccount In objCompany.Accounts
                .Items.Add(objAccount.Title)
            Next objAccount
        End With
    End Sub

    Public Shared Function objGetSelectedAccountAndUnload(ByVal lst As ListBox, ByVal frm As Form, ByVal objCompany As Company) As Account

        If lst.SelectedIndex = -1 Then
            objGetSelectedAccountAndUnload = Nothing
            Exit Function
        End If
        objGetSelectedAccountAndUnload = objCompany.Accounts.Item(lst.SelectedIndex)
        frm.Close()
        Application.DoEvents()
    End Function

    Public Shared Sub LoadComboFromStringTranslator(ByVal cbo As System.Windows.Forms.ComboBox, ByVal objList As IStringTranslator, ByVal blnAddEmpty As Boolean)

        Try

            Dim intIndex As Integer
            With cbo
                .Items.Clear()
                If blnAddEmpty Then
                    .Items.Add(UITools.CreateListBoxItem("", 0))
                End If
                For intIndex = 1 To objList.ElementCount
                    .Items.Add(UITools.CreateListBoxItem(objList.GetValue1(intIndex), intIndex))
                Next
            End With

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

End Class
