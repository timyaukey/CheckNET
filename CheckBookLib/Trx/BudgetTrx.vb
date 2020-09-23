﻿Option Strict On
Option Explicit On

''' <summary>
''' A Trx subclass representing an amount of money set aside for some use.
''' TrxSplit objects can reference a BudgetTrx, which causes the effective
''' amount of the budget item used in running balance computations to be
''' different than the nominal budget amount.
''' </summary>

Public Class BudgetTrx
    Inherits Trx

    'Original budget amount.
    'Is NOT changed as other TrxSplit are applied and unapplied to this one.
    'Sign has the same meaning as TrxSplit.curAmount, so is negative for expenses.
    Private mcurBudgetLimit As Decimal
    'First date in the budget period.
    Private mdatBudgetStarts As Date
    'Last date in the budget period.
    Private mdatBudgetEnds As Date
    'For budget Trx, a unique ID for that budget. All budget Trx in a repeating
    'budget will have the same mstrBudgetKey, and the appropriate one for applying
    'Trx will be chosen by the budget period. Is a foreign key into some external
    'database defining how to create budget Trx.
    Private mstrBudgetKey As String
    'Amount applied toward budget. Magnitude may be greater than mcurBudgetLimit,
    'in which case mcurAmount will equal zero instead of a credit.
    Private mcurBudgetApplied As Decimal
    'Budget is expired (amount will be zero).
    Private mblnIsExpired As Boolean
    'Collection of TrxSplit objects belonging to other NormalTrx and applied to this BudgetTrx.
    Private mcolAppliedSplits As List(Of TrxSplit)

    Public Sub New(ByVal objReg_ As Register)
        MyBase.New(objReg_)
    End Sub

    '$Description Initialize a new budget Trx object. Normally followed by
    '   Register.NewLoadEnd() or Register.NewAddEnd().

    Public Sub NewStartBudget(ByVal blnWillAddToRegister As Boolean, ByVal datDate_ As Date, ByVal strDescription_ As String,
                              ByVal strMemo_ As String, ByVal blnAwaitingReview_ As Boolean, ByVal blnAutoGenerated_ As Boolean,
                              ByVal intRepeatSeq_ As Integer, ByVal strRepeatKey_ As String, ByVal curBudgetLimit_ As Decimal,
                              ByVal datBudgetStarts_ As Date, ByVal strBudgetKey_ As String)

        If blnWillAddToRegister Then
            mobjReg.ClearFirstAffected()
        End If

        mstrNumber = "Budget"
        mdatDate = datDate_
        mstrDescription = strDescription_
        mstrMemo = strMemo_
        mlngStatus = TrxStatus.NonBank
        mblnFake = True
        mblnAwaitingReview = blnAwaitingReview_
        mblnAutoGenerated = blnAutoGenerated_
        mintRepeatSeq = intRepeatSeq_
        mstrRepeatKey = strRepeatKey_

        mcurBudgetLimit = curBudgetLimit_
        mdatBudgetStarts = datBudgetStarts_
        mdatBudgetEnds = datDate_
        PutPeriodDatesInCorrectOrder()
        mstrBudgetKey = strBudgetKey_
        mcurBudgetApplied = 0
        mblnIsExpired = False

        If blnWillAddToRegister Then
            SetAmountForBudget()
        End If
        mcurBalance = 0

        mcolAppliedSplits = New List(Of TrxSplit)

        RaiseErrorOnBadData("NewStartBudget")
        RaiseErrorOnBadBudget("NewStartBudget")

    End Sub

    '$Description Update all updatable properties of this budget Trx object.
    '   Normally followed Register.UpdateEnd().

    Public Sub UpdateStartBudget(ByVal datDate_ As Date, ByVal strDescription_ As String, ByVal strMemo_ As String,
                                 ByVal blnAwaitingReview_ As Boolean, ByVal blnAutoGenerated_ As Boolean,
                                 ByVal intRepeatSeq_ As Integer, ByVal strRepeatKey_ As String, ByVal curBudgetLimit_ As Decimal,
                                 ByVal datBudgetStarts_ As Date, ByVal strBudgetKey_ As String)

        mdatDate = datDate_
        mstrDescription = strDescription_
        mstrMemo = strMemo_
        mblnAwaitingReview = blnAwaitingReview_
        mblnAutoGenerated = blnAutoGenerated_
        mintRepeatSeq = intRepeatSeq_
        mstrRepeatKey = strRepeatKey_
        mcurBudgetLimit = curBudgetLimit_
        mdatBudgetStarts = datBudgetStarts_
        mdatBudgetEnds = datDate_
        PutPeriodDatesInCorrectOrder()
        mstrBudgetKey = strBudgetKey_

        'We do NOT clear mcurBudgetApplied, because updating the budget
        'does not change what splits have been applied to it.
        SetAmountForBudget()
        mcurBalance = 0

        'Will not unapply any budgets, even though the budget period or budget key
        'may have been changed by this update. The caller should warn the operator
        'that normal transactions will not be reapplied to budgets until they are
        'edited and saved or the register reloaded.

        RaiseErrorOnBadData("UpdateStartBudget")
        RaiseErrorOnBadBudget("UpdateStartBudget")
    End Sub

    ''' <summary>
    ''' Only needed to load old data files that have trx date
    ''' equal to the ending date, not the starting date.
    ''' This method can be removed after all old data files
    ''' have been loaded and saved. Which might be hard to 
    ''' determine, because a .ACT file will not be saved
    ''' until there is actually a change in it.
    ''' </summary>
    Private Sub PutPeriodDatesInCorrectOrder()
        If mdatBudgetStarts > mdatBudgetEnds Then
            Dim datTemp As Date = mdatBudgetStarts
            mdatBudgetStarts = mdatBudgetEnds
            mdatBudgetEnds = datTemp
            mdatDate = datTemp
            Me.objReg.objAccount.SetChanged()
        End If
    End Sub

    Public Property curBudgetLimit() As Decimal
        Get
            Return mcurBudgetLimit
        End Get
        Set(value As Decimal)
            mcurBudgetLimit = value
        End Set
    End Property

    Public ReadOnly Property datBudgetStarts() As Date
        Get
            Return mdatBudgetStarts
        End Get
    End Property

    Public ReadOnly Property datBudgetEnds() As Date
        Get
            Return mdatBudgetEnds
        End Get
    End Property

    Public Function InBudgetPeriod(ByVal datTarget As DateTime) As Boolean
        Return datTarget >= mdatBudgetStarts And datTarget <= mdatBudgetEnds
    End Function

    Public ReadOnly Property strBudgetKey() As String
        Get
            Return mstrBudgetKey
        End Get
    End Property

    Public ReadOnly Property curBudgetApplied() As Decimal
        Get
            Return mcurBudgetApplied
        End Get
    End Property

    Public ReadOnly Property blnIsExpired() As Boolean
        Get
            Return mblnIsExpired
        End Get
    End Property

    Public Overrides ReadOnly Property strCategory As String
        Get
            Return ""
        End Get
    End Property

    Protected Sub RaiseErrorOnBadBudget(ByVal strRoutine As String)
        If mstrBudgetKey = "" Then
            gRaiseError("Missing budget key in " & strRoutine)
        End If
        If mdatBudgetEnds = System.DateTime.FromOADate(0) Then
            gRaiseError("Missing budget end date in " & strRoutine)
        End If
        If mdatBudgetEnds < mdatBudgetStarts Then
            gRaiseError("Budget period ends before it begins")
        End If
    End Sub

    '$Description Set mcurAmount for a budget Trx. Called whenever
    '   mcurBudgetApplied or mcurBudgetLimit changes.

    Public Sub SetAmountForBudget()
        If mdatBudgetEnds < mobjReg.datOldestBudgetEndAllowed Or System.Math.Abs(mcurBudgetApplied) > System.Math.Abs(mcurBudgetLimit) Then
            mcurAmount = 0
            mblnIsExpired = True
        Else
            mcurAmount = mcurBudgetLimit - mcurBudgetApplied
            mblnIsExpired = False
        End If
    End Sub

    Public Overrides Sub UnApply()
        For Each objSplit As TrxSplit In mcolAppliedSplits
            objSplit.objBudget = Nothing
        Next objSplit
        mcolAppliedSplits = New List(Of TrxSplit)()
        mcurBudgetApplied = 0D
        SetAmountForBudget()
        mobjReg.RaiseBudgetChanged(Me)
    End Sub

    Public Overrides Sub Apply(ByVal blnLoading As Boolean)
        Dim blnAnyApplied As Boolean = False
        For lngScanIndex As Integer = Me.lngEarliestPossibleAppliedIndex() To mobjReg.lngTrxCount
            Dim objScanTrx As Trx = mobjReg.objTrx(lngScanIndex)
            If objScanTrx.datDate > mdatBudgetEnds Then
                Exit For
            End If
            If objScanTrx.GetType() = GetType(NormalTrx) Then
                Dim objNormalTrx As NormalTrx = DirectCast(objScanTrx, NormalTrx)
                For Each objSplit As TrxSplit In objNormalTrx.colSplits
                    If objSplit.objBudget Is Nothing Then
                        If objSplit.strBudgetKey = Me.strBudgetKey Then
                            mcolAppliedSplits.Add(objSplit)
                            objSplit.objBudget = Me
                            mcurBudgetApplied = mcurBudgetApplied + objSplit.curAmount
                            blnAnyApplied = True
                        End If
                    End If
                Next
            End If
        Next
        If blnAnyApplied Then
            SetAmountForBudget()
            mobjReg.RaiseBudgetChanged(Me)
        End If
    End Sub

    ''' <summary>
    ''' Return the index of the earliest Trx in this register that could possibly
    ''' be applied to this BudgetTrx. The returned index will always be valid,
    ''' but it may not be a NormalTrx and it may not be applied to the Budget.
    ''' It is merely the earliest that COULD be applied, based on the dates.
    ''' The caller normally scans forward from here until they find a Trx
    ''' dated after mdatBudgetEnds.
    ''' </summary>
    ''' <returns></returns>
    Public Function lngEarliestPossibleAppliedIndex() As Integer
        Dim lngEarliest As Integer = Me.lngIndex
        Dim objCurrent As Trx
        Do
            If lngEarliest = 1 Then
                Return lngEarliest
            End If
            objCurrent = mobjReg.objTrx(lngEarliest)
            If objCurrent.datDate < mdatBudgetStarts Then
                Return lngEarliest + 1
            End If
            lngEarliest = lngEarliest - 1
        Loop
    End Function

    '$Description Apply a Split to this BudgetTrx.

    Public Sub ApplyToThisBudget(ByVal objSplit As TrxSplit)
        mcolAppliedSplits.Add(objSplit)
        objSplit.objBudget = Me
        mcurBudgetApplied = mcurBudgetApplied + objSplit.curAmount
        SetAmountForBudget()
        mobjReg.RaiseBudgetChanged(Me)
    End Sub

    '$Description Un-apply a Split from this BudgetTrx.

    Public Sub UnApplyFromThisBudget(ByVal objSplit As TrxSplit)
        If Not mcolAppliedSplits.Remove(objSplit) Then
            gRaiseError("Could not find split in Trx.UnApplyFromThisBudget")
        End If
        mcurBudgetApplied = mcurBudgetApplied - objSplit.curAmount
        SetAmountForBudget()
        mobjReg.RaiseBudgetChanged(Me)
    End Sub

    Public ReadOnly Property colAppliedSplits As List(Of TrxSplit)
        Get
            Return mcolAppliedSplits
        End Get
    End Property

    Public Overrides Function objClone(ByVal blnWillAddToRegister As Boolean) As Trx
        Dim objBudgetTrx As BudgetTrx = New BudgetTrx(mobjReg)
        objBudgetTrx.NewStartBudget(blnWillAddToRegister, mdatDate, mstrDescription, mstrMemo, mblnAwaitingReview, mblnAutoGenerated,
                                    mintRepeatSeq, mstrRepeatKey, mcurBudgetLimit, mdatBudgetStarts, mstrBudgetKey)
        objBudgetTrx.curAmount = mcurAmount
        Return objBudgetTrx
    End Function

    Public Function objGetTrxManager() As BudgetTrxManager
        Return New BudgetTrxManager(Me)
    End Function

    Public Overrides Sub Validate()
        Dim objSplit As TrxSplit
        Dim curTotal As Decimal
        MyBase.Validate()
        If mstrBudgetKey = "" Then
            objReg.RaiseValidationError(Me, "Budget trx requires budget key")
            Exit Sub
        End If
        If mcolAppliedSplits Is Nothing Then
            objReg.RaiseValidationError(Me, "Missing applied split collection")
        Else
            curTotal = 0
            For Each objSplit In mcolAppliedSplits
                curTotal = curTotal + objSplit.curAmount
                If Not objSplit.objBudget Is Me Then
                    objReg.RaiseValidationError(Me, "Split applied to budget trx has wrong objBudget")
                End If
                If objSplit.strBudgetKey <> mstrBudgetKey Then
                    objReg.RaiseValidationError(Me, "Split applied to budget trx has wrong budget key")
                End If
            Next objSplit
            If curTotal <> mcurBudgetApplied Then
                objReg.RaiseValidationError(Me, "Budget trx applied splits add up wrong")
            End If
        End If
        If Not mblnFake Then
            objReg.RaiseValidationError(Me, "Budget trx must be fake")
        End If
    End Sub

    Public Overrides Function strSummary() As String
        Return Me.datDate.ToShortDateString() + " " + Me.strDescription + " " + Me.curBudgetLimit.ToString()
    End Function

    Public Overrides ReadOnly Property intTrxTypeSortKey As Integer
        Get
            Return 3
        End Get
    End Property

    Public Overrides ReadOnly Property strDocNumberSortKey As String
        Get
            Return ""
        End Get
    End Property

    Public Overrides ReadOnly Property intAmountSortKey As Integer
        Get
            'Have to use mcurBudgetLimit instead of mcurAmount because the algorithms that
            'insert, update and delete transactions in the register require that only
            'the transaction being inserted, updated or deleted can have its register
            'position changed by that operation. BudgetTrx.mcurAmount can change when
            'a split is applied to it, which could cause this rule to be violated if
            'mcurAmount is used by this property.
            'So we use mcurBudgetLimit instead, which is never changes except when
            'inserting, updating or deleting the budget.

            Return If(mcurBudgetLimit > 0, 0, 1)
            'Return MyBase.intAmountSortKey()
        End Get
    End Property
End Class
