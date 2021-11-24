Option Strict On
Option Explicit On


''' <summary>
''' All invoice import builders must inherit from this.
''' </summary>

Public MustInherit Class InvoiceImportBuilder
    Inherits TrxImportBuilder

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportHandler() As IImportHandler
        Return New ImportHandlerInvoices()
    End Function

End Class
