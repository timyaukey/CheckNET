Option Strict On
Option Explicit On

''' <summary>
''' A LineStringTranslator that parses each line into the three parts of a StringTransElement.
''' The first character of each line is the separator character for that line,
''' and must appear two more times in the line. The substring between the first
''' and second occurences is the key string, between the second and third
''' occurences is the first value string, and after the third occurence is
''' the second value string.
''' </summary>

Public Class SimpleStringTranslator
    Inherits LineStringTranslator(Of StringTransElement)

    Protected Overrides Function ParseLine(strLine As String) As StringTransElement
        Dim intPos1 As Integer
        Dim intPos2 As Integer
        Dim strSeparator As String
        Dim strKey As String
        Dim strValue1 As String
        Dim strValue2 As String

        strSeparator = Left(strLine, 1)
        intPos1 = InStr(2, strLine, strSeparator)
        If intPos1 = 0 Then
            gRaiseError("Cannot find second separator " & strLine)
        End If
        intPos2 = InStr(intPos1 + 1, strLine, strSeparator)
        If intPos2 = 0 Then
            gRaiseError("Cannot find third separator " & strLine)
        End If

        strKey = Mid(strLine, 2, intPos1 - 2)
        strValue1 = Mid(strLine, intPos1 + 1, intPos2 - intPos1 - 1)
        strValue2 = Mid(strLine, intPos2 + 1)

        Return New StringTransElement(strKey, strValue1, strValue2)
    End Function
End Class
