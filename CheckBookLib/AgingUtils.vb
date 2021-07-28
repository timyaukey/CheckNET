Option Strict On
Option Explicit On

Public Class AgingUtils

    Public Shared Function MakeAgeBracket(ByVal datAgingDate As Date, ByVal intBracketSize As Short, ByVal blnFake As Boolean, ByVal datTrxDate As Date, ByVal datInvDate As Date, ByVal datDueDate As Date) As String
        Dim strBracket As String

        strBracket = NotInvoicedLabel()
        'If item was invoiced as of report date.
        Dim intAgeInDays As Long
        Dim intAgeBracket As Long
        Dim intStartingAge As Long
        Dim intEndingAge As Long
        If datInvDate <= datAgingDate Then
            'If item was not paid as of the report date.
            If blnFake Or (datTrxDate > datAgingDate) Then
                intAgeInDays = DateDiff(DateInterval.Day, datDueDate, datAgingDate)
                'If intBracketSize = 30:
                '1 to 30 = bracket 0, 31 to 60 = bracket 1, etc.
                '-29 to 0 = bracket -1, -59 to -30 = bracket -2, etc.
                'int() always rounds down, so int(-5/30)=-1 and not 0.
                intAgeBracket = CLng(Int((intAgeInDays - 1) / intBracketSize))
                intStartingAge = 1 + (intAgeBracket * intBracketSize)
                intEndingAge = intStartingAge + intBracketSize - 1
                If intAgeBracket = -1 Then
                    strBracket = CurrentLabel()
                ElseIf intAgeBracket >= 0 Then
                    strBracket = PastDueLabel(intStartingAge, intEndingAge)
                Else
                    strBracket = FutureLabel(intStartingAge, intEndingAge)
                End If
            Else
                strBracket = PaidLabel()
            End If
        End If
        MakeAgeBracket = strBracket

    End Function

    Public Shared Function MakeDateBracket(ByVal datInputDate As Date, ByVal intBracketSize As Short, ByVal datBaseDate As Date) As String

        Dim intOffsetDays As Long
        Dim intMonthPart As Integer
        Dim datBracketDate As Date

        MakeDateBracket = ""
        If intBracketSize < 0 Then
            If intBracketSize = -1 Then
                MakeDateBracket = Utilities.strFormatDate(datInputDate, "yyyy/MM/01")
            ElseIf intBracketSize = -2 Then
                intMonthPart = CInt(Int((Microsoft.VisualBasic.Day(datInputDate) - 1) / 15))
                If intMonthPart > 1 Then
                    intMonthPart = 1
                End If
                MakeDateBracket = Utilities.strFormatDate(datInputDate, "yyyy/MM/") & Utilities.strFormatInteger(1 + intMonthPart * 15, "0#")
            ElseIf intBracketSize = -4 Then
                intMonthPart = CInt(Int((Microsoft.VisualBasic.Day(datInputDate) - 1) / 8))
                MakeDateBracket = Utilities.strFormatDate(datInputDate, "yyyy/MM/") & Utilities.strFormatInteger(1 + intMonthPart * 8, "0#")
            End If
        Else
            intOffsetDays = DateDiff(Microsoft.VisualBasic.DateInterval.Day, datBaseDate, datInputDate)
            datBracketDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, Int(intOffsetDays / intBracketSize) * intBracketSize, datBaseDate)
            MakeDateBracket = Utilities.strFormatDate(datBracketDate, "yyyy/MM/dd")
        End If

    End Function

    Public Shared Function NotInvoicedLabel() As String
        Return "Not Invoiced"
    End Function

    Public Shared Function CurrentLabel() As String
        Return "Current"
    End Function

    Public Shared Function PaidLabel() As String
        Return "Paid"
    End Function

    Public Shared Function FutureLabel(ByVal intStartingAge As Long, ByVal intEndingAge As Long) As String
        Return "Due In " & Utilities.strFormatInteger(-intEndingAge, "000") & "-" & Utilities.strFormatInteger(-intStartingAge, "000") & " Days"
    End Function

    Public Shared Function PastDueLabel(ByVal intStartingAge As Long, ByVal intEndingAge As Long) As String
        Return "Due " & Utilities.strFormatInteger(intStartingAge, "000") & "-" & Utilities.strFormatInteger(intEndingAge, "000") & " Days Ago"
    End Function
End Class