using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public class SplitDetailCell : DetailCell<SplitCarrier>
    {
        public SplitDetailCell()
        {
        }

        public SplitDetailCell(decimal budgetLimit, decimal budgetApplied)
            : base(budgetLimit, budgetApplied)
        {
        }
    }
}
