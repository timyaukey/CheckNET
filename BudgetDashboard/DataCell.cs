using System;

namespace BudgetDashboard
{
    public class DataCell
    {
        public decimal CellAmount;
        public decimal GeneratedAmount;
        public decimal BudgetLimit;
        public decimal BudgetUsed;

        public DataCell()
        {
            CellAmount = 0M;
            GeneratedAmount = 0M;
            BudgetLimit = 0M;
            BudgetUsed = 0M;
        }

        public void ClearAmounts()
        {
            this.CellAmount = 0M;
            this.GeneratedAmount = 0M;
            this.BudgetLimit = 0M;
            this.BudgetUsed = 0M;
        }

        public void Add(DataCell cell)
        {
            this.CellAmount += cell.CellAmount;
            this.GeneratedAmount += cell.GeneratedAmount;
            this.BudgetLimit += cell.BudgetLimit;
            this.BudgetUsed += cell.BudgetUsed;
        }

        public override string ToString()
        {
            return "(cellamount=" + CellAmount.ToString("F2") + ")";
        }
    }
}
