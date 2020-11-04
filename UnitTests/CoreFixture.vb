Option Strict On
Option Explicit On

<TestFixture>
Public Class CoreFixture

    Private mobjCompany As Company

    <Test>
    Public Sub TestLoad()
        Dim objUTReg As UTRegister

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
        objUTReg.Validate("Check 8 trx", 2, 1, 7, 5, 8, 4, 6, 3)

        objUTReg = objLoadBuild(9)
        objUTReg.Validate("Check 9 trx", 2, 1, 7, 5, 9, 8, 4, 6, 3)

        objUTReg = objLoadBuild(10)
        objUTReg.Validate("Check 10 trx", 2, 1, 7, 5, 9, 8, 4, 10, 6, 3)

        objUTReg = objLoadBuild(11)
        objUTReg.Validate("Check 11 trx", 2, 1, 7, 5, 9, 8, 11, 4, 10, 6, 3)

        objUTReg = objLoadBuild(12)
        objUTReg.Validate("Check 12 trx", 2, 1, 7, 5, 9, 8, 11, 12, 4, 10, 6, 3)

        objUTReg = objLoadBuild(13)
        objUTReg.Validate("Check 13 trx", 2, 1, 7, 5, 9, 8, 11, 12, 13, 4, 10, 6, 3)

        objUTReg = objLoadBuild(14)
        objUTReg.Validate("Check 14 trx", 2, 1, 7, 5, 9, 8, 11, 12, 14, 13, 4, 10, 6, 3)

        objUTReg = objLoadBuild(15)
        objUTReg.Validate("Check 15 trx", 2, 1, 7, 5, 9, 8, 11, 12, 14, 13, 15, 4, 10, 6, 3)

        objUTReg = objLoadBuild(16)
        objUTReg.Validate("Check 16 trx", 2, 1, 7, 5, 9, 8, 11, 12, 14, 13, 16, 15, 4, 10, 6, 3)

        objUTReg = objLoadBuild(17)
        objUTReg.Validate("Check 17 trx", 2, 1, 7, 5, 9, 8, 11, 12, 14, 13, 16, 17, 15, 4, 10, 6, 3)
    End Sub

    Private Function objLoadBuild(ByVal intTrxCount As Integer) As UTRegister

        'This Trx sequence tests Register.lngNewInsert() and Register.lngMoveUp()
        'exhaustively.

        'It also tests budget matching and application exhaustively. Tests
        'multiple splits on a normal trx applied to different budgets, splits from
        'multiple normal trx applied to the same budget, budget limits, and
        'debit and credit budgets. Does NOT test un-applying from budget, changing
        'budget, or deleting budget.

        gUTSetSubTest("objLoadBuild(" + intTrxCount.ToString() + ")")

        Dim objUTReg As UTRegister
        objUTReg = gobjUTNewReg()
        If intTrxCount >= 1 Then
            objUTReg.LoadNormal("1000", #4/1/2000#, 100D, strDescr:="A")
        End If
        If intTrxCount >= 2 Then
            objUTReg.LoadNormal("1001", #3/1/2000#, 200D, strDescr:="A")
        End If
        If intTrxCount >= 3 Then
            objUTReg.LoadNormal("1002", #5/1/2000#, -50.99D, strDescr:="A")
        End If
        If intTrxCount >= 4 Then
            objUTReg.LoadBudget(#4/20/2000#, -200D, #4/10/2000#, "bud1", strDescr:="B")
        End If
        If intTrxCount >= 5 Then
            'One day before start of budget range, so won't apply to #4.
            objUTReg.LoadNormal("Card", #4/9/2000#, -10D, strBudgetKey:="bud1", strDescr:="B")
        End If
        If intTrxCount >= 6 Then
            'One day after end of budget range, so won't apply to #4.
            objUTReg.LoadNormal("Card", #4/21/2000#, -10D, strBudgetKey:="bud1", strDescr:="A")
        End If
        If intTrxCount >= 7 Then
            '#5 will be applied to this. Is a one day budget date range, so
            'now we've tested before and after the beginning and ending dates.
            objUTReg.LoadBudget(#4/9/2000#, -15D, #4/9/2000#, "bud1", strDescr:="A")
            objUTReg.SetTrxAmount(7, -5D)
        End If
        If intTrxCount >= 8 Then
            'Won't apply this until #10 is loaded.
            objUTReg.LoadNormal("1500", #4/11/2000#, -20D, strBudgetKey:="bud2", strDescr:="B")
        End If
        If intTrxCount >= 9 Then
            'Will apply this to the budget in #4.
            objUTReg.LoadNormal("1499", #4/11/2000#, -21D, strBudgetKey:="bud1", strDescr:="A")
            objUTReg.SetTrxAmount(4, -179D)
        End If
        If intTrxCount >= 10 Then
            '#8 will be applied to this.
            objUTReg.LoadBudget(#4/20/2000#, -100D, #4/10/2000#, "bud2", strDescr:="C")
            objUTReg.SetTrxAmount(10, -80D)
        End If
        If intTrxCount >= 11 Then
            'Cause budget in #10 to be used up exactly.
            'This also tests multiple splits applied to a single budget.
            objUTReg.LoadNormal("1501", #4/13/2000#, -80D, strBudgetKey:="bud2", strDescr:="A")
            objUTReg.SetTrxAmount(10, 0.0D)
        End If
        If intTrxCount >= 12 Then
            'Cause budget in #10 to be exceeded.
            objUTReg.LoadNormal("1502", #4/13/2000#, -2D, strBudgetKey:="bud2", strDescr:="B")
        End If
        If intTrxCount >= 13 Then
            objUTReg.LoadBudget(#4/17/2000#, -50D, #4/14/2000#, "bud3", strDescr:="A")
        End If
        If intTrxCount >= 14 Then
            'Splits applied to different budgets.
            objUTReg.LoadNormal("1503", #4/15/2000#, -10D, strBudgetKey:="bud1", vcurAmount2:=-7.3, strBudgetKey2:="bud3", strDescr:="A")
            objUTReg.SetTrxAmount(4, -169D)
            objUTReg.SetTrxAmount(13, -42.7D)
        End If
        If intTrxCount >= 15 Then
            'Credit budget.
            objUTReg.LoadBudget(#4/20/2000#, 100D, #4/10/2000#, "bud4", strDescr:="A")
        End If
        If intTrxCount >= 16 Then
            'Apply to credit budget.
            objUTReg.LoadNormal("DEP", #4/19/2000#, 0.21D, strBudgetKey:="bud4", strDescr:="A")
            objUTReg.SetTrxAmount(15, 99.79D)
        End If
        If intTrxCount >= 17 Then
            'Exceed credit budget.
            objUTReg.LoadNormal("DEP", #4/19/2000#, 120D, strBudgetKey:="bud4", strDescr:="B")
            objUTReg.SetTrxAmount(15, 0.0D)
        End If
        objUTReg.objReg.Sort()
        objUTReg.objReg.LoadApply()
        objUTReg.objReg.Sort()
        objUTReg.objReg.LoadFinish()
        objLoadBuild = objUTReg

    End Function

    <Test>
    Public Sub TestAddUpdDel()
        Dim objUTReg As UTRegister
        Dim objTrx As Trx

        objUTReg = gobjUTNewReg()
        With objUTReg

            'First we test sort order and balances (no budgets) during adds.

            gUTSetSubTest("Test add for sort order")

            .AddNormal("1500", #6/1/2000#, -50.75D, "First add", 1, 1, 1)
            .Validate("", 1)

            .AddNormal("1501", #6/1/2000#, -24.95D, "Second add", 2, 2, 2)
            .Validate("", 1, 2)
            gUTAssert(.strBudgetsChanged = "", "Expected no budgets to change")

            .AddNormal("1502", #6/1/2000#, -20D, "Third add", 3, 3, 3)
            .Validate("", 1, 2, 3)

            .AddNormal("DEP", #6/1/2000#, 400D, "Fourth add", 1, 1, 4)
            .Validate("", 4, 1, 2, 3)

            .AddNormal("1499", #6/1/2000#, -10D, "Fifth", 2, 2, 5)
            .Validate("", 4, 5, 1, 2, 3)

            'Next we test sort order and balances (no budgets) during updates.

            gUTSetSubTest("Test update for sort order")

            'This tests Register.lngMoveDown() exhaustively.
            'Register.lngMoveUp() was completely tested in register loading.

            'Move first register entry up and change amount.
            'No change to position, but all balances change.
            .SetTrxAmount(4, 399D)
            .SetTrxNumber(4, "DP2")
            .SetTrxDate(4, #5/1/2000#)
            .UpdateNormal("DP2", #5/1/2000#, 399D, "First upd", 1, 1, 1, 5)
            .Validate("", 4, 5, 1, 2, 3)

            'Move last register entry down.
            'No change to position.
            .SetTrxDate(3, #6/15/2000#)
            .UpdateNormal("1502", #6/15/2000#, -20D, "Second upd", 5, 5, 5, 5)
            .Validate("", 4, 5, 1, 2, 3)

            'Move middle register entry down one line.
            .SetTrxDate(1, #6/5/2000#)
            .SetTrxAmount(1, -49D)
            .UpdateNormal("1500", #6/5/2000#, -49D, "Third upd", 3, 4, 3, 5)
            .Validate("", 4, 5, 2, 1, 3)

            'Move middle register entry to end of registry.
            .SetTrxDate(2, #6/16/2000#)
            .UpdateNormal("1501", #6/16/2000#, -24.95D, "Fourth upd", 3, 5, 3, 5)
            .Validate("", 4, 5, 1, 3, 2)

            'Move second register entry down two lines.
            .SetTrxDate(5, #6/15/2000#)
            .SetTrxNumber(5, "1503")
            .UpdateNormal("1503", #6/15/2000#, -10D, "Fifth upd", 2, 4, 2, 4)
            .Validate("", 4, 1, 3, 5, 2)

            'Move first register entry to end of registry.
            .SetTrxDate(4, #7/1/2000#)
            .UpdateNormal("DP2", #7/1/2000#, 399D, "Sixth upd", 1, 5, 1, 5)
            .Validate("", 1, 3, 5, 2, 4)

            'Make change that doesn't affect sort order.
            .SetTrxAmount(3, -21D)
            .UpdateNormal("1502", #6/15/2000#, -21D, "Seventh upd", 2, 2, 2, 5)
            .Validate("", 1, 3, 5, 2, 4)

            'Now test budgets.

            gUTSetSubTest("Test add for budgets")

            .AddBudget(#6/16/2000#, -50D, #6/20/2000#, "bud1", "First add", 5, 5, 6)
            .Validate("", 1, 3, 5, 2, 6, 4)
            gUTAssert(.strBudgetsChanged = "", "Did not expect budgets to change")

            .SetTrxAmount(6, -46D)
            .AddNormal("Card", #6/17/2000#, -4D, "Second add", 5, 5, 5, strBudgetKey:="bud1")
            .Validate("", 1, 3, 5, 2, 7, 6, 4)
            gUTAssert(.strBudgetsChanged = ",6", "Expected budgets to change")

            gUTSetSubTest("Test update for budgets")

            'Add a second budget trx
            .AddBudget(#6/17/2000#, -100D, #6/17/2000#, "bud2", "First add", 5, 5, 8)
            .Validate("", 1, 3, 5, 2, 8, 7, 6, 4)

            'Update the normal trx currently applied to the first budget,
            'and make it split between the first and second budgets instead.
            'Note that this does not change the register ending balance, because
            'even though we change the trx amount both the new and old amounts
            'come out of budgets (though different budgets) so it nets out the
            'same in the end.
            .SetTrxAmount(8, -82.5D)
            .SetTrxAmount(7, -39.5D)
            .SetTrxAmount(6, -28D)
            .UpdateNormal("Card", #6/17/2000#, -22D, "Second add", 6, 6, 5, 6, strBudgetKey:="bud1", vcurAmount2:=-17.5, strBudgetKey2:="bud2")
            .Validate("", 1, 3, 5, 2, 8, 7, 6, 4)
            gUTAssert(.strBudgetsChanged = ",7,7,5", "Expected 3 budget changes")

            'Reduce the limit of the first budget to cause it to be exhausted.
            .SetTrxAmount(6, 0.0D)
            .UpdateBudget(#6/20/2000#, -21.94, #6/16/2000#, "bud1", "Third update", 7, 7, 7, 8)
            .Validate("", 1, 3, 5, 2, 8, 7, 6, 4)

            gUTSetSubTest("Test delete")

            'Delete from either end and the middle.
            .DeleteEntry(1, True, "First delete")
            .Validate("", 3, 5, 2, 8, 7, 6, 4)

            .DeleteEntry(7, False, "Second delete")
            .Validate("", 3, 5, 2, 8, 7, 6)

            .DeleteEntry(4, True, "Third delete")
            .Validate("", 3, 5, 2, 7, 6)

            'Delete one of the budgets, to show the applied splits are un-applied.
            objTrx = objUTReg.objReg.objTrx(4)
            gUTAssert(Not DirectCast(objTrx, NormalTrx).objFirstSplit.objBudget Is Nothing, "Expected first split to be applied")
            gUTAssert(DirectCast(objTrx, NormalTrx).objSecondSplit.objBudget Is Nothing, "Expected second split to not be applied")
            .DeleteEntry(5, False, "Fourth delete")
            .Validate("", 3, 5, 2, 7)
            gUTAssert(DirectCast(objTrx, NormalTrx).objFirstSplit.objBudget Is Nothing, "Expected split to be un-applied")

            .AddBudget(#6/20/2000#, -32D, #6/17/2000#, "bud1", "re-add", 5, 5, 5)
            .SetTrxAmount(9, -10D)
            .Validate("", 3, 5, 2, 7, 9)

            'Delete a normal trx applied to a budget, to show it un-applies.
            .SetTrxAmount(9, -32D)
            .DeleteEntry(4, True, "Fifth delete")
            .Validate("", 3, 5, 2, 9)

            'Delete the remaining trx.
            .DeleteEntry(4, False, "Sixth delete")
            .Validate("", 3, 5, 2)

            .DeleteEntry(1, True, "Seventh delete")
            .Validate("", 5, 2)

            .DeleteEntry(1, True, "Eighth delete")
            .Validate("", 2)

            .DeleteEntry(1, False, "Ninth delete")
            .Validate("")

        End With
    End Sub

    <Test>
    Public Sub TestTransfer()
        Dim objUTReg1 As UTRegister
        Dim objUTReg2 As UTRegister
        Dim objXfr As TransferManager

        objXfr = New TransferManager

        gUTSetSubTest("Init register 1")

        objUTReg1 = gobjUTNewReg(strRegisterKey:="reg1")
        With objUTReg1
            .AddNormal("DEP", #4/1/2000#, 2000D, "Add1", 1, 1, 1)
            .AddNormal("100", #4/3/2000#, -25D, "Add2", 2, 2, 2)
            .AddNormal("101", #4/5/2000#, -37D, "Add3", 3, 3, 3)
            .AddNormal("102", #4/7/2000#, -45.3D, "Add4", 4, 4, 4)
            .Validate("", 1, 2, 3, 4)
        End With

        gUTSetSubTest("Init register 2")

        objUTReg2 = gobjUTNewReg(strRegisterKey:="reg2")
        With objUTReg2
            .AddNormal("DEP", #4/1/2000#, 3000D, "Add1", 1, 1, 1)
            .AddNormal("200", #4/3/2000#, -25D, "Add2", 2, 2, 2)
            .AddNormal("201", #4/15/2000#, -37D, "Add3", 3, 3, 3)
            .AddNormal("202", #4/17/2000#, -45.3D, "Add4", 4, 4, 4)
            .Validate("", 1, 2, 3, 4)
        End With

        gUTSetSubTest("Create transfer")

        objXfr.AddTransfer(objUTReg1.objReg, objUTReg2.objReg, #4/4/2000#, "xfer1", "", False, 100.39D, "", False, False, 0)

        objUTReg1.AddTrx(objUTReg1.objReg.objTrx(3))
        objUTReg1.Validate("Transfer added to 1", 1, 2, 5, 3, 4)

        objUTReg2.AddTrx(objUTReg2.objReg.objTrx(3))
        objUTReg2.Validate("Transfer added to 2", 1, 2, 5, 3, 4)

        gUTSetSubTest("Update transfer")

        objXfr.UpdateTransfer(objUTReg1.objReg, objUTReg1.objReg.objTransferTrx(3), objUTReg2.objReg, #4/6/2000#, "xfer1", "", False, 29.95D, "", False, False, 0)

        objUTReg1.SetTrxAmount(5, 29.95D)
        objUTReg1.SetTrxDate(5, #4/6/2000#)
        objUTReg1.Validate("Transfer changed 1", 1, 2, 3, 5, 4)

        objUTReg2.SetTrxAmount(5, -29.95D)
        objUTReg2.SetTrxDate(5, #4/6/2000#)
        objUTReg2.Validate("Transfer changed 2", 1, 2, 5, 3, 4)

        gUTSetSubTest("Delete transfer")

        objXfr.DeleteTransfer(objUTReg1.objReg, objUTReg1.objReg.objTrx(4), objUTReg2.objReg)
        objUTReg1.Validate("Transfer deleted 1", 1, 2, 3, 4)
        objUTReg2.Validate("Transfer deleted 2", 1, 2, 3, 4)

        objXfr = New TransferManager

        gUTSetSubTest("Init register 1 (repeat)")

        objUTReg1 = gobjUTNewReg(strRegisterKey:="reg1")
        With objUTReg1
            .AddNormal("DEP", #4/1/2000#, 2000D, "Add1", 1, 1, 1)
            .AddNormal("100", #4/3/2000#, -25D, "Add2", 2, 2, 2)
            .AddNormal("101", #4/5/2000#, -37D, "Add3", 3, 3, 3, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("102", #4/7/2000#, -45.3D, "Add4", 4, 4, 4)
            .Validate("", 1, 2, 3, 4)
        End With

        gUTSetSubTest("Init register 2 (repeat)")

        objUTReg2 = gobjUTNewReg(strRegisterKey:="reg2")
        With objUTReg2
            .AddNormal("DEP", #4/1/2000#, 3000D, "Add1", 1, 1, 1)
            .AddNormal("200", #4/3/2000#, -25D, "Add2", 2, 2, 2)
            .AddNormal("201", #4/15/2000#, -37D, "Add3", 3, 3, 3)
            .AddNormal("202", #4/17/2000#, -45.3D, "Add4", 4, 4, 4)
            .Validate("", 1, 2, 3, 4)
        End With

        gUTSetSubTest("Create transfer (repeat)")

        objXfr.AddTransfer(objUTReg1.objReg, objUTReg2.objReg, #4/4/2000#, "xfer1", "", False, 100.39D, "r2", False, False, 2)

        objUTReg1.AddTrx(objUTReg1.objReg.objTrx(3))
        objUTReg1.Validate("Transfer added to 1", 1, 2, 5, 3, 4)
        gUTAssert(objUTReg1.objReg.colDbgRepeatTrx.Count() = 2, "")

        objUTReg2.AddTrx(objUTReg2.objReg.objTrx(3))
        objUTReg2.Validate("Transfer added to 2", 1, 2, 5, 3, 4)

        gUTSetSubTest("Update transfer (repeat)")

        objXfr.UpdateTransfer(objUTReg1.objReg, objUTReg1.objReg.objTransferTrx(3), objUTReg2.objReg, #4/6/2000#, "xfer1", "", False, 29.95D, "r3", False, False, 3)

        objUTReg1.SetTrxAmount(5, 29.95D)
        objUTReg1.SetTrxDate(5, #4/6/2000#)
        objUTReg1.SetTrxRepeatKey(5, "r3")
        objUTReg1.SetTrxRepeatSeq(5, 3)
        objUTReg1.Validate("Transfer changed 1", 1, 2, 3, 5, 4)
        gUTAssert(objUTReg1.objReg.colDbgRepeatTrx.Count() = 2, "")

        objUTReg2.SetTrxAmount(5, -29.95D)
        objUTReg2.SetTrxDate(5, #4/6/2000#)
        objUTReg2.SetTrxRepeatKey(5, "r3")
        objUTReg2.SetTrxRepeatSeq(5, 3)
        objUTReg2.Validate("Transfer changed 2", 1, 2, 5, 3, 4)
        gUTAssert(objUTReg1.objReg.colDbgRepeatTrx.Count() = 2, "")

        gUTSetSubTest("Delete transfer (repeat)")

        objXfr.DeleteTransfer(objUTReg1.objReg, objUTReg1.objReg.objTrx(4), objUTReg2.objReg)
        objUTReg1.Validate("Transfer deleted 1", 1, 2, 3, 4)
        objUTReg2.Validate("Transfer deleted 2", 1, 2, 3, 4)
        gUTAssert(objUTReg1.objReg.colDbgRepeatTrx.Count() = 1, "")

    End Sub

    <Test>
    Public Sub TestFileLoad()
        Dim objCompany As Company
        Dim objAccount As Account
        Dim objReg As Register
        Dim objTrx As Trx
        Dim objSplit As TrxSplit

        gUTSetSubTest("Load")
        objCompany = New Company(My.Application.Info.DirectoryPath & "\Data")
        objAccount = New Account()
        objAccount.Init(objCompany)
        objReg = objLoadFile(objAccount, "UTRegLoad1.txt")
        If objReg.lngTrxCount <> 4 Then
            Exit Sub
        End If

        gUTSetSubTest("Verify 1")

        objTrx = objReg.objTrx(1)
        With DirectCast(objTrx, NormalTrx)
            gUTAssert(.GetType() Is GetType(NormalTrx), "Wrong type")
            gUTAssert(.datDate = #4/13/2000#, "Wrong date")
            gUTAssert(.strDescription = "Hadley Garden Center", "Wrong description")
            gUTAssert(.strMemo = "Bird seed", "Wrong memo")
            gUTAssert(.curAmount = (-24.95 - 10.99), "Wrong amount")
            gUTAssert(.lngSplits = 2, "Wrong numbe of splits")
            objSplit = .objFirstSplit
            gUTAssert(objSplit.curAmount = -24.95, "Wrong split1 amount")
            gUTAssert(objSplit.strBudgetKey = "", "Wrong split1 budget key")
            gUTAssert(objSplit.strCategoryKey = "cat1", "Wrong split1 category key")
            objSplit = .objSecondSplit
            gUTAssert(objSplit.curAmount = -10.99, "Wrong split2 amount")
            gUTAssert(objSplit.strCategoryKey = "cat2", "Wrong split2 category key")
            gUTAssert(objSplit.strMemo = "sunflower", "Wrong split2 memo")
        End With

        gUTSetSubTest("Verify 2")

        objTrx = objReg.objTrx(2)
        With DirectCast(objTrx, NormalTrx)
            gUTAssert(.GetType() Is GetType(NormalTrx), "Wrong type")
            gUTAssert(.datDate = #4/15/2000#, "Wrong date")
            gUTAssert(.strNumber = "1001", "Wrong number")
            gUTAssert(.strImportKey = "imp1", "Wrong import key")
            gUTAssert(.strRepeatKey = "rep1", "Wrong repeat key")
            gUTAssert(.curNormalMatchRange = 22.01, "Wrong match range")
        End With

        gUTSetSubTest("Verify 3")

        objTrx = objReg.objTrx(3)
        With DirectCast(objTrx, BudgetTrx)
            gUTAssert(.GetType() Is GetType(BudgetTrx), "Wrong type")
            gUTAssert(.datDate = #4/16/2000#, "Wrong date")
            gUTAssert(.strDescription = "General household", "Wrong description")
            gUTAssert(.curAmount = (-150D + 10.99), "Wrong amount")
            gUTAssert(.datBudgetStarts = #4/10/2000#, "Wrong budget end date")
            gUTAssert(.strBudgetKey = "bud1", "Wrong budget key")
            gUTAssert(.curBudgetLimit = -150D, "Wrong budget limit")
        End With

        gUTSetSubTest("Verify 4")

        objTrx = objReg.objTrx(4)
        With DirectCast(objTrx, TransferTrx)
            gUTAssert(.GetType() Is GetType(TransferTrx), "Wrong type")
            gUTAssert(.datDate = #4/20/2000#, "Wrong date")
            gUTAssert(.strTransferKey = "xfr55", "Wrong transfer key")
            gUTAssert(.curAmount = 140.01, "Wrong transfer amount")
        End With

    End Sub

    Private Function objLoadFile(ByVal objAccount As Account, ByVal strFileName As String) As Register
        Dim objLoader As RegisterLoader
        'Dim intFile As Short
        Dim objReg As Register
        Dim objRepeatSummarizer As RepeatSummarizer
        Dim lngLinesRead As Integer

        objLoader = New RegisterLoader
        objReg = New Register
        objReg.Init(objAccount, "Regular", "reg", False, 3)
        objRepeatSummarizer = New RepeatSummarizer()
        lngLinesRead = 0
        Dim objReader As System.IO.TextReader = Nothing
        Try
            objReader = New System.IO.StreamReader(mobjCompany.strAddPath("UTData\" & strFileName))
            objLoader.LoadFile(objReader, objReg, objRepeatSummarizer, False, lngLinesRead)
        Catch ex As Exception
            Throw ex
        Finally
            objReader.Close()
        End Try
        objReg.LoadApply()
        objReg.LoadFinish()
        objLoadFile = objReg
    End Function

    <Test>
    Public Sub TestStringTranslator()
        Dim objString As SimpleStringTranslator

        gUTSetSubTest("Load")

        objString = New SimpleStringTranslator()
        objString.LoadFile(mobjCompany.strAddPath("UTData\UTStringTran1.txt"))

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
        gUTAssert(objString.strKeyToValue1("v1") = "value1", "Wrong strKeyToValue1(v1)")
        gUTAssert(objString.strKeyToValue1("zzz") = "", "Wrong strKeyToValue1(zzz)")

    End Sub

    <Test>
    Public Sub TestRepeat()

        Dim objUTReg As UTRegister

        gUTSetSubTest("Test register loading")

        'One repeating trx.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .LoadNormal("1500", #6/1/2000#, -50.75D, strRepeatKey:="r1", intRepeatSeq:=1)
            .objReg.LoadApply()
            .objReg.LoadFinish()
            .Validate("", 1)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 1, "count")
        End With

        'Two repeating trx in one sequence.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .LoadNormal("1500", #6/1/2000#, -50.75D, strRepeatKey:="r1", intRepeatSeq:=1)
            .LoadNormal("1501", #6/2/2000#, -10D, strRepeatKey:="r1", intRepeatSeq:=2)
            .objReg.LoadApply()
            .objReg.LoadFinish()
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
        End With

        'Three repeating trx in two sequences.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .LoadNormal("1500", #6/1/2000#, -50.75D, strRepeatKey:="r1", intRepeatSeq:=1)
            .LoadNormal("1501", #6/2/2000#, -10D, strRepeatKey:="r1", intRepeatSeq:=2)
            .LoadNormal("1499", #6/30/2000#, -10D, strRepeatKey:="r2", intRepeatSeq:=1)
            .LoadNormal("1502", #7/1/2000#, -20D)
            .objReg.LoadApply()
            .objReg.LoadFinish()
            .Validate("", 1, 2, 3, 4)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 3, "count")
        End With

        gUTSetSubTest("Test register adding")

        'One repeating trx.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75D, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .Validate("", 1)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 1, "count")
        End With

        'One repeating and one non-repeating trx.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75D, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10D, "Second add", 2, 2, 2)
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 1, "count")
        End With

        'Two repeating trx in one sequence.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75D, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10D, "Second add", 2, 2, 2, strRepeatKey:="r1", intRepeatSeq:=2)
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
        End With

        'Three repeating trx in two sequences.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75D, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10D, "Second add", 2, 2, 2, strRepeatKey:="r1", intRepeatSeq:=2)
            .AddNormal("1499", #5/30/2000#, -10D, "Third add", 1, 1, 3, strRepeatKey:="r2", intRepeatSeq:=1)
            .Validate("", 3, 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 3, "count")
        End With

        gUTSetSubTest("Test trx changing")

        'One repeating and one non-repeating trx.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75D, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10D, "Second add", 2, 2, 2)
            'Change the repeating trx to a different key and seq.
            .SetTrxAmount(1, 399D)
            .SetTrxRepeatKey(1, "R4")
            .SetTrxRepeatSeq(1, 3)
            .UpdateNormal("1500", #6/1/2000#, 399D, "First upd", 1, 1, 1, 2, strRepeatKey:="R4", intRepeatSeq:=3)
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 1, "count")
        End With

        'One repeating and one non-repeating trx (2).
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75D, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10D, "Second add", 2, 2, 2)
            'Change the non-repeating trx to repeating.
            .SetTrxAmount(2, 399D)
            .SetTrxRepeatKey(2, "R4")
            .SetTrxRepeatSeq(2, 3)
            .UpdateNormal("1501", #6/2/2000#, 399D, "First upd", 2, 2, 2, 2, strRepeatKey:="R4", intRepeatSeq:=3)
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
        End With

        'Two repeating, change one to a different key.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75D, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10D, "Second add", 2, 2, 2, strRepeatKey:="r2", intRepeatSeq:=1)
            'Change the second.
            .SetTrxRepeatKey(2, "R4")
            .SetTrxRepeatSeq(2, 3)
            .UpdateNormal("1501", #6/2/2000#, -10D, "First upd", 2, 2, 2, 2, strRepeatKey:="R4", intRepeatSeq:=3)
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
        End With

        'Two repeating, change one to a different seq.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75D, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10D, "Second add", 2, 2, 2, strRepeatKey:="r2", intRepeatSeq:=1)
            'Change the second.
            .SetTrxRepeatSeq(2, 3)
            .UpdateNormal("1501", #6/2/2000#, -10D, "First upd", 2, 2, 2, 2, strRepeatKey:="r2", intRepeatSeq:=3)
            .Validate("", 1, 2)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
        End With

        gUTSetSubTest("Test delete trx")

        'Two repeating trx, then delete one.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75D, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10D, "Second add", 2, 2, 2, strRepeatKey:="t2", intRepeatSeq:=1)
            'Delete the second trx.
            .DeleteEntry(2, False, "delete")
            .Validate("", 1)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 1, "count")
        End With

        gUTSetSubTest("Test long list with many changes")

        'Long list with many changes.
        objUTReg = gobjUTNewReg()
        With objUTReg
            .AddNormal("1500", #6/1/2000#, -50.75D, "First add", 1, 1, 1, strRepeatKey:="r1", intRepeatSeq:=1)
            .AddNormal("1501", #6/2/2000#, -10D, "Second add", 2, 2, 2, strRepeatKey:="r2", intRepeatSeq:=1)
            .AddNormal("Pmt", #6/4/2000#, -10D, "payee", 3, 3, 3)
            .AddNormal("Pmt", #6/15/2000#, -11D, "payee", 4, 4, 4)
            .AddNormal("Pmt", #6/16/2000#, -12D, "payee", 5, 5, 5)
            .AddNormal("Pmt", #6/17/2000#, -13D, "payee", 6, 6, 6)
            .Validate("", 1, 2, 3, 4, 5, 6)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
            'Change the second.
            .SetTrxRepeatKey(2, "r3")
            .SetTrxRepeatSeq(2, 3)
            .SetTrxDate(2, #6/14/2000#)
            .UpdateNormal("1501", #6/14/2000#, -10D, "First upd", 2, 3, 2, 3, strRepeatKey:="r3", intRepeatSeq:=3)
            .Validate("", 1, 3, 2, 4, 5, 6)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
            .DeleteEntry(4, True, "delete")
            .Validate("", 1, 3, 2, 5, 6)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
            .UpdateNormal("Pmt", #6/16/2000#, -11D, "update", 4, 4, 4, 5)
            .SetTrxAmount(5, -11D)
            .Validate("", 1, 3, 2, 5, 6)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
            .AddNormal("Inv", #7/1/2000#, -100D, "payee", 6, 6, 6, strRepeatKey:="r3", intRepeatSeq:=1)
            .Validate("", 1, 3, 2, 5, 6, 7)
        End With

        'Test budget trx
        objUTReg = gobjUTNewReg()
        With objUTReg
            .LoadNormal("1500", #6/1/2000#, -10D)
            .LoadNormal("1501", #6/1/2000#, -20D, strRepeatKey:="r1", intRepeatSeq:=1)
            .LoadBudget(#6/10/2000#, -1000D, #5/15/2000#, "01")
            .LoadBudget(#6/12/2000#, -500D, #5/20/2000#, "02", strRepeatKey:="r2", intRepeatSeq:=1)
            .objReg.LoadApply()
            .objReg.LoadFinish()
            .Validate("", 1, 2, 3, 4)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 2, "count")
            .AddBudget(#6/20/2000#, -100D, #6/3/2000#, "03", "", 5, 5, 5, strRepeatKey:="r2", intRepeatSeq:=2)
            .Validate("", 1, 2, 3, 4, 5)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 3, "count")
            .UpdateBudget(#7/10/2000#, -100D, #7/1/2000#, "01", "", 3, 5, 3, 5)
            .SetTrxAmount(3, -100D)
            .SetTrxDate(3, #7/10/2000#)
            .Validate("", 1, 2, 4, 5, 3)
            gUTAssert(.objReg.colDbgRepeatTrx.Count() = 3, "count")
            .DeleteEntry(1, True, "")
            .Validate("", 2, 4, 5, 3)
        End With
    End Sub

    <Test>
    Public Sub TestSecurity()

        gUTSetSubTest("Main")

        Dim objSec As Security
        objSec = New Security(mobjCompany)
        objSec.CreateEmpty()
        gUTAssert(objSec.strLogin = "", "Login name was not empty")
        objSec.CreateUser("admin", "Administrator")
        gUTAssert(objSec.blnHaveUser, "Create did not remember user")
        objSec.SetPassword("master")
        objSec.CreateSignatures()
        objSec.Save()

        objSec = New Security(mobjCompany)
        objSec.Load()
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

        objSec = New Security(mobjCompany)
        objSec.Load()
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

    <Test>
    Public Sub TestCriticalOperation()

        gUTSetSubTest("Main")

        Dim objUTReg As UTRegister = gobjUTNewReg()

        objUTReg.AddNormal("1500", #6/1/2000#, -50.75D, "First add", 1, 1, 1)
        objUTReg.Validate("", 1)

        objUTReg.AddNormal("1501", #6/1/2000#, -24.95D, "Second add", 2, 2, 2)
        objUTReg.Validate("", 1, 2)

        objUTReg.SetTrxAmount(1, 399D)
        objUTReg.SetTrxNumber(1, "DP2")
        objUTReg.SetTrxDate(1, #5/1/2000#)
        objUTReg.UpdateNormal("DP2", #5/1/2000#, 399D, "First upd", 1, 1, 1, 5)
        objUTReg.Validate("", 1, 2)

        Dim objTrxManager As NormalTrxManager = New NormalTrxManager(objUTReg.objReg.objNormalTrx(1))
        objTrxManager.UpdateStart()

        objTrxManager = New NormalTrxManager(objUTReg.objReg.objNormalTrx(2))
        Try
            objTrxManager.UpdateStart()
            gUTFailure("Did not fail critical operation check")
        Catch ex As Register.CriticalOperationException
            'Expected
        End Try

        '=======================================

        objUTReg = gobjUTNewReg()

        objUTReg.AddNormal("1500", #6/1/2000#, -50.75D, "First add", 1, 1, 1)
        objUTReg.Validate("", 1)

        gUTAssert(Not objUTReg.objReg.objAccount.objCompany.blnCriticalOperationFailed, "Unexpectedly said critical operation failed")

        objUTReg.AddNormal("1501", #6/1/2000#, -24.95D, "Second add", 2, 2, 2)
        objUTReg.Validate("", 1, 2)

        objUTReg.SetTrxAmount(1, 399D)
        objUTReg.SetTrxNumber(1, "DP2")
        objUTReg.SetTrxDate(1, #5/1/2000#)
        objUTReg.UpdateNormal("DP2", #5/1/2000#, 399D, "First upd", 1, 1, 1, 5)
        objUTReg.Validate("", 1, 2)

        objTrxManager = New NormalTrxManager(objUTReg.objReg.objNormalTrx(1))
        objTrxManager.UpdateStart()

        gUTAssert(objUTReg.objReg.objAccount.objCompany.blnCriticalOperationFailed, "Did not detect interrupted critical operation 2")


    End Sub

    <OneTimeSetUp>
    Public Sub OneTimeSetup()
        Dim strDataPathValue As String = My.Application.Info.DirectoryPath & "\..\..\Data"
        mobjCompany = New Company(strDataPathValue)
        CompanyLoader.LoadGlobalLists(mobjCompany)
        mobjCompany.objSecurity.CreateEmpty()
    End Sub

    <OneTimeTearDown>
    Public Sub OneTimeTearDown()
        mobjCompany.Teardown()
    End Sub
End Class