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
        objTrx As Trx,
        dlgAddTrxResult As AddSearchMatchTrxDelegate,
        dlgAddSplitResult As AddSearchMatchSplitDelegate)

        If TypeOf (objTrx) Is NormalTrx Then
            For Each objSplit In DirectCast(objTrx, NormalTrx).colSplits
                If objComparer.blnCompare(objSplit.strPONumber, strParameter) Then
                    dlgAddSplitResult(DirectCast(objTrx, NormalTrx), objSplit)
                End If
            Next
        ElseIf TypeOf (objTrx) Is ReplicaTrx Then
            If objComparer.blnCompare(DirectCast(objTrx, ReplicaTrx).strPONumber, strParameter) Then
                dlgAddTrxResult(objTrx)
            End If
        End If
    End Sub
End Class
