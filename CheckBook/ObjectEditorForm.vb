Option Explicit On
Option Strict On

Imports CheckBookLib

Public Class ObjectEditorForm

    Private mobjData As IFilePersistable

    Public Function ShowEditor(ByVal objData As IFilePersistable, ByVal strTitle As String) As Boolean
        Dim result As DialogResult
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
            MsgBox(validationError, MsgBoxStyle.OkOnly)
            Return
        End If
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
End Class