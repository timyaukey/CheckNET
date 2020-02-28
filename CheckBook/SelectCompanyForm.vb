Option Strict On
Option Explicit On

Public Class SelectCompanyForm
    Public strDataPath As String

    Private mobjHostUI As IHostUI
    Private mobjShowMessage As Action(Of String)

    Public Function ShowCompanyDialog(ByVal hostUI As IHostUI, ByVal objShowMessage As Action(Of String)) As DialogResult
        mobjHostUI = hostUI
        mobjShowMessage = objShowMessage

        Dim strDataPathValue As String = System.Configuration.ConfigurationManager.AppSettings("DataPath")
        If String.IsNullOrEmpty(strDataPathValue) Then
            strDataPathValue = Company.strExecutableFolder() & "\Data"
        End If

        If Not Company.blnDataPathExists(strDataPathValue) Then
            Dim objCreateCompany As Company = New Company(strDataPathValue)
            objCreateCompany.CreateInitialData(mobjShowMessage)
        End If

        strDataPath = strDataPathValue
        'Return ShowDialog()
        Return DialogResult.OK
    End Function

    Private Sub btnOpen_Click(sender As Object, e As EventArgs) Handles btnOpen.Click

        Me.DialogResult = DialogResult.OK
        Me.Close()

    End Sub
End Class