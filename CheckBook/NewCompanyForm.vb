Option Strict On
Option Explicit On

Public Class NewCompanyForm
    Private mobjHostUI As IHostUI
    Public strCompanyName As String

    Public Function ShowNewCompanyDialog(ByVal objHostUI As IHostUI) As DialogResult
        mobjHostUI = objHostUI
        Return ShowDialog()
    End Function

    Private Sub btnOkay_Click(sender As Object, e As EventArgs) Handles btnOkay.Click
        If String.IsNullOrEmpty(txtCompanyName.Text) Then
            mobjHostUI.ErrorMessageBox("A company name is required.")
            Return
        End If
        Dim regex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex("^[a-zA-Z0-9][a-zA-Z0-9 \-_.]+$")
        If Not regex.IsMatch(txtCompanyName.Text) Then
            mobjHostUI.ErrorMessageBox("Company name must start with a letter or number, and may only " +
                "include letters, numbers, spaces, dashes, underscores and periods.")
            Return
        End If
        strCompanyName = txtCompanyName.Text
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub
End Class