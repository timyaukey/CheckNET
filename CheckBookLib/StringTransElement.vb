Option Strict On
Option Explicit On

Public Class StringTransElement
    Public Sub New(ByVal strKey_ As String, ByVal strValue1_ As String, ByVal strValue2_ As String)
        strKey = strKey_
        strValue1 = strValue1_
        strValue2 = strValue2_
    End Sub

    'Unique identifier in the list.
    'This is what is stored in the database.
    Public ReadOnly strKey As String
    'Unique name of list element.
    Public ReadOnly strValue1 As String
    'Alternate name of list element, not suitable for searching.
    Public ReadOnly strValue2 As String

    Public Overrides Function ToString() As String
        Return strKey + "|" + strValue1
    End Function
End Class
