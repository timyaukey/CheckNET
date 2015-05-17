Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic.PowerPacks.Printing.Compatibility.VB6
Imports CheckBookLib

Module CheckPrinting
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Public gstrNextCheckNumToPrint As String

    Private mdblMarginLeft As Double
    Private mdblMarginTop As Double

    Public Function gdomGetCheckFormat() As VB6XmlDocument
        Dim domCheckFormat As VB6XmlDocument
        Dim strCheckFormatFile As String
        Dim objParseError As VB6XmlParseError

        'UPGRADE_NOTE: Object gdomGetCheckFormat may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        gdomGetCheckFormat = Nothing
        domCheckFormat = New VB6XmlDocument
        strCheckFormatFile = gstrAddPath("CheckFormat.xml")
        domCheckFormat.Load(strCheckFormatFile)
        objParseError = domCheckFormat.ParseError
        If Not objParseError Is Nothing Then
            MsgBox("Error loading check format file: " & gstrXMLParseErrorText(objParseError))
            Exit Function
        End If

        domCheckFormat.SetProperty("SelectionLanguage", "XPath")
        gdomGetCheckFormat = domCheckFormat

    End Function

    Public Sub gPrintCheck(ByVal domCheckFormat As VB6XmlDocument, ByVal objTrx As Trx)
        Dim Printer As New Object

        Dim colPayees As VB6XmlNodeList
        Dim objPayee As VB6XmlElement
        Dim strMailName As String
        Dim strMailAddr As String
        Dim strMailAddr2 As String
        Dim strMailAddrLine As String
        Dim strMailCityStateZip As String
        Dim strAccountNumber As String
        Dim intPayeeIndex As Short
        Dim intSemiPos As Short
        Dim elmItem As VB6XmlElement
        Dim dblX As Double
        Dim dblY As Double
        Dim dblLineHeight As Double
        Dim curAmount As Decimal

        'Printer.ScaleMode = ScaleModeConstants.vbInches
        Printer.FontName = "arial"
        Printer.FontSize = 10
        'UPGRADE_WARNING: Couldn't resolve default property of object domCheckFormat. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        GetCheckPrintPos(domCheckFormat, "Margins", elmItem, mdblMarginLeft, mdblMarginTop)
        dblLineHeight = Printer.TextHeight("X")

        curAmount = -objTrx.curAmount

        'Find the first memorized trx with the same payee name
        'and a mailing address.
        colPayees = gcolFindPayeeMatches((objTrx.strDescription))
        intPayeeIndex = 0
        Do
            If intPayeeIndex >= colPayees.Length Then
                Exit Do
            End If
            objPayee = colPayees(intPayeeIndex)
            strMailAddr = gstrGetXMLChildText(objPayee, "Address1")
            strMailAddr2 = gstrGetXMLChildText(objPayee, "Address2")
            strMailCityStateZip = gstrGetXMLChildText(objPayee, "City") & ", " & gstrGetXMLChildText(objPayee, "State") & " " & gstrGetXMLChildText(objPayee, "Zip")
            strAccountNumber = gstrGetXMLChildText(objPayee, "Account")
            intSemiPos = InStr(strMailAddr, ";")
            If intSemiPos = 0 Then
                strMailName = objTrx.strDescription
            Else
                strMailName = Left(strMailAddr, intSemiPos - 1)
                strMailAddr = Mid(strMailAddr, intSemiPos + 1)
            End If
            strMailAddrLine = strMailAddr
            If strMailAddr2 <> "" Then
                strMailAddrLine = strMailAddrLine & ", " & strMailAddr2
            End If
            If strMailAddr <> "" Then
                Exit Do
            End If
            intPayeeIndex = intPayeeIndex + 1
        Loop

        'UPGRADE_WARNING: Couldn't resolve default property of object domCheckFormat. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PrintCheckText(domCheckFormat, "Date", VB6.Format(objTrx.datDate, gstrFORMAT_DATE))
        'UPGRADE_WARNING: Couldn't resolve default property of object domCheckFormat. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PrintCheckText(domCheckFormat, "ShortAmount", VB6.Format(curAmount, gstrFORMAT_CURRENCY))
        'UPGRADE_WARNING: Couldn't resolve default property of object domCheckFormat. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PrintCheckText(domCheckFormat, "Payee", objTrx.strDescription)
        Dim intPennies As Short
        intPennies = Fix(curAmount * 100.0#) - Fix(curAmount) * 100.0#
        Dim strDollars As String
        strDollars = gstrAmountToWords(curAmount)
        strDollars = UCase(Left(strDollars, 1)) & Mid(strDollars, 2)
        'UPGRADE_WARNING: Couldn't resolve default property of object domCheckFormat. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PrintCheckText(domCheckFormat, "LongAmount", strDollars & " and " & VB6.Format(intPennies, "00") & "/100")
        If strAccountNumber <> "" Then
            'UPGRADE_WARNING: Couldn't resolve default property of object domCheckFormat. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            PrintCheckText(domCheckFormat, "AccountNumber", "Account #: " & strAccountNumber)
        End If

        If strMailAddr <> "" Then
            'UPGRADE_WARNING: Couldn't resolve default property of object domCheckFormat. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            GetCheckPrintPos(domCheckFormat, "MailingAddress", elmItem, dblX, dblY)
            PrintCheckLine(dblX, dblY, dblLineHeight, strMailName)
            PrintCheckLine(dblX, dblY, dblLineHeight, strMailAddrLine)
            PrintCheckLine(dblX, dblY, dblLineHeight, strMailCityStateZip)
        End If

        'Deliberately do NOT print the memo, because this is a note to the operator
        'and may be information that should not be communicated to payee.
        'Everything the payee needs to see (account numbers, invoice numbers)
        'is printed elsewhere.

        'UPGRADE_WARNING: Couldn't resolve default property of object domCheckFormat. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PrintOptionalCheckText(domCheckFormat, "Payee2", strMailName)
        'UPGRADE_WARNING: Couldn't resolve default property of object domCheckFormat. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PrintOptionalCheckText(domCheckFormat, "Amount2", "$" & VB6.Format(curAmount, gstrFORMAT_CURRENCY))
        'UPGRADE_WARNING: Couldn't resolve default property of object domCheckFormat. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PrintOptionalCheckText(domCheckFormat, "Date2", VB6.Format(objTrx.datDate, gstrFORMAT_DATE))
        'UPGRADE_WARNING: Couldn't resolve default property of object domCheckFormat. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PrintOptionalCheckText(domCheckFormat, "Number2", "#" & objTrx.strNumber)

        'UPGRADE_WARNING: Couldn't resolve default property of object domCheckFormat. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PrintInvoiceNumbers(domCheckFormat, "InvoiceList1", objTrx, dblLineHeight)
        'UPGRADE_WARNING: Couldn't resolve default property of object domCheckFormat. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        PrintInvoiceNumbers(domCheckFormat, "InvoiceList2", objTrx, dblLineHeight)

        Printer.EndDoc()

    End Sub

    Private Sub PrintInvoiceNumbers(ByVal domCheckFormat As VB6XmlDocument, ByVal strItemName As String, ByVal objTrx As Trx, ByVal dblLineHeight As Double)

        Dim elmInvoiceList As VB6XmlElement
        Dim dblX As Double
        Dim dblY As Double
        Dim dblStartY As Double
        Dim intMaxRows As Short
        Dim intMaxCols As Short
        Dim dblColWidth As Double
        Dim intRowNum As Short
        Dim intColNum As Short
        Dim vntAttrib As Object
        Dim objSplit As Split_Renamed
        Dim strInvoiceNum As String

        '<InvoiceList1 x="2.0" y="4.0" rows="3" cols="2" colwidth="1.5" />

        'Does the check format include a place to print invoice numbers?
        elmInvoiceList = objGetCheckPrintPos(domCheckFormat, strItemName, dblX, dblY)
        If elmInvoiceList Is Nothing Then
            Exit Sub
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object elmInvoiceList.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmInvoiceList.GetAttribute("rows")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            MsgBox("Could not find ""rows"" attribute of <" & strItemName & "> in check format file")
            Exit Sub
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        intMaxRows = Val(vntAttrib)

        'UPGRADE_WARNING: Couldn't resolve default property of object elmInvoiceList.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmInvoiceList.GetAttribute("cols")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            MsgBox("Could not find ""cols"" attribute of <" & strItemName & "> in check format file")
            Exit Sub
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        intMaxCols = Val(vntAttrib)

        'UPGRADE_WARNING: Couldn't resolve default property of object elmInvoiceList.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmInvoiceList.GetAttribute("colwidth")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            MsgBox("Could not find ""colwidth"" attribute of <" & strItemName & "> in check format file")
            Exit Sub
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        dblColWidth = Val(vntAttrib)

        intRowNum = 1
        intColNum = 1

        For Each objSplit In objTrx.colSplits
            strInvoiceNum = objSplit.strInvoiceNum
            If strInvoiceNum <> "" Then
                If intRowNum = 1 And intColNum = 1 Then
                    PrintCheckLine(dblX, dblY, dblLineHeight, "Invoice Numbers:")
                    dblY = dblY + dblLineHeight / 2
                    dblStartY = dblY
                End If
                If intRowNum > intMaxRows Then
                    intRowNum = 1
                    intColNum = intColNum + 1
                    If intColNum > intMaxCols Then
                        MsgBox("There are too many invoice numbers to print. " & "Will print as many as possible.")
                        Exit Sub
                    End If
                    dblY = dblStartY
                    dblX = dblX + dblColWidth
                End If
                PrintCheckLine(dblX, dblY, dblLineHeight, strInvoiceNum)
                intRowNum = intRowNum + 1
            End If
        Next objSplit

    End Sub

    Private Sub PrintCheckText(ByVal domCheckFormat As VB6XmlDocument, ByVal strItemName As String, ByVal strValue As String)

        Dim elmItem As VB6XmlElement
        Dim dblX As Double
        Dim dblY As Double

        GetCheckPrintPos(domCheckFormat, strItemName, elmItem, dblX, dblY)
        If elmItem Is Nothing Then
            Exit Sub
        End If

        SetLocation(dblX, dblY)

        'Text below prints with upper left corner of character cell at CurrentX, CurrentY
        PrintString(strValue)

        ' (x,y)
        'Printer.Line (100, 300)-(200, 500), vbBlack

    End Sub

    Private Sub PrintOptionalCheckText(ByVal domCheckFormat As VB6XmlDocument, ByVal strItemName As String, ByVal strValue As String)

        Dim elmItem As VB6XmlElement
        Dim dblX As Double
        Dim dblY As Double

        elmItem = objGetCheckPrintPos(domCheckFormat, strItemName, dblX, dblY)
        If elmItem Is Nothing Then
            Exit Sub
        End If

        SetLocation(dblX, dblY)

        'Text below prints with upper left corner of character cell at CurrentX, CurrentY
        PrintString(strValue)

        ' (x,y)
        'Printer.Line (100, 300)-(200, 500), vbBlack

    End Sub

    Private Sub PrintCheckLine(ByVal dblX As Double, ByRef dblY As Double, ByVal dblLineHeight As Double, ByVal strValue As String)

        SetLocation(dblX, dblY)
        PrintString(strValue)
        dblY = dblY + dblLineHeight

    End Sub

    Private Sub PrintString(ByVal strValue As String)
        Dim Printer As New Object
        Printer.Print(strValue)
    End Sub

    Private Sub SetLocation(ByVal dblX As Double, ByVal dblY As Double)
        Dim Printer As New Object
        Printer.CurrentX = dblX - mdblMarginLeft
        Printer.CurrentY = dblY - mdblMarginTop
    End Sub

    Private Sub GetCheckPrintPos(ByVal domCheckFormat As VB6XmlDocument, ByVal strItemName As String, ByRef elmItem As VB6XmlElement, ByRef dblX As Double, ByRef dblY As Double)

        elmItem = objGetCheckPrintPos(domCheckFormat, strItemName, dblX, dblY)
        If elmItem Is Nothing Then
            MsgBox("Could not find <" & strItemName & "> in check format file")
            Exit Sub
        End If
    End Sub

    Private Function objGetCheckPrintPos(ByVal domCheckFormat As VB6XmlDocument, ByVal strItemName As String, ByRef dblX As Double, ByRef dblY As Double) As VB6XmlElement

        Dim elmItem As VB6XmlElement
        Dim vntAttrib As Object

        'UPGRADE_NOTE: Object objGetCheckPrintPos may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objGetCheckPrintPos = Nothing

        elmItem = domCheckFormat.DocumentElement.SelectSingleNode(strItemName)
        If elmItem Is Nothing Then
            Exit Function
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object elmItem.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmItem.GetAttribute("x")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            MsgBox("Could not find ""x"" attribute of <" & strItemName & "> in check format file")
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        dblX = Val(vntAttrib)

        'UPGRADE_WARNING: Couldn't resolve default property of object elmItem.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmItem.GetAttribute("y")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            MsgBox("Could not find ""y"" attribute of <" & strItemName & "> in check format file")
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        dblY = Val(vntAttrib)

        objGetCheckPrintPos = elmItem
    End Function
End Module