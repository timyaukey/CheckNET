using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public class BudgetDetailCell : DetailCell<BudgetTrx>
    {
        public BudgetDetailCell()
        {
        }

        public BudgetDetailCell(decimal budgetLimit, decimal budgetApplied)
            : base(budgetLimit, budgetApplied)
        {
        }
    }
}
