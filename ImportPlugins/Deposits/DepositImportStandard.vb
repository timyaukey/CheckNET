Option Strict On
Option Explicit On

Imports CheckBookLib
Imports PluginCore

Public Class DepositImportStandard
    Inherits DepositImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register()
        HostUI.objDepositImportMenu.Add(New MenuElementAction("Standard Clipboard", 1, AddressOf ClickHandler, GetPluginPath()))
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import Deposit Amounts"
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadDeposits(Utilities.objClipboardReader(), "(clipboard)")
    End Function
End Class
