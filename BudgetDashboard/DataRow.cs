using System;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.BudgetDashboard
{
    public abstract class DataRow<TCell>
        where TCell : DataCell, new()
    {
        public readonly TCell[] Cells;
        public TCell RowTotal;
        public readonly string Key;
        public readonly string Label;
        public readonly string Sequence;

        public DataRow(int periodCount, string key, string label, string sequence)
        {
            this.Key = key;
            this.Label = label;
            this.Sequence = sequence;
            this.Cells = new TCell[periodCount];
            for (int i = 0; i < periodCount; i++)
                Cells[i] = new TCell();
            RowTotal = new TCell();
        }

        public void AddRow<TRow, TCell2>(TRow row)
            where TRow : DataRow<TCell2>
            where TCell2 : DataCell, new()
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                this.Cells[i].Add(row.Cells[i]);
            }
            this.RowTotal.Add(row.RowTotal);
        }

        public void ComputeTotals()
        {
            RowTotal.ClearAmounts();
            for (int i = 0; i < Cells.Length; i++)
            {
                RowTotal.Add(this.Cells[i]);
            }
        }

        public void ClearAmounts()
        {
            RowTotal.ClearAmounts();
            foreach (var periodCell in Cells)
            {
                periodCell.ClearAmounts();
            }
        }

        public override string ToString()
        {
            return Label + " " + Sequence + " " + RowTotal.ToString();
        }
    }
}
