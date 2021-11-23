Option Strict On
Option Explicit On


Public Class CategoryReportPlugin
    Inherits PluginBase

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register(ByVal setup As IHostSetup)
        setup.ReportMenu.Add(New MenuElementAction("Totals By Category", 200, AddressOf ClickHandler))
        MetadataInternal = New PluginMetadata("Category Report", "Willow Creek Software",
            Reflection.Assembly.GetExecutingAssembly(), Nothing, "", Nothing)
    End Sub

    Private Sub ClickHandler(sender As Object, e As EventArgs)
        Try
            Dim frmRpt As RptScanSplitsForm = New RptScanSplitsForm
            frmRpt.ShowMe(RptScanSplitsForm.SplitReportType.Totals, Me.HostUI)
            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub
End Class
