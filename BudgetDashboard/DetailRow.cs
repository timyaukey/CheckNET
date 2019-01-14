using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public abstract class DetailRow<TCell, TData> : DataRow<TCell>
        where TCell : DetailCell<TData>, new()
        where TData : class
    {
        public DetailRow(int periodCount, string key, string label, string sequence)
            : base(periodCount, key, label, sequence)
        {
        }

        public void AddToPeriod(int period, TData detail)
        {
            Cells[period].AddDetail(detail);
        }

        public void AddGeneratedToPeriod(int period, decimal generated)
        {
            Cells[period].GeneratedAmount += generated;
        }

        public void ComputeTotals()
        {
            ClearAmounts();
            foreach(var periodCell in Cells)
            {
                foreach(var detail in periodCell.Details)
                {
                    TCell cell = MakeDataCell(detail);
                    periodCell.AddData(cell.CellAmount);
                    AddExtraData(periodCell, cell);
                }
                RowTotal.AddData(periodCell.CellAmount);
            }
        }

        protected abstract TCell MakeDataCell(TData detail);

        protected abstract void AddExtraData(TCell accumulator, TCell source);
    }
}
