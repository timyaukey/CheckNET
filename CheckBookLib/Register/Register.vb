Option Strict Off
Option Explicit On
Public Class Register
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Represent a transaction register. The running balance is computed from scratch
    'each time it is loaded. Any transactions which are transfers to or from other
    'registers must have those registers loaded as well, which generally means all
    'registers must be loaded.

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
    'Do not search for budget Trx before this date.
    Private mdatEarliestBudgetStart As Date
    'Number of maobjTrx() array elements allocated at a time.
    Private mlngAllocationUnit As Integer
    'Show this register initially in the UI.
    Private mblnShowInitially As Boolean
    'Register contains only trx not known to the bank (all fake, no running balance).
    Private mblnNonBank As Boolean
    'Index of current Trx. Used only by the UI.
    Private mlngTrxCurrent As Integer
    'Date of oldest fake normal Trx found in LoadPostProcessing().
    Private mdatOldestFakeNormal As Date
    'Budget Trx with ending date older than this are always zero amount.
    Private mdatOldestBudgetEndAllowed As Date
    'Collection of all Trx that are part of a repeating sequence,
    'keyed by "#" & strRepeatKey & "." & intRepeatSeq.
    Private mcolRepeatTrx As Collection
    'Register is to define repeating Trx.
    Private mblnRepeat As Boolean
    'EventLog for Register.
    Private mobjLog As EventLog
    'True iff operator deleted register.
    Private mblnDeleted As Boolean

    'Temp hack to assign initial sequence numbers.
    'UPGRADE_WARNING: Lower bound of array maintNextSeq was changed from gintLBOUND1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
    Private maintNextSeq(200) As Short

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

    Public Sub Init(ByVal strTitle_ As String, ByVal strRegisterKey_ As String, ByVal blnShowInitially_ As Boolean, ByVal blnNonBank_ As Boolean, ByVal lngAllocationUnit_ As Integer, ByVal datOldestBudgetEndAllowed_ As Date, ByVal blnRepeat_ As Boolean)

        mstrTitle = strTitle_
        mstrRegisterKey = strRegisterKey_
        mlngAllocationUnit = lngAllocationUnit_
        mblnShowInitially = blnShowInitially_
        mblnNonBank = blnNonBank_
        If mstrRegisterKey = "" Then
            gRaiseError("Missing register key in Register.Init")
        End If
        mdatEarliestBudgetStart = #1/1/2100#
        mdatOldestFakeNormal = System.DateTime.FromOADate(0)
        mdatOldestBudgetEndAllowed = datOldestBudgetEndAllowed_
        mcolRepeatTrx = New Collection
        mblnRepeat = blnRepeat_
        Erase maobjTrx
        mlngTrxAllocated = 0
        mlngTrxUsed = 0
        mlngTrxCurrent = 0
        ClearFirstAffected()
        mobjLog = New EventLog
        mobjLog.Init(Me, gobjSecurity.strLogin)

    End Sub

    'Temp hack to assign initial sequence numbers.
    Public Function intGetNextRepeatSeq(ByVal intRepeatKey As Short) As Short
        maintNextSeq(intRepeatKey) = maintNextSeq(intRepeatKey) + 1
        intGetNextRepeatSeq = maintNextSeq(intRepeatKey)
    End Function

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

    Public Sub NewAddEnd(ByVal objNew As Trx, ByVal objAddLogger As ILogAdd, ByVal strTitle As String)

        Dim lngIndex As Integer

        If objNew Is Nothing Then
            gRaiseError("objNew is Nothing in Register.NewAddEnd")
        End If
        objNew.ApplyToBudgets(Me)
        lngIndex = lngNewInsert(objNew)
        mlngTrxCurrent = lngIndex
        RaiseEvent TrxAdded(lngIndex, objNew)
        UpdateFirstAffected(lngIndex)
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
            'UPGRADE_WARNING: Lower bound of array maobjTrx was changed from gintLBOUND1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
            ReDim Preserve maobjTrx(mlngTrxAllocated)
        End If
        objNew.SetSortKey()
        UpdateEarliestBudget(objNew)
        mlngTrxUsed = mlngTrxUsed + 1
        lngIndex = lngMoveUp(mlngTrxUsed - 1, objNew)
        lngNewInsert = lngIndex
    End Function

    '$Description Update datEarliestBudgetStart if a specified Trx is a budget
    '   with a date before datEarliestBudgetStart.
    '$Param objTrx Trx object to check.

    Private Sub UpdateEarliestBudget(ByVal objTrx As Trx)
        If objTrx.lngType = Trx.TrxType.glngTRXTYP_BUDGET Then
            If objTrx.datDate < mdatEarliestBudgetStart Then
                mdatEarliestBudgetStart = objTrx.datDate
            End If
        End If
    End Sub

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
        Dim strNewSortKey As String
        strNewSortKey = objTrx.strSortKey
        If lngEndIndex < 0 Or lngEndIndex > (mlngTrxUsed - 1) Then
            gRaiseError("Register.lngMoveUp passed invalid lngEndIndex=" & lngEndIndex)
        End If
        'When the loop is done, lngFirstLesser will equal the largest index whose Trx
        'sort key is less than objNew.strSortKey, or zero if there is none.
        For lngFirstLesser = lngEndIndex To 1 Step -1
            If strNewSortKey >= maobjTrx(lngFirstLesser).strSortKey Then
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
            maobjTrx(lngMoveIndex + 1) = maobjTrx(lngMoveIndex)
        Next
        maobjTrx(lngFirstLesser + 1) = objTrx
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
        Dim strNewSortKey As String
        If lngStartIndex < 2 Or lngStartIndex > mlngTrxUsed Then
            gRaiseError("Register.lngMoveDown passed invalid lngStartIndex " & lngStartIndex)
        End If
        strNewSortKey = objTrx.strSortKey
        'When the loop is done, lngFirstGreater will equal the smallest index whose Trx
        'sort key is greater than objNew.strSortKey, or zero if there is none.
        For lngFirstGreater = lngStartIndex To mlngTrxUsed
            If strNewSortKey <= maobjTrx(lngFirstGreater).strSortKey Then
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
            maobjTrx(lngMoveIndex - 1) = maobjTrx(lngMoveIndex)
        Next
        maobjTrx(lngFirstGreater - 1) = objTrx
        lngMoveDown = lngFirstGreater - 1
    End Function

    '$Description Finish updating an existing transaction. Moves it to the correct
    '   position in the sort order.
    '   Is optimized for the case where position in the sort order is not changed much.
    '$Param lngIndex The index of the Trx object in the register to move.

    Public Sub UpdateEnd(ByVal lngOldIndex As Integer, ByVal objChangeLogger As ILogChange, ByVal strTitle As String, ByVal objOldTrx As Trx)

        Dim lngNewIndex As Integer
        Dim objTrx As Trx

        'Hack error trap
        Try

            If lngOldIndex < 1 Or lngOldIndex > mlngTrxUsed Then
                gRaiseError("Invalid index " & lngOldIndex & " passed to Register.UpdateEnd")
            End If
            objTrx = maobjTrx(lngOldIndex)
            UpdateEarliestBudget(objTrx)
            If objTrx.intRepeatSeq > 0 Then
                SetRepeatTrx(objTrx)
            End If
            objTrx.ApplyToBudgets(Me)
            lngNewIndex = lngUpdateMove(lngOldIndex)
            mlngTrxCurrent = lngNewIndex
            RaiseEvent TrxUpdated(lngOldIndex, lngNewIndex, objTrx)
            UpdateFirstAffected(lngOldIndex)
            UpdateFirstAffected(lngNewIndex)
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
        Dim strNewSortKey As String

        objTrx = maobjTrx(lngOldIndex)
        objTrx.SetSortKey()
        strNewSortKey = objTrx.strSortKey

        If lngOldIndex > 1 Then
            If strNewSortKey < maobjTrx(lngOldIndex - 1).strSortKey Then
                lngUpdateMove = lngMoveUp(lngOldIndex - 1, objTrx)
                Exit Function
            End If
        End If

        If lngOldIndex < mlngTrxUsed Then
            If strNewSortKey > maobjTrx(lngOldIndex + 1).strSortKey Then
                lngUpdateMove = lngMoveDown(lngOldIndex + 1, objTrx)
                Exit Function
            End If
        End If

        lngUpdateMove = lngOldIndex

    End Function

    Public Sub ClearFirstAffected()
        mlngFirstAffected = mlngNO_TRX_AFFECTED
    End Sub

    Private Sub UpdateFirstAffected(ByVal lngIndex As Integer)
        If lngIndex < mlngFirstAffected Then
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
        ShowCurrent_Renamed()
    End Sub

    '$Description Set the status of a transaction, and fire a StatusChanged event for it.
    '$Param lngIndex The index of the Trx to change.
    '$Param lngStatus The new status.

    Public Sub SetTrxStatus(ByVal lngIndex As Integer, ByVal lngStatus As Trx.TrxStatus, ByVal objAddLogger As ILogAdd, ByVal strTitle As String)

        Dim objStatusTrx As Trx
        objStatusTrx = maobjTrx(lngIndex)
        objStatusTrx.SetStatus(lngStatus)
        RaiseEvent StatusChanged(lngIndex)
        'Use an ILogAdd instead of a specialized logger because it's a cheap
        'hack to reuse an existing type with the correct signature rather than
        'define an entire new one solely for this purpose.
        mobjLog.AddILogAdd(objAddLogger, strTitle, objStatusTrx)

    End Sub

    '$Description Delete a transaction from the register.
    '$Param lngIndex The index of the Trx to delete.

    Public Sub Delete(ByVal lngIndex As Integer, ByVal objDeleteLogger As ILogDelete, ByVal strTitle As String)
        Dim lngMoveIndex As Integer
        Dim objTrxOld As Trx
        If lngIndex < 1 Or lngIndex > mlngTrxUsed Then
            gRaiseError("Register.Delete passed invalid index=" & lngIndex)
        End If
        ClearFirstAffected()
        With maobjTrx(lngIndex)
            objTrxOld = .objClone(Nothing)
            'Budget Trx always come before any Split objects applied to
            'them, so deleting a normal Trx cannot change the index of
            'any budget Trx affected by the following statement.
            .UnApplyFromBudgets(Me)
            .DestroyThisBudget()
            If .strRepeatKey <> "" Then
                RemoveRepeatTrx(maobjTrx(lngIndex))
            End If
        End With
        For lngMoveIndex = lngIndex + 1 To mlngTrxUsed
            maobjTrx(lngMoveIndex - 1) = maobjTrx(lngMoveIndex)
        Next
        'UPGRADE_NOTE: Object maobjTrx() may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        maobjTrx(mlngTrxUsed) = Nothing
        mlngTrxUsed = mlngTrxUsed - 1
        RaiseEvent TrxDeleted(lngIndex)
        mlngTrxCurrent = lngIndex
        'Condition will be false if last trx deleted.
        If lngIndex <= mlngTrxUsed Then
            UpdateFirstAffected(lngIndex)
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
            curBalance = maobjTrx(lngStartIndex - 1).curBalance
        End If
        lngLastChange = lngStartIndex
        For lngIndex = lngStartIndex To mlngTrxUsed
            With maobjTrx(lngIndex)
                curBalance = curBalance + .curAmount
                If curBalance <> .curBalance Then
                    .SetBalance(curBalance)
                    lngLastChange = lngIndex
                End If
            End With
        Next
        lngFixBalances = lngLastChange
    End Function

    '$Description Perform all post processing steps after this Register is
    '   loaded with Trx objects from an external database. Applies everything to
    '   appropriate budgets, and computes balances.
    '   The register is ready to display in a UI after this.

    Public Sub LoadPostProcessing()

        Dim lngIndex As Integer

        For lngIndex = 1 To mlngTrxUsed
            With maobjTrx(lngIndex)
                .ApplyToBudgets(Me)
                If .lngType = Trx.TrxType.glngTRXTYP_NORMAL Then
                    If .blnFake Then
                        If mdatOldestFakeNormal = System.DateTime.FromOADate(0) Then
                            mdatOldestFakeNormal = .datDate
                        End If
                    End If
                End If
            End With
        Next
        lngFixBalances(1)

    End Sub

    '$Description Remove all generated Trx from the Register, and clear
    '   all budget allocations.

    Public Sub PurgeGenerated()
        Dim lngInIndex As Integer
        Dim lngOutIndex As Integer
        Dim objTrx As Trx
        Dim objSplit As Split_Renamed

        If mlngTrxUsed = 0 Then
            Exit Sub
        End If
        lngOutIndex = 0
        mdatEarliestBudgetStart = System.DateTime.FromOADate(0)
        For lngInIndex = 1 To mlngTrxUsed
            objTrx = maobjTrx(lngInIndex)
            If objTrx.blnAutoGenerated Then
                If objTrx.strRepeatKey <> "" Then
                    RemoveRepeatTrx(objTrx)
                End If
            Else
                lngOutIndex = lngOutIndex + 1
                maobjTrx(lngOutIndex) = objTrx
                If objTrx.lngType = Trx.TrxType.glngTRXTYP_BUDGET Then
                    objTrx.ClearThisBudget()
                    If mdatEarliestBudgetStart = System.DateTime.FromOADate(0) Then
                        mdatEarliestBudgetStart = objTrx.datDate
                    End If
                ElseIf objTrx.lngType = Trx.TrxType.glngTRXTYP_NORMAL Then
                    For Each objSplit In objTrx.colSplits
                        objSplit.ClearBudgetReference()
                    Next objSplit
                End If
            End If
        Next
        If lngOutIndex = 0 Then
            Erase maobjTrx
        Else
            'UPGRADE_WARNING: Lower bound of array maobjTrx was changed from gintLBOUND1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
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

    '$Description Find Trx object already in register with the specified strImportKey.
    '   Used to determine if a transaction has already been imported. Will only
    '   search real and normal Trx objects, because all imported Trx are real and
    '   normal. The usual procedure for importing a Trx is to first call this method
    '   to determine if it has already been imported, and if not then follow the
    '   normal procedure for adding a Trx. See MatchNormal() for notes on the
    '   procedure for adding a Trx.
    '$Param strImportKey The import key to match.
    '$Returns The index of the matching Trx, or zero if there is no match.

    Public Function lngMatchImportKey(ByVal strImportKey As String) As Integer
        Dim lngIndex As Integer
        For lngIndex = mlngTrxUsed To 1 Step -1
            With maobjTrx(lngIndex)
                If .lngType = Trx.TrxType.glngTRXTYP_NORMAL And Not .blnFake Then
                    If .strImportKey = strImportKey Then
                        Return lngIndex
                    End If
                End If
            End With
        Next
        Return 0
    End Function

    '$Description Find the register index of the specified Trx object.
    '$Param objTrx The Trx object to find.
    '$Returns The index of objTrx in this register, or zero if not found.

    Public Function lngFindTrx(ByVal objTrx As Trx) As Integer
        Dim lngIndex As Integer
        For lngIndex = mlngTrxUsed To 1 Step -1
            If maobjTrx(lngIndex) Is objTrx Then
                Return lngIndex
            End If
        Next
        Return 0
    End Function

    '$Description Find Trx object already in register matching all the arguments.
    '   Used to determine if a transaction has already been imported. Will only
    '   search real and normal Trx objects, because all imported Trx are real and
    '   normal. The usual procedure for importing a Trx is to first call this method
    '   to determine if it has already been imported, and if not then follow the
    '   normal procedure for adding a Trx. See MatchNormal() for notes on the
    '   procedure for adding a Trx.
    '$Returns The index of the matching Trx, or zero if there is no match.

    Public Function lngMatchPaymentDetails(ByVal strNumber As String, ByVal datDate As Date, ByVal intDateRange As Short, _
                                           ByVal strDescription As String, ByVal curAmount As Decimal) As Integer
        Dim lngIndex As Integer
        Dim datEarliestMatch As Date
        Dim datLatestMatch As Date
        datEarliestMatch = DateAdd(DateInterval.Day, -(intDateRange - 1), datDate)
        datLatestMatch = DateAdd(DateInterval.Day, intDateRange - 1, datDate)
        For lngIndex = mlngTrxUsed To 1 Step -1
            With maobjTrx(lngIndex)
                If .datDate < datEarliestMatch Then
                    Exit For
                End If
                If .datDate <= datLatestMatch Then
                    If .lngType = Trx.TrxType.glngTRXTYP_NORMAL And Not .blnFake Then
                        If .strNumber = strNumber And (.curAmount = curAmount Or curAmount = 0.0#) Then
                            If Left(.strDescription, 10).ToLower() = Left(strDescription, 10).ToLower() Then
                                Return lngIndex
                            End If
                        End If
                    End If
                End If
            End With
        Next
        Return 0
    End Function

    '$Description Update an existing Trx with information from a bank import. Only for
    '   updating an existing Trx. Use the normal steps for adding a new Trx if not
    '   updating an existing Trx, and be sure to pass the appropriate import key
    '   and say it is not fake.

    Public Sub ImportUpdateBank(ByVal lngOldIndex As Integer, ByVal datDate As Date, ByVal strNumber As String, ByVal blnFake As Boolean, _
                                ByVal curAmount As Decimal, ByVal strImportKey As String)

        Dim objTrx As Trx
        Dim objOldTrx As Trx

        If lngOldIndex < 1 Or lngOldIndex > mlngTrxUsed Then
            gRaiseError("Invalid index " & lngOldIndex & " passed to Register.ImportUpdateBank")
        End If
        objTrx = maobjTrx(lngOldIndex)
        objOldTrx = objTrx.objClone(Nothing)
        With objTrx
            .UnApplyFromBudgets(Me)
            .ImportUpdateBank(datDate, strNumber, blnFake, curAmount, strImportKey)
        End With
        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogChange). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        UpdateEnd(lngOldIndex, New LogChange, "ImportUpdateBank", objOldTrx)
    End Sub

    '$Description Update an existing Trx with number and amount. Only for
    '   updating an existing Trx. Use the normal steps for adding a new Trx if not
    '   updating an existing Trx.

    Public Sub ImportUpdateNumAmt(ByVal lngOldIndex As Integer, ByVal strNumber As String, ByVal blnFake As Boolean, ByVal curAmount As Decimal)

        Dim objTrx As Trx
        Dim objOldTrx As Trx

        If lngOldIndex < 1 Or lngOldIndex > mlngTrxUsed Then
            gRaiseError("Invalid index " & lngOldIndex & " passed to Register.ImportUpdateNumAmt")
        End If
        objTrx = maobjTrx(lngOldIndex)
        objOldTrx = objTrx.objClone(Nothing)
        With objTrx
            .UnApplyFromBudgets(Me)
            .ImportUpdateNumAmt(strNumber, blnFake, curAmount)
        End With
        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogChange). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        UpdateEnd(lngOldIndex, New LogChange, "ImportUpdateNumAmt", objOldTrx)
    End Sub

    '$Description Update an existing fake Trx with new amount and make it non-generated.
    '   Intended to update a generated Trx when the actual amount is known.

    Public Sub ImportUpdateAmount(ByVal lngOldIndex As Integer, ByVal blnFake As Boolean, ByVal curAmount As Decimal)

        Dim objTrx As Trx
        Dim objOldTrx As Trx

        If lngOldIndex < 1 Or lngOldIndex > mlngTrxUsed Then
            gRaiseError("Invalid index " & lngOldIndex & " passed to Register.ImportUpdateAmount")
        End If
        objTrx = maobjTrx(lngOldIndex)
        objOldTrx = objTrx.objClone(Nothing)
        With objTrx
            .UnApplyFromBudgets(Me)
            .ImportUpdateAmount(blnFake, curAmount)
        End With
        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogChange). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        UpdateEnd(lngOldIndex, New LogChange, "ImportUpdateAmount", objOldTrx)
    End Sub

    '$Description Update a specified Split in existing Trx with a new invoice for
    '   the same purchase order. Reduces the amount of the existing Split by the
    '   amount of the new invoice, and adds the new invoice as a new Split to the
    '   existing Trx. The Trx total amount does not change.

    Public Sub ImportUpdatePurchaseOrder(ByVal lngOldIndex As Integer, ByVal objPOSplit As Split_Renamed, ByVal objImportedSplit As Split_Renamed)

        Dim objTrx As Trx
        Dim objOldTrx As Trx

        If lngOldIndex < 1 Or lngOldIndex > mlngTrxUsed Then
            gRaiseError("Invalid index " & lngOldIndex & " passed to Register.ImportUpdatePurchaseOrder")
        End If
        objTrx = maobjTrx(lngOldIndex)
        objOldTrx = objTrx.objClone(Nothing)
        With objTrx
            .UnApplyFromBudgets(Me)
            .ImportUpdatePurchaseOrder(objPOSplit, objImportedSplit)
        End With
        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogChange). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        UpdateEnd(lngOldIndex, New LogChange, "ImportUpdatePurchaseOrder", objOldTrx)
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

    Public Sub MatchNormal(ByVal lngNumber As Integer, ByVal datDate As Date, ByVal intDateRange As Short, _
                           ByVal strDescription As String, ByVal curAmount As Decimal, ByVal blnLooseMatch As Boolean, _
                           ByRef colMatches As Collection, ByRef blnExactMatch As Boolean)
        Dim colExactMatches As Collection = Nothing
        MatchCore(lngNumber, datDate, intDateRange, strDescription, curAmount, 0.0#, 0.0#, blnLooseMatch, colMatches, colExactMatches, blnExactMatch)
        PruneToExactMatches(colExactMatches, datDate, colMatches, blnExactMatch)
    End Sub

    Public Sub MatchCore(ByVal lngNumber As Integer, ByVal datDate As Date, ByVal intDateRange As Short, _
                         ByVal strDescription As String, ByVal curAmount As Decimal, _
                         ByVal curMatchMin As Decimal, ByVal curMatchMax As Decimal, _
                         ByVal blnLooseMatch As Boolean, ByRef colMatches As Collection, _
                         ByRef colExactMatches As Collection, ByRef blnExactMatch As Boolean)

        Dim lngIndex As Integer
        Dim datEnd As Date
        Dim strNumber As String
        Dim blnMatched As Boolean
        Dim strDescrLC As String
        Dim curAmountRange As Decimal
        Dim blnDescrMatches As Boolean
        Dim blnDateMatches As Boolean
        Dim blnAmountMatches As Boolean
        Dim intDescrMatchLen As Short
        Dim objTrx As Trx

        colMatches = New Collection
        blnExactMatch = False
        colExactMatches = New Collection
        intDescrMatchLen = 10
        lngIndex = lngFindBeforeDate(DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDateRange, datDate)) + 1
        datEnd = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDateRange, datDate)
        strNumber = CStr(lngNumber)
        strDescrLC = Left(LCase(strDescription), intDescrMatchLen)
        Do
            If lngIndex > mlngTrxUsed Then
                Exit Do
            End If
            objTrx = maobjTrx(lngIndex)
            With objTrx
                If .datDate > datEnd Then
                    Exit Do
                End If
                If .lngType = Trx.TrxType.glngTRXTYP_NORMAL Then
                    blnMatched = False
                    If lngNumber <> 0 Then
                        If .strNumber = strNumber Then
                            blnMatched = True
                        End If
                    End If
                    If blnLooseMatch Then
                        curAmountRange = System.Math.Abs(curAmount * 0.2)
                        If .curNormalMatchRange > curAmountRange Then
                            curAmountRange = .curNormalMatchRange
                        End If
                    Else
                        curAmountRange = .curNormalMatchRange
                    End If
                    blnDescrMatches = (Left(LCase(.strDescription), intDescrMatchLen) = strDescrLC)
                    'UPGRADE_WARNING: DateDiff behavior may be different. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B38EC3F-686D-4B2E-B5A5-9E8E7A762E32"'
                    blnDateMatches = (System.Math.Abs(DateDiff(Microsoft.VisualBasic.DateInterval.Day, .datDate, datDate)) < 6)
                    blnAmountMatches = (System.Math.Abs(.curAmount - curAmount) <= curAmountRange)
                    If (IIf(blnAmountMatches, 1, 0) + IIf(blnDateMatches, 1, 0)) >= IIf(blnLooseMatch, 1, 2) Then
                        blnMatched = True
                    End If
                    If blnMatched Or blnDescrMatches Then
                        'If min/max were specified this is a mandatory amount filter, separate from blnAmountMatches.
                        If curMatchMin <> 0.0# And curMatchMax <> 0.0# Then
                            If (.curAmount >= curMatchMin) And (.curAmount <= curMatchMax) Then
                                colMatches.Add(lngIndex)
                            End If
                        Else
                            colMatches.Add(lngIndex)
                        End If
                    End If
                    If .curAmount = curAmount Then
                        If (blnDescrMatches And blnDateMatches) Or (.strNumber = CStr(lngNumber)) Then
                            colExactMatches.Add(lngIndex)
                        End If
                    End If
                End If
            End With
            lngIndex = lngIndex + 1
        Loop

    End Sub

    Delegate Function PruneMatchesTrx(ByVal objTrx As Trx) As Boolean

    ''' <summary>
    ''' Narrow down the results to one or more Trx in colExactMatches if 
    ''' there is anything in colExactMatches.
    ''' </summary>
    ''' <param name="colExactMatches"></param>
    ''' <param name="colMatches"></param>
    ''' <param name="blnExactMatch"></param>
    ''' <param name="blnTrxPruner"></param>
    ''' <remarks></remarks>
    Public Sub PruneSearchMatches(ByVal colExactMatches As Collection, ByRef colMatches As Collection, _
                                  ByRef blnExactMatch As Boolean, ByVal blnTrxPruner As PruneMatchesTrx)
        Dim lngIndex As Integer
        Dim lngPerfectMatchIndex As Integer
        Dim vlngMatchIndex As Object
        Dim datFirstMatch As DateTime
        Dim datLastMatch As DateTime
        Dim blnFirstIteration As Boolean
        Dim objTrx As Trx

        'If we have multiple exact matches, see if all are within a range of 5 days
        'and one passes the test of blnTrxPruner(). If so use that one alone as the
        'list of exact matches.
        If colExactMatches.Count() > 1 Then
            lngPerfectMatchIndex = -1
            blnFirstIteration = True
            For Each vlngMatchIndex In colExactMatches
                lngIndex = vlngMatchIndex
                objTrx = maobjTrx(lngIndex)
                If blnFirstIteration Then
                    datFirstMatch = objTrx.datDate
                    datLastMatch = objTrx.datDate
                    blnFirstIteration = False
                Else
                    If objTrx.datDate < datFirstMatch Then
                        datFirstMatch = objTrx.datDate
                    End If
                    If objTrx.datDate > datLastMatch Then
                        datLastMatch = objTrx.datDate
                    End If
                End If
                If blnTrxPruner(objTrx) Then
                    If lngPerfectMatchIndex = -1 Then
                        lngPerfectMatchIndex = lngIndex
                    End If
                    Exit For
                End If
            Next vlngMatchIndex
            If lngPerfectMatchIndex <> -1 And datLastMatch.Subtract(datFirstMatch).TotalDays <= 2D Then
                colExactMatches = New Collection
                colExactMatches.Add(lngPerfectMatchIndex)
            End If
        End If

        'If have exact matches, return them only.
        If colExactMatches.Count > 0 Then
            colMatches = colExactMatches
            'If we have one exact match, say we have only one.
            If colExactMatches.Count = 1 Then
                blnExactMatch = True
            End If
        End If

    End Sub

    Public Sub PruneToExactMatches(ByVal colExactMatches As Collection, ByVal datDate As Date, ByRef colMatches As Collection, ByRef blnExactMatch As Boolean)

        PruneSearchMatches(colExactMatches, colMatches, blnExactMatch, Function(objTrx As Trx) objTrx.datDate = datDate)

    End Sub

    Public Sub PruneToNonImportedExactMatches(ByVal colExactMatches As Collection, ByVal datDate As Date, ByRef colMatches As Collection, ByRef blnExactMatch As Boolean)

        PruneSearchMatches(colExactMatches, colMatches, blnExactMatch, _
                           Function(objTrx As Trx) As Boolean
                               If objTrx.datDate = datDate Then
                                   If objTrx.lngStatus <> Trx.TrxStatus.glngTRXSTS_RECON Then
                                       If String.IsNullOrEmpty(objTrx.strImportKey) Then
                                           Return True
                                       End If
                                   End If
                               End If
                               Return False
                           End Function)
    End Sub

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

    Public Sub MatchPayee(ByVal datDate As Date, ByVal intDateRange As Short, ByVal strDescription As String, ByVal blnMatchImportedFromBank As Boolean, ByRef colMatches As Collection, ByRef blnExactMatch As Boolean)

        Dim lngIndex As Integer
        Dim datEnd As Date
        '    Dim strNumber As String
        Dim blnImportOkay As Boolean

        colMatches = New Collection
        blnExactMatch = False
        lngIndex = lngFindBeforeDate(DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDateRange, datDate)) + 1
        datEnd = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDateRange, datDate)
        Do
            If lngIndex > mlngTrxUsed Then
                Exit Do
            End If
            With maobjTrx(lngIndex)
                If .datDate > datEnd Then
                    Exit Do
                End If
                If .lngType = Trx.TrxType.glngTRXTYP_NORMAL Then
                    blnImportOkay = (.strImportKey = "") Or (blnMatchImportedFromBank) 'Used to be (not blnMatchImportedFromBank)
                    If .strDescription = strDescription And blnImportOkay Then
                        colMatches.Add(lngIndex)
                    End If
                End If
            End With
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

    Public Sub MatchInvoice(ByVal datDate As Date, ByVal intDateRange As Short, ByVal strPayee As String, ByVal strInvoiceNum As String, ByRef colMatches As Collection)

        Dim lngIndex As Integer
        Dim datEnd As Date
        Dim objSplit As Split_Renamed

        colMatches = New Collection
        lngIndex = lngFindBeforeDate(DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDateRange, datDate)) + 1
        datEnd = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDateRange, datDate)
        Do
            If lngIndex > mlngTrxUsed Then
                Exit Do
            End If
            With maobjTrx(lngIndex)
                If .datDate > datEnd Then
                    Exit Do
                End If
                If .lngType = Trx.TrxType.glngTRXTYP_NORMAL Then
                    If .strDescription = strPayee Then
                        For Each objSplit In .colSplits
                            If objSplit.strInvoiceNum = strInvoiceNum Then
                                colMatches.Add(lngIndex)
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

    Public Sub MatchPONumber(ByVal datDate As Date, ByVal intDateRange As Short, ByVal strPayee As String, ByVal strPONumber As String, ByRef colMatches As Collection)

        Dim lngIndex As Integer
        Dim datEnd As Date
        Dim objSplit As Split_Renamed

        colMatches = New Collection
        lngIndex = lngFindBeforeDate(DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDateRange, datDate)) + 1
        datEnd = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDateRange, datDate)
        Do
            If lngIndex > mlngTrxUsed Then
                Exit Do
            End If
            With maobjTrx(lngIndex)
                If .datDate > datEnd Then
                    Exit Do
                End If
                If .lngType = Trx.TrxType.glngTRXTYP_NORMAL Then
                    If .strDescription = strPayee Then
                        For Each objSplit In .colSplits
                            If objSplit.strPONumber = strPONumber Then
                                colMatches.Add(lngIndex)
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
            If maobjTrx(lngIndex).datDate < datDate Then
                Exit For
            End If
        Next
        lngFindBeforeDate = lngIndex
    End Function

    '$Description Find the transfer Trx which is an exact match to the specified info.
    '$Param datDate The transfer Trx date.
    '$Param strTransferKey The strTransferKey of the Trx in THIS register to look for.
    '$Param curAmount The curAmount of the Trx in THIS register to look for.
    '$Returns The index of the Trx exactly matching all the parameters, or zero if
    '   there is no such match. If there are multiple matches it will pick one,
    '   but it is undefined which it will pick.

    Public Function lngMatchTransfer(ByVal datDate As Date, ByVal strTransferKey_ As String, ByVal curAmount As Decimal) As Integer

        Dim lngIndex As Integer

        lngMatchTransfer = 0

        For lngIndex = mlngTrxUsed To 1 Step -1
            With maobjTrx(lngIndex)
                If .datDate < datDate Then
                    Exit Function
                End If
                If .lngType = Trx.TrxType.glngTRXTYP_TRANSFER Then
                    If .datDate = datDate Then
                        If .curAmount = curAmount Then
                            If .strTransferKey = strTransferKey_ Then
                                lngMatchTransfer = lngIndex
                                Exit Function
                            End If
                        End If
                    End If
                End If
            End With
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

    Public Function lngMatchBudget(ByVal datDate As Date, ByVal strBudgetKey As String, ByRef blnNoMatch As Boolean) As Integer

        Dim lngIndex As Integer

        lngMatchBudget = 0
        blnNoMatch = False
        If datDate < mdatEarliestBudgetStart Then
            Exit Function
        End If
        For lngIndex = mlngTrxUsed To 1 Step -1
            With maobjTrx(lngIndex)
                If .datDate < mdatEarliestBudgetStart Then
                    Exit For
                End If
                If .lngType = Trx.TrxType.glngTRXTYP_BUDGET Then
                    If datDate >= .datDate And datDate <= .datBudgetEnds Then
                        If strBudgetKey = .strBudgetKey Then
                            lngMatchBudget = lngIndex
                            Exit Function
                        End If
                    End If
                End If
            End With
        Next
        blnNoMatch = True

    End Function

    '$Description Used to report to the UI that a budget Trx has changed, in
    '   case balances need to be updated.

    'UPGRADE_NOTE: BudgetChanged was upgraded to BudgetChanged_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Public Sub BudgetChanged_Renamed(ByVal objBudgetTrx As Trx)
        Dim lngIndex As Integer
        lngIndex = lngTrxIndex(objBudgetTrx)
        UpdateFirstAffected(lngIndex)
        RaiseEvent BudgetChanged(lngIndex, objBudgetTrx)
    End Sub

    '$Description Determine the index at which the specified Trx exists.
    '   Optimized for case where Trx is near end of register.
    '$Param objTrx The Trx to search for.
    '$Returns The index of that Trx, or zero if Trx not found in register.

    Public Function lngTrxIndex(ByVal objTrx As Trx) As Integer
        Dim lngIndex As Integer
        For lngIndex = mlngTrxUsed To 1 Step -1
            If maobjTrx(lngIndex) Is objTrx Then
                lngTrxIndex = lngIndex
                Exit Function
            End If
        Next
        lngTrxIndex = 0
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

    '$Description The number of transactions in the register.

    Public ReadOnly Property lngTrxCount() As Integer
        Get
            lngTrxCount = mlngTrxUsed
        End Get
    End Property

    '$Description The unique identifier for this register (sub-account).
    '   Used to identify transfer destinations, among other things.

    Public ReadOnly Property strRegisterKey() As String
        Get
            strRegisterKey = mstrRegisterKey
        End Get
    End Property


    Public Property strTitle() As String
        Get
            strTitle = mstrTitle
        End Get
        Set(ByVal Value As String)
            mstrTitle = Value
            RaiseEvent MiscChange()
        End Set
    End Property


    Public Property datEarliestBudgetStart() As Date
        Get
            datEarliestBudgetStart = mdatEarliestBudgetStart
        End Get
        Set(ByVal Value As Date)
            mdatEarliestBudgetStart = Value
        End Set
    End Property

    Public ReadOnly Property datOldestBudgetEndAllowed() As Date
        Get
            datOldestBudgetEndAllowed = mdatOldestBudgetEndAllowed
        End Get
    End Property

    Public ReadOnly Property datOldestFakeNormal() As Date
        Get
            datOldestFakeNormal = mdatOldestFakeNormal
        End Get
    End Property


    Public Property blnShowInitially() As Boolean
        Get
            blnShowInitially = mblnShowInitially
        End Get
        Set(ByVal Value As Boolean)
            mblnShowInitially = Value
            RaiseEvent MiscChange()
        End Set
    End Property


    Public Property blnNonBank() As Boolean
        Get
            blnNonBank = mblnNonBank
        End Get
        Set(ByVal Value As Boolean)
            mblnNonBank = Value
            RaiseEvent MiscChange()
        End Set
    End Property

    Public ReadOnly Property blnRepeat() As Boolean
        Get
            blnRepeat = mblnRepeat
        End Get
    End Property

    Public Property blnDeleted() As Boolean
        Get
            blnDeleted = mblnDeleted
        End Get
        Set(ByVal Value As Boolean)
            mblnDeleted = Value
        End Set
    End Property

    '$Description For debugging use only - return the internal mcolRepeatTrx collection.

    Public ReadOnly Property colDbgRepeatTrx() As Collection
        Get
            colDbgRepeatTrx = mcolRepeatTrx
        End Get
    End Property

    'UPGRADE_NOTE: ShowCurrent was upgraded to ShowCurrent_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Public Sub ShowCurrent_Renamed()
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

    Public Sub AddRepeatTrx(ByVal objTrx As Trx)
        mcolRepeatTrx.Add(objTrx, objTrx.strRepeatId)
    End Sub

    Public Sub SetRepeatTrx(ByVal objTrx As Trx)
        Try
            mcolRepeatTrx.Add(objTrx, objTrx.strRepeatId)
        Catch
            mcolRepeatTrx.Remove(objTrx.strRepeatId)
            mcolRepeatTrx.Add(objTrx, objTrx.strRepeatId)
        End Try
    End Sub

    Public Function objRepeatTrx(ByVal strRepeatKey As String, ByVal intRepeatSeq As Short) As Trx
        Try
            objRepeatTrx = mcolRepeatTrx.Item(gstrMakeRepeatId(strRepeatKey, intRepeatSeq))
        Catch
            objRepeatTrx = Nothing
        End Try
    End Function

    Public Sub RemoveRepeatTrx(ByVal objTrx As Trx)
        mcolRepeatTrx.Remove(objTrx.strRepeatId)
    End Sub

    Public Function lngCurrentTrxIndex() As Integer
        lngCurrentTrxIndex = mlngTrxCurrent
    End Function

    '$Description Return a new RegCursor initialized with this Register.

    Public Function objGetCursor() As RegCursor
        Dim objCursor As RegCursor
        objCursor = New RegCursor
        objCursor.Init(Me)
        objGetCursor = objCursor
    End Function

    '$Description Validate the register, and report all errors by firing
    '   ValidationError events. Checks balances, sort order, and consistency
    '   of data in individual Trx.

    Public Sub ValidateRegister()
        Dim lngIndex As Integer
        Dim objTrx As Trx
        Dim objTrx2 As Trx
        Dim strPriorSortKey As String
        Dim curPriorBalance As Decimal
        Dim intRepeatTrxCount As Short

        curPriorBalance = 0
        strPriorSortKey = ""

        For lngIndex = 1 To mlngTrxUsed
            objTrx = maobjTrx(lngIndex)
            With objTrx
                If Not mblnRepeat Then
                    If .curBalance <> (curPriorBalance + .curAmount) Then
                        ValidationError_Renamed(lngIndex, "Incorrect balance")
                    End If
                End If
                curPriorBalance = .curBalance
                If .strSortKey < strPriorSortKey Then
                    ValidationError_Renamed(lngIndex, "Not in correct sort order")
                End If
                strPriorSortKey = .strSortKey
                .Validate(Me, lngIndex)
                If .intRepeatSeq > 0 Then
                    intRepeatTrxCount = intRepeatTrxCount + 1
                End If
            End With
        Next

        If intRepeatTrxCount <> mcolRepeatTrx.Count() Then
            ValidationError_Renamed(0, "Wrong number of repeat trx")
        End If

        For Each objTrx In mcolRepeatTrx
            objTrx2 = objRepeatTrx(objTrx.strRepeatKey, objTrx.intRepeatSeq)
            If Not objTrx Is objTrx2 Then
                ValidationError_Renamed(0, "Repeat collection element points to wrong trx")
            End If
        Next objTrx

    End Sub

    'UPGRADE_NOTE: ValidationError was upgraded to ValidationError_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Public Sub ValidationError_Renamed(ByVal lngIndex As Integer, ByVal strMsg As String)
        RaiseEvent ValidationError(lngIndex, strMsg)
    End Sub

    Public Sub LogAction(ByVal strTitle As String)
        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogAction). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        mobjLog.AddILogAction(New LogAction, strTitle)
    End Sub

    Public Sub LogSave()
        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogSave). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        mobjLog.AddILogAction(New LogSave, "Register.Save")
    End Sub

    Public Function objLogGroupStart(ByVal strTitle As String) As ILogGroupStart
        Dim objStartLogger As ILogGroupStart
        objStartLogger = New LogGroupStart
        mobjLog.AddILogGroupStart(objStartLogger, strTitle)
        objLogGroupStart = objStartLogger
    End Function

    Public Sub LogGroupEnd(ByVal objStartLogger As ILogGroupStart)
        'UPGRADE_WARNING: Couldn't resolve default property of object New (LogGroupEnd). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        mobjLog.AddILogGroupEnd(New LogGroupEnd, objStartLogger)
    End Sub

    Public Sub WriteEventLog(ByVal strAccountTitle As String, ByVal objRepeats As StringTranslator)
        mobjLog.WriteAll(strAccountTitle, objRepeats)
    End Sub
End Class