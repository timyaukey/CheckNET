using System;

namespace BudgetDashboard
{
    public class DataCell
    {
        public decimal TrxAmount;
        public decimal BudgetLimit;
        public decimal BudgetApplied;
        public decimal CellAmount;

        public DataCell()
        {
            TrxAmount = 0M;
            BudgetLimit = 0M;
            BudgetApplied = 0M;
            CellAmount = 0M;
        }

        public DataCell(decimal trxAmount, decimal budgetLimit, decimal budgetApplied, decimal cellAmount)
        {
            TrxAmount = trxAmount;
            BudgetLimit = budgetLimit;
            BudgetApplied = budgetApplied;
            CellAmount = cellAmount;
        }

        public void AddData(DataCell cell)
        {
            this.TrxAmount += cell.TrxAmount;
            this.BudgetLimit += cell.BudgetLimit;
            this.BudgetApplied += cell.BudgetApplied;
            this.CellAmount += cell.CellAmount;
        }

        public override string ToString()
        {
            return "(trxamount=" + TrxAmount.ToString("F2") + " limit=" + BudgetLimit.ToString("F2") +
                " applied=" + BudgetApplied.ToString("F2") + " cellamount=" + CellAmount.ToString("F2") + ")";
        }
    }
}
