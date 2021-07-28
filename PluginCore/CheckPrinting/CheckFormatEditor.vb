Option Strict On
Option Explicit On

Imports System.Xml.Serialization

Public Class CheckFormatEditor
    Private mobjHostUI As IHostUI
    Private mobjCompany As Company
    Private mobjFormat As CheckFormat

    Public Sub ShowMe(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
        mobjCompany = mobjHostUI.objCompany
        Me.ShowDialog()
    End Sub

    Private Sub CheckFormatEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim ser As XmlSerializer = New XmlSerializer(GetType(CheckFormat))
            Using inputStream As System.IO.FileStream = New IO.FileStream(mobjCompany.CheckFormatFilePath(), IO.FileMode.Open)
                mobjFormat = DirectCast(ser.Deserialize(inputStream), CheckFormat)
                prpDetails.SelectedObject = mobjFormat
            End Using
        Catch ex As System.IO.FileNotFoundException
            prpDetails.SelectedObject = New CheckFormat()
            mobjHostUI.InfoMessageBox("Creating default check format. You will need to fix it to match your check stock, and then save it.")
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(sender As Object, e As EventArgs) Handles cmdOkay.Click
        Try
            Dim ser As XmlSerializer = New XmlSerializer(GetType(CheckFormat))
            Using outputStream As System.IO.FileStream = New IO.FileStream(mobjCompany.CheckFormatFilePath(), IO.FileMode.Create)
                ser.Serialize(outputStream, prpDetails.SelectedObject)
                Me.Close()
            End Using
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub
End Class