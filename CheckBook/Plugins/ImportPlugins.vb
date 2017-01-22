Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class ImportPlugins
    Implements IPluginFactory

    Public Iterator Function colGetPlugins(hostUI_ As IHostUI) As IEnumerable(Of ToolPlugin) _
        Implements IPluginFactory.colGetPlugins

        Yield New BankImportOFX(hostUI_)
        Yield New BankImportQIF(hostUI_)
        Yield New CheckImportStandard(hostUI_)
        Yield New CheckImportInsight(hostUI_)
        Yield New CheckImportCompuPay(hostUI_)
        Yield New DepositImportStandard(hostUI_)
        Yield New InvoiceImportStandard(hostUI_)
    End Function
End Class
