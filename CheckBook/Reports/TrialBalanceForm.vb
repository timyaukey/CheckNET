Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class TrialBalanceForm
    Private mobjCompany As Company
    Private mobjBalSheet As AccountGroupManager
    Private mobjIncExp As CategoryGroupManager

    Public Sub ShowWindow(ByVal objCompany As Company, ByVal objHostUI As IHostUI)
        mobjCompany = objCompany
        Me.MdiParent = objHostUI.objGetMainForm()
        Me.Show()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub btnTrialBalance_Click(sender As Object, e As EventArgs) Handles btnTrialBalance.Click
        Try
            mobjBalSheet = BalanceSheetScanner.objRun(mobjCompany, ctlEndDate.Value)
            ShowInListView(lvwBalanceSheetAccounts, mobjBalSheet, "Balance Sheet Through End Date")
            mobjIncExp = IncomeExpenseScanner.objRun(mobjCompany, New DateTime(1900, 1, 1), ctlEndDate.Value, True)
            ShowInListView(lvwIncExpAccounts, mobjIncExp, "Income/Expenses Through End Date")
            ConfigureStatementButtons(False)
            Dim curBalanceError As Decimal = mobjBalSheet.curGrandTotal + mobjIncExp.curGrandTotal
            If curBalanceError <> 0D Then
                lblResultSummary.Text = "Accounts out of balance by " + gstrFormatCurrency(curBalanceError)
            ElseIf mobjIncExp.curGrandTotal <> 0D Then
                lblResultSummary.Text = "Accounts balance, but update retained earnings"
            Else
                lblResultSummary.Text = "Accounts are in balance, and inc/exp are cleared"
            End If
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub btnStatementTotals_Click(sender As Object, e As EventArgs) Handles btnStatementTotals.Click
        Try
            mobjBalSheet = BalanceSheetScanner.objRun(mobjCompany, ctlEndDate.Value)
            ShowInListView(lvwBalanceSheetAccounts, mobjBalSheet, "Balance Sheet Through End Date")
            mobjIncExp = IncomeExpenseScanner.objRun(mobjCompany, ctlStartDate.Value, ctlEndDate.Value, False)
            ShowInListView(lvwIncExpAccounts, mobjIncExp, "Income/Expenses For Date Range")
            Dim objIncExpTotal As CategoryGroupManager = IncomeExpenseScanner.objRun(mobjCompany, New DateTime(1900, 1, 1), ctlEndDate.Value, True)
            ConfigureStatementButtons(False)
            'ConfigureStatementButtons(True) 'Comment this line out after testing the statements
            Dim curBalanceError As Decimal = mobjBalSheet.curGrandTotal + objIncExpTotal.curGrandTotal
            If curBalanceError <> 0D Then
                lblResultSummary.Text = "Accounts out of balance by " + gstrFormatCurrency(curBalanceError)
            ElseIf objIncExpTotal.curGrandTotal <> 0D Then
                lblResultSummary.Text = "Accounts balance, but update retained earnings"
            Else
                lblResultSummary.Text = "Accounts are in balance, and inc/exp are cleared"
                ConfigureStatementButtons(True)
            End If
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub ShowInListView(ByVal lvw As ListView, ByVal objData As ReportGroupManager, ByVal strGrandTotalPrefix As String)
        Dim lvwItem As ListViewItem
        lvw.Items.Clear()
        For Each objGroup As LineItemGroup In objData.colGroups
            lvwItem = New ListViewItem(objGroup.strGroupTitle)
            lvwItem.SubItems.Add(gstrFormatCurrency(objGroup.curGroupTotal))
            lvwItem.BackColor = Color.LightGray
            lvw.Items.Add(lvwItem)
            For Each objLine As ReportLineItem In objGroup.colItems
                If objLine.curTotal <> 0D Then
                    lvwItem = New ListViewItem("  " + objLine.strItemTitle)
                    lvwItem.SubItems.Add(gstrFormatCurrency(objLine.curTotal))
                    lvw.Items.Add(lvwItem)
                End If
            Next
        Next
        lvwItem = New ListViewItem(strGrandTotalPrefix)
        lvwItem.SubItems.Add(gstrFormatCurrency(objData.curGrandTotal))
        lvwItem.BackColor = Color.LightGray
        lvw.Items.Add(lvwItem)
    End Sub

    Private Sub ConfigureStatementButtons(ByVal blnEnabled As Boolean)
        btnBalanceSheet.Enabled = blnEnabled
        btnIncomeExpenseStatement.Enabled = blnEnabled
    End Sub

    Private Sub btnBalanceSheet_Click(sender As Object, e As EventArgs) Handles btnBalanceSheet.Click
        Dim objWriter As HTMLWriter = New HTMLWriter(mobjCompany, "BalanceSheet", mobjBalSheet)
        Dim objAccumAssets As ReportAccumulator = New ReportAccumulator()
        Dim objAccumLiabilities As ReportAccumulator = New ReportAccumulator()
        Dim objAccumEquity As ReportAccumulator = New ReportAccumulator()
        Dim objAccumTotal As ReportAccumulator = New ReportAccumulator()
        Dim strLineHeaderClass As String = "ReportHeader2"
        Dim strLineTitleClass As String = "ReportLineTitle2"
        Dim strLineAmountClass As String = "ReportLineAmount2"
        Dim strLineFooterTitleClass As String = "ReportFooterTitle2"
        Dim strLineFooterAmountClass As String = "ReportFooterAmount2"
        Dim strMinusClass As String = "Minus"

        objWriter.BeginReport()
        objWriter.OutputHeader("Balance Sheet", "Ending Date " + ctlEndDate.Value.ToLongDateString())

        objWriter.OutputText(strLineHeaderClass, "Assets")
        objWriter.OutputGroupSummary(strLineTitleClass, "Checking Accounts", strLineAmountClass, strMinusClass,
            Account.SubType.Asset_CheckingAccount.ToString(), objAccumAssets)
        objWriter.OutputGroupSummary(strLineTitleClass, "Savings Accounts", strLineAmountClass, strMinusClass,
            Account.SubType.Asset_SavingsAccount.ToString(), objAccumAssets)
        objWriter.OutputGroupSummary(strLineTitleClass, "Inventory", strLineAmountClass, strMinusClass,
            Account.SubType.Asset_Inventory.ToString(), objAccumAssets)
        objWriter.OutputGroupSummary(strLineTitleClass, "Accounts Receivable", strLineAmountClass, strMinusClass,
            Account.SubType.Asset_AccountsReceivable.ToString(), objAccumAssets)
        objWriter.OutputGroupSummary(strLineTitleClass, "Loans Receivable", strLineAmountClass, strMinusClass,
            Account.SubType.Asset_LoanReceivable.ToString(), objAccumAssets)
        objWriter.OutputGroupSummary(strLineTitleClass, "Real Property", strLineAmountClass, strMinusClass,
            Account.SubType.Asset_RealProperty.ToString(), objAccumAssets)
        objWriter.OutputGroupSummary(strLineTitleClass, "Other Property", strLineAmountClass, strMinusClass,
            Account.SubType.Asset_OtherProperty.ToString(), objAccumAssets)
        objWriter.OutputGroupSummary(strLineTitleClass, "Investments", strLineAmountClass, strMinusClass,
            Account.SubType.Asset_Investment.ToString(), objAccumAssets)
        objWriter.OutputGroupSummary(strLineTitleClass, "Other Assets", strLineAmountClass, strMinusClass,
            Account.SubType.Asset_Other.ToString(), objAccumAssets)
        objWriter.OutputAmount(strLineFooterTitleClass, "Total Assets", strLineFooterAmountClass, strMinusClass, objAccumAssets.curTotal, objAccumTotal)

        objWriter.OutputText(strLineHeaderClass, "Liabilities")
        objWriter.OutputGroupSummary(strLineTitleClass, "Loans Payable", strLineAmountClass, strMinusClass,
            Account.SubType.Liability_LoanPayable.ToString(), objAccumLiabilities)
        objWriter.OutputGroupSummary(strLineTitleClass, "Accounts Payable", strLineAmountClass, strMinusClass,
            Account.SubType.Liability_AccountsPayable.ToString(), objAccumLiabilities)
        objWriter.OutputGroupSummary(strLineTitleClass, "Other", strLineAmountClass, strMinusClass,
            Account.SubType.Liability_Other.ToString(), objAccumLiabilities)
        objWriter.OutputGroupSummary(strLineTitleClass, "Taxes", strLineAmountClass, strMinusClass,
            Account.SubType.Liability_Taxes.ToString(), objAccumLiabilities)
        objWriter.OutputAmount(strLineFooterTitleClass, "Total Liabilities", strLineFooterAmountClass, strMinusClass, objAccumLiabilities.curTotal, objAccumTotal)

        objWriter.OutputText(strLineHeaderClass, "Equity")
        objWriter.OutputGroupSummary(strLineTitleClass, "Cash Invested", strLineAmountClass, strMinusClass,
            Account.SubType.Equity_CashInvested.ToString(), objAccumEquity)
        objWriter.OutputGroupSummary(strLineTitleClass, "Property Invested", strLineAmountClass, strMinusClass,
            Account.SubType.Equity_PropertyInvested.ToString(), objAccumEquity)
        objWriter.OutputGroupSummary(strLineTitleClass, "Retained Earnings", strLineAmountClass, strMinusClass,
            Account.SubType.Equity_RetainedEarnings.ToString(), objAccumEquity)
        objWriter.OutputGroupSummary(strLineTitleClass, "Stock", strLineAmountClass, strMinusClass,
            Account.SubType.Equity_Stock.ToString(), objAccumEquity)
        objWriter.OutputGroupSummary(strLineTitleClass, "Other", strLineAmountClass, strMinusClass,
            Account.SubType.Equity_Other.ToString(), objAccumEquity)
        objWriter.OutputAmount(strLineFooterTitleClass, "Total Equity", strLineFooterAmountClass, strMinusClass, objAccumEquity.curTotal, objAccumTotal)

        objWriter.EndReport()
        objWriter.CheckPrinted()
        MsgBox("Balance sheet bottom line is: " + gstrFormatCurrency(objAccumTotal.curTotal))
        objWriter.ShowReport()
    End Sub

    Private Sub btnIncomeExpenseStatement_Click(sender As Object, e As EventArgs) Handles btnIncomeExpenseStatement.Click

    End Sub

    Private Sub btnPostRetainedEarnings_Click(sender As Object, e As EventArgs) Handles btnPostRetainedEarnings.Click
        Try
            Dim objRegister As Register = Nothing
            If MsgBox("This will transfer all income and expense balances to retained earnings as of " + ctlEndDate.Value.ToShortDateString() +
                      ". Are you sure you want to do this?", MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.OkCancel) <> MsgBoxResult.Ok Then
                Exit Sub
            End If
            'Find a Retained Earnings register to add NormalTrx to.
            For Each objAccount As Account In mobjCompany.colAccounts
                If objAccount.lngSubType = Account.SubType.Equity_RetainedEarnings Then
                    objRegister = objAccount.colRegisters(0)
                    Exit For
                End If
            Next
            If objRegister Is Nothing Then
                MsgBox("Unable to find Retained Earnings register")
                Exit Sub
            End If
            Dim objIncExpTotal As CategoryGroupManager = IncomeExpenseScanner.objRun(mobjCompany, New DateTime(1900, 1, 1), ctlEndDate.Value, False)
            Dim objTrx As NormalTrx = New NormalTrx(objRegister)
            objTrx.NewStartNormal(True, "Pmt", ctlEndDate.Value, "Post to retained earnings", "", Trx.TrxStatus.glngTRXSTS_UNREC,
                                  False, 0D, False, False, 0, "", "")
            For Each objGroup As LineItemGroup In objIncExpTotal.colGroups
                For Each objItem As ReportLineItem In objGroup.colItems
                    objTrx.AddSplit("", objItem.strItemKey, "", "", System.DateTime.FromOADate(0), System.DateTime.FromOADate(0), "", "", objItem.curTotal)
                Next
            Next
            objRegister.NewAddEnd(objTrx, New LogAdd(), "PostRetainedEarnings.AddTrx")
            MsgBox("Income and expenses posted to retained earnings.")
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub
End Class