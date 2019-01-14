using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public abstract class DataRow<TCell>
        where TCell : DataCell, new()
    {
        public readonly TCell[] Cells;
        public DataCell RowTotal;
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
            RowTotal = new DataCell();
        }

        public void AddRow<TRow, TCell2>(TRow row)
            where TRow : DataRow<TCell2>
            where TCell2 : DataCell, new()
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                this.Cells[i].AddData(row.Cells[i].CellAmount);
            }
            this.RowTotal.AddData(row.RowTotal.CellAmount);
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
