Option Strict On
Option Explicit On


Public Class TrialBalanceForm
    Private mobjHostUI As IHostUI
    Private mobjCompany As Company

    Public Sub ShowWindow(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
        mobjCompany = mobjHostUI.Company
        Me.MdiParent = mobjHostUI.GetMainForm()
        ConfigureStatementButtons(False)
        Me.Show()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub btnTrialBalance_Click(sender As Object, e As EventArgs) Handles btnTrialBalance.Click
        Try
            If ctlEndDate.Value.Date < ctlStartDate.Value.Date Then
                mobjHostUI.ErrorMessageBox("Ending date may not be before starting date")
                Return
            End If
            If ctlAgingDate.Value.Date > ctlEndDate.Value.Date Then
                mobjHostUI.ErrorMessageBox("Aging date may not be after ending date")
                Return
            End If
            Dim objBalSheet As AccountGroupManager = objGetBalanceSheetData()
            Dim objIncExp As CategoryGroupManager = IncomeExpenseScanner.Run(mobjCompany, New DateTime(1900, 1, 1), ctlEndDate.Value.Date, True)
            ShowInListView(lvwBalanceSheetAccounts, objBalSheet, "Balance Sheet Through End Date")
            ShowInListView(lvwIncExpAccounts, objIncExp, "Income/Expenses Through End Date")
            ConfigureStatementButtons(True)
            Dim curBalanceError As Decimal = objBalSheet.GrandTotal - objIncExp.GrandTotal
            If curBalanceError <> 0D Then
                lblResultSummary.Text = "Accounts out of balance by " + Utilities.FormatCurrency(curBalanceError)
            ElseIf objIncExp.GrandTotal <> 0D Then
                lblResultSummary.Text = "Accounts balance, but update retained earnings"
            Else
                lblResultSummary.Text = "Accounts are in balance, and inc/exp are cleared"
            End If
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Function objGetBalanceSheetData() As AccountGroupManager
        Return BalanceSheetScanner.Run(mobjCompany, ctlEndDate.Value.Date)
    End Function

    Private Sub ShowInListView(ByVal lvw As ListView, ByVal objData As ReportGroupManager, ByVal strGrandTotalPrefix As String)
        Dim lvwItem As ListViewItem
        lvw.Items.Clear()
        For Each objGroup As LineItemGroup In objData.Groups
            lvwItem = New ListViewItem(objGroup.GroupTitle)
            lvwItem.SubItems.Add(Utilities.FormatCurrency(objGroup.GroupTotal))
            lvwItem.BackColor = Color.LightGray
            lvw.Items.Add(lvwItem)
            For Each objLine As ReportLineItem In objGroup.Items
                If objLine.Total <> 0D Then
                    lvwItem = New ListViewItem("  " + objLine.ItemTitle)
                    lvwItem.SubItems.Add(Utilities.FormatCurrency(objLine.Total))
                    lvw.Items.Add(lvwItem)
                End If
            Next
        Next
        lvwItem = New ListViewItem(strGrandTotalPrefix)
        lvwItem.SubItems.Add(Utilities.FormatCurrency(objData.GrandTotal))
        lvwItem.BackColor = Color.LightGray
        lvw.Items.Add(lvwItem)
    End Sub

    Private Sub ConfigureStatementButtons(ByVal blnEnabled As Boolean)
        btnBalanceSheet.Enabled = blnEnabled
        btnIncomeExpenseStatement.Enabled = blnEnabled
        btnLoansPayable.Enabled = blnEnabled
        btnLoansReceivable.Enabled = blnEnabled
        btnAccountsPayable.Enabled = blnEnabled
        btnAccountsReceivable.Enabled = blnEnabled
        btnPostRetainedEarnings.Enabled = blnEnabled
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
            Dim objConfig As CompanyInfo = mobjCompany.Info

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

            If objConfig.IsLoansReceivableSummaryOnly Then
                objWriter.OutputGroupSummary(strLineTitleClass, "Loans Receivable", strLineAmountClass, strMinusClass,
                    objBalSheet, Account.SubType.Asset_LoanReceivable.ToString(), objConfig.IsLoansReceivableSuppressZero, objAccumAssets)
            Else
                objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                    objBalSheet, Account.SubType.Asset_LoanReceivable.ToString(), objAccumAssets)
            End If

            If objConfig.IsRealPropertySummaryOnly Then
                objWriter.OutputGroupSummary(strLineTitleClass, "Real Property", strLineAmountClass, strMinusClass,
                    objBalSheet, Account.SubType.Asset_RealProperty.ToString(), objConfig.IsRealPropertySuppressZero, objAccumAssets)
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
            objWriter.OutputAmount(strLineFooterTitleClass, "Total Assets", strLineFooterAmountClass, strMinusClass, objAccumAssets.Total, objAccumTotal)

            objWriter.UseMinusNumbers = True

            objWriter.OutputText(strLineHeaderClass, "Liabilities")

            If objConfig.IsLoansPayableSummaryOnly Then
                objWriter.OutputGroupSummary(strLineTitleClass, "Loans Payable", strLineAmountClass, strMinusClass,
                    objBalSheet, Account.SubType.Liability_LoanPayable.ToString(), objConfig.IsLoansPayableSuppressZero, objAccumLiabilities)
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
            objWriter.OutputAmount(strLineFooterTitleClass, "Total Liabilities", strLineFooterAmountClass, strMinusClass, objAccumLiabilities.Total, objAccumTotal)

            objWriter.OutputText(strLineHeaderClass, "Equity")
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Equity_RetainedEarnings.ToString(), objAccumEquity)
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Equity_Stock.ToString(), objAccumEquity)
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objBalSheet, Account.SubType.Equity_Capital.ToString(), objAccumEquity)
            Dim objNetIncome As CategoryGroupManager = objComputeRetainedEarningsTrxAmt()
            If objNetIncome.GrandTotal <> 0 Then
                objWriter.OutputAmount(strLineTitleClass, "Net Income", strLineAmountClass, strMinusClass,
                    -objNetIncome.GrandTotal, objAccumEquity)
            End If
            objWriter.OutputAmount(strLineFooterTitleClass, "Total Equity", strLineFooterAmountClass, strMinusClass, objAccumEquity.Total, objAccumTotal)

            objWriter.OutputText(strLineHeaderClass, " ")
            objWriter.OutputAmount(strLineFooterTitleClass, "Total Liabilities and Equity", strLineFooterAmountClass, strMinusClass,
                                   objAccumLiabilities.Total + objAccumEquity.Total, objAccumTotal)

            objWriter.EndReport()
            objWriter.CheckPrinted(objBalSheet)
            objWriter.ShowReport()
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub btnIncomeExpenseStatement_Click(sender As Object, e As EventArgs) Handles btnIncomeExpenseStatement.Click
        Try
            Dim objIncExp As CategoryGroupManager = IncomeExpenseScanner.Run(mobjCompany, ctlStartDate.Value.Date, ctlEndDate.Value.Date, False)
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
                objIncExp, CategoryTranslator.TypeSales, False, objAccumIncome)
            objWriter.OutputGroupSummary(strLineTitleClass, "Returns", strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.TypeReturns, False, objAccumIncome)
            objWriter.OutputGroupSummary(strLineTitleClass, "Cost of Goods Sold", strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.TypeCOGS, False, objAccumIncome)
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.TypeOtherIncome, objAccumIncome)
            objWriter.OutputAmount(strLineFooterTitleClass, "Net Income", strLineFooterAmountClass, strMinusClass, objAccumIncome.Total, objAccumTotal)

            objWriter.OutputText(strLineHeaderClass, "Operating Expenses")
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.TypeOperatingExpenses, objAccumOperExp)
            objWriter.OutputGroupSummary(strLineTitleClass, "Office Expense", strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.TypeOfficeExpense, False, objAccumOperExp)
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.TypePayroll, objAccumOperExp)
            objWriter.OutputGroupSummary(strLineTitleClass, "Rental Income", strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.TypeRentInc, True, objAccumOperExp)
            objWriter.OutputGroupSummary(strLineTitleClass, "Rental Expense", strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.TypeRentExp, True, objAccumOperExp)
            objWriter.OutputAmount(strLineFooterTitleClass, "Total Operating Expenses", strLineFooterAmountClass, strMinusClass, objAccumOperExp.Total, objAccumTotal)

            objWriter.OutputText(strLineHeaderClass, "Other Expenses")
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.TypeOtherExpense, objAccumOtherExp)
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.TypeTaxes, objAccumOtherExp)
            objWriter.OutputGroupItems(strLineTitleClass, strLineAmountClass, strMinusClass,
                objIncExp, CategoryTranslator.TypeDepreciation, objAccumOtherExp)
            objWriter.OutputAmount(strLineFooterTitleClass, "Total Other Expenses", strLineFooterAmountClass, strMinusClass, objAccumOtherExp.Total, objAccumTotal)

            objWriter.OutputText(strLineHeaderClass, "Grand Total Income/Expense")
            objWriter.OutputAmount(strLineFooterTitleClass, "", strLineFooterAmountClass, strMinusClass, objAccumTotal.Total, objAccumDummy)

            objWriter.EndReport()
            objWriter.CheckPrinted(objIncExp)
            mobjHostUI.InfoMessageBox("Net profit bottom line is: " + Utilities.FormatCurrency(objAccumTotal.Total))
            objWriter.ShowReport()
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub btnPostRetainedEarnings_Click(sender As Object, e As EventArgs) Handles btnPostRetainedEarnings.Click
        Try
            Dim objRegister As Register = Nothing
            If MsgBox("This will transfer all income and expense balances to retained earnings as of " + ctlEndDate.Value.Date.ToShortDateString() +
                      ". Are you sure you want to do this?", MsgBoxStyle.OkCancel) <> MsgBoxResult.Ok Then
                Exit Sub
            End If
            'Find a Retained Earnings register to add BankTrx to.
            For Each objAccount As Account In mobjCompany.Accounts
                If objAccount.AcctSubType = Account.SubType.Equity_RetainedEarnings Then
                    objRegister = objAccount.Registers(0)
                    Exit For
                End If
            Next
            If objRegister Is Nothing Then
                mobjHostUI.InfoMessageBox("Unable to find Retained Earnings register")
                Exit Sub
            End If
            Dim objIncExpTotal As CategoryGroupManager = objComputeRetainedEarningsTrxAmt()
            Dim objTrx As BankTrx = New BankTrx(objRegister)
            objTrx.NewStartNormal(True, "Pmt", ctlEndDate.Value.Date, "Post to retained earnings", "", BaseTrx.TrxStatus.Unreconciled,
                                  False, 0D, False, False, 0, "", "")
            For Each objGroup As LineItemGroup In objIncExpTotal.Groups
                For Each objItem As ReportLineItem In objGroup.Items
                    If objItem.Total <> 0 Then
                        objTrx.AddSplit("", objItem.ItemKey, "", "", Utilities.EmptyDate, Utilities.EmptyDate, "", "", -objItem.Total)
                    End If
                Next
            Next
            objRegister.NewAddEnd(objTrx, New LogAdd(), "PostRetainedEarnings.AddTrx")
            mobjHostUI.InfoMessageBox("Income and expenses posted to retained earnings.")
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Function objComputeRetainedEarningsTrxAmt() As CategoryGroupManager
        Return IncomeExpenseScanner.Run(mobjCompany, New DateTime(1900, 1, 1), ctlEndDate.Value.Date, True)
    End Function


    Private Sub btnLoansPayable_Click(sender As Object, e As EventArgs) Handles btnLoansPayable.Click
        Try
            OutputLoansPayableOrReceivable(Account.SubType.Liability_LoanPayable,
                "LoansPayable", "Loans Payable Balances", "Total Loans Payable Balance")
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub btnLoansReceivable_Click(sender As Object, e As EventArgs) Handles btnLoansReceivable.Click
        Try
            OutputLoansPayableOrReceivable(Account.SubType.Asset_LoanReceivable,
                "LoansReceivable", "Loans Receivable Balances", "Total Loans Receivable Balance")
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub OutputLoansPayableOrReceivable(ByVal lngSubType As Account.SubType,
                                            ByVal strReportFileName As String,
                                            ByVal strReportTitle As String,
                                            ByVal strReportTotalTag As String)

        Dim objWriter As HTMLWriter = New HTMLWriter(mobjHostUI, strReportFileName, False)
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
        objWriter.OutputHeader(strReportTitle, "As Of " + ctlEndDate.Value.Date.ToShortDateString())

        Dim objLoanGroup As LineItemGroup = objBalSheet.GetGroup(lngSubType.ToString())
        For Each objItem As ReportLineItem In objLoanGroup.Items
            If objItem.Total <> 0D Then
                objWriter.OutputAmount(strLineTitleClass, objItem.ItemTitle, strLineAmountClass, strMinusClass, objItem.Total, objAccumTotal)
            End If
        Next

        objWriter.OutputText(strLineHeaderClass, strReportTotalTag)
        objWriter.OutputAmount(strLineFooterTitleClass, "", strLineFooterAmountClass, strMinusClass, objAccumTotal.Total, objAccumDummy)

        objWriter.EndReport()
        objWriter.ShowReport()
    End Sub

    Private Sub btnAccountsPayable_Click(sender As Object, e As EventArgs) Handles btnAccountsPayable.Click
        Try
            OutputAccounts(Of VendorSummary)(Account.SubType.Liability_AccountsPayable,
                "AccountsPayable", "Accounts Payable", "Total Accounts Payable")
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub btnAccountsReceivable_Click(sender As Object, e As EventArgs) Handles btnAccountsReceivable.Click
        Try
            OutputAccounts(Of CustomerSummary)(Account.SubType.Asset_AccountsReceivable,
                "AccountsReceivable", "Accounts Receivable", "Total Accounts Receivable")
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub OutputAccounts(Of TSummary As {TrxNameSummary, New})(ByVal lngSubType As Account.SubType,
                                            ByVal strReportFileName As String,
                                            ByVal strReportTitle As String,
                                            ByVal strReportTotalTag As String)

        Dim objWriter As HTMLWriter = New HTMLWriter(mobjHostUI, strReportFileName, False)
        'Dim objAccumTotal As ReportAccumulator = New ReportAccumulator()
        'Dim objAccumDummy As ReportAccumulator = New ReportAccumulator()
        'Dim strLineHeaderClass As String = "ReportHeader2"
        Dim strLineTitleClass As String = "ReportTableDataTitle"
        Dim strLineAmountClass As String = "ReportTableDataAmount"
        'Dim strLineFooterTitleClass As String = "ReportFooterTitle2"
        'Dim strLineFooterAmountClass As String = "ReportFooterAmount2"
        Dim strMinusClass As String = "Minus"

        Dim colSummary As List(Of TSummary) =
            TrxNameSummary.ScanTrx(Of TSummary)(mobjCompany, ctlEndDate.Value.Date, ctlAgingDate.Value.Date, lngSubType)
        Dim objTotals As TSummary = New TSummary()
        objTotals.TrxName = "Total"

        objWriter.BeginReport()
        objWriter.OutputHeader(strReportTitle, "As Of " + ctlEndDate.Value.Date.ToShortDateString())
        objWriter.OutputText("ReportSubTitle", "Aging Date " + ctlAgingDate.Value.Date.ToShortDateString())
        objWriter.OutputText("ReportSubTitle", "&nbsp;")

        objWriter.OutputTableStart()
        objWriter.OutputTableHeaderStart()
        objWriter.OutputTableHeaderTitle("Account")
        objWriter.OutputTableHeaderAmount("Current")
        objWriter.OutputTableHeaderAmount("1-30 days")
        objWriter.OutputTableHeaderAmount("31-60 days")
        objWriter.OutputTableHeaderAmount("61-90 days")
        objWriter.OutputTableHeaderAmount("90+ days")
        objWriter.OutputTableHeaderAmount("Future")
        objWriter.OutputTableHeaderAmount("Credits")
        objWriter.OutputTableHeaderAmount("Total")
        objWriter.OutputTableHeaderEnd()

        For Each objSummary As TSummary In colSummary
            If objSummary.Balance <> 0D Then
                OutputAccountRow(objWriter, strLineTitleClass, strLineAmountClass, strMinusClass, objSummary)
            End If
            objTotals.SummaryCurrentCharges.DateTotal += objSummary.SummaryCurrentCharges.DateTotal
            objTotals.Summary1To30Charges.DateTotal += objSummary.Summary1To30Charges.DateTotal
            objTotals.Summary31To60Charges.DateTotal += objSummary.Summary31To60Charges.DateTotal
            objTotals.Summary61To90Charges.DateTotal += objSummary.Summary61To90Charges.DateTotal
            objTotals.SummaryOver90Charges.DateTotal += objSummary.SummaryOver90Charges.DateTotal
            objTotals.SummaryFutureCharges.DateTotal += objSummary.SummaryFutureCharges.DateTotal
            objTotals.SummaryPayments.DateTotal += objSummary.SummaryPayments.DateTotal
            objTotals.Balance += objSummary.Balance
        Next
        OutputAccountRow(objWriter, strLineTitleClass, strLineAmountClass, strMinusClass, objTotals)

        objWriter.OutputTableEnd()

        objWriter.EndReport()
        objWriter.ShowReport()
    End Sub

    Private Shared Sub OutputAccountRow(Of TSummary As {TrxNameSummary, New})(objWriter As HTMLWriter,
        strLineTitleClass As String, strLineAmountClass As String, strMinusClass As String, objSummary As TSummary)
        objWriter.OutputTableRowStart()
        objWriter.OutputTableDataTitle(strLineTitleClass, objSummary.TrxName)
        objWriter.OutputTableDataAmount(strLineAmountClass, strMinusClass, objSummary.SummaryCurrentCharges.DateTotal)
        objWriter.OutputTableDataAmount(strLineAmountClass, strMinusClass, objSummary.Summary1To30Charges.DateTotal)
        objWriter.OutputTableDataAmount(strLineAmountClass, strMinusClass, objSummary.Summary31To60Charges.DateTotal)
        objWriter.OutputTableDataAmount(strLineAmountClass, strMinusClass, objSummary.Summary61To90Charges.DateTotal)
        objWriter.OutputTableDataAmount(strLineAmountClass, strMinusClass, objSummary.SummaryOver90Charges.DateTotal)
        objWriter.OutputTableDataAmount(strLineAmountClass, strMinusClass, objSummary.SummaryFutureCharges.DateTotal)
        objWriter.OutputTableDataAmount(strLineAmountClass, strMinusClass, objSummary.SummaryPayments.DateTotal)
        objWriter.OutputTableDataAmount(strLineAmountClass, strMinusClass, objSummary.Balance)
        objWriter.OutputTableRowEnd()
    End Sub
End Class