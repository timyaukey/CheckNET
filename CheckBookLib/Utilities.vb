Option Strict On
Option Explicit On

Public Class Utilities
    'Lower bound of many arrays
    Public Const LowerBound1 As Short = 1

    Public Shared ReadOnly EmptyDate As DateTime = System.DateTime.FromOADate(0)

    Public Const DateFormatWithTwoDigitYear As String = "MM/dd/yy"

    Public Shared Function Split(ByVal strInput As String, ByVal strSeparator As String) As String()
        Dim sep(1) As String
        sep(0) = strSeparator
        Dim tmp() As String = strInput.Split(sep, StringSplitOptions.None)
        Return tmp
    End Function

    Public Shared Function IsValidDate(ByVal strDate As String) As Boolean
        If strDate Like "*#/*#/*##" Then
            If IsDate(strDate) Then
                IsValidDate = True
                Exit Function
            End If
        End If
        IsValidDate = False
    End Function

    Public Shared Function TryParseUniversalDate(ByVal strInput As String, ByRef datOutput As DateTime) As Boolean
        Dim intMonth As Integer
        Dim intDay As Integer
        Dim intYear As Integer
        Dim intIndex As Integer
        Dim intInputLength As Integer = strInput.Length
        Dim chr As Char

        Do
            If intIndex >= intInputLength Then
                Return False
            End If
            chr = strInput(intIndex)
            intIndex += 1
            If chr = "/"c Then
                If intMonth < 1 Or intMonth > 12 Then
                    Return False
                End If
                Exit Do
            ElseIf chr >= "0"c And chr <= "9"c Then
                intMonth = intMonth * 10 + (Strings.Asc(chr) - 48)
            Else
                Return False
            End If
        Loop

        Do
            If intIndex >= intInputLength Then
                Return False
            End If
            chr = strInput(intIndex)
            intIndex += 1
            If chr = "/"c Then
                If intDay < 1 Or intDay > 31 Then
                    Return False
                End If
                Exit Do
            ElseIf chr >= "0"c And chr <= "9"c Then
                intDay = intDay * 10 + (Strings.Asc(chr) - 48)
            Else
                Return False
            End If
        Loop

        Do
            If intIndex >= intInputLength Then
                If intYear > 2999 Then
                    Return False
                End If
                Exit Do
            End If
            chr = strInput(intIndex)
            intIndex += 1
            If chr >= "0"c And chr <= "9"c Then
                intYear = intYear * 10 + (Strings.Asc(chr) - 48)
            Else
                Return False
            End If
        Loop
        If intYear < 70 Then
            intYear += 2000
        ElseIf intYear < 100 Then
            intYear += 1900
        End If

        If intDay > DateTime.DaysInMonth(intYear, intMonth) Then
            Return False
        End If

        datOutput = New DateTime(intYear, intMonth, intDay)
        Return True
    End Function

    Public Shared Function IsValidAmount(ByVal strAmount As String) As Boolean
        Dim intDotPos As Integer
        IsValidAmount = False
        If Not IsNumeric(strAmount) Then
            Exit Function
        End If
        intDotPos = InStr(strAmount, ".")
        If intDotPos > 0 Then
            If (Len(strAmount) - intDotPos) > 2 Then
                Exit Function
            End If
        End If
        IsValidAmount = True
    End Function

    Public Shared Function FormatInteger(input As Long, style As String) As String
        Dim result As String = input.ToString(style)
        Return result
    End Function

    Public Shared Function FormatCurrency(input As Decimal) As String
        Dim result As String = input.ToString("#######0.00")
        Return result
    End Function

    Public Shared Function FormatDate(input As Date) As String
        Dim result As String = input.ToString("MM/dd/yy")
        Return result
    End Function

    Public Shared Function FormatDate(input As Date, style As String) As String
        Dim result As String = input.ToString(style)
        Return result
    End Function

    Public Shared Function GetClipboardReader() As System.IO.TextReader
        Dim strData As String
        strData = Trim(My.Computer.Clipboard.GetText())
        GetClipboardReader = New System.IO.StringReader(strData)
    End Function

    Public Shared Function GetFirstElement(Of T)(enumerable As IEnumerable(Of T)) As T
        Using enumerator As IEnumerator(Of T) = enumerable.GetEnumerator()
            enumerator.MoveNext()
            Return enumerator.Current
        End Using
    End Function

    Public Shared Function GetSecondElement(Of T)(enumerable As IEnumerable(Of T)) As T
        Using enumerator As IEnumerator(Of T) = enumerable.GetEnumerator()
            enumerator.MoveNext()
            enumerator.MoveNext()
            Return enumerator.Current
        End Using
    End Function

End Class
