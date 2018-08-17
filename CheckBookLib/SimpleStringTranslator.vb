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
        Dim intPos3 As Integer
        Dim strSeparator As String
        Dim strKey As String
        Dim strValue1 As String
        Dim strValue2 As String
        Dim strValue3 As String
        Dim objNew As StringTransElement

        'Formats:
        '/key/value1/value2
        '/key/value1/value2/xk1:xv1/xk2:xv2/...

        strSeparator = Left(strLine, 1)
        intPos1 = InStr(2, strLine, strSeparator)
        If intPos1 = 0 Then
            gRaiseError("Cannot find second separator " & strLine)
        End If
        intPos2 = InStr(intPos1 + 1, strLine, strSeparator)
        If intPos2 = 0 Then
            gRaiseError("Cannot find third separator " & strLine)
        End If
        intPos3 = InStr(intPos2 + 1, strLine, strSeparator)
        If intPos3 = 0 Then
            intPos3 = Len(strLine) + 1
        End If

        strKey = Mid(strLine, 2, intPos1 - 2)
        strValue1 = Mid(strLine, intPos1 + 1, intPos2 - intPos1 - 1)
        strValue2 = Mid(strLine, intPos2 + 1, intPos3 - intPos2 - 1)
        strValue3 = Mid(strLine, intPos3 + 1)

        objNew = New StringTransElement(Me, strKey, strValue1, strValue2)

        If strValue3 <> "" Then
            Dim strExtraPairs() As String = strValue3.Split(strSeparator(0))
            For Each strExtraPair As String In strExtraPairs
                Dim strPairParts() As String = strExtraPair.Split(":"c)
                If UBound(strPairParts) = 1 Then
                    objNew.colValues.Add(strPairParts(0), strPairParts(1))
                End If
            Next
        End If

        Return objNew
    End Function
End Class
