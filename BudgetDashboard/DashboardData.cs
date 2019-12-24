using System;
using System.Collections.Generic;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.BudgetDashboard
{
    public class DashboardData
    {
        private readonly Company Company;
        private readonly BudgetTypeHandler Handler;
        public readonly int PeriodDays;
        public readonly int PeriodCount;
        public readonly DateTime StartDate;
        public readonly DateTime EndDate;
        public Decimal StartingBalance;
        public readonly Dictionary<string, BudgetDetailRow> BudgetDetailRows;
        public readonly Dictionary<string, SplitDetailRow> SplitDetailRows;
        public List<BudgetDetailRow> BudgetedIncome;
        public List<SplitDetailRow> UnbudgetedIncome;
        public List<BudgetDetailRow> BudgetedExpenses;
        public List<SplitDetailRow> UnbudgetedExpenses;
        public TotalRow TotalIncome;
        public TotalRow TotalExpense;
        public TotalRow NetProfit;
        public TotalRow RunningBalance;

        public DashboardData(Company objCompany, BudgetTypeHandler handler, int periodDays, int periodCount, DateTime startDate)
        {
            Company = objCompany;
            Handler = handler;
            PeriodDays = periodDays;
            PeriodCount = periodCount;
            StartDate = startDate;
            EndDate = startDate.AddDays(periodCount * periodDays -1);
            BudgetDetailRows = new Dictionary<string, BudgetDetailRow>();
            SplitDetailRows = new Dictionary<string, SplitDetailRow>();
        }

        public void Load()
        {
            StartingBalance = 0m;
            foreach(Account account in Company.colAccounts)
            {
                if (Handler.IncludeAccount(account))
                {
                    foreach (Register reg in account.colRegisters)
                    {
                        StartingBalance += reg.curEndingBalance(StartDate.AddDays(-1d));
                        foreach (Trx trx in reg.colDateRange(StartDate, EndDate))
                        {
                            LoadTrx(trx);
                        }
                    }
                }
            }

            BudgetedIncome = new List<BudgetDetailRow>();
            BudgetedExpenses = new List<BudgetDetailRow>();
            foreach (var row in BudgetDetailRows.Values)
            {
                if (row.RowTotal.CellAmount > 0)
                {
                    BudgetedIncome.Add(row);
                }
                else
                {
                    BudgetedExpenses.Add(row);
                }
            }
            BudgetedIncome.Sort(DataRowComparer);
            BudgetedExpenses.Sort(DataRowComparer);

            UnbudgetedIncome = new List<SplitDetailRow>();
            UnbudgetedExpenses = new List<SplitDetailRow>();
            foreach (var row in SplitDetailRows.Values)
            {
                if (row.RowTotal.CellAmount > 0)
                {
                    UnbudgetedIncome.Add(row);
                }
                else
                {
                    UnbudgetedExpenses.Add(row);
                }
            }
            UnbudgetedIncome.Sort(DataRowComparer);
            UnbudgetedExpenses.Sort(DataRowComparer);

            TotalIncome = new TotalRow(PeriodCount, "", "Total Credits", "");
            TotalExpense = new TotalRow(PeriodCount, "", "Total Debits", "");
            NetProfit = new TotalRow(PeriodCount, "", "Net Debits/Credits", "");
            RunningBalance = new TotalRow(PeriodCount, "", "Running Balance", "");
            ComputeTotals();
        }

        private int DataRowComparer<TCell>(DataRow<TCell> row1, DataRow<TCell> row2)
            where TCell : DataCell, new()
        {
            return row1.Label.CompareTo(row2.Label);
        }

        private void LoadTrx(Trx trx)
        {
            int period = GetPeriod(trx.datDate);
            NormalTrx normalTrx = trx as NormalTrx;
            if (normalTrx != null)
            {
                if (Handler.IncludeNormalTrx(normalTrx))
                {
                    SplitDetailRow row = null;
                    foreach (TrxSplit split in normalTrx.colSplits)
                    {
                        if (Handler.IncludeSplit(split))
                        {
                            if (split.objBudget == null)
                            {
                                row = GetSplitDetailRow(split);
                                row.Cells[period].CellAmount += split.curAmount;
                                row.Cells[period].Splits.Add(split);
                            }
                            else
                            {
                                BudgetDetailRow budgetRow = GetBudgetDetailRow(split.objBudget);
                                budgetRow.Cells[period].CellAmount += split.curAmount;
                                budgetRow.Cells[period].Splits.Add(split);
                            }
                        }
                    }
                    if (row != null)
                        row.Cells[period].GeneratedAmount += normalTrx.curGeneratedAmount;
                }
            }
            else
            {
                BudgetTrx budgetTrx = trx as BudgetTrx;
                if (budgetTrx != null)
                {
                    if (Handler.IncludeBudgetTrx(budgetTrx))
                    {
                        BudgetDetailRow row = GetBudgetDetailRow(budgetTrx);
                        BudgetDetailCell cell = row.Cells[period];
                        cell.CellAmount += budgetTrx.curAmount;
                        cell.GeneratedAmount += budgetTrx.curGeneratedAmount;
                        cell.BudgetLimit += budgetTrx.curBudgetLimit;
                        cell.BudgetUsed += budgetTrx.curBudgetApplied;
                        cell.Budgets.Add(budgetTrx);
                    }
                }
            }
        }

        private int GetPeriod(DateTime trxDate)
        {
            return (int)trxDate.Subtract(StartDate).TotalDays / PeriodDays;
        }

        private SplitDetailRow GetSplitDetailRow(TrxSplit split)
        {
            // Unlike for BudgetTrx we do not incorporate repeat key in the row key,
            // because there are so many different generated NormalTrx sequences it
            // would make the resulting grid unwieldy.
            string rowKey = split.strCategoryKey;
            if (!SplitDetailRows.TryGetValue(rowKey, out SplitDetailRow row))
            {
                string sequence = "";
                if (!string.IsNullOrEmpty(split.objParent.strRepeatKey))
                {
                    sequence = split.objParent.objReg.objAccount.objRepeats.strKeyToValue1(split.objParent.strRepeatKey);
                }
                row = new SplitDetailRow(PeriodCount, split.strCategoryKey,
                    Company.objCategories.strKeyToValue1(split.strCategoryKey), sequence);
                SplitDetailRows[rowKey] = row;
            }
            return row;
        }

        private BudgetDetailRow GetBudgetDetailRow(BudgetTrx budgetTrx)
        {
            string rowKey = budgetTrx.strBudgetKey + ":" + budgetTrx.strRepeatKey;
            if (!BudgetDetailRows.TryGetValue(rowKey, out BudgetDetailRow row))
            {
                string sequence = "(none)";
                if (!string.IsNullOrEmpty(budgetTrx.strRepeatKey))
                {
                    sequence = budgetTrx.objReg.objAccount.objRepeats.strKeyToValue1(budgetTrx.strRepeatKey);
                }
                row = new BudgetDetailRow(PeriodCount, budgetTrx.strBudgetKey,
                    Company.objBudgets.strKeyToValue1(budgetTrx.strBudgetKey), sequence);
                BudgetDetailRows[rowKey] = row;
            }
            return row;
        }

        public void ComputeTotals()
        {
            TotalIncome.ClearAmounts();
            TotalExpense.ClearAmounts();
            NetProfit.ClearAmounts();
            RunningBalance.ClearAmounts();
            foreach (var row in BudgetedIncome)
            {
                row.ComputeTotals();
                TotalIncome.AddRow<BudgetDetailRow, BudgetDetailCell>(row);
            }
            foreach (var row in BudgetedExpenses)
            {
                row.ComputeTotals();
                TotalExpense.AddRow<BudgetDetailRow, BudgetDetailCell>(row);
            }
            foreach (var row in UnbudgetedIncome)
            {
                row.ComputeTotals();
                TotalIncome.AddRow<SplitDetailRow, SplitDetailCell>(row);
            }
            foreach (var row in UnbudgetedExpenses)
            {
                row.ComputeTotals();
                TotalExpense.AddRow<SplitDetailRow, SplitDetailCell>(row);
            }
            NetProfit.AddRow<TotalRow, DataCell>(TotalIncome);
            NetProfit.AddRow<TotalRow, DataCell>(TotalExpense);
            decimal currentBalance = StartingBalance;
            RunningBalance.RowTotal.CellAmount = StartingBalance;
            for (var cellIndex = 0; cellIndex < PeriodCount; cellIndex++)
            {
                currentBalance += NetProfit.Cells[cellIndex].CellAmount;
                RunningBalance.Cells[cellIndex].CellAmount = currentBalance;
            }
        }
    }
}
