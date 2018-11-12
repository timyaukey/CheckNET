using System;

namespace BudgetDashboard
{
    public class DataCell
    {
        public decimal CellAmount;

        public DataCell()
        {
            CellAmount = 0M;
        }

        public DataCell(decimal cellAmount)
        {
            CellAmount = cellAmount;
        }

        public void AddData(decimal cellAmount)
        {
            this.CellAmount += cellAmount;
        }

        public override string ToString()
        {
            return "(cellamount=" + CellAmount.ToString("F2") + ")";
        }
    }
}
