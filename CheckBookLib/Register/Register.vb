Option Strict On
Option Explicit On

''' <summary>
''' A transaction register, which is a list of BaseTrx objects from one Account and
''' a running balance of those transactions. Use .objTrx() to access those transactions.
''' The running balance is computed from scratch each time it is loaded.
''' Any transactions which are transfers to or from other registers must have those
''' registers loaded as well, which generally means all registers must be loaded.
''' </summary>

Public Class Register
    'Parent Account.
    Private mobjAccount As Account
    'Number of elements allocated in maobjTrx().
    Private mlngTrxAllocated As Integer
    'Number of elements used in maobjTrx(). May be < mlngTrxAllocated.
    Private mlngTrxUsed As Integer
    'Array of BaseTrx objects in register.
    Private maobjTrx() As BaseTrx
    'Index of first BaseTrx affected by NewAddEnd() or UpdateEnd().
    Private mlngFirstAffected As Integer
    'Special value for mlngFirstAffected indicating no BaseTrx affected.
    Private Const mlngNO_TRX_AFFECTED As Integer = 9999999
    'Register title.
    Private mstrTitle As String
    'Unique identifier for register.
    Private mstrRegisterKey As String
    'Number of maobjTrx() array elements allocated at a time.
    Private mlngAllocationUnit As Integer
    'Show this register initially in the UI.
    Private mblnShowInitially As Boolean
    'The current BaseTrx. Used only by the UI.
    Private mobjTrxCurrent As BaseTrx
    'Budget BaseTrx with ending date older than this are always zero amount.
    Private mdatOldestBudgetEndAllowed As Date
    'Collection of all BaseTrx that are part of a repeating sequence,
    'keyed by "#" & strRepeatKey & "." & intRepeatSeq.
    Private mcolRepeatTrx As Dictionary(Of String, BaseTrx)
    'EventLog for Register.
    Private mobjLog As EventLog
    'True iff operator deleted register.
    Private mblnDeleted As Boolean
    'True iff in a critical operation.
    Private mblnInCriticalOperation As Boolean
    'True iff a critical operation failed.
    Private mblnCriticalOperationFailed As Boolean

    'Fired by NewAddEnd() when it adds a BaseTrx to the register.
    'Intended to allow the UI to update itself.
    Public Event TrxAdded(ByVal objTrx As BaseTrx)

    'Fired by UpdateEnd() when it updates a BaseTrx in the register.
    'Intended to allow the UI to update itself.
    Public Event TrxUpdated(ByVal blnPositionChanged As Boolean, ByVal objTrx As BaseTrx)

    'Fired by Delete().
    'Intended to allow the UI to update itself.
    'Values returned by objTrx.objNext and objTrx.objPrevious are undefined.
    'Value of objTrx.lngIndex is unchanged from before the delete.
    Public Event TrxDeleted(ByVal objTrx As BaseTrx)

    'Fired when a Split is applied or un-applied to a budget BaseTrx.
    'Intended to allow the UI to update itself.
    Public Event BudgetChanged(ByVal objBudget As BaseTrx)

    'Fired after register balances have been updated.
    'Intended to allow the UI to update itself.
    Public Event ManyTrxChanged()

    'Fired by ShowCurrent() method.
    'Intended to allow the UI to update itself.
    Public Event ShowCurrent(ByVal objTrx As BaseTrx)

    'Fired to say that the Register has been changed but no BaseTrx were changed,
    'e.g. the register title or some other property changed.
    Public Event MiscChange()

    'Fired before recreating generated transactions,
    'to allow clients to temporarily hide their UI.
    'Will be fired for all registers in all accounts before any
    'changes are made in any registers.
    Public Event BeginRegenerating()

    'Fired after recreating generated transactions.
    'Will be fired after all changes are made in all registers in all accounts.
    Public Event EndRegenerating()

    'Fired by Validate() or a method called from Validate() when a validation
    'error is detected.
    Public Event ValidationError(ByVal objTrx As BaseTrx, ByVal strMsg As String)

    Friend Sub Init(ByVal objAccount_ As Account, ByVal strTitle_ As String, ByVal strRegisterKey_ As String, ByVal blnShowInitially_ As Boolean, ByVal lngAllocationUnit_ As Integer)

        mobjAccount = objAccount_
        mstrTitle = strTitle_
        mstrRegisterKey = strRegisterKey_
        mlngAllocationUnit = lngAllocationUnit_
        mblnShowInitially = blnShowInitially_
        If mstrRegisterKey = "" Then
            gRaiseError("Missing register key in Register.Init")
        End If
        mdatOldestBudgetEndAllowed = DateTime.MinValue
        mcolRepeatTrx = New Dictionary(Of String, BaseTrx)
        Erase maobjTrx
        mlngTrxAllocated = 0
        mlngTrxUsed = 0
        mobjTrxCurrent = Nothing
        ClearFirstAffected()
        mblnInCriticalOperation = False
        mblnCriticalOperationFailed = False
        mobjLog = New EventLog
        mobjLog.Init(Me, mobjAccount.objCompany.objSecurity.strLogin)

    End Sub

    '$Description Add a BaseTrx object to the register at the correct place in the sort order,
    '   as part of a register loading operation.
    '   Does not search for any kind of match to this BaseTrx, apply to budgets, or adjust
    '   curBalance properties. Optimized for the case where objNew falls near or at the
    '   end of the sort order.
    '$Param objNew BaseTrx object to add. This actual object instance will
    '   become the register entry, rather than making a copy of it.

    Public Sub NewLoadEnd(ByVal objNew As BaseTrx)
        BeginCriticalOperation()
        ExpandTrxArray()
        SetTrx(mlngTrxUsed, objNew)
        If objNew.intRepeatSeq > 0 Then
            AddRepeatTrx(objNew)
        End If
        EndCriticalOperation()
    End Sub

    '$Description Add a BaseTrx object to the register at the correct place in the sort order,
    '   as part of adding a BaseTrx to an already loaded register. Optimized for the case
    '   where objNew falls near or at the end of the sort order.
    '$Param objNew BaseTrx object to add. This actual object instance will
    '   become the register entry, rather than making a copy of it.

    Public Sub NewAddEnd(ByVal objNew As BaseTrx, ByVal objAddLogger As ILogAdd, ByVal strTitle As String,
                         Optional ByVal blnSetChanged As Boolean = True)

        BeginCriticalOperation()
        If objNew Is Nothing Then
            gRaiseError("objNew is Nothing in Register.NewAddEnd")
        End If
        NewInsert(objNew)
        objNew.Apply(False)
        RaiseEvent TrxAdded(objNew)
        If blnSetChanged Then
            mobjAccount.SetChanged()
        End If
        UpdateFirstAffected(objNew.lngIndex)
        If objNew.intRepeatSeq > 0 Then
            SetRepeatTrx(objNew)
        End If
        FixBalancesAndRefreshUI()
        mobjLog.AddILogAdd(objAddLogger, strTitle, objNew)
        EndCriticalOperation()

    End Sub

    '$Description Insert a BaseTrx in the correct place in the sort order.
    '   Optimized for the case where objNew falls near or at the end of the sort order.
    '$Param objNew BaseTrx object to add. This actual object instance will
    '   become the register entry, rather than making a copy of it.
    '$Returns The index of the new transaction in the register.

    Private Sub NewInsert(ByVal objNew As BaseTrx)
        ExpandTrxArray()
        MoveUp(mlngTrxUsed - 1, objNew)
        mobjTrxCurrent = objNew
    End Sub

    Private Sub ExpandTrxArray()
        If mlngTrxUsed = mlngTrxAllocated Then
            mlngTrxAllocated = mlngTrxAllocated + mlngAllocationUnit
            ReDim Preserve maobjTrx(mlngTrxAllocated)
        End If
        mlngTrxUsed = mlngTrxUsed + 1
    End Sub

    '$Description Move a BaseTrx the shortest distance possible toward the beginning of the
    '   register which will leave it in the correct place in the sort order. Will not
    '   change the register if the BaseTrx is already in the correct place.
    '$Param lngEndIndex The index immediately above the current location of the BaseTrx
    '   being moved. maobjTrx(lngEndIndex+1) does not actually have to refer to objTrx,
    '   but this method will act as if it is in the sense that it will make room for
    '   it at the new location by moving everything between the new location and
    '   lngEndIndex down one row in the register, thus overwriting the object currently
    '   in that position.
    '$Param objTrx The BaseTrx object to move. Will insert this in maobjTrx().

    Private Sub MoveUp(ByVal lngEndIndex As Integer, ByVal objTrx As BaseTrx)
        Dim lngFirstLesser As Integer
        Dim lngMoveIndex As Integer
        If lngEndIndex < 0 Or lngEndIndex > (mlngTrxUsed - 1) Then
            gRaiseError("Register.lngMoveUp passed invalid lngEndIndex=" & lngEndIndex)
        End If
        'When the loop is done, lngFirstLesser will equal the largest index whose BaseTrx
        'sort key is less than objNew.strSortKey, or zero if there is none.
        For lngFirstLesser = lngEndIndex To 1 Step -1
            If Register.intSortComparison(objTrx, maobjTrx(lngFirstLesser)) >= 0 Then
                Exit For
            End If
        Next
        If (lngFirstLesser + 1) < 1 Then
            gRaiseError("Register.lngMoveUp moved too far")
        End If
        If lngFirstLesser > lngEndIndex Then
            gRaiseError("Register.lngMoveUp did not move far enough")
        End If
        For lngMoveIndex = lngEndIndex To lngFirstLesser + 1 Step -1
            SetTrx(lngMoveIndex + 1, maobjTrx(lngMoveIndex))
        Next
        SetTrx(lngFirstLesser + 1, objTrx)
    End Sub

    '$Description Like MoveUp(), but moves a BaseTrx the shortest distance possible
    '   toward the end of the register.
    '$Param lngStartIndex The index immediately below the current location of the BaseTrx
    '   being moved. See lngEndIndex argument to lngMoveUp().
    '$Param objTrx See lngMoveUp().

    Private Sub MoveDown(ByVal lngStartIndex As Integer, ByVal objTrx As BaseTrx)
        Dim lngFirstGreater As Integer
        Dim lngMoveIndex As Integer
        If lngStartIndex < 2 Or lngStartIndex > mlngTrxUsed Then
            gRaiseError("Register.lngMoveDown passed invalid lngStartIndex " & lngStartIndex)
        End If
        'When the loop is done, lngFirstGreater will equal the smallest index whose BaseTrx
        'sort key is greater than objNew.strSortKey, or zero if there is none.
        For lngFirstGreater = lngStartIndex To mlngTrxUsed
            If Register.intSortComparison(objTrx, maobjTrx(lngFirstGreater)) <= 0 Then
                Exit For
            End If
        Next
        If (lngFirstGreater - 1) > mlngTrxUsed Then
            gRaiseError("Register.lngMoveDown lngFirstGreater-1=" & lngFirstGreater - 1 & " is greater than mlngTrxUsed=" & mlngTrxUsed)
        End If
        If lngFirstGreater < lngStartIndex Then
            gRaiseError("Register.lngMoveDown lngFirstGreater=" & lngFirstGreater & " is smaller than lngStartIndex=" & lngStartIndex)
        End If
        For lngMoveIndex = lngStartIndex To lngFirstGreater - 1
            SetTrx(lngMoveIndex - 1, maobjTrx(lngMoveIndex))
        Next
        SetTrx(lngFirstGreater - 1, objTrx)
    End Sub

    '$Description Finish updating an existing transaction. Moves it to the correct
    '   position in the sort order.
    '   Is optimized for the case where position in the sort order is not changed much.
    '$Param lngIndex The index of the BaseTrx object in the register to move.

    Friend Sub UpdateEnd(ByVal objTrx As BaseTrx, ByVal objChangeLogger As ILogChange, ByVal strTitle As String, ByVal objOldTrx As BaseTrx)

        Dim lngOldIndex As Integer = objTrx.lngIndex

        Try

            If objTrx.intRepeatSeq > 0 Then
                SetRepeatTrx(objTrx)
            End If
            objTrx.Apply(False)
            UpdateMove(lngOldIndex)
            RaiseEvent TrxUpdated(lngOldIndex <> objTrx.lngIndex, objTrx)
            mobjAccount.SetChanged()
            UpdateFirstAffected(lngOldIndex)
            UpdateFirstAffected(objTrx.lngIndex)
            FixBalancesAndRefreshUI()
            mobjLog.AddILogChange(objChangeLogger, strTitle, objTrx, objOldTrx)

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Move a BaseTrx to the correct position based on sort key.
    '$Param lngOldIndex The index of the BaseTrx in the register to move.
    '$Returns The new index position of the BaseTrx after being moved.
    '   This position is where the BaseTrx lives in the register AFTER it has
    '   been deleted from its old position. For example, if you tell it to
    '   move the BaseTrx at index 3, and the new location is after the BaseTrx in
    '   position 7 prior to the move, then the new location will be 7 and
    '   not 8 because the BaseTrx previously at position 7 was moved up to make
    '   room for it. In other words you can always update a UI by first
    '   deleting a row from the old position and then inserting a row at
    '   the new position.

    Private Sub UpdateMove(ByVal lngOldIndex As Integer)

        Dim objTrx As BaseTrx = Me.objTrx(lngOldIndex)

        If lngOldIndex > 1 Then
            If Register.intSortComparison(objTrx, Me.objTrx(lngOldIndex - 1)) < 0 Then
                MoveUp(lngOldIndex - 1, objTrx)
                Exit Sub
            End If
        End If

        If lngOldIndex < mlngTrxUsed Then
            If Register.intSortComparison(objTrx, Me.objTrx(lngOldIndex + 1)) > 0 Then
                MoveDown(lngOldIndex + 1, objTrx)
                Exit Sub
            End If
        End If

    End Sub

    Public Shared Function intSortComparison(ByVal objTrx1 As BaseTrx, ByVal objTrx2 As BaseTrx) As Integer
        Dim result As Integer = objTrx1.datDate.CompareTo(objTrx2.datDate)
        If result <> 0 Then
            Return result
        End If
        result = objTrx1.intAmountSortKey.CompareTo(objTrx2.intAmountSortKey)
        If result <> 0 Then
            Return result
        End If
        result = objTrx1.intTrxTypeSortKey.CompareTo(objTrx2.intTrxTypeSortKey)
        If result <> 0 Then
            Return result
        End If
        result = String.CompareOrdinal(objTrx1.strNumber, objTrx2.strNumber)
        If result <> 0 Then
            Return result
        End If
        result = String.CompareOrdinal(objTrx1.strDescription, objTrx2.strDescription)
        If result <> 0 Then
            Return result
        End If
        Return String.CompareOrdinal(objTrx1.strDocNumberSortKey, objTrx2.strDocNumberSortKey)
    End Function

    Public Sub Sort()
        If (Not maobjTrx Is Nothing) And (mlngTrxUsed > 0) Then
            Array.Sort(Of BaseTrx)(maobjTrx, 1, mlngTrxUsed, New SortComparer())
            For lngIndex As Integer = 1 To mlngTrxUsed
                maobjTrx(lngIndex).lngIndex = lngIndex
            Next
        End If
    End Sub

    Private Class SortComparer
        Implements IComparer(Of BaseTrx)

        Public Function Compare(objTrx1 As BaseTrx, objTrx2 As BaseTrx) As Integer Implements IComparer(Of BaseTrx).Compare
            Return intSortComparison(objTrx1, objTrx2)
        End Function
    End Class

    Public Sub BeginCriticalOperation()
        If mblnInCriticalOperation Then
            mblnCriticalOperationFailed = True
            Throw New CriticalOperationException("Attempted to begin a critical operation while already in one")
        End If
        mblnInCriticalOperation = True
    End Sub

    Public Class CriticalOperationException
        Inherits InvalidOperationException
        Public Sub New(ByVal strMsg As String)
            MyBase.New(strMsg)
        End Sub
    End Class

    Public Sub EndCriticalOperation()
        If Not mblnInCriticalOperation Then
            mblnCriticalOperationFailed = True
            Throw New Exception("Attempted to end a critical operation when not in one")
        End If
        mblnInCriticalOperation = False
    End Sub

    Public Sub CheckIfInCriticalOperation()
        If mblnInCriticalOperation Then
            mblnCriticalOperationFailed = True
        End If
    End Sub

    Public ReadOnly Property blnCriticalOperationFailed() As Boolean
        Get
            Return mblnCriticalOperationFailed
        End Get
    End Property

    Friend Sub ClearFirstAffected()
        mlngFirstAffected = mlngNO_TRX_AFFECTED
    End Sub

    Private Sub UpdateFirstAffected(ByVal lngIndex As Integer)
        If lngIndex < mlngFirstAffected And lngIndex <= mlngTrxUsed Then
            mlngFirstAffected = lngIndex
        End If
    End Sub

    '$Description Compute curBalance for all BaseTrx for which it may need to be
    '   changed, then if any were changed inform the UI what needs to be updated.
    '   Called after all operations which may cause balances to change in an already
    '   loaded register.

    Private Sub FixBalancesAndRefreshUI()
        'This condition might actually not be true, for example if
        'the last BaseTrx in the register is deleted and it was not
        'applied to any budgets.
        If mlngFirstAffected <> mlngNO_TRX_AFFECTED Then
            FixBalances(mlngFirstAffected)
            FireManyTrxChanged()
        End If
        FireShowCurrent()
    End Sub

    '$Description Set the status of a transaction, and fire a StatusChanged event for it.
    '$Param lngIndex The index of the BaseTrx to change.
    '$Param lngStatus The new status.

    Public Sub SetTrxStatus(ByVal objStatusTrx As BankTrx, ByVal lngStatus As BaseTrx.TrxStatus, ByVal objAddLogger As ILogAdd, ByVal strTitle As String)

        objStatusTrx.lngStatus = lngStatus
        mobjAccount.SetChanged()
        'Use an ILogAdd instead of a specialized logger because it's a cheap
        'hack to reuse an existing type with the correct signature rather than
        'define an entire new one solely for this purpose.
        mobjLog.AddILogAdd(objAddLogger, strTitle, objStatusTrx)

    End Sub

    '$Description Delete a transaction from the register.
    '$Param lngIndex The index of the BaseTrx to delete.

    Public Sub Delete(ByVal objTrx As BaseTrx, ByVal objDeleteLogger As ILogDelete, ByVal strTitle As String,
                      Optional ByVal blnSetChanged As Boolean = True)
        Dim lngMoveIndex As Integer
        Dim objTrxOld As BaseTrx
        BeginCriticalOperation()
        ClearFirstAffected()
        With objTrx
            objTrxOld = .objClone(False)
            objTrx.UnApply()
            If .strRepeatKey <> "" Then
                RemoveRepeatTrx(objTrx)
            End If
        End With
        Dim lngIndex As Integer = objTrx.lngIndex
        For lngMoveIndex = lngIndex + 1 To mlngTrxUsed
            SetTrx(lngMoveIndex - 1, maobjTrx(lngMoveIndex))
        Next
        maobjTrx(mlngTrxUsed) = Nothing
        mlngTrxUsed = mlngTrxUsed - 1
        If mlngFirstAffected > mlngTrxUsed Then
            'Will happen when deleting the last BaseTrx in the register,
            'because will call UnApply() on that BaseTrx.
            mlngFirstAffected = mlngNO_TRX_AFFECTED
        End If
        If blnSetChanged Then
            mobjAccount.SetChanged()
        End If
        mobjTrxCurrent = Nothing
        'Condition will be false if last trx deleted.
        If lngIndex <= mlngTrxUsed Then
            UpdateFirstAffected(lngIndex)
        End If
        RaiseEvent TrxDeleted(objTrx)
        'Still have to "fix balances" even if deleting the last BaseTrx,
        'because that BaseTrx might be applied to budgets.
        FixBalancesAndRefreshUI()
        mobjLog.AddILogDelete(objDeleteLogger, strTitle, objTrxOld)
        EndCriticalOperation()
    End Sub

    '$Description Compute curBalance properties of all BaseTrx objects starting at a
    '   particular index value. Does not stop if it finds a BaseTrx whose curBalance
    '   is the same after being recomputed, for example because a BaseTrx was moved in
    '   the register but its curAmount property did not change. It keeps going because
    '   updating a BaseTrx can cause the curAmount property of the budget BaseTrx it is
    '   applied to to change in many ways, and it is possible the register may be
    '   back "in synch" for a few BaseTrx and then out of synch.
    '$Param lngStartIndex The index of the first BaseTrx whose curBalance property
    '   needs to be computed.
    '$Returns The index of the last BaseTrx whose curBalance property actually changed.
    '   The caller can use this to decide what part of register UI needs to be
    '   refreshed to show new running balance.

    Private Sub FixBalances(ByVal lngStartIndex As Integer)
        Dim curBalance As Decimal
        Dim lngIndex As Integer
        Dim lngLastChange As Integer
        If lngStartIndex < 1 Or (lngStartIndex > mlngTrxUsed And mlngTrxUsed > 0) Then
            gRaiseError("Register.lngFixBalances passed invalid lngStartIndex=" & lngStartIndex)
        End If
        If lngStartIndex = 1 Then
            curBalance = 0
        Else
            curBalance = Me.objTrx(lngStartIndex - 1).curBalance
        End If
        lngLastChange = lngStartIndex
        For lngIndex = lngStartIndex To mlngTrxUsed
            With Me.objTrx(lngIndex)
                curBalance = curBalance + .curBalanceChange
                If curBalance <> .curBalance Then
                    .curBalance = curBalance
                    lngLastChange = lngIndex
                End If
            End With
        Next
    End Sub

    '$Description Call BaseTrx.Apply() on all BaseTrx in this Register after this Register is
    '   loaded with BaseTrx objects from an external database. This may create additional BaseTrx.
    '   Must be done for all BaseTrx in all Registers in all Accounts before calling
    '   LoadFinish() for any Registers in any Accounts.

    Public Sub LoadApply()
        BeginCriticalOperation()
        For lngIndex As Integer = 1 To mlngTrxUsed
            Me.objTrx(lngIndex).Apply(True)
        Next
        EndCriticalOperation()
    End Sub

    '$Description Perform final post processing steps after this Register is
    '   loaded and LoadApply() is called. Computes balances.
    '   The register is ready to display in a UI after this.

    Public Sub LoadFinish()
        Dim curBalance As Decimal = 0
        BeginCriticalOperation()
        mdatOldestBudgetEndAllowed = mobjAccount.datLastReconciled.AddDays(1D)
        For Each objTrx As BaseTrx In colAllTrx(Of BaseTrx)()
            If TypeOf (objTrx) Is BudgetTrx Then
                DirectCast(objTrx, BudgetTrx).SetAmountForBudget()
            End If
            curBalance = curBalance + objTrx.curBalanceChange
            objTrx.curBalance = curBalance
        Next
        EndCriticalOperation()
    End Sub

    '$Description Remove all generated BaseTrx from the Register, and clear
    '   all budget allocations.

    Public Sub PurgeGenerated()
        Dim lngInIndex As Integer
        Dim lngOutIndex As Integer
        Dim objTrx As BaseTrx
        'Dim objSplit As TrxSplit

        If mlngTrxUsed = 0 Then
            Exit Sub
        End If
        BeginCriticalOperation()
        lngOutIndex = 0
        For lngInIndex = 1 To mlngTrxUsed
            objTrx = maobjTrx(lngInIndex)
            objTrx.UnApply()
            If objTrx.blnAutoGenerated Then
                If objTrx.strRepeatKey <> "" Then
                    RemoveRepeatTrx(objTrx)
                End If
            Else
                lngOutIndex = lngOutIndex + 1
                SetTrx(lngOutIndex, objTrx)
            End If
        Next
        If lngOutIndex = 0 Then
            Erase maobjTrx
        Else
            ReDim Preserve maobjTrx(lngOutIndex)
        End If
        mlngTrxUsed = lngOutIndex
        mlngTrxAllocated = mlngTrxUsed
        mobjTrxCurrent = Nothing
        EndCriticalOperation()
    End Sub

    Public Sub FireManyTrxChanged()
        RaiseEvent ManyTrxChanged()
    End Sub

    '$Description Called to tell everyone who cares that many BaseTrx are about
    '   to change, and clients may not want to update their UI until all
    '   the changes are done which is signaled by the RedisplayTrx event.

    Public Sub FireBeginRegenerating()
        RaiseEvent BeginRegenerating()
    End Sub

    '$Description Called to tell everyone who cares that they must redisplay
    '   all BaseTrx in this Register.

    Public Sub FireEndRegenerating()
        RaiseEvent EndRegenerating()
    End Sub

    '$Description Find BankTrx object already in register with the specified strImportKey.
    '   Used to determine if a transaction has already been imported. Will only
    '   search real and normal BaseTrx objects, because all imported BaseTrx are real and
    '   normal. The usual procedure for importing a BaseTrx is to first call this method
    '   to determine if it has already been imported, and if not then follow the
    '   normal procedure for adding a BaseTrx. See MatchNormal() for notes on the
    '   procedure for adding a BaseTrx.
    '$Param strImportKey The import key to match.
    '$Returns The index of the matching BaseTrx, or zero if there is no match.

    Public Function objMatchImportKey(ByVal strImportKey As String) As BankTrx
        For Each objTrx As BankTrx In Me.colAllTrxReverse(Of BankTrx)
            If Not objTrx.blnFake Then
                If objTrx.strImportKey = strImportKey Then
                    Return objTrx
                End If
            End If
        Next
        Return Nothing
    End Function

    '$Description Find BaseTrx object already in register matching all the arguments.
    '   Used to determine if a transaction has already been imported. Will only
    '   search real and normal BaseTrx objects, because all imported BaseTrx are real and
    '   normal. The usual procedure for importing a BaseTrx is to first call this method
    '   to determine if it has already been imported, and if not then follow the
    '   normal procedure for adding a BaseTrx. See MatchNormal() for notes on the
    '   procedure for adding a BaseTrx.
    '$Returns The index of the matching BaseTrx, or zero if there is no match.

    Public Function objMatchPaymentDetails(ByVal strNumber As String, ByVal datDate As Date, ByVal intDateRange As Short,
                                           ByVal strDescription As String, ByVal curAmount As Decimal) As BankTrx
        Dim datEarliestMatch As Date
        Dim datLatestMatch As Date
        datEarliestMatch = DateAdd(DateInterval.Day, -(intDateRange - 1), datDate)
        datLatestMatch = DateAdd(DateInterval.Day, intDateRange - 1, datDate)
        For Each objTrx As BankTrx In Me.colAllTrxReverse(Of BankTrx)
            With objTrx
                If .datDate < datEarliestMatch Then
                    Exit For
                End If
                If .datDate <= datLatestMatch Then
                    If Not .blnFake Then
                        If .strNumber = strNumber And (.curAmount = curAmount Or curAmount = 0.0#) Then
                            If Left(.strDescription, 10).ToLower() = Left(strDescription, 10).ToLower() Then
                                Return objTrx
                            End If
                        End If
                    End If
                End If
            End With
        Next
        Return Nothing
    End Function

    '$Description Update an existing BaseTrx with information from a bank import. Only for
    '   updating an existing BaseTrx. Use the normal steps for adding a new BaseTrx if not
    '   updating an existing BaseTrx, and be sure to pass the appropriate import key
    '   and say it is not fake.

    Public Sub ImportUpdateBank(ByVal objNormalTrx As BankTrx, ByVal datDate As Date, ByVal strNumber As String,
                                ByVal curAmount As Decimal, ByVal strImportKey As String)

        Dim objTrxManager As NormalTrxManager = New NormalTrxManager(objNormalTrx)
        objTrxManager.UpdateStart()
        objTrxManager.objTrx.ImportUpdateBank(datDate, strNumber, curAmount, strImportKey)
        objTrxManager.UpdateEnd(New LogChange, "ImportUpdateBank")
    End Sub

    '$Description Update an existing BaseTrx with number and amount. Only for
    '   updating an existing BaseTrx. Use the normal steps for adding a new BaseTrx if not
    '   updating an existing BaseTrx.

    Public Sub ImportUpdateNumAmt(ByVal objNormalTrx As BankTrx, ByVal strNumber As String, ByVal curAmount As Decimal)

        Dim objTrxManager As NormalTrxManager = New NormalTrxManager(objNormalTrx)
        objTrxManager.UpdateStart()
        objTrxManager.objTrx.ImportUpdateNumAmt(strNumber, curAmount)
        objTrxManager.UpdateEnd(New LogChange, "ImportUpdateNumAmt")
    End Sub

    '$Description Update an existing fake BaseTrx with new amount and make it non-generated.
    '   Intended to update a generated BaseTrx when the actual amount is known.

    Public Sub ImportUpdateAmount(ByVal objNormalTrx As BankTrx, ByVal curAmount As Decimal)

        Dim objTrxManager As NormalTrxManager = New NormalTrxManager(objNormalTrx)
        objTrxManager.UpdateStart()
        objTrxManager.objTrx.ImportUpdateAmount(curAmount)
        objTrxManager.UpdateEnd(New LogChange, "ImportUpdateAmount")
    End Sub

    '$Description Update a specified Split in existing BaseTrx with a new invoice for
    '   the same purchase order. Reduces the amount of the existing Split by the
    '   amount of the new invoice, and adds the new invoice as a new Split to the
    '   existing BaseTrx. The BaseTrx total amount does not change.

    Public Sub ImportUpdatePurchaseOrder(ByVal objNormalTrx As BankTrx, ByVal objPOSplit As TrxSplit, ByVal objImportedSplit As TrxSplit)

        Dim objTrxManager As NormalTrxManager = New NormalTrxManager(objNormalTrx)
        objTrxManager.UpdateStart()
        objTrxManager.objTrx.ImportUpdatePurchaseOrder(objPOSplit, objImportedSplit)
        objTrxManager.UpdateEnd(New LogChange, "ImportUpdatePurchaseOrder")
    End Sub

    '$Description Find normal BaseTrx objects which may be a match to arguments.
    '   A matched BaseTrx must be in the date range specified by the arguments, and
    '   one of the following must be true: (1) A non-zero lngNumber was passed,
    '   and is equal to the BaseTrx strNumber. (2) Two or more of the following comparisons
    '   indicate a match - the date matches within a few days, the first few characters
    '   of the description matches exactly (case insensitive), the amount is "close enough".
    '   "Close enough" means within curNormalMatchRange unless doing a loose match, in which
    '   case it means within curNormalMatchRange or 20 percent of imported trx amount.
    '   Will return all BaseTrx objects satisfying the above criteria, and set blnExactMatch to
    '   false, unless it finds exactly one BaseTrx that is an exact match on amount and satisfies
    '   the above criteria for date and description or trans number. In this case it will
    '   instead return a collection with only the one BaseTrx object and set blnExactMatch to true.
    '   If blnExactMatch is true, the caller can safely assume there is exactly one
    '   one element in colMatches, it is an exact match for the search criteria, and
    '   the software can act on that assumption without asking the user to verify that
    '   it really is the desired transaction.
    '   Generally called for all BaseTrx being entered, unless they are matched by
    '   lngMatchImport(), to determine if there is a fake placeholder BaseTrx for them
    '   or they've already been entered for real.
    '$Param lngNumber BaseTrx strNumber to match, if non-zero value passed.
    '$Param datDate BaseTrx datDate to match.
    '$Param intDateRange Number of days before and after datDate to check.
    '$Param strDescription BaseTrx strDescription to match.
    '$Param curAmount BaseTrx curAmount to match.
    '$Param blnLooseMatch Reduce the number of criteria required for a match
    '   from 2 to 1.
    '$Param colMatches A new Collection object created by this method, containing
    '   indices of all possible (or a single exact) matching BaseTrx objects.
    '$Param blnExactMatch True iff there is exactly one very reliable match in colMatches.

    Public Sub MatchNormal(ByVal lngNumber As Integer, ByVal datDate As Date, ByVal intDateRange As Integer,
                           ByVal strDescription As String, ByVal curAmount As Decimal, ByVal blnLooseMatch As Boolean,
                           ByRef colMatches As ICollection(Of BankTrx), ByRef blnExactMatch As Boolean)
        Dim colExactMatches As ICollection(Of BankTrx) = Nothing
        MatchNormalCore(lngNumber, datDate, intDateRange, intDateRange, strDescription, curAmount, 0.0D, 0.0D, blnLooseMatch, colMatches, colExactMatches, blnExactMatch)
        SearchUtilities.PruneToExactMatches(colExactMatches, datDate, colMatches, blnExactMatch)
    End Sub

    Public Sub MatchNormalCore(ByVal lngNumber As Integer, ByVal datDate As Date, ByVal intDaysBefore As Integer,
                         ByVal intDaysAfter As Integer, ByVal strDescription As String, ByVal curAmount As Decimal,
                         ByVal curMatchMin As Decimal, ByVal curMatchMax As Decimal,
                         ByVal blnLooseMatch As Boolean, ByRef colMatches As ICollection(Of BankTrx),
                         ByRef colExactMatches As ICollection(Of BankTrx), ByRef blnExactMatch As Boolean)

        Dim datStart As Date
        Dim datEnd As Date
        Dim strNumber As String
        Dim blnMatched As Boolean
        Dim strDescrLC As String
        Dim curAmountRange As Decimal
        Dim blnDescrMatches As Boolean
        Dim blnDateMatches As Boolean
        Dim blnAmountMatches As Boolean
        Dim intDescrMatchLen As Integer

        colMatches = New List(Of BankTrx)
        blnExactMatch = False
        colExactMatches = New List(Of BankTrx)
        intDescrMatchLen = 10
        datStart = DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDaysBefore, datDate)
        datEnd = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDaysAfter, datDate)
        strNumber = strComparableCheckNumber(CStr(lngNumber))
        strDescrLC = Left(LCase(strDescription), intDescrMatchLen)
        For Each objNormalTrx As BankTrx In Me.colDateRange(Of BankTrx)(datStart, datEnd)
            With objNormalTrx
                blnMatched = False
                If lngNumber <> 0 Then
                    If (curAmount = .curAmount Or blnLooseMatch) And strComparableCheckNumber(.strNumber) = strNumber Then
                        blnMatched = True
                    End If
                End If
                If blnLooseMatch Then
                    curAmountRange = System.Math.Abs(curAmount * 0.2D)
                    If .curNormalMatchRange > curAmountRange Then
                        curAmountRange = .curNormalMatchRange
                    End If
                Else
                    curAmountRange = .curNormalMatchRange
                End If
                blnDescrMatches = (Left(LCase(.strDescription), intDescrMatchLen) = strDescrLC)
                blnDateMatches = (System.Math.Abs(DateDiff(Microsoft.VisualBasic.DateInterval.Day, .datDate, datDate)) < 6)
                blnAmountMatches = (System.Math.Abs(.curAmount - curAmount) <= curAmountRange)
                If (CInt(IIf(blnAmountMatches, 1, 0)) + CInt(IIf(blnDateMatches, 1, 0))) >= CInt(IIf(blnLooseMatch, 1, 2)) Then
                    blnMatched = True
                End If
                If blnMatched Or blnDescrMatches Then
                    'If min/max were specified this is a mandatory amount filter, separate from blnAmountMatches.
                    If curMatchMin <> 0.0# And curMatchMax <> 0.0# Then
                        If (.curAmount >= curMatchMin) And (.curAmount <= curMatchMax) Then
                            colMatches.Add(objNormalTrx)
                        End If
                    Else
                        colMatches.Add(objNormalTrx)
                    End If
                End If
                If .curAmount = curAmount Then
                    If (blnDescrMatches And blnDateMatches) Or (strComparableCheckNumber(.strNumber) = strNumber) Then
                        colExactMatches.Add(objNormalTrx)
                    End If
                End If
            End With
        Next

    End Sub

    ''' <summary>
    ''' Return the last 4 digits of a check number.
    ''' Used to create a check number to compare to for import matching,
    ''' because some bank systems only give the last 4 digits of the check number.
    ''' </summary>
    ''' <param name="strInput"></param>
    ''' <returns></returns>
    Private Function strComparableCheckNumber(ByVal strInput As String) As String
        Dim CheckNumDigitsToUse As Integer = 3
        If Val(strInput) = 0 Then
            Return strInput
        End If
        If Len(strInput) < CheckNumDigitsToUse Then
            Return strInput.PadLeft(CheckNumDigitsToUse, "0"c)
        End If
        If Len(strInput) > CheckNumDigitsToUse Then
            Return Right(strInput, CheckNumDigitsToUse)
        End If
        Return strInput
    End Function

    '$Description Find all normal BaseTrx objects which are an exact match to the description
    '   and close to the specified date.
    '$Param datDate BaseTrx datDate to match.
    '$Param intDateRange Number of days before and after datDate to check.
    '$Param strDescription BaseTrx strDescription to match.
    '$Param blnMatchImportedFromBank True if include BaseTrx that have already been imported
    '   from the bank in the search. Non-imported trx are always included in search.
    '$Param colMatches A new Collection object created by this method, containing
    '   indices of all possible (or a single exact) matching BaseTrx objects.
    '$Param blnExactMatch True iff there is exactly one match in colMatches.

    Public Sub MatchPayee(ByVal datDate As Date, ByVal intDateRange As Short, ByVal strDescription As String, ByVal blnMatchImportedFromBank As Boolean, ByRef colMatches As ICollection(Of BankTrx), ByRef blnExactMatch As Boolean)

        Dim datStart As Date
        Dim datEnd As Date
        Dim blnImportOkay As Boolean

        colMatches = New List(Of BankTrx)
        blnExactMatch = False
        datStart = DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDateRange, datDate)
        datEnd = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDateRange, datDate)
        For Each objNormalTrx As BankTrx In Me.colDateRange(Of BankTrx)(datStart, datEnd)
            blnImportOkay = (objNormalTrx.strImportKey = "") Or (blnMatchImportedFromBank) 'Used to be (not blnMatchImportedFromBank)
            If objNormalTrx.strDescription = strDescription And blnImportOkay Then
                colMatches.Add(objNormalTrx)
            End If
        Next
        If colMatches.Count() = 1 Then
            blnExactMatch = True
        End If

    End Sub

    '$Description Find all normal BaseTrx objects which are an exact match to the payee name
    '   and invoice number, and within the specified number of days of the BaseTrx date.
    '$Param datDate BaseTrx datDate to match.
    '$Param intDateRange Number of days before and after datDate to check.
    '$Param strPayee BaseTrx strDescription (payee name) to match.
    '$Param strInvoiceNum Invoice number to match.
    '$Param colMatches A new Collection object created by this method, containing
    '   indices of all matching BaseTrx objects.

    Public Sub MatchInvoice(ByVal datDate As Date, ByVal intDateRange As Short, ByVal strPayee As String, ByVal strInvoiceNum As String, ByRef colMatches As ICollection(Of BankTrx))

        Dim datStart As Date
        Dim datEnd As Date
        Dim objSplit As TrxSplit

        colMatches = New List(Of BankTrx)
        datStart = DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDateRange, datDate)
        datEnd = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDateRange, datDate)
        For Each objNormalTrx As BankTrx In Me.colDateRange(Of BankTrx)(datStart, datEnd)
            If objNormalTrx.strDescription = strPayee Then
                For Each objSplit In objNormalTrx.colSplits
                    If objSplit.strInvoiceNum = strInvoiceNum Then
                        colMatches.Add(objNormalTrx)
                    End If
                Next objSplit
            End If
        Next

    End Sub

    '$Description Find all normal BaseTrx objects which are an exact match to the payee name
    '   and purchase order number, and within the specified number of days of the BaseTrx date.
    '$Param datDate BaseTrx datDate to match.
    '$Param intDateRange Number of days before and after datDate to check.
    '$Param strPayee BaseTrx strDescription (payee name) to match.
    '$Param strPONumber Purchase order number to match.
    '$Param colMatches A new Collection object created by this method, containing
    '   indices of all matching BaseTrx objects.

    Public Sub MatchPONumber(ByVal datDate As Date, ByVal intDateRange As Short, ByVal strPayee As String, ByVal strPONumber As String, ByRef colMatches As ICollection(Of BankTrx))

        Dim datStart As Date
        Dim datEnd As Date
        Dim objSplit As TrxSplit

        colMatches = New List(Of BankTrx)
        datStart = DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDateRange, datDate)
        datEnd = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDateRange, datDate)
        For Each objNormalTrx As BankTrx In Me.colDateRange(Of BankTrx)(datStart, datEnd)
            If objNormalTrx.strDescription = strPayee Then
                For Each objSplit In objNormalTrx.colSplits
                    If objSplit.strPONumber = strPONumber Then
                        colMatches.Add(objNormalTrx)
                    End If
                Next objSplit
            End If
        Next

    End Sub

    Public Function objFirstOnOrAfter(ByVal datDate As Date) As BaseTrx
        Dim lngIndexBeforeDate As Integer
        'lngIndex is initialized to mlngTrxUsed even if the body of the loop
        'is never executed, and will be left as zero if no matching BaseTrx is found.
        For lngIndexBeforeDate = mlngTrxUsed To 1 Step -1
            If Me.objTrx(lngIndexBeforeDate).datDate < datDate Then
                Exit For
            End If
        Next
        Dim lngIndex As Integer = lngIndexBeforeDate + 1
        If lngIndex > mlngTrxUsed Then
            Return Nothing
        End If
        Return Me.objTrx(lngIndex)
    End Function

    '$Description Find the transfer BaseTrx which is an exact match to the specified info.
    '$Param datDate The transfer BaseTrx date.
    '$Param strTransferKey The strTransferKey of the BaseTrx in THIS register to look for.
    '$Param curAmount The curAmount of the BaseTrx in THIS register to look for.
    '$Returns The index of the BaseTrx exactly matching all the parameters, or zero if
    '   there is no such match. If there are multiple matches it will pick one,
    '   but it is undefined which it will pick.

    Friend Function objMatchTransfer(ByVal datDate As Date, ByVal strTransferKey_ As String, ByVal curAmount As Decimal) As TransferTrx

        For Each objTrx As TransferTrx In Me.colAllTrxReverse(Of TransferTrx)
            If objTrx.datDate < datDate Then
                Return Nothing
            End If
            If objTrx.datDate = datDate Then
                If objTrx.curAmount = curAmount Then
                    If objTrx.strTransferKey = strTransferKey_ Then
                        Return objTrx
                    End If
                End If
            End If
        Next
        Return Nothing

    End Function

    '$Description Find a budget BaseTrx matching the specified BaseTrx date and budget key.
    '$Param datDate The datDate of the BaseTrx to find a budget BaseTrx for.
    '$Param strBudgetKey The strBudgetKey of the BaseTrx to find a budget BaseTrx for.
    '$Param blnNoMatch Set to True iff datDate>=mdatEarliestBudgetStart and no matching
    '   budget was found, otherwise set to False.
    '$Returns The index of the matching BaseTrx, or zero if there is no match. This
    '   will be the latest dated budget BaseTrx whose budget period includes datDate,
    '   and with the exact same strBudgetKey value.

    Friend Function objMatchBudget(ByVal objNormalTrx As BankTrx, ByVal strBudgetKey As String, ByRef blnNoMatch As Boolean) As BudgetTrx

        Dim datDate As DateTime = objNormalTrx.datDate
        Dim objNextTrx As BaseTrx
        Dim objBudgetTrx As BudgetTrx

        blnNoMatch = False
        objNextTrx = objNormalTrx
        Do
            If objNextTrx.objPrevious Is Nothing Then
                Exit Do
            End If
            If objNextTrx.objPrevious.datDate < datDate Then
                Exit Do
            End If
            objNextTrx = objNextTrx.objPrevious
        Loop
        Do
            objBudgetTrx = TryCast(objNextTrx, BudgetTrx)
            If Not objBudgetTrx Is Nothing Then
                If objBudgetTrx.InBudgetPeriod(datDate) Then
                    If strBudgetKey = objBudgetTrx.strBudgetKey Then
                        Return objBudgetTrx
                    End If
                End If
            End If
            objNextTrx = objNextTrx.objNext
            If objNextTrx Is Nothing Then
                Exit Do
            End If
            If objNextTrx.datDate.Subtract(datDate).TotalDays > 400 Then
                Exit Do
            End If
        Loop
        blnNoMatch = True
        Return Nothing

    End Function

    '$Description Used to report to the UI that a budget BaseTrx has changed, in
    '   case balances need to be updated.

    Friend Sub FireBudgetChanged(ByVal objBudgetTrx As BaseTrx)
        UpdateFirstAffected(objBudgetTrx.lngIndex)
        RaiseEvent BudgetChanged(objBudgetTrx)
    End Sub

    '$Description Return the BaseTrx object at the specified row of the register.
    '$Param lngIndex 1 to lngTrxCount.
    '$Returns The BaseTrx object. Properties of this object may be changed, if the
    '   caller takes the proper steps to update the register afterward.

    Public ReadOnly Property objTrx(ByVal lngIndex As Integer) As BaseTrx
        Get
            'This does not have to be implemented as a single array. If you convert all
            'the code inside this class to use objTrx() instead of maobjTrx(), this can
            'be implemented any way you want. One choice is a Collection of "month list"
            'objects, where each "month list" contains the array for that month. objTrx()
            'would have to count through the Collection to find the correct month, but
            'that's fast enough for a few dozen months. You could even use a 2 level
            'structure with years on the top and months underneath. Organizing by date
            'would also make date searches MUCH faster. The purpose in getting away from
            'maobjTrx() is to make inserts and deletions much more efficient away from
            'the end of the register.
            If lngIndex < 1 Or lngIndex > mlngTrxUsed Then
                gRaiseError("Invalid index " & lngIndex & " in Register.objTrx")
            End If
            Return maobjTrx(lngIndex)
        End Get
    End Property

    Public ReadOnly Property objFirstTrx() As BaseTrx
        Get
            If mlngTrxUsed = 0 Then
                Return Nothing
            Else
                Return maobjTrx(1)
            End If
        End Get
    End Property

    Public ReadOnly Property objLastTrx() As BaseTrx
        Get
            If mlngTrxUsed = 0 Then
                Return Nothing
            Else
                Return maobjTrx(mlngTrxUsed)
            End If
        End Get
    End Property

    Private Sub SetTrx(lngIndex As Integer, ByVal objTrx_ As BaseTrx)
        maobjTrx(lngIndex) = objTrx_
        objTrx_.lngIndex = lngIndex
    End Sub

    Public ReadOnly Property objNormalTrx(ByVal lngIndex As Integer) As BankTrx
        Get
            Return DirectCast(objTrx(lngIndex), BankTrx)
        End Get
    End Property

    Public ReadOnly Property objBudgetTrx(ByVal lngIndex As Integer) As BudgetTrx
        Get
            Return DirectCast(objTrx(lngIndex), BudgetTrx)
        End Get
    End Property

    Public ReadOnly Property objTransferTrx(ByVal lngIndex As Integer) As TransferTrx
        Get
            Return DirectCast(objTrx(lngIndex), TransferTrx)
        End Get
    End Property

    '$Description The number of transactions in the register.

    Public ReadOnly Property lngTrxCount() As Integer
        Get
            Return mlngTrxUsed
        End Get
    End Property

    '$Description The unique identifier for this register (sub-account).
    '   Used to identify transfer destinations, among other things.

    Public ReadOnly Property strRegisterKey() As String
        Get
            Return mstrRegisterKey
        End Get
    End Property

    Public ReadOnly Property objAccount() As Account
        Get
            Return mobjAccount
        End Get
    End Property

    Public ReadOnly Property strCatKey() As String
        Get
            Return objAccount.intKey.ToString() + "." + strRegisterKey
        End Get
    End Property

    Public Property strTitle() As String
        Get
            Return mstrTitle
        End Get
        Set(ByVal Value As String)
            mstrTitle = Value
            RaiseEvent MiscChange()
            mobjAccount.SetChanged()
        End Set
    End Property

    Public Property datOldestBudgetEndAllowed() As Date
        Get
            Return mdatOldestBudgetEndAllowed
        End Get
        Set(value As Date)
            mdatOldestBudgetEndAllowed = value
        End Set
    End Property

    Public Property blnShowInitially() As Boolean
        Get
            Return mblnShowInitially
        End Get
        Set(ByVal Value As Boolean)
            mblnShowInitially = Value
            RaiseEvent MiscChange()
            mobjAccount.SetChanged()
        End Set
    End Property

    Public Property blnDeleted() As Boolean
        Get
            Return mblnDeleted
        End Get
        Set(ByVal Value As Boolean)
            mblnDeleted = Value
        End Set
    End Property

    '$Description For debugging use only - return the internal mcolRepeatTrx collection.

    Public ReadOnly Property colDbgRepeatTrx() As Dictionary(Of String, BaseTrx)
        Get
            Return mcolRepeatTrx
        End Get
    End Property

    Public Sub FireShowCurrent()
        If mlngTrxUsed = 0 Or mobjTrxCurrent Is Nothing Then
            Exit Sub
        End If
        RaiseEvent ShowCurrent(mobjTrxCurrent)
    End Sub

    Public Sub SetCurrent(ByVal objNewCurrent As BaseTrx)
        mobjTrxCurrent = objNewCurrent
    End Sub

    Private Sub AddRepeatTrx(ByVal objTrx As BaseTrx)
        mcolRepeatTrx.Add(objTrx.strRepeatId, objTrx)
    End Sub

    Private Sub SetRepeatTrx(ByVal objTrx As BaseTrx)
        mcolRepeatTrx.Item(objTrx.strRepeatId) = objTrx
    End Sub

    Public Function objRepeatTrx(ByVal strRepeatKey As String, ByVal intRepeatSeq As Integer) As BaseTrx
        Dim objTrx As BaseTrx = Nothing
        If mcolRepeatTrx.TryGetValue(BaseTrx.strMakeRepeatId(strRepeatKey, intRepeatSeq), objTrx) Then
            Return objTrx
        Else
            Return Nothing
        End If
    End Function

    Friend Sub RemoveRepeatTrx(ByVal objTrx As BaseTrx)
        mcolRepeatTrx.Remove(objTrx.strRepeatId)
    End Sub

    Public ReadOnly Property objCurrentTrx() As BaseTrx
        Get
            Return mobjTrxCurrent
        End Get
    End Property

    Public Function colDateRange(Of TTrx As BaseTrx)(ByVal datStart As DateTime, ByVal datEnd As DateTime) As IEnumerable(Of TTrx)
        Return New RegDateRange(Of TTrx)(Me, datStart, datEnd)
    End Function

    Public Function colAllTrx(Of TTrx As BaseTrx)() As IEnumerable(Of TTrx)
        Return New RegIterator(Of TTrx)(Me)
    End Function

    Public Function colAllTrxReverse(Of TTrx As BaseTrx)() As IEnumerable(Of TTrx)
        Return New RegReverse(Of TTrx)(Me)
    End Function

    Public Function curEndingBalance(ByVal datEndDate As DateTime) As Decimal
        Dim curBalance As Decimal
        For Each objTrx As BaseTrx In colAllTrx(Of BaseTrx)()
            If objTrx.datDate > datEndDate Then
                Exit For
            End If
            curBalance = curBalance + objTrx.curAmount
        Next
        Return curBalance
    End Function

    '$Description Validate the register, and report all errors by firing
    '   ValidationError events. Checks balances, sort order, and consistency
    '   of data in individual BaseTrx.

    Public Sub ValidateRegister()
        Dim objTrx As BaseTrx
        Dim objTrx2 As BaseTrx
        Dim objPriorTrx As BaseTrx
        Dim curPriorBalance As Decimal
        Dim intRepeatTrxCount As Short

        curPriorBalance = 0
        objPriorTrx = Nothing

        For Each objTrx In Me.colAllTrx(Of BaseTrx)()
            With objTrx
                If .curBalance <> (curPriorBalance + .curAmount) Then
                    FireValidationError(objTrx, "Incorrect balance")
                End If
                curPriorBalance = .curBalance
                If Not objPriorTrx Is Nothing Then
                    If Register.intSortComparison(objTrx, objPriorTrx) < 0 Then
                        FireValidationError(objTrx, "Not in correct sort order")
                    End If
                End If
                objPriorTrx = objTrx
                .Validate()
                If .intRepeatSeq > 0 Then
                    intRepeatTrxCount = intRepeatTrxCount + 1S
                End If
            End With
        Next

        If intRepeatTrxCount <> mcolRepeatTrx.Count() Then
            FireValidationError(Nothing, "Wrong number of repeat trx")
        End If

        For Each objTrx In mcolRepeatTrx.Values
            objTrx2 = objRepeatTrx(objTrx.strRepeatKey, objTrx.intRepeatSeq)
            If Not objTrx Is objTrx2 Then
                FireValidationError(Nothing, "Repeat collection element points to wrong trx")
            End If
        Next objTrx

    End Sub

    Friend Sub FireValidationError(ByVal objTrx As BaseTrx, ByVal strMsg As String)
        RaiseEvent ValidationError(objTrx, strMsg)
    End Sub

    '$Description Registry key name specific to a register.

    Public Function strRegistryKey() As String
        Return "Registers\" & strTitle
    End Function

    Public Sub LogAction(ByVal strTitle As String)
        mobjLog.AddILogAction(New LogAction, strTitle)
    End Sub

    Public Sub LogSave()
        mobjLog.AddILogAction(New LogSave, "Register.Save")
    End Sub

    Public Function objLogGroupStart(ByVal strTitle As String) As ILogGroupStart
        Dim objStartLogger As ILogGroupStart
        objStartLogger = New LogGroupStart
        mobjLog.AddILogGroupStart(objStartLogger, strTitle)
        objLogGroupStart = objStartLogger
    End Function

    Public Sub LogGroupEnd(ByVal objStartLogger As ILogGroupStart)
        mobjLog.AddILogGroupEnd(New LogGroupEnd, objStartLogger)
    End Sub

    Public Sub WriteEventLog(ByVal strAccountTitle As String, ByVal objRepeats As IStringTranslator)
        mobjLog.WriteAll(strAccountTitle, objRepeats)
    End Sub

    Public Overrides Function ToString() As String
        Return Me.strTitle
    End Function
End Class