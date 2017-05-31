Option Strict On
Option Explicit On
Imports CheckBookLib

Public Class HTMLWriter
    Private mobjCompany As Company
    Private mstrFileNameRoot As String
    Private mobjBuilder As System.Text.StringBuilder

    Public Sub New(ByVal objCompany As Company, ByVal strFileNameRoot As String)
        mobjCompany = objCompany
        mstrFileNameRoot = strFileNameRoot
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
        OutputDiv("ReportTitle", strTitle)
        OutputDiv("ReportSubTitle", strSubTitle)
        OutputDiv("ReportPrepared", "Prepared On " + DateTime.Today.ToLongDateString())
    End Sub

    Public Sub OutputGroup(ByVal strTitleClass As String, ByVal strAmountClass As String,
                           ByVal strNegativeClass As String, ByVal objGroup As LineItemGroup)
        For Each objLine As ReportLineItem In objGroup.colItems
            OutputPair(strTitleClass, objLine.strItemTitle, strAmountClass, strNegativeClass, objLine.curTotal)
        Next
    End Sub

    Public Sub OutputGroupSummary(ByVal strTitleClass As String, ByVal strTitle As String, ByVal strAmountClass As String,
                           ByVal strNegativeClass As String, ByVal objGroup As LineItemGroup)
        OutputPair(strTitleClass, strTitle, strAmountClass, strNegativeClass, objGroup.curGroupTotal)
    End Sub

    Public Sub OutputText(ByVal strClass As String, ByVal strContent As String)
        OutputDiv(strClass, strContent)
    End Sub

    Private Sub OutputDiv(ByVal strClass As String, ByVal strContent As String)
        OutputLine("<div class='" + strClass + "'>" + strContent + "</div>")
    End Sub

    Public Sub OutputPair(ByVal strTitleClass As String, ByVal strTitle As String, ByVal strAmountClass As String,
                          ByVal strNegativeClass As String, ByVal curAmount As Decimal)
        Dim strExtraClass As String = ""
        If curAmount < 0 Then
            strExtraClass = " " + strNegativeClass
        End If
        OutputLine("<div>")
        OutputLine("<span class='" + strTitleClass + "'>" + strTitle + "</span>")
        OutputLine("<span class='" + strAmountClass + strExtraClass + "'>" + gstrFormatCurrency(curAmount) + "</span>")
        OutputLine("</div>")
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
End Class
