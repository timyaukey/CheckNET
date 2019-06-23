Option Strict On
Option Explicit On

Imports CheckBookLib

Public Delegate Function GetTrxSearchDataDelegate(ByVal objTrx As Trx) As String
Public Delegate Function GetSplitSearchDataDelegate(ByVal objSplit As TrxSplit) As String
Public Delegate Function SearchComparerDelegate(ByVal strTrxData As String, ByVal strSearchFor As String) As Boolean

Public Interface ISearchHandler
    ''' <summary>
    ''' Name to show for this search method in the UI.
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property strName As String

    ''' <summary>
    ''' Read and validate all search parameters. Called by the UI
    ''' when the user explicitly requests a search, not when the UI
    ''' re-executes a search in response to an event such as changes
    ''' to a register. Such a re-execution bypasses the call to this
    ''' method and ProcessTrx() must use the same parameters last
    ''' retrieved by blnPrepareSearch().
    ''' </summary>
    ''' <returns>Return true iff parameters are valid and search may proceed.</returns>
    Function blnPrepareSearch() As Boolean

    ''' <summary>
    ''' Evaluate one Trx for the search, and if the Trx meets the conditions
    ''' for being included in the search results then call one of the two
    ''' delegates to include it. One delegate is used to report the Trx as
    ''' a whole, the other to report a single split in the Trx.
    ''' </summary>
    ''' <param name="objTrx"></param>
    ''' <param name="dlgAddTrxResult"></param>
    ''' <param name="dlgAddSplitResult"></param>
    Sub ProcessTrx(
        ByVal objTrx As Trx,
        ByVal dlgAddTrxResult As Trx.AddSearchMaxTrxDelegate,
        ByVal dlgAddSplitResult As Trx.AddSearchMaxSplitDelegate)
End Interface
