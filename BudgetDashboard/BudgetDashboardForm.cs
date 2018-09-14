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
            ConfigureColumn(0, "Category", 200);
            DateTime periodStart = mData.StartDate;
            for (int colIndex = 1; colIndex <= mData.PeriodCount; colIndex++)
            {
                ConfigureColumn(colIndex, periodStart.ToShortDateString(), 100);
                periodStart = periodStart.AddDays(mData.PeriodDays);
            }
            foreach(var row in mData.UnbudgetedIncome)
            {
                AddGridRow<SplitDetailRow, SplitDetailCell>(row);
            }
            foreach (var row in mData.BudgetedIncome)
            {
                AddGridRow<BudgetDetailRow, BudgetDetailCell>(row);
            }
            AddGridRow<TotalRow, DataCell>(mData.TotalIncome);
            foreach (var row in mData.UnbudgetedExpenses)
            {
                AddGridRow<SplitDetailRow, SplitDetailCell>(row);
            }
            foreach(var row in mData.BudgetedExpenses)
            {
                AddGridRow<BudgetDetailRow, BudgetDetailCell>(row);
            }
            AddGridRow<TotalRow, DataCell>(mData.TotalExpense);
            AddGridRow<TotalRow, DataCell>(mData.NetProfit);
        }

        private void ConfigureColumn(int colIndex, string title, int width)
        {
            var col = grdMain.Columns[colIndex];
            col.HeaderText = title;
            col.Width = width;
        }

        private void AddGridRow<TRow, TCell2>(TRow row)
            where TRow : DataRow<TCell2>
            where TCell2 : DataCell, new()
        {
            DataGridViewRow gridRow = new DataGridViewRow();
            AddCell(gridRow, row.Label);
            for (int periodIndex = 0; periodIndex < mData.PeriodCount; periodIndex++)
            {
                AddCell(gridRow, row.Cells[periodIndex].BudgetApplied.ToString("C2"));
            }
            grdMain.Rows.Add(gridRow);
        }

        private void AddCell(DataGridViewRow gridRow, string text)
        {
            DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
            cell.Value = text;
            gridRow.Cells.Add(cell);
        }
    }
}
