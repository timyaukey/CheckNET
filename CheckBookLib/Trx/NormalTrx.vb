﻿Option Strict On
Option Explicit On

''' <summary>
''' A Trx subclass representing a transaction that exists somewhere in the real
''' world, like a credit card company or a bank. The actual amount of the transaction
''' is represented by a collection of Split objects. Each NormalTrx has at least
''' one TrxSplit object.
''' </summary>

Public Class NormalTrx
    Inherits Trx

    'Unique key identifying an external transaction imported to create this NormalTrx.
    'Used for matching new imported NormalTrx against prior imports, to avoid duplicates.
    Protected mstrImportKey As String
    'True iff any split with a budget key was not matched to a BudgetTrx,
    'and the NormalTrx date was not before the earliest budget in the register.
    Protected mblnAnyUnmatchedBudget As Boolean
    'Trx amount may be different from a matching NormalTrx
    'by this amount, either positive or negative.
    Protected mcurNormalMatchRange As Decimal
    'Collection of TrxSplit objects belonging to this Trx.
    Protected mcolSplits As List(Of TrxSplit)

    Public Sub New(ByVal objReg_ As Register)
        MyBase.New(objReg_)
    End Sub

    '$Description Initialize a new normal Trx object. Normally followed by calls to
    '   AddSplit(), and finally either Register.NewLoadEnd() or Register.NewAddEnd().
    '$Param objReg The Register this Trx will be added to. May be Nothing, in which case
    '   the caller is responsible for calling ClearFirstAffected() for the appropriate
    '   Register before NewAddEnd().

    Public Sub NewStartNormal(ByVal blnWillAddToRegister As Boolean, ByVal strNumber_ As String, ByVal datDate_ As Date,
                              ByVal strDescription_ As String, ByVal strMemo_ As String, ByVal lngStatus_ As TrxStatus,
                              ByVal blnFake_ As Boolean, ByVal curNormalMatchRange_ As Decimal,
                              ByVal blnAwaitingReview_ As Boolean, ByVal blnAutoGenerated_ As Boolean,
                              ByVal intRepeatSeq_ As Integer, ByVal strImportKey_ As String,
                              ByVal strRepeatKey_ As String)

        If blnWillAddToRegister Then
            mobjReg.ClearFirstAffected()
        End If

        'NOTE: NewStartNormal() and NewEmptyNormal() must set the same properties.
        mstrNumber = strNumber_
        mdatDate = datDate_
        mstrDescription = strDescription_
        mstrMemo = strMemo_
        mlngStatus = lngStatus_
        mblnFake = blnFake_
        mcurNormalMatchRange = curNormalMatchRange_
        mblnAwaitingReview = blnAwaitingReview_
        mblnAutoGenerated = blnAutoGenerated_
        mintRepeatSeq = intRepeatSeq_
        mstrImportKey = strImportKey_
        mstrRepeatKey = strRepeatKey_

        ClearNormal()

        RaiseErrorOnBadData("NewStartNormal")

    End Sub

    Public Sub NewStartNormal(ByVal blnWillAddToRegister As Boolean, ByVal strNumber_ As String, ByVal datDate_ As Date,
                              ByVal strDescription_ As String, ByVal strMemo_ As String, ByVal lngStatus_ As TrxStatus,
                              ByVal objData As TrxGenImportData)
        With objData
            NewStartNormal(blnWillAddToRegister, strNumber_, datDate_, strDescription_, strMemo_, lngStatus_,
                .blnFake, .curNormalMatchRange, .blnAwaitingReview, .blnAutoGenerated,
                .intRepeatSeq, .strImportKey, .strRepeatKey)
        End With
    End Sub

    Public Sub NewStartNormal(ByVal blnWillAddToRegister As Boolean, ByVal objTrx As NormalTrx)
        With objTrx
            NewStartNormal(blnWillAddToRegister, .strNumber, .datDate, .strDescription, .strMemo, .lngStatus,
                .blnFake, .curNormalMatchRange, .blnAwaitingReview, .blnAutoGenerated, .intRepeatSeq, .strImportKey,
                .strRepeatKey)
        End With
    End Sub

    '$Description Initial a new normal Trx object with all default values.

    Public Sub NewEmptyNormal(ByVal datDate_ As Date)

        'NOTE: NewStartNormal() and NewEmptyNormal() must set the same properties.
        mstrNumber = ""
        mdatDate = datDate_
        mstrDescription = ""
        mstrMemo = ""
        mlngStatus = TrxStatus.glngTRXSTS_UNREC
        mblnFake = False
        mcurNormalMatchRange = 0.0D
        mblnAwaitingReview = False
        mblnAutoGenerated = False
        mintRepeatSeq = 0
        mstrImportKey = ""
        mstrRepeatKey = ""
        ClearNormal()
    End Sub

    Private Sub ClearNormal()
        mcurAmount = 0
        mcurBalance = 0
        mcolSplits = New List(Of TrxSplit)
    End Sub

    '$Description Update all updatable properties of this normal Trx object.
    '   Un-applies any existing splits, then clears the existing splits and sets
    '   curAmount to zero. Normally followed by calls to AddSplit(), and finally
    '   Register.UpdateEnd().

    Public Sub UpdateStartNormal(ByVal strNumber_ As String, ByVal datDate_ As Date, ByVal strDescription_ As String,
                                 ByVal strMemo_ As String, ByVal lngStatus_ As TrxStatus, ByVal blnFake_ As Boolean,
                                 ByVal curNormalMatchRange_ As Decimal, ByVal blnAwaitingReview_ As Boolean,
                                 ByVal blnAutoGenerated_ As Boolean, ByVal intRepeatSeq_ As Integer,
                                 ByVal strImportKey_ As String, ByVal strRepeatKey_ As String)

        mstrNumber = strNumber_
        mdatDate = datDate_
        mstrDescription = strDescription_
        mstrMemo = strMemo_
        mlngStatus = lngStatus_
        mblnFake = blnFake_
        mcurNormalMatchRange = curNormalMatchRange_
        mblnAwaitingReview = blnAwaitingReview_
        mblnAutoGenerated = blnAutoGenerated_
        mintRepeatSeq = intRepeatSeq_
        mstrImportKey = strImportKey_
        mstrRepeatKey = strRepeatKey_

        ClearSplits()

        RaiseErrorOnBadData("UpdateStartNormal")

    End Sub

    Public Sub ClearSplits()
        mcurAmount = 0
        mcurBalance = 0
        mcolSplits = New List(Of TrxSplit)
    End Sub

    Public Sub UpdateStartNormal(ByVal objTrx As NormalTrx)
        With objTrx
            UpdateStartNormal(.strNumber, .datDate, .strDescription, .strMemo, .lngStatus, .blnFake,
                              .curNormalMatchRange, .blnAwaitingReview, .blnAutoGenerated, .intRepeatSeq,
                              .strImportKey, .strRepeatKey)
        End With
    End Sub

    Public Overrides ReadOnly Property lngType As TrxType
        Get
            Return TrxType.glngTRXTYP_NORMAL
        End Get
    End Property

    Public ReadOnly Property strImportKey() As String
        Get
            strImportKey = mstrImportKey
        End Get
    End Property

    Public ReadOnly Property curNormalMatchRange() As Decimal
        Get
            curNormalMatchRange = mcurNormalMatchRange
        End Get
    End Property

    Public ReadOnly Property blnAnyUnmatchedBudget() As Boolean
        Get
            blnAnyUnmatchedBudget = mblnAnyUnmatchedBudget
        End Get
    End Property

    Public ReadOnly Property lngSplits() As Integer
        Get
            lngSplits = mcolSplits.Count()
        End Get
    End Property

    Public ReadOnly Property objFirstSplit() As TrxSplit
        Get
            Return mcolSplits.Item(0)
        End Get
    End Property

    Public ReadOnly Property objSecondSplit() As TrxSplit
        Get
            Return mcolSplits.Item(1)
        End Get
    End Property

    Public ReadOnly Property colSplits() As IEnumerable(Of TrxSplit)
        Get
            Return mcolSplits
        End Get
    End Property

    Public Overrides ReadOnly Property strCategory As String
        Get
            Dim objSplit As TrxSplit
            Dim strCategoryKey As String = ""

            For Each objSplit In Me.colSplits
                If strCategoryKey = "" Then
                    strCategoryKey = objSplit.strCategoryKey
                ElseIf strCategoryKey <> objSplit.strCategoryKey Then
                    Return "(mixed)"
                End If
            Next objSplit
            Return mobjReg.objAccount.objCompany.objCategories.strTranslateKey(strCategoryKey)
        End Get
    End Property

    Public Function strSummarizeDueDate() As String

        Dim objSplit As TrxSplit
        Dim datDueDate As Date = System.DateTime.FromOADate(0)

        For Each objSplit In Me.colSplits
            If datDueDate = System.DateTime.FromOADate(0) Then
                datDueDate = objSplit.datDueDate
            ElseIf datDueDate <> objSplit.datDueDate And objSplit.datDueDate <> System.DateTime.FromOADate(0) Then
                Return "(mixed)"
            End If
        Next objSplit
        If datDueDate = System.DateTime.FromOADate(0) Then
            Return ""
        Else
            Return datDueDate.ToString(Utilities.strDateWithTwoDigitYear)
        End If

    End Function

    Public Function strSummarizeInvoiceDate() As String

        Dim objSplit As TrxSplit
        Dim datInvoiceDate As Date = System.DateTime.FromOADate(0)

        For Each objSplit In Me.colSplits
            If datInvoiceDate = System.DateTime.FromOADate(0) Then
                datInvoiceDate = objSplit.datInvoiceDate
            ElseIf datInvoiceDate <> objSplit.datInvoiceDate And objSplit.datInvoiceDate <> System.DateTime.FromOADate(0) Then
                Return "(mixed)"
            End If
        Next objSplit
        If datInvoiceDate = System.DateTime.FromOADate(0) Then
            Return ""
        Else
            Return datInvoiceDate.ToString(Utilities.strDateWithTwoDigitYear)
        End If

    End Function

    Public Function strSummarizePONumber() As String

        Dim objSplit As TrxSplit
        Dim strPONumber As String = ""

        For Each objSplit In Me.colSplits
            If strPONumber = "" Then
                strPONumber = objSplit.strPONumber
            ElseIf strPONumber <> objSplit.strPONumber And objSplit.strPONumber <> "" Then
                Return "(mixed)"
            End If
        Next objSplit
        Return strPONumber

    End Function

    Public Function strSummarizeTerms() As String

        Dim objSplit As TrxSplit
        Dim strTerms As String = ""

        For Each objSplit In Me.colSplits
            If strTerms = "" Then
                strTerms = objSplit.strTerms
            ElseIf strTerms <> objSplit.strTerms And objSplit.strTerms <> "" Then
                Return "(mixed)"
            End If
        Next objSplit
        Return strTerms

    End Function

    Public Function strSummarizeInvoiceNum() As String

        Dim objSplit As TrxSplit
        Dim strInvNumber As String = ""

        For Each objSplit In Me.colSplits
            If strInvNumber = "" Then
                strInvNumber = objSplit.strInvoiceNum
            ElseIf strInvNumber <> objSplit.strInvoiceNum And objSplit.strInvoiceNum <> "" Then
                Return "(mixed)"
            End If
        Next objSplit
        Return strInvNumber

    End Function

    Public Sub SummarizeSplits(ByVal objCompany As Company, ByRef strCategory As String, ByRef strPONumber As String,
                                ByRef strInvoiceNum As String, ByRef strInvoiceDate As String, ByRef strDueDate As String,
                               ByRef strTerms As String, ByRef strBudget As String, ByRef curAvailable As Decimal)

        Dim objSplit As TrxSplit
        Dim strCatKey As String = ""
        Dim strPONumber2 As String = ""
        Dim strInvoiceNum2 As String = ""
        Dim datInvoiceDate As Date
        Dim datDueDate As Date
        Dim strTerms2 As String = ""
        Dim strBudgetKey As String = ""
        Dim blnFirstSplit As Boolean

        blnFirstSplit = True
        curAvailable = 0
        For Each objSplit In Me.colSplits
            If objSplit.strBudgetKey = objCompany.strPlaceholderBudgetKey Then
                curAvailable = curAvailable + objSplit.curAmount
            End If
            If blnFirstSplit Then
                'Remember fields from the first split.
                strCatKey = objSplit.strCategoryKey
                strPONumber2 = objSplit.strPONumber
                strInvoiceNum2 = objSplit.strInvoiceNum
                datInvoiceDate = objSplit.datInvoiceDate
                datDueDate = objSplit.datDueDate
                strTerms2 = objSplit.strTerms
                strBudgetKey = objSplit.strBudgetKey
                'Format fields from the first split.
                strCategory = objCompany.objCategories.strTranslateKey(strCatKey)
                strInvoiceNum = strInvoiceNum2
                strPONumber = strPONumber2
                If datInvoiceDate = System.DateTime.FromOADate(0) Then
                    strInvoiceDate = ""
                Else
                    strInvoiceDate = Utilities.strFormatDate(datInvoiceDate)
                End If
                If datDueDate = System.DateTime.FromOADate(0) Then
                    strDueDate = ""
                Else
                    strDueDate = Utilities.strFormatDate(datDueDate)
                End If
                strTerms = strTerms2
                strBudget = objCompany.objBudgets.strTranslateKey(strBudgetKey)
                blnFirstSplit = False
            Else
                If strCatKey <> objSplit.strCategoryKey Then
                    strCategory = "(mixed)"
                End If
                If strPONumber2 <> objSplit.strPONumber Then
                    strPONumber = "(mixed)"
                End If
                If strInvoiceNum2 <> objSplit.strInvoiceNum Then
                    strInvoiceNum = "(mixed)"
                End If
                If datInvoiceDate <> objSplit.datInvoiceDate Then
                    strInvoiceDate = "(mixed)"
                End If
                If datDueDate <> objSplit.datDueDate Then
                    strDueDate = "(mixed)"
                End If
                If strTerms2 <> objSplit.strTerms Then
                    strTerms = "(mixed)"
                End If
                If strBudgetKey <> objSplit.strBudgetKey Then
                    strBudget = "(mixed)"
                End If
            End If
        Next objSplit

    End Sub

    Public Overrides Sub UnApply()
        'Regenerating generated Trx must cause this and Apply() to be called.
        UnApplyFromBudgets()
        DeleteReplicaTrx()
    End Sub

    Public Overrides Sub Apply(ByVal blnLoading As Boolean)
        ApplyToBudgets()
        CreateReplicaTrx(blnLoading)
    End Sub

    '$Description Apply the Split objects in this Trx to any matching budgets.
    '   Does nothing except for normal Trx.

    Public Sub ApplyToBudgets()
        Dim blnNoMatch As Boolean
        mblnAnyUnmatchedBudget = False
        For Each objSplit As TrxSplit In mcolSplits
            If objSplit.objBudget Is Nothing Then
                objSplit.ApplyToBudget(objReg, mdatDate, blnNoMatch)
                mblnAnyUnmatchedBudget = mblnAnyUnmatchedBudget Or blnNoMatch
            End If
        Next
    End Sub

    Public Sub CreateReplicaTrx(ByVal blnLoading As Boolean)
        If Not objReg.objAccount Is Nothing Then
            Dim objCompany As Company = objReg.objAccount.objCompany
            For Each objSplit As TrxSplit In mcolSplits
                objSplit.CreateReplicaTrx(objCompany, Me, blnLoading)
            Next
        End If
    End Sub

    '$Description If Split objects for this Trx have been applied to any budgets,
    '   un-apply them from those budgets. No error or other reporting if Split objects
    '   not currently applied to budgets. Does nothing except for normal Trx.

    Public Sub UnApplyFromBudgets()
        For Each objSplit As TrxSplit In mcolSplits
            objSplit.UnApplyFromBudget(objReg)
        Next objSplit
    End Sub

    Public Sub DeleteReplicaTrx()
        For Each objSplit As TrxSplit In mcolSplits
            objSplit.DeleteReplicaTrx()
        Next
    End Sub

    '$Description Update a Trx as a result of matching it to imported bank data.

    Public Sub ImportUpdateBank(ByVal datDate_ As Date, ByVal strNumber_ As String, ByVal curAmount_ As Decimal, ByVal strImportKey_ As String)
        ImportUpdateShared()
        If Not IsNumeric(strNumber_) Then
            mdatDate = datDate_
        End If
        mstrNumber = strNumber_
        AdjustSplitsProportionally(curAmount_)
        mstrImportKey = strImportKey_
    End Sub

    '$Description Update a Trx with new number and amount.

    Public Sub ImportUpdateNumAmt(ByVal strNumber_ As String, ByVal curAmount_ As Decimal)
        ImportUpdateShared()
        mstrNumber = strNumber_
        AdjustSplitsProportionally(curAmount_)
    End Sub

    '$Description Update a generated Trx with new amount, and make it non-generated.

    Public Sub ImportUpdateAmount(ByVal curAmount_ As Decimal)
        ImportUpdateShared()
        AdjustSplitsProportionally(curAmount_)
    End Sub

    Protected Sub ImportUpdateShared()
        mblnAutoGenerated = False
        mblnFake = False
    End Sub

    '$Description Adjust split amounts to add up to a new total amount
    '   but retain the same proportions relative to each other. Does nothing
    '   if the Trx amount doesn't change, and assigns the entire amount to one
    '   split if either new or old amount is zero.
    '   Sets Trx amount to curNewAmount.

    Protected Sub AdjustSplitsProportionally(ByVal curNewAmount As Decimal)
        Dim dblRatio As Double
        Dim curRemainder As Decimal
        Dim objSplit As TrxSplit
        Dim lngSplit As Integer
        Dim curThisSplit As Decimal

        If curNewAmount = mcurAmount Then
            Exit Sub
        End If
        'The proportional distribution algorithm breaks if either amount is zero.
        If curNewAmount = 0 Or mcurAmount = 0 Then
            lngSplit = 0
            For Each objSplit In mcolSplits
                lngSplit = lngSplit + 1
                If lngSplit = mcolSplits.Count() Then
                    objSplit.AdjustAmount(curNewAmount)
                    mcurAmount = curNewAmount
                Else
                    objSplit.AdjustAmount(0D)
                End If
            Next
            Exit Sub
        End If
        'Use proportional distribution.
        dblRatio = curNewAmount / mcurAmount
        curRemainder = curNewAmount
        lngSplit = 0
        mcurAmount = 0
        For Each objSplit In mcolSplits
            lngSplit = lngSplit + 1
            If lngSplit = mcolSplits.Count() Then
                objSplit.AdjustAmount(curRemainder)
                mcurAmount = mcurAmount + curRemainder
            Else
                curThisSplit = CType(System.Math.Round(objSplit.curAmount * dblRatio, 2), Decimal)
                objSplit.AdjustAmount(curThisSplit)
                curRemainder = curRemainder - objSplit.curAmount
                mcurAmount = mcurAmount + curThisSplit
            End If
        Next objSplit
    End Sub

    '$Description Add a new split to a Trx and subtract that amount from another
    'split in the same Trx. Normally both splits have the same PO# and this operation
    'represents applying an invoice to an open purchase order, but this routine
    'does not check the PO#.

    Public Sub ImportUpdatePurchaseOrder(ByVal objPOSplit As TrxSplit, ByVal objImportedSplit As TrxSplit)

        'A split with a PO# represents part of a purchase order due on that date.
        'If the split has no invoice number it represents a part of that purchase
        'order date that hasn't been matched to an invoice. If the split has an
        'invoice number it represents an invoice matched to that purchase order.
        'There should be only one split for a particular PO# and no invoice number
        'on a particular date, representing the uninvoiced portion of the purchase
        'order due on that date.
        'All splits related to the same purchase order and due on the same date
        'must belong to the same Trx, or Trx import will not divide the unreceived
        'portion of the purchase order correctly when applying invoices.
        With objImportedSplit
            objPOSplit.AdjustAmount(objPOSplit.curAmount - .curAmount)
            mcurAmount = mcurAmount - .curAmount
            AddSplit(.strMemo, .strCategoryKey, .strPONumber, .strInvoiceNum, .datInvoiceDate, .datDueDate,
                     .strTerms, objPOSplit.strBudgetKey, .curAmount)
        End With

    End Sub

    Private Sub SetSplitDocInfo(ByVal strPONumber_ As String, ByVal strInvoiceNum_ As String, ByVal datInvoiceDate_ As Date,
                                ByVal datDueDate_ As Date, ByVal strTerms_ As String)
        Dim objSplit As TrxSplit
        For Each objSplit In mcolSplits
            With objSplit
                .strPONumber = strPONumber_
                .strInvoiceNum = strInvoiceNum_
                .datInvoiceDate = datInvoiceDate_
                .datDueDate = datDueDate_
                .strTerms = strTerms_
            End With
        Next objSplit
    End Sub

    '$Description Add a split to the Trx. Updates the overall Trx amount as well,
    '   but does not apply to budget because the budget Trx may not exist yet.
    '   This is the only valid way to modify the splits, or the Trx amount.

    Public Sub AddSplit(ByVal strMemo_ As String, ByVal strCategoryKey_ As String, ByVal strPONumber_ As String,
                        ByVal strInvoiceNum_ As String, ByVal datInvoiceDate_ As Date, ByVal datDueDate_ As Date,
                        ByVal strTerms_ As String, ByVal strBudgetKey_ As String, ByVal curAmount_ As Decimal)

        Dim objSplit As TrxSplit
        objSplit = New TrxSplit
        objSplit.Init(strMemo_, strCategoryKey_, strPONumber_, strInvoiceNum_, datInvoiceDate_, datDueDate_, strTerms_, strBudgetKey_, curAmount_)
        mcolSplits.Add(objSplit)
        mcurAmount = mcurAmount + curAmount_

    End Sub

    '$Description Like AddSplit(), but clones an existing split and returns the new split.

    Public Function objAddSplit(ByVal objSrcSplit As TrxSplit) As TrxSplit
        Dim objDstSplit As TrxSplit
        objDstSplit = New TrxSplit
        With objSrcSplit
            objDstSplit.Init(.strMemo, .strCategoryKey, .strPONumber, .strInvoiceNum, .datInvoiceDate, .datDueDate, .strTerms, .strBudgetKey, .curAmount)
            mcolSplits.Add(objDstSplit)
            mcurAmount = mcurAmount + .curAmount
        End With
        objAddSplit = objDstSplit
    End Function

    Public Sub CopySplits(ByVal objDstTrx As NormalTrx)
        Dim objSrcSplit As TrxSplit
        For Each objSrcSplit In mcolSplits
            objDstTrx.objAddSplit(objSrcSplit)
        Next objSrcSplit
    End Sub

    Public Overrides Function objClone(ByVal blnWillAddToRegister As Boolean) As Trx
        Dim objNormalTrx As NormalTrx = New NormalTrx(mobjReg)
        objNormalTrx.NewStartNormal(blnWillAddToRegister, Me)
        CopySplits(objNormalTrx)
        Return objNormalTrx
    End Function

    Public Function objGetTrxManager() As NormalTrxManager
        Return New NormalTrxManager(Me)
    End Function

    Public Overrides Sub Validate()
        Dim objSplit As TrxSplit
        Dim curTotal As Decimal
        Dim objCategories As CategoryTranslator = mobjReg.objAccount.objCompany.objCategories
        Dim blnAccountIsPersonal As Boolean = (mobjReg.objAccount.lngType = Account.AccountType.Personal)
        MyBase.Validate()
        If mcolSplits Is Nothing Then
            objReg.RaiseValidationError(lngIndex, "Missing split collection")
        Else
            curTotal = 0
            For Each objSplit In mcolSplits
                curTotal = curTotal + objSplit.curAmount
                If Not objSplit.objBudget Is Nothing Then
                    If objSplit.objBudget.lngType <> TrxType.glngTRXTYP_BUDGET Then
                        objReg.RaiseValidationError(lngIndex, "Split applied to non-budget trx")
                    End If
                    If objSplit.strBudgetKey = "" Then
                        objReg.RaiseValidationError(lngIndex, "Split applied to budget trx has no budget key")
                    End If
                    If objSplit.strBudgetKey <> objSplit.objBudget.strBudgetKey Then
                        objReg.RaiseValidationError(lngIndex, "Split applied to budget trx has wrong budget key")
                    End If
                End If
                If blnAccountIsPersonal <> CategoryTranslator.blnIsPersonal(objCategories.strKeyToValue1(objSplit.strCategoryKey)) Then
                    objReg.RaiseValidationError(lngIndex, "Split category mixes personal and business")
                End If
                Dim intDotOffset As Integer = objSplit.strCategoryKey.IndexOf("."c)
                If intDotOffset > 0 Then
                    Dim intAccountKey As Integer = Integer.Parse(objSplit.strCategoryKey.Substring(0, intDotOffset))
                    If intAccountKey = objReg.objAccount.intKey Then
                        objReg.RaiseValidationError(lngIndex, "Split category uses the same account")
                    End If
                End If
            Next objSplit
            If curTotal <> mcurAmount Then
                objReg.RaiseValidationError(lngIndex, "Normal trx splits add up wrong")
            End If
        End If
        If mblnFake Then
            If mstrImportKey <> "" Then
                objReg.RaiseValidationError(lngIndex, "Normal trx cannot have import key if it is fake")
            End If
            If mlngStatus <> TrxStatus.glngTRXSTS_UNREC Then
                objReg.RaiseValidationError(lngIndex, "Normal trx must be unreconciled if it is fake")
            End If
        End If
    End Sub
End Class
