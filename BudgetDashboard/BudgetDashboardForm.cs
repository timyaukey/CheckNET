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
        private const int NonPeriodColumns = 3;

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
            this.Text = "Budget Dashboard - " + mData.Account.strTitle;
            grdMain.ColumnCount = mData.PeriodCount + 3;
            ConfigureColumn(0, "Category", 160, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft);
            ConfigureColumn(1, "Sequence", 160, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleLeft);
            ConfigureColumn(2, "Row Total", 100, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.MiddleRight);
            DateTime periodStart = mData.StartDate;
            for (int colIndex = 1; colIndex <= mData.PeriodCount; colIndex++)
            {
                ConfigureColumn(colIndex + 2, periodStart.ToShortDateString(), 80, DataGridViewContentAlignment.MiddleRight, DataGridViewContentAlignment.MiddleRight);
                periodStart = periodStart.AddDays(mData.PeriodDays);
            }
            grdMain.Columns[0].Frozen = true;
            grdMain.Columns[1].Frozen = true;
            grdMain.Columns[2].Frozen = true;
            foreach (var row in mData.UnbudgetedIncome)
            {
                AddGridRow<SplitDetailRow, SplitDetailCell>(row, Color.White, false);
            }
            foreach (var row in mData.BudgetedIncome)
            {
                AddGridRow<BudgetDetailRow, BudgetDetailCell>(row, row.HasUnalignedPeriods ? Color.Red : Color.White, true);
            }
            AddGridRow<TotalRow, DataCell>(mData.TotalIncome, Color.LightGray, false);
            foreach (var row in mData.UnbudgetedExpenses)
            {
                AddGridRow<SplitDetailRow, SplitDetailCell>(row, Color.White, false);
            }
            foreach (var row in mData.BudgetedExpenses)
            {
                AddGridRow<BudgetDetailRow, BudgetDetailCell>(row, row.HasUnalignedPeriods ? Color.Red : Color.White, true);
            }
            AddGridRow<TotalRow, DataCell>(mData.TotalExpense, Color.LightGray, false);
            AddGridRow<TotalRow, DataCell>(mData.NetProfit, Color.LightGreen, false);
            if (mData.HasUnalignedBudgetPeriods)
                mHostUI.ErrorMessageBox("One or more budget rows has budget transaction(s) whose period does not fit in a single dashboard column. " + 
                    "All such budget rows are highlighted in red.");
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

        private void AddGridRow<TRow, TCell>(TRow row, Color rowBackgroundColor, bool useBudgetCell)
            where TRow : DataRow<TCell>
            where TCell : DataCell, new()
        {
            DataGridViewRow gridRow = new DataGridViewRow();
            gridRow.Tag = row;
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

        private void grdMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = grdMain.Rows[e.RowIndex];
            if (row.Tag is SplitDetailRow)
            {
                SplitDetailRow detailRow = row.Tag as SplitDetailRow;
                ShowGridCell<SplitDetailRow, SplitDetailCell>(detailRow, e.ColumnIndex, "Category");
            }
            else if (row.Tag is BudgetDetailRow)
            {
                BudgetDetailRow budgetRow = row.Tag as BudgetDetailRow;
                ShowGridCell<BudgetDetailRow, BudgetDetailCell>(budgetRow, e.ColumnIndex, "Budget");
                
            }
        }

        private void ShowGridCell<TRow, TCell>(TRow row, int columnIndex, string labelType)
            where TRow : DataRow<TCell>
            where TCell : DataCell, new()
        {
            if (columnIndex >= NonPeriodColumns)
            {
                TCell cell = row.Cells[columnIndex - NonPeriodColumns];
                lblRowLabel.Text = labelType + ": " + row.Label;
                lblRowSequence.Text = "Sequence: " + row.Sequence;
                lblColumnDate.Text = "Period Starts: " + grdMain.Columns[columnIndex].HeaderText;
                if (cell is SplitDetailCell)
                    ShowSplitCellDetails(cell as SplitDetailCell);
                else if (cell is BudgetDetailCell)
                    ShowBudgetDetailCell(cell as BudgetDetailCell);
            }
        }

        private void ShowSplitCellDetails(SplitDetailCell cell)
        {
            lvwDetails.Items.Clear();
            foreach(TrxSplit split in cell.Details)
            {
                ShowDetailValues(split.objParent.datDate, split.objParent.strDescription, split.curAmount);
            }
        }

        private void ShowBudgetDetailCell(BudgetDetailCell cell)
        {
            lvwDetails.Items.Clear();
            foreach (BudgetTrx budget in cell.Details)
            {
                foreach(TrxSplit split in budget.colAppliedSplits)
                {
                    ShowDetailValues(split.objParent.datDate, split.objParent.strDescription, split.curAmount);
                }
            }
        }

        private void ShowDetailValues(DateTime trxDate, string descr, decimal amount)
        {
            lvwDetails.Items.Add(new ListViewItem(new string[] { trxDate.ToString("MM/dd/yy"), descr, amount.ToString("F2") }));
        }
    }
}
