Option Strict On
Option Explicit On

Public Class MemoSearchHandler
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

        If objComparer.blnCompare(objTrx.Memo, strParameter) Then
            dlgAddTrxResult(objTrx)
        End If
        If TypeOf (objTrx) Is BankTrx Then
            For Each objSplit In DirectCast(objTrx, BankTrx).Splits
                If objComparer.blnCompare(objSplit.Memo, strParameter) Then
                    dlgAddSplitResult(DirectCast(objTrx, BankTrx), objSplit)
                End If
            Next
        End If
    End Sub
End Class
