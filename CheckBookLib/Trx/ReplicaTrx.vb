﻿Option Strict On
Option Explicit On

Public Class ReplicaTrx
    Inherits BaseTrx

    Private mstrCatKey As String
    Private mstrPONumber As String
    Private mstrInvoiceNum As String

    Public Sub New(ByVal objReg_ As Register)
        MyBase.New(objReg_)
    End Sub

    Public Overrides ReadOnly Property CategoryLabel As String
        Get
            Return RegisterInternal.Account.Company.Categories.TranslateKey(mstrCatKey)
        End Get
    End Property

    Public Overrides ReadOnly Property PONumber As String
        Get
            Return mstrPONumber
        End Get
    End Property

    Public Overrides ReadOnly Property InvoiceNum As String
        Get
            Return mstrInvoiceNum
        End Get
    End Property

    Public Sub NewStartReplica(ByVal blnWillAddToRegister As Boolean, ByVal datDate_ As Date, ByVal strDescription_ As String,
                              ByVal strCatKey_ As String, ByVal strPONumber_ As String, ByVal strInvoiceNum_ As String,
                               ByVal curAmount_ As Decimal, ByVal blnFake_ As Boolean)

        If blnWillAddToRegister Then
            RegisterInternal.ClearFirstAffected()
        End If

        NumberInternal = "Repl"
        TrxDateInternal = datDate_
        DescriptionInternal = strDescription_
        mstrCatKey = strCatKey_
        mstrPONumber = strPONumber_
        mstrInvoiceNum = strInvoiceNum_
        MemoInternal = ""
        StatusInternal = TrxStatus.NonBank
        IsFakeInternal = blnFake_
        IsAwaitingReviewInternal = False
        IsAutoGeneratedInternal = False
        AmountInternal = curAmount_

        RaiseErrorOnBadData("NewStartReplica")

    End Sub

    Public Overrides Sub AddApply()
        'Do nothing for ReplicaTrx
    End Sub

    Public Overrides Sub UpdateUnApply()
        'Do nothing for ReplicaTrx
    End Sub

    Public Overrides Sub UpdateApply()
        'Do nothing for ReplicaTrx
    End Sub

    Public Overrides Sub DeleteUnApply()
        'Do nothing for ReplicaTrx
    End Sub

    Public Overrides Sub LoadApply()
        'Do nothing for ReplicaTrx
    End Sub

    Public Overrides Sub LoadFinish()
        'Do nothing for ReplicaTrx
    End Sub

    Public Overrides Function CloneTrx(ByVal blnWillAddToRegister As Boolean) As BaseTrx
        Dim objReplicaTrx As ReplicaTrx = New ReplicaTrx(RegisterInternal)
        objReplicaTrx.NewStartReplica(blnWillAddToRegister, TrxDateInternal, DescriptionInternal, mstrCatKey, mstrPONumber, mstrInvoiceNum, AmountInternal, IsFakeInternal)
        Return objReplicaTrx
    End Function

    Public Overrides ReadOnly Property TrxTypeSortKey As Integer
        Get
            Return 1
        End Get
    End Property

    Public Overrides ReadOnly Property DocNumberSortKey As String
        Get
            Return ""
        End Get
    End Property
End Class
