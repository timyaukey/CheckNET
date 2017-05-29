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
    Public strValue1 As String
    'Alternate name of list element, not suitable for searching.
    Public strValue2 As String
    'Additional values to associate with element.
    Public colValues As Dictionary(Of String, String) = New Dictionary(Of String, String)()

    Public Function objClone() As StringTransElement
        Dim objNew = New StringTransElement(strKey, strValue1, strValue2)
        objNew.colValues = New Dictionary(Of String, String)(colValues)
        Return objNew
    End Function

    Public Overrides Function ToString() As String
        Return strValue1
    End Function
End Class
