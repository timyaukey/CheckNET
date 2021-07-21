Option Strict On
Option Explicit On

Imports System.Xml.Serialization

Public Class CompanyInfoEditor
    Private mobjCompany As Company

    Public Sub ShowMe(ByVal objCompany As Company)
        mobjCompany = objCompany
        Me.ShowDialog()
    End Sub

    Private Sub CompanyInfoEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        prpDetails.SelectedObject = mobjCompany.Info.Clone()
    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(sender As Object, e As EventArgs) Handles cmdOkay.Click
        Try
            Dim ser As XmlSerializer = New XmlSerializer(GetType(CompanyInfo))
            Using outputStream As System.IO.FileStream = New IO.FileStream(mobjCompany.CompanyInfoFilePath(), IO.FileMode.Create)
                ser.Serialize(outputStream, prpDetails.SelectedObject)
                mobjCompany.Info = DirectCast(prpDetails.SelectedObject, CompanyInfo)
                Me.Close()
            End Using
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub
End Class