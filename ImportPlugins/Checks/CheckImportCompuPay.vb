Option Strict On
Option Explicit On

Imports CheckBookLib
Imports PluginCore

Public Class CheckImportCompuPay
    Inherits CheckImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import CompuPay Payroll Checks"
    End Function

    Public Overrides Function GetMenuTitle() As String
        Return "CompuPay Payroll Clipboard"
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadChecks(Utilities.objClipboardReader(), "(clipboard)", New ReadChecksSpec(0, 5, 9, 12, -1))
    End Function
End Class
