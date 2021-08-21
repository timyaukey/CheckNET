Option Strict On
Option Explicit On

''' <summary>
''' Implemented by search window to expose functionality
''' to ISearchHandler implementations.
''' </summary>

Public Interface IHostSearchUI
    Sub UseTextCriteria()
    Sub UseComboBoxCriteria(ByVal objChoices As IEnumerable(Of Object))
    Function GetTextSearchFor() As String
    Function GetComboBoxSearchFor() As Object
    Function GetSearchType() As Object
End Interface
