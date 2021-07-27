Option Explicit On
Option Strict On


Public Class ObjectEditorForm

    Private mobjHostUI As IHostUI
    Private mobjData As IFilePersistable

    Public Function ShowEditor(ByVal objHostUI As IHostUI, ByVal objData As IFilePersistable, ByVal strTitle As String) As Boolean
        Dim result As DialogResult
        mobjHostUI = objHostUI
        StringTranslatorUIEditor.Company = mobjHostUI.objCompany
        mobjData = objData
        grdData.SelectedObject = mobjData
        Me.Text = strTitle
        result = Me.ShowDialog()
        Return result = DialogResult.OK
    End Function

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim validationError As String = mobjData.Validate()
        If Not validationError Is Nothing Then
            mobjHostUI.InfoMessageBox(validationError)
            Return
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class