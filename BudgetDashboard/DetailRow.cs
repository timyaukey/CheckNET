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
            TCell cell = MakeDataCell(detail);
            Cells[period].AddDetail(detail);
            Cells[period].AddData(cell.CellAmount);
            AddExtraData(Cells[period], cell);
            this.RowTotal.AddData(cell.CellAmount);
        }

        protected abstract TCell MakeDataCell(TData detail);

        protected abstract void AddExtraData(TCell accumulator, TCell source);
    }
}
