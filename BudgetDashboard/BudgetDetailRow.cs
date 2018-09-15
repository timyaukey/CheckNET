using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public class BudgetDetailRow : DetailRow<BudgetDetailCell, BudgetTrx>
    {
        public BudgetDetailRow(int periodCount, string key, string label, string sequence)
            : base(periodCount, key, label, sequence)
        {
        }

        public override BudgetDetailCell MakeDataCell(BudgetTrx detail)
        {
            return new BudgetDetailCell(detail);
        }
    }
}
