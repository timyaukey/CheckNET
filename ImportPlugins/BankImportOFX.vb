Option Strict On
Option Explicit On

Imports CheckBookLib
Imports System.IO

Public Class BankImportOFX
    Inherits BankImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import OFX File From Bank"
    End Function

    Public Overrides Function GetMenuTitle() As String
        Return "OFX File"
    End Function

    Protected Overrides Function GetFileSelectionWindowCaption() As String
        Return "Select OFX File From Bank To Import"
    End Function

    Protected Overrides Function GetFileType() As String
        Return "OFX"
    End Function

    Protected Overrides Function GetSettingsKey() As String
        Return "BankOFXPath"
    End Function

    Protected Overrides Function GetBankReader(objFile As TextReader, strFile As String) As ITrxReader
        Return New ReadBankOFX(objFile, strFile)
    End Function

End Class
