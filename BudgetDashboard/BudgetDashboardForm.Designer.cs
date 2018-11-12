namespace BudgetDashboard
{
    partial class BudgetDashboardForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grdMain = new System.Windows.Forms.DataGridView();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.lblDashboardAmount = new System.Windows.Forms.Label();
            this.lblBudgetApplied = new System.Windows.Forms.Label();
            this.lblBudgetLimit = new System.Windows.Forms.Label();
            this.lblTrxAmount = new System.Windows.Forms.Label();
            this.lvwDetails = new System.Windows.Forms.ListView();
            this.colDtlDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDtlDescr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDtlAmount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblColumnDate = new System.Windows.Forms.Label();
            this.lblRowSequence = new System.Windows.Forms.Label();
            this.lblRowLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdMain
            // 
            this.grdMain.AllowUserToAddRows = false;
            this.grdMain.AllowUserToDeleteRows = false;
            this.grdMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdMain.Location = new System.Drawing.Point(0, 0);
            this.grdMain.Name = "grdMain";
            this.grdMain.ReadOnly = true;
            this.grdMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdMain.Size = new System.Drawing.Size(914, 518);
            this.grdMain.TabIndex = 0;
            this.grdMain.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdMain_CellClick);
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(12, 12);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.grdMain);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.lblDashboardAmount);
            this.splitContainer.Panel2.Controls.Add(this.lblBudgetApplied);
            this.splitContainer.Panel2.Controls.Add(this.lblBudgetLimit);
            this.splitContainer.Panel2.Controls.Add(this.lblTrxAmount);
            this.splitContainer.Panel2.Controls.Add(this.lvwDetails);
            this.splitContainer.Panel2.Controls.Add(this.lblColumnDate);
            this.splitContainer.Panel2.Controls.Add(this.lblRowSequence);
            this.splitContainer.Panel2.Controls.Add(this.lblRowLabel);
            this.splitContainer.Size = new System.Drawing.Size(1253, 518);
            this.splitContainer.SplitterDistance = 914;
            this.splitContainer.TabIndex = 1;
            // 
            // lblDashboardAmount
            // 
            this.lblDashboardAmount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDashboardAmount.Location = new System.Drawing.Point(3, 411);
            this.lblDashboardAmount.Name = "lblDashboardAmount";
            this.lblDashboardAmount.Size = new System.Drawing.Size(291, 20);
            this.lblDashboardAmount.TabIndex = 7;
            this.lblDashboardAmount.Text = "(dashboard amount)";
            // 
            // lblBudgetApplied
            // 
            this.lblBudgetApplied.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBudgetApplied.Location = new System.Drawing.Point(3, 391);
            this.lblBudgetApplied.Name = "lblBudgetApplied";
            this.lblBudgetApplied.Size = new System.Drawing.Size(291, 20);
            this.lblBudgetApplied.TabIndex = 6;
            this.lblBudgetApplied.Text = "(budget applied)";
            // 
            // lblBudgetLimit
            // 
            this.lblBudgetLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBudgetLimit.Location = new System.Drawing.Point(3, 371);
            this.lblBudgetLimit.Name = "lblBudgetLimit";
            this.lblBudgetLimit.Size = new System.Drawing.Size(291, 20);
            this.lblBudgetLimit.TabIndex = 5;
            this.lblBudgetLimit.Text = "(budget limit)";
            // 
            // lblTrxAmount
            // 
            this.lblTrxAmount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTrxAmount.Location = new System.Drawing.Point(3, 351);
            this.lblTrxAmount.Name = "lblTrxAmount";
            this.lblTrxAmount.Size = new System.Drawing.Size(291, 20);
            this.lblTrxAmount.TabIndex = 4;
            this.lblTrxAmount.Text = "(register amount)";
            // 
            // lvwDetails
            // 
            this.lvwDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDtlDate,
            this.colDtlDescr,
            this.colDtlAmount});
            this.lvwDetails.Location = new System.Drawing.Point(5, 63);
            this.lvwDetails.Name = "lvwDetails";
            this.lvwDetails.Size = new System.Drawing.Size(326, 271);
            this.lvwDetails.TabIndex = 3;
            this.lvwDetails.UseCompatibleStateImageBehavior = false;
            this.lvwDetails.View = System.Windows.Forms.View.Details;
            // 
            // colDtlDate
            // 
            this.colDtlDate.Text = "Date";
            this.colDtlDate.Width = 64;
            // 
            // colDtlDescr
            // 
            this.colDtlDescr.Text = "Description";
            this.colDtlDescr.Width = 170;
            // 
            // colDtlAmount
            // 
            this.colDtlAmount.Text = "Amount";
            this.colDtlAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblColumnDate
            // 
            this.lblColumnDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColumnDate.Location = new System.Drawing.Point(2, 40);
            this.lblColumnDate.Name = "lblColumnDate";
            this.lblColumnDate.Size = new System.Drawing.Size(291, 20);
            this.lblColumnDate.TabIndex = 2;
            this.lblColumnDate.Text = "(date)";
            // 
            // lblRowSequence
            // 
            this.lblRowSequence.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRowSequence.Location = new System.Drawing.Point(2, 20);
            this.lblRowSequence.Name = "lblRowSequence";
            this.lblRowSequence.Size = new System.Drawing.Size(329, 20);
            this.lblRowSequence.TabIndex = 1;
            this.lblRowSequence.Text = "(row sequence)";
            // 
            // lblRowLabel
            // 
            this.lblRowLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRowLabel.Location = new System.Drawing.Point(3, 0);
            this.lblRowLabel.Name = "lblRowLabel";
            this.lblRowLabel.Size = new System.Drawing.Size(329, 20);
            this.lblRowLabel.TabIndex = 0;
            this.lblRowLabel.Text = "(row label)";
            // 
            // BudgetDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1277, 570);
            this.Controls.Add(this.splitContainer);
            this.Name = "BudgetDashboardForm";
            this.Text = "Budget Dashboard";
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdMain;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Label lblRowLabel;
        private System.Windows.Forms.Label lblRowSequence;
        private System.Windows.Forms.Label lblColumnDate;
        private System.Windows.Forms.ListView lvwDetails;
        private System.Windows.Forms.ColumnHeader colDtlDate;
        private System.Windows.Forms.ColumnHeader colDtlDescr;
        private System.Windows.Forms.ColumnHeader colDtlAmount;
        private System.Windows.Forms.Label lblDashboardAmount;
        private System.Windows.Forms.Label lblBudgetApplied;
        private System.Windows.Forms.Label lblBudgetLimit;
        private System.Windows.Forms.Label lblTrxAmount;
    }
}