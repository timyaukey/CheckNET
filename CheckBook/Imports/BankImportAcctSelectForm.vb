Option Strict Off
Option Explicit On

Imports CheckBookLib

Friend Class BankImportAcctSelectForm
    Inherits System.Windows.Forms.Form

    Private mobjTrxImport As ITrxImport
    Private mobjAccount As Account

    Public Sub ShowMe(ByVal strTitle As String, ByVal objTrxImport As ITrxImport, ByVal lngStatusSearchType As CBMain.ImportStatusSearch, ByVal lngUpdateSearchType As CBMain.ImportBatchUpdateSearch, ByVal lngNewSearchType As CBMain.ImportBatchNewSearch, ByVal lngIndividualUpdateType As CBMain.ImportIndividualUpdateType, ByVal lngIndividualSearchType As CBMain.ImportIndividualSearchType, ByVal lngBatchUpdateType As CBMain.ImportBatchUpdateType, ByVal blnFake As Boolean)

        Dim frm As BankImportForm
        Try
            mobjTrxImport = objTrxImport
            'UPGRADE_NOTE: Object mobjAccount may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            mobjAccount = Nothing
            gLoadAccountListBox(lstAccounts)
            Me.ShowDialog()
            System.Windows.Forms.Application.DoEvents()
            If Not mobjAccount Is Nothing Then
                frm = New BankImportForm
                frm.ShowMe(strTitle, mobjAccount, mobjTrxImport, lngStatusSearchType, lngUpdateSearchType, lngNewSearchType, lngIndividualUpdateType, lngIndividualSearchType, lngBatchUpdateType, blnFake)
            End If
            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        Try

            mobjAccount = gobjGetSelectedAccountAndUnload(lstAccounts, Me)
            If mobjAccount Is Nothing Then
                MsgBox("Please select the account to import into.", MsgBoxStyle.Critical)
                Exit Sub
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub lstAccounts_DoubleClick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles lstAccounts.DoubleClick
        cmdOkay_Click(cmdOkay, New System.EventArgs())
    End Sub
End Class