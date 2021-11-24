Option Strict On
Option Explicit On

Public Class InvoiceImportStandard
    Inherits InvoiceImportBuilder

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Build(ByVal setup As IHostSetup)
        setup.InvoiceImportMenu.Add(New MenuElementAction("Standard Clipboard", 1, AddressOf ClickHandler))
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import Invoices"
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadInvoices(HostUI.Company, Utilities.GetClipboardReader(), "(clipboard)")
    End Function
End Class
