Option Strict On
Option Explicit On

Public Class TrxSearchHandler
    Implements ISearchHandler

    Private mobjHostUI As IHostUI
    Private dlgGetTrxData As GetTrxSearchDataDelegate
    Private objComparer As SearchComparer
    Private strParameter As String

    Public Sub New(
        ByVal objHostUI_ As IHostUI,
        ByVal strName_ As String,
        ByVal dlgGetTrxData_ As GetTrxSearchDataDelegate)

        mobjHostUI = objHostUI_
        Name = strName_
        dlgGetTrxData = dlgGetTrxData_
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
        objComparer = DirectCast(objHostSearchUI.GetSearchType(), SearchComparer)
        strParameter = objHostSearchUI.GetTextSearchFor()
        Return True
    End Function

    Public Sub ProcessTrx(
        ByVal objTrx As BaseTrx,
        ByVal dlgAddTrxResult As AddSearchMatchTrxDelegate,
        ByVal dlgAddSplitResult As AddSearchMatchSplitDelegate) _
        Implements ISearchHandler.ProcessTrx

        If objComparer.Compare(dlgGetTrxData(objTrx), strParameter) Then
            dlgAddTrxResult(objTrx)
        End If
    End Sub
End Class
