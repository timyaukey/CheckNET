Option Strict On
Option Explicit On

Public Class TransferManager
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    '$Description Add a new transfer between two *ALREADY LOADED* registers, by creating
    '   a transfer BaseTrx in each register. Don't need a TransferManager function to do the
    '   equivalent during register loading of *NON-GENERATED* transfers, because each
    '   half of the transfer is saved with its register. This function fires events, so
    '   is not suitable for creating *GENERATED* transfer BaseTrx pairs while loading
    '   a register.

    Public Sub AddTransfer(ByVal objReg1 As Register, ByVal objReg2 As Register, ByVal datDate As Date, ByVal strDescription As String, ByVal strMemo As String, ByVal blnFake As Boolean, ByVal curTransferAmount As Decimal, ByVal strRepeatKey As String, ByVal blnAwaitingReview As Boolean, ByVal blnAutoGenerated As Boolean, ByVal intRepeatSeq As Integer)

        Dim objTrx1 As TransferTrx
        Dim objTrx2 As TransferTrx

        objTrx1 = New TransferTrx(objReg1)
        objTrx1.NewStartTransfer(True, datDate, strDescription, strMemo, blnFake, blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, objReg2.RegisterKey, curTransferAmount)
        objReg1.NewAddEnd(objTrx1, New LogAdd, "AddTransfer1")

        objTrx2 = New TransferTrx(objReg2)
        objTrx2.NewStartTransfer(True, datDate, strDescription, strMemo, blnFake, blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, objReg1.RegisterKey, -curTransferAmount)
        objReg2.NewAddEnd(objTrx2, New LogAdd, "AddTransfer2")

    End Sub

    '$Description Same as AddTransfer(), but for creating transfer BaseTrx pairs for
    '   *GENERATED* transfers during register loading. I.e., does not fire events.

    Public Sub LoadGeneratedTransfer(ByVal objReg1 As Register, ByVal objReg2 As Register, ByVal datDate As Date, ByVal strDescription As String, ByVal strMemo As String, ByVal blnFake As Boolean, ByVal curTransferAmount As Decimal, ByVal strRepeatKey As String, ByVal blnAwaitingReview As Boolean, ByVal blnAutoGenerated As Boolean, ByVal intRepeatSeq As Integer)

        Dim objTrx1 As TransferTrx
        Dim objTrx2 As TransferTrx

        objTrx1 = New TransferTrx(objReg1)
        objTrx1.NewStartTransfer(True, datDate, strDescription, strMemo, blnFake, blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, objReg2.RegisterKey, curTransferAmount)
        objReg1.NewLoadEnd(objTrx1)

        objTrx2 = New TransferTrx(objReg2)
        objTrx2.NewStartTransfer(True, datDate, strDescription, strMemo, blnFake, blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, objReg1.RegisterKey, -curTransferAmount)
        objReg2.NewLoadEnd(objTrx2)

    End Sub

    '$Description Update a transfer. Will fail harmlessly with a clear runtime error
    '   message if you attempt to change the transfer key.

    Public Sub UpdateTransfer(ByVal objReg1 As Register, ByVal objTrx1 As TransferTrx, ByVal objReg2 As Register, ByVal datDate As Date, ByVal strDescription As String, ByVal strMemo As String, ByVal blnFake As Boolean, ByVal curTransferAmount As Decimal, ByVal strRepeatKey As String, ByVal blnAwaitingReview As Boolean, ByVal blnAutoGenerated As Boolean, ByVal intRepeatSeq As Integer)

        Dim objTrxManager1 As TransferTrxManager
        Dim objTrxManager2 As TransferTrxManager
        Dim objTrx2 As TransferTrx

        objTrxManager1 = New TransferTrxManager(objTrx1)
        If objTrx1.GetType() IsNot GetType(TransferTrx) Then
            gRaiseError("Trx is not a transfer in TransferManager.UpdateTransfer")
        End If
        'objTrx1.strTransferKey is the OLD transfer key, because objTrx1 hasn't been
        'updated yet. objReg2 is the Register chosen to save as the new TransferKey.
        If objTrx1.TransferKey <> objReg2.RegisterKey Then
            gRaiseError("Transfer key may not be changed in TransferManager.UpdateTransfer")
        End If
        objTrx2 = objReg2.MatchTransfer(objTrx1.TrxDate, objReg1.RegisterKey, -objTrx1.Amount)
        If objTrx2 Is Nothing Then
            gRaiseError("Could not find matching Trx in TransferManager.UpdateTransfer")
        End If
        objTrxManager2 = New TransferTrxManager(objTrx2)
        objTrxManager1.UpdateStart()
        objTrxManager2.UpdateStart()
        objTrx1.UpdateStartTransfer(datDate, strDescription, strMemo, blnFake, blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, curTransferAmount)
        objTrx2.UpdateStartTransfer(datDate, strDescription, strMemo, blnFake, blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, -curTransferAmount)
        objTrxManager1.UpdateEnd(New LogChange, "UpdateTransfer1")
        objTrxManager2.UpdateEnd(New LogChange, "UpdateTransfer2")

    End Sub

    Public Sub DeleteTransfer(ByVal objReg1 As Register, ByVal objTrx1 As BaseTrx, ByVal objReg2 As Register)

        If objTrx1.GetType() IsNot GetType(TransferTrx) Then
            gRaiseError("Trx is not a transfer in TransferManager.DeleteTransfer")
        End If
        Dim objTrx2 As TransferTrx = objReg2.MatchTransfer(objTrx1.TrxDate, objReg1.RegisterKey, -objTrx1.Amount)
        If objTrx2 Is Nothing Then
            gRaiseError("Could not find matching Trx in TransferManager.DeleteTransfer")
        End If
        objReg1.Delete(objTrx1, New LogDelete, "DeleteTransfer1")
        objReg2.Delete(objTrx2, New LogDelete, "DeleteTransfer2")

    End Sub
End Class