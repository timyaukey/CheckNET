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
            grdMain.ColumnCount = mData.PeriodCount + 1;
            ConfigureColumn(0, "Category", 200, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft);
            DateTime periodStart = mData.StartDate;
            for (int colIndex = 1; colIndex <= mData.PeriodCount; colIndex++)
            {
                ConfigureColumn(colIndex, periodStart.ToShortDateString(), 100, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.MiddleRight);
                periodStart = periodStart.AddDays(mData.PeriodDays);
            }
            grdMain.Columns[0].Frozen = true;
            foreach(var row in mData.UnbudgetedIncome)
            {
                AddGridRow<SplitDetailRow, SplitDetailCell>(row, Color.White);
            }
            foreach (var row in mData.BudgetedIncome)
            {
                AddGridRow<BudgetDetailRow, BudgetDetailCell>(row, Color.White);
            }
            AddGridRow<TotalRow, DataCell>(mData.TotalIncome, Color.LightGray);
            foreach (var row in mData.UnbudgetedExpenses)
            {
                AddGridRow<SplitDetailRow, SplitDetailCell>(row, Color.White);
            }
            foreach(var row in mData.BudgetedExpenses)
            {
                AddGridRow<BudgetDetailRow, BudgetDetailCell>(row, Color.White);
            }
            AddGridRow<TotalRow, DataCell>(mData.TotalExpense, Color.LightGray);
            AddGridRow<TotalRow, DataCell>(mData.NetProfit, Color.LightGreen);
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

        private void AddGridRow<TRow, TCell2>(TRow row, Color rowColor)
            where TRow : DataRow<TCell2>
            where TCell2 : DataCell, new()
        {
            DataGridViewRow gridRow = new DataGridViewRow();
            AddCell(gridRow, row.Label, rowColor);
            for (int periodIndex = 0; periodIndex < mData.PeriodCount; periodIndex++)
            {
                AddCell(gridRow, row.Cells[periodIndex].Amount.ToString("F2"), rowColor);
            }
            grdMain.Rows.Add(gridRow);
        }

        private void AddCell(DataGridViewRow gridRow, string text, Color cellColor)
        {
            DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
            cell.Value = text;
            cell.Style.BackColor = cellColor;
            gridRow.Cells.Add(cell);
        }
    }
}
