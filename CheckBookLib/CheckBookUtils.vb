Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Public Module CheckBookUtils
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Non-UI related stuff.

    'Global security context.
    Public gobjSecurity As Security

    'Collection of loaded Account objects.
    Public gcolAccounts As List(Of Account)

    'Global category and budget lists.
    Public gobjCategories As StringTranslator
    Public gobjBudgets As StringTranslator

    'Category keys of categories which typically have due dates
    '14 days or less after invoice or billing dates. Category
    'keys have "(" and ")" around them.
    Public gstrShortTermsCatKeys As String

    'Key of budget used as placeholder in fake trx.
    Public gstrPlaceholderBudgetKey As String

    'Temp hack
    Public gblnAssignRepeatSeq As Boolean

    Public Function gstrAccountPath() As String
        gstrAccountPath = gstrDataPath() & "\Accounts"
    End Function

    Public Function gstrReportPath() As String
        gstrReportPath = gstrDataPath() & "\Reports"
    End Function

    Public Function gstrBackupPath() As String
        gstrBackupPath = gstrDataPath() & "\Backup"
    End Function

    Public Function gstrImageFilePath() As String
        gstrImageFilePath = gstrDataPath() & "\ImageFiles"
    End Function

    Public Sub gLoadGlobalLists()
        Try

            gobjBudgets = New StringTranslator
            gobjBudgets.LoadFile(gstrAddPath("Shared.bud"))
            gobjCategories = New StringTranslator
            gobjCategories.LoadFile(gstrAddPath("Shared.cat"))
            gBuildShortTermsCatKeys()
            gFindPlaceholderBudget()

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    'Set gstrPlaceholderBudgetKey to the key of the budget whose name
    'is "(budget)", or set it to "---" if there is no such budget.
    Public Sub gFindPlaceholderBudget()
        Dim intPlaceholderIndex As Short
        intPlaceholderIndex = gobjBudgets.intLookupValue1("(placeholder)")
        If intPlaceholderIndex > 0 Then
            gstrPlaceholderBudgetKey = gobjBudgets.strKey(intPlaceholderIndex)
        Else
            'Don't use empty string, because that's the key used
            'if a split doesn't use a budget.
            gstrPlaceholderBudgetKey = "---"
        End If
    End Sub

    'Set gstrShortTermsCatKeys by the heuristic of looking for
    'recognizable strings in the category names.
    Public Sub gBuildShortTermsCatKeys()
        Dim intCatIndex As Short
        Dim strCatName As String
        Dim blnPossibleCredit As Boolean
        Dim blnPossibleUtility As Boolean

        gstrShortTermsCatKeys = ""
        For intCatIndex = 1 To gobjCategories.intElements
            strCatName = LCase(gobjCategories.strValue1(intCatIndex))

            blnPossibleUtility = blnHasWord(strCatName, "util") Or blnHasWord(strCatName, "phone") Or blnHasWord(strCatName, "trash") Or blnHasWord(strCatName, "garbage") Or blnHasWord(strCatName, "oil") Or blnHasWord(strCatName, "heat") Or blnHasWord(strCatName, "electric") Or blnHasWord(strCatName, "cable") Or blnHasWord(strCatName, "comcast") Or blnHasWord(strCatName, "web") Or blnHasWord(strCatName, "internet") Or blnHasWord(strCatName, "qwest") Or blnHasWord(strCatName, "verizon")

            blnPossibleCredit = blnHasWord(strCatName, "card") Or blnHasWord(strCatName, "bank") Or blnHasWord(strCatName, "loan") Or blnHasWord(strCatName, "auto") Or blnHasWord(strCatName, "car") Or blnHasWord(strCatName, "truck") Or blnHasWord(strCatName, "mortgage") Or blnHasWord(strCatName, "house")

            If blnPossibleCredit Or blnPossibleUtility Then
                gstrShortTermsCatKeys = gstrShortTermsCatKeys & gstrEncodeCatKey(gobjCategories.strKey(intCatIndex))
            End If
        Next

    End Sub

    Private Function blnHasWord(ByVal strCatName As String, ByVal strPrefix As String) As Boolean
        blnHasWord = (InStr(strCatName, ":" & strPrefix) > 0) Or (InStr(strCatName, " " & strPrefix) > 0)
    End Function

    Public Function gstrEncodeCatKey(ByVal strCatKey As String) As String
        gstrEncodeCatKey = "(" & strCatKey & ")"
    End Function

    Public Function gstrMakeRepeatId(ByVal strRepeatKey As String, ByVal intRepeatSeq As Short) As String
        gstrMakeRepeatId = "#" & strRepeatKey & "." & intRepeatSeq
    End Function

    Public Function gstrMakeAgingBracket(ByVal datAgingDate As Date, ByVal intBracketSize As Short, ByVal blnFake As Boolean, ByVal datTrxDate As Date, ByVal datInvDate As Date, ByVal datDueDate As Date) As String
        Dim strBracket As String

        strBracket = gstrAgingBracketNotInvoiced()
        'If item was invoiced as of report date.
        Dim intAgeInDays As Short
        Dim intAgeBracket As Short
        Dim intStartingAge As Short
        Dim intEndingAge As Short
        If datInvDate <= datAgingDate Then
            'If item was not paid as of the report date.
            If blnFake Or (datTrxDate > datAgingDate) Then
                'UPGRADE_WARNING: DateDiff behavior may be different. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B38EC3F-686D-4B2E-B5A5-9E8E7A762E32"'
                intAgeInDays = DateDiff(Microsoft.VisualBasic.DateInterval.Day, datDueDate, datAgingDate)
                'If intBracketSize = 30:
                '1 to 30 = bracket 0, 31 to 60 = bracket 1, etc.
                '-29 to 0 = bracket -1, -59 to -30 = bracket -2, etc.
                'int() always rounds down, so int(-5/30)=-1 and not 0.
                intAgeBracket = Int((intAgeInDays - 1) / intBracketSize)
                intStartingAge = 1 + (intAgeBracket * intBracketSize)
                intEndingAge = intStartingAge + intBracketSize - 1
                If intAgeBracket = -1 Then
                    strBracket = gstrAgingBracketCurrent()
                ElseIf intAgeBracket >= 0 Then
                    strBracket = gstrAgingBracketPastDue(intStartingAge, intEndingAge)
                Else
                    strBracket = gstrAgingBracketFuture(intStartingAge, intEndingAge)
                End If
            Else
                strBracket = gstrAgingBracketPaid()
            End If
        End If
        gstrMakeAgingBracket = strBracket

    End Function

    Public Function gstrMakeDateBracket(ByVal datInputDate As Date, ByVal intBracketSize As Short, ByVal datBaseDate As Date) As String

        Dim intOffsetDays As Short
        Dim intMonthPart As Short
        Dim datBracketDate As Date

        gstrMakeDateBracket = ""
        If intBracketSize < 0 Then
            If intBracketSize = -1 Then
                gstrMakeDateBracket = gstrFormatDate(datInputDate, "yyyy/MM/01")
            ElseIf intBracketSize = -2 Then
                intMonthPart = Int((VB.Day(datInputDate) - 1) / 15)
                If intMonthPart > 1 Then
                    intMonthPart = 1
                End If
                gstrMakeDateBracket = gstrFormatDate(datInputDate, "yyyy/MM/") & gstrFormatInteger(1 + intMonthPart * 15, "0#")
            ElseIf intBracketSize = -4 Then
                intMonthPart = Int((VB.Day(datInputDate) - 1) / 8)
                gstrMakeDateBracket = gstrFormatDate(datInputDate, "yyyy/MM/") & gstrFormatInteger(1 + intMonthPart * 8, "0#")
            End If
        Else
            'UPGRADE_WARNING: DateDiff behavior may be different. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B38EC3F-686D-4B2E-B5A5-9E8E7A762E32"'
            intOffsetDays = DateDiff(Microsoft.VisualBasic.DateInterval.Day, datBaseDate, datInputDate)
            datBracketDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, Int(intOffsetDays / intBracketSize) * intBracketSize, datBaseDate)
            gstrMakeDateBracket = gstrFormatDate(datBracketDate, "yyyy/MM/dd")
        End If

    End Function

    Public Function gstrAgingBracketNotInvoiced() As String
        gstrAgingBracketNotInvoiced = "Not Invoiced"
    End Function

    Public Function gstrAgingBracketCurrent() As String
        gstrAgingBracketCurrent = "Current"
    End Function

    Public Function gstrAgingBracketPaid() As String
        gstrAgingBracketPaid = "Paid"
    End Function

    Public Function gstrAgingBracketFuture(ByVal intStartingAge As Short, ByVal intEndingAge As Short) As String
        gstrAgingBracketFuture = "Due In " & gstrFormatInteger(-intEndingAge, "000") & "-" & gstrFormatInteger(-intStartingAge, "000") & " Days"
    End Function

    Public Function gstrAgingBracketPastDue(ByVal intStartingAge As Short, ByVal intEndingAge As Short) As String
        gstrAgingBracketPastDue = "Due " & gstrFormatInteger(intStartingAge, "000") & "-" & gstrFormatInteger(intEndingAge, "000") & " Days Ago"
    End Function
End Module