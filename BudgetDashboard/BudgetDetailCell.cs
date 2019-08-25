using System;
using System.Collections.Generic;

using CheckBookLib;

namespace BudgetDashboard
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
