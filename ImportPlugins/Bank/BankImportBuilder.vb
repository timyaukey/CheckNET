Option Strict On
Option Explicit On


''' <summary>
''' All bank import builders must inherit from this.
''' </summary>

Public MustInherit Class BankImportBuilder
    Inherits TrxImportBuilder

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportHandler() As IImportHandler
        Return New ImportHandlerBank()
    End Function

End Class
