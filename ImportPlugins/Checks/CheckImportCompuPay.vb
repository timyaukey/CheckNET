Option Strict On
Option Explicit On

Public Class CheckImportCompuPay
    Inherits CheckImportBuilder

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Build(ByVal setup As IHostSetup)
        setup.CheckImportMenu.Add(New MenuElementAction("CompuPay Payroll Clipboard", StandardSortCode(), AddressOf ClickHandler))
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import CompuPay Payroll Checks"
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadChecks(Utilities.GetClipboardReader(), "(clipboard)", New ReadChecksSpec(0, 5, 9, 12, -1))
    End Function
End Class
