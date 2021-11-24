Option Strict On
Option Explicit On


''' <summary>
''' All check import builders must inherit from this.
''' </summary>

Public MustInherit Class CheckImportBuilder
    Inherits TrxImportBuilder

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportHandler() As IImportHandler
        Return New ImportHandlerChecks()
    End Function

End Class
