Option Strict On
Option Explicit On

Imports System.Xml

Public Module XMLMisc

    'Description:
    '   Return a standard text description of an XML parse error.

    Public Function gstrXMLParseErrorText(ByVal objParseError As VB6XmlParseError) As String
        With objParseError
            gstrXMLParseErrorText = "Line=" & .line & ", Message=" & .Message
        End With
    End Function

    'Description:
    '   Return the text of the named child element of the element passed in.

    Public Function gstrGetXMLChildText(ByVal objParent As VB6XmlElement, ByVal strChild As String) As String
        Dim objChild As VB6XmlElement
        objChild = DirectCast(objParent.SelectSingleNode(strChild), VB6XmlElement)
        If objChild Is Nothing Then
            gstrGetXMLChildText = Nothing
            Exit Function
        End If
        gstrGetXMLChildText = objChild.Text
    End Function

    Public Function gblnXmlAttributeMissing(ByVal objValue As Object) As Boolean
        If objValue Is Nothing Then
            gblnXmlAttributeMissing = True
            Exit Function
        End If
        If TypeOf objValue Is String Then
            If DirectCast(objValue, String) = "" Then
                gblnXmlAttributeMissing = True
                Exit Function
            End If
        End If
        gblnXmlAttributeMissing = False
    End Function
End Module