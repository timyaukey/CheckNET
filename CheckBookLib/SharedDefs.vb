Option Strict Off
Option Explicit On

Imports System.IO
Imports VB = Microsoft.VisualBasic

'Ways to narrow down Trx search results during import.
Public Enum ImportMatchNarrowMethod
    None = 1
    ClosestDate = 2
    EarliestDate = 3
End Enum

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

    Public gstrCmdLinArgs() As String
    Public gstrDataPathValue As String

    'Public Declare Function CopyFile Lib "kernel32" Alias _
    ''    "CopyFileA" (ByVal lpExistingFileName As String, _
    ''    ByVal lpNewFileName As String, ByVal bFailIfExists As Long) As Long

    'Table with memorized payees.
    Public gdomTransTable As VB6XmlDocument
    'Above with Output attributes of Payee elements converted to upper case.
    Public gdomTransTableUCS As VB6XmlDocument

    'Public Declare Function FormatMessage Lib "kernel32" Alias _
    ''    "FormatMessageA" (ByVal dwFlags As Long, lpSource As Any, _
    ''    ByVal dwMessageId As Long, ByVal dwLanguageId As Long, _
    ''    ByVal lpBuffer As String, ByVal nSize As Long, Arguments As Long) As Long

    'Public Const FORMAT_MESSAGE_IGNORE_INSERTS = &H200
    'Public Const FORMAT_MESSAGE_FROM_SYSTEM = &H1000

    'Public Function gstrWin32ErrorText(ByVal lngDLLError As Long) As String
    '
    '    Dim lngResult As Long
    '    Dim strError As String
    '
    '    strError = Space$(200)
    '    lngResult = FormatMessage(FORMAT_MESSAGE_IGNORE_INSERTS Or FORMAT_MESSAGE_FROM_SYSTEM, _
    ''        0, lngDLLError, 0, strError, Len(strError), 0)
    '    If lngResult > 0 Then
    '        If Mid$(strError, lngResult - 1, 2) = vbCrLf Then
    '            strError = Left$(strError, lngResult - 2)
    '        Else
    '            strError = Left$(strError, lngResult)
    '        End If
    '    Else
    '        strError = "Could not translate Win32 error code " & lngDLLError & " decimal"
    '    End If
    '
    '    gstrWin32ErrorText = strError
    '
    'End Function

    Public Function gobjInitialize() As Everything
        Dim intIndex As Short
        Dim strArg As String
        Dim strPath As String
        Dim objEverything As Everything

        objEverything = New Everything
        objEverything.Init()
        gstrCmdLinArgs = gaSplit(VB.Command(), " ")
        gstrDataPathValue = My.Application.Info.DirectoryPath & "\Data"
        For intIndex = LBound(gstrCmdLinArgs) To UBound(gstrCmdLinArgs)
            strArg = gstrCmdLinArgs(intIndex)
            'All args recognized should have an "//" prefix, to keep them in a separate
            'namespace from args recognized by individual programs.
            '//r:(path) gives the path to the root data folder. It is relative to
            'App.Path unless starts with a device letter or UNC path prefix.
            If Left(strArg, 4) = "//r:" Then
                strPath = Mid(strArg, 5)
                If (Left(strPath, 2) = "\\") Or (Mid(strPath, 2, 1) = ":") Then
                    gstrDataPathValue = strPath
                Else
                    gstrDataPathValue = My.Application.Info.DirectoryPath & "\" & strPath
                End If
                gstrCmdLinArgs(intIndex) = ""
            End If
        Next
        gobjInitialize = objEverything
    End Function

    Public Function gblnUnrecognizedArgs() As Boolean
        Dim intIndex As Short
        Dim strArg As String

        gblnUnrecognizedArgs = True
        For intIndex = LBound(gstrCmdLinArgs) To UBound(gstrCmdLinArgs)
            strArg = gstrCmdLinArgs(intIndex)
            If Trim(strArg) <> "" Then
                MsgBox("Unrecognized command line argument: " & strArg)
                Exit Function
            End If
        Next
        gblnUnrecognizedArgs = False
    End Function

    Public Function gaSplit(ByVal strInput As String, ByVal strSeparator As String) As String()
        'gaSplit = Split(strInput, strSeparator, -1, vbBinaryCompare)
        Dim lngStartIndex As Integer
        Dim lngNextSepIndex As Integer
        Dim intSepLen As Short
        Dim colResults As Collection
        Dim astrResults() As String
        Dim vstrPart As Object
        Dim lngOutIndex As Integer

        colResults = New Collection
        intSepLen = Len(strSeparator)
        lngStartIndex = 1
        Do
            lngNextSepIndex = InStr(lngStartIndex, strInput, strSeparator)
            If lngNextSepIndex <= 0 Then
                colResults.Add(Mid(strInput, lngStartIndex))
                Exit Do
            End If
            colResults.Add(Mid(strInput, lngStartIndex, lngNextSepIndex - lngStartIndex))
            lngStartIndex = lngNextSepIndex + intSepLen
        Loop
        ReDim astrResults(colResults.Count() - 1)
        lngOutIndex = 0
        For Each vstrPart In colResults
            'UPGRADE_WARNING: Couldn't resolve default property of object vstrPart. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            astrResults(lngOutIndex) = vstrPart
            lngOutIndex = lngOutIndex + 1
        Next vstrPart
        'gaSplit = VB6.CopyArray(astrResults)
        gaSplit = astrResults.Clone()
    End Function

    Public Function gstrDataPath() As String
        gstrDataPath = gstrDataPathValue
    End Function

    Public Function gstrAddPath(ByVal strBareName As String) As String
        gstrAddPath = gstrDataPath() & "\" & strBareName
    End Function

    '$Description Path and name of payee file.

    Public Function gstrPayeeFilePath() As String
        gstrPayeeFilePath = gstrAddPath("PayeeList.xml")
    End Function

    '$Description Return a string summarizing the categories used by a normal transaction.

    Public Function gstrSummarizeTrxCat(ByVal objTrx As Trx) As String

        Dim objSplit As Split_Renamed
        Dim strCategoryKey As String = ""

        For Each objSplit In objTrx.colSplits
            If strCategoryKey = "" Then
                strCategoryKey = objSplit.strCategoryKey
            ElseIf strCategoryKey <> objSplit.strCategoryKey Then
                gstrSummarizeTrxCat = "(mixed)"
                Exit Function
            End If
        Next objSplit
        gstrSummarizeTrxCat = gstrTranslateCatKey(strCategoryKey)

    End Function

    '$Description Return a string summarizing the budgets used by a normal transaction.

    Public Function gstrSummarizeTrxBudget(ByVal objTrx As Trx) As String

        Dim objSplit As Split_Renamed
        Dim strBudgetKey As String = ""

        For Each objSplit In objTrx.colSplits
            If strBudgetKey = "" Then
                strBudgetKey = objSplit.strBudgetKey
            ElseIf strBudgetKey <> objSplit.strBudgetKey Then
                gstrSummarizeTrxBudget = "(mixed)"
                Exit Function
            End If
        Next objSplit
        gstrSummarizeTrxBudget = gstrTranslateBudgetKey(strBudgetKey)

    End Function

    '$Description Return a string summarizing the split due dates used by a normal transaction.

    Public Function gstrSummarizeTrxDueDate(ByVal objTrx As Trx) As String

        Dim objSplit As Split_Renamed
        Dim datDueDate As Date = System.DateTime.FromOADate(0)

        For Each objSplit In objTrx.colSplits
            If datDueDate = System.DateTime.FromOADate(0) Then
                datDueDate = objSplit.datDueDate
            ElseIf datDueDate <> objSplit.datDueDate And objSplit.datDueDate <> System.DateTime.FromOADate(0) Then
                gstrSummarizeTrxDueDate = "(mixed)"
                Exit Function
            End If
        Next objSplit
        If datDueDate = System.DateTime.FromOADate(0) Then
            gstrSummarizeTrxDueDate = ""
        Else
            gstrSummarizeTrxDueDate = datDueDate.ToString(gstrFORMAT_DATE2)
        End If

    End Function

    '$Description Return a string summarizing the split invoice dates used by a normal transaction.

    Public Function gstrSummarizeTrxInvoiceDate(ByVal objTrx As Trx) As String

        Dim objSplit As Split_Renamed
        Dim datInvoiceDate As Date = System.DateTime.FromOADate(0)

        For Each objSplit In objTrx.colSplits
            If datInvoiceDate = System.DateTime.FromOADate(0) Then
                datInvoiceDate = objSplit.datInvoiceDate
            ElseIf datInvoiceDate <> objSplit.datInvoiceDate And objSplit.datInvoiceDate <> System.DateTime.FromOADate(0) Then
                gstrSummarizeTrxInvoiceDate = "(mixed)"
                Exit Function
            End If
        Next objSplit
        If datInvoiceDate = System.DateTime.FromOADate(0) Then
            gstrSummarizeTrxInvoiceDate = ""
        Else
            gstrSummarizeTrxInvoiceDate = datInvoiceDate.ToString(gstrFORMAT_DATE2)
        End If

    End Function

    '$Description Return a string summarizing the PO numbers used by a normal transaction.

    Public Function gstrSummarizeTrxPONumber(ByVal objTrx As Trx) As String

        Dim objSplit As Split_Renamed
        Dim strPONumber As String = ""

        For Each objSplit In objTrx.colSplits
            If strPONumber = "" Then
                strPONumber = objSplit.strPONumber
            ElseIf strPONumber <> objSplit.strPONumber And objSplit.strPONumber <> "" Then
                gstrSummarizeTrxPONumber = "(mixed)"
                Exit Function
            End If
        Next objSplit
        gstrSummarizeTrxPONumber = strPONumber

    End Function

    '$Description Return a string summarizing the terms used by a normal transaction.

    Public Function gstrSummarizeTrxTerms(ByVal objTrx As Trx) As String

        Dim objSplit As Split_Renamed
        Dim strTerms As String = ""

        For Each objSplit In objTrx.colSplits
            If strTerms = "" Then
                strTerms = objSplit.strTerms
            ElseIf strTerms <> objSplit.strTerms And objSplit.strTerms <> "" Then
                gstrSummarizeTrxTerms = "(mixed)"
                Exit Function
            End If
        Next objSplit
        gstrSummarizeTrxTerms = strTerms

    End Function

    '$Description Return a string summarizing the invoice numbers used by a normal transaction.

    Public Function gstrSummarizeTrxInvoiceNum(ByVal objTrx As Trx) As String

        Dim objSplit As Split_Renamed
        Dim strInvNumber As String = ""

        For Each objSplit In objTrx.colSplits
            If strInvNumber = "" Then
                strInvNumber = objSplit.strInvoiceNum
            ElseIf strInvNumber <> objSplit.strInvoiceNum And objSplit.strInvoiceNum <> "" Then
                gstrSummarizeTrxInvoiceNum = "(mixed)"
                Exit Function
            End If
        Next objSplit
        gstrSummarizeTrxInvoiceNum = strInvNumber

    End Function

    Public Function gstrTranslateCatKey(ByVal strKey As String) As String
        Dim strName As String
        Dim strRoot As String
        strName = gobjCategories.strKeyToValue1(strKey)
        If strName = "" Then
            strRoot = "TmpCat#" & strKey
            strName = "E:" & strRoot
            gobjCategories.Add(strKey, strName, " " & strRoot)
            MsgBox("Error: Could not find code " & strKey & " in category " & "list. Have assigned it temporary category name " & strName & ", which " & "you will probably want to edit to make this category " & "permanent.", MsgBoxStyle.Information)
        End If
        gstrTranslateCatKey = strName
    End Function

    '$Description Like gstrSummarizeTrxCat(), but summarizes multiple split properties.

    Public Sub gSummarizeSplits(ByVal objTrx As Trx, ByRef strCategory As String, ByRef strPONumber As String, ByRef strInvoiceNum As String, ByRef strInvoiceDate As String, ByRef strDueDate As String, ByRef strTerms As String, ByRef strBudget As String, ByRef curAvailable As Decimal)

        Dim objSplit As Split_Renamed
        Dim strCatKey As String = ""
        Dim strPONumber2 As String = ""
        Dim strInvoiceNum2 As String = ""
        Dim datInvoiceDate As Date
        Dim datDueDate As Date
        Dim strTerms2 As String = ""
        Dim strBudgetKey As String = ""
        Dim blnFirstSplit As Boolean

        blnFirstSplit = True
        curAvailable = 0
        For Each objSplit In objTrx.colSplits
            If objSplit.strBudgetKey = gstrPlaceholderBudgetKey Then
                curAvailable = curAvailable + objSplit.curAmount
            End If
            If blnFirstSplit Then
                'Remember fields from the first split.
                strCatKey = objSplit.strCategoryKey
                strPONumber2 = objSplit.strPONumber
                strInvoiceNum2 = objSplit.strInvoiceNum
                datInvoiceDate = objSplit.datInvoiceDate
                datDueDate = objSplit.datDueDate
                strTerms2 = objSplit.strTerms
                strBudgetKey = objSplit.strBudgetKey
                'Format fields from the first split.
                strCategory = gstrTranslateCatKey(strCatKey)
                strInvoiceNum = strInvoiceNum2
                strPONumber = strPONumber2
                If datInvoiceDate = System.DateTime.FromOADate(0) Then
                    strInvoiceDate = ""
                Else
                    strInvoiceDate = gstrFormatDate(datInvoiceDate)
                End If
                If datDueDate = System.DateTime.FromOADate(0) Then
                    strDueDate = ""
                Else
                    strDueDate = gstrFormatDate(datDueDate)
                End If
                strTerms = strTerms2
                strBudget = gstrTranslateBudgetKey(strBudgetKey)
                blnFirstSplit = False
            Else
                If strCatKey <> objSplit.strCategoryKey Then
                    strCategory = "(mixed)"
                End If
                If strPONumber2 <> objSplit.strPONumber Then
                    strPONumber = "(mixed)"
                End If
                If strInvoiceNum2 <> objSplit.strInvoiceNum Then
                    strInvoiceNum = "(mixed)"
                End If
                If datInvoiceDate <> objSplit.datInvoiceDate Then
                    strInvoiceDate = "(mixed)"
                End If
                If datDueDate <> objSplit.datDueDate Then
                    strDueDate = "(mixed)"
                End If
                If strTerms2 <> objSplit.strTerms Then
                    strTerms = "(mixed)"
                End If
                If strBudgetKey <> objSplit.strBudgetKey Then
                    strBudget = "(mixed)"
                End If
            End If
        Next objSplit

    End Sub

    Public Function gstrTranslateBudgetKey(ByVal strKey As String) As String
        Dim strName As String
        gstrTranslateBudgetKey = ""
        If strKey <> "" Then
            strName = gobjBudgets.strKeyToValue1(strKey)
            If strName = "" Then
                strName = "TmpBud#" & strKey
                gobjBudgets.Add(strKey, strName, strName)
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

        Dim intMillions As Short
        intMillions = Fix(curAmount / 1000000.0#)
        Dim intThousands As Short
        intThousands = Fix((curAmount - intMillions * 1000000.0#) / 1000.0#)
        Dim intRemainder As Short
        intRemainder = Fix(curAmount - intMillions * 1000000.0# - intThousands * 1000.0#)

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

    Private Function strWordsLessThan1000(ByVal intNumber As Short) As String
        Dim intHundredMult As Short
        Dim intRemainder As Short
        intHundredMult = Fix(intNumber / 100)
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

    Private Function strWordLessThan100(ByVal intNumber As Short) As String
        Dim intTenMult As Short
        Dim intRemainder As Short
        If intNumber < 20 Then
            strWordLessThan100 = strWordLessThan20(intNumber)
        Else
            intTenMult = Fix(intNumber / 10)
            intRemainder = intNumber Mod 10
            If intRemainder > 0 Then
                strWordLessThan100 = strWordMultipliedByTen(intTenMult) & " " & strWordLessThan20(intRemainder)
            Else
                strWordLessThan100 = strWordMultipliedByTen(intTenMult)
            End If
        End If
    End Function

    Private Function strWordLessThan20(ByVal intNumber As Short) As String
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

    Private Function strWordMultipliedByTen(ByVal intNumber As Short) As String
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

    '$Description Load list of memorized payees and import translation instructions.

    Public Sub gLoadTransTable()
        Try

            Dim strTableFile As String

            strTableFile = gstrPayeeFilePath()
            gdomTransTable = gdomLoadFile(strTableFile)
            gCreateTransTableUCS()

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Deep clone gdomTransTable into gdomTransTableUCS, adding
    '   an OutputUCS attribute for each Payee element with an Output attribute.
    '   This routine must be called whenever payee information changes. The
    '   resulting DOM is temporary, and never saved to an XML file.

    Public Sub gCreateTransTableUCS()
        Dim colPayees As VB6XmlNodeList
        Dim elmPayee As VB6XmlElement
        Dim vntOutput As Object

        Try

            gdomTransTableUCS = gdomTransTable.CloneNode(True)
            colPayees = gdomTransTableUCS.DocumentElement.SelectNodes("Payee")
            For Each elmPayee In colPayees
                'UPGRADE_WARNING: Couldn't resolve default property of object elmPayee.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vntOutput. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                vntOutput = elmPayee.GetAttribute("Output")
                'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
                If Not gblnXmlAttributeMissing(vntOutput) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vntOutput. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    elmPayee.SetAttribute("OutputUCS", UCase(vntOutput))
                End If
            Next elmPayee

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public Function gcolFindPayeeMatches(ByRef strRawInput As String) As VB6XmlNodeList
        Dim strInput As String
        Dim strXPath As String

        gcolFindPayeeMatches = Nothing
        Try

            strInput = UCase(Trim(strRawInput))
            strInput = Replace(strInput, "'", "")
            strXPath = "Payee[substring(@OutputUCS,1," & Len(strInput) & ")='" & strInput & "']"
            gcolFindPayeeMatches = gdomTransTableUCS.DocumentElement.SelectNodes(strXPath)

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    '$Description Load an XML file into a new DOM and return it.

    Public Function gdomLoadFile(ByVal strFile As String) As VB6XmlDocument
        Dim dom As VB6XmlDocument
        Dim objParseError As VB6XmlParseError

        gdomLoadFile = Nothing
        Try

            dom = New VB6XmlDocument
            With dom
                .Load(strFile)
                objParseError = .ParseError
                If Not objParseError Is Nothing Then
                    gRaiseError("XML parse error loading file: " & gstrXMLParseErrorText(objParseError))
                End If
                .SetProperty("SelectionLanguage", "XPath")
            End With
            gdomLoadFile = dom

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
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
        Dim intDotPos As Short
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

    Public Function gstrFormatInteger(input As Integer, style As String) As String
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
End Module