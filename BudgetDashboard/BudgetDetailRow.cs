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

        public override DataCell MakeDataCell(BudgetTrx detail)
        {
            return new DataCell(detail.curBudgetLimit, detail.curBudgetApplied);
        }
    }
}
