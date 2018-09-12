Option Strict On
Option Explicit On

Public Class UITools

    Public Shared Function CreateListBoxItem(ByVal strName As String, ByVal intValue As Integer) As CBListBoxItem
        Return New CBListBoxItem(strName, intValue)
    End Function

    Public Shared Function GetItemString(ctl As System.Windows.Forms.ComboBox, intIndex As Integer) As String
        Return DirectCast(ctl.Items(intIndex), CBListBoxItem).strName
    End Function

    Public Shared Function GetItemData(ctl As System.Windows.Forms.ComboBox, intIndex As Integer) As Integer
        Return DirectCast(ctl.Items(intIndex), CBListBoxItem).intValue
    End Function

End Class
