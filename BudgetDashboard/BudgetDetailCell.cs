using System;
using System.Collections.Generic;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.BudgetDashboard
{
    public class BudgetDetailCell : DataCell
    {
        public List<TrxSplit> Splits;
        public List<BudgetTrx> Budgets;

        public BudgetDetailCell()
        {
            Splits = new List<TrxSplit>();
            Budgets = new List<BudgetTrx>();
        }

        public override void SetAmountsFromDetail()
        {
            this.ClearAmounts();
            foreach(BudgetTrx budgetTrx in this.Budgets)
            {
                this.CellAmount += budgetTrx.Amount;
                this.GeneratedAmount += budgetTrx.GeneratedAmount;
                this.BudgetLimit += budgetTrx.BudgetLimit;
                this.BudgetUsed += budgetTrx.BudgetApplied;
            }
            foreach(TrxSplit split in this.Splits)
            {
                this.CellAmount += split.Amount;
            }
        }
    }
}
