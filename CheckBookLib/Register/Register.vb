Option Strict On
Option Explicit On

''' <summary>
''' A transaction register, which is a list of Trx objects from one Account and
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
    'Array of Trx objects in register.
    Private maobjTrx() As Trx
    'Index of first Trx affected by NewAddEnd() or UpdateEnd().
    Private mlngFirstAffected As Integer
    'Special value for mlngFirstAffected indicating no Trx affected.
    Private Const mlngNO_TRX_AFFECTED As Integer = 9999999
    'Register title.
    Private mstrTitle As String
    'Unique identifier for register.
    Private mstrRegisterKey As String
    'Number of maobjTrx() array elements allocated at a time.
    Private mlngAllocationUnit As Integer
    'Show this register initially in the UI.
    Private mblnShowInitially As Boolean
    'Index of current Trx. Used only by the UI.
    Private mlngTrxCurrent As Integer
    'Budget Trx with ending date older than this are always zero amount.
    Private mdatOldestBudgetEndAllowed As Date
    'Collection of all Trx that are part of a repeating sequence,
    'keyed by "#" & strRepeatKey & "." & intRepeatSeq.
    Private mcolRepeatTrx As Dictionary(Of String, Trx)
    'EventLog for Register.
    Private mobjLog As EventLog
    'True iff operator deleted register.
    Private mblnDeleted As Boolean

    'Fired by NewAddEnd() when it adds a Trx to the register.
    'Intended to allow the UI to update itself.
    Public Event TrxAdded(ByVal lngIndex As Integer, ByVal objTrx As Trx)

    'Fired by UpdateEnd() when it updates a Trx in the register.
    'Intended to allow the UI to update itself.
    Public Event TrxUpdated(ByVal lngOldIndex As Integer, ByVal lngNewIndex As Integer, ByVal objTrx As Trx)

    'Fired by Delete().
    'Intended to allow the UI to update itself.
    Public Event TrxDeleted(ByVal lngIndex As Integer)

    'Fired when a Split is applied or un-applied to a budget Trx.
    'Intended to allow the UI to update itself.
    Public Event BudgetChanged(ByVal lngIndex As Integer, ByVal objBudget As Trx)

    'Fired after register balances have been updated.
    'Intended to allow the UI to update itself.
    Public Event BalancesChanged(ByVal lngFirstIndex As Integer, ByVal lngLastIndex As Integer)

    'Fired by SetTrxStatus() method.
    'Intended to allow the UI to update itself.
    Public Event StatusChanged(ByVal lngIndex As Integer)

    'Fired by ShowCurrent() method.
    'Intended to allow the UI to update itself.
    Public Event ShowCurrent(ByVal lngIndex As Integer)

    'Fired to say that the Register has been changed but no Trx were changed,
    'e.g. the register title or some other property changed.
    Public Event MiscChange()

    'Fired before operations which cause large numbers of Trx to be changed.
    'Always followed by RedisplayTrx event when Trx modifications are done.
    'Allows clients to ignore Trx changes while they are happening, and simply
    'redisplay everything at once when RedisplayTrx is fired.
    Public Event HideTrx()

    'Fired when all Trx in the Register must be redisplayed.
    Public Event RedisplayTrx()

    'Fired by Validate() or a method called from Validate() when a validation
    'error is detected.
    Public Event ValidationError(ByVal lngIndex As Integer, ByVal strMsg As String)

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
        mcolRepeatTrx = New Dictionary(Of String, Trx)
        Erase maobjTrx
        mlngTrxAllocated = 0
        mlngTrxUsed = 0
        mlngTrxCurrent = 0
        ClearFirstAffected()
        mobjLog = New EventLog
        mobjLog.Init(Me, mobjAccount.objCompany.objSecurity.strLogin)

    End Sub

    '$Description Add a Trx object to the register at the correct place in the sort order,
    '   as part of a register loading operation.
    '   Does not search for any kind of match to this Trx, apply to budgets, or adjust
    '   curBalance properties. Optimized for the case where objNew falls near or at the
    '   end of the sort order.
    '$Param objNew Trx object to add. This actual object instance will
    '   become the register entry, rather than making a copy of it.

    Public Sub NewLoadEnd(ByVal objNew As Trx)
        If objNew Is Nothing Then
            gRaiseError("objNew is Nothing in Register.NewLoadEnd")
        End If
        lngNewInsert(objNew)
        If objNew.intRepeatSeq > 0 Then
            AddRepeatTrx(objNew)
        End If
    End Sub

    '$Description Add a Trx object to the register at the correct place in the sort order,
    '   as part of adding a Trx to an already loaded register. Optimized for the case
    '   where objNew falls near or at the end of the sort order.
    '$Param objNew Trx object to add. This actual object instance will
    '   become the register entry, rather than making a copy of it.

    Public Sub NewAddEnd(ByVal objNew As Trx, ByVal objAddLogger As ILogAdd, ByVal strTitle As String,
                         Optional ByVal blnSetChanged As Boolean = True)

        If objNew Is Nothing Then
            gRaiseError("objNew is Nothing in Register.NewAddEnd")
        End If
        mlngTrxCurrent = lngNewInsert(objNew)
        objNew.Apply(False)
        RaiseEvent TrxAdded(mlngTrxCurrent, objNew)
        If blnSetChanged Then
            mobjAccount.SetChanged()
        End If
        UpdateFirstAffected(mlngTrxCurrent)
        If objNew.intRepeatSeq > 0 Then
            SetRepeatTrx(objNew)
        End If
        FixBalancesAndRefreshUI()
        mobjLog.AddILogAdd(objAddLogger, strTitle, objNew)

    End Sub

    '$Description Insert a Trx in the correct place in the sort order.
    '   Optimized for the case where objNew falls near or at the end of the sort order.
    '$Param objNew Trx object to add. This actual object instance will
    '   become the register entry, rather than making a copy of it.
    '$Returns The index of the new transaction in the register.

    Private Function lngNewInsert(ByVal objNew As Trx) As Integer
        Dim lngIndex As Integer
        If mlngTrxUsed = mlngTrxAllocated Then
            mlngTrxAllocated = mlngTrxAllocated + mlngAllocationUnit
            ReDim Preserve maobjTrx(mlngTrxAllocated)
        End If
        mlngTrxUsed = mlngTrxUsed + 1
        lngIndex = lngMoveUp(mlngTrxUsed - 1, objNew)
        lngNewInsert = lngIndex
    End Function

    '$Description Move a Trx the shortest distance possible toward the beginning of the
    '   register which will leave it in the correct place in the sort order. Will not
    '   change the register if the Trx is already in the correct place.
    '$Param lngEndIndex The index immediately above the current location of the Trx
    '   being moved. maobjTrx(lngEndIndex+1) does not actually have to refer to objTrx,
    '   but this method will act as if it is in the sense that it will make room for
    '   it at the new location by moving everything between the new location and
    '   lngEndIndex down one row in the register, thus overwriting the object currently
    '   in that position.
    '$Param objTrx The Trx object to move. Will insert this in maobjTrx().
    '$Returns The index at which objTrx was inserted, or its original locaction if it
    '   was already in the correct place.

    Private Function lngMoveUp(ByVal lngEndIndex As Integer, ByVal objTrx As Trx) As Integer
        Dim lngFirstLesser As Integer
        Dim lngMoveIndex As Integer
        If lngEndIndex < 0 Or lngEndIndex > (mlngTrxUsed - 1) Then
            gRaiseError("Register.lngMoveUp passed invalid lngEndIndex=" & lngEndIndex)
        End If
        'When the loop is done, lngFirstLesser will equal the largest index whose Trx
        'sort key is less than objNew.strSortKey, or zero if there is none.
        For lngFirstLesser = lngEndIndex To 1 Step -1
            If Register.intSortComparison(objTrx, maobjTrx(lngFirstLesser)) >= 0 Then
                Exit For
            End If
        Next
        lngMoveUp = lngFirstLesser + 1
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
    End Function

    '$Description Like lngMoveUp(), but moves a Trx the shortest distance possible
    '   toward the end of the register.
    '$Param lngStartIndex The index immediately below the current location of the Trx
    '   being moved. See lngEndIndex argument to lngMoveUp().
    '$Param objTrx See lngMoveUp().
    '$Returns See lngMoveUp().

    Private Function lngMoveDown(ByVal lngStartIndex As Integer, ByVal objTrx As Trx) As Integer
        Dim lngFirstGreater As Integer
        Dim lngMoveIndex As Integer
        If lngStartIndex < 2 Or lngStartIndex > mlngTrxUsed Then
            gRaiseError("Register.lngMoveDown passed invalid lngStartIndex " & lngStartIndex)
        End If
        'When the loop is done, lngFirstGreater will equal the smallest index whose Trx
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
        lngMoveDown = lngFirstGreater - 1
    End Function

    '$Description Finish updating an existing transaction. Moves it to the correct
    '   position in the sort order.
    '   Is optimized for the case where position in the sort order is not changed much.
    '$Param lngIndex The index of the Trx object in the register to move.

    Friend Sub UpdateEnd(ByVal objTrx As Trx, ByVal objChangeLogger As ILogChange, ByVal strTitle As String, ByVal objOldTrx As Trx)

        Dim lngOldIndex As Integer = objTrx.lngIndex

        Try

            If objTrx.intRepeatSeq > 0 Then
                SetRepeatTrx(objTrx)
            End If
            objTrx.Apply(False)
            mlngTrxCurrent = lngUpdateMove(lngOldIndex)
            RaiseEvent TrxUpdated(lngOldIndex, mlngTrxCurrent, objTrx)
            mobjAccount.SetChanged()
            UpdateFirstAffected(lngOldIndex)
            UpdateFirstAffected(mlngTrxCurrent)
            FixBalancesAndRefreshUI()
            mobjLog.AddILogChange(objChangeLogger, strTitle, objTrx, objOldTrx)

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Move a Trx to the correct position based on sort key.
    '$Param lngOldIndex The index of the Trx in the register to move.
    '$Returns The new index position of the Trx after being moved.
    '   This position is where the Trx lives in the register AFTER it has
    '   been deleted from its old position. For example, if you tell it to
    '   move the Trx at index 3, and the new location is after the Trx in
    '   position 7 prior to the move, then the new location will be 7 and
    '   not 8 because the Trx previously at position 7 was moved up to make
    '   room for it. In other words you can always update a UI by first
    '   deleting a row from the old position and then inserting a row at
    '   the new position.

    Private Function lngUpdateMove(ByVal lngOldIndex As Integer) As Integer
        Dim objTrx As Trx

        objTrx = Me.objTrx(lngOldIndex)

        If lngOldIndex > 1 Then
            If Register.intSortComparison(objTrx, Me.objTrx(lngOldIndex - 1)) < 0 Then
                lngUpdateMove = lngMoveUp(lngOldIndex - 1, objTrx)
                Exit Function
            End If
        End If

        If lngOldIndex < mlngTrxUsed Then
            If Register.intSortComparison(objTrx, Me.objTrx(lngOldIndex + 1)) > 0 Then
                lngUpdateMove = lngMoveDown(lngOldIndex + 1, objTrx)
                Exit Function
            End If
        End If

        lngUpdateMove = lngOldIndex

    End Function

    Public Shared Function intSortComparison(ByVal objTrx1 As Trx, ByVal objTrx2 As Trx) As Integer
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

    Friend Sub ClearFirstAffected()
        mlngFirstAffected = mlngNO_TRX_AFFECTED
    End Sub

    Private Sub UpdateFirstAffected(ByVal lngIndex As Integer)
        If lngIndex < mlngFirstAffected And lngIndex <= mlngTrxUsed Then
            mlngFirstAffected = lngIndex
        End If
    End Sub

    '$Description Compute curBalance for all Trx for which it may need to be
    '   changed, then if any were changed inform the UI what needs to be updated.
    '   Called after all operations which may cause balances to change in an already
    '   loaded register.

    Private Sub FixBalancesAndRefreshUI()
        Dim lngLastAffectedIndex As Integer
        'This condition might actually not be true, for example if
        'the last Trx in the register is deleted and it was not
        'applied to any budgets.
        If mlngFirstAffected <> mlngNO_TRX_AFFECTED Then
            lngLastAffectedIndex = lngFixBalances(mlngFirstAffected)
            RaiseEvent BalancesChanged(mlngFirstAffected, lngLastAffectedIndex)
        End If
        RaiseShowCurrent()
    End Sub

    '$Description Set the status of a transaction, and fire a StatusChanged event for it.
    '$Param lngIndex The index of the Trx to change.
    '$Param lngStatus The new status.

    Public Sub SetTrxStatus(ByVal lngIndex As Integer, ByVal lngStatus As Trx.TrxStatus, ByVal objAddLogger As ILogAdd, ByVal strTitle As String)

        Dim objStatusTrx As Trx
        objStatusTrx = Me.objTrx(lngIndex)
        objStatusTrx.lngStatus = lngStatus
        RaiseEvent StatusChanged(lngIndex)
        mobjAccount.SetChanged()
        'Use an ILogAdd instead of a specialized logger because it's a cheap
        'hack to reuse an existing type with the correct signature rather than
        'define an entire new one solely for this purpose.
        mobjLog.AddILogAdd(objAddLogger, strTitle, objStatusTrx)

    End Sub

    '$Description Delete a transaction from the register.
    '$Param lngIndex The index of the Trx to delete.

    Public Sub Delete(ByVal lngIndex As Integer, ByVal objDeleteLogger As ILogDelete, ByVal strTitle As String,
                      Optional ByVal blnSetChanged As Boolean = True)
        Dim lngMoveIndex As Integer
        Dim objTrxOld As Trx
        Dim objTrx As Trx
        If lngIndex < 1 Or lngIndex > mlngTrxUsed Then
            gRaiseError("Register.Delete passed invalid index=" & lngIndex)
        End If
        ClearFirstAffected()
        objTrx = Me.objTrx(lngIndex)
        With objTrx
            objTrxOld = .objClone(False)
            objTrx.UnApply()
            If .strRepeatKey <> "" Then
                RemoveRepeatTrx(maobjTrx(lngIndex))
            End If
        End With
        For lngMoveIndex = lngIndex + 1 To mlngTrxUsed
            SetTrx(lngMoveIndex - 1, maobjTrx(lngMoveIndex))
        Next
        maobjTrx(mlngTrxUsed) = Nothing
        mlngTrxUsed = mlngTrxUsed - 1
        If mlngFirstAffected > mlngTrxUsed Then
            'Will happen when deleting the last Trx in the register,
            'because will call UnApply() on that Trx.
            mlngFirstAffected = mlngNO_TRX_AFFECTED
        End If
        RaiseEvent TrxDeleted(lngIndex)
        If blnSetChanged Then
            mobjAccount.SetChanged()
        End If
        mlngTrxCurrent = lngIndex
        'Condition will be false if last trx deleted.
        If lngIndex <= mlngTrxUsed Then
            UpdateFirstAffected(lngIndex)
        End If
        If mlngTrxCurrent > mlngTrxUsed Then
            mlngTrxCurrent = mlngTrxUsed
        End If
        'Still have to "fix balances" even if deleting the last Trx,
        'because that Trx might be applied to budgets.
        FixBalancesAndRefreshUI()
        mobjLog.AddILogDelete(objDeleteLogger, strTitle, objTrxOld)
    End Sub

    '$Description Compute curBalance properties of all Trx objects starting at a
    '   particular index value. Does not stop if it finds a Trx whose curBalance
    '   is the same after being recomputed, for example because a Trx was moved in
    '   the register but its curAmount property did not change. It keeps going because
    '   updating a Trx can cause the curAmount property of the budget Trx it is
    '   applied to to change in many ways, and it is possible the register may be
    '   back "in synch" for a few Trx and then out of synch.
    '$Param lngStartIndex The index of the first Trx whose curBalance property
    '   needs to be computed.
    '$Returns The index of the last Trx whose curBalance property actually changed.
    '   The caller can use this to decide what part of register UI needs to be
    '   refreshed to show new running balance.

    Private Function lngFixBalances(ByVal lngStartIndex As Integer) As Integer
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
        lngFixBalances = lngLastChange
    End Function

    '$Description Call Trx.Apply() on all Trx in this Register after this Register is
    '   loaded with Trx objects from an external database. This may create additional Trx.
    '   Must be done for all Trx in all Registers in all Accounts before calling
    '   LoadFinish() for any Registers in any Accounts.

    Public Sub LoadApply()

        For lngIndex As Integer = 1 To mlngTrxUsed
            Me.objTrx(lngIndex).Apply(True)
        Next

    End Sub

    '$Description Perform final post processing steps after this Register is
    '   loaded and LoadApply() is called. Computes balances.
    '   The register is ready to display in a UI after this.

    Public Sub LoadFinish()
        Dim curBalance As Decimal = 0
        mdatOldestBudgetEndAllowed = mobjAccount.datLastReconciled.AddDays(1D)
        For Each objTrx As Trx In colAllTrx()
            If TypeOf (objTrx) Is BudgetTrx Then
                DirectCast(objTrx, BudgetTrx).SetAmountForBudget()
            End If
            curBalance = curBalance + objTrx.curBalanceChange
            objTrx.curBalance = curBalance
        Next
    End Sub

    '$Description Remove all generated Trx from the Register, and clear
    '   all budget allocations.

    Public Sub PurgeGenerated()
        Dim lngInIndex As Integer
        Dim lngOutIndex As Integer
        Dim objTrx As Trx
        'Dim objSplit As TrxSplit

        If mlngTrxUsed = 0 Then
            Exit Sub
        End If
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
        mlngTrxCurrent = 0
    End Sub

    '$Description Called to tell everyone who cares that many Trx are about
    '   to change, and clients may not want to update their UI until all
    '   the changes are done which is signaled by the RedisplayTrx event.

    Public Sub FireHideTrx()
        RaiseEvent HideTrx()
    End Sub

    '$Description Called to tell everyone who cares that they must redisplay
    '   all Trx in this Register.

    Public Sub FireRedisplayTrx()
        RaiseEvent RedisplayTrx()
    End Sub

    '$Description Find NormalTrx object already in register with the specified strImportKey.
    '   Used to determine if a transaction has already been imported. Will only
    '   search real and normal Trx objects, because all imported Trx are real and
    '   normal. The usual procedure for importing a Trx is to first call this method
    '   to determine if it has already been imported, and if not then follow the
    '   normal procedure for adding a Trx. See MatchNormal() for notes on the
    '   procedure for adding a Trx.
    '$Param strImportKey The import key to match.
    '$Returns The index of the matching Trx, or zero if there is no match.

    Public Function objMatchImportKey(ByVal strImportKey As String) As NormalTrx
        Dim lngIndex As Integer
        Dim objTrx As Trx
        For lngIndex = mlngTrxUsed To 1 Step -1
            objTrx = Me.objTrx(lngIndex)
            If objTrx.GetType() Is GetType(NormalTrx) And Not objTrx.blnFake Then
                If DirectCast(objTrx, NormalTrx).strImportKey = strImportKey Then
                    Return DirectCast(objTrx, NormalTrx)
                End If
            End If
        Next
        Return Nothing
    End Function

    '$Description Find Trx object already in register matching all the arguments.
    '   Used to determine if a transaction has already been imported. Will only
    '   search real and normal Trx objects, because all imported Trx are real and
    '   normal. The usual procedure for importing a Trx is to first call this method
    '   to determine if it has already been imported, and if not then follow the
    '   normal procedure for adding a Trx. See MatchNormal() for notes on the
    '   procedure for adding a Trx.
    '$Returns The index of the matching Trx, or zero if there is no match.

    Public Function objMatchPaymentDetails(ByVal strNumber As String, ByVal datDate As Date, ByVal intDateRange As Short,
                                           ByVal strDescription As String, ByVal curAmount As Decimal) As NormalTrx
        Dim lngIndex As Integer
        Dim datEarliestMatch As Date
        Dim datLatestMatch As Date
        datEarliestMatch = DateAdd(DateInterval.Day, -(intDateRange - 1), datDate)
        datLatestMatch = DateAdd(DateInterval.Day, intDateRange - 1, datDate)
        For lngIndex = mlngTrxUsed To 1 Step -1
            With Me.objTrx(lngIndex)
                If .datDate < datEarliestMatch Then
                    Exit For
                End If
                If .datDate <= datLatestMatch Then
                    If .GetType() Is GetType(NormalTrx) And Not .blnFake Then
                        If .strNumber = strNumber And (.curAmount = curAmount Or curAmount = 0.0#) Then
                            If Left(.strDescription, 10).ToLower() = Left(strDescription, 10).ToLower() Then
                                Return DirectCast(Me.objTrx(lngIndex), NormalTrx)
                            End If
                        End If
                    End If
                End If
            End With
        Next
        Return Nothing
    End Function

    '$Description Update an existing Trx with information from a bank import. Only for
    '   updating an existing Trx. Use the normal steps for adding a new Trx if not
    '   updating an existing Trx, and be sure to pass the appropriate import key
    '   and say it is not fake.

    Public Sub ImportUpdateBank(ByVal lngOldIndex As Integer, ByVal datDate As Date, ByVal strNumber As String,
                                ByVal curAmount As Decimal, ByVal strImportKey As String)

        Dim objTrxManager As NormalTrxManager = Me.objGetNormalTrxManager(lngOldIndex)
        objTrxManager.UpdateStart()
        objTrxManager.objTrx.ImportUpdateBank(datDate, strNumber, curAmount, strImportKey)
        objTrxManager.UpdateEnd(New LogChange, "ImportUpdateBank")
    End Sub

    '$Description Update an existing Trx with number and amount. Only for
    '   updating an existing Trx. Use the normal steps for adding a new Trx if not
    '   updating an existing Trx.

    Public Sub ImportUpdateNumAmt(ByVal lngOldIndex As Integer, ByVal strNumber As String, ByVal curAmount As Decimal)

        Dim objTrxManager As NormalTrxManager = Me.objGetNormalTrxManager(lngOldIndex)
        objTrxManager.UpdateStart()
        objTrxManager.objTrx.ImportUpdateNumAmt(strNumber, curAmount)
        objTrxManager.UpdateEnd(New LogChange, "ImportUpdateNumAmt")
    End Sub

    '$Description Update an existing fake Trx with new amount and make it non-generated.
    '   Intended to update a generated Trx when the actual amount is known.

    Public Sub ImportUpdateAmount(ByVal lngOldIndex As Integer, ByVal curAmount As Decimal)

        Dim objTrxManager As NormalTrxManager = Me.objGetNormalTrxManager(lngOldIndex)
        objTrxManager.UpdateStart()
        objTrxManager.objTrx.ImportUpdateAmount(curAmount)
        objTrxManager.UpdateEnd(New LogChange, "ImportUpdateAmount")
    End Sub

    '$Description Update a specified Split in existing Trx with a new invoice for
    '   the same purchase order. Reduces the amount of the existing Split by the
    '   amount of the new invoice, and adds the new invoice as a new Split to the
    '   existing Trx. The Trx total amount does not change.

    Public Sub ImportUpdatePurchaseOrder(ByVal lngOldIndex As Integer, ByVal objPOSplit As TrxSplit, ByVal objImportedSplit As TrxSplit)

        Dim objTrxManager As NormalTrxManager = Me.objGetNormalTrxManager(lngOldIndex)
        objTrxManager.UpdateStart()
        objTrxManager.objTrx.ImportUpdatePurchaseOrder(objPOSplit, objImportedSplit)
        objTrxManager.UpdateEnd(New LogChange, "ImportUpdatePurchaseOrder")
    End Sub

    '$Description Find normal Trx objects which may be a match to arguments.
    '   A matched Trx must be in the date range specified by the arguments, and
    '   one of the following must be true: (1) A non-zero lngNumber was passed,
    '   and is equal to the Trx strNumber. (2) Two or more of the following comparisons
    '   indicate a match - the date matches within a few days, the first few characters
    '   of the description matches exactly (case insensitive), the amount is "close enough".
    '   "Close enough" means within curNormalMatchRange unless doing a loose match, in which
    '   case it means within curNormalMatchRange or 20 percent of imported trx amount.
    '   Will return all Trx objects satisfying the above criteria, and set blnExactMatch to
    '   false, unless it finds exactly one Trx that is an exact match on amount and satisfies
    '   the above criteria for date and description or trans number. In this case it will
    '   instead return a collection with only the one Trx object and set blnExactMatch to true.
    '   If blnExactMatch is true, the caller can safely assume there is exactly one
    '   one element in colMatches, it is an exact match for the search criteria, and
    '   the software can act on that assumption without asking the user to verify that
    '   it really is the desired transaction.
    '   Generally called for all Trx being entered, unless they are matched by
    '   lngMatchImport(), to determine if there is a fake placeholder Trx for them
    '   or they've already been entered for real.
    '$Param lngNumber Trx strNumber to match, if non-zero value passed.
    '$Param datDate Trx datDate to match.
    '$Param intDateRange Number of days before and after datDate to check.
    '$Param strDescription Trx strDescription to match.
    '$Param curAmount Trx curAmount to match.
    '$Param blnLooseMatch Reduce the number of criteria required for a match
    '   from 2 to 1.
    '$Param colMatches A new Collection object created by this method, containing
    '   indices of all possible (or a single exact) matching Trx objects.
    '$Param blnExactMatch True iff there is exactly one very reliable match in colMatches.

    Public Sub MatchNormal(ByVal lngNumber As Integer, ByVal datDate As Date, ByVal intDateRange As Integer,
                           ByVal strDescription As String, ByVal curAmount As Decimal, ByVal blnLooseMatch As Boolean,
                           ByRef colMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean)
        Dim colExactMatches As ICollection(Of NormalTrx) = Nothing
        MatchNormalCore(lngNumber, datDate, intDateRange, intDateRange, strDescription, curAmount, 0.0D, 0.0D, blnLooseMatch, colMatches, colExactMatches, blnExactMatch)
        SearchUtilities.PruneToExactMatches(colExactMatches, datDate, colMatches, blnExactMatch)
    End Sub

    Public Sub MatchNormalCore(ByVal lngNumber As Integer, ByVal datDate As Date, ByVal intDaysBefore As Integer,
                         ByVal intDaysAfter As Integer, ByVal strDescription As String, ByVal curAmount As Decimal,
                         ByVal curMatchMin As Decimal, ByVal curMatchMax As Decimal,
                         ByVal blnLooseMatch As Boolean, ByRef colMatches As ICollection(Of NormalTrx),
                         ByRef colExactMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean)

        Dim lngIndex As Integer
        Dim datEnd As Date
        Dim strNumber As String
        Dim blnMatched As Boolean
        Dim strDescrLC As String
        Dim curAmountRange As Decimal
        Dim blnDescrMatches As Boolean
        Dim blnDateMatches As Boolean
        Dim blnAmountMatches As Boolean
        Dim intDescrMatchLen As Integer
        Dim objTrx As Trx
        Dim objNormalTrx As NormalTrx

        colMatches = New List(Of NormalTrx)
        blnExactMatch = False
        colExactMatches = New List(Of NormalTrx)
        intDescrMatchLen = 10
        lngIndex = lngFindBeforeDate(DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDaysBefore, datDate)) + 1
        datEnd = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDaysAfter, datDate)
        strNumber = strComparableCheckNumber(CStr(lngNumber))
        strDescrLC = Left(LCase(strDescription), intDescrMatchLen)
        Do
            If lngIndex > mlngTrxUsed Then
                Exit Do
            End If
            objTrx = Me.objTrx(lngIndex)
            With objTrx
                If .datDate > datEnd Then
                    Exit Do
                End If
                If .GetType() Is GetType(NormalTrx) Then
                    objNormalTrx = DirectCast(objTrx, NormalTrx)
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
                End If
            End With
            lngIndex = lngIndex + 1
        Loop

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

    '$Description Find all normal Trx objects which are an exact match to the description
    '   and close to the specified date.
    '$Param datDate Trx datDate to match.
    '$Param intDateRange Number of days before and after datDate to check.
    '$Param strDescription Trx strDescription to match.
    '$Param blnMatchImportedFromBank True if include Trx that have already been imported
    '   from the bank in the search. Non-imported trx are always included in search.
    '$Param colMatches A new Collection object created by this method, containing
    '   indices of all possible (or a single exact) matching Trx objects.
    '$Param blnExactMatch True iff there is exactly one match in colMatches.

    Public Sub MatchPayee(ByVal datDate As Date, ByVal intDateRange As Short, ByVal strDescription As String, ByVal blnMatchImportedFromBank As Boolean, ByRef colMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean)

        Dim lngIndex As Integer
        Dim datEnd As Date
        Dim blnImportOkay As Boolean
        Dim objTrx As Trx
        Dim objNormalTrx As NormalTrx

        colMatches = New List(Of NormalTrx)
        blnExactMatch = False
        lngIndex = lngFindBeforeDate(DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDateRange, datDate)) + 1
        datEnd = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDateRange, datDate)
        Do
            If lngIndex > mlngTrxUsed Then
                Exit Do
            End If
            objTrx = Me.objTrx(lngIndex)
            If objTrx.datDate > datEnd Then
                Exit Do
            End If
            If objTrx.GetType() Is GetType(NormalTrx) Then
                objNormalTrx = DirectCast(objTrx, NormalTrx)
                blnImportOkay = (objNormalTrx.strImportKey = "") Or (blnMatchImportedFromBank) 'Used to be (not blnMatchImportedFromBank)
                If objNormalTrx.strDescription = strDescription And blnImportOkay Then
                    colMatches.Add(objNormalTrx)
                End If
            End If
            lngIndex = lngIndex + 1
        Loop
        If colMatches.Count() = 1 Then
            blnExactMatch = True
        End If

    End Sub

    '$Description Find all normal Trx objects which are an exact match to the payee name
    '   and invoice number, and within the specified number of days of the Trx date.
    '$Param datDate Trx datDate to match.
    '$Param intDateRange Number of days before and after datDate to check.
    '$Param strPayee Trx strDescription (payee name) to match.
    '$Param strInvoiceNum Invoice number to match.
    '$Param colMatches A new Collection object created by this method, containing
    '   indices of all matching Trx objects.

    Public Sub MatchInvoice(ByVal datDate As Date, ByVal intDateRange As Short, ByVal strPayee As String, ByVal strInvoiceNum As String, ByRef colMatches As ICollection(Of NormalTrx))

        Dim lngIndex As Integer
        Dim datEnd As Date
        Dim objSplit As TrxSplit
        Dim objTrx As Trx

        colMatches = New List(Of NormalTrx)
        lngIndex = lngFindBeforeDate(DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDateRange, datDate)) + 1
        datEnd = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDateRange, datDate)
        Do
            If lngIndex > mlngTrxUsed Then
                Exit Do
            End If
            objTrx = Me.objTrx(lngIndex)
            With objTrx
                If .datDate > datEnd Then
                    Exit Do
                End If
                If .GetType() Is GetType(NormalTrx) Then
                    If .strDescription = strPayee Then
                        For Each objSplit In DirectCast(objTrx, NormalTrx).colSplits
                            If objSplit.strInvoiceNum = strInvoiceNum Then
                                colMatches.Add(DirectCast(objTrx, NormalTrx))
                            End If
                        Next objSplit
                    End If
                End If
            End With
            lngIndex = lngIndex + 1
        Loop

    End Sub

    '$Description Find all normal Trx objects which are an exact match to the payee name
    '   and purchase order number, and within the specified number of days of the Trx date.
    '$Param datDate Trx datDate to match.
    '$Param intDateRange Number of days before and after datDate to check.
    '$Param strPayee Trx strDescription (payee name) to match.
    '$Param strPONumber Purchase order number to match.
    '$Param colMatches A new Collection object created by this method, containing
    '   indices of all matching Trx objects.

    Public Sub MatchPONumber(ByVal datDate As Date, ByVal intDateRange As Short, ByVal strPayee As String, ByVal strPONumber As String, ByRef colMatches As ICollection(Of NormalTrx))

        Dim lngIndex As Integer
        Dim datEnd As Date
        Dim objSplit As TrxSplit
        Dim objTrx As Trx

        colMatches = New List(Of NormalTrx)
        lngIndex = lngFindBeforeDate(DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDateRange, datDate)) + 1
        datEnd = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDateRange, datDate)
        Do
            If lngIndex > mlngTrxUsed Then
                Exit Do
            End If
            objTrx = Me.objTrx(lngIndex)
            With objTrx
                If .datDate > datEnd Then
                    Exit Do
                End If
                If .GetType() Is GetType(NormalTrx) Then
                    If .strDescription = strPayee Then
                        For Each objSplit In DirectCast(objTrx, NormalTrx).colSplits
                            If objSplit.strPONumber = strPONumber Then
                                colMatches.Add(DirectCast(objTrx, NormalTrx))
                            End If
                        Next objSplit
                    End If
                End If
            End With
            lngIndex = lngIndex + 1
        Loop

    End Sub

    '$Description Return the largest index whose Trx has a date less than
    '   a specified date, i.e. finds the index before a specified date.
    '   Optimized for dates near the end of the register.
    '$Param datDate The date to search for.
    '$Returns The index of the Trx located, or zero if there were no Trx with
    '   date less than datDate.

    Public Function lngFindBeforeDate(ByVal datDate As Date) As Integer
        Dim lngIndex As Integer
        'lngIndex is initialized to mlngTrxUsed even if the body of the loop
        'is never executed, and will be left as zero if no matching Trx is found.
        For lngIndex = mlngTrxUsed To 1 Step -1
            If Me.objTrx(lngIndex).datDate < datDate Then
                Exit For
            End If
        Next
        lngFindBeforeDate = lngIndex
    End Function

    Friend Function objFirstOnOrAfter(ByVal datDate As Date) As Trx
        Dim lngIndex As Integer = lngFindBeforeDate(datDate) + 1
        If lngIndex > mlngTrxUsed Then
            Return Nothing
        End If
        Return Me.objTrx(lngIndex)
    End Function

    '$Description Find the transfer Trx which is an exact match to the specified info.
    '$Param datDate The transfer Trx date.
    '$Param strTransferKey The strTransferKey of the Trx in THIS register to look for.
    '$Param curAmount The curAmount of the Trx in THIS register to look for.
    '$Returns The index of the Trx exactly matching all the parameters, or zero if
    '   there is no such match. If there are multiple matches it will pick one,
    '   but it is undefined which it will pick.

    Friend Function lngMatchTransfer(ByVal datDate As Date, ByVal strTransferKey_ As String, ByVal curAmount As Decimal) As Integer

        Dim lngIndex As Integer
        Dim objTrx As Trx

        lngMatchTransfer = 0

        For lngIndex = mlngTrxUsed To 1 Step -1
            objTrx = Me.objTrx(lngIndex)
            If objTrx.datDate < datDate Then
                Exit Function
            End If
            If objTrx.GetType() Is GetType(TransferTrx) Then
                If objTrx.datDate = datDate Then
                    With DirectCast(objTrx, TransferTrx)
                        If .curAmount = curAmount Then
                            If .strTransferKey = strTransferKey_ Then
                                lngMatchTransfer = lngIndex
                                Exit Function
                            End If
                        End If
                    End With
                End If
            End If
        Next

    End Function

    '$Description Find a budget Trx matching the specified Trx date and budget key.
    '$Param datDate The datDate of the Trx to find a budget Trx for.
    '$Param strBudgetKey The strBudgetKey of the Trx to find a budget Trx for.
    '$Param blnNoMatch Set to True iff datDate>=mdatEarliestBudgetStart and no matching
    '   budget was found, otherwise set to False.
    '$Returns The index of the matching Trx, or zero if there is no match. This
    '   will be the latest dated budget Trx whose budget period includes datDate,
    '   and with the exact same strBudgetKey value.

    Friend Function objMatchBudget(ByVal datDate As Date, ByVal strBudgetKey As String, ByRef blnNoMatch As Boolean) As BudgetTrx

        Dim lngIndex As Integer
        Dim objBudgetTrx As BudgetTrx

        blnNoMatch = False
        For lngIndex = mlngTrxUsed To 1 Step -1
            objBudgetTrx = TryCast(Me.objTrx(lngIndex), BudgetTrx)
            If Not objBudgetTrx Is Nothing Then
                If objBudgetTrx.InBudgetPeriod(datDate) Then
                    If strBudgetKey = objBudgetTrx.strBudgetKey Then
                        Return objBudgetTrx
                    End If
                End If
            End If
        Next
        blnNoMatch = True
        Return Nothing

    End Function

    '$Description Used to report to the UI that a budget Trx has changed, in
    '   case balances need to be updated.

    Friend Sub RaiseBudgetChanged(ByVal objBudgetTrx As Trx)
        UpdateFirstAffected(objBudgetTrx.lngIndex)
        RaiseEvent BudgetChanged(objBudgetTrx.lngIndex, objBudgetTrx)
    End Sub

    Public Function objGetNormalTrxManager(ByVal lngIndex As Integer) As NormalTrxManager
        Return Me.objNormalTrx(lngIndex).objGetTrxManager()
    End Function

    Public Function objGetBudgetTrxManager(ByVal lngIndex As Integer) As BudgetTrxManager
        Return Me.objBudgetTrx(lngIndex).objGetTrxManager()
    End Function

    Public Function objGetTransferTrxManager(ByVal lngIndex As Integer) As TransferTrxManager
        Return Me.objTransferTrx(lngIndex).objGetTrxManager()
    End Function

    '$Description Return the Trx object at the specified row of the register.
    '$Param lngIndex 1 to lngTrxCount.
    '$Returns The Trx object. Properties of this object may be changed, if the
    '   caller takes the proper steps to update the register afterward.

    Public ReadOnly Property objTrx(ByVal lngIndex As Integer) As Trx
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
            objTrx = maobjTrx(lngIndex)
        End Get
    End Property

    Private Sub SetTrx(lngIndex As Integer, ByVal objTrx_ As Trx)
        maobjTrx(lngIndex) = objTrx_
        objTrx_.lngIndex = lngIndex
    End Sub

    Public ReadOnly Property objNormalTrx(ByVal lngIndex As Integer) As NormalTrx
        Get
            Return DirectCast(objTrx(lngIndex), NormalTrx)
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

    Public ReadOnly Property colDbgRepeatTrx() As Dictionary(Of String, Trx)
        Get
            Return mcolRepeatTrx
        End Get
    End Property

    Public Sub RaiseShowCurrent()
        If mlngTrxUsed = 0 Then
            Exit Sub
        End If
        If mlngTrxCurrent < 1 Then
            mlngTrxCurrent = 1
        ElseIf mlngTrxCurrent > mlngTrxUsed Then
            mlngTrxCurrent = mlngTrxUsed
        End If
        RaiseEvent ShowCurrent(mlngTrxCurrent)
    End Sub

    Public Sub SetCurrent(ByVal lngTrxCurrent_ As Integer)
        mlngTrxCurrent = lngTrxCurrent_
    End Sub

    Private Sub AddRepeatTrx(ByVal objTrx As Trx)
        mcolRepeatTrx.Add(objTrx.strRepeatId, objTrx)
    End Sub

    Private Sub SetRepeatTrx(ByVal objTrx As Trx)
        mcolRepeatTrx.Item(objTrx.strRepeatId) = objTrx
    End Sub

    Public Function objRepeatTrx(ByVal strRepeatKey As String, ByVal intRepeatSeq As Integer) As Trx
        Dim objTrx As Trx = Nothing
        If mcolRepeatTrx.TryGetValue(Trx.strMakeRepeatId(strRepeatKey, intRepeatSeq), objTrx) Then
            Return objTrx
        Else
            Return Nothing
        End If
    End Function

    Friend Sub RemoveRepeatTrx(ByVal objTrx As Trx)
        mcolRepeatTrx.Remove(objTrx.strRepeatId)
    End Sub

    Public Function lngCurrentTrxIndex() As Integer
        lngCurrentTrxIndex = mlngTrxCurrent
    End Function

    Public Function colDateRange(ByVal datStart As DateTime, ByVal datEnd As DateTime) As IEnumerable(Of Trx)
        Dim objRange As RegDateRange = New RegDateRange(Me, datStart, datEnd)
        Return objRange.colTrx()
    End Function

    Public Function colAllTrx() As IEnumerable(Of Trx)
        Dim objRange As RegIterator = New RegIterator(Me)
        Return objRange.colTrx()
    End Function

    Public Function curEndingBalance(ByVal datEndDate As DateTime) As Decimal
        Dim curBalance As Decimal
        For Each objTrx As Trx In colAllTrx()
            If objTrx.datDate > datEndDate Then
                Exit For
            End If
            curBalance = curBalance + objTrx.curAmount
        Next
        Return curBalance
    End Function

    '$Description Validate the register, and report all errors by firing
    '   ValidationError events. Checks balances, sort order, and consistency
    '   of data in individual Trx.

    Public Sub ValidateRegister()
        Dim lngIndex As Integer
        Dim objTrx As Trx
        Dim objTrx2 As Trx
        Dim objPriorTrx As Trx
        Dim curPriorBalance As Decimal
        Dim intRepeatTrxCount As Short

        curPriorBalance = 0
        objPriorTrx = Nothing

        For lngIndex = 1 To mlngTrxUsed
            objTrx = Me.objTrx(lngIndex)
            With objTrx
                If .curBalance <> (curPriorBalance + .curAmount) Then
                    RaiseValidationError(lngIndex, "Incorrect balance")
                End If
                curPriorBalance = .curBalance
                If Not objPriorTrx Is Nothing Then
                    If Register.intSortComparison(objTrx, objPriorTrx) < 0 Then
                        RaiseValidationError(lngIndex, "Not in correct sort order")
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
            RaiseValidationError(0, "Wrong number of repeat trx")
        End If

        For Each objTrx In mcolRepeatTrx.Values
            objTrx2 = objRepeatTrx(objTrx.strRepeatKey, objTrx.intRepeatSeq)
            If Not objTrx Is objTrx2 Then
                RaiseValidationError(0, "Repeat collection element points to wrong trx")
            End If
        Next objTrx

    End Sub

    Friend Sub RaiseValidationError(ByVal lngIndex As Integer, ByVal strMsg As String)
        RaiseEvent ValidationError(lngIndex, strMsg)
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