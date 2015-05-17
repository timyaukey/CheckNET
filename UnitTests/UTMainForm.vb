Option Strict Off
Option Explicit On

Imports CheckBookLib

Friend Class UTMainForm
    Inherits System.Windows.Forms.Form

    Private mobjEverything As Everything

    Private Sub cmdRunBasic_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRunBasic.Click
        On Error GoTo ErrorHandler

        TestFunctions()
        TestLoad()
        TestAddUpdDel()
        TestTransfer()
        TestMatchNormal()
        TestImportUpdateBank()
        TestMatchImportKey()
        TestMatchPayee()
        TestMatchInvoice()
        TestMatchPONumber()
        TestStringTranslator()
        TestCursor()
        TestAgingBrackets()
        TestDateBrackets()
        TestRepeat()
        TestAmountsToWords()
        TestSecurity()

        MsgBox("Basic tests complete.")

        Exit Sub
ErrorHandler:
        MsgBox(Err.Description)
    End Sub

    Private Sub TestFunctions()
        gUTSetTestTitle("Test Functions")

        gUTSetSubTest("gaSplit")

        Dim astrParts() As String

        astrParts = gaSplit("", " ")
        gUTAssert(LBound(astrParts) = 0, "Empty string lbound")
        gUTAssert(UBound(astrParts) = 0, "Empty string ubound")
        gUTAssert(astrParts(0) = "", "Empty string content")

        astrParts = gaSplit("red", " ")
        gUTAssert(LBound(astrParts) = 0, "One string lbound")
        gUTAssert(UBound(astrParts) = 0, "One string ubound")
        gUTAssert(astrParts(0) = "red", "One string content")

        astrParts = gaSplit("red green", " ")
        gUTAssert(LBound(astrParts) = 0, "Two string lbound")
        gUTAssert(UBound(astrParts) = 1, "Two string ubound")
        gUTAssert(astrParts(0) = "red", "Two string content red")
        gUTAssert(astrParts(1) = "green", "Two string content green")

        astrParts = gaSplit("red green blue", " ")
        gUTAssert(LBound(astrParts) = 0, "Three string lbound")
        gUTAssert(UBound(astrParts) = 2, "Three string ubound")
        gUTAssert(astrParts(0) = "red", "Three string content red")
        gUTAssert(astrParts(1) = "green", "Three string content green")
        gUTAssert(astrParts(2) = "blue", "Three string content blue")

        astrParts = gaSplit(" ", " ")
        gUTAssert(LBound(astrParts) = 0, "Blank string lbound")
        gUTAssert(UBound(astrParts) = 1, "Blank string ubound")
        gUTAssert(astrParts(0) = "", "Blank string content 1")
        gUTAssert(astrParts(1) = "", "Blank string content 2")

        astrParts = gaSplit("red  green", " ")
        gUTAssert(LBound(astrParts) = 0, "Double blank string lbound")
        gUTAssert(UBound(astrParts) = 2, "Double blank string ubound")
        gUTAssert(astrParts(0) = "red", "Double blank string content 1")
        gUTAssert(astrParts(1) = "", "Double blank string content 2")
        gUTAssert(astrParts(2) = "green", "Double blank string content 3")

        astrParts = gaSplit(" red  green", " ")
        gUTAssert(LBound(astrParts) = 0, "Triple blank string lbound")
        gUTAssert(UBound(astrParts) = 3, "Triple blank string ubound")
        gUTAssert(astrParts(0) = "", "Triple blank string content 1")
        gUTAssert(astrParts(1) = "red", "Triple blank string content 2")
        gUTAssert(astrParts(2) = "", "Triple blank string content 3")
        gUTAssert(astrParts(3) = "green", "Triple blank string content 4")

        astrParts = gaSplit("red ", " ")
        gUTAssert(LBound(astrParts) = 0, "Trail lbound")
        gUTAssert(UBound(astrParts) = 1, "Trail ubound")
        gUTAssert(astrParts(0) = "red", "Trail content red")
        gUTAssert(astrParts(1) = "", "Trail content blank")

        astrParts = gaSplit("", "12")
        gUTAssert(LBound(astrParts) = 0, "12 Empty string lbound")
        gUTAssert(UBound(astrParts) = 0, "12 Empty string ubound")
        gUTAssert(astrParts(0) = "", "12 Empty string content")

        astrParts = gaSplit("red", "12")
        gUTAssert(LBound(astrParts) = 0, "12 string lbound")
        gUTAssert(UBound(astrParts) = 0, "12 string ubound")
        gUTAssert(astrParts(0) = "red", "12 string content")

        astrParts = gaSplit("12", "12")
        gUTAssert(LBound(astrParts) = 0, "12 sep lbound")
        gUTAssert(UBound(astrParts) = 1, "12 sep ubound")
        gUTAssert(astrParts(0) = "", "12 sep content 1")
        gUTAssert(astrParts(1) = "", "12 sep content 2")

        astrParts = gaSplit("red12green", "12")
        gUTAssert(LBound(astrParts) = 0, "12 Two string lbound")
        gUTAssert(UBound(astrParts) = 1, "12 Two string ubound")
        gUTAssert(astrParts(0) = "red", "12 Two string content red")
        gUTAssert(astrParts(1) = "green", "12 Two string content green")

        astrParts = gaSplit("red1212green", "12")
        gUTAssert(LBound(astrParts) = 0, "12 double string lbound")
        gUTAssert(UBound(astrParts) = 2, "12 double string ubound")
        gUTAssert(astrParts(0) = "red", "12 double string content red")
        gUTAssert(astrParts(1) = "", "12 double string content empty")
        gUTAssert(astrParts(2) = "green", "12 double string content green")
    End Sub

    Private Sub TestLoad()
        Dim objUTReg As UTRegister

        gUTSetTestTitle("Loading register")

        objUTReg = objLoadBuild(0)
        objUTReg.Validate("Check empty register")

        objUTReg = objLoadBuild(1)
        objUTReg.Validate("Check 1 trx", 1)

        objUTReg = objLoadBuild(2)
        objUTReg.Validate("Check 2 trx", 2, 1)

        objUTReg = objLoadBuild(3)
        objUTReg.Validate("Check 3 trx", 2, 1, 3)

        objUTReg = objLoadBuild(4)
        objUTReg.Validate("Check 4 trx", 2, 1, 4, 3)

        objUTReg = objLoadBuild(6)
        objUTReg.Validate("Check 6 trx", 2, 1, 5, 4, 6, 3)

        objUTReg = objLoadBuild(7)
        objUTReg.Validate("Check 7 trx", 2, 1, 7, 5, 4, 6, 3)

        objUTReg = objLoadBuild(8)
        objUTReg.Validate("Check 8 trx", 2, 1, 7, 5, 4, 8, 6, 3)

        objUTReg = objLoadBuild(9)
        objUTReg.Validate("Check 9 trx", 2, 1, 7, 5, 4, 9, 8, 6, 3)

        objUTReg = objLoadBuild(10)
        objUTReg.Validate("Check 10 trx", 2, 1, 7, 5, 4, 10, 9, 8, 6, 3)

        objUTReg = objLoadBuild(11)
        objUTReg.Validate("Check 11 trx", 2, 1, 7, 5, 4, 10, 9, 8, 11, 6, 3)

        objUTReg = objLoadBuild(12)
        objUTReg.Validate("Check 12 trx", 2, 1, 7, 5, 4, 10, 9, 8, 11, 12, 6, 3)

        objUTReg = objLoadBuild(13)
        objUTReg.Validate("Check 13 trx", 2, 1, 7, 5, 4, 10, 9, 8, 11, 12, 13, 6, 3)

        objUTReg = objLoadBuild(14)
        objUTReg.Validate("Check 14 trx", 2, 1, 7, 5, 4, 10, 9, 8, 11, 12, 13, 14, 6, 3)

        objUTReg = objLoadBuild(15)
        objUTReg.Validate("Check 15 trx", 2, 1, 7, 5, 15, 4, 10, 9, 8, 11, 12, 13, 14, 6, 3)

        objUTReg = objLoadBuild(16)
        objUTReg.Validate("Check 16 trx", 2, 1, 7, 5, 15, 4, 10, 9, 8, 11, 12, 13, 14, 16, 6, 3)

        objUTReg = objLoadBuild(17)
        objUTReg.Validate("Check 17 trx", 2, 1, 7, 5, 15, 4, 10, 9, 8, 11, 12, 13, 14, 16, 17, 6, 3)
    End Sub

    Private Function objLoadBuild(ByVal intTrxCount As Short) As UTRegister

        'This Trx sequence tests Register.lngNewInsert() and Register.lngMoveUp()
        'exhaustively.

        'It also tests budget matching and application exhaustively. Tests
        'multiple splits on a normal trx applied to different budgets, splits from
        'multiple normal trx applied to the same budget, budget limits, and
        'debit and credit budgets. Does NOT test un-applying from budget, changing
        'budget, or deleting budget.

        Dim objUTReg As UTRegister
        objUTReg = gobjUTNewReg()
        If intTrxCount >= 1 Then
            objUTReg.LoadNormal("1000", #4/1/2000#, 100.0#)
        End If
        If intTrxCount >= 2 Then
            objUTReg.LoadNormal("1001", #3/1/2000#, 200.0#)
        End If
        If intTrxCount >= 3 Then
            objUTReg.LoadNormal("1002", #5/1/2000#, -50.99)
        End If
        If intTrxCount >= 4 Then
            objUTReg.LoadBudget(#4/10/2000#, -200.0#, #4/20/2000#, "bud1")
        End If
        If intTrxCount >= 5 Then
            'One day before start of budget range, so won't apply to #4.
            objUTReg.LoadNormal("Card", #4/9/2000#, -10.0#, strBudgetKey:="bud1")
        End If
        If intTrxCount >= 6 Then
            'One day after end of budget range, so won't apply to #4.
            objUTReg.LoadNormal("Card", #4/21/2000#, -10.0#, strBudgetKey:="bud1")
        End If
        If intTrxCount >= 7 Then
            '#5 will be applied to this. Is a one day budget date range, so
            'now we've tested before and after the beginning and ending dates.
            objUTReg.LoadBudget(#4/9/2000#, -15.0#, #4/9/2000#, "bud1")
            objUTReg.SetTrxAmount(7, -5.0#)
        End If
        If intTrxCount >= 8 Then
            'Won't apply this until #10 is loaded.
            objUTReg.LoadNormal("1500", #4/11/2000#, -20.0#, strBudgetKey:="bud2")
        End If
        If intTrxCount >= 9 Then
            'Will apply this to the budget in #4.
            objUTReg.LoadNormal("1499", #4/11/2000#, -21.0#, strBudgetKey:="bud1")
            objUTReg.SetTrxAmount(4, -179.0#)
        End If
        If intTrxCount >= 10 Then
            '#8 will be applied to this.
            objUTReg.LoadBudget(#4/10/2000#, -100.0#, #4/20/2000#, "bud2")
            objUTReg.SetTrxAmount(10, -80.0#)
        End If
        If intTrxCount >= 11 Then
            'Cause budget in #10 to be used up exactly.
            'This also tests multiple splits applied to a single budget.
            objUTReg.LoadNormal("1501", #4/13/2000#, -80.0#, strBudgetKey:="bud2")
            objUTReg.SetTrxAmount(10, 0.0#)
        End If
        If intTrxCount >= 12 Then
            'Cause budget in #10 to be exceeded.
            objUTReg.LoadNormal("1502", #4/13/2000#, -2.0#, strBudgetKey:="bud2")
        End If
        If intTrxCount >= 13 Then
            objUTReg.LoadBudget(#4/14/2000#, -50.0#, #4/17/2000#, "bud3")
        End If
        If intTrxCount >= 14 Then
            'Splits applied to different budgets.
            objUTReg.LoadNormal("1503", #4/15/2000#, -10.0#, strBudgetKey:="bud1", vcurAmount2:=-7.3, strBudgetKey2:="bud3")
            objUTReg.SetTrxAmount(4, -169.0#)
            objUTReg.SetTrxAmount(13, -42.7)
        End If
        If intTrxCount >= 15 Then
            'Credit budget.
            objUTReg.LoadBudget(#4/10/2000#, 100.0#, #4/20/2000#, "bud4")
        End If
        If intTrxCount >= 16 Then
            'Apply to credit budget.
            objUTReg.LoadNormal("DEP", #4/19/2000#, 0.21, strBudgetKey:="bud4")
            objUTReg.SetTrxAmount(15, 99.79)
        End If
        If intTrxCount >= 17 Then
            'Exceed credit budget.
            objUTReg.LoadNormal("DEP", #4/19/2000#, 120.0#, strBudgetKey:="bud4")
            objUTReg.SetTrxAmount(15, 0.0#)
        End If
        objUTReg.objReg.LoadPostProcessing()
        objLoadBuild = objUTReg

    End Function

    Private Sub TestAddUpdDel()
        Dim objUTReg As UTRegister
        Dim objTrx As Trx

        gUTSetTestTitle("Adding to and updating register")

        objUTReg = gobjUTNewReg()
        With objUTReg

            'First we test sort order and balances (no budgets) during adds.

            gUTSetSubTest("Test add for sort order")

            .AddNormal("1500", #6/1/2000#, -50.75, "First add", 1, 1, 1)
            .Validate("", 1)

            .AddNormal("1501", #6/1/2000#, -24.95, "Second add", 2, 2, 2)
            .Validate("", 1, 2)
            gUTAssert(.strBudgetsChanged = "", "Expected no budgets to change")

            .AddNormal("1502", #6/1/2000#, -20.0#, "Third add", 3, 3, 3)
            .Validate("", 1, 2, 3)

            .AddNormal("DEP", #6/1/2000#, 400.0#, "Fourth add", 1, 1, 4)
            .Validate("", 4, 1, 2, 3)

            .AddNormal("1499", #6/1/2000#, -10.0#, "Fifth", 2, 2, 5)
            .Validate("", 4, 5, 1, 2, 3)

            'Next we test sort order and balances (no budgets) during updates.

            gUTSetSubTest("Test update for sort order")

            'This tests Register.lngMoveDown() exhaustively.
            'Register.lngMoveUp() was completely tested in register loading.

            'Move first register entry up and change amount.
            'No change to position, but all balances change.
            .SetTrxAmount(4, 399.0#)
            .SetTrxNumber(4, "DP2")
            .SetTrxDate(4, #5/1/2000#)
            .UpdateNormal("DP2", #5/1/2000#, 399.0#, "First upd", 1, 1, 1, 5)
            .Validate("", 4, 5, 1, 2, 3)

            'Move last register entry down.
            'No change to position.
            .SetTrxDate(3, #6/15/2000#)
            .UpdateNormal("1502", #6/15/2000#, -20.0#, "Second upd", 5, 5, 5, 5)
            .Validate("", 4, 5, 1, 2, 3)

            'Move middle register entry down one line.
            .SetTrxDate(1, #6/5/2000#)
            .SetTrxAmount(1, -49.0#)
            .UpdateNormal("1500", #6/5/2000#, -49.0#, "Third upd", 3, 4, 3, 5)
            .Validate("", 4, 5, 2, 1, 3)

            'Move middle register entry to end of registry.
            .SetTrxDate(2, #6/16/2000#)
            .UpdateNormal("1501", #6/16/2000#, -24.95, "Fourth upd", 3, 5, 3, 5)
            .Validate("", 4, 5, 1, 3, 2)

            'Move second register entry down two lines.
            .SetTrxDate(5, #6/15/2000#)
            .SetTrxNumber(5, "1503")
            .UpdateNormal("1503", #6/15/2000#, -10.0#, "Fifth upd", 2, 4, 2, 4)
            .Validate("", 4, 1, 3, 5, 2)

            'Move first register entry to end of registry.
            .SetTrxDate(4, #7/1/2000#)
            .UpdateNormal("DP2", #7/1/2000#, 399.0#, "Sixth upd", 1, 5, 1, 5)
            .Validate("", 1, 3, 5, 2, 4)

            'Make change that doesn't affect sort order.
            .SetTrxAmount(3, -21.0#)
            .UpdateNormal("1502", #6/15/2000#, -21.0#, "Seventh upd", 2, 2, 2, 5)
            .Validate("", 1, 3, 5, 2, 4)

            'Now test budgets.

            gUTSetSubTest("Test add for budgets")

            .AddBudget(#6/16/2000#, -50.0#, #6/20/2000#, "bud1", "First add", 4, 4, 6)
            .Validate("", 1, 3, 5, 6, 2, 4)
            gUTAssert(.strBudgetsChanged = "", "Did not expect budgets to change")

            .SetTrxAmount(6, -46.0#)
            .AddNormal("Card", #6/17/2000#, -4.0#, "Second add", 6, 4, 6, strBudgetKey:="bud1")
            .Validate("", 1, 3, 5, 6, 2, 7, 4)
            gUTAssert(.strBudgetsChanged = ",4", "Expected budgets to change")

            gUTSetSubTest("Test update for budgets")

            'Add a second budget trx
            .AddBudget(#6/17/2000#, -100.0#, #6/17/2000#, "bud2", "First add", 6, 6, 8)
            .Validate("", 1, 3, 5, 6, 2, 8, 7, 4)

            'Update the normal trx currently applied to the first budget,
            'and make it split between the first and second budgets instead.
            'Note that this does not change the register ending balance, because
            'even though we change the trx amount both the new and old amounts
            'come out of budgets (though different budgets) so it nets out the
            'same in the end.
            .SetTrxAmount(6, -28.0#)
            .SetTrxAmount(8, -82.5)
            .SetTrxAmount(7, -39.5)
            .UpdateNormal("Card", #6/17/2000#, -22.0#, "Second add", 7, 7, 4, 7, strBudgetKey:="bud1", vcurAmount2:=-17.5, strBudgetKey2:="bud2")
            .Validate("", 1, 3, 5, 6, 2, 8, 7, 4)
            gUTAssert(.strBudgetsChanged = ",4,4,6", "Expected 3 budget changes")

            'Reduce the limit of the first budget to cause it to be exhausted.
            .SetTrxAmount(6, 0.0#)
            .UpdateBudget(#6/16/2000#, -21.94, #6/20/2000#, "bud1", "Third update", 4, 4, 4, 8)
            .Validate("", 1, 3, 5, 6, 2, 8, 7, 4)

            gUTSetSubTest("Test delete")

            'Delete from either end and the middle.
            .DeleteEntry(1, 1, 7, "First delete")
            .Validate("", 3, 5, 6, 2, 8, 7, 4)

            .DeleteEntry(7, 0, 0, "Second delete")
            .Validate("", 3, 5, 6, 2, 8, 7)

            .DeleteEntry(4, 4, 5, "Third delete")
            .Validate("", 3, 5, 6, 8, 7)

            'Delete one of the budgets, to show the applied splits are un-applied.
            objTrx = objUTReg.objReg.objTrx(5)
            gUTAssert(Not objTrx.objSplit(2).objBudget Is Nothing, "Expected split to be applied")
            .DeleteEntry(4, 4, 4, "Fourth delete")
            .Validate("", 3, 5, 6, 7)
            gUTAssert(objTrx.objSplit(2).objBudget Is Nothing, "Expected split to be un-applied")

            'Delete a normal trx applied to a budget, to show it un-applies.
            .SetTrxAmount(6, -21.94)
            .DeleteEntry(4, 3, 3, "Fifth delete")
            .Validate("", 3, 5, 6)

            'Delete the remaining trx.
            .DeleteEntry(3, 0, 0, "Sixth delete")
            .Validate("", 3, 5)

            .DeleteEntry(1, 1, 1, "Seventh delete")
            .Validate("", 5)

            .DeleteEntry(1, 0, 0, "Eighth delete")
            .Validate("")

        End With
    End Sub

    Private Sub TestTransfer()
        Dim objUTReg1 As UTRegister
        Dim objUTReg2 As UTRegister
        Dim objXfr As TransferManager

        gUTSetTestTitle("Test transfers")

        objXfr = New TransferManager

        gUTSetSubTest("Init register 1")

        objUTReg1 = gobjUTNewReg(strRegisterKey:="reg1")
        With objUTReg1
            .AddNormal("DEP", #4/1/2000#, 2000.0#, "Add1", 1, 1, 1)
            .AddNormal("100", #4/3/2000#, -25.0#, "Add2", 2, 2, 2)
            .AddNormal("101", #4/5/2000#, -37.0#, "Add3", 3, 3, 3)
            .AddNormal("102", #4/7/2000#, -45.3, "Add4", 4, 4, 4)
            .Validate("", 1, 2, 3, 4)
        End With

        gUTSetSubTest("Init register 2")

        objUTReg2 = gobjUTNewReg(strRegisterKey:="reg2")
        With objUTReg2
            .AddNormal("DEP", #4/1/2000#, 3000.0#, "Add1", 1, 1, 1)
            .AddNormal("200", #4/3/2000#, -25.0#, "Add2", 2, 2, 2)
            .AddNormal("201", #4/15/2000#, -37.0#, "Add3", 3, 3, 3)
            .AddNormal("202", #4/17/2000#, -45.3, "Add4", 4, 4, 4)
            .Validate("", 1, 2, 3, 4)
        End With

        gUTSetSubTest("Create transfer")

        objXfr.AddTransfer(objUTReg1.objReg, objUTReg2.objReg, #4/4/2000#, "xfer1", "", False, 100.39, "", False, False, 0)

        objUTReg1.AddTrx(objUTReg1.objReg.objTrx(3))
        objUTReg1.Validate("Transfer added to 1", 1, 2, 5, 3, 4)

        objUTReg2.AddTrx(objUTReg2.objReg.objTrx(3))
        objUTReg2.Validate("Transfer added to 2", 1, 2, 5, 3, 4)

        gUTSetSubTest("Update transfer")

        objXfr.UpdateTransfer(objUTReg1.objReg, 3, objUTReg2.objReg, #4/6/2000#, "xfer1", "", False, 29.95, "", False, False, 0)

        objUTReg1.SetTrxAmount(5, 29.95)
        objUTReg1.SetTrxDate(5, #4/6/2000#)
        objUTReg1.Validate("Transfer changed 1", 1, 2, 3, 5, 4)

        objUTReg2.SetTrxAmount(5, -29.95)
        objUTReg2.SetTrxDate(5, #4/6/2000#)
        objUTReg2.Validate("Transfer changed 2", 1, 2, 5, 3, 4)

        gUTSetSubTest("Delete transfer")

        objXfr.DeleteTransfer(objUTReg1.objReg, 4, objUTReg2.objReg)
        objUTReg1.Validate("Transfer deleted 1", 1, 2, 3, 4)
        objUTReg2.Validate("Transfer deleted 2", 1, 2, 3, 4)

        objXfr = New TransferManager

        gUTSetSubTest("Init register 1 (repeat)")

        objUTReg1 = gobjUTNewReg(strRegisterKey:="reg1")
        With objUTReg1
            .AddNormal("DEP", #4/1/2000#, 2000.0#, "Add1", 1, 1, 1)
            .AddNormal("100", #4/3/2000#, -25.0#, "Add2", 2, 2, 2)
            .AddNormal("101", #4/5/2000#, -37.0#, "Add3", 3, 3, 3, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("102", #4/7/2000#, -45.3, "Add4", 4, 4, 4)
            .Validate("", 1, 2, 3, 4)
        End With

        gUTSetSubTest("Init register 2 (repeat)")

        objUTReg2 = gobjUTNewReg(strRegisterKey:="reg2")
        With objUTReg2
            .AddNormal("DEP", #4/1/2000#, 3000.0#, "Add1", 1, 1, 1)
            .AddNormal("200", #4/3/2000#, -25.0#, "Add2", 2, 2, 2)
            .AddNormal("201", #4/15/2000#, -37.0#, "Add3", 3, 3, 3)
            .AddNormal("202", #4/17/2000#, -45.3, "Add4", 4, 4, 4)
            .Validate("", 1, 2, 3, 4)
        End With

        gUTSetSubTest("Create transfer (repeat)")

        objXfr.AddTransfer(objUTReg1.objReg, objUTReg2.objReg, #4/4/2000#, "xfer1", "", False, 100.39, "r2", False, False, 2)

        objUTReg1.AddTrx(objUTReg1.objReg.objTrx(3))
        objUTReg1.Validate("Transfer added to 1", 1, 2, 5, 3, 4)
        gUTAssert(objUTReg1.objReg.colDbgRepeatTrx.Count() = 2, "")

        objUTReg2.AddTrx(objUTReg2.objReg.objTrx(3))
        objUTReg2.Validate("Transfer added to 2", 1, 2, 5, 3, 4)

        gUTSetSubTest("Update transfer (repeat)")

        objXfr.UpdateTransfer(objUTReg1.objReg, 3, objUTReg2.objReg, #4/6/2000#, "xfer1", "", False, 29.95, "r3", False, False, 3)

        objUTReg1.SetTrxAmount(5, 29.95)
        objUTReg1.SetTrxDate(5, #4/6/2000#)
        objUTReg1.SetTrxRepeatKey(5, "r3")
        objUTReg1.SetTrxRepeatSeq(5, 3)
        objUTReg1.Validate("Transfer changed 1", 1, 2, 3, 5, 4)
        gUTAssert(objUTReg1.objReg.colDbgRepeatTrx.Count() = 2, "")

        objUTReg2.SetTrxAmount(5, -29.95)
        objUTReg2.SetTrxDate(5, #4/6/2000#)
        objUTReg2.SetTrxRepeatKey(5, "r3")
        objUTReg2.SetTrxRepeatSeq(5, 3)
        objUTReg2.Validate("Transfer changed 2", 1, 2, 5, 3, 4)
        gUTAssert(objUTReg1.objReg.colDbgRepeatTrx.Count() = 2, "")

        gUTSetSubTest("Delete transfer (repeat)")

        objXfr.DeleteTransfer(objUTReg1.objReg, 4, objUTReg2.objReg)
        objUTReg1.Validate("Transfer deleted 1", 1, 2, 3, 4)
        objUTReg2.Validate("Transfer deleted 2", 1, 2, 3, 4)
        gUTAssert(objUTReg1.objReg.colDbgRepeatTrx.Count() = 1, "")

    End Sub

    Private Sub TestMatchNormal()
        Dim objUTReg As UTRegister
        Dim colMatches As Collection
        Dim blnExactMatch As Boolean

        gUTSetTestTitle("Test MatchNormal")

        gUTSetSubTest("Init register")

        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("DEP", #4/1/2000#, 2000.0#, "Add1", 1, 1, 1, strDescription:="Descr1")
            .AddNormal("100", #4/3/2000#, -25.0#, "Add2", 2, 2, 2, strDescription:="Descr2")
            .AddNormal("101", #4/5/2000#, -37.0#, "Add3", 3, 3, 3, strDescription:="Descr3", curNormalMatchRange:=1.0#)
            .AddNormal("Pmt", #4/5/2000#, -37.0#, "Add4", 4, 4, 4, strDescription:="Descr4")
            .AddNormal("102", #4/12/2000#, -45.3, "Add4", 5, 5, 5, strDescription:="Descr5")
            .AddNormal("Pmt", #4/18/2000#, -100.0#, "Add5", 6, 6, 6, strDescription:="Descr6")
        End With

        gUTSetSubTest("Search register")

        With objUTReg.objReg
            .MatchNormal(101, #4/1/2000#, 20, "", 555.0#, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";3", "Didn't find 101 (number only match)")
            'Verify date range filtering. Number, amount and descr always match,
            'so date filter is the only way to fail.
            'Date out of range before.
            .MatchNormal(100, #4/2/2000#, 0, "Descr2", -25.0#, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Found 100 when out of date range before")
            'Date in range before.
            .MatchNormal(100, #4/2/2000#, 1, "Descr2", -25.0#, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";2", "Expected to find 100-A")
            'Date out of range after.
            .MatchNormal(100, #4/4/2000#, 0, "Descr2", -25.0#, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Found 100 when out of date range-B")
            'Date in range after.
            .MatchNormal(100, #4/4/2000#, 1, "Descr2", -25.0#, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";2", "Expected to find 100-B")
            'End of date range filter checking.
            'Look for 101 without using trx number.
            'Two trx match that date and amount, because one of them
            'has an amount match range=1, but one is an exact match so only that is returned.
            .MatchNormal(0, #4/5/2000#, 10, "Descr3", -37.0#, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";3", "Expected to find 101-A")
            'Match all 3 properties.
            .MatchNormal(0, #4/5/2000#, 10, "Descr3", -38.0#, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";3", "Expected to find 101-B")
            'All the ways of matching 2 of 3 properties.
            'Descr+amount
            .MatchNormal(0, #4/4/2000#, 10, "Descr3", -38.0#, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";3", "Expected to find 101-D")
            'Date+amount, date+descr
            .MatchNormal(0, #4/5/2000#, 10, "Descr2", -38.0#, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";2;3", "Expected to find 100,101-E")
            'Date+descr
            .MatchNormal(0, #4/5/2000#, 10, "Descr3", -40.0#, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";3", "Expected to find 101-F")
            'Date only, so fail.
            .MatchNormal(0, #4/5/2000#, 0, "Descr2", -40.0#, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Did not expect to find 101-G")
            'Date only, so succeed in loose search.
            .MatchNormal(0, #4/5/2000#, 10, "Descr2", -40.0#, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";1;2;3;4;5", "Did not expect to find 101-H")
            'Close date only, so succeed in loose search.
            .MatchNormal(0, #4/16/2000#, 10, "zzzz", -40000.0#, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";5;6", "Expected to find Pmt-I")
            'Close date only, so succeed in loose search.
            .MatchNormal(0, #4/20/2000#, 10, "zzzz", -40000.0#, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";6", "Expected to find Pmt-J1")
            'Close date only, so fail in strict search.
            .MatchNormal(0, #4/20/2000#, 10, "zzzz", -40000.0#, False, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Did not expect to find anything-J2")
            'Just outside date, so fail in loose search.
            .MatchNormal(0, #4/28/2000#, 10, "zzzz", -40000.0#, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Did not expect to find anything-K")
            'Just outside date, so fail in loose search.
            .MatchNormal(0, #3/20/2000#, 10, "zzzz", -40000.0#, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Did not expect to find anything-L")
            'Amount within 20%.
            .MatchNormal(0, #4/24/2000#, 10, "zzzz", -125.0#, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";6", "Expected to find Pmt-M1")
            'Amount within 20%.
            .MatchNormal(0, #4/24/2000#, 10, "zzzz", -84.0#, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = ";6", "Expected to find Pmt-M2")
            'Amount not within 20%.
            .MatchNormal(0, #4/24/2000#, 10, "zzzz", -126.0#, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Did not expect to find anything-O1")
            'Amount not within 20%.
            .MatchNormal(0, #4/24/2000#, 10, "zzzz", -82.0#, True, colMatches, blnExactMatch)
            gUTAssert(strConcatMatchResults(colMatches) = "", "Did not expect to find anything-O2")
        End With

    End Sub

    Private Function strConcatMatchResults(ByVal colMatches As Collection) As String
        Dim vntElement As Object
        Dim strResult As String
        For Each vntElement In colMatches
            'UPGRADE_WARNING: Couldn't resolve default property of object vntElement. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            strResult = strResult & ";" & vntElement
        Next vntElement
        strConcatMatchResults = strResult
    End Function

    Private Sub TestImportUpdateBank()
        Dim objTrx As Trx
        Dim objUTReg As UTRegister

        gUTSetTestTitle("Test ImportUpdateBank")

        gUTSetSubTest("Init register")

        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("100", #4/3/2000#, -25.0#, "Add1", 1, 1, 1, strDescription:="Descr1", blnFake:=True, blnAutoGenerated:=True)
            .AddNormal("101", #4/4/2000#, -30.2, "Add2", 2, 2, 2, strDescription:="Descr2", vcurAmount2:=-10.0#)
            .AddNormal("102", #4/5/2000#, -20.99, "Add3", 3, 3, 3, strDescription:="Descr3")
        End With

        gUTSetSubTest("Test first import")

        objUTReg.objReg.ImportUpdateBank(1, "200", False, -25.0#, "importkey-1")
        objTrx = objUTReg.objReg.objTrx(1)
        With objTrx
            gUTAssert(.strNumber = "200", "Bad number")
            gUTAssert(.curAmount = -25.0#, "Bad amount")
            gUTAssert(.strImportKey = "importkey-1", "Bad import key")
            gUTAssert(.blnFake = False, "Bad blnFake")
            gUTAssert(.blnAutoGenerated = False, "Bad auto generated")
        End With

        gUTSetSubTest("Test second import")

        objUTReg.objReg.ImportUpdateBank(2, "201", False, -50.0#, "importkey-2")
        objTrx = objUTReg.objReg.objTrx(2)
        With objTrx
            gUTAssert(.strNumber = "201", "Bad number")
            gUTAssert(.curAmount = -50.0#, "Bad amount " & .curAmount)
            gUTAssert(.blnFake = False, "Bad blnFake")
            gUTAssert(.blnAutoGenerated = False, "Bad auto generated")
        End With

        gUTSetSubTest("Test third import")

        objUTReg.objReg.ImportUpdateBank(3, "102", False, -40.01, "importkey-3")
        objTrx = objUTReg.objReg.objTrx(3)
        With objTrx
            gUTAssert(.strNumber = "102", "Bad number")
            gUTAssert(.curAmount = -40.01, "Bad amount " & .curAmount)
        End With

    End Sub

    Private Sub TestMatchImportKey()
        Dim objUTReg As UTRegister
        Dim lngMatchIndex As Integer

        gUTSetTestTitle("Test MatchImportKey")

        gUTSetSubTest("Init register")

        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("100", #4/1/2000#, -2000.0#, "Add1", 1, 1, 1, strImportKey:="imp1")
            .AddNormal("101", #4/3/2000#, -25.0#, "Add2", 2, 2, 2, strImportKey:="imp2", blnFake:=True)
            .AddNormal("102", #4/3/2000#, -26.0#, "Add2", 3, 3, 3)
            .AddNormal("103", #4/3/2000#, -27.0#, "Add2", 4, 4, 4, strImportKey:="imp4")
            lngMatchIndex = .objReg.lngMatchImportKey("imp1")
            gUTAssert(lngMatchIndex = 1, "Did not find 100")
            lngMatchIndex = .objReg.lngMatchImportKey("imp2")
            gUTAssert(lngMatchIndex = 0, "Did not expect to find 101")
            lngMatchIndex = .objReg.lngMatchImportKey("imp4")
            gUTAssert(lngMatchIndex = 4, "Did not find 103")
        End With

    End Sub

    Private Sub TestMatchPayee()
        Dim objUTReg As UTRegister
        Dim colMatches As Collection
        Dim blnExactMatch As Boolean
        Dim objTrx As Trx

        gUTSetTestTitle("Test MatchPayee")

        gUTSetSubTest("Init register")

        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("100", #4/1/2000#, -2000.0#, "Add1", 1, 1, 1, strDescription:="payee1")
            .AddNormal("101", #4/3/2000#, -25.0#, "Add2", 2, 2, 2, strDescription:="company2")
            .AddNormal("102", #4/5/2000#, -26.0#, "Add2", 3, 3, 3, strDescription:="payee1")
            .AddNormal("103", #4/7/2000#, -27.0#, "Add2", 4, 4, 4, strDescription:="payee1")

            .objReg.MatchPayee(#4/3/2000#, 1, "company2", True, colMatches, blnExactMatch)
            gUTAssert(colMatches.Count() = 1, "company2 fail")
            'UPGRADE_WARNING: Couldn't resolve default property of object colMatches(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            objTrx = .objReg.objTrx(colMatches.Item(1))
            'UPGRADE_WARNING: Couldn't resolve default property of object colMatches(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            gUTAssert(colMatches.Item(1) = 2, "company2 index fail")
            gUTAssert(objTrx.strDescription = "company2", "company2 name fail")
            gUTAssert(objTrx.datDate = #4/3/2000#, "company2 date fail")
            gUTAssert(blnExactMatch = True, "company2 exact fail")

            .objReg.MatchPayee(#4/6/2000#, 1, "payee1", True, colMatches, blnExactMatch)
            gUTAssert(colMatches.Count() = 2, "payee1 fail")
            'UPGRADE_WARNING: Couldn't resolve default property of object colMatches(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            objTrx = .objReg.objTrx(colMatches.Item(1))
            'UPGRADE_WARNING: Couldn't resolve default property of object colMatches(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            gUTAssert(colMatches.Item(1) = 3, "payee1#1 index fail")
            'UPGRADE_WARNING: Couldn't resolve default property of object colMatches(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            gUTAssert(colMatches.Item(2) = 4, "payee1#2 index fail")
            gUTAssert(blnExactMatch = False, "payee#1 exact fail")
        End With

    End Sub

    Private Sub TestMatchInvoice()
        Dim objUTReg As UTRegister
        Dim colMatches As Collection

        gUTSetTestTitle("Test MatchInvoice")

        gUTSetSubTest("Init register")

        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("100", #4/1/2000#, -2000.0#, "Add1", 1, 1, 1, strDescription:="payee1", strInvoiceNum:="I1000")
            .AddNormal("101", #4/3/2000#, -25.0#, "Add2", 2, 2, 2, strDescription:="company2", strInvoiceNum:="I1000", vcurAmount2:=-10.0#, strInvoiceNum2:="I1001")
            .AddNormal("102", #4/5/2000#, -26.0#, "Add2", 3, 3, 3, strDescription:="payee1", strInvoiceNum:="I1001")
            .AddNormal("103", #4/7/2000#, -27.0#, "Add2", 4, 4, 4, strDescription:="payee1", strInvoiceNum:="I1002")

            .objReg.MatchInvoice(#4/3/2000#, 10, "company2", "I1000", colMatches)
            gUTAssert(colMatches.Count() = 1, "company2 I1000 fail")
            'UPGRADE_WARNING: Couldn't resolve default property of object colMatches(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            gUTAssert(colMatches.Item(1) = 2, "company2 I1000 index fail")

            .objReg.MatchInvoice(#4/3/2000#, 10, "company2", "I1001", colMatches)
            gUTAssert(colMatches.Count() = 1, "company2 I1001 fail")
            'UPGRADE_WARNING: Couldn't resolve default property of object colMatches(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            gUTAssert(colMatches.Item(1) = 2, "company2 I1001 index fail")

            .objReg.MatchInvoice(#4/5/2000#, 1, "company2", "I1000", colMatches)
            gUTAssert(colMatches.Count() = 0, "company2 I1000 -2 fail")

            .objReg.MatchInvoice(#4/5/2000#, 3, "company3", "I1000", colMatches)
            gUTAssert(colMatches.Count() = 0, "company3 I1000 fail")

            .objReg.MatchInvoice(#4/5/2000#, 3, "company2", "I1000", colMatches)
            gUTAssert(colMatches.Count() = 1, "company2 I1000 -3 fail")
        End With

    End Sub

    Private Sub TestMatchPONumber()
        Dim objUTReg As UTRegister
        Dim colMatches As Collection

        gUTSetTestTitle("Test MatchPONumber")

        gUTSetSubTest("Init register")

        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("100", #4/1/2000#, -2000.0#, "Add1", 1, 1, 1, strDescription:="payee1", strPONumber:="P1")
            .AddNormal("101", #4/3/2000#, -25.0#, "Add2", 2, 2, 2, strDescription:="company2", strPONumber:="P1", vcurAmount2:=-10.0#, strPONumber2:="P2")
            .AddNormal("102", #4/5/2000#, -26.0#, "Add2", 3, 3, 3, strDescription:="payee1", strPONumber:="P2")
            .AddNormal("103", #4/7/2000#, -27.0#, "Add2", 4, 4, 4, strDescription:="payee1", strPONumber:="P3")

            .objReg.MatchPONumber(#4/3/2000#, 10, "company2", "P1", colMatches)
            gUTAssert(colMatches.Count() = 1, "company2 P1 fail")
            'UPGRADE_WARNING: Couldn't resolve default property of object colMatches(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            gUTAssert(colMatches.Item(1) = 2, "company2 P1 index fail")

            .objReg.MatchPONumber(#4/3/2000#, 10, "company2", "P2", colMatches)
            gUTAssert(colMatches.Count() = 1, "company2 P2 fail")
            'UPGRADE_WARNING: Couldn't resolve default property of object colMatches(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            gUTAssert(colMatches.Item(1) = 2, "company2 I1001 index fail")

            .objReg.MatchPONumber(#4/5/2000#, 1, "company2", "P1", colMatches)
            gUTAssert(colMatches.Count() = 0, "company2 P1 -2 fail")

            .objReg.MatchPONumber(#4/5/2000#, 3, "company3", "P1", colMatches)
            gUTAssert(colMatches.Count() = 0, "company3 P1 fail")

            .objReg.MatchPONumber(#4/5/2000#, 3, "company2", "P1", colMatches)
            gUTAssert(colMatches.Count() = 1, "company2 P1 -3 fail")
        End With

    End Sub

    Private Sub cmdRunFileLoad_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRunFileLoad.Click
        On Error GoTo ErrorHandler

        TestFileLoad1()

        MsgBox("File loading tests complete.")

        Exit Sub
ErrorHandler:
        MsgBox(Err.Description)
    End Sub

    Private Sub TestFileLoad1()
        Dim objReg As Register
        Dim objTrx As Trx
        Dim objSplit As Split_Renamed

        gUTSetTestTitle("TestFileLoad1")

        gUTSetSubTest("Load")
        objReg = objLoadFile("UTRegLoad1.txt")
        If objReg.lngTrxCount <> 4 Then
            Exit Sub
        End If

        gUTSetSubTest("Verify 1")

        objTrx = objReg.objTrx(1)
        With objTrx
            gUTAssert(.lngType = Trx.TrxType.glngTRXTYP_BUDGET, "Wrong type")
            gUTAssert(.datDate = #4/10/2000#, "Wrong date")
            gUTAssert(.strDescription = "General household", "Wrong description")
            gUTAssert(.curAmount = (-150.0# + 10.99), "Wrong amount")
            gUTAssert(.datBudgetEnds = #4/16/2000#, "Wrong budget end date")
            gUTAssert(.strBudgetKey = "bud1", "Wrong budget key")
            gUTAssert(.curBudgetLimit = -150.0#, "Wrong budget limit")
        End With

        gUTSetSubTest("Verify 2")

        objTrx = objReg.objTrx(2)
        With objTrx
            gUTAssert(.lngType = Trx.TrxType.glngTRXTYP_NORMAL, "Wrong type")
            gUTAssert(.datDate = #4/13/2000#, "Wrong date")
            gUTAssert(.strDescription = "Hadley Garden Center", "Wrong description")
            gUTAssert(.strMemo = "Bird seed", "Wrong memo")
            gUTAssert(.curAmount = (-24.95 - 10.99), "Wrong amount")
            gUTAssert(.lngSplits = 2, "Wrong numbe of splits")
            objSplit = .objSplit(1)
            gUTAssert(objSplit.curAmount = -24.95, "Wrong split1 amount")
            gUTAssert(objSplit.strBudgetKey = "", "Wrong split1 budget key")
            gUTAssert(objSplit.strCategoryKey = "cat1", "Wrong split1 category key")
            objSplit = .objSplit(2)
            gUTAssert(objSplit.curAmount = -10.99, "Wrong split2 amount")
            gUTAssert(objSplit.strCategoryKey = "cat2", "Wrong split2 category key")
            gUTAssert(objSplit.strMemo = "sunflower", "Wrong split2 memo")
        End With

        gUTSetSubTest("Verify 3")

        objTrx = objReg.objTrx(3)
        With objTrx
            gUTAssert(.lngType = Trx.TrxType.glngTRXTYP_NORMAL, "Wrong type")
            gUTAssert(.datDate = #4/15/2000#, "Wrong date")
            gUTAssert(.strNumber = "1001", "Wrong number")
            gUTAssert(.strImportKey = "imp1", "Wrong import key")
            gUTAssert(.strRepeatKey = "rep1", "Wrong repeat key")
            gUTAssert(.curNormalMatchRange = 22.01, "Wrong match range")
        End With

        gUTSetSubTest("Verify 4")

        objTrx = objReg.objTrx(4)
        With objTrx
            gUTAssert(.lngType = Trx.TrxType.glngTRXTYP_TRANSFER, "Wrong type")
            gUTAssert(.datDate = #4/20/2000#, "Wrong date")
            gUTAssert(.strTransferKey = "xfr55", "Wrong transfer key")
            gUTAssert(.curAmount = 140.01, "Wrong transfer amount")
        End With

    End Sub

    Private Function objLoadFile(ByVal strFileName As String) As Register
        Dim objLoader As RegisterLoader
        'Dim intFile As Short
        Dim objReg As Register
        Dim objRegRpt As Register
        Dim lngLinesRead As Integer

        objLoader = New RegisterLoader
        objReg = New Register
        objReg.Init("Regular", "reg", False, False, 3, System.DateTime.FromOADate(0), False)
        objRegRpt = New Register
        objRegRpt.Init("Repeat", "rpt", False, False, 3, System.DateTime.FromOADate(0), True)
        'intFile = FreeFile
        'FileOpen(intFile, gstrAddPath("UTData\" & strFileName), OpenMode.Input)
        lngLinesRead = 0
        objLoader.LoadFileUT(objReg, gstrAddPath("UTData\" & strFileName), False, #1/1/1980#, lngLinesRead)
        'FileClose(intFile)
        objReg.LoadPostProcessing()
        objLoadFile = objReg
    End Function

    Private Sub TestStringTranslator()
        Dim objString As StringTranslator

        gUTSetTestTitle("TestStringTranslator")

        gUTSetSubTest("Load")

        objString = New StringTranslator
        objString.LoadFile(gstrAddPath("UTData\UTStringTran1.txt"))

        gUTSetSubTest("Verify")

        gUTAssert(objString.intElements = 4, "Wrong number of elements")
        gUTAssert(objString.intLookupKey("asdf") = 0, "Did not expect to find asdf")
        gUTAssert(objString.intLookupKey("v1") = 1, "Did not found v1")
        gUTAssert(objString.intLookupKey("2") = 2, "Did not find 2")
        gUTAssert(objString.intLookupKey("third") = 3, "Did not find third")
        gUTAssert(objString.intLookupKey("Last") = 4, "Did not find last")
        gUTAssert(objString.strKey(2) = "2", "Wrong strKey(2)")
        gUTAssert(objString.strValue1(1) = "value1", "Wrong strValue1(1)")
        gUTAssert(objString.strValue2(3) = "THIRD line.", "Wrong strValue2(3)")

    End Sub

    Private Sub TestCursor()

        gUTSetTestTitle("TestCursor")

        Dim objUTReg As UTRegister
        Dim objReg As Register
        Dim objTrx As Trx
        Dim objCursor As RegCursor
        Dim objCursor2 As RegCursor

        gUTSetSubTest("Empty Register")

        objUTReg = gobjUTNewReg()
        objReg = objUTReg.objReg

        gUTAssert(objReg.lngTrxCount = 0, "Expected to be empty")
        objCursor = objReg.objGetCursor()

        gUTAssert(objCursor.blnHasCurrent = False, "Initial blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "Initial objCurrent not Nothing")
        gUTAssert(objCursor.objGetNext() Is Nothing, "Initial objGetNext() not Nothing")
        gUTAssert(objCursor.objGetPrev() Is Nothing, "Initial objGetPrev() not Nothing")

        objCursor.MoveBeforeFirst()
        gUTAssert(objCursor.blnHasCurrent = False, "MoveBeforeFirst set blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After MoveBeforeFirst objCurrent not Nothing")

        objCursor.MoveAfterLast()
        gUTAssert(objCursor.blnHasCurrent = False, "MoveAfterLast set blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After MoveAfterLast objCurrent not Nothing")

        objCursor.MovePrev()
        gUTAssert(objCursor.blnHasCurrent = False, "After 1 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 1 MovePrev objCurrent not Nothing")
        objCursor.MovePrev()
        gUTAssert(objCursor.blnHasCurrent = False, "After 2 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 2 MovePrev objCurrent not Nothing")
        objCursor.MovePrev()
        gUTAssert(objCursor.blnHasCurrent = False, "After 3 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 3 MovePrev objCurrent not Nothing")

        objCursor.MoveNext()
        gUTAssert(objCursor.blnHasCurrent = False, "After 1 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 1 MovePrev objCurrent not Nothing")
        objCursor.MoveNext()
        gUTAssert(objCursor.blnHasCurrent = False, "After 2 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 2 MovePrev objCurrent not Nothing")
        objCursor.MoveNext()
        gUTAssert(objCursor.blnHasCurrent = False, "After 3 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 3 MovePrev objCurrent not Nothing")

        gUTAssert(objCursor.objGetNext Is Nothing, "First objGetNext not Nothing")
        gUTAssert(objCursor.objGetNext Is Nothing, "Second objGetNext not Nothing")
        gUTAssert(objCursor.objGetNext Is Nothing, "Third objGetNext not Nothing")

        gUTAssert(objCursor.objGetPrev Is Nothing, "First objGetPrev not Nothing")
        gUTAssert(objCursor.objGetPrev Is Nothing, "Second objGetPrev not Nothing")
        gUTAssert(objCursor.objGetPrev Is Nothing, "Third objGetPrev not Nothing")

        gUTSetSubTest("Register With One Trx")

        objUTReg = gobjUTNewReg()
        objReg = objUTReg.objReg

        objUTReg.LoadNormal("1001", Now, 100.25)

        gUTAssert(objReg.lngTrxCount = 1, "Expected to have one Trx")
        objCursor = objReg.objGetCursor()

        gUTAssert(objCursor.blnHasCurrent = False, "Initial blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "Initial objCurrent not Nothing")
        objTrx = objCursor.objGetNext()
        gUTAssert(Not objTrx Is Nothing, "Initial objGetNext() got Nothing")
        gUTAssert(objTrx.strNumber = "1001", "First Trx wrong number")
        gUTAssert(objTrx.curAmount = 100.25, "First Trx wrong amount")

        objCursor.MoveBeforeFirst()
        gUTAssert(objCursor.blnHasCurrent = False, "MoveBeforeFirst set blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After MoveBeforeFirst objCurrent not Nothing")

        objCursor.MoveAfterLast()
        gUTAssert(objCursor.blnHasCurrent = False, "MoveAfterLast set blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After MoveAfterLast objCurrent not Nothing")

        objCursor.MovePrev()
        gUTAssert(objCursor.blnHasCurrent = True, "After 1 MovePrev blnHasCurrent=false")
        objTrx = objCursor.objCurrent
        gUTAssert(Not objTrx Is Nothing, "After 1 MovePrev objCurrent Is Nothing")
        gUTAssert(objTrx.strNumber = "1001", "Wrong Trx.strNumber")
        objCursor.MovePrev()
        gUTAssert(objCursor.blnHasCurrent = False, "After 2 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 2 MovePrev objCurrent not Nothing")
        objCursor.MovePrev()
        gUTAssert(objCursor.blnHasCurrent = False, "After 3 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 3 MovePrev objCurrent not Nothing")

        objCursor.MoveNext()
        gUTAssert(objCursor.blnHasCurrent = True, "After 1 MovePrev blnHasCurrent=false")
        objTrx = objCursor.objCurrent
        gUTAssert(Not objTrx Is Nothing, "After 1 MovePrev objCurrent Is Nothing")
        gUTAssert(objTrx.strNumber = "1001", "Wrong Trx.strNumber")
        objCursor.MoveNext()
        gUTAssert(objCursor.blnHasCurrent = False, "After 2 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 2 MovePrev objCurrent not Nothing")
        objCursor.MoveNext()
        gUTAssert(objCursor.blnHasCurrent = False, "After 3 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 3 MovePrev objCurrent not Nothing")

        objCursor.MoveBeforeFirst()
        objTrx = objCursor.objGetNext()
        gUTAssert(objTrx.strNumber = "1001", "Wrong number")
        objTrx = objCursor.objGetNext()
        gUTAssert(objTrx Is Nothing, "Was not after last")

        objCursor.MoveAfterLast()
        objTrx = objCursor.objGetPrev()
        gUTAssert(objTrx.strNumber = "1001", "Wrong number")
        objTrx = objCursor.objGetPrev()
        gUTAssert(objTrx Is Nothing, "Was not before first")

        gUTSetSubTest("Register With Two Trx")

        objUTReg = gobjUTNewReg()
        objReg = objUTReg.objReg

        objUTReg.LoadNormal("2001", Now, 201.25)
        objUTReg.LoadNormal("2002", Now, 202.25)

        gUTAssert(objReg.lngTrxCount = 2, "Expected to have two Trx")
        objCursor = objReg.objGetCursor()

        gUTAssert(objCursor.blnHasCurrent = False, "Initial blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "Initial objCurrent not Nothing")
        objTrx = objCursor.objGetNext()
        gUTAssert(Not objTrx Is Nothing, "Initial objGetNext() got Nothing")
        gUTAssert(objTrx.strNumber = "2001", "First Trx wrong number")
        gUTAssert(objTrx.curAmount = 201.25, "First Trx wrong amount")

        objCursor.MoveBeforeFirst()
        gUTAssert(objCursor.blnHasCurrent = False, "MoveBeforeFirst set blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After MoveBeforeFirst objCurrent not Nothing")

        objCursor.MoveAfterLast()
        gUTAssert(objCursor.blnHasCurrent = False, "MoveAfterLast set blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After MoveAfterLast objCurrent not Nothing")

        objCursor.MovePrev()
        gUTAssert(objCursor.blnHasCurrent = True, "After 1 MovePrev blnHasCurrent=false")
        objTrx = objCursor.objCurrent
        gUTAssert(Not objTrx Is Nothing, "After 1 MovePrev objCurrent Is Nothing")
        gUTAssert(objTrx.strNumber = "2002", "Wrong Trx.strNumber")
        objCursor.MovePrev()
        gUTAssert(objCursor.blnHasCurrent = True, "After 2 MovePrev blnHasCurrent=false")
        objTrx = objCursor.objCurrent
        gUTAssert(Not objTrx Is Nothing, "After 2 MovePrev objCurrent Is Nothing")
        gUTAssert(objTrx.strNumber = "2001", "Wrong Trx.strNumber")
        objCursor.MovePrev()
        gUTAssert(objCursor.blnHasCurrent = False, "After 3 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 3 MovePrev objCurrent not Nothing")

        objCursor.MoveNext()
        gUTAssert(objCursor.blnHasCurrent = True, "After 1 MovePrev blnHasCurrent=false")
        objTrx = objCursor.objCurrent
        gUTAssert(Not objTrx Is Nothing, "After 1 MovePrev objCurrent Is Nothing")
        gUTAssert(objTrx.strNumber = "2001", "Wrong Trx.strNumber")
        objCursor.MoveNext()
        gUTAssert(objCursor.blnHasCurrent = True, "After 2 MovePrev blnHasCurrent=false")
        objTrx = objCursor.objCurrent
        gUTAssert(Not objTrx Is Nothing, "After 2 MovePrev objCurrent Is Nothing")
        gUTAssert(objTrx.strNumber = "2002", "Wrong Trx.strNumber")
        objCursor.MoveNext()
        gUTAssert(objCursor.blnHasCurrent = False, "After 3 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 3 MovePrev objCurrent not Nothing")

        objCursor.MoveBeforeFirst()
        objTrx = objCursor.objGetNext()
        gUTAssert(objTrx.strNumber = "2001", "Wrong number")
        objTrx = objCursor.objGetNext()
        gUTAssert(objTrx.strNumber = "2002", "Wrong number")
        objTrx = objCursor.objGetNext()
        gUTAssert(objTrx Is Nothing, "Was not after last")

        objCursor.MoveAfterLast()
        objTrx = objCursor.objGetPrev()
        gUTAssert(objTrx.strNumber = "2002", "Wrong number")
        objTrx = objCursor.objGetPrev()
        gUTAssert(objTrx.strNumber = "2001", "Wrong number")
        objTrx = objCursor.objGetPrev()
        gUTAssert(objTrx Is Nothing, "Was not before first")

        gUTSetSubTest("Register With Three Trx")

        objUTReg = gobjUTNewReg()
        objReg = objUTReg.objReg

        objUTReg.LoadNormal("3001", Now, 301.25)
        objUTReg.LoadNormal("3002", Now, 302.25)
        objUTReg.LoadNormal("3003", Now, 303.25)

        gUTAssert(objReg.lngTrxCount = 3, "Expected to have three Trx")
        objCursor = objReg.objGetCursor()

        gUTAssert(objCursor.blnHasCurrent = False, "Initial blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "Initial objCurrent not Nothing")
        objTrx = objCursor.objGetNext()
        gUTAssert(Not objTrx Is Nothing, "Initial objGetNext() got Nothing")
        gUTAssert(objTrx.strNumber = "3001", "First Trx wrong number")
        gUTAssert(objTrx.curAmount = 301.25, "First Trx wrong amount")

        objCursor.MoveBeforeFirst()
        gUTAssert(objCursor.blnHasCurrent = False, "MoveBeforeFirst set blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After MoveBeforeFirst objCurrent not Nothing")

        objCursor.MoveAfterLast()
        gUTAssert(objCursor.blnHasCurrent = False, "MoveAfterLast set blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After MoveAfterLast objCurrent not Nothing")

        objCursor.MovePrev()
        gUTAssert(objCursor.blnHasCurrent = True, "After 1 MovePrev blnHasCurrent=false")
        objTrx = objCursor.objCurrent
        gUTAssert(Not objTrx Is Nothing, "After 1 MovePrev objCurrent Is Nothing")
        gUTAssert(objTrx.strNumber = "3003", "Wrong Trx.strNumber")
        objCursor.MovePrev()
        gUTAssert(objCursor.blnHasCurrent = True, "After 2 MovePrev blnHasCurrent=false")
        objTrx = objCursor.objCurrent
        gUTAssert(Not objTrx Is Nothing, "After 2 MovePrev objCurrent Is Nothing")
        gUTAssert(objTrx.strNumber = "3002", "Wrong Trx.strNumber")
        objCursor.MovePrev()
        gUTAssert(objCursor.blnHasCurrent = True, "After 3 MovePrev blnHasCurrent=false")
        objTrx = objCursor.objCurrent
        gUTAssert(Not objTrx Is Nothing, "After 3 MovePrev objCurrent Is Nothing")
        gUTAssert(objTrx.strNumber = "3001", "Wrong Trx.strNumber")
        objCursor.MovePrev()
        gUTAssert(objCursor.blnHasCurrent = False, "After 4 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 4 MovePrev objCurrent not Nothing")

        objCursor.MoveNext()
        gUTAssert(objCursor.blnHasCurrent = True, "After 1 MovePrev blnHasCurrent=false")
        objTrx = objCursor.objCurrent
        gUTAssert(Not objTrx Is Nothing, "After 1 MovePrev objCurrent Is Nothing")
        gUTAssert(objTrx.strNumber = "3001", "Wrong Trx.strNumber")
        objCursor.MoveNext()
        gUTAssert(objCursor.blnHasCurrent = True, "After 2 MovePrev blnHasCurrent=false")
        objTrx = objCursor.objCurrent
        gUTAssert(Not objTrx Is Nothing, "After 2 MovePrev objCurrent Is Nothing")
        gUTAssert(objTrx.strNumber = "3002", "Wrong Trx.strNumber")
        objCursor.MoveNext()
        gUTAssert(objCursor.blnHasCurrent = True, "After 3 MovePrev blnHasCurrent=false")
        objTrx = objCursor.objCurrent
        gUTAssert(Not objTrx Is Nothing, "After 3 MovePrev objCurrent Is Nothing")
        gUTAssert(objTrx.strNumber = "3003", "Wrong Trx.strNumber")
        objCursor.MoveNext()
        gUTAssert(objCursor.blnHasCurrent = False, "After 4 MovePrev blnHasCurrent=true")
        gUTAssert(objCursor.objCurrent Is Nothing, "After 4 MovePrev objCurrent not Nothing")

        objCursor.MoveBeforeFirst()
        objTrx = objCursor.objGetNext()
        gUTAssert(objTrx.strNumber = "3001", "Wrong number")
        objTrx = objCursor.objGetNext()
        gUTAssert(objTrx.strNumber = "3002", "Wrong number")
        objTrx = objCursor.objGetNext()
        gUTAssert(objTrx.strNumber = "3003", "Wrong number")
        objTrx = objCursor.objGetNext()
        gUTAssert(objTrx Is Nothing, "Was not after last")

        objCursor.MoveAfterLast()
        objTrx = objCursor.objGetPrev()
        gUTAssert(objTrx.strNumber = "3003", "Wrong number")
        objTrx = objCursor.objGetPrev()
        gUTAssert(objTrx.strNumber = "3002", "Wrong number")
        objTrx = objCursor.objGetPrev()
        gUTAssert(objTrx.strNumber = "3001", "Wrong number")
        objTrx = objCursor.objGetPrev()
        gUTAssert(objTrx Is Nothing, "Was not before first")

        gUTSetSubTest("Insert and Delete")

        objUTReg = gobjUTNewReg()
        objReg = objUTReg.objReg

        objUTReg.LoadNormal("3001", Now, 301.25)
        objUTReg.LoadNormal("3003", Now, 303.25)
        objUTReg.LoadNormal("3005", Now, 305.25)

        gUTAssert(objReg.lngTrxCount = 3, "Expected to have three Trx")
        objCursor = objReg.objGetCursor()

        objCursor.MoveNext()
        objUTReg.AddNormal("3000", Now, 300.25, "", 1, 1, 4)
        objTrx = objCursor.objCurrent
        gUTAssert(objTrx.strNumber = "3001", "Did not adjust position on add at current index")
        objUTReg.AddNormal("2999", Now, 299.25, "", 1, 1, 5)
        objTrx = objCursor.objCurrent
        gUTAssert(objTrx.strNumber = "3001", "Did not adjust position on add before current index")
        objUTReg.AddNormal("3002", Now, 302.25, "", 4, 4, 6)
        objTrx = objCursor.objCurrent
        gUTAssert(objTrx.strNumber = "3001", "Adjusted position on add after current index")
        objTrx = objCursor.objGetNext()
        gUTAssert(objTrx.strNumber = "3002", "Got wrong Trx")
        objUTReg.DeleteEntry(1, 1, 5, "")
        objTrx = objCursor.objCurrent
        gUTAssert(objTrx.strNumber = "3002", "Did not adjust when deleting Trx before cursor")
        objUTReg.DeleteEntry(4, 4, 4, "")
        objTrx = objCursor.objCurrent
        gUTAssert(objTrx.strNumber = "3002", "Adjusted when deleting Trx after cursor")
        objUTReg.DeleteEntry(3, 3, 3, "")
        objTrx = objCursor.objCurrent
        gUTAssert(objTrx.strNumber = "3005", "Did not move next when deleting current")
        objUTReg.AddNormal("2998", Now, 298.25, "", 1, 1, 4)
        objTrx = objCursor.objCurrent
        gUTAssert(objTrx.strNumber = "3005", "Did not move next last when adding trx at front")
        objUTReg.DeleteEntry(4, 0, 0, "")
        gUTAssert(Not objCursor.blnHasCurrent, "Delete last=current did not make no current")
        objUTReg.DeleteEntry(3, 0, 0, "")
        gUTAssert(Not objCursor.blnHasCurrent, "Delete made current when after last")
        objTrx = objCursor.objGetPrev()
        gUTAssert(objTrx.strNumber = "3000", "Did not follow end when deleting")

        gUTSetSubTest("Clone")

        objUTReg = gobjUTNewReg()
        objReg = objUTReg.objReg

        objUTReg.LoadNormal("3001", Now, 301.25)
        objUTReg.LoadNormal("3003", Now, 303.25)
        objUTReg.LoadNormal("3005", Now, 305.25)

        gUTAssert(objReg.lngTrxCount = 3, "Expected to have three Trx")
        objCursor = objReg.objGetCursor()

        objCursor.MoveNext()
        objTrx = objCursor.objGetNext()
        gUTAssert(objTrx.strNumber = "3003", "Wrong initial pos")
        objCursor2 = objCursor.objClone()
        objTrx = objCursor2.objCurrent
        gUTAssert(objTrx.strNumber = "3003", "Wrong cloned pos")
        objCursor.MoveNext()
        objTrx = objCursor2.objCurrent
        gUTAssert(objTrx.strNumber = "3003", "Cloned pos changed")
        objTrx = objCursor.objCurrent
        gUTAssert(objTrx.strNumber = "3005", "Main pos did not change")

    End Sub

    Private Sub TestAgingBrackets()
        gUTSetTestTitle("TestAgingBrackets")

        gUTSetSubTest("Current")

        gUTAssert(gstrMakeAgingBracket(#6/1/2009#, 30, False, #6/10/2009#, #5/10/2009#, #6/20/2009#) = gstrAgingBracketCurrent(), "not paid, invoiced, due in 20 days")

        gUTAssert(gstrMakeAgingBracket(#6/1/2009#, 30, True, #6/10/2009#, #5/10/2009#, #6/20/2009#) = gstrAgingBracketCurrent(), "not paid, fake, invoiced, due in 20 days")

        gUTSetSubTest("Not invoiced")

        gUTAssert(gstrMakeAgingBracket(#6/1/2009#, 30, False, #7/5/2009#, #6/10/2009#, #7/10/2009#) = gstrAgingBracketNotInvoiced(), "future invoice date")

        gUTSetSubTest("Paid")

        gUTAssert(gstrMakeAgingBracket(#6/1/2009#, 30, False, #5/30/2009#, #5/10/2009#, #5/20/2009#) = gstrAgingBracketPaid(), "paid before aging date")

        gUTAssert(gstrMakeAgingBracket(#9/1/2009#, 30, False, #5/30/2009#, #5/10/2009#, #5/20/2009#) = gstrAgingBracketPaid(), "paid WAY before aging date")

        gUTSetSubTest("Past Due")

        gUTAssert(gstrMakeAgingBracket(#6/1/2009#, 30, False, #8/30/2009#, #5/10/2009#, #5/20/2009#) = gstrAgingBracketPastDue(1, 30), "unpaid 11 days after due date")

        gUTAssert(gstrMakeAgingBracket(#6/1/2009#, 30, False, #8/30/2009#, #4/10/2009#, #4/20/2009#) = gstrAgingBracketPastDue(31, 60), "unpaid ~40 days after due date")

        gUTSetSubTest("Future")

        gUTAssert(gstrMakeAgingBracket(#6/1/2009#, 30, False, #10/30/2009#, #5/10/2009#, #7/10/2009#) = gstrAgingBracketFuture(-59, -30), "due ~40 days after aging date")

        gUTAssert(gstrMakeAgingBracket(#6/1/2009#, 30, False, #10/30/2009#, #5/10/2009#, #8/10/2009#) = gstrAgingBracketFuture(-89, -60), "due ~70 days after aging date")
    End Sub

    Private Sub TestDateBrackets()
        gUTSetTestTitle("TestDateBrackets")

        gUTSetSubTest("Day count")

        gUTAssert(gstrMakeDateBracket(#6/1/2009#, 10, #6/1/2009#) = "2009/06/01", "equal to base date")

        gUTAssert(gstrMakeDateBracket(#6/2/2009#, 10, #6/1/2009#) = "2009/06/01", "day after base date")

        gUTAssert(gstrMakeDateBracket(#6/10/2009#, 10, #6/1/2009#) = "2009/06/01", "end of base bracket")

        gUTAssert(gstrMakeDateBracket(#6/11/2009#, 10, #6/1/2009#) = "2009/06/11", "start of next bracket")

        gUTAssert(gstrMakeDateBracket(#6/14/2009#, 10, #6/1/2009#) = "2009/06/11", "middle of next bracket")

        gUTAssert(gstrMakeDateBracket(#6/20/2009#, 10, #6/1/2009#) = "2009/06/11", "end of next bracket")

        gUTAssert(gstrMakeDateBracket(#6/21/2009#, 10, #6/1/2009#) = "2009/06/21", "start of next bracket")

        gUTAssert(gstrMakeDateBracket(#5/31/2009#, 10, #6/1/2009#) = "2009/05/22", "end of previous bracket")

        gUTAssert(gstrMakeDateBracket(#5/24/2009#, 10, #6/1/2009#) = "2009/05/22", "middle of previous bracket")

        gUTAssert(gstrMakeDateBracket(#5/22/2009#, 10, #6/1/2009#) = "2009/05/22", "start of previous bracket")

        gUTAssert(gstrMakeDateBracket(#5/21/2009#, 10, #6/1/2009#) = "2009/05/12", "start of second previous bracket")

        gUTSetSubTest("Whole month")

        gUTAssert(gstrMakeDateBracket(#6/1/2009#, -1, #1/1/2001#) = "2009/06/01", "first day of month")

        gUTAssert(gstrMakeDateBracket(#6/2/2009#, -1, #1/1/2001#) = "2009/06/01", "second day of month")

        gUTAssert(gstrMakeDateBracket(#7/31/2009#, -1, #1/1/2001#) = "2009/07/01", "last day of month")

        gUTSetSubTest("Half month")

        gUTAssert(gstrMakeDateBracket(#7/1/2009#, -2, #1/1/2001#) = "2009/07/01", "first day of first half of month")

        gUTAssert(gstrMakeDateBracket(#7/5/2009#, -2, #1/1/2001#) = "2009/07/01", "middle of first half of month")

        gUTAssert(gstrMakeDateBracket(#7/15/2009#, -2, #1/1/2001#) = "2009/07/01", "end of first half of month")

        gUTAssert(gstrMakeDateBracket(#7/16/2009#, -2, #1/1/2001#) = "2009/07/16", "first day of second half of month")

        gUTAssert(gstrMakeDateBracket(#7/21/2009#, -2, #1/1/2001#) = "2009/07/16", "middle of second half of month")

        gUTAssert(gstrMakeDateBracket(#7/31/2009#, -2, #1/1/2001#) = "2009/07/16", "last day of second half of month")

        gUTSetSubTest("Quarter month")

        gUTAssert(gstrMakeDateBracket(#7/1/2009#, -4, #1/1/2001#) = "2009/07/01", "first day of first quarter of month")

        gUTAssert(gstrMakeDateBracket(#7/5/2009#, -4, #1/1/2001#) = "2009/07/01", "middle of first quarter of month")

        gUTAssert(gstrMakeDateBracket(#7/8/2009#, -4, #1/1/2001#) = "2009/07/01", "end of first quarter of month")

        gUTAssert(gstrMakeDateBracket(#7/9/2009#, -4, #1/1/2001#) = "2009/07/09", "first day of second quarter of month")

        gUTAssert(gstrMakeDateBracket(#7/12/2009#, -4, #1/1/2001#) = "2009/07/09", "middle of second quarter of month")

        gUTAssert(gstrMakeDateBracket(#7/16/2009#, -4, #1/1/2001#) = "2009/07/09", "last day of second quarter of month")

        gUTAssert(gstrMakeDateBracket(#7/17/2009#, -4, #1/1/2001#) = "2009/07/17", "first day of third quarter of month")

        gUTAssert(gstrMakeDateBracket(#7/24/2009#, -4, #1/1/2001#) = "2009/07/17", "last day of third quarter of month")

        gUTAssert(gstrMakeDateBracket(#7/25/2009#, -4, #1/1/2001#) = "2009/07/25", "first day of last quarter of month")

        gUTAssert(gstrMakeDateBracket(#7/31/2009#, -4, #1/1/2001#) = "2009/07/25", "last day of last quarter of month")

    End Sub

    Private Sub TestRepeat()

        gUTSetTestTitle("Test repeat sequences")
        Dim objUTReg As UTRegister

        gUTSetSubTest("Test register loading")

        'One repeating trx.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .LoadNormal("1500", #6/1/2000#, -50.75, strRepeatKey:="r1", intRepeatSeq:=1)
            .objReg.LoadPostProcessing()
            .Validate("", 1)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 1, "count")
        End With

        'Two repeating trx in one sequence.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .LoadNormal("1500", #6/1/2000#, -50.75, strRepeatKey:="r1", intRepeatSeq:=1)
            .LoadNormal("1501", #6/2/2000#, -10.0#, strRepeatKey:="r1", intRepeatSeq:=2)
            .objReg.LoadPostProcessing()
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
        End With

        'Three repeating trx in two sequences.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .LoadNormal("1500", #6/1/2000#, -50.75, strRepeatKey:="r1", intRepeatSeq:=1)
            .LoadNormal("1501", #6/2/2000#, -10.0#, strRepeatKey:="r1", intRepeatSeq:=2)
            .LoadNormal("1499", #6/30/2000#, -10.0#, strRepeatKey:="r2", intRepeatSeq:=1)
            .LoadNormal("1502", #7/1/2000#, -20.0#)
            .objReg.LoadPostProcessing()
            .Validate("", 1, 2, 3, 4)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 3, "count")
        End With

        gUTSetSubTest("Test register adding")

        'One repeating trx.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .Validate("", 1)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 1, "count")
        End With

        'One repeating and one non-repeating trx.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10.0#, "Second add", 2, 2, 2)
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 1, "count")
        End With

        'Two repeating trx in one sequence.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10.0#, "Second add", 2, 2, 2, strRepeatKey:="r1", intRepeatSeq:=2)
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
        End With

        'Three repeating trx in two sequences.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10.0#, "Second add", 2, 2, 2, strRepeatKey:="r1", intRepeatSeq:=2)
            .AddNormal("1499", #5/30/2000#, -10.0#, "Third add", 1, 1, 3, strRepeatKey:="r2", intRepeatSeq:=1)
            .Validate("", 3, 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 3, "count")
        End With

        gUTSetSubTest("Test trx changing")

        'One repeating and one non-repeating trx.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10.0#, "Second add", 2, 2, 2)
            'Change the repeating trx to a different key and seq.
            .SetTrxAmount(1, 399.0#)
            .SetTrxRepeatKey(1, "R4")
            .SetTrxRepeatSeq(1, 3)
            .UpdateNormal("1500", #6/1/2000#, 399.0#, "First upd", 1, 1, 1, 2, strRepeatKey:="R4", intRepeatSeq:=3)
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 1, "count")
        End With

        'One repeating and one non-repeating trx (2).
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10.0#, "Second add", 2, 2, 2)
            'Change the non-repeating trx to repeating.
            .SetTrxAmount(2, 399.0#)
            .SetTrxRepeatKey(2, "R4")
            .SetTrxRepeatSeq(2, 3)
            .UpdateNormal("1501", #6/2/2000#, 399.0#, "First upd", 2, 2, 2, 2, strRepeatKey:="R4", intRepeatSeq:=3)
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
        End With

        'Two repeating, change one to a different key.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10.0#, "Second add", 2, 2, 2, strRepeatKey:="r2", intRepeatSeq:=1)
            'Change the second.
            .SetTrxRepeatKey(2, "R4")
            .SetTrxRepeatSeq(2, 3)
            .UpdateNormal("1501", #6/2/2000#, -10.0#, "First upd", 2, 2, 2, 2, strRepeatKey:="R4", intRepeatSeq:=3)
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
        End With

        'Two repeating, change one to a different seq.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10.0#, "Second add", 2, 2, 2, strRepeatKey:="r2", intRepeatSeq:=1)
            'Change the second.
            .SetTrxRepeatSeq(2, 3)
            .UpdateNormal("1501", #6/2/2000#, -10.0#, "First upd", 2, 2, 2, 2, strRepeatKey:="r2", intRepeatSeq:=3)
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
        End With

        gUTSetSubTest("Test delete trx")

        'Two repeating trx, then delete one.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10.0#, "Second add", 2, 2, 2, strRepeatKey:="t2", intRepeatSeq:=1)
            'Delete the second trx.
            .DeleteEntry(2, 0, 0, "delete")
            .Validate("", 1)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 1, "count")
        End With

        gUTSetSubTest("Test long list with many changes")

        'Long list with many changes.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10.0#, "Second add", 2, 2, 2, strRepeatKey:="r2", intRepeatSeq:=1)
            .AddNormal("Pmt", #6/4/2000#, -10.0#, "payee", 3, 3, 3)
            .AddNormal("Pmt", #6/15/2000#, -11.0#, "payee", 4, 4, 4)
            .AddNormal("Pmt", #6/16/2000#, -12.0#, "payee", 5, 5, 5)
            .AddNormal("Pmt", #6/17/2000#, -13.0#, "payee", 6, 6, 6)
            .Validate("", 1, 2, 3, 4, 5, 6)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
            'Change the second.
            .SetTrxRepeatKey(2, "r3")
            .SetTrxRepeatSeq(2, 3)
            .SetTrxDate(2, #6/14/2000#)
            .UpdateNormal("1501", #6/14/2000#, -10.0#, "First upd", 2, 3, 2, 3, strRepeatKey:="r3", intRepeatSeq:=3)
            .Validate("", 1, 3, 2, 4, 5, 6)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
            .DeleteEntry(4, 4, 5, "delete")
            .Validate("", 1, 3, 2, 5, 6)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
            .UpdateNormal("Pmt", #6/16/2000#, -11.0#, "update", 4, 4, 4, 5)
            .SetTrxAmount(5, -11.0#)
            .Validate("", 1, 3, 2, 5, 6)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
            .AddNormal("Inv", #7/1/2000#, -100.0#, "payee", 6, 6, 6, strRepeatKey:="r3", intRepeatSeq:=1)
            .Validate("", 1, 3, 2, 5, 6, 7)
        End With

        'Test budget trx
        objUTReg = gobjUTNewReg()
        With objUTReg
            .LoadNormal("1500", #6/1/2000#, -10.0#)
            .LoadNormal("1501", #6/1/2000#, -20.0#, strRepeatKey:="r1", intRepeatSeq:=1)
            .LoadBudget(#5/15/2000#, -1000.0#, #6/10/2000#, "01")
            .LoadBudget(#5/20/2000#, -500.0#, #6/12/2000#, "02", strRepeatKey:="r2", intRepeatSeq:=1)
            .objReg.LoadPostProcessing()
            .Validate("", 3, 4, 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
            .AddBudget(#6/3/2000#, -100.0#, #6/20/2000#, "03", "", 5, 5, 5, strRepeatKey:="r2", intRepeatSeq:=2)
            .Validate("", 3, 4, 1, 2, 5)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 3, "count")
            .UpdateBudget(#7/1/2000#, -100.0#, #7/10/2000#, "01", "", 1, 5, 1, 5)
            .SetTrxAmount(3, -100.0#)
            .SetTrxDate(3, #7/1/2000#)
            .Validate("", 4, 1, 2, 5, 3)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 3, "count")
            .DeleteEntry(1, 1, 4, "")
            .Validate("", 1, 2, 5, 3)
        End With
    End Sub

    Private Sub TestAmountsToWords()

        gUTSetTestTitle("Test gstrAmountToWords()")

        gUTSetSubTest("Main")

        TestOneAmountToWords(0, "zero")
        TestOneAmountToWords(1, "one")
        TestOneAmountToWords(1.1, "one")
        TestOneAmountToWords(1.9, "one")
        TestOneAmountToWords(2, "two")
        TestOneAmountToWords(3, "three")
        TestOneAmountToWords(3.01, "three")
        TestOneAmountToWords(3.99, "three")
        TestOneAmountToWords(4, "four")
        TestOneAmountToWords(5, "five")
        TestOneAmountToWords(6, "six")
        TestOneAmountToWords(7, "seven")
        TestOneAmountToWords(8, "eight")
        TestOneAmountToWords(9, "nine")
        TestOneAmountToWords(10, "ten")
        TestOneAmountToWords(11, "eleven")
        TestOneAmountToWords(12, "twelve")
        TestOneAmountToWords(13, "thirteen")
        TestOneAmountToWords(14, "fourteen")
        TestOneAmountToWords(15, "fifteen")
        TestOneAmountToWords(16, "sixteen")
        TestOneAmountToWords(17, "seventeen")
        TestOneAmountToWords(18, "eighteen")
        TestOneAmountToWords(19, "nineteen")
        TestOneAmountToWords(20, "twenty")
        TestOneAmountToWords(21, "twenty one")
        TestOneAmountToWords(22, "twenty two")
        TestOneAmountToWords(29, "twenty nine")
        TestOneAmountToWords(30, "thirty")
        TestOneAmountToWords(31, "thirty one")
        TestOneAmountToWords(39, "thirty nine")
        TestOneAmountToWords(40, "forty")
        TestOneAmountToWords(50, "fifty")
        TestOneAmountToWords(60, "sixty")
        TestOneAmountToWords(70, "seventy")
        TestOneAmountToWords(80, "eighty")
        TestOneAmountToWords(90, "ninety")
        TestOneAmountToWords(91, "ninety one")
        TestOneAmountToWords(99, "ninety nine")
        TestOneAmountToWords(100, "one hundred")
        TestOneAmountToWords(101, "one hundred one")
        TestOneAmountToWords(102, "one hundred two")
        TestOneAmountToWords(110, "one hundred ten")
        TestOneAmountToWords(111, "one hundred eleven")
        TestOneAmountToWords(112, "one hundred twelve")
        TestOneAmountToWords(119, "one hundred nineteen")
        TestOneAmountToWords(120, "one hundred twenty")
        TestOneAmountToWords(121, "one hundred twenty one")
        TestOneAmountToWords(129, "one hundred twenty nine")
        TestOneAmountToWords(130, "one hundred thirty")
        TestOneAmountToWords(190, "one hundred ninety")
        TestOneAmountToWords(199, "one hundred ninety nine")
        TestOneAmountToWords(200, "two hundred")
        TestOneAmountToWords(201, "two hundred one")
        TestOneAmountToWords(210, "two hundred ten")
        TestOneAmountToWords(211, "two hundred eleven")
        TestOneAmountToWords(900, "nine hundred")
        TestOneAmountToWords(990, "nine hundred ninety")
        TestOneAmountToWords(999, "nine hundred ninety nine")
        TestOneAmountToWords(1000, "one thousand")
        TestOneAmountToWords(1001, "one thousand one")
        TestOneAmountToWords(1002, "one thousand two")
        TestOneAmountToWords(1010, "one thousand ten")
        TestOneAmountToWords(1011, "one thousand eleven")
        TestOneAmountToWords(1019, "one thousand nineteen")
        TestOneAmountToWords(1020, "one thousand twenty")
        TestOneAmountToWords(1021, "one thousand twenty one")
        TestOneAmountToWords(1099, "one thousand ninety nine")
        TestOneAmountToWords(1100, "one thousand one hundred")
        TestOneAmountToWords(1101, "one thousand one hundred one")
        TestOneAmountToWords(1102, "one thousand one hundred two")
        TestOneAmountToWords(1119, "one thousand one hundred nineteen")
        TestOneAmountToWords(1130, "one thousand one hundred thirty")
        TestOneAmountToWords(1131, "one thousand one hundred thirty one")
        TestOneAmountToWords(1200, "one thousand two hundred")
        TestOneAmountToWords(1280, "one thousand two hundred eighty")
        TestOneAmountToWords(1289, "one thousand two hundred eighty nine")
        TestOneAmountToWords(1999, "one thousand nine hundred ninety nine")
        TestOneAmountToWords(2000, "two thousand")
        TestOneAmountToWords(2001, "two thousand one")
        TestOneAmountToWords(2015, "two thousand fifteen")
        TestOneAmountToWords(2050, "two thousand fifty")
        TestOneAmountToWords(2059, "two thousand fifty nine")
        TestOneAmountToWords(2150, "two thousand one hundred fifty")
        TestOneAmountToWords(2159, "two thousand one hundred fifty nine")
        TestOneAmountToWords(9999, "nine thousand nine hundred ninety nine")
        TestOneAmountToWords(10000, "ten thousand")
        TestOneAmountToWords(10001, "ten thousand one")
        TestOneAmountToWords(10019, "ten thousand nineteen")
        TestOneAmountToWords(10021, "ten thousand twenty one")
        TestOneAmountToWords(10200, "ten thousand two hundred")
        TestOneAmountToWords(10209, "ten thousand two hundred nine")
        TestOneAmountToWords(11000, "eleven thousand")
        TestOneAmountToWords(12500, "twelve thousand five hundred")
        TestOneAmountToWords(12502, "twelve thousand five hundred two")
        TestOneAmountToWords(12150, "twelve thousand one hundred fifty")
        TestOneAmountToWords(12152, "twelve thousand one hundred fifty two")
        TestOneAmountToWords(19000, "nineteen thousand")
        TestOneAmountToWords(19999, "nineteen thousand nine hundred ninety nine")
        TestOneAmountToWords(20000, "twenty thousand")
        TestOneAmountToWords(20001, "twenty thousand one")
        TestOneAmountToWords(20200, "twenty thousand two hundred")
        TestOneAmountToWords(20901, "twenty thousand nine hundred one")
        TestOneAmountToWords(32949, "thirty two thousand nine hundred forty nine")
        TestOneAmountToWords(100000, "one hundred thousand")
        TestOneAmountToWords(100500, "one hundred thousand five hundred")
        TestOneAmountToWords(100522, "one hundred thousand five hundred twenty two")
        TestOneAmountToWords(150000, "one hundred fifty thousand")
        TestOneAmountToWords(150001, "one hundred fifty thousand one")
        TestOneAmountToWords(152401, "one hundred fifty two thousand four hundred one")
        TestOneAmountToWords(152471, "one hundred fifty two thousand four hundred seventy one")
        TestOneAmountToWords(1000000, "one million")
        TestOneAmountToWords(1200000, "one million two hundred thousand")
        TestOneAmountToWords(1200471, "one million two hundred thousand four hundred seventy one")

    End Sub

    Private Sub TestOneAmountToWords(ByVal curInput As Decimal, ByVal strExpectedOutput As String)

        Dim strActualOutput As String
        strActualOutput = gstrAmountToWords(curInput)
        gUTAssert(strExpectedOutput = strActualOutput, curInput & " yields <" & strActualOutput & "> instead of <" & strExpectedOutput & ">")
    End Sub

    Private Sub TestSecurity()

        gUTSetTestTitle("Test Security")

        gUTSetSubTest("Main")

        Dim objSec As Security
        objSec = New Security
        objSec.CreateEmpty("UTSecurity.xml")
        gUTAssert(objSec.strLogin = "", "Login name was not empty")
        objSec.CreateUser("admin", "Administrator")
        gUTAssert(objSec.blnHaveUser, "Create did not remember user")
        objSec.SetPassword("master")
        objSec.CreateSignatures()
        objSec.Save()

        objSec = New Security
        objSec.Load("UTSecurity.xml")
        gUTAssert(objSec.strLogin = "", "Login name was not empty 2")
        gUTAssert(Not objSec.blnFindUser("nouser"), "Should not have found user")
        gUTAssert(objSec.blnFindUser("admin"), "Did not find admin user")
        gUTAssert(objSec.blnHaveUser, "Did not remember user was found")
        gUTAssert(objSec.blnUserSignatureIsValid, "Admin signature invalid")
        gUTAssert(Not objSec.blnPasswordMatches("wrongpass"), "Password failure")
        gUTAssert(objSec.blnPasswordMatches("master"), "Password failure 2")

        objSec.CreateUser("john", "John Public")
        gUTAssert(objSec.strLogin = "john", "Did not remember login name")
        objSec.SetPassword("password")
        objSec.CreateSignatures()
        objSec.Save()

        objSec = New Security
        objSec.Load("UTSecurity.xml")
        gUTAssert(objSec.blnFindUser("admin"), "Did not find admin 2")
        gUTAssert(objSec.strLogin = "admin", "Login was not admin 2")
        gUTAssert(Not objSec.blnPasswordMatches("wrongpass"), "Admin pass fail 2")
        gUTAssert(objSec.blnPasswordMatches("master"), "Admin pass okay 2")
        gUTAssert(objSec.blnUserSignatureIsValid, "Admin sig bad 2")
        gUTAssert(objSec.blnFindUser("john"), "Did not find john")
        gUTAssert(objSec.strLogin = "john", "Login was not john 2")
        gUTAssert(objSec.blnPasswordMatches("password"), "Bad john password")
        gUTAssert(objSec.blnUserSignatureIsValid, "Bad john sig 2")
        objSec.elmDbgUser.SetAttribute("name", "new name")
        gUTAssert(Not objSec.blnUserSignatureIsValid, "Changing name did not change signature")

    End Sub

    Private Sub UTMainForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        mobjEverything = gobjInitialize()
        If gblnUnrecognizedArgs() Then
            Exit Sub
        End If
        gLoadGlobalLists()
        gobjSecurity = New Security
        gobjSecurity.CreateEmpty("")
    End Sub

    Private Sub UTMainForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        mobjEverything.Teardown()
    End Sub
End Class