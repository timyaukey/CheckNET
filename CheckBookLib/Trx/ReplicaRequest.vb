Option Strict On
Option Explicit On

Public Class ReplicaRequest
    Private mobjSplit As TrxSplit
    Private mobjBankTrx As BankTrx
    Private mobjTargetReg As Register
    Private mobjRepMgr As ReplicaTrxManager

    Public Sub New(ByVal objSplit As TrxSplit, ByVal objTargetReg_ As Register)
        mobjSplit = objSplit
        mobjBankTrx = objSplit.Parent
        mobjTargetReg = objTargetReg_
    End Sub

    Public ReadOnly Property TargetReg() As Register
        Get
            Return mobjTargetReg
        End Get
    End Property

    Public Sub Add(ByVal blnLoading As Boolean)
        Dim objReplicaTrx As ReplicaTrx = New ReplicaTrx(mobjTargetReg)
        Dim strCatKey As String = mobjBankTrx.Register.Account.AccountKey.ToString() + "." + mobjBankTrx.Register.RegisterKey
        Dim strReplDescr As String
        If Not String.IsNullOrEmpty(mobjSplit.Memo) Then
            strReplDescr = mobjSplit.Memo
        Else
            strReplDescr = mobjBankTrx.Description
        End If
        objReplicaTrx.NewStartReplica(True, mobjBankTrx.TrxDate, strReplDescr,
                                strCatKey, mobjSplit.PONumber, mobjSplit.InvoiceNum, -mobjSplit.Amount, mobjBankTrx.IsFake)
        If blnLoading Then
            mobjTargetReg.NewLoadEnd(objReplicaTrx)
        Else
            mobjTargetReg.NewAddEnd(objReplicaTrx, New LogAddNull(), "", blnSetChanged:=False)
        End If
        mobjRepMgr = New ReplicaTrxManager(objReplicaTrx)
    End Sub

    Public Sub Delete()
        If Not mobjRepMgr Is Nothing Then
            mobjRepMgr.Trx.Delete(New LogDeleteNull(), "", blnSetChanged:=False)
            mobjRepMgr = Nothing
        End If
    End Sub
End Class

