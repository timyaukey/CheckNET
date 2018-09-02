Option Strict On
Option Explicit On

Imports CheckBookLib
Imports System.IO

Public Class BankImportQIF
    Inherits BankImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import QIF File From Bank"
    End Function

    Public Overrides Function GetMenuTitle() As String
        Return "QIF File"
    End Function

    Protected Overrides Function GetFileSelectionWindowCaption() As String
        Return "Select QIF File From Bank To Import"
    End Function

    Protected Overrides Function GetFileType() As String
        Return "QIF"
    End Function

    Protected Overrides Function GetSettingsKey() As String
        Return "BankQIFPath"
    End Function

    Protected Overrides Function GetBankReader(objFile As TextReader, strFile As String) As ITrxReader
        Return New ReadBankQIF(objFile, strFile)
    End Function

End Class
