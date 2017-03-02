Option Strict On
Option Explicit On

Public Class CBListBoxItem
    Public Property intValue As Integer
    Public Property strName As String

    Public Sub New(ByVal _strName As String, ByVal _intValue As Integer)
        strName = _strName
        intValue = _intValue
    End Sub

    Public Overrides Function ToString() As String
        Return strName
    End Function
End Class
