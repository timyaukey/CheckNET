Option Strict On
Option Explicit On

Imports CheckBookLib

Public Interface ISearchTool
    ReadOnly Property strTitle() As String
    Sub Run(ByVal objHostSearchToolUI As IHostSearchToolUI)
End Interface
