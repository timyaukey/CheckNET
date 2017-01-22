Option Strict On
Option Explicit On

Imports System.IO

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

    Protected Function objAskForFile(ByVal strWindowCaption As String, ByVal strFileType As String,
        ByVal strSettingsKey As String) As InputFile

        Dim strFile As String = HostUI.strChooseFile(strWindowCaption, strFileType, strSettingsKey)
        If strFile <> "" Then
            Return New InputFile(New StreamReader(strFile), strFile)
        End If
        Return Nothing
    End Function

    Public Class InputFile
        Public ReadOnly objFile As TextReader
        Public ReadOnly strFile As String

        Public Sub New(ByVal objFile_ As TextReader, ByVal strFile_ As String)
            objFile = objFile_
            strFile = strFile_
        End Sub
    End Class

End Class
