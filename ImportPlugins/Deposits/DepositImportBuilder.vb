Option Strict On
Option Explicit On


''' <summary>
''' All deposit import builders must inherit from this.
''' </summary>

Public MustInherit Class DepositImportBuilder
    Inherits TrxImportBuilder

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportHandler() As IImportHandler
        Return New ImportHandlerDeposits()
    End Function

End Class
