Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class SearchExportTool
    Implements ISearchTool

    Private mobjHostUI As IHostUI

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property strTitle As String Implements ISearchTool.strTitle
        Get
            Return "Export"
        End Get
    End Property

    Public Sub Run(objHostSearchToolUI As IHostSearchToolUI) Implements ISearchTool.Run
        Using frmExport As ExportForm = New ExportForm
            Dim objTrx As Trx
            Dim colSplits As IEnumerable(Of TrxSplit)
            Dim objSplit As TrxSplit
            Dim lngExportCount As Integer

            If Not frmExport.blnGetSettings(mobjHostUI) Then
                mobjHostUI.InfoMessageBox("Export canceled.")
                Exit Sub
            End If

            frmExport.OpenOutput()

            lngExportCount = 0
            For Each objTrx In objHostSearchToolUI.objAllSelectedTrx()
                'Ignore budgets and transfers instead of showing an error, because
                'it is common to export all trx in a date range except these.
                If TypeOf objTrx Is NormalTrx Then
                    colSplits = DirectCast(objTrx, NormalTrx).colSplits
                    For Each objSplit In colSplits
                        frmExport.WriteSplit(objTrx, objSplit)
                        lngExportCount = lngExportCount + 1
                    Next objSplit
                End If
            Next

            frmExport.CloseOutput()
            frmExport.Close()

            mobjHostUI.InfoMessageBox(lngExportCount & " splits exported.")

            Exit Sub
        End Using
    End Sub
End Class
