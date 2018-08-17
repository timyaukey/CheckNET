Option Strict On
Option Explicit On

Public Class StringTransElement
    Public Sub New(ByVal objTrans_ As IStringTranslator, ByVal strKey_ As String, ByVal strValue1_ As String, ByVal strValue2_ As String)
        objTrans = objTrans_
        If objTrans Is Nothing Then
            Throw New NullReferenceException("Null IStringTranslator passed to StringTransElement()")
        End If
        strKey = strKey_
        strValue1 = strValue1_
        strValue2 = strValue2_
    End Sub

    Public ReadOnly objTrans As IStringTranslator
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
        Dim objNew = New StringTransElement(objTrans, strKey, strValue1, strValue2)
        objNew.colValues = New Dictionary(Of String, String)(colValues)
        Return objNew
    End Function

    Public Overrides Function ToString() As String
        Return objTrans.strFormatElement(Me)
    End Function
End Class
