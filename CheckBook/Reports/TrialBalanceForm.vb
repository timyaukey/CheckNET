Option Strict On
Option Explicit On


Public Class TrialBalanceForm
    Private mobjHostUI As IHostUI
    Private mobjCompany As Company

    Public Sub ShowWindow(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
        mobjCompany = mobjHostUI.objCompany
        Me.MdiParent = mobjHostUI.objGetMainForm()
        ConfigureStatementButtons(False)
        Me.Show()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub btnTrialBalance_Click(sender As Object, e As EventArgs) Handles btnTrialBalance.Click
        Try
            Dim objBalSheet As AccountGroupManager = objGetBalanceSheetData()
            Dim objIncExp As CategoryGroupManager = IncomeExpenseScanner.objRun(mobjCompany, New DateTime(1900, 1, 1), ctlEndDate.Value.Date, True)
            ShowInListView(lvwBalanceSheetAccounts, objBalSheet, "Balance Sheet Through End Date")
            ShowInListView(lvwIncExpAccounts, objIncExp, "Income/Expenses Through End Date")
            ConfigureStatementButtons(True)
            Dim curBalanceError As Decimal = objBalSheet.curGrandTotal - objIncExp.curGrandTotal
            If curBalanceError <> 0D Then
                lblResultSummary.Text = "Accounts out of balance by " + Utilities.strFormatCurrency(curBalanceError)
            ElseIf objIncExp.curGrandTotal <> 0D Then
                lblResultSummary.Text = "Accounts balance, but update retained earnings"
            Else
                lblResultSummary.Text = "Accounts are in balance, and inc/exp are cleared"
            End If
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function objGetBalanceSheetData() As AccountGroupManager
        Return BalanceSheetScanner.objRun(mobjCompany, ctlEndDate.Value.Date)
    End Function

    Private Sub ShowInListView(ByVal lvw As ListView, ByVal objData As ReportGroupManager, ByVal strGrandTotalPrefix As String)
        Dim lvwItem As ListViewItem
        lvw.Items.Clear()
        For Each objGroup As LineItemGroup In objData.colGroups
            lvwItem = New ListViewItem(objGroup.strGroupTitle)
            lvwItem.SubItems.Add(Utilities.strFormatCurrency(objGroup.curGroupTotal))
            lvwItem.BackColor = Color.LightGray
            lvw.Items.Add(lvwItem)
            For Each objLine As ReportLineItem In objGroup.colItems
                If objLine.curTotal <> 0D Then
                    lvwItem = New ListViewItem("  " + objLine.strItemTitle)
                    lvwItem.SubItems.Add(Utilities.strFormatCurrency(objLine.curTotal))
                    lvw.Items.Add(lvwItem)
                End If
            Next
        Next
        lvwItem = New ListViewItem(strGrandTotalPrefix)
        lvwItem.SubItems.Add(Utilities.strFormatCurrency(objData.curGrandTotal))
        lvwItem.BackColor = Color.LightGray
        lvw.Items.Add(lvwItem)
    End Sub

    Private Sub ConfigureStatementButtons(ByVal blnEnabled As Boolean)
        btnBalanceSheet.Enabled = blnEnabled
        btnIncomeExpenseStatement.Enabled = blnEnabled
        btnLoanBalances.Enabled = blnEnabled
        btnVendorBalances.Enabled = blnEnabled
    End Sub

    Private Sub btnBalanceSheet_Click(sender As Object, e As EventArgs) Handles btnBalanceSheet.Click
        Try
            Dim objBalSheet As AccountGroupManager = objGetBalanceSheetData()
            Dim objWriter As HTMLWriter = New HTMLWriter(mobjHostUI, "BalanceSheet", True)
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
            Dim objConfig As CompanyInfo = mobjCompany.objInfo

            objWriter.BeginReport()
            objWriter.OutputHeader("Balance Sheet", "As Of " + ctlEndDate.Value.Date.ToShortDateString())

            objWriter.OutputText(strLineHeaderClass, "Assets")
            objWriter.OutputGroupSummary(strLineTitleClass, "Checking Accounts", strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Asset_CheckingAccount.ToString(), False, objAccumAssets)
            objWriter.OutputGroupSummary(strLineTitleClass, "Savings Accounts", strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Asset_SavingsAccount.ToString(), True, objAccumAssets)
            objWriter.OutputGroupSummary(strLineTitleClass, "Inventory", strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Asset_Inventory.ToString(), False, objAccumAssets)
            objWriter.OutputGroupSummary(strLineTitleClass, "Accounts Receivable", strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Asset_AccountsReceivable.ToString(), False, objAccumAssets)

            If objConfig.blnLoansReceivableSummaryOnly Then
                objWriter.OutputGroupSummary(strLineTitleClass, "Loans Receivable", strLineAmountClass, strMinusClass,
                    objBalSheet, Account.SubType.Asset_LoanReceivable.ToString(), objConfig.blnLoansReceivableSuppressZero, objAccumAssets)
            Else
                objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                    objBalSheet, Account.SubType.Asset_LoanReceivable.ToString(), objAccumAssets)
            End If

            If objConfig.blnRealPropertySummaryOnly Then
                objWriter.OutputGroupSummary(strLineTitleClass, "Real Property", strLineAmountClass, strMinusClass,
                    objBalSheet, Account.SubType.Asset_RealProperty.ToString(), objConfig.blnRealPropertySuppressZero, objAccumAssets)
            Else
                objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                    objBalSheet, Account.SubType.Asset_RealProperty.ToString(), objAccumAssets)
            End If

            objWriter.OutputGroupSummary(strLineTitleClass, "Other Property", strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Asset_OtherProperty.ToString(), False, objAccumAssets)
            objWriter.OutputGroupSummary(strLineTitleClass, "Investments", strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Asset_Investment.ToString(), True, objAccumAssets)
            objWriter.OutputGroupSummary(strLineTitleClass, "Other Assets", strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Asset_Other.ToString(), True, objAccumAssets)
            objWriter.OutputAmount(strLineFooterTitleClass, "Total Assets", strLineFooterAmountClass, strMinusClass, objAccumAssets.curTotal, objAccumTotal)

            objWriter.blnUseMinusNumbers = True

            objWriter.OutputText(strLineHeaderClass, "Liabilities")

            If objConfig.blnLoansPayableSummaryOnly Then
                objWriter.OutputGroupSummary(strLineTitleClass, "Loans Payable", strLineAmountClass, strMinusClass,
                    objBalSheet, Account.SubType.Liability_LoanPayable.ToString(), objConfig.blnLoansPayableSuppressZero, objAccumLiabilities)
            Else
                objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                    objBalSheet, Account.SubType.Liability_LoanPayable.ToString(), objAccumLiabilities)
            End If

            objWriter.OutputGroupSummary(strLineTitleClass, "Accounts Payable", strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Liability_AccountsPayable.ToString(), False, objAccumLiabilities)
            objWriter.OutputGroupSummary(strLineTitleClass, "Other", strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Liability_Other.ToString(), True, objAccumLiabilities)
            objWriter.OutputGroupSummary(strLineTitleClass, "Taxes", strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Liability_Taxes.ToString(), True, objAccumLiabilities)
            objWriter.OutputAmount(strLineFooterTitleClass, "Total Liabilities", strLineFooterAmountClass, strMinusClass, objAccumLiabilities.curTotal, objAccumTotal)

            objWriter.OutputText(strLineHeaderClass, "Equity")
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Equity_RetainedEarnings.ToString(), objAccumEquity)
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Equity_Stock.ToString(), objAccumEquity)
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Equity_Capital.ToString(), objAccumEquity)
            objWriter.OutputAmount(strLineFooterTitleClass, "Total Equity", strLineFooterAmountClass, strMinusClass, objAccumEquity.curTotal, objAccumTotal)

            objWriter.OutputText(strLineHeaderClass, " ")
            objWriter.OutputAmount(strLineFooterTitleClass, "Total Liabilities and Equity", strLineFooterAmountClass, strMinusClass,
                                   objAccumLiabilities.curTotal + objAccumEquity.curTotal, objAccumTotal)

            objWriter.EndReport()
            objWriter.CheckPrinted(objBalSheet)
            objWriter.ShowReport()
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub btnIncomeExpenseStatement_Click(sender As Object, e As EventArgs) Handles btnIncomeExpenseStatement.Click
        Try
            Dim objIncExp As CategoryGroupManager = IncomeExpenseScanner.objRun(mobjCompany, ctlStartDate.Value.Date, ctlEndDate.Value.Date, False)
            Dim objWriter As HTMLWriter = New HTMLWriter(mobjHostUI, "ProfitAndLoss", True)
            Dim objAccumIncome As ReportAccumulator = New ReportAccumulator()
            Dim objAccumOperExp As ReportAccumulator = New ReportAccumulator()
            Dim objAccumOtherExp As ReportAccumulator = New ReportAccumulator()
            Dim objAccumTotal As ReportAccumulator = New ReportAccumulator()
            Dim objAccumDummy As ReportAccumulator = New ReportAccumulator()
            Dim strLineHeaderClass As String = "ReportHeader2"
            Dim strLineTitleClass As String = "ReportLineTitle2"
            Dim strLineAmountClass As String = "ReportLineAmount2"
            Dim strLineFooterTitleClass As String = "ReportFooterTitle2"
            Dim strLineFooterAmountClass As String = "ReportFooterAmount2"
            Dim strMinusClass As String = "Minus"

            objWriter.BeginReport()
            objWriter.OutputHeader("Profit and Loss Statement",
                                   "From " + ctlStartDate.Value.Date.ToShortDateString() + " To " + ctlEndDate.Value.Date.ToShortDateString())

            objWriter.OutputText(strLineHeaderClass, "Income")
            objWriter.OutputGroupSummary(strLineTitleClass, "Sales", strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.strTypeSales, False, objAccumIncome)
            objWriter.OutputGroupSummary(strLineTitleClass, "Returns", strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.strTypeReturns, False, objAccumIncome)
            objWriter.OutputGroupSummary(strLineTitleClass, "Cost of Goods Sold", strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.strTypeCOGS, False, objAccumIncome)
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.strTypeOtherIncome, objAccumIncome)
            objWriter.OutputAmount(strLineFooterTitleClass, "Net Income", strLineFooterAmountClass, strMinusClass, objAccumIncome.curTotal, objAccumTotal)

            objWriter.OutputText(strLineHeaderClass, "Operating Expenses")
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.strTypeOperatingExpenses, objAccumOperExp)
            objWriter.OutputGroupSummary(strLineTitleClass, "Office Expense", strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.strTypeOfficeExpense, False, objAccumOperExp)
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.strTypePayroll, objAccumOperExp)
            objWriter.OutputGroupSummary(strLineTitleClass, "Rental Income", strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.strTypeRentInc, True, objAccumOperExp)
            objWriter.OutputGroupSummary(strLineTitleClass, "Rental Expense", strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.strTypeRentExp, True, objAccumOperExp)
            objWriter.OutputAmount(strLineFooterTitleClass, "Total Operating Expenses", strLineFooterAmountClass, strMinusClass, objAccumOperExp.curTotal, objAccumTotal)

            objWriter.OutputText(strLineHeaderClass, "Other Expenses")
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.strTypeOtherExpense, objAccumOtherExp)
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.strTypeTaxes, objAccumOtherExp)
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.strTypeDepreciation, objAccumOtherExp)
            objWriter.OutputAmount(strLineFooterTitleClass, "Total Other Expenses", strLineFooterAmountClass, strMinusClass, objAccumOtherExp.curTotal, objAccumTotal)

            objWriter.OutputText(strLineHeaderClass, "Grand Total Income/Expense")
            objWriter.OutputAmount(strLineFooterTitleClass, "", strLineFooterAmountClass, strMinusClass, objAccumTotal.curTotal, objAccumDummy)

            objWriter.EndReport()
            objWriter.CheckPrinted(objIncExp)
            mobjHostUI.InfoMessageBox("Net profit bottom line is: " + Utilities.strFormatCurrency(objAccumTotal.curTotal))
            objWriter.ShowReport()
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub btnPostRetainedEarnings_Click(sender As Object, e As EventArgs) Handles btnPostRetainedEarnings.Click
        Try
            Dim objRegister As Register = Nothing
            If MsgBox("This will transfer all income and expense balances to retained earnings as of " + ctlEndDate.Value.Date.ToShortDateString() +
                      ". Are you sure you want to do this?", MsgBoxStyle.OkCancel) <> MsgBoxResult.Ok Then
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
                mobjHostUI.InfoMessageBox("Unable to find Retained Earnings register")
                Exit Sub
            End If
            Dim objIncExpTotal As CategoryGroupManager = IncomeExpenseScanner.objRun(mobjCompany, New DateTime(1900, 1, 1), ctlEndDate.Value.Date, True)
            Dim objTrx As NormalTrx = New NormalTrx(objRegister)
            objTrx.NewStartNormal(True, "Pmt", ctlEndDate.Value.Date, "Post to retained earnings", "", Trx.TrxStatus.Unreconciled,
                                  False, 0D, False, False, 0, "", "")
            For Each objGroup As LineItemGroup In objIncExpTotal.colGroups
                For Each objItem As ReportLineItem In objGroup.colItems
                    If objItem.curTotal <> 0 Then
                        objTrx.AddSplit("", objItem.strItemKey, "", "", System.DateTime.FromOADate(0), System.DateTime.FromOADate(0), "", "", -objItem.curTotal)
                    End If
                Next
            Next
            objRegister.NewAddEnd(objTrx, New LogAdd(), "PostRetainedEarnings.AddTrx")
            mobjHostUI.InfoMessageBox("Income and expenses posted to retained earnings.")
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub btnLoanBalances_Click(sender As Object, e As EventArgs) Handles btnLoanBalances.Click
        Try
            Dim objWriter As HTMLWriter = New HTMLWriter(mobjHostUI, "LoanBalances", False)
            Dim objBalSheet As AccountGroupManager = objGetBalanceSheetData()
            Dim objAccumTotal As ReportAccumulator = New ReportAccumulator()
            Dim objAccumDummy As ReportAccumulator = New ReportAccumulator()
            Dim strLineHeaderClass As String = "ReportHeader2"
            Dim strLineTitleClass As String = "ReportLineTitle2"
            Dim strLineAmountClass As String = "ReportLineAmount2"
            Dim strLineFooterTitleClass As String = "ReportFooterTitle2"
            Dim strLineFooterAmountClass As String = "ReportFooterAmount2"
            Dim strMinusClass As String = "Minus"

            objWriter.BeginReport()
            objWriter.OutputHeader("Long Term Debt Balances", "As Of " + ctlEndDate.Value.Date.ToShortDateString())

            Dim objLoanGroup As LineItemGroup = objBalSheet.objGetGroup(Account.SubType.Liability_LoanPayable.ToString())
            For Each objItem As ReportLineItem In objLoanGroup.colItems
                If objItem.curTotal <> 0D Then
                    objWriter.OutputAmount(strLineTitleClass, objItem.strItemTitle, strLineAmountClass, strMinusClass, objItem.curTotal, objAccumTotal)
                End If
            Next

            objWriter.OutputText(strLineHeaderClass, "Total Long Term Debt")
            objWriter.OutputAmount(strLineFooterTitleClass, "", strLineFooterAmountClass, strMinusClass, objAccumTotal.curTotal, objAccumDummy)

            objWriter.EndReport()
            objWriter.ShowReport()
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub btnVendorBalances_Click(sender As Object, e As EventArgs) Handles btnVendorBalances.Click
        Try
            Dim objWriter As HTMLWriter = New HTMLWriter(mobjHostUI, "VendorBalances", False)
            Dim objAccumTotal As ReportAccumulator = New ReportAccumulator()
            Dim objAccumDummy As ReportAccumulator = New ReportAccumulator()
            Dim strLineHeaderClass As String = "ReportHeader2"
            Dim strLineTitleClass As String = "ReportLineTitle2"
            Dim strLineAmountClass As String = "ReportLineAmount2"
            Dim strLineFooterTitleClass As String = "ReportFooterTitle2"
            Dim strLineFooterAmountClass As String = "ReportFooterAmount2"
            Dim strMinusClass As String = "Minus"

            Dim colVendors As List(Of VendorSummary) = VendorSummary.colScanVendors(mobjCompany, ctlEndDate.Value.Date)

            objWriter.BeginReport()
            objWriter.OutputHeader("Vendor Balances", "As Of " + ctlEndDate.Value.Date.ToShortDateString())

            For Each objVendor As VendorSummary In colVendors
                If objVendor.curBalance <> 0D Then
                    objWriter.OutputAmount(strLineTitleClass, objVendor.strVendorName, strLineAmountClass, strMinusClass, objVendor.curBalance, objAccumTotal)
                End If
            Next

            objWriter.OutputText(strLineHeaderClass, "Total Vendor Debt")
            objWriter.OutputAmount(strLineFooterTitleClass, "", strLineFooterAmountClass, strMinusClass, objAccumTotal.curTotal, objAccumDummy)

            objWriter.EndReport()
            objWriter.ShowReport()
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub
End Class