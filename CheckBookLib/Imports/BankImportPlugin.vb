Option Strict On
Option Explicit On

Imports System.IO

''' <summary>
''' All bank import plugins must inherit from this.
''' </summary>

Public MustInherit Class BankImportPlugin
    Inherits TrxImportPlugin

    Protected MustOverride Function GetFileSelectionWindowCaption() As String
    Protected MustOverride Function GetFileType() As String
    Protected MustOverride Function GetSettingsKey() As String
    Protected MustOverride Function GetBankReader(ByVal objFile As TextReader, ByVal strFile As String) As ITrxReader

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportHandler() As IImportHandler
        Return New ImportHandlerBank()
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Dim objInput As TrxImportPlugin.InputFile = objAskForFile(GetFileSelectionWindowCaption(), GetFileType(), GetSettingsKey())
        If Not objInput Is Nothing Then
            Return GetBankReader(objInput.objFile, objInput.strFile)
        End If
        Return Nothing
    End Function
End Class
