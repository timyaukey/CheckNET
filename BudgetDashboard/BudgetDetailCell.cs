using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public class BudgetDetailCell : DetailCell<BudgetTrx>
    {
        public decimal BudgetLimit;
        public decimal BudgetApplied;

        public BudgetDetailCell()
        {
            BudgetLimit = 0M;
            BudgetApplied = 0M;
        }

        public override void ClearAmounts()
        {
            base.ClearAmounts();
            BudgetLimit = 0M;
            BudgetApplied = 0M;
        }

        public BudgetDetailCell(BudgetTrx budgetTrx)
            : base(budgetTrx.blnIsExpired ? budgetTrx.curBudgetApplied : budgetTrx.curBudgetLimit)
        {
            BudgetLimit = budgetTrx.curBudgetLimit;
            BudgetApplied = budgetTrx.curBudgetApplied;
        }
    }
}
