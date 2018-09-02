Option Strict On
Option Explicit On

Public Class Utilities
    'Lower bound of many arrays
    Public Const intLBOUND1 As Short = 1

    Public Const strDateWithTwoDigitYear As String = "MM/dd/yy"

    Public Shared Function Split(ByVal strInput As String, ByVal strSeparator As String) As String()
        Dim sep(1) As String
        sep(0) = strSeparator
        Dim tmp() As String = strInput.Split(sep, StringSplitOptions.None)
        Return tmp
    End Function

    Public Shared Function blnIsValidDate(ByVal strDate As String) As Boolean
        If strDate Like "*#/*#/*##" Then
            If IsDate(strDate) Then
                blnIsValidDate = True
                Exit Function
            End If
        End If
        blnIsValidDate = False
    End Function

    Public Shared Function blnIsValidAmount(ByVal strAmount As String) As Boolean
        Dim intDotPos As Integer
        blnIsValidAmount = False
        If Not IsNumeric(strAmount) Then
            Exit Function
        End If
        intDotPos = InStr(strAmount, ".")
        If intDotPos > 0 Then
            If (Len(strAmount) - intDotPos) > 2 Then
                Exit Function
            End If
        End If
        blnIsValidAmount = True
    End Function

    Public Shared Function strFormatInteger(input As Long, style As String) As String
        Dim result As String = input.ToString(style)
        Return result
    End Function

    Public Shared Function strFormatCurrency(input As Decimal) As String
        Dim result As String = input.ToString("#######0.00") ' VB6.Format(input, "" + gstrFORMAT_CURRENCY)
        Return result
    End Function

    Public Shared Function strFormatDate(input As Date) As String
        Dim result As String = input.ToString("MM/dd/yy") ' VB6.Format(input, "" + gstrFORMAT_DATE)
        Return result
    End Function

    Public Shared Function strFormatDate(input As Date, style As String) As String
        Dim result As String = input.ToString(style)
        Return result
    End Function

    Public Shared Function objClipboardReader() As System.IO.TextReader
        Dim strData As String
        strData = Trim(My.Computer.Clipboard.GetText())
        objClipboardReader = New System.IO.StringReader(strData)
    End Function

    Public Shared Function objFirstElement(Of T)(enumerable As IEnumerable(Of T)) As T
        Using enumerator As IEnumerator(Of T) = enumerable.GetEnumerator()
            enumerator.MoveNext()
            Return enumerator.Current
        End Using
    End Function

    Public Shared Function objSecondElement(Of T)(enumerable As IEnumerable(Of T)) As T
        Using enumerator As IEnumerator(Of T) = enumerable.GetEnumerator()
            enumerator.MoveNext()
            enumerator.MoveNext()
            Return enumerator.Current
        End Using
    End Function

End Class
