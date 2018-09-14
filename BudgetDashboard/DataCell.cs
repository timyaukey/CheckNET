using System;

namespace BudgetDashboard
{
    public class DataCell
    {
        public decimal BudgetLimit;
        public decimal BudgetApplied;

        public DataCell()
        {
            BudgetLimit = 0M;
            BudgetApplied = 0M;
        }

        public DataCell(decimal budgetLimit, decimal budgetApplied)
        {
            BudgetLimit = budgetLimit;
            BudgetApplied = budgetApplied;
        }

        public void AddData(DataCell cell)
        {
            this.BudgetLimit += cell.BudgetLimit;
            this.BudgetApplied += cell.BudgetApplied;
        }

        public override string ToString()
        {
            return "(limit=" + BudgetLimit.ToString("F2") + " applied=" + BudgetApplied.ToString("F2") + ")";
        }
    }
}
