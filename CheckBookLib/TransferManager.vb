Option Strict On
Option Explicit On

Public Class TransferManager
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    '$Description Add a new transfer between two *ALREADY LOADED* registers, by creating
    '   a transfer Trx in each register. Don't need a TransferManager function to do the
    '   equivalent during register loading of *NON-GENERATED* transfers, because each
    '   half of the transfer is saved with its register. This function fires events, so
    '   is not suitable for creating *GENERATED* transfer Trx pairs while loading
    '   a register.

    Public Sub AddTransfer(ByVal objReg1 As Register, ByVal objReg2 As Register, ByVal datDate As Date, ByVal strDescription As String, ByVal strMemo As String, ByVal blnFake As Boolean, ByVal curTransferAmount As Decimal, ByVal strRepeatKey As String, ByVal blnAwaitingReview As Boolean, ByVal blnAutoGenerated As Boolean, ByVal intRepeatSeq As Integer)

        Dim objTrx1 As Trx
        Dim objTrx2 As Trx

        objTrx1 = New Trx
        objTrx1.NewStartTransfer(objReg1, datDate, strDescription, strMemo, blnFake, blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, objReg2.strRegisterKey, curTransferAmount)
        objReg1.NewAddEnd(objTrx1, New LogAdd, "AddTransfer1")

        objTrx2 = New Trx
        objTrx2.NewStartTransfer(objReg2, datDate, strDescription, strMemo, blnFake, blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, objReg1.strRegisterKey, -curTransferAmount)
        objReg2.NewAddEnd(objTrx2, New LogAdd, "AddTransfer2")

    End Sub

    '$Description Same as AddTransfer(), but for creating transfer Trx pairs for
    '   *GENERATED* transfers during register loading. I.e., does not fire events.

    Public Sub LoadGeneratedTransfer(ByVal objReg1 As Register, ByVal objReg2 As Register, ByVal datDate As Date, ByVal strDescription As String, ByVal strMemo As String, ByVal blnFake As Boolean, ByVal curTransferAmount As Decimal, ByVal strRepeatKey As String, ByVal blnAwaitingReview As Boolean, ByVal blnAutoGenerated As Boolean, ByVal intRepeatSeq As Integer)

        Dim objTrx1 As Trx
        Dim objTrx2 As Trx

        objTrx1 = New Trx
        objTrx1.NewStartTransfer(objReg1, datDate, strDescription, strMemo, blnFake, blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, objReg2.strRegisterKey, curTransferAmount)
        objReg1.NewLoadEnd(objTrx1)

        objTrx2 = New Trx
        objTrx2.NewStartTransfer(objReg2, datDate, strDescription, strMemo, blnFake, blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, objReg1.strRegisterKey, -curTransferAmount)
        objReg2.NewLoadEnd(objTrx2)

    End Sub

    '$Description Update a transfer. Will fail harmlessly with a clear runtime error
    '   message if you attempt to change the transfer key.

    Public Sub UpdateTransfer(ByVal objReg1 As Register, ByVal lngIndex1 As Integer, ByVal objReg2 As Register, ByVal datDate As Date, ByVal strDescription As String, ByVal strMemo As String, ByVal blnFake As Boolean, ByVal curTransferAmount As Decimal, ByVal strRepeatKey As String, ByVal blnAwaitingReview As Boolean, ByVal blnAutoGenerated As Boolean, ByVal intRepeatSeq As Integer)

        Dim lngIndex2 As Integer
        Dim objTrxManager1 As TrxManager
        Dim objTrx1 As Trx
        Dim objTrxManager2 As TrxManager
        Dim objTrx2 As Trx

        objTrxManager1 = objReg1.objGetTrxManager(lngIndex1)
        objTrx1 = objTrxManager1.objTrx
        If objTrx1.lngType <> Trx.TrxType.glngTRXTYP_TRANSFER Then
            gRaiseError("Trx is not a transfer in TransferManager.UpdateTransfer")
        End If
        'objTrx1.strTransferKey is the OLD transfer key, because objTrx1 hasn't been
        'updated yet. objReg2 is the Register chosen to save as the new TransferKey.
        If objTrx1.strTransferKey <> objReg2.strRegisterKey Then
            gRaiseError("Transfer key may not be changed in TransferManager.UpdateTransfer")
        End If
        lngIndex2 = objReg2.lngMatchTransfer(objTrx1.datDate, objReg1.strRegisterKey, -objTrx1.curAmount)
        If lngIndex2 = 0 Then
            gRaiseError("Could not find matching Trx in TransferManager.UpdateTransfer")
        End If
        objTrxManager2 = objReg2.objGetTrxManager(lngIndex2)
        objTrx2 = objTrxManager2.objTrx
        objTrxManager1.UpdateStart()
        objTrxManager2.UpdateStart()
        objTrx1.UpdateStartTransfer(datDate, strDescription, strMemo, blnFake, blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, curTransferAmount)
        objTrx2.UpdateStartTransfer(datDate, strDescription, strMemo, blnFake, blnAwaitingReview, blnAutoGenerated, intRepeatSeq, strRepeatKey, -curTransferAmount)
        objTrxManager1.UpdateEnd(New LogChange, "UpdateTransfer1")
        objTrxManager2.UpdateEnd(New LogChange, "UpdateTransfer2")

    End Sub

    Public Sub DeleteTransfer(ByVal objReg1 As Register, ByVal lngIndex1 As Integer, ByVal objReg2 As Register)

        Dim lngIndex2 As Integer
        Dim objTrx1 As Trx

        objTrx1 = objReg1.objTrx(lngIndex1)
        If objTrx1.lngType <> Trx.TrxType.glngTRXTYP_TRANSFER Then
            gRaiseError("Trx is not a transfer in TransferManager.DeleteTransfer")
        End If
        lngIndex2 = objReg2.lngMatchTransfer(objTrx1.datDate, objReg1.strRegisterKey, -objTrx1.curAmount)
        If lngIndex2 = 0 Then
            gRaiseError("Could not find matching Trx in TransferManager.DeleteTransfer")
        End If
        objReg1.Delete(lngIndex1, New LogDelete, "DeleteTransfer1")
        objReg2.Delete(lngIndex2, New LogDelete, "DeleteTransfer2")

    End Sub
End Class