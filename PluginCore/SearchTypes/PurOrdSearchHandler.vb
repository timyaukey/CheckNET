Option Strict On
Option Explicit On

Public Class PurOrdSearchHandler
    Inherits CustomSearchHandler

    Public Sub New(
        ByVal objHostUI_ As IHostUI,
        ByVal strName_ As String)
        MyBase.New(objHostUI_, strName_)

    End Sub

    Public Overrides Sub ProcessTrx(
        objTrx As BaseTrx,
        dlgAddTrxResult As AddSearchMatchTrxDelegate,
        dlgAddSplitResult As AddSearchMatchSplitDelegate)

        If TypeOf (objTrx) Is BankTrx Then
            For Each objSplit In DirectCast(objTrx, BankTrx).Splits
                If Comparer.Compare(objSplit.PONumber, SearchParam) Then
                    dlgAddSplitResult(DirectCast(objTrx, BankTrx), objSplit)
                End If
            Next
        ElseIf TypeOf (objTrx) Is ReplicaTrx Then
            If Comparer.Compare(DirectCast(objTrx, ReplicaTrx).PONumber, SearchParam) Then
                dlgAddTrxResult(objTrx)
            End If
        End If
    End Sub
End Class
