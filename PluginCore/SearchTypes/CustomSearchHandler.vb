Option Strict On
Option Explicit On

Public MustInherit Class CustomSearchHandler
    Implements ISearchHandler

    Protected mobjHostUI As IHostUI
    Protected objComparer As SearchComparer
    Protected strParameter As String

    Public Sub New(
        ByVal objHostUI_ As IHostUI,
        ByVal strName_ As String)

        mobjHostUI = objHostUI_
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
        objComparer = DirectCast(objHostSearchUI.objGetSearchType(), SearchComparer)
        strParameter = objHostSearchUI.strGetTextSearchFor()
        Return True
    End Function

    Public MustOverride Sub ProcessTrx(
        ByVal objTrx As BaseTrx,
        ByVal dlgAddTrxResult As AddSearchMatchTrxDelegate,
        ByVal dlgAddSplitResult As AddSearchMatchSplitDelegate) _
        Implements ISearchHandler.ProcessTrx

End Class
