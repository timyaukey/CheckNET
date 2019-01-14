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
        private BudgetDetailCell mSelectedBudgetCell;
        private BudgetDetailRow mSelectedBudgetRow;
        private int mSelectedBudgetColumn;
        private BudgetGridCell mSelectedBudgetGridCell;
        private const int NonPeriodColumns = 3;
        private BudgetTrx mBudgetToSubtractFrom;
        private decimal mAmountToSubtract;

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
            AddDecimalCell(gridRow, row.RowTotal, rowBackgroundColor);
            for (int periodIndex = 0; periodIndex < mData.PeriodCount; periodIndex++)
            {
                AddDecimalCell(gridRow, row.Cells[periodIndex], rowBackgroundColor);
            }
            grdMain.Rows.Add(gridRow);
        }

        private void AddGridRow(BudgetDetailRow row, Color rowBackgroundColor)
        {
            DataGridViewRow gridRow = new DataGridViewRow();
            gridRow.Tag = row;
            AddTextCell(gridRow, row.Label, rowBackgroundColor);
            AddTextCell(gridRow, row.Sequence, rowBackgroundColor);
            AddDecimalCell(gridRow, row.RowTotal, rowBackgroundColor);
            for (int periodIndex = 0; periodIndex < mData.PeriodCount; periodIndex++)
            {
                AddBudgetCell(gridRow, row.Cells[periodIndex], rowBackgroundColor);
            }
            grdMain.Rows.Add(gridRow);
        }

        private void AddTextCell(DataGridViewRow gridRow, string text, Color cellBackgroundColor)
        {
            AddCell(gridRow, new DataGridViewTextBoxCell(), text, cellBackgroundColor);
        }

        private void AddBudgetCell(DataGridViewRow gridRow, BudgetDetailCell dataCell, Color cellBackgroundColor)
        {
            AddCell(gridRow, new BudgetGridCell(dataCell), "", cellBackgroundColor);
        }

        private void AddDecimalCell(DataGridViewRow gridRow, DataCell dataCell, Color cellBackgroundColor)
        {
            AddCell(gridRow, new DataCellGridCell<DataCell>(dataCell), "", cellBackgroundColor);
        }

        private void AddCell(DataGridViewRow gridRow, DataGridViewTextBoxCell cell, string text, Color cellBackgroundColor)
        {
            cell.Value = text;
            cell.Style.BackColor = cellBackgroundColor;
            gridRow.Cells.Add(cell);
        }

        private void grdMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            mSelectedBudgetCell = null;
            mSelectedBudgetRow = null;
            mSelectedBudgetColumn = 0;
            mSelectedBudgetGridCell = null;
            DataGridViewRow row = grdMain.Rows[e.RowIndex];
            if (row.Tag is SplitDetailRow)
            {
                ShowSplitCell(row.Tag as SplitDetailRow, e.ColumnIndex);
            }
            else if (row.Tag is BudgetDetailRow)
            {
                ShowBudgetCellInPanel(row.Tag as BudgetDetailRow, e.ColumnIndex, row.Cells[e.ColumnIndex]);
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
                lblDashboardAmount.Text = "Dashboard Amount: " + cell.CellAmount.ToString("F2");
                lblGeneratedAmount.Text = "Generated Amount: " + cell.GeneratedAmount.ToString("F2");
                lblBudgetLimit.Text = "";
                lblBudgetApplied.Text = "";
                SetBudgetDetailVisibility(false);
                foreach (TrxSplit split in cell.Details)
                {
                    ShowDetailValues(split.objParent.datDate, split.objParent.strDescription, split.curAmount, null, null);
                }
            }
            else
            {
                CheckCellDetailVisibility(false);
            }
        }

        private void ShowBudgetCellInPanel(BudgetDetailRow row, int columnIndex, DataGridViewCell gridCell)
        {
            if (columnIndex >= NonPeriodColumns)
            {
                BudgetDetailCell cell = row.Cells[columnIndex - NonPeriodColumns];
                mSelectedBudgetCell = cell;
                mSelectedBudgetRow = row;
                mSelectedBudgetColumn = columnIndex;
                mSelectedBudgetGridCell = (BudgetGridCell)gridCell;
                StartShowCell(row, columnIndex, "Budget");
                lblDashboardAmount.Text = "Dashboard Amount: " + cell.CellAmount.ToString("F2");
                lblGeneratedAmount.Text = "Original Limit: " + cell.GeneratedAmount.ToString("F2");
                lblBudgetLimit.Text = "Current Limit: " + cell.BudgetLimit.ToString("F2");
                lblBudgetApplied.Text = "Amount Used: " + cell.BudgetApplied.ToString("F2");
                SetBudgetDetailVisibility(true);
                bool useGroups = false;
                if (OSFeature.Feature.IsPresent(OSFeature.Themes))
                    useGroups = true;
                lvwDetails.ShowGroups = useGroups;
                foreach (BudgetTrx budget in cell.Details)
                {
                    ListViewGroup currentGroup = null;
                    if (useGroups)
                    {
                        string originally = "";
                        if (budget.curGeneratedAmount != 0m && budget.curGeneratedAmount != budget.curBudgetLimit)
                            originally = " (orig $" + budget.curGeneratedAmount.ToString("F2") + ")";
                        currentGroup = new ListViewGroup(budget.datBudgetEnds.ToString("MM/dd/yy") + " " + 
                            budget.strDescription + " $" + budget.curBudgetLimit.ToString("F2") + originally);
                        lvwDetails.Groups.Add(currentGroup);
                    }
                    if (budget.colAppliedSplits.Count > 0)
                    {
                        foreach (TrxSplit split in budget.colAppliedSplits)
                        {
                            ShowDetailValues(split.objParent.datDate, split.objParent.strDescription, split.curAmount, currentGroup, budget);
                        }
                    }
                    else
                    {
                        ShowDetailValues("", "(unused budget)", "", currentGroup, budget);
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
            lvwDetails.Groups.Clear();
            lvwDetails.ShowGroups = false;
        }

        private void ShowDetailValues(DateTime trxDate, string descr, decimal amount, ListViewGroup group, object itemTag)
        {
            ShowDetailValues(trxDate.ToString("MM/dd/yy"), descr, amount.ToString("F2"), group, itemTag);
        }

        private void ShowDetailValues(string trxDateFormatted, string descr, string amountFormatted, ListViewGroup group, object itemTag)
        {
            ListViewItem item = new ListViewItem(new string[] { trxDateFormatted, descr, amountFormatted });
            item.Tag = itemTag;
            if (group != null)
                item.Group = group;
            lvwDetails.Items.Add(item);
        }

        private void CheckCellDetailVisibility(bool showDetail)
        {
            if (lvwDetails.Visible != showDetail)
            {
                SetCellDetailVisiblity(showDetail);
            }
        }

        private void SetCellDetailVisiblity(bool showControls)
        {
            lblRowLabel.Visible = showControls;
            lblRowSequence.Visible = showControls;
            lblColumnDate.Visible = showControls;
            lblBudgetLimit.Visible = showControls;
            lblBudgetApplied.Visible = showControls;
            lblDashboardAmount.Visible = showControls;
            lblGeneratedAmount.Visible = showControls;
            lvwDetails.Visible = showControls;
            SetBudgetDetailVisibility(showControls);
        }

        private void SetBudgetDetailVisibility(bool showControls)
        {
            lblAdjustment.Visible = showControls;
            txtAdjustment.Visible = showControls;
            btnAddAdj.Visible = showControls;
            btnSubAdj.Visible = showControls;
            btnSetAdj.Visible = showControls;
            btnCancelAdj.Visible = showControls;
            EnableAdjustmentButtons();
        }

        private void EnableAdjustmentButtons()
        {
            bool adjustmentIsPending = (mBudgetToSubtractFrom != null);
            btnAddAdj.Enabled = adjustmentIsPending;
            btnCancelAdj.Enabled = adjustmentIsPending;
            btnSubAdj.Enabled = !adjustmentIsPending;
            btnSetAdj.Enabled = !adjustmentIsPending;
            txtAdjustment.Enabled = !adjustmentIsPending;
        }

        private void btnSubAdj_Click(object sender, EventArgs e)
        {
            mBudgetToSubtractFrom = GetAdjustmentBudget();
            if (mBudgetToSubtractFrom == null)
                return;
            if (!TryGetAdjustment(out mAmountToSubtract, mBudgetToSubtractFrom))
                return;
            EnableAdjustmentButtons();
        }

        private void btnAddAdj_Click(object sender, EventArgs e)
        {
            BudgetTrx budgetTrx = GetAdjustmentBudget();
            if (budgetTrx == null)
                return;
            string moveMessage = "Moved " + mAmountToSubtract.ToString("F2") + " from" + Environment.NewLine + 
                mBudgetToSubtractFrom.strSummary() + Environment.NewLine +
                "to" + Environment.NewLine + 
                budgetTrx.strSummary() + ".";
            SetBudgetAmount(mBudgetToSubtractFrom, mBudgetToSubtractFrom.curBudgetLimit - mAmountToSubtract);
            SetBudgetAmount(budgetTrx, budgetTrx.curBudgetLimit + mAmountToSubtract);
            ClearPendingSubtraction();
            EnableAdjustmentButtons();
            RefreshGridTotals();
            ShowBudgetCellInPanel(mSelectedBudgetRow, mSelectedBudgetColumn, mSelectedBudgetGridCell);
            mHostUI.InfoMessageBox(moveMessage);
        }

        private void btnCancelAdj_Click(object sender, EventArgs e)
        {
            ClearPendingSubtraction();
            EnableAdjustmentButtons();
        }

        private void btnSetAdj_Click(object sender, EventArgs e)
        {
            BudgetTrx budgetTrx = GetAdjustmentBudget();
            if (budgetTrx == null)
                return;
            if (!TryGetAdjustment(out decimal adjAmount, budgetTrx))
                return;
            SetBudgetAmount(budgetTrx, adjAmount);
            RefreshGridTotals();
            ShowBudgetCellInPanel(mSelectedBudgetRow, mSelectedBudgetColumn, mSelectedBudgetGridCell);
            mHostUI.InfoMessageBox("Budget amount set to " + adjAmount.ToString("F2") + ".");
        }

        private void ClearPendingSubtraction()
        {
            mBudgetToSubtractFrom = null;
            mAmountToSubtract = 0m;
        }

        private void RefreshGridTotals()
        {
            mData.ComputeTotals();
            grdMain.Refresh();
        }

        private bool TryGetAdjustment(out decimal adjAmount, BudgetTrx target)
        {
            if (decimal.TryParse(txtAdjustment.Text, out adjAmount))
            {
                // Amount should have the same sign as specified budget limit.
                if (target.curBudgetLimit != 0m)
                {
                    if ((target.curBudgetLimit < 0m) != (adjAmount < 0m))
                        adjAmount = -adjAmount;
                }
                return true;
            }
            adjAmount = 0m;
            mHostUI.ErrorMessageBox("Invalid adjustment amount.");
            return false;
        }

        private BudgetTrx GetAdjustmentBudget()
        {
            if (mSelectedBudgetCell == null) // Should be impossible
                return null;
            if (mSelectedBudgetCell.Details.Count < 1)
            {
                mHostUI.ErrorMessageBox("The selected cell has no budget transaction to adjust.");
                return null;
            }
            if (mSelectedBudgetCell.Details.Count == 1)
                return mSelectedBudgetCell.Details[0];
            if (lvwDetails.SelectedItems.Count < 1)
            {
                mHostUI.ErrorMessageBox("Select a detail row belonging to the budget you want to change.");
                return null;
            }
            return ((BudgetTrx)lvwDetails.SelectedItems[0].Tag);
        }

        private void SetBudgetAmount(BudgetTrx budgetTrx, decimal newAmount)
        {
            BudgetTrxManager mgr = budgetTrx.objGetTrxManager();
            mgr.UpdateStart();
            mgr.objTrx.UpdateStartBudget(budgetTrx.datDate, budgetTrx.strDescription, budgetTrx.strMemo,
                budgetTrx.blnAwaitingReview, false, budgetTrx.intRepeatSeq, budgetTrx.strRepeatKey,
                newAmount, budgetTrx.datBudgetStarts, budgetTrx.strBudgetKey, budgetTrx.objReg.datOldestBudgetEndAllowed);
            mgr.UpdateEnd(new LogChange(), "BudgetDashboard.Adjustment");
        }
    }
}
