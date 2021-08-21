Option Strict On
Option Explicit On

Public Interface ISearchTool
    ReadOnly Property Title() As String
    Sub Run(ByVal objHostSearchToolUI As IHostSearchToolUI)
End Interface
