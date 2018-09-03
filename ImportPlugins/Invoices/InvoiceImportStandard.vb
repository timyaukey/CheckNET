Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class InvoiceImportStandard
    Inherits InvoiceImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import Invoices"
    End Function

    Public Overrides Function GetMenuTitle() As String
        Return "Standard Clipboard"
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadInvoices(HostUI.objCompany, Utilities.objClipboardReader(), "(clipboard)")
    End Function
End Class
