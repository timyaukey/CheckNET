Option Strict On
Option Explicit On

''' <summary>
''' Implemented by host program search window to expose functionality
''' to ISearchHandler implementations.
''' </summary>

Public Interface IHostSearchUI
    Sub UseTextCriteria()
    Sub UseComboBoxCriteria(ByVal objChoices As IEnumerable(Of Object))
    Function strGetTextSearchFor() As String
    Function objGetComboBoxSearchFor() As Object
    Function objGetSearchType() As Object
End Interface
