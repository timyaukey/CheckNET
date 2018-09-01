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

    Public Const gstrFORMAT_CURRENCY As String = "#######0.00"
    Public Const gstrFORMAT_DATE As String = "mm/dd/yy"
    Public Const gstrFORMAT_DATE2 As String = "MM/dd/yy"
    Public Const gstrUNABLE_TO_TRANSLATE As String = "???"

    'Lower bound of many arrays
    Public Const gintLBOUND1 As Short = 1

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