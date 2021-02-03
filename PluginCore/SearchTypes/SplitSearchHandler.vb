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
        strName = strName_
        dlgGetSplitData = dlgGetSplitData_
    End Sub

    Public ReadOnly Property strName As String _
        Implements ISearchHandler.strName

    Public Overrides Function ToString() As String
        Return Me.strName
    End Function

    Public Sub HandlerSelected(ByVal objHostSearchUI As IHostSearchUI) _
        Implements ISearchHandler.HandlerSelected
        objHostSearchUI.UseTextCriteria()
    End Sub

    Public Function blnPrepareSearch(ByVal objHostSearchUI As IHostSearchUI) As Boolean _
        Implements ISearchHandler.blnPrepareSearch
        objComparer = DirectCast(objHostSearchUI.objGetSearchType(), SearchComparer)
        strParameter = objHostSearchUI.strGetTextSearchFor()
        Return True
    End Function

    Public Sub ProcessTrx(
        ByVal objTrx As Trx,
        ByVal dlgAddTrxResult As AddSearchMatchTrxDelegate,
        ByVal dlgAddSplitResult As AddSearchMatchSplitDelegate) _
        Implements ISearchHandler.ProcessTrx

        If TypeOf (objTrx) Is NormalTrx Then
            For Each objSplit In DirectCast(objTrx, NormalTrx).colSplits
                If objComparer.blnCompare(dlgGetSplitData(objSplit), strParameter) Then
                    dlgAddSplitResult(DirectCast(objTrx, NormalTrx), objSplit)
                End If
            Next
        End If
    End Sub
End Class
