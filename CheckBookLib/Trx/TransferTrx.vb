﻿Option Strict On
Option Explicit On

''' <summary>
''' A BaseTrx subclass representing a transfer between two Register objects
''' in the same account.
''' </summary>

Public Class TransferTrx
    Inherits BaseTrx

    'For transfer BaseTrx, the unique ID of the Register being transfered to or from.
    'For transfer BaseTrx, there must exist a transfer BaseTrx in this account with the same
    'date, the RegisterKey of THIS subaccount, and the negative of the amount of
    'this BaseTrx. For non-transfer BaseTrx, must be an empty string.
    Private mstrTransferKey As String
    'Amount of a subaccount transfer.
    Private mcurTransferAmount As Decimal

    Public Sub New(ByVal objReg_ As Register)
        MyBase.New(objReg_)
    End Sub

    '$Description Initialize a new transfer BaseTrx object. Normally followed by
    '   either Register.NewLoadEnd() or Register.NewAddEnd().

    Public Sub NewStartTransfer(ByVal blnWillAddToRegister As Boolean, ByVal datDate_ As Date, ByVal strDescription_ As String,
                                ByVal strMemo_ As String, ByVal blnFake_ As Boolean, ByVal blnAwaitingReview_ As Boolean,
                                ByVal blnAutoGenerated_ As Boolean, ByVal intRepeatSeq_ As Integer, ByVal strRepeatKey_ As String,
                                ByVal strTransferKey_ As String, ByVal curTransferAmount_ As Decimal)

        If blnWillAddToRegister Then
            RegisterInternal.ClearFirstAffected()
        End If

        NumberInternal = "XFR"
        TrxDateInternal = datDate_
        DescriptionInternal = strDescription_
        MemoInternal = strMemo_
        StatusInternal = TrxStatus.NonBank
        IsFakeInternal = blnFake_
        IsAwaitingReviewInternal = blnAwaitingReview_
        IsAutoGeneratedInternal = blnAutoGenerated_
        RepeatSeqInternal = intRepeatSeq_
        RepeatKeyInternal = strRepeatKey_

        mstrTransferKey = strTransferKey_
        mcurTransferAmount = curTransferAmount_

        AmountInternal = mcurTransferAmount
        BalanceInternal = 0

        RaiseErrorOnBadData("NewStartTransfer")
        RaiseErrorOnBadTransfer("NewStartTransfer")

    End Sub

    '$Description Update all updatable properties of this BaseTrx transfer object.
    '   Normally followed by Register.UpdateEnd().

    Public Sub UpdateStartTransfer(ByVal datDate_ As Date, ByVal strDescription_ As String, ByVal strMemo_ As String, ByVal blnFake_ As Boolean, ByVal blnAwaitingReview_ As Boolean, ByVal blnAutoGenerated_ As Boolean, ByVal intRepeatSeq_ As Integer, ByVal strRepeatKey_ As String, ByVal curTransferAmount_ As Decimal)

        TrxDateInternal = datDate_
        DescriptionInternal = strDescription_
        MemoInternal = strMemo_
        IsFakeInternal = blnFake_
        IsAwaitingReviewInternal = blnAwaitingReview_
        IsAutoGeneratedInternal = blnAutoGenerated_
        RepeatSeqInternal = intRepeatSeq_
        RepeatKeyInternal = strRepeatKey_
        mcurTransferAmount = curTransferAmount_

        'We do NOT clear mcurBudgetApplied, because updating the budget
        'does not change what splits have been applied to it.
        AmountInternal = mcurTransferAmount
        BalanceInternal = 0

        RaiseErrorOnBadData("UpdateStartTransfer")
        RaiseErrorOnBadTransfer("UpdateStartTransfer")
    End Sub

    Public ReadOnly Property TransferKey() As String
        Get
            TransferKey = mstrTransferKey
        End Get
    End Property

    Public ReadOnly Property TransferAmount() As Decimal
        Get
            TransferAmount = mcurTransferAmount
        End Get
    End Property

    Public Overrides ReadOnly Property CategoryLabel As String
        Get
            Return ""
        End Get
    End Property

    Public Overrides Sub UnApply()
        'Do nothing for TransferTrx
    End Sub

    Public Overrides Sub Apply(ByVal blnLoading As Boolean)
        'Do nothing for TransferTrx
    End Sub

    Public Overrides Function CloneTrx(ByVal blnWillAddToRegister As Boolean) As BaseTrx
        Dim objXferTrx As TransferTrx = New TransferTrx(RegisterInternal)
        objXferTrx.NewStartTransfer(blnWillAddToRegister, TrxDateInternal, DescriptionInternal, MemoInternal, IsFakeInternal, IsAwaitingReviewInternal,
                                    IsAutoGeneratedInternal, RepeatSeqInternal, RepeatKeyInternal, mstrTransferKey, mcurTransferAmount)
        Return objXferTrx
    End Function

    Public Overrides Sub Validate()
        MyBase.Validate()
        If mstrTransferKey = "" Then
            Register.FireValidationError(Me, "Transfer trx requires transfer key")
        End If
        If mstrTransferKey = Register.RegisterKey Then
            Register.FireValidationError(Me, "Transfer trx cannot transfer to same register")
        End If
    End Sub

    Public Overrides ReadOnly Property TrxTypeSortKey As Integer
        Get
            Return 2
        End Get
    End Property

    Public Overrides ReadOnly Property DocNumberSortKey As String
        Get
            Return ""
        End Get
    End Property

    Protected Sub RaiseErrorOnBadTransfer(ByVal strRoutine As String)
        If mstrTransferKey = "" Then
            gRaiseError("Missing transfer key in " & strRoutine)
        End If
    End Sub
End Class
