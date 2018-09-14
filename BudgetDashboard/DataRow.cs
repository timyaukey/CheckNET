using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public abstract class DataRow<TCell, TData>
        where TCell : DetailCell<TData>, new()
        where TData : class
    {
        public readonly TCell[] Cells;
        public DataCell RowTotal;
        public readonly string Key;
        public readonly string Label;

        public DataRow(int periodCount, string key, string label)
        {
            this.Key = key;
            this.Label = label;
            this.Cells = new TCell[periodCount];
            for (int i = 0; i < periodCount; i++)
                Cells[i] = new TCell();
            RowTotal = new DataCell();
        }

        public void AddToPeriod(int period, TData data)
        {
            DataCell newCell = MakeDataCell(data);
            Cells[period].AddDetail(data);
            Cells[period].AddData(newCell);
            this.RowTotal.AddData(newCell);
        }

        public abstract DataCell MakeDataCell(TData detail);

        public override string ToString()
        {
            return Label + " " + RowTotal.ToString();
        }
    }
}
