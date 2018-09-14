using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public abstract class DetailRow<TCell, TData> : DataRow<TCell>
        where TCell : DetailCell<TData>, new()
        where TData : class
    {
        public DetailRow(int periodCount, string key, string label)
            : base(periodCount, key, label)
        {
        }

        public void AddToPeriod(int period, TData data)
        {
            TCell newCell = MakeDataCell(data);
            Cells[period].AddDetail(data);
            Cells[period].AddData(newCell);
            this.RowTotal.AddData(newCell);
        }

        public abstract TCell MakeDataCell(TData detail);
    }
}
