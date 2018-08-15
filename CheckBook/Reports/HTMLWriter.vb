Option Strict On
Option Explicit On
Imports CheckBookLib

Public Class HTMLWriter
    Private mobjCompany As Company
    Private mstrFileNameRoot As String
    Private mblnLocalStylesheet As Boolean
    Private mobjBuilder As System.Text.StringBuilder
    Public blnUseMinusNumbers As Boolean

    Public Sub New(ByVal objCompany As Company, ByVal strFileNameRoot As String, ByVal blnLocalStylesheet As Boolean)
        mobjCompany = objCompany
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
        Dim strInputPath As String = Company.strExecutableFolder() + "\\Reports\\" + strFileName
        Dim strOutputPath As String = mobjCompany.strReportPath() + "\\" + strFileName
        System.IO.File.Copy(strInputPath, strOutputPath, True)
    End Sub

    Public Sub EndReport()
        OutputLine("</div>")
        OutputLine("</body>")
        OutputLine("</html>")
    End Sub

    Public Sub OutputHeader(ByVal strTitle As String, ByVal strSubTitle As String)
        OutputDiv("ReportCompanyName", mobjCompany.objInfo.strCompanyName)
        OutputDiv("ReportTitle", strTitle)
        OutputDiv("ReportSubTitle", strSubTitle)
        OutputDiv("ReportPrepared", "Prepared " + DateTime.Today.ToLongDateString())
    End Sub

    Public Sub OutputGroupItems(ByVal strTitleClass As String, ByVal strAmountClass As String,
                           ByVal strNegativeClass As String, ByVal objReportManager As ReportGroupManager,
                           ByVal strGroupKey As String, ByVal objAccum As ReportAccumulator)
        Dim objGroup As LineItemGroup = objReportManager.objGetGroup(strGroupKey)
        For Each objLine As ReportLineItem In objGroup.colItems
            If objLine.curTotal <> 0D Then
                OutputAmount(strTitleClass, objLine.strItemTitle, strAmountClass, strNegativeClass, objLine.curTotal, objAccum)
            End If
            objLine.blnPrinted = True
        Next
    End Sub

    Public Sub OutputGroupSummary(ByVal strTitleClass As String, ByVal strTitle As String, ByVal strAmountClass As String,
                           ByVal strNegativeClass As String, ByVal objReportManager As ReportGroupManager,
                           ByVal strGroupKey As String, ByVal blnOmitIfZero As Boolean, ByVal objAccum As ReportAccumulator)
        Dim objGroup As LineItemGroup = objReportManager.objGetGroup(strGroupKey)
        If objGroup.curGroupTotal <> 0D Or Not blnOmitIfZero Then
            OutputAmount(strTitleClass, strTitle, strAmountClass, strNegativeClass, objGroup.curGroupTotal, objAccum)
        End If
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

    Private Sub OutputLine(ByVal strLine As String)
        mobjBuilder.AppendLine(strLine)
    End Sub

    Public Sub ShowReport()
        Dim strReportFile As String = mobjCompany.strReportPath() + "\" + mstrFileNameRoot + ".html"
        Using objFile As System.IO.TextWriter = New System.IO.StreamWriter(strReportFile)
            objFile.Write(mobjBuilder.ToString())
        End Using
        Dim info As System.Diagnostics.ProcessStartInfo = New ProcessStartInfo(strReportFile)
        info.UseShellExecute = True
        System.Diagnostics.Process.Start(info)
    End Sub

    Public Sub CheckPrinted(ByVal objReportManager As ReportGroupManager)
        For Each objGroup As LineItemGroup In objReportManager.colGroups
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
