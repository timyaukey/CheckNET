Option Strict On
Option Explicit On

Imports System.Xml

Public Class XMLMisc

    'Description:
    '   Return a standard text description of an XML parse error.

    Public Shared Function GetParseErrorText(ByVal objParseError As CBXmlParseError) As String
        With objParseError
            GetParseErrorText = "Line=" & .Line & ", Message=" & .Message
        End With
    End Function

    'Description:
    '   Return the text of the named child element of the element passed in.

    Public Shared Function GetChildText(ByVal objParent As CBXmlElement, ByVal strChild As String) As String
        Dim objChild As CBXmlElement
        objChild = DirectCast(objParent.SelectSingleNode(strChild), CBXmlElement)
        If objChild Is Nothing Then
            GetChildText = Nothing
            Exit Function
        End If
        GetChildText = objChild.Text
    End Function

    Public Shared Function IsAttributeMissing(ByVal objValue As Object) As Boolean
        If objValue Is Nothing Then
            IsAttributeMissing = True
            Exit Function
        End If
        If TypeOf objValue Is String Then
            If DirectCast(objValue, String) = "" Then
                IsAttributeMissing = True
                Exit Function
            End If
        End If
        IsAttributeMissing = False
    End Function
End Class