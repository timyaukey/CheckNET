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
            mData.Load();
            DisplayData();
            SetCellDetailVisiblity(false);
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
                AddGridRow<SplitDetailRow, SplitDetailCell>(row, Color.White);
            }
            foreach (var row in mData.BudgetedIncome)
            {
                AddGridRow(row, row.HasUnalignedPeriods ? Color.SandyBrown : Color.White);
            }
            AddGridRow<TotalRow, DataCell>(mData.TotalIncome, Color.LightGray);
            foreach (var row in mData.UnbudgetedExpenses)
            {
                AddGridRow<SplitDetailRow, SplitDetailCell>(row, Color.White);
            }
            foreach (var row in mData.BudgetedExpenses)
            {
                AddGridRow(row, row.HasUnalignedPeriods ? Color.SandyBrown : Color.White);
            }
            AddGridRow<TotalRow, DataCell>(mData.TotalExpense, Color.LightGray);
            AddGridRow<TotalRow, DataCell>(mData.NetProfit, Color.LightGreen);
            AddGridRow<TotalRow, DataCell>(mData.RunningBalance, Color.LightBlue);
            if (mData.HasUnalignedBudgetPeriods)
                mHostUI.ErrorMessageBox("One or more budget rows has budget transaction(s) whose period does not fit in a single dashboard column. " + 
                    "All such budget rows are highlighted in light brown.");
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

        private void AddGridRow<TRow, TCell>(TRow row, Color rowBackgroundColor)
            where TRow : DataRow<TCell>
            where TCell : DataCell, new()
        {
            DataGridViewRow gridRow = new DataGridViewRow();
            gridRow.Tag = row;
            AddTextCell(gridRow, row.Label, rowBackgroundColor);
            AddTextCell(gridRow, row.Sequence, rowBackgroundColor);
            AddDecimalCell(gridRow, row.RowTotal.CellAmount, rowBackgroundColor);
            for (int periodIndex = 0; periodIndex < mData.PeriodCount; periodIndex++)
            {
                AddDecimalCell(gridRow, row.Cells[periodIndex].CellAmount, rowBackgroundColor);
            }
            grdMain.Rows.Add(gridRow);
        }

        private void AddGridRow(BudgetDetailRow row, Color rowBackgroundColor)
        {
            DataGridViewRow gridRow = new DataGridViewRow();
            gridRow.Tag = row;
            AddTextCell(gridRow, row.Label, rowBackgroundColor);
            AddTextCell(gridRow, row.Sequence, rowBackgroundColor);
            AddDecimalCell(gridRow, row.RowTotal.CellAmount, rowBackgroundColor);
            for (int periodIndex = 0; periodIndex < mData.PeriodCount; periodIndex++)
            {
                AddBudgetCell(gridRow, row.Cells[periodIndex], rowBackgroundColor);
            }
            grdMain.Rows.Add(gridRow);
        }

        private void AddTextCell(DataGridViewRow gridRow, string text, Color cellBackgroundColor)
        {
            AddCell(gridRow, new DataGridViewTextBoxCell(),
                text, cellBackgroundColor);
        }

        private void AddBudgetCell(DataGridViewRow gridRow, BudgetDetailCell dataCell, Color cellBackgroundColor)
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
                ShowSplitCell(row.Tag as SplitDetailRow, e.ColumnIndex);
            }
            else if (row.Tag is BudgetDetailRow)
            {
                ShowBudgetCell(row.Tag as BudgetDetailRow, e.ColumnIndex);
                
            }
            else
                CheckCellDetailVisibility(false);
        }

        private void ShowSplitCell(SplitDetailRow row, int columnIndex)
        {
            if (columnIndex >= NonPeriodColumns)
            {
                SplitDetailCell cell = row.Cells[columnIndex - NonPeriodColumns];
                StartShowCell(row, columnIndex, "Category");
                lblDashboardAmount.Text = "Amount: " + cell.CellAmount.ToString("F2");
                lblBudgetLimit.Text = "";
                lblBudgetApplied.Text = "";
                foreach (TrxSplit split in cell.Details)
                {
                    ShowDetailValues(split.objParent.datDate, split.objParent.strDescription, split.curAmount);
                }
            }
            else
            {
                CheckCellDetailVisibility(false);
            }
        }

        private void ShowBudgetCell(BudgetDetailRow row, int columnIndex)
        {
            if (columnIndex >= NonPeriodColumns)
            {
                BudgetDetailCell cell = row.Cells[columnIndex - NonPeriodColumns];
                StartShowCell(row, columnIndex, "Budget");
                lblDashboardAmount.Text = "Dashboard Amount: " + cell.CellAmount.ToString("F2");
                lblBudgetLimit.Text = "Budget Limit: " + cell.BudgetLimit.ToString("F2");
                lblBudgetApplied.Text = "Budget Applied: " + cell.BudgetApplied.ToString("F2");
                foreach (BudgetTrx budget in cell.Details)
                {
                    foreach (TrxSplit split in budget.colAppliedSplits)
                    {
                        ShowDetailValues(split.objParent.datDate, split.objParent.strDescription, split.curAmount);
                    }
                }
            }
            else
            {
                CheckCellDetailVisibility(false);
            }
        }

        private void StartShowCell<TCell>(DataRow<TCell> row, int columnIndex, string labelType)
            where TCell : DataCell, new()
        {
            CheckCellDetailVisibility(true);
            lblRowLabel.Text = labelType + ": " + row.Label;
            lblRowSequence.Text = "Sequence: " + row.Sequence;
            lblColumnDate.Text = "Period Starts: " + grdMain.Columns[columnIndex].HeaderText;
            lvwDetails.Items.Clear();
        }

        private void ShowDetailValues(DateTime trxDate, string descr, decimal amount)
        {
            lvwDetails.Items.Add(new ListViewItem(new string[] { trxDate.ToString("MM/dd/yy"), descr, amount.ToString("F2") }));
        }

        private void CheckCellDetailVisibility(bool showDetail)
        {
            if (lvwDetails.Visible != showDetail)
            {
                SetCellDetailVisiblity(showDetail);
            }
        }

        private void SetCellDetailVisiblity(bool showDetail)
        {
            lblRowLabel.Visible = showDetail;
            lblRowSequence.Visible = showDetail;
            lblColumnDate.Visible = showDetail;
            lblBudgetLimit.Visible = showDetail;
            lblBudgetApplied.Visible = showDetail;
            lblDashboardAmount.Visible = showDetail;
            lvwDetails.Visible = showDetail;
        }
    }
}
