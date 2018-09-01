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