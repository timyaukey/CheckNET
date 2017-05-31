Option Strict On
Option Explicit On
Imports CheckBookLib

Public Class HTMLWriter
    Private mobjCompany As Company
    Private mstrFileNameRoot As String
    Private mobjReportManager As ReportGroupManager
    Private mobjBuilder As System.Text.StringBuilder

    Public Sub New(ByVal objCompany As Company, ByVal strFileNameRoot As String, ByVal objReportManager As ReportGroupManager)
        mobjCompany = objCompany
        mstrFileNameRoot = strFileNameRoot
        mobjReportManager = objReportManager
        mobjBuilder = New System.Text.StringBuilder()
    End Sub

    Public Sub BeginReport()
        OutputLine("<html>")
        OutputLine("<head>")
        OutputLine(" <link rel='stylesheet' type='text/css' href='" + mstrFileNameRoot + ".css'>")
        OutputLine("</head>")
        OutputLine("<body>")
        OutputLine("<div class='ReportBody'>")
    End Sub

    Public Sub EndReport()
        OutputLine("</div>")
        OutputLine("</body>")
        OutputLine("</html>")
    End Sub

    Public Sub OutputHeader(ByVal strTitle As String, ByVal strSubTitle As String)
        OutputDiv("ReportCompanyName", mobjCompany.strCompanyName)
        OutputDiv("ReportTitle", strTitle)
        OutputDiv("ReportSubTitle", strSubTitle)
        OutputDiv("ReportPrepared", "Prepared On " + DateTime.Today.ToLongDateString())
    End Sub

    Public Sub OutputGroupItems(ByVal strTitleClass As String, ByVal strAmountClass As String,
                           ByVal strNegativeClass As String, ByVal strGroupKey As String, ByVal objAccum As ReportAccumulator)
        Dim objGroup As LineItemGroup = mobjReportManager.objGetGroup(strGroupKey)
        For Each objLine As ReportLineItem In objGroup.colItems
            OutputAmount(strTitleClass, objLine.strItemTitle, strAmountClass, strNegativeClass, objLine.curTotal, objAccum)
            objLine.blnPrinted = True
        Next
    End Sub

    Public Sub OutputGroupSummary(ByVal strTitleClass As String, ByVal strTitle As String, ByVal strAmountClass As String,
                           ByVal strNegativeClass As String, ByVal strGroupKey As String, ByVal objAccum As ReportAccumulator)
        Dim objGroup As LineItemGroup = mobjReportManager.objGetGroup(strGroupKey)
        OutputAmount(strTitleClass, strTitle, strAmountClass, strNegativeClass, objGroup.curGroupTotal, objAccum)
        objGroup.blnPrinted = True
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
        If curAmount < 0 Then
            strExtraClass = " " + strNegativeClass
        End If
        OutputLine("<div>")
        OutputLine("<span class='" + strTitleClass + "'>" + strTitle + "</span>")
        OutputLine("<span class='" + strAmountClass + strExtraClass + "'>" + gstrFormatCurrency(curAmount) + "</span>")
        OutputLine("</div>")
        objAccum.Add(curAmount)
    End Sub

    Private Sub OutputLine(ByVal strLine As String)
        mobjBuilder.AppendLine(strLine)
    End Sub

    Public Sub ShowReport()
        Dim strReportFile As String = gstrReportPath() + "\" + mstrFileNameRoot + ".html"
        Using objFile As System.IO.TextWriter = New System.IO.StreamWriter(strReportFile)
            objFile.Write(mobjBuilder.ToString())
        End Using
        Dim info As System.Diagnostics.ProcessStartInfo = New ProcessStartInfo(strReportFile)
        info.UseShellExecute = True
        System.Diagnostics.Process.Start(info)
    End Sub

    Public Sub CheckPrinted()
        For Each objGroup As LineItemGroup In mobjReportManager.colGroups
            If Not objGroup.blnPrinted Then
                For Each objItem As ReportLineItem In objGroup.colItems
                    If Not objItem.blnPrinted Then
                        MsgBox("Report line item with key [" + objItem.strItemKey + "] was not printed")
                    End If
                Next
            End If
        Next
    End Sub
End Class
