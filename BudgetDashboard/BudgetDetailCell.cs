using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public class BudgetDetailCell : DetailCell<BudgetTrx>
    {
        public BudgetDetailCell()
        {
        }

        public BudgetDetailCell(BudgetTrx budgetTrx)
            : base(budgetTrx.curAmount, budgetTrx.curBudgetLimit, budgetTrx.curBudgetApplied)
        {
        }
    }
}
