Option Strict On
Option Explicit On

Public Class CBListBoxItem
    Public Property LBValue As Integer
    Public Property LBName As String

    Public Sub New(ByVal _strName As String, ByVal _intValue As Integer)
        LBName = _strName
        LBValue = _intValue
    End Sub

    Public Overrides Function ToString() As String
        Return LBName
    End Function
End Class
