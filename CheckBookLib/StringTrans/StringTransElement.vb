Option Strict On
Option Explicit On

Public Class StringTransElement
    Public Sub New(ByVal objTrans_ As IStringTranslator, ByVal strKey_ As String, ByVal strValue1_ As String, ByVal strValue2_ As String)
        Translator = objTrans_
        If Translator Is Nothing Then
            Throw New NullReferenceException("Null IStringTranslator passed to StringTransElement()")
        End If
        Key = strKey_
        Value1 = strValue1_
        Value2 = strValue2_
    End Sub

    Public ReadOnly Translator As IStringTranslator
    'Unique identifier in the list.
    'This is what is stored in the database.
    Public ReadOnly Key As String
    'Unique name of list element.
    Public Value1 As String
    'Alternate name of list element, not suitable for searching.
    Public Value2 As String
    'Additional values to associate with element.
    Public ExtraValues As Dictionary(Of String, String) = New Dictionary(Of String, String)()

    Public Function CloneElement() As StringTransElement
        Dim objNew = New StringTransElement(Translator, Key, Value1, Value2)
        objNew.ExtraValues = New Dictionary(Of String, String)(ExtraValues)
        Return objNew
    End Function

    Public Overrides Function ToString() As String
        Return Translator.FormatElement(Me)
    End Function
End Class
