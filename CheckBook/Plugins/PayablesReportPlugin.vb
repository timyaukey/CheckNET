Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class PayablesReportPlugin
    Inherits ReportPlugin

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
        SortCode = 1
    End Sub

    Public Overrides Sub ClickHandler(sender As Object, e As EventArgs)
        Try
            Dim frmRpt As RptScanSplitsForm = New RptScanSplitsForm
            frmRpt.ShowMe(RptScanSplitsForm.SplitReportType.glngSPLTRPT_PAYABLES)
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Overrides Function GetMenuTitle() As String
        Return "Accounts Payable"
    End Function
End Class
