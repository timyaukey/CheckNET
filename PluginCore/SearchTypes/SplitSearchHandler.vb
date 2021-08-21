Option Strict On
Option Explicit On

Public Class SplitSearchHandler
    Implements ISearchHandler

    Private mobjHostUI As IHostUI
    Private dlgGetSplitData As GetSplitSearchDataDelegate
    Private objComparer As SearchComparer
    Private strParameter As String

    Public Sub New(
        ByVal objHostUI_ As IHostUI,
        ByVal strName_ As String,
        ByVal dlgGetSplitData_ As GetSplitSearchDataDelegate)

        mobjHostUI = objHostUI_
        Name = strName_
        dlgGetSplitData = dlgGetSplitData_
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

    Public Sub ProcessTrx(
        ByVal objTrx As BaseTrx,
        ByVal dlgAddTrxResult As AddSearchMatchTrxDelegate,
        ByVal dlgAddSplitResult As AddSearchMatchSplitDelegate) _
        Implements ISearchHandler.ProcessTrx

        If TypeOf (objTrx) Is BankTrx Then
            For Each objSplit In DirectCast(objTrx, BankTrx).Splits
                If objComparer.Compare(dlgGetSplitData(objSplit), strParameter) Then
                    dlgAddSplitResult(DirectCast(objTrx, BankTrx), objSplit)
                End If
            Next
        End If
    End Sub
End Class
