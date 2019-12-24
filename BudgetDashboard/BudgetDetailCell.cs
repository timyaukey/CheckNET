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
    }
}
