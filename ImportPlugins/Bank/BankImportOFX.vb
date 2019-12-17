Option Strict On
Option Explicit On

Imports System.IO

Public Class BankImportOFX
    Inherits BankImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register()
        HostUI.objBankImportMenu.Add(New MenuElementAction("OFX File", StandardSortCode(), AddressOf ClickHandler, GetPluginPath()))
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import OFX File From Bank"
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Dim objInput As TrxImportPlugin.InputFile = objAskForFile("Select OFX File From Bank To Import", "OFX", "BankOFXPath")
        If Not objInput Is Nothing Then
            Return New ReadBankOFX(HostUI, objInput.objFile, objInput.strFile)
        End If
        Return Nothing
    End Function

End Class
