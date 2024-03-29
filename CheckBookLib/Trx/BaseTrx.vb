Option Strict On
Option Explicit On

''' <summary>
''' Represents one transaction. Subclassed for different kinds of transactions.
'''
''' To create a transaction and add it to a Register, instantiate a BankTrx, BudgetTrx
''' or TransferTrx and call NewStartNormal(), NewStartBudget() or NewStartTransfer() on
''' that instance. For a BankTrx add at least one split with AddSplit(). Then call either
''' Register.NewLoadEnd() or Register.NewAddEnd(): Call Register.NewLoadEnd() if you are in 
''' the process of loading multiple BaseTrx into a Register from some external source, and will 
''' finish that process by calling Register.LoadPostProcessing(). 
''' Otherwise call Register.NewAddEnd(), which is what will typically happen if you are 
''' adding to a Register which is already visible in the UI.
''' 
''' To update an existing transaction, create a NormalTrxManager, BudgetTrxManager, 
''' TransferTrxManager or ReplicaTrxManager and call UpdateStart() on that instance.
''' Then make changes to members of .GetTrx of that instance. You can make changes to individual
''' members directly, or remake the entire BaseTrx from scratch by calling something like 
''' .GetTrx.UpdateStartNormal(). If you do call UpdateStartNormal(), always call
''' .GetTrx.AddSplit() at least once. When you are done making changes to .GetTrx,
''' finish by calling TrxManager.UpdateEnd().
''' 
''' To delete a transaction, call either Register.Delete() or BaseTrx.Delete().
'''
''' The UI is not updated directly by the caller to these methods, rather it is
''' updated indirectly by responding to events fired by the Register object as
''' its methods are called and complete their tasks.
''' </summary>

Public MustInherit Class BaseTrx

    Public Enum TrxStatus
        'Missing value (should never be this).
        Missing = 0
        'Unreconciled (new).
        Unreconciled = 1
        'Reconciled.
        Reconciled = 2
        'Non-bank (budget, transfer, etc.)
        NonBank = 3
        'Selected in current reconciliation.
        Selected = 4
    End Enum

    Public Enum RepeatUnit
        Missing = 0
        Day = 1
        Week = 2
        Month = 3
    End Enum

    'Register this BaseTrx belongs to.
    Protected RegisterInternal As Register
    'Index of this BaseTrx in Register.GetTrx().
    Protected RegIndexInternal As Integer
    'Transaction number.
    Protected NumberInternal As String
    'Transaction date.
    Protected TrxDateInternal As Date
    'Payee name, or other description.
    Protected DescriptionInternal As String
    'Transaction memo.
    Protected MemoInternal As String
    'Transaction status.
    Protected StatusInternal As TrxStatus
    'Is this transaction fake (future dated)?
    Protected IsFakeInternal As Boolean
    'BaseTrx needs to be reviewed by the operator.
    Protected IsAwaitingReviewInternal As Boolean
    'BaseTrx was added to register as part of an automatically generated
    'series. Such a BaseTrx will always have a non-empty mstrRepeatKey,
    'but a non-empty mstrRepeatKey does not imply mblnAutoGenerated=True.
    Protected IsAutoGeneratedInternal As Boolean
    'Sequence number of BaseTrx in generated sequence.
    Protected RepeatSeqInternal As Integer
    'All fake and real BaseTrx originating from the same generated sequence
    'have the same value.
    Protected RepeatKeyInternal As String
    'Computed from the sum of the Split amounts for normal trx.
    'Set directly for other trx types.
    Protected AmountInternal As Decimal
    'Original amount for a generated trx. This value is not persisted.
    Protected GeneratedAmountInternal As Decimal
    'Register balance after mcurAmount added.
    Protected BalanceInternal As Decimal

    Public Sub New(ByVal objReg_ As Register)
        RegisterInternal = objReg_
    End Sub

    Public Property Register() As Register
        Get
            Return RegisterInternal
        End Get
        Set(value As Register)
            RegisterInternal = value
        End Set
    End Property

    Public Sub ClearRepeatTrx()
        If RepeatSeqInternal > 0 Then
            'Remove the repeat trx index entry for the old values
            'of repeat key and repeat seq, which may be different
            'than the new ones.
            RegisterInternal.RemoveRepeatTrx(Me)
        End If
    End Sub

    Public Property RegIndex() As Integer
        Set(value As Integer)
            RegIndexInternal = value
        End Set
        Get
            Return RegIndexInternal
        End Get
    End Property

    Public ReadOnly Property NextTrx() As BaseTrx
        Get
            If RegIndexInternal >= RegisterInternal.TrxCount Then
                Return Nothing
            Else
                Return RegisterInternal.GetTrx(RegIndexInternal + 1)
            End If
        End Get
    End Property

    Public ReadOnly Property PreviousTrx() As BaseTrx
        Get
            If RegIndexInternal <= 1 Then
                Return Nothing
            Else
                Return RegisterInternal.GetTrx(RegIndexInternal - 1)
            End If
        End Get
    End Property

    Public ReadOnly Property RepeatKey() As String
        Get
            Return RepeatKeyInternal
        End Get
    End Property

    Public ReadOnly Property RepeatSeq() As Integer
        Get
            Return RepeatSeqInternal
        End Get
    End Property

    Public ReadOnly Property RepeatId() As String
        Get
            Return MakeRepeatId(RepeatKeyInternal, RepeatSeqInternal)
        End Get
    End Property

    Public Shared Function MakeRepeatId(ByVal strRepeatKey As String, ByVal intRepeatSeq As Integer) As String
        Return "#" & strRepeatKey & "." & intRepeatSeq
    End Function

    Public Sub ClearRepeat()
        RepeatSeqInternal = 0
        RepeatKeyInternal = ""
    End Sub

    Public Property Number() As String
        Get
            Return NumberInternal
        End Get
        Set(value As String)
            NumberInternal = value
        End Set
    End Property

    Public Property TrxDate() As Date
        Get
            Return TrxDateInternal
        End Get
        Set(value As Date)
            TrxDateInternal = value
        End Set
    End Property

    Public ReadOnly Property Description() As String
        Get
            Return DescriptionInternal
        End Get
    End Property

    Public MustOverride ReadOnly Property CategoryLabel() As String

    Public ReadOnly Property Memo() As String
        Get
            Return MemoInternal
        End Get
    End Property

    Public Property Status() As TrxStatus
        Get
            Return StatusInternal
        End Get
        Set(value As TrxStatus)
            StatusInternal = value
        End Set
    End Property

    Public ReadOnly Property StatusLabel() As String
        Get
            Select Case StatusInternal
                Case TrxStatus.NonBank
                    Return "NonBank"
                Case TrxStatus.Unreconciled
                    Return "Unreconciled"
                Case TrxStatus.Selected
                    Return "Selected"
                Case TrxStatus.Reconciled
                    Return "Reconciled"
                Case Else
                    Return ""
            End Select
        End Get
    End Property

    Public Property IsFake() As Boolean
        Get
            Return IsFakeInternal
        End Get
        Set(ByVal Value As Boolean)
            IsFakeInternal = Value
        End Set
    End Property

    Public ReadOnly Property FakeStatusLabel() As String
        Get
            If IsAutoGeneratedInternal Then
                Return "Gen"
            ElseIf IsFakeInternal Then
                Return "Fake"
            Else
                Return ""
            End If
        End Get
    End Property

    Public ReadOnly Property IsAwaitingReview() As Boolean
        Get
            Return IsAwaitingReviewInternal
        End Get
    End Property

    Public ReadOnly Property IsAutoGenerated() As Boolean
        Get
            Return IsAutoGeneratedInternal
        End Get
    End Property

    ''' <summary>
    ''' The face value amount of the transaction.
    ''' </summary>
    ''' <returns></returns>
    Public Property Amount() As Decimal
        Get
            Return AmountInternal
        End Get
        Set(value As Decimal)
            AmountInternal = value
        End Set
    End Property

    ''' <summary>
    ''' The amount this transaction will change the running balance in a register.
    ''' We distinguish this from the face value of the transaction because the
    ''' software may in the future treat some transactions as not affecting the balance.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property BalanceChangeAmount() As Decimal
        Get
            Return AmountInternal
        End Get
    End Property

    Public Property GeneratedAmount() As Decimal
        Get
            Return GeneratedAmountInternal
        End Get
        Set(value As Decimal)
            GeneratedAmountInternal = value
        End Set
    End Property

    ''' <summary>
    ''' The register balance after adding this transaction.
    ''' </summary>
    ''' <returns></returns>
    Public Property Balance() As Decimal
        Get
            Return BalanceInternal
        End Get
        Set(value As Decimal)
            BalanceInternal = value
        End Set
    End Property

    Protected Sub RaiseErrorOnBadData(ByVal strRoutine As String)
        If TrxDateInternal = Utilities.EmptyDate Then
            RaiseErrorMsg("Missing date in " & strRoutine)
        End If
        If DescriptionInternal = "" Then
            RaiseErrorMsg("Missing description in " & strRoutine)
        End If
        If StatusInternal = TrxStatus.Missing Then
            RaiseErrorMsg("Missing status in " & strRoutine)
        End If
    End Sub

    Public MustOverride ReadOnly Property TrxTypeSortKey() As Integer
    Public MustOverride ReadOnly Property DocNumberSortKey() As String

    Public Overridable ReadOnly Property AmountSortKey() As Integer
        Get
            Return If(AmountInternal > 0, 0, 1)
        End Get
    End Property

    Public Overridable ReadOnly Property InvoiceNum() As String
        Get
            Return ""
        End Get
    End Property

    Public Overridable ReadOnly Property PONumber() As String
        Get
            Return ""
        End Get
    End Property

    Public Sub Delete(ByVal objDeleteLogger As ILogDelete, ByVal strLogTitle As String,
                      Optional ByVal blnSetChanged As Boolean = True)
        RegisterInternal.Delete(Me, objDeleteLogger, strLogTitle, blnSetChanged)
    End Sub

    Public MustOverride Sub AddApply()
    Public MustOverride Sub UpdateUnApply()
    Public MustOverride Sub UpdateApply()
    Public MustOverride Sub DeleteUnApply()
    Public MustOverride Sub LoadApply()
    Public MustOverride Sub LoadFinish()

    '$Description Check for validation errors for Register.Validate().

    Public Overridable Sub Validate()
        Dim objRepeatTrx As BaseTrx
        If RegIndexInternal > 0 Then
            If Not RegisterInternal.GetTrx(RegIndexInternal) Is Me Then
                RegisterInternal.FireValidationError(Me, "lngIndex is wrong")
            End If
        End If
        If TrxDateInternal = Utilities.EmptyDate Then
            RegisterInternal.FireValidationError(Me, "Missing date")
        End If
        If RepeatKeyInternal <> "" Then
            If RepeatSeqInternal = 0 Then
                RegisterInternal.FireValidationError(Me, "Repeat key has no repeat seq")
            End If
            objRepeatTrx = Register.FindRepeatTrx(RepeatKeyInternal, RepeatSeqInternal)
            If Not objRepeatTrx Is Me Then
                RegisterInternal.FireValidationError(Me, "objRepeatTrx() returned wrong Trx")
            End If
        Else
            If RepeatSeqInternal <> 0 Then
                RegisterInternal.FireValidationError(Me, "Repeat seq should be zero")
            End If
        End If
    End Sub

    Public MustOverride Function CloneTrx(ByVal blnWillAddToRegister As Boolean) As BaseTrx

    Public Overridable Function GetSummary() As String
        Return Me.TrxDate.ToShortDateString() + " " + Me.Description + " " + Me.Amount.ToString()
    End Function

    Public Overrides Function ToString() As String
        Return GetSummary()
    End Function
End Class