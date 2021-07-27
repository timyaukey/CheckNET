Option Strict On
Option Explicit On

Imports System.Drawing.Printing

Public Class CheckPrinting

    Public Shared strNextCheckNumToPrint As String

    Private mobjHostUI As IHostUI
    Private mobjCompany As Company
    Private mdblMarginLeft As Double
    Private mdblMarginTop As Double
    Private mdblCurrentX As Double
    Private mdblCurrentY As Double
    Private mdomCheckFormat As CBXmlDocument
    Private mobjTrx As BankTrx
    Private mobjFont As Font

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public Function blnAllowedToPrintCheck(ByVal objTestTrx As BankTrx) As Boolean

        If objTestTrx.Amount >= 0 Then
            mobjHostUI.ErrorMessageBox("You may only print a check for a debit transaction.")
            Return False
        End If

        Dim intCheckNum As Integer
        If Int32.TryParse(objTestTrx.Number, intCheckNum) Then
            mobjHostUI.ErrorMessageBox("You may not print a check for a transaction that already has a check number.")
            Return False
        End If

        Return True

    End Function

    Public Function blnPrepareForFirstCheck() As Boolean

        If Not System.IO.File.Exists(mobjHostUI.objCompany.CheckFormatFilePath()) Then
            mobjHostUI.InfoMessageBox("You must set up your check format first, using the option on the ""Setup"" menu.")
            Return False
        End If

        CheckPrinting.strNextCheckNumToPrint = InputBox("Please enter the check number to use:", "Check Number", CheckPrinting.strNextCheckNumToPrint)
        If CheckPrinting.strNextCheckNumToPrint = "" Then
            Return False
        End If

        Return True

    End Function

    Private Sub IncrementCheckNumber()
        CheckPrinting.strNextCheckNumToPrint = CStr(Val(CheckPrinting.strNextCheckNumToPrint) + 1)
    End Sub

    Private Function blnGetCheckFormat() As Boolean

        Dim strCheckFormatFile As String
        Dim objParseError As CBXmlParseError

        mdomCheckFormat = New CBXmlDocument
        strCheckFormatFile = mobjHostUI.objCompany.CheckFormatFilePath()
        mdomCheckFormat.Load(strCheckFormatFile)
        objParseError = mdomCheckFormat.ParseError
        If Not objParseError Is Nothing Then
            mobjHostUI.InfoMessageBox("Error loading check format file: " & gstrXMLParseErrorText(objParseError))
            Return False
        End If

        mdomCheckFormat.SetProperty("SelectionLanguage", "XPath")
        Return True

    End Function

    Public Function blnPrintCheck(ByVal objTrx_ As BankTrx) As Boolean
        Dim objPrintDoc As PrintDocument
        Dim blnPreview As Boolean = False

        mobjCompany = mobjHostUI.objCompany
        If Not blnGetCheckFormat() Then
            Return False
        End If
        mobjTrx = objTrx_
        objPrintDoc = New PrintDocument
        AddHandler objPrintDoc.PrintPage, AddressOf pd_PrintPage

        If blnPreview Then
            Dim preview As PrintPreviewDialog = New PrintPreviewDialog()
            preview.Height = 800
            preview.Width = 600
            preview.Document = objPrintDoc
            preview.ShowDialog()
        Else
            Dim dlg As PrintDialog = New PrintDialog()
            dlg.PrintToFile = False
            dlg.AllowCurrentPage = False
            dlg.AllowSomePages = False
            dlg.Document = objPrintDoc
            If dlg.ShowDialog() = DialogResult.OK Then
                objPrintDoc.Print()
                IncrementCheckNumber()
                Return True
            End If
        End If
        Return False

    End Function

    Private Sub pd_PrintPage(ByVal sender As Object, ByVal ev As PrintPageEventArgs)
        Dim colPayees As CBXmlNodeList
        Dim objPayee As CBXmlElement
        Dim strMailName As String = ""
        Dim strMailAddr As String = ""
        Dim strMailAddr2 As String = ""
        Dim strMailAddrLine As String = ""
        Dim strMailCityStateZip As String = ""
        Dim strAccountNumber As String = ""
        Dim intPayeeIndex As Integer
        Dim intSemiPos As Integer
        Dim elmItem As CBXmlElement = Nothing
        Dim dblX As Double
        Dim dblY As Double
        Dim dblLineHeight As Double
        Dim curAmount As Decimal

        mobjFont = New Font("Arial", 10)
        GetCheckPrintPos("Margins", elmItem, mdblMarginLeft, mdblMarginTop)
        ev.Graphics.PageUnit = GraphicsUnit.Inch
        dblLineHeight = mobjFont.GetHeight(ev.Graphics)

        curAmount = -mobjTrx.Amount

        'Find the first memorized trx with the same payee name
        'and a mailing address.
        colPayees = mobjCompany.FindPayeeMatches((mobjTrx.Description))
        intPayeeIndex = 0
        Do
            If intPayeeIndex >= colPayees.Length Then
                Exit Do
            End If
            objPayee = DirectCast(colPayees(intPayeeIndex), CBXmlElement)
            strMailAddr = gstrGetXMLChildText(objPayee, "Address1")
            strMailAddr2 = gstrGetXMLChildText(objPayee, "Address2")
            strMailCityStateZip = gstrGetXMLChildText(objPayee, "City") & ", " & gstrGetXMLChildText(objPayee, "State") & " " & gstrGetXMLChildText(objPayee, "Zip")
            strAccountNumber = gstrGetXMLChildText(objPayee, "Account")
            intSemiPos = InStr(strMailAddr, ";")
            If intSemiPos = 0 Then
                strMailName = mobjTrx.Description
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

        PrintCheckText("Date", Utilities.strFormatDate(mobjTrx.TrxDate), ev)
        PrintCheckText("ShortAmount", Utilities.strFormatCurrency(curAmount), ev)
        PrintCheckText("Payee", mobjTrx.Description, ev)
        Dim intPennies As Integer
        intPennies = CInt(Fix(curAmount * 100.0#) - Fix(curAmount) * 100.0#)
        Dim strDollars As String
        strDollars = MoneyFormat.strAmountToWords(curAmount)
        strDollars = UCase(Left(strDollars, 1)) & Mid(strDollars, 2)
        PrintCheckText("LongAmount", strDollars & " and " & Utilities.strFormatInteger(intPennies, "00") & "/100", ev)
        If strAccountNumber <> "" Then
            PrintCheckText("AccountNumber", "Account #: " & strAccountNumber, ev)
        End If

        If strMailAddr <> "" Then
            GetCheckPrintPos("MailingAddress", elmItem, dblX, dblY)
            PrintCheckLine(dblX, dblY, dblLineHeight, strMailName, ev)
            PrintCheckLine(dblX, dblY, dblLineHeight, strMailAddrLine, ev)
            PrintCheckLine(dblX, dblY, dblLineHeight, strMailCityStateZip, ev)
        End If

        'Deliberately do NOT print the memo, because this is a note to the operator
        'and may be information that should not be communicated to payee.
        'Everything the payee needs to see (account numbers, invoice numbers)
        'is printed elsewhere.

        PrintOptionalCheckText("Payee2", strMailName, ev)
        PrintOptionalCheckText("Amount2", "$" & Utilities.strFormatCurrency(curAmount), ev)
        PrintOptionalCheckText("Date2", Utilities.strFormatDate(mobjTrx.TrxDate), ev)
        PrintOptionalCheckText("Number2", "#" & mobjTrx.Number, ev)

        PrintInvoiceNumbers("InvoiceList1", mobjTrx, dblLineHeight, ev)
        PrintInvoiceNumbers("InvoiceList2", mobjTrx, dblLineHeight, ev)

        ev.HasMorePages = False

    End Sub

    Private Sub PrintInvoiceNumbers(ByVal strItemName As String, ByVal objTrx As BankTrx, ByVal dblLineHeight As Double, ByVal ev As PrintPageEventArgs)

        Dim elmInvoiceList As CBXmlElement
        Dim dblX As Double
        Dim dblY As Double
        Dim dblStartY As Double
        Dim intMaxRows As Integer
        Dim intMaxCols As Integer
        Dim dblColWidth As Double
        Dim intRowNum As Integer
        Dim intColNum As Integer
        Dim vntAttrib As Object
        Dim objSplit As TrxSplit
        Dim strInvoiceNum As String

        '<InvoiceList1 x="2.0" y="4.0" rows="3" cols="2" colwidth="1.5" />

        'Does the check format include a place to print invoice numbers?
        elmInvoiceList = objGetCheckPrintPos(strItemName, dblX, dblY)
        If elmInvoiceList Is Nothing Then
            Exit Sub
        End If
        If dblX = 0.0D And dblY = 0.0D Then
            Exit Sub
        End If

        vntAttrib = elmInvoiceList.GetAttribute("rows")
        If gblnXmlAttributeMissing(vntAttrib) Then
            mobjHostUI.InfoMessageBox("Could not find ""rows"" attribute of <" & strItemName & "> in check format file")
            Exit Sub
        End If
        intMaxRows = CInt(Val(vntAttrib))

        vntAttrib = elmInvoiceList.GetAttribute("cols")
        If gblnXmlAttributeMissing(vntAttrib) Then
            mobjHostUI.InfoMessageBox("Could not find ""cols"" attribute of <" & strItemName & "> in check format file")
            Exit Sub
        End If
        intMaxCols = CInt(Val(vntAttrib))

        vntAttrib = elmInvoiceList.GetAttribute("colwidth")
        If gblnXmlAttributeMissing(vntAttrib) Then
            mobjHostUI.InfoMessageBox("Could not find ""colwidth"" attribute of <" & strItemName & "> in check format file")
            Exit Sub
        End If
        dblColWidth = Val(vntAttrib)

        intRowNum = 1
        intColNum = 1

        For Each objSplit In objTrx.Splits
            strInvoiceNum = objSplit.InvoiceNum
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
                        mobjHostUI.InfoMessageBox("There are too many invoice numbers to print. " & "Will print as many as possible.")
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

    Private Sub PrintCheckText(ByVal strItemName As String, ByVal strValue As String, ByVal ev As PrintPageEventArgs)

        Dim elmItem As CBXmlElement = Nothing
        Dim dblX As Double
        Dim dblY As Double

        GetCheckPrintPos(strItemName, elmItem, dblX, dblY)
        If elmItem Is Nothing Then
            Exit Sub
        End If

        SetLocation(dblX, dblY, ev)

        'Text below prints with upper left corner of character cell at CurrentX, CurrentY
        PrintString(strValue, ev)

        ' (x,y)
        'Printer.Line (100, 300)-(200, 500), vbBlack

    End Sub

    Private Sub PrintOptionalCheckText(ByVal strItemName As String, ByVal strValue As String, ByVal ev As PrintPageEventArgs)

        Dim elmItem As CBXmlElement
        Dim dblX As Double
        Dim dblY As Double

        elmItem = objGetCheckPrintPos(strItemName, dblX, dblY)
        If elmItem Is Nothing Then
            Exit Sub
        End If
        If dblX = 0.0D And dblY = 0.0D Then
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
        ev.Graphics.DrawString(strValue, mobjFont, Brushes.Black, CSng(mdblCurrentX), CSng(mdblCurrentY), New StringFormat())
    End Sub

    Private Sub SetLocation(ByVal dblX As Double, ByVal dblY As Double, ByVal ev As PrintPageEventArgs)
        mdblCurrentX = dblX - mdblMarginLeft
        mdblCurrentY = dblY - mdblMarginTop
    End Sub

    Private Sub GetCheckPrintPos(ByVal strItemName As String, ByRef elmItem As CBXmlElement, ByRef dblX As Double, ByRef dblY As Double)

        elmItem = objGetCheckPrintPos(strItemName, dblX, dblY)
        If elmItem Is Nothing Then
            mobjHostUI.InfoMessageBox("Could not find <" & strItemName & "> in check format file")
            Exit Sub
        End If
    End Sub

    Private Function objGetCheckPrintPos(ByVal strItemName As String, ByRef dblX As Double, ByRef dblY As Double) As CBXmlElement

        Dim elmItem As CBXmlElement
        Dim vntAttrib As Object

        objGetCheckPrintPos = Nothing

        elmItem = DirectCast(mdomCheckFormat.DocumentElement.SelectSingleNode(strItemName), CBXmlElement)
        If elmItem Is Nothing Then
            Exit Function
        End If

        vntAttrib = elmItem.GetAttribute("x")
        If gblnXmlAttributeMissing(vntAttrib) Then
            mobjHostUI.InfoMessageBox("Could not find ""x"" attribute of <" & strItemName & "> in check format file")
            Exit Function
        End If
        dblX = Val(vntAttrib)

        vntAttrib = elmItem.GetAttribute("y")
        If gblnXmlAttributeMissing(vntAttrib) Then
            mobjHostUI.InfoMessageBox("Could not find ""y"" attribute of <" & strItemName & "> in check format file")
            Exit Function
        End If
        dblY = Val(vntAttrib)

        objGetCheckPrintPos = elmItem
    End Function
End Class