Option Strict On
Option Explicit On

<Assembly: PluginAssembly()>

Public Class TrxImportPlugin
    Inherits PluginBase

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register(setup As IHostSetup)
        Dim x As TrxImportBuilder
        x = New BankImportOFX(HostUI) : x.Build(setup)
        x = New BankImportQIF(HostUI) : x.Build(setup)
        x = New CheckImportCompuPay(HostUI) : x.Build(setup)
        x = New CheckImportInsight(HostUI) : x.Build(setup)
        x = New CheckImportStandard(HostUI) : x.Build(setup)
        x = New DepositImportStandard(HostUI) : x.Build(setup)
        x = New InvoiceImportStandard(HostUI) : x.Build(setup)

        MetadataInternal = New PluginMetadata("Transaction Import Handlers", "Willow Creek Software",
            Reflection.Assembly.GetExecutingAssembly(), Nothing, "", Nothing)
    End Sub

End Class
