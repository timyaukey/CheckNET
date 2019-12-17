Option Strict On
Option Explicit On


Public Class CheckImportCompuPay
    Inherits CheckImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register()
        HostUI.objCheckImportMenu.Add(New MenuElementAction("CompuPay Payroll Clipboard", StandardSortCode(), AddressOf ClickHandler, GetPluginPath()))
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import CompuPay Payroll Checks"
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadChecks(Utilities.objClipboardReader(), "(clipboard)", New ReadChecksSpec(0, 5, 9, 12, -1))
    End Function
End Class
