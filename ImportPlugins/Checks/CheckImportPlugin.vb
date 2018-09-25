Option Strict On
Option Explicit On

Imports CheckBookLib

''' <summary>
''' All check import plugins must inherit from this.
''' </summary>

Public MustInherit Class CheckImportPlugin
    Inherits TrxImportPlugin
    Implements ICheckImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportHandler() As IImportHandler
        Return New ImportHandlerChecks()
    End Function

End Class
