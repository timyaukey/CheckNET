Option Explicit On
Option Strict On

Public MustInherit Class CheckImportPlugin
    Inherits TrxImportPlugin

    Protected MustOverride Function GetCheckSpecs() As ReadChecksSpec

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportHandler() As IImportHandler
        Return New ImportHandlerChecks()
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadChecks(gobjClipboardReader(), "(clipboard)", GetCheckSpecs())
    End Function
End Class
