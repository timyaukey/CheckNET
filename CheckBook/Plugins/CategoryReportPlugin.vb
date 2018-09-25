Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class CategoryReportPlugin
    Inherits ReportPlugin

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub ClickHandler(sender As Object, e As EventArgs)
        Try
            Dim frmRpt As RptScanSplitsForm = New RptScanSplitsForm
            frmRpt.ShowMe(RptScanSplitsForm.SplitReportType.Totals, Me.HostUI)
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Overrides Function GetMenuTitle() As String
        Return "Totals By Category"
    End Function

    Public Overrides Function SortCode() As Integer
        Return 2
    End Function
End Class
