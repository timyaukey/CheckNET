Option Strict On
Option Explicit On


Public Delegate Function GetTrxSearchDataDelegate(ByVal objTrx As BaseTrx) As String
Public Delegate Function GetSplitSearchDataDelegate(ByVal objSplit As TrxSplit) As String

Public Delegate Sub AddSearchMatchTrxDelegate(ByVal objTrx As BaseTrx)
Public Delegate Sub AddSearchMatchSplitDelegate(ByVal objTrx As BaseTrx, ByVal objSplit As TrxSplit)

Public Interface ISearchHandler
    ''' <summary>
    ''' Name to show for this search method in the UI.
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property Name As String

    ''' <summary>
    ''' Called with the user picks this handler in the search user interface.
    ''' Is responsible for configuring the UI for the selected type of search.
    ''' </summary>
    ''' <param name="objHostSearchUI"></param>
    Sub HandlerSelected(ByVal objHostSearchUI As IHostSearchUI)

    ''' <summary>
    ''' Read and validate all search parameters. Called by the UI
    ''' when the user explicitly requests a search, not when the UI
    ''' re-executes a search in response to an event such as changes
    ''' to a register. Such a re-execution bypasses the call to this
    ''' method and ProcessTrx() must use the same parameters last
    ''' retrieved by blnPrepareSearch().
    ''' </summary>
    ''' <returns>Return true iff parameters are valid and search may proceed.</returns>
    Function PrepareSearch(ByVal objHostSearchUI As IHostSearchUI) As Boolean

    ''' <summary>
    ''' Evaluate one BaseTrx for the search, and if the BaseTrx meets the conditions
    ''' for being included in the search results then call one of the two
    ''' delegates to include it. One delegate is used to report the BaseTrx as
    ''' a whole, the other to report a single split in the BaseTrx.
    ''' </summary>
    ''' <param name="objTrx"></param>
    ''' <param name="dlgAddTrxResult"></param>
    ''' <param name="dlgAddSplitResult"></param>
    Sub ProcessTrx(
        ByVal objTrx As BaseTrx,
        ByVal dlgAddTrxResult As AddSearchMatchTrxDelegate,
        ByVal dlgAddSplitResult As AddSearchMatchSplitDelegate)
End Interface
