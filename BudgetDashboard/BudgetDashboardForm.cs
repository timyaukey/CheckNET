using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CheckBookLib;

namespace BudgetDashboard
{
    public partial class BudgetDashboardForm : Form
    {
        private Company mCompany;
        private IHostUI mHostUI;
        private DashboardData mData;

        public BudgetDashboardForm()
        {
            InitializeComponent();
        }

        public void Show(IHostUI hostUI, DashboardData data)
        {
            mHostUI = hostUI;
            mCompany = mHostUI.objCompany;
            mData = data;
            DisplayData();
            this.MdiParent = mHostUI.objGetMainForm();
            this.Show();
        }

        private void DisplayData()
        {
            grdMain.ColumnCount = mData.PeriodCount + 3;
            ConfigureColumn(0, "Category", 200, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft);
            ConfigureColumn(1, "Sequence", 200, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft);
            ConfigureColumn(2, "Row Total", 100, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.MiddleRight);
            DateTime periodStart = mData.StartDate;
            for (int colIndex = 1; colIndex <= mData.PeriodCount; colIndex++)
            {
                ConfigureColumn(colIndex + 2, periodStart.ToShortDateString(), 100, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.MiddleRight);
                periodStart = periodStart.AddDays(mData.PeriodDays);
            }
            grdMain.Columns[0].Frozen = true;
            grdMain.Columns[1].Frozen = true;
            grdMain.Columns[2].Frozen = true;
            foreach(var row in mData.UnbudgetedIncome)
            {
                AddGridRow<SplitDetailRow, SplitDetailCell>(row, Color.White, false);
            }
            foreach (var row in mData.BudgetedIncome)
            {
                AddGridRow<BudgetDetailRow, BudgetDetailCell>(row, Color.White, true);
            }
            AddGridRow<TotalRow, DataCell>(mData.TotalIncome, Color.LightGray, false);
            foreach (var row in mData.UnbudgetedExpenses)
            {
                AddGridRow<SplitDetailRow, SplitDetailCell>(row, Color.White, false);
            }
            foreach(var row in mData.BudgetedExpenses)
            {
                AddGridRow<BudgetDetailRow, BudgetDetailCell>(row, Color.White, true);
            }
            AddGridRow<TotalRow, DataCell>(mData.TotalExpense, Color.LightGray, false);
            AddGridRow<TotalRow, DataCell>(mData.NetProfit, Color.LightGreen, false);
        }

        private void ConfigureColumn(int colIndex, string title, int width, 
            DataGridViewContentAlignment headerAlignment, DataGridViewContentAlignment dataAlignment)
        {
            var col = grdMain.Columns[colIndex];
            col.HeaderText = title;
            col.Width = width;
            col.HeaderCell.Style.Alignment = headerAlignment;
            col.DefaultCellStyle.Alignment = dataAlignment;
        }

        private void AddGridRow<TRow, TCell2>(TRow row, Color rowBackgroundColor, bool useBudgetCell)
            where TRow : DataRow<TCell2>
            where TCell2 : DataCell, new()
        {
            DataGridViewRow gridRow = new DataGridViewRow();
            AddTextCell(gridRow, row.Label, rowBackgroundColor);
            AddTextCell(gridRow, row.Sequence, rowBackgroundColor);
            if (useBudgetCell)
                AddBudgetCell(gridRow, row.RowTotal, rowBackgroundColor);
            else
                AddDecimalCell(gridRow, row.RowTotal.CellAmount, rowBackgroundColor);
            for (int periodIndex = 0; periodIndex < mData.PeriodCount; periodIndex++)
            {
                if (useBudgetCell)
                    AddBudgetCell(gridRow, row.Cells[periodIndex], rowBackgroundColor);
                else
                    AddDecimalCell(gridRow, row.Cells[periodIndex].CellAmount, rowBackgroundColor);
            }
            grdMain.Rows.Add(gridRow);
        }

        private void AddTextCell(DataGridViewRow gridRow, string text, Color cellBackgroundColor)
        {
            AddCell(gridRow, new DataGridViewTextBoxCell(),
                text, cellBackgroundColor);
        }

        private void AddBudgetCell(DataGridViewRow gridRow, DataCell dataCell, Color cellBackgroundColor)
        {
            AddCell(gridRow, new BudgetGridCell(dataCell.BudgetLimit, dataCell.BudgetApplied), 
                dataCell.CellAmount.ToString("F2"), cellBackgroundColor);
        }

        private void AddDecimalCell(DataGridViewRow gridRow, decimal amount, Color cellBackgroundColor)
        {
            AddCell(gridRow, new DataGridViewTextBoxCell(),
                amount.ToString("F2"), cellBackgroundColor);
        }

        private void AddCell(DataGridViewRow gridRow, DataGridViewTextBoxCell cell, string text, Color cellBackgroundColor)
        {
            cell.Value = text;
            cell.Style.BackColor = cellBackgroundColor;
            gridRow.Cells.Add(cell);
        }
    }
}
