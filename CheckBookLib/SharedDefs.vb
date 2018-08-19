Option Strict On
Option Explicit On

Imports System.IO

'Ways to narrow down Trx search results during import.
Public Enum ImportMatchNarrowMethod
    None = 1
    ClosestDate = 2
    EarliestDate = 3
End Enum

''' <summary>
''' Static methods not associated with WinForm user interface management.
''' </summary>

Public Module SharedDefs

    'Document types
    Public Const gintDOCTYP_INVOICE As Short = 1
    Public Const gintDOCTYP_PACKLIST As Short = 2
    Public Const gintDOCTYP_BILL As Short = 3
    Public Const gintDOCTYP_STATEMENT As Short = 4
    Public Const gintDOCTYP_CREDIT As Short = 5
    Public Const gintDOCTYP_CRMCONF As Short = 6
    Public Const gintDOCTYP_ORDCONF As Short = 7
    Public Const gintDOCTYP_LATENOTICE As Short = 8

    Public Const gstrFORMAT_CURRENCY As String = "#######0.00"
    Public Const gstrFORMAT_DATE As String = "mm/dd/yy"
    Public Const gstrFORMAT_DATE2 As String = "MM/dd/yy"
    Public Const gstrUNABLE_TO_TRANSLATE As String = "???"

    'Lower bound of many arrays
    Public Const gintLBOUND1 As Short = 1

    Public Function gaSplit(ByVal strInput As String, ByVal strSeparator As String) As String()
        Dim sep(1) As String
        sep(0) = strSeparator
        Dim tmp() As String = strInput.Split(sep, StringSplitOptions.None)
        Return tmp
    End Function

    Public Function gstrMakeRepeatId(ByVal strRepeatKey As String, ByVal intRepeatSeq As Integer) As String
        gstrMakeRepeatId = "#" & strRepeatKey & "." & intRepeatSeq
    End Function

    Public Function gstrTranslateCatKey(ByVal objCategories As CategoryTranslator, ByVal strKey As String) As String
        Dim strName As String
        Dim strRoot As String
        strName = objCategories.strKeyToValue1(strKey)
        If strName = "" Then
            strRoot = "TmpCat#" & strKey
            strName = "E:" & strRoot
            objCategories.Add(New StringTransElement(objCategories, strKey, strName, " " & strRoot))
            MsgBox("Error: Could not find code " & strKey & " in category " & "list. Have assigned it temporary category name " & strName & ", which " & "you will probably want to edit to make this category " & "permanent.", MsgBoxStyle.Information)
        End If
        gstrTranslateCatKey = strName
    End Function

    Public Function gstrTranslateBudgetKey(ByVal objCompany As Company, ByVal strKey As String) As String
        Dim strName As String
        gstrTranslateBudgetKey = ""
        If strKey <> "" Then
            strName = objCompany.objBudgets.strKeyToValue1(strKey)
            If strName = "" Then
                strName = "TmpBud#" & strKey
                objCompany.objBudgets.Add(New StringTransElement(objCompany.objBudgets, strKey, strName, strName))
                MsgBox("Error: Could not find code " & strKey & " in budget " & "list. Have assigned it temporary budget name " & strName & ", which " & "you will probably want to edit to make this budget " & "permanent.", MsgBoxStyle.Information)
            End If
            gstrTranslateBudgetKey = strName
        End If
    End Function

    '$Description Registry key name specific to a register.

    Public Function gstrRegkeyRegister(ByVal objReg As Register) As String
        gstrRegkeyRegister = "Registers\" & objReg.strTitle
    End Function

    Public Function gstrAmountToWords(ByVal curAmount As Decimal) As String
        '1 = one
        '19 = nineteen
        '20 = twenty
        '21 = twenty one
        '29 = twenty nine
        '100 = one hundred
        '101 = one hundred one
        '119 = one humdred nineteen
        '900 = nine hundred
        '901 = nine hundred one
        '920 = nine hundred twenty
        '1000 = one thousand
        '1200 = one thousand two hundred

        'Do millions (if > 0, 1 to 999 "million")
        'Do thousands (if >0, 1 to 999 "thousand")
        'Do remainder (0 to 999)

        Dim intMillions As Integer
        intMillions = CInt(Fix(curAmount / 1000000.0#))
        Dim intThousands As Integer
        intThousands = CInt(Fix((curAmount - intMillions * 1000000.0#) / 1000.0#))
        Dim intRemainder As Integer
        intRemainder = CInt(Fix(curAmount - intMillions * 1000000.0# - intThousands * 1000.0#))

        Dim strResult As String = ""
        If intRemainder > 0 Or (intMillions = 0 And intThousands = 0) Then
            strResult = strWordsLessThan1000(intRemainder)
        End If
        If intThousands > 0 Then
            strResult = strWordsLessThan1000(intThousands) & " thousand " & strResult
        End If
        If intMillions > 0 Then
            strResult = strWordsLessThan1000(intMillions) & " million " & strResult
        End If

        gstrAmountToWords = Trim(strResult)
    End Function

    Private Function strWordsLessThan1000(ByVal intNumber As Integer) As String
        Dim intHundredMult As Integer
        Dim intRemainder As Integer
        intHundredMult = CInt(Fix(intNumber / 100))
        intRemainder = intNumber Mod 100
        If intHundredMult > 0 Then
            If intRemainder = 0 Then
                strWordsLessThan1000 = strWordLessThan20(intHundredMult) & " hundred"
            Else
                strWordsLessThan1000 = strWordLessThan20(intHundredMult) & " hundred " & strWordLessThan100(intRemainder)
            End If
        Else
            strWordsLessThan1000 = strWordLessThan100(intNumber)
        End If
    End Function

    Private Function strWordLessThan100(ByVal intNumber As Integer) As String
        Dim intTenMult As Integer
        Dim intRemainder As Integer
        If intNumber < 20 Then
            strWordLessThan100 = strWordLessThan20(intNumber)
        Else
            intTenMult = CInt(Fix(intNumber / 10))
            intRemainder = intNumber Mod 10
            If intRemainder > 0 Then
                strWordLessThan100 = strWordMultipliedByTen(intTenMult) & " " & strWordLessThan20(intRemainder)
            Else
                strWordLessThan100 = strWordMultipliedByTen(intTenMult)
            End If
        End If
    End Function

    Private Function strWordLessThan20(ByVal intNumber As Integer) As String
        Select Case intNumber
            Case 0 : strWordLessThan20 = "zero"
            Case 1 : strWordLessThan20 = "one"
            Case 2 : strWordLessThan20 = "two"
            Case 3 : strWordLessThan20 = "three"
            Case 4 : strWordLessThan20 = "four"
            Case 5 : strWordLessThan20 = "five"
            Case 6 : strWordLessThan20 = "six"
            Case 7 : strWordLessThan20 = "seven"
            Case 8 : strWordLessThan20 = "eight"
            Case 9 : strWordLessThan20 = "nine"
            Case 10 : strWordLessThan20 = "ten"
            Case 11 : strWordLessThan20 = "eleven"
            Case 12 : strWordLessThan20 = "twelve"
            Case 13 : strWordLessThan20 = "thirteen"
            Case 14 : strWordLessThan20 = "fourteen"
            Case 15 : strWordLessThan20 = "fifteen"
            Case 16 : strWordLessThan20 = "sixteen"
            Case 17 : strWordLessThan20 = "seventeen"
            Case 18 : strWordLessThan20 = "eighteen"
            Case 19 : strWordLessThan20 = "nineteen"
            Case Else : strWordLessThan20 = ""
        End Select
    End Function

    Private Function strWordMultipliedByTen(ByVal intNumber As Integer) As String
        Select Case intNumber
            Case 2 : strWordMultipliedByTen = "twenty"
            Case 3 : strWordMultipliedByTen = "thirty"
            Case 4 : strWordMultipliedByTen = "forty"
            Case 5 : strWordMultipliedByTen = "fifty"
            Case 6 : strWordMultipliedByTen = "sixty"
            Case 7 : strWordMultipliedByTen = "seventy"
            Case 8 : strWordMultipliedByTen = "eighty"
            Case 9 : strWordMultipliedByTen = "ninety"
            Case Else : strWordMultipliedByTen = ""
        End Select
    End Function

    Public Function gblnValidDate(ByVal strDate As String) As Boolean
        If strDate Like "*#/*#/*##" Then
            If IsDate(strDate) Then
                gblnValidDate = True
                Exit Function
            End If
        End If
        gblnValidDate = False
    End Function

    Public Function gblnValidAmount(ByVal strAmount As String) As Boolean
        Dim intDotPos As Integer
        gblnValidAmount = False
        If Not IsNumeric(strAmount) Then
            Exit Function
        End If
        intDotPos = InStr(strAmount, ".")
        If intDotPos > 0 Then
            If (Len(strAmount) - intDotPos) > 2 Then
                Exit Function
            End If
        End If
        gblnValidAmount = True
    End Function

    Public Function gstrFormatInteger(input As Long, style As String) As String
        Dim result As String = input.ToString(style)
        Return result
    End Function

    Public Function gstrFormatCurrency(input As Decimal) As String
        Dim result As String = input.ToString("#######0.00") ' VB6.Format(input, "" + gstrFORMAT_CURRENCY)
        Return result
    End Function

    Public Function gstrFormatCurrency(input As Decimal, style As String) As String
        Dim result As String = input.ToString(style)
        Return result
    End Function

    Public Function gstrFormatDate(input As Date) As String
        Dim result As String = input.ToString("MM/dd/yy") ' VB6.Format(input, "" + gstrFORMAT_DATE)
        Return result
    End Function

    Public Function gstrFormatDate(input As Date, style As String) As String
        Dim result As String = input.ToString(style)
        Return result
    End Function

    Public Function gobjCreateListBoxItem(ByVal strName As String, ByVal intValue As Integer) As CBListBoxItem 'Object
        'Return New VB6.ListBoxItem(strName, intValue)
        Return New CBListBoxItem(strName, intValue)
    End Function

    Public Function gstrVB6GetItemString(ctl As System.Windows.Forms.ListBox, intIndex As Integer) As String
        'Return VB6.GetItemString(ctl, intIndex)
        Return DirectCast(ctl.Items(intIndex), CBListBoxItem).strName
    End Function

    Public Function gstrVB6GetItemString(ctl As System.Windows.Forms.ComboBox, intIndex As Integer) As String
        'Return VB6.GetItemString(ctl, intIndex)
        Return DirectCast(ctl.Items(intIndex), CBListBoxItem).strName
    End Function

    Public Sub gVB6SetItemData(ctl As System.Windows.Forms.ListBox, intIndex As Integer, intItemData As Integer)
        'VB6.SetItemData(ctl, intIndex, intItemData)
        Dim item As Object = ctl.Items(intIndex)
        DirectCast(item, CBListBoxItem).intValue = intItemData
    End Sub

    Public Sub gVB6SetItemData(ctl As System.Windows.Forms.ComboBox, intIndex As Integer, intItemData As Integer)
        'VB6.SetItemData(ctl, intIndex, intItemData)
        DirectCast(ctl.Items(intIndex), CBListBoxItem).intValue = intItemData
    End Sub

    Public Function gintVB6GetItemData(ctl As System.Windows.Forms.ListBox, intIndex As Integer) As Integer
        'Return VB6.GetItemData(ctl, intIndex)
        Return DirectCast(ctl.Items(intIndex), CBListBoxItem).intValue
    End Function

    Public Function gintVB6GetItemData(ctl As System.Windows.Forms.ComboBox, intIndex As Integer) As Integer
        'Return VB6.GetItemData(ctl, intIndex)
        Return DirectCast(ctl.Items(intIndex), CBListBoxItem).intValue
    End Function

    Public Function gobjClipboardReader() As TextReader
        Dim strData As String
        strData = Trim(My.Computer.Clipboard.GetText())
        gobjClipboardReader = New StringReader(strData)
    End Function

    Public Function gdatFirstElement(Of T)(enumerable As IEnumerable(Of T)) As T
        Using enumerator As IEnumerator(Of T) = enumerable.GetEnumerator()
            enumerator.MoveNext()
            Return enumerator.Current
        End Using
    End Function

    Public Function gdatSecondElement(Of T)(enumerable As IEnumerable(Of T)) As T
        Using enumerator As IEnumerator(Of T) = enumerable.GetEnumerator()
            enumerator.MoveNext()
            enumerator.MoveNext()
            Return enumerator.Current
        End Using
    End Function
End Module