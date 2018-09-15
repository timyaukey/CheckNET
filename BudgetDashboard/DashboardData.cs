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
        public readonly Dictionary<string, BudgetDetailRow> BudgetDetailRows;
        public readonly Dictionary<string, SplitDetailRow> SplitDetailRows;
        public readonly List<BudgetDetailRow> BudgetedIncome;
        public readonly List<SplitDetailRow> UnbudgetedIncome;
        public readonly List<BudgetDetailRow> BudgetedExpenses;
        public readonly List<SplitDetailRow> UnbudgetedExpenses;
        public readonly TotalRow TotalIncome;
        public readonly TotalRow TotalExpense;
        public readonly TotalRow NetProfit;

        public DashboardData(Account account, int periodDays, int periodCount, DateTime startDate)
        {
            this.Company = account.objCompany;
            this.Account = account;
            this.PeriodDays = periodDays;
            this.PeriodCount = periodCount;
            this.StartDate = startDate;
            this.EndDate = startDate.AddDays(periodCount * periodDays -1);
            BudgetDetailRows = new Dictionary<string, BudgetDetailRow>();
            SplitDetailRows = new Dictionary<string, SplitDetailRow>();
            BudgetedIncome = new List<BudgetDetailRow>();
            UnbudgetedIncome = new List<SplitDetailRow>();
            BudgetedExpenses = new List<BudgetDetailRow>();
            UnbudgetedExpenses = new List<SplitDetailRow>();
            TotalIncome = new TotalRow(periodCount, "", "Total Income");
            TotalExpense = new TotalRow(periodCount, "", "Total Expense");
            NetProfit = new TotalRow(periodCount, "", "Net Profit");
        }

        public void Load()
        {
            foreach(Register reg in Account.colRegisters)
            {
                foreach(Trx trx in reg.colDateRange(StartDate, EndDate))
                {
                    LoadTrx(trx);
                }
            }
            foreach(var row in BudgetDetailRows.Values)
            {
                if (row.RowTotal.Amount > 0)
                {
                    BudgetedIncome.Add(row);
                    TotalIncome.AddRow<BudgetDetailRow, BudgetDetailCell>(row);
                }
                else
                {
                    BudgetedExpenses.Add(row);
                    TotalExpense.AddRow<BudgetDetailRow, BudgetDetailCell>(row);
                }
            }
            BudgetedIncome.Sort(DataRowComparer);
            BudgetedExpenses.Sort(DataRowComparer);
            foreach(var row in SplitDetailRows.Values)
            {
                if (row.RowTotal.Amount > 0)
                {
                    UnbudgetedIncome.Add(row);
                    TotalIncome.AddRow<SplitDetailRow, SplitDetailCell>(row);
                }
                else
                {
                    UnbudgetedExpenses.Add(row);
                    TotalExpense.AddRow<SplitDetailRow, SplitDetailCell>(row);
                }
            }
            NetProfit.AddRow<TotalRow, DataCell>(TotalIncome);
            NetProfit.AddRow<TotalRow, DataCell>(TotalExpense);
            UnbudgetedIncome.Sort(DataRowComparer);
            UnbudgetedExpenses.Sort(DataRowComparer);
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
                foreach (TrxSplit split in normalTrx.colSplits)
                {
                    if (split.objBudget == null)
                    {
                        SplitDetailRow row;
                        if (!SplitDetailRows.TryGetValue(split.strCategoryKey, out row))
                        {
                            row = new SplitDetailRow(PeriodCount, split.strCategoryKey,
                                Company.objCategories.strKeyToValue1(split.strCategoryKey));
                            SplitDetailRows[split.strCategoryKey] = row;
                        }
                        SplitCarrier carrier = new SplitCarrier(split, normalTrx);
                        row.AddToPeriod(period, carrier);
                    }
                }
            }
            else
            {
                BudgetTrx budgetTrx = trx as BudgetTrx;
                if (budgetTrx != null)
                {
                    BudgetDetailRow row;
                    if (!BudgetDetailRows.TryGetValue(budgetTrx.strBudgetKey, out row))
                    {
                        row = new BudgetDetailRow(PeriodCount, budgetTrx.strBudgetKey,
                            Company.objBudgets.strKeyToValue1(budgetTrx.strBudgetKey));
                        BudgetDetailRows[budgetTrx.strBudgetKey] = row;
                    }
                    row.AddToPeriod(period, budgetTrx);
                }
            }
        }

        private int GetPeriod(DateTime trxDate)
        {
            return (int)trxDate.Subtract(StartDate).TotalDays / PeriodDays;
        }
    }
}
