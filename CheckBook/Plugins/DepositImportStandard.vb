Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class DepositImportStandard
    Inherits DepositImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import Deposit Amounts"
    End Function

    Public Overrides Function GetMenuTitle() As String
        Return "Standard Clipboard"
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadDeposits(gobjClipboardReader(), "(clipboard)")
    End Function
End Class
