Option Strict On
Option Explicit On

Public Class HTMLWriter
    Private ReadOnly mobjHostUI As IHostUI
    Private ReadOnly mobjCompany As Company
    Private ReadOnly mstrFileNameRoot As String
    Private ReadOnly mblnLocalStylesheet As Boolean
    Private ReadOnly mobjBuilder As System.Text.StringBuilder
    Public blnUseMinusNumbers As Boolean

    Public Sub New(ByVal objHostUI As IHostUI, ByVal strFileNameRoot As String, ByVal blnLocalStylesheet As Boolean)
        mobjHostUI = objHostUI
        mobjCompany = mobjHostUI.objCompany
        mstrFileNameRoot = strFileNameRoot
        mblnLocalStylesheet = blnLocalStylesheet
        mobjBuilder = New System.Text.StringBuilder()
        blnUseMinusNumbers = False
    End Sub

    Public Sub BeginReport()
        OutputLine("<html>")
        OutputLine("<head>")
        OutputLine(" <link rel='stylesheet' type='text/css' href='AllReports.css'>")
        CopyStylesheet("AllReports.css")
        If mblnLocalStylesheet Then
            OutputLine(" <link rel='stylesheet' type='text/css' href='" + mstrFileNameRoot + ".css'>")
            CopyStylesheet(mstrFileNameRoot + ".css")
        End If
        OutputLine("</head>")
        OutputLine("<body>")
        OutputLine("<div class='ReportBody'>")
    End Sub

    Private Sub CopyStylesheet(ByVal strFileName As String)
        Dim strInputPath As String = Company.ExecutableFolder() + "\\Reports\\" + strFileName
        Dim strOutputPath As String = mobjCompany.ReportsFolderPath() + "\\" + strFileName
        System.IO.File.Copy(strInputPath, strOutputPath, True)
    End Sub

    Public Sub EndReport()
        OutputLine("</div>")
        OutputLine("</body>")
        OutputLine("</html>")
    End Sub

    Public Sub OutputHeader(ByVal strTitle As String, ByVal strSubTitle As String)
        OutputDiv("ReportCompanyName", mobjCompany.Info.CompanyName)
        OutputDiv("ReportTitle", strTitle)
        OutputDiv("ReportSubTitle", strSubTitle)
        OutputDiv("ReportPrepared", "Prepared " + DateTime.Today.ToLongDateString())
    End Sub

    Public Sub OutputGroupItems(ByVal strTitleClass As String, ByVal strAmountClass As String,
                           ByVal strNegativeClass As String, ByVal objReportManager As ReportGroupManager,
                           ByVal strGroupKey As String, ByVal objAccum As ReportAccumulator)
        Dim objGroup As LineItemGroup = objReportManager.GetGroup(strGroupKey)
        For Each objLine As ReportLineItem In objGroup.Items
            If objLine.Total <> 0D Then
                OutputAmount(strTitleClass, objLine.ItemTitle, strAmountClass, strNegativeClass, objLine.Total, objAccum)
            End If
            objLine.IsPrinted = True
        Next
    End Sub

    Public Sub OutputGroupSummary(ByVal strTitleClass As String, ByVal strTitle As String, ByVal strAmountClass As String,
                           ByVal strNegativeClass As String, ByVal objReportManager As ReportGroupManager,
                           ByVal strGroupKey As String, ByVal blnOmitIfZero As Boolean, ByVal objAccum As ReportAccumulator)
        Dim objGroup As LineItemGroup = objReportManager.GetGroup(strGroupKey)
        If objGroup.GroupTotal <> 0D Or Not blnOmitIfZero Then
            OutputAmount(strTitleClass, strTitle, strAmountClass, strNegativeClass, objGroup.GroupTotal, objAccum)
        End If
        objGroup.IsPrinted = True
    End Sub

    Public Sub OutputText(ByVal strClass As String, ByVal strContent As String)
        OutputDiv(strClass, strContent)
    End Sub

    Private Sub OutputDiv(ByVal strClass As String, ByVal strContent As String)
        OutputLine("<div class='" + strClass + "'>" + strContent + "</div>")
    End Sub

    Public Sub OutputAmount(ByVal strTitleClass As String, ByVal strTitle As String, ByVal strAmountClass As String,
                          ByVal strNegativeClass As String, ByVal curAmount As Decimal, objAccum As ReportAccumulator)
        Dim strExtraClass As String = ""
        Dim curOutputAmount As Decimal = curAmount
        If blnUseMinusNumbers Then
            curOutputAmount = -curOutputAmount
        End If
        If curOutputAmount < 0 Then
            strExtraClass = " " + strNegativeClass
        End If
        OutputLine("<div>")
        OutputLine("<span class='" + strTitleClass + "'>" + strTitle + "</span>")
        OutputLine("<span class='" + strAmountClass + strExtraClass + "'>" + curOutputAmount.ToString("###,###,###,##0.00") + "</span>")
        OutputLine("</div>")
        objAccum.Add(curAmount)
    End Sub

    Public Sub OutputTableStart()
        OutputLine("<table class='ReportTable'>")
    End Sub

    Public Sub OutputTableEnd()
        OutputLine("</table>")
    End Sub

    Public Sub OutputTableHeaderStart()
        OutputLine("<thead class='ReportTableHeaderRow'><tr>")
    End Sub

    Public Sub OutputTableHeaderEnd()
        OutputLine("</tr></thead>")
    End Sub

    Public Sub OutputTableHeaderTitle(ByVal strLabel As String)
        OutputLine("<th class='ReportTableHeaderTitle'>" + strLabel + "</td>")
    End Sub

    Public Sub OutputTableHeaderAmount(ByVal strLabel As String)
        OutputLine("<th class='ReportTableHeaderAmount'>" + strLabel + "</td>")
    End Sub

    Public Sub OutputTableRowStart()
        OutputLine("<tr class='ReportTableDataRow'>")
    End Sub

    Public Sub OutputTableRowEnd()
        OutputLine("</tr>")
    End Sub

    Public Sub OutputTableDataTitle(ByVal strTitleClass As String, ByVal strTitle As String)
        OutputLine("<td class='" + strTitleClass + "'>" + strTitle + "</td>")
    End Sub

    Public Sub OutputTableDataAmount(ByVal strAmountClass As String, ByVal strNegativeClass As String, ByVal curAmount As Decimal)
        Dim strExtraClass As String = ""
        If curAmount < 0 Then
            strExtraClass = " " + strNegativeClass
        End If
        OutputLine("<td class='" + strAmountClass + strExtraClass + "'>" + curAmount.ToString("###,###,###,##0.00") + "</td>")
    End Sub

    Private Sub OutputLine(ByVal strLine As String)
        mobjBuilder.AppendLine(strLine)
    End Sub

    Public Sub ShowReport()
        Dim strReportFile As String = mobjCompany.ReportsFolderPath() + "\" + mstrFileNameRoot + ".html"
        Using objFile As System.IO.TextWriter = New System.IO.StreamWriter(strReportFile)
            objFile.Write(mobjBuilder.ToString())
        End Using
        Dim info As System.Diagnostics.ProcessStartInfo = New ProcessStartInfo(strReportFile)
        info.UseShellExecute = True
        System.Diagnostics.Process.Start(info)
    End Sub

    Public Sub CheckPrinted(ByVal objReportManager As ReportGroupManager)
        For Each objGroup As LineItemGroup In objReportManager.Groups
            If Not objGroup.IsPrinted Then
                For Each objItem As ReportLineItem In objGroup.Items
                    If Not objItem.IsPrinted Then
                        mobjHostUI.InfoMessageBox("Report line item with key [" + objItem.ItemKey + "] was not printed")
                    End If
                Next
            End If
        Next
    End Sub
End Class
