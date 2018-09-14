using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public class BudgetDetailRow : DataRow<BudgetDetailCell, BudgetTrx>
    {
        public BudgetDetailRow(int periodCount, string key, string label)
            : base(periodCount, key, label)
        {
        }

        public override BudgetDetailCell MakeDataCell(BudgetTrx detail)
        {
            return new BudgetDetailCell(detail.curBudgetLimit, detail.curBudgetApplied);
        }
    }
}
