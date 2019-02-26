Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class CategoryReportPlugin
    Inherits ToolPlugin

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register()
        HostUI.objReportMenu.Add(New MenuElementAction("Totals By Category", 2, AddressOf ClickHandler, GetPluginPath()))
    End Sub

    Private Sub ClickHandler(sender As Object, e As EventArgs)
        Try
            Dim frmRpt As RptScanSplitsForm = New RptScanSplitsForm
            frmRpt.ShowMe(RptScanSplitsForm.SplitReportType.Totals, Me.HostUI)
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub
End Class
