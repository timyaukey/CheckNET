using System;

namespace BudgetDashboard
{
    public class DataCell
    {
        public decimal Amount;
        public decimal BudgetLimit;
        public decimal BudgetApplied;

        public DataCell()
        {
            Amount = 0M;
            BudgetLimit = 0M;
            BudgetApplied = 0M;
        }

        public DataCell(decimal amount, decimal budgetLimit, decimal budgetApplied)
        {
            Amount = amount;
            BudgetLimit = budgetLimit;
            BudgetApplied = budgetApplied;
        }

        public void AddData(DataCell cell)
        {
            this.Amount += cell.Amount;
            this.BudgetLimit += cell.BudgetLimit;
            this.BudgetApplied += cell.BudgetApplied;
        }

        public override string ToString()
        {
            return "(limit=" + BudgetLimit.ToString("F2") + " applied=" + BudgetApplied.ToString("F2") + ")";
        }
    }
}
