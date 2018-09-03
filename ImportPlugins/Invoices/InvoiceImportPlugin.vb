Option Strict On
Option Explicit On

Imports CheckBookLib

''' <summary>
''' All invoice import plugins must inherit from this.
''' </summary>

Public MustInherit Class InvoiceImportPlugin
    Inherits TrxImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportHandler() As IImportHandler
        Return New ImportHandlerInvoices()
    End Function

End Class
