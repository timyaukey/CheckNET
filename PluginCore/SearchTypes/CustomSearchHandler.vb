Option Strict On
Option Explicit On

Public MustInherit Class CustomSearchHandler
    Implements ISearchHandler

    Protected HostUI As IHostUI
    Protected Comparer As SearchComparer
    Protected SearchParam As String

    Public Sub New(
        ByVal objHostUI_ As IHostUI,
        ByVal strName_ As String)

        HostUI = objHostUI_
        Name = strName_
    End Sub

    Public ReadOnly Property Name As String _
        Implements ISearchHandler.Name

    Public Overrides Function ToString() As String
        Return Me.Name
    End Function

    Public Sub HandlerSelected(ByVal objHostSearchUI As IHostSearchUI) _
        Implements ISearchHandler.HandlerSelected
        objHostSearchUI.UseTextCriteria()
    End Sub

    Public Function PrepareSearch(ByVal objHostSearchUI As IHostSearchUI) As Boolean _
        Implements ISearchHandler.PrepareSearch
        Comparer = DirectCast(objHostSearchUI.GetSearchType(), SearchComparer)
        SearchParam = objHostSearchUI.GetTextSearchFor()
        Return True
    End Function

    Public MustOverride Sub ProcessTrx(
        ByVal objTrx As BaseTrx,
        ByVal dlgAddTrxResult As AddSearchMatchTrxDelegate,
        ByVal dlgAddSplitResult As AddSearchMatchSplitDelegate) _
        Implements ISearchHandler.ProcessTrx

End Class
