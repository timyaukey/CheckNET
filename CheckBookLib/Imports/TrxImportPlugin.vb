Option Explicit On
Option Strict On

Public MustInherit Class TrxImportPlugin
    Inherits ToolPlugin

    Public MustOverride Function GetImportWindowCaption() As String
    Public MustOverride Function GetTrxReader() As ITrxReader
    Public MustOverride Function GetImportHandler() As IImportHandler

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub


    Public Overrides Sub ClickHandler(sender As Object, e As EventArgs)

        Try
            If HostUI.blnImportFormAlreadyOpen() Then
                Exit Sub
            End If
            Dim objReader As ITrxReader = GetTrxReader()
            If Not objReader Is Nothing Then
                HostUI.OpenImportForm(GetImportWindowCaption, GetImportHandler(), objReader, False)
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try

    End Sub
End Class
