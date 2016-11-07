Option Strict Off
Option Explicit On

Imports System.Drawing.Printing
Imports CheckBookLib

Module CheckPrinting
    '23456678901234567890123456789012345678901234567890123456789012345678901234567890123456

    Public gstrNextCheckNumToPrint As String

    Private mdblMarginLeft As Double
    Private mdblMarginTop As Double
    Private mdblCurrentX As Double
    Private mdblCurrentY As Double
    Private mdomCheckFormat As VB6XmlDocument
    Private mobjTrx As Trx
    Private mobjFont As Font

    Public Function gdomGetCheckFormat() As VB6XmlDocument
        Dim domCheckFormat As VB6XmlDocument
        Dim strCheckFormatFile As String
        Dim objParseError As VB6XmlParseError

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

    Public Sub gPrintCheck(ByVal domCheckFormat_ As VB6XmlDocument, ByVal objTrx_ As Trx)
        Dim objPrintDoc As PrintDocument

        mdomCheckFormat = domCheckFormat_
        mobjTrx = objTrx_
        objPrintDoc = New PrintDocument
        AddHandler objPrintDoc.PrintPage, AddressOf pd_PrintPage
        '        objPrintDoc.Print()
        Dim obj As PrintPreviewDialog = New PrintPreviewDialog()
        obj.Document = objPrintDoc
        obj.ShowDialog()

    End Sub

    Private Sub pd_PrintPage(ByVal sender As Object, ByVal ev As PrintPageEventArgs)
        Dim colPayees As VB6XmlNodeList
        Dim objPayee As VB6XmlElement
        Dim strMailName As String = ""
        Dim strMailAddr As String = ""
        Dim strMailAddr2 As String = ""
        Dim strMailAddrLine As String = ""
        Dim strMailCityStateZip As String = ""
        Dim strAccountNumber As String = ""
        Dim intPayeeIndex As Short
        Dim intSemiPos As Short
        Dim elmItem As VB6XmlElement = Nothing
        Dim dblX As Double
        Dim dblY As Double
        Dim dblLineHeight As Double
        Dim curAmount As Decimal

        mobjFont = New Font("Arial", 10)
        GetCheckPrintPos(mdomCheckFormat, "Margins", elmItem, mdblMarginLeft, mdblMarginTop)
        ev.Graphics.PageUnit = GraphicsUnit.Inch
        dblLineHeight = mobjFont.GetHeight(ev.Graphics)

        curAmount = -mobjTrx.curAmount

        'Find the first memorized trx with the same payee name
        'and a mailing address.
        colPayees = gcolFindPayeeMatches((mobjTrx.strDescription))
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
                strMailName = mobjTrx.strDescription
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

        PrintCheckText(mdomCheckFormat, "Date", gstrFormatDate(mobjTrx.datDate), ev)
        PrintCheckText(mdomCheckFormat, "ShortAmount", gstrFormatCurrency(curAmount), ev)
        PrintCheckText(mdomCheckFormat, "Payee", mobjTrx.strDescription, ev)
        Dim intPennies As Short
        intPennies = Fix(curAmount * 100.0#) - Fix(curAmount) * 100.0#
        Dim strDollars As String
        strDollars = gstrAmountToWords(curAmount)
        strDollars = UCase(Left(strDollars, 1)) & Mid(strDollars, 2)
        PrintCheckText(mdomCheckFormat, "LongAmount", strDollars & " and " & gstrFormatInteger(intPennies, "00") & "/100", ev)
        If strAccountNumber <> "" Then
            PrintCheckText(mdomCheckFormat, "AccountNumber", "Account #: " & strAccountNumber, ev)
        End If

        If strMailAddr <> "" Then
            GetCheckPrintPos(mdomCheckFormat, "MailingAddress", elmItem, dblX, dblY)
            PrintCheckLine(dblX, dblY, dblLineHeight, strMailName, ev)
            PrintCheckLine(dblX, dblY, dblLineHeight, strMailAddrLine, ev)
            PrintCheckLine(dblX, dblY, dblLineHeight, strMailCityStateZip, ev)
        End If

        'Deliberately do NOT print the memo, because this is a note to the operator
        'and may be information that should not be communicated to payee.
        'Everything the payee needs to see (account numbers, invoice numbers)
        'is printed elsewhere.

        PrintOptionalCheckText(mdomCheckFormat, "Payee2", strMailName, ev)
        PrintOptionalCheckText(mdomCheckFormat, "Amount2", "$" & gstrFormatCurrency(curAmount), ev)
        PrintOptionalCheckText(mdomCheckFormat, "Date2", gstrFormatDate(mobjTrx.datDate), ev)
        PrintOptionalCheckText(mdomCheckFormat, "Number2", "#" & mobjTrx.strNumber, ev)

        PrintInvoiceNumbers(mdomCheckFormat, "InvoiceList1", mobjTrx, dblLineHeight, ev)
        PrintInvoiceNumbers(mdomCheckFormat, "InvoiceList2", mobjTrx, dblLineHeight, ev)

        ev.HasMorePages = False

    End Sub

    Private Sub PrintInvoiceNumbers(ByVal domCheckFormat As VB6XmlDocument, ByVal strItemName As String, ByVal objTrx As Trx, ByVal dblLineHeight As Double, ByVal ev As PrintPageEventArgs)

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

        vntAttrib = elmInvoiceList.GetAttribute("rows")
        If gblnXmlAttributeMissing(vntAttrib) Then
            MsgBox("Could not find ""rows"" attribute of <" & strItemName & "> in check format file")
            Exit Sub
        End If
        intMaxRows = Val(vntAttrib)

        vntAttrib = elmInvoiceList.GetAttribute("cols")
        If gblnXmlAttributeMissing(vntAttrib) Then
            MsgBox("Could not find ""cols"" attribute of <" & strItemName & "> in check format file")
            Exit Sub
        End If
        intMaxCols = Val(vntAttrib)

        vntAttrib = elmInvoiceList.GetAttribute("colwidth")
        If gblnXmlAttributeMissing(vntAttrib) Then
            MsgBox("Could not find ""colwidth"" attribute of <" & strItemName & "> in check format file")
            Exit Sub
        End If
        dblColWidth = Val(vntAttrib)

        intRowNum = 1
        intColNum = 1

        For Each objSplit In objTrx.colSplits
            strInvoiceNum = objSplit.strInvoiceNum
            If strInvoiceNum <> "" Then
                If intRowNum = 1 And intColNum = 1 Then
                    PrintCheckLine(dblX, dblY, dblLineHeight, "Invoice Numbers:", ev)
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
                PrintCheckLine(dblX, dblY, dblLineHeight, strInvoiceNum, ev)
                intRowNum = intRowNum + 1
            End If
        Next objSplit

    End Sub

    Private Sub PrintCheckText(ByVal domCheckFormat As VB6XmlDocument, ByVal strItemName As String, ByVal strValue As String, ByVal ev As PrintPageEventArgs)

        Dim elmItem As VB6XmlElement = Nothing
        Dim dblX As Double
        Dim dblY As Double

        GetCheckPrintPos(domCheckFormat, strItemName, elmItem, dblX, dblY)
        If elmItem Is Nothing Then
            Exit Sub
        End If

        SetLocation(dblX, dblY, ev)

        'Text below prints with upper left corner of character cell at CurrentX, CurrentY
        PrintString(strValue, ev)

        ' (x,y)
        'Printer.Line (100, 300)-(200, 500), vbBlack

    End Sub

    Private Sub PrintOptionalCheckText(ByVal domCheckFormat As VB6XmlDocument, ByVal strItemName As String, ByVal strValue As String, ByVal ev As PrintPageEventArgs)

        Dim elmItem As VB6XmlElement
        Dim dblX As Double
        Dim dblY As Double

        elmItem = objGetCheckPrintPos(domCheckFormat, strItemName, dblX, dblY)
        If elmItem Is Nothing Then
            Exit Sub
        End If

        SetLocation(dblX, dblY, ev)

        'Text below prints with upper left corner of character cell at CurrentX, CurrentY
        PrintString(strValue, ev)

        ' (x,y)
        'Printer.Line (100, 300)-(200, 500), vbBlack

    End Sub

    Private Sub PrintCheckLine(ByVal dblX As Double, ByRef dblY As Double, ByVal dblLineHeight As Double, ByVal strValue As String, ByVal ev As PrintPageEventArgs)

        SetLocation(dblX, dblY, ev)
        PrintString(strValue, ev)
        dblY = dblY + dblLineHeight

    End Sub

    Private Sub PrintString(ByVal strValue As String, ByVal ev As PrintPageEventArgs)
        ev.Graphics.DrawString(strValue, mobjFont, Brushes.Black, mdblCurrentX, mdblCurrentY, New StringFormat())
    End Sub

    Private Sub SetLocation(ByVal dblX As Double, ByVal dblY As Double, ByVal ev As PrintPageEventArgs)
        mdblCurrentX = dblX - mdblMarginLeft
        mdblCurrentY = dblY - mdblMarginTop
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