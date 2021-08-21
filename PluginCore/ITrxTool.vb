Option Strict On
Option Explicit On

Public Interface ITrxTool
    ReadOnly Property Title() As String
    Sub Run(ByVal objHostTrxToolUI As IHostTrxToolUI)
End Interface
