Option Explicit On
Option Strict On

Public MustInherit Class BankImportPlugin
    Inherits TrxImportPlugin

    Protected MustOverride Function GetFileSelectionWindowCaption() As String
    Protected MustOverride Function GetFileType() As String
    Protected MustOverride Function GetSettingsKey() As String
    Protected MustOverride Function GetBankReader(ByVal objFile As System.IO.TextReader, ByVal strFile As String) As ITrxReader

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportHandler() As IImportHandler
        Return New ImportHandlerBank()
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Dim strFile As String = HostUI.strChooseFile(GetFileSelectionWindowCaption(), GetFileType(), GetSettingsKey())
        If strFile <> "" Then
            Dim objFile As System.IO.TextReader = New System.IO.StreamReader(strFile)
            Return GetBankReader(objFile, strFile)
        End If
        Return Nothing
    End Function
End Class
