Option Strict On
Option Explicit On

Imports CheckBookLib
Imports System.IO

''' <summary>
''' All plugins that import Trx using ITrxReader and IImportHandler
''' inherit from this. This class is subclassed several different
''' ways for different types of Trx importers. The user interface checks
''' the subclass to determine which menu to add the plugin to.
''' </summary>

Public MustInherit Class TrxImportPlugin
    Implements IToolPlugin

    Public MustOverride Function GetImportWindowCaption() As String
    Public MustOverride Function GetTrxReader() As ITrxReader
    Public MustOverride Function GetImportHandler() As IImportHandler

    Protected ReadOnly HostUI As IHostUI

    Public Sub New(ByVal hostUI_ As IHostUI)
        HostUI = hostUI_
    End Sub

    Public MustOverride Function GetMenuTitle() As String Implements IToolPlugin.GetMenuTitle

    Public Overridable Function SortCode() As Integer Implements IToolPlugin.SortCode
        Return 10
    End Function

    Public Sub ClickHandler(sender As Object, e As EventArgs) Implements IToolPlugin.ClickHandler
        Try
            If BankImportForm.intOpenCount > 0 Then
                MsgBox("An import form is already open, and only one can be open at a time.")
                Exit Sub
            End If
            Dim objReader As ITrxReader = GetTrxReader()
            If Not objReader Is Nothing Then
                Using frm As BankImportAcctSelectForm = New BankImportAcctSelectForm()
                    Dim objImportHandler As IImportHandler = GetImportHandler()
                    objImportHandler.Init(HostUI)
                    frm.ShowMe(Me.HostUI, GetImportWindowCaption(), GetImportHandler(), objReader)
                End Using
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
