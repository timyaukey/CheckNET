Option Strict On
Option Explicit On

Imports CheckBookLib

Friend Class AdjustBudgetsToCashForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Adjust a specified set of budget transactions so the minimum balance in a
    'register after some date exactly equals some target amount. The budget
    'transactions adjusted are those with a specified budget key on or after a
    'specified date, normally today. On each day with any of those budgets, all
    'budgets not present on that day will be added and then the total set on that
    'day will be adjusted to achieve the desired target balance.

    Private mobjReg As Register
    Private mobjBudgets As IStringTranslator

    Private Const mintNUM_BUDGETS As Short = 5

    Private cboBudget(mintNUM_BUDGETS) As ComboBox
    Private txtPercent(mintNUM_BUDGETS) As TextBox
    'Register index of budget Trx with budget in cboBudget(i).
    Private malngRegIndex(mintNUM_BUDGETS) As Integer
    'Trx objects referenced by malngRegIndex(). Used for sanity check.
    Private maobjRegTrx(mintNUM_BUDGETS) As BudgetTrx
    'Budget key of budget Trx with budget in cboBudget(i).
    Private mastrBudgetKey(mintNUM_BUDGETS) As String

    'The budget limit of at least one adjusted budget was exceeded.
    Private mblnAdjustedBudgetExceeded As Boolean

    Private Const mstrREG_BUDKEY_NAME As String = "BudgetKey"
    Private Const mstrREG_BUDPCT_NAME As String = "BudgetPercentage"
    Private Const mstrREG_MINBAL As String = "MinBalance"
    Private Const mstrREG_NAMEPRE As String = "NamePrefix"

    Public Sub ShowModal(ByVal objReg_ As Register, ByVal objBudgets_ As IStringTranslator)
        Try

            cboBudget(1) = _cboBudget_1
            cboBudget(2) = _cboBudget_2
            cboBudget(3) = _cboBudget_3
            cboBudget(4) = _cboBudget_4
            cboBudget(5) = _cboBudget_5
            txtPercent(1) = _txtPercent_1
            txtPercent(2) = _txtPercent_2
            txtPercent(3) = _txtPercent_3
            txtPercent(4) = _txtPercent_4
            txtPercent(5) = _txtPercent_5
            mobjReg = objReg_
            mobjBudgets = objBudgets_
            Me.Text = "Adjust Budgets For " & objReg_.strTitle
            LoadSavedSpecs()
            Me.ShowDialog()

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub LoadSavedSpecs()
        Dim intIndex As Short

        For intIndex = 1 To mintNUM_BUDGETS
            LoadSavedBudget(intIndex)
        Next
        txtStartingDate.Text = gstrFormatDate(Today)
        txtMinBal.Text = GetSetting(gstrREG_APP, strRegSection(), mstrREG_MINBAL)
        txtPrefix.Text = GetSetting(gstrREG_APP, strRegSection(), mstrREG_NAMEPRE)
    End Sub

    Private Function strRegSection() As String
        strRegSection = mobjReg.strRegistryKey() & "\CashFlowBudgetAdj"
    End Function

    Private Sub LoadSavedBudget(ByVal intIndex As Short)
        Dim strValue1 As String

        Try

            gLoadComboFromStringTranslator(cboBudget(intIndex), mobjBudgets, True)
            With cboBudget(intIndex)
                strValue1 = mobjBudgets.strKeyToValue1(GetSetting(gstrREG_APP, strRegSection(), mstrREG_BUDKEY_NAME & intIndex))
                If strValue1 = "" Then
                    .SelectedIndex = -1
                Else
                    .Text = strValue1
                End If
            End With
            txtPercent(intIndex).Text = GetSetting(gstrREG_APP, strRegSection(), mstrREG_BUDPCT_NAME & intIndex)

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub cmdAdjust_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdjust.Click
        Dim lngStartIndex As Integer
        Dim lngIndex As Integer
        Dim datMatch As Date
        Dim objStartLogger As ILogGroupStart

        Try

            If Not blnValidate() Then
                Exit Sub
            End If

            objStartLogger = mobjReg.objLogGroupStart("AdjustBudgets")
            mblnAdjustedBudgetExceeded = False
            lvwResults.Items.Clear()
            lngStartIndex = mobjReg.lngFindBeforeDate(CDate(txtStartingDate.Text)) + 1
            lngIndex = lngStartIndex
            While lngIndex <= mobjReg.lngTrxCount
                If blnScanWholeDaysForBudgets(lngIndex, datMatch) Then
                    ZeroMatchedBudgets()
                End If
            End While
            lngIndex = lngStartIndex
            While lngIndex <= mobjReg.lngTrxCount
                If blnScanWholeDaysForBudgets(lngIndex, datMatch) Then
                    SetBudgets(lngIndex, datMatch)
                End If
            End While
            mobjReg.LogGroupEnd(objStartLogger)
            DoubleCheckBalances(lngStartIndex)
            If mblnAdjustedBudgetExceeded Then
                MsgBox("WARNING: At least one of the adjusted budgets is already " & "overspent, which confuses the budget adjustment algorithm. " & "The register is still okay, but the adjusted budgets may have " & "silly amounts.", MsgBoxStyle.Critical)
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    '$Description Validate the specifications, and set up mastrBudgetKey().

    Private Function blnValidate() As Boolean
        Dim intTotalPercent As Integer
        Dim intIndex As Integer
        Dim intBudgetsSelected As Integer

        Try

            For intIndex = 1 To mintNUM_BUDGETS
                mastrBudgetKey(intIndex) = ""
                If blnBudgetUsed(intIndex) Then
                    If Not IsNumeric(txtPercent(intIndex).Text) Then
                        MsgBox("Invalid percentage in budget #" & intIndex)
                        Exit Function
                    End If
                    intTotalPercent = intTotalPercent + CShort(txtPercent(intIndex).Text)
                    intBudgetsSelected = intBudgetsSelected + 1
                    mastrBudgetKey(intIndex) = strGetStringTranslatorKeyFromCombo(cboBudget(intIndex), mobjBudgets)
                Else
                    If txtPercent(intIndex).Text <> "" Then
                        MsgBox("Percentage must be empty unless budget is selected.", MsgBoxStyle.Critical)
                        Exit Function
                    End If
                End If
            Next

            If intBudgetsSelected = 0 Then
                MsgBox("Please select at least one budget to adjust.", MsgBoxStyle.Critical)
                Exit Function
            End If
            If intTotalPercent <> 100 Then
                MsgBox("Percentages of selected budgets must add up to 100.", MsgBoxStyle.Critical)
                Exit Function
            End If
            If Not gblnValidDate(txtStartingDate.Text) Then
                MsgBox("Invalid starting date.", MsgBoxStyle.Critical)
                Exit Function
            End If
            If Not gblnValidAmount(txtMinBal.Text) Then
                MsgBox("Invalid minimum balance.", MsgBoxStyle.Critical)
                Exit Function
            End If

            If MsgBox("Do you really want to fix these budgets?", MsgBoxStyle.Question Or MsgBoxStyle.OkCancel Or MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Ok Then
                Exit Function
            End If

            blnValidate = True

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Private Function blnBudgetUsed(ByVal intIndex As Integer) As Boolean
        blnBudgetUsed = (cboBudget(intIndex).SelectedIndex > 0)
    End Function

    '$Description Check whole days, and return when a day has any
    '   of the specified budget Trx or it is the last day in the
    '   register.
    '$Param lngIndex On entry the first Trx to check. On return will be the first
    '   Trx after the day just completed, i.e. the index to pass the next
    '   time this method is called.
    '$Param datMatch The date on which budgets were matched.
    '$Returns True iff one or more of the specified budgets was found.

    Private Function blnScanWholeDaysForBudgets(ByRef lngIndex As Integer, ByRef datMatch As Date) As Boolean

        Dim lngTrxCount As Integer
        Dim datPrev As Date
        Dim objTrx As Trx
        Dim objBudget As BudgetTrx
        Dim blnAnyMatches As Boolean
        Dim intBudget As Short
        Dim strTrxBudgetKey As String

        Try

            'Clear match information.
            For intBudget = 1 To mintNUM_BUDGETS
                malngRegIndex(intBudget) = 0
                'UPGRADE_NOTE: Object maobjRegTrx() may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                maobjRegTrx(intBudget) = Nothing
            Next

            datPrev = mobjReg.objTrx(lngIndex).datDate
            lngTrxCount = mobjReg.lngTrxCount
            Do
                'Are we past the end of the register?
                If lngIndex > lngTrxCount Then
                    'The day is definitely finished (!),
                    'the only question is whether we found any
                    'matches on it. This is the only way we may
                    'exit without any matches.
                    Exit Do
                End If
                'There IS a trx, so check it.
                objTrx = mobjReg.objTrx(lngIndex)
                'If we are starting a new day.
                If objTrx.datDate <> datPrev Then
                    'If there are any matches on the day just ended.
                    'Since we check after each day, any matches at all is
                    'the same as any matches on the day just ended.
                    If blnAnyMatches Then
                        Exit Do
                    End If
                    'No matches on previous date, so restart on this date.
                    datPrev = objTrx.datDate
                End If
                'If this is a budget trx, see if it matches anything we are looking for.
                If objTrx.lngType = Trx.TrxType.glngTRXTYP_BUDGET Then
                    objBudget = DirectCast(objTrx, BudgetTrx)
                    strTrxBudgetKey = objBudget.strBudgetKey
                    'Check against each budget key we are looking for.
                    For intBudget = 1 To mintNUM_BUDGETS
                        'Is this budget key specified?
                        If Len(mastrBudgetKey(intBudget)) > 0 Then
                            'Does it match the current trx?
                            If mastrBudgetKey(intBudget) = strTrxBudgetKey Then
                                'Remember where we found it. Note that this index
                                'only remains valid so long as we don't insert or
                                'delete above it in the register, so we must do
                                'our updates before adding new budgets! And every
                                'budget we add, we must increment the starting
                                'position of the next can to this method because
                                'the added budget will always be inserted above
                                'the starting index of that scan.
                                malngRegIndex(intBudget) = lngIndex
                                'Will confirm this later during the update as a sanity
                                'check to insure intervening activity has not caused
                                'the Trx to move in the register.
                                maobjRegTrx(intBudget) = objBudget
                                blnAnyMatches = True
                            End If
                        End If
                    Next
                End If
                lngIndex = lngIndex + 1
            Loop
            blnScanWholeDaysForBudgets = blnAnyMatches
            If blnAnyMatches Then
                datMatch = datPrev
            End If

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    '$Description Set the budget limits for all matched budget transactions to zero.
    '   Called once for each day found by blnScanWholeDaysForBudgets().

    Private Sub ZeroMatchedBudgets()
        Dim intBudget As Integer

        Try

            For intBudget = 1 To mintNUM_BUDGETS
                If Len(mastrBudgetKey(intBudget)) > 0 Then
                    If malngRegIndex(intBudget) > 0 Then
                        UpdateBudget(malngRegIndex(intBudget), 0, intBudget)
                    End If
                End If
            Next

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Set the budget limits all budgets on one day to amounts computed
    '   to yield a specific minimum register balance from this point forward. Called
    '   once for each day found by blnScanWholeDaysForBudgets(). Adjusts budget
    '   transactions which were found, and inserts any that were not. Works correctly
    '   even if normal transactions are already applied to those budgets, so long
    '   as the budgets are not overspent. Compensates for applied transactions by
    '   acting as if they were applied after the budgets were adjusted.

    Private Sub SetBudgets(ByRef lngContinueIndex As Integer, ByVal datMatch As Date)

        Dim intBudget As Integer
        Dim lngUpdateIndex As Integer
        Dim objBudgetTrx As BudgetTrx
        Dim datBudgetEnds As Date
        Dim acurNewLimit(mintNUM_BUDGETS) As Decimal
        Dim curTotalAvailable As Decimal
        Dim curRemainderAvailable As Decimal
        Dim intLastSpecifiedBudget As Integer
        Dim curAlreadySpent As Decimal

        Try

            'NOTE NOTE NOTE!
            'This algorithm gets confused when transactions applied to budgets which
            'are being adjusted exceed the budget limits. This should not be a problem
            'in practice, since we generally only adjust future dated budgets and those
            'should never be overspent!

            'How much money do we have available?
            For intBudget = 1 To mintNUM_BUDGETS
                If Len(mastrBudgetKey(intBudget)) > 0 Then
                    'To avoid rounding problems, the last specified budget always gets
                    'what is left over rather than computing it as a percentage of the
                    'total available as we do for the others.
                    intLastSpecifiedBudget = intBudget
                    If Not maobjRegTrx(intBudget) Is Nothing Then
                        curAlreadySpent = curAlreadySpent + maobjRegTrx(intBudget).curBudgetApplied
                    End If
                End If
            Next
            curTotalAvailable = curLowestBalance(lngContinueIndex) - CDec(txtMinBal.Text) - curAlreadySpent
            If curTotalAvailable < 0 Then
                curTotalAvailable = 0
            End If
            'Divide it up.
            curRemainderAvailable = curTotalAvailable
            For intBudget = 1 To mintNUM_BUDGETS
                If Len(mastrBudgetKey(intBudget)) > 0 Then
                    If intBudget = intLastSpecifiedBudget Then
                        acurNewLimit(intBudget) = -curRemainderAvailable
                    Else
                        acurNewLimit(intBudget) = -curTotalAvailable * (CDec(txtPercent(intBudget).Text) / 100D)
                        curRemainderAvailable = curRemainderAvailable + acurNewLimit(intBudget)
                    End If
                End If
            Next

            'First update existing budgets.
            'We update first because adding budgets will invalidate
            'the indices of the budgets we need to update. Actually
            'they may be inserted at the end of the day so the existing
            'budgets on the same day would not be affected, but coding
            'the updates first means we won't break if that behavior
            'changes.
            For intBudget = 1 To mintNUM_BUDGETS
                If Len(mastrBudgetKey(intBudget)) > 0 Then
                    lngUpdateIndex = malngRegIndex(intBudget)
                    If lngUpdateIndex > 0 Then
                        UpdateBudget(lngUpdateIndex, acurNewLimit(intBudget), intBudget)
                        objBudgetTrx = DirectCast(mobjReg.objTrx(lngUpdateIndex), BudgetTrx)
                        ShowBudget(objBudgetTrx)
                        If System.Math.Abs(objBudgetTrx.curBudgetApplied) > System.Math.Abs(objBudgetTrx.curBudgetLimit) Then
                            mblnAdjustedBudgetExceeded = True
                        End If
                        'Use the same budget ending date for new trx.
                        'Assume they all end on the same date.
                        datBudgetEnds = objBudgetTrx.datBudgetEnds
                    End If
                End If
            Next

            'Now add any missing budgets.
            For intBudget = 1 To mintNUM_BUDGETS
                If Len(mastrBudgetKey(intBudget)) > 0 Then
                    If malngRegIndex(intBudget) = 0 Then
                        objBudgetTrx = New BudgetTrx(mobjReg)
                        objBudgetTrx.NewStartBudget(True, datMatch, txtPrefix.Text & mobjBudgets.strKeyToValue1(mastrBudgetKey(intBudget)), "", False, False, 0, "", acurNewLimit(intBudget), datBudgetEnds, mastrBudgetKey(intBudget))
                        mobjReg.NewAddEnd(objBudgetTrx, New LogAdd, "AdjustBudgetsToCashForm.SetBudgets")
                        ShowBudget(objBudgetTrx)
                        'The added budget by definition will be inserted
                        'above the continue point for the budget search,
                        'so increment that to keep it accurate.
                        lngContinueIndex = lngContinueIndex + 1
                    End If
                End If
            Next

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Update the limit of an existing budget, after confirming as a
    '   sanity check that it still has the expected register index.

    Private Sub UpdateBudget(ByVal lngIndex As Integer, ByVal curLimit As Decimal, ByVal intBudget As Integer)

        Dim objTrxManager As BudgetTrxManager

        Try
            objTrxManager = mobjReg.objGetBudgetTrxManager(lngIndex)
            If Not objTrxManager.objTrx Is maobjRegTrx(intBudget) Then
                gRaiseError("Trx is at wrong index")
            End If
            objTrxManager.UpdateStart()
            With DirectCast(objTrxManager.objTrx, BudgetTrx)
                .curBudgetLimit = curLimit
                .SetAmountForBudget(mobjReg.datOldestBudgetEndAllowed)
                .blnFake = False
            End With
            objTrxManager.UpdateEnd(New LogChange, "AdjustBudgetsToCashForm.Update")

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Show a budget transaction in the list of budgets affected by
    '   this form.

    Private Sub ShowBudget(ByVal objBudgetTrx As BudgetTrx)
        Dim objItem As System.Windows.Forms.ListViewItem

        objItem = gobjListViewAdd(lvwResults)
        With objItem
            .Text = gstrFormatDate(objBudgetTrx.datDate)
            If objItem.SubItems.Count > 1 Then
                objItem.SubItems(1).Text = objBudgetTrx.strDescription
            Else
                objItem.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, objBudgetTrx.strDescription))
            End If
            If objItem.SubItems.Count > 2 Then
                objItem.SubItems(2).Text = gstrFormatCurrency(objBudgetTrx.curBudgetLimit)
            Else
                objItem.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, gstrFormatCurrency(objBudgetTrx.curBudgetLimit)))
            End If
            If objItem.SubItems.Count > 3 Then
                objItem.SubItems(3).Text = gstrFormatCurrency(objBudgetTrx.curBudgetApplied)
            Else
                objItem.SubItems.Insert(3, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, gstrFormatCurrency(objBudgetTrx.curBudgetApplied)))
            End If
        End With
    End Sub

    '$Description Called after all budgets have been inserted and adjusted to
    '   double check that the correct minimum balance was achieved.

    Private Sub DoubleCheckBalances(ByVal lngStartIndex As Integer)
        If curLowestBalance(lngStartIndex) <> CDec(txtMinBal.Text) Then
            MsgBox("ERROR: Adjustments did not yield the correct minimum balance.", MsgBoxStyle.Critical)
        End If
    End Sub

    '$Description Find the lowest register balance on or after a specified
    '   register index minus one. Use "minus one" because there are times
    '   when this is called with an index after the end of the register and
    '   in that case what they really want is the register ending balance.

    Private Function curLowestBalance(ByVal lngStartIndex As Integer) As Decimal

        Dim curResult As Decimal
        Dim lngIndex As Integer
        Dim objTrx As Trx

        If lngStartIndex < 1 Then
            curResult = 0
        ElseIf lngStartIndex > mobjReg.lngTrxCount Then
            curResult = mobjReg.objTrx(mobjReg.lngTrxCount).curBalance
        Else
            curResult = mobjReg.objTrx(lngStartIndex).curBalance
        End If

        For lngIndex = lngStartIndex To mobjReg.lngTrxCount
            objTrx = mobjReg.objTrx(lngIndex)
            With objTrx
                If .curBalance < curResult Then
                    curResult = .curBalance
                End If
            End With
        Next
        curLowestBalance = curResult
    End Function

    Private Sub AdjustBudgetsToCashForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Dim intIndex As Short

        Try

            'Save all specifications.
            For intIndex = 1 To mintNUM_BUDGETS
                SaveSetting(gstrREG_APP, strRegSection(), mstrREG_BUDKEY_NAME & intIndex, strGetStringTranslatorKeyFromCombo(cboBudget(intIndex), mobjBudgets))
                SaveSetting(gstrREG_APP, strRegSection(), mstrREG_BUDPCT_NAME & intIndex, txtPercent(intIndex).Text)
            Next
            SaveSetting(gstrREG_APP, strRegSection(), mstrREG_NAMEPRE, txtPrefix.Text)
            SaveSetting(gstrREG_APP, strRegSection(), mstrREG_MINBAL, txtMinBal.Text)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function strGetStringTranslatorKeyFromCombo(ByVal cbo As System.Windows.Forms.ComboBox, ByVal objList As IStringTranslator) As String

        Dim lngItemData As Integer

        If cbo.SelectedIndex = -1 Then
            strGetStringTranslatorKeyFromCombo = ""
        Else
            lngItemData = gintVB6GetItemData(cbo, cbo.SelectedIndex)
            If lngItemData = 0 Then
                strGetStringTranslatorKeyFromCombo = ""
            Else
                strGetStringTranslatorKeyFromCombo = objList.strKey(lngItemData)
            End If
        End If
    End Function
End Class