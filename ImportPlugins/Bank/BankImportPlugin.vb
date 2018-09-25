Option Strict On
Option Explicit On

Imports CheckBookLib

''' <summary>
''' All bank import plugins must inherit from this.
''' </summary>

Public MustInherit Class BankImportPlugin
    Inherits TrxImportPlugin
    Implements IBankImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportHandler() As IImportHandler
        Return New ImportHandlerBank()
    End Function

End Class
