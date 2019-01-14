using System;
using System.Collections.Generic;

using CheckBookLib;

namespace BudgetDashboard
{
    public class DashboardData
    {
        private readonly Company Company;
        public readonly Account Account;
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

        public bool HasUnalignedBudgetPeriods;

        public DashboardData(Account account, int periodDays, int periodCount, DateTime startDate)
        {
            Company = account.objCompany;
            Account = account;
            PeriodDays = periodDays;
            PeriodCount = periodCount;
            StartDate = startDate;
            EndDate = startDate.AddDays(periodCount * periodDays -1);
            BudgetDetailRows = new Dictionary<string, BudgetDetailRow>();
            SplitDetailRows = new Dictionary<string, SplitDetailRow>();
            HasUnalignedBudgetPeriods = false;
        }

        public void Load()
        {
            StartingBalance = 0m;
            foreach(Register reg in Account.colRegisters)
            {
                StartingBalance += reg.curEndingBalance(StartDate.AddDays(-1d));
                foreach(Trx trx in reg.colDateRange(StartDate, EndDate))
                {
                    LoadTrx(trx);
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

        private int DataRowComparer<TCell, TData>(DetailRow<TCell, TData> row1, DetailRow<TCell, TData> row2)
            where TCell : DetailCell<TData>, new()
            where TData : class
        {
            return row1.Label.CompareTo(row2.Label);
        }

        private void LoadTrx(Trx trx)
        {
            int period = GetPeriod(trx.datDate);
            NormalTrx normalTrx = trx as NormalTrx;
            if (normalTrx != null)
            {
                SplitDetailRow row = null;
                foreach (TrxSplit split in normalTrx.colSplits)
                {
                    if (split.objBudget == null)
                    {
                        // Unlike for BudgetTrx we do not incorporate repeat key in the row key,
                        // because there are so many different generated NormalTrx sequences it
                        // would make the resulting grid unwieldy.
                        string rowKey = split.strCategoryKey;
                        if (!SplitDetailRows.TryGetValue(rowKey, out row))
                        {
                            string sequence = "";
                            if (!string.IsNullOrEmpty(normalTrx.strRepeatKey))
                            {
                                sequence = normalTrx.objReg.objAccount.objRepeats.strKeyToValue1(normalTrx.strRepeatKey);
                            }
                            row = new SplitDetailRow(PeriodCount, split.strCategoryKey,
                                Company.objCategories.strKeyToValue1(split.strCategoryKey), sequence);
                            SplitDetailRows[rowKey] = row;
                        }
                        row.AddToPeriod(period, split);
                    }
                }
                if (row != null)
                    row.AddGeneratedToPeriod(period, normalTrx.curGeneratedAmount);
            }
            else
            {
                BudgetTrx budgetTrx = trx as BudgetTrx;
                if (budgetTrx != null)
                {
                    BudgetDetailRow row;
                    string rowKey = budgetTrx.strBudgetKey + ":" + budgetTrx.strRepeatKey;
                    if (!BudgetDetailRows.TryGetValue(rowKey, out row))
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
                    row.AddToPeriod(period, budgetTrx);
                    row.AddGeneratedToPeriod(period, budgetTrx.curGeneratedAmount);
                    if (period != GetPeriod(budgetTrx.datBudgetStarts))
                    {
                        row.HasUnalignedPeriods = true;
                        HasUnalignedBudgetPeriods = true;
                    }
                }
            }
        }

        private int GetPeriod(DateTime trxDate)
        {
            return (int)trxDate.Subtract(StartDate).TotalDays / PeriodDays;
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
