Option Strict On
Option Explicit On

Public Class DepositImportStandard
    Inherits DepositImportBuilder

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Build(ByVal setup As IHostSetup)
        setup.DepositImportMenu.Add(New MenuElementAction("Standard Clipboard", 1, AddressOf ClickHandler))
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import Deposit Amounts"
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadDeposits(Utilities.GetClipboardReader(), "(clipboard)")
    End Function
End Class
