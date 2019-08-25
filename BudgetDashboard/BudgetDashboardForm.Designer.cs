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
            this.btnCancelAdj = new System.Windows.Forms.Button();
            this.lblGeneratedAmount = new System.Windows.Forms.Label();
            this.lblBudgetLimit = new System.Windows.Forms.Label();
            this.btnSetAdj = new System.Windows.Forms.Button();
            this.btnSubAdj = new System.Windows.Forms.Button();
            this.btnAddAdj = new System.Windows.Forms.Button();
            this.txtAdjustment = new System.Windows.Forms.TextBox();
            this.lblAdjustment = new System.Windows.Forms.Label();
            this.lblDashboardAmount = new System.Windows.Forms.Label();
            this.lblBudgetApplied = new System.Windows.Forms.Label();
            this.lvwDetails = new System.Windows.Forms.ListView();
            this.colDtlDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDtlNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDtlDescr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDtlAmount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDtlAccount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
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
            this.grdMain.MultiSelect = false;
            this.grdMain.Name = "grdMain";
            this.grdMain.ReadOnly = true;
            this.grdMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdMain.Size = new System.Drawing.Size(731, 548);
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
            this.splitContainer.Panel2.Controls.Add(this.btnCancelAdj);
            this.splitContainer.Panel2.Controls.Add(this.lblGeneratedAmount);
            this.splitContainer.Panel2.Controls.Add(this.lblBudgetLimit);
            this.splitContainer.Panel2.Controls.Add(this.btnSetAdj);
            this.splitContainer.Panel2.Controls.Add(this.btnSubAdj);
            this.splitContainer.Panel2.Controls.Add(this.btnAddAdj);
            this.splitContainer.Panel2.Controls.Add(this.txtAdjustment);
            this.splitContainer.Panel2.Controls.Add(this.lblAdjustment);
            this.splitContainer.Panel2.Controls.Add(this.lblDashboardAmount);
            this.splitContainer.Panel2.Controls.Add(this.lblBudgetApplied);
            this.splitContainer.Panel2.Controls.Add(this.lvwDetails);
            this.splitContainer.Panel2.Controls.Add(this.lblColumnDate);
            this.splitContainer.Panel2.Controls.Add(this.lblRowSequence);
            this.splitContainer.Panel2.Controls.Add(this.lblRowLabel);
            this.splitContainer.Size = new System.Drawing.Size(1190, 548);
            this.splitContainer.SplitterDistance = 731;
            this.splitContainer.TabIndex = 1;
            // 
            // btnCancelAdj
            // 
            this.btnCancelAdj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelAdj.Location = new System.Drawing.Point(341, 522);
            this.btnCancelAdj.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnCancelAdj.Name = "btnCancelAdj";
            this.btnCancelAdj.Size = new System.Drawing.Size(54, 23);
            this.btnCancelAdj.TabIndex = 14;
            this.btnCancelAdj.Text = "Cancel";
            this.btnCancelAdj.UseVisualStyleBackColor = true;
            this.btnCancelAdj.Click += new System.EventHandler(this.btnCancelAdj_Click);
            // 
            // lblGeneratedAmount
            // 
            this.lblGeneratedAmount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGeneratedAmount.Location = new System.Drawing.Point(3, 459);
            this.lblGeneratedAmount.Name = "lblGeneratedAmount";
            this.lblGeneratedAmount.Size = new System.Drawing.Size(411, 20);
            this.lblGeneratedAmount.TabIndex = 13;
            this.lblGeneratedAmount.Text = "(generated amount)";
            // 
            // lblBudgetLimit
            // 
            this.lblBudgetLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBudgetLimit.Location = new System.Drawing.Point(3, 479);
            this.lblBudgetLimit.Name = "lblBudgetLimit";
            this.lblBudgetLimit.Size = new System.Drawing.Size(279, 20);
            this.lblBudgetLimit.TabIndex = 5;
            this.lblBudgetLimit.Text = "(budget limit)";
            // 
            // btnSetAdj
            // 
            this.btnSetAdj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetAdj.Location = new System.Drawing.Point(397, 522);
            this.btnSetAdj.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnSetAdj.Name = "btnSetAdj";
            this.btnSetAdj.Size = new System.Drawing.Size(54, 23);
            this.btnSetAdj.TabIndex = 12;
            this.btnSetAdj.Text = "Set";
            this.btnSetAdj.UseVisualStyleBackColor = true;
            this.btnSetAdj.Click += new System.EventHandler(this.btnSetAdj_Click);
            // 
            // btnSubAdj
            // 
            this.btnSubAdj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubAdj.Location = new System.Drawing.Point(200, 522);
            this.btnSubAdj.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnSubAdj.Name = "btnSubAdj";
            this.btnSubAdj.Size = new System.Drawing.Size(82, 23);
            this.btnSubAdj.TabIndex = 11;
            this.btnSubAdj.Text = "Subtract";
            this.btnSubAdj.UseVisualStyleBackColor = true;
            this.btnSubAdj.Click += new System.EventHandler(this.btnSubAdj_Click);
            // 
            // btnAddAdj
            // 
            this.btnAddAdj.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAdj.Location = new System.Drawing.Point(285, 522);
            this.btnAddAdj.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.btnAddAdj.Name = "btnAddAdj";
            this.btnAddAdj.Size = new System.Drawing.Size(54, 23);
            this.btnAddAdj.TabIndex = 10;
            this.btnAddAdj.Text = "Add";
            this.btnAddAdj.UseVisualStyleBackColor = true;
            this.btnAddAdj.Click += new System.EventHandler(this.btnAddAdj_Click);
            // 
            // txtAdjustment
            // 
            this.txtAdjustment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAdjustment.Location = new System.Drawing.Point(397, 499);
            this.txtAdjustment.Name = "txtAdjustment";
            this.txtAdjustment.Size = new System.Drawing.Size(54, 20);
            this.txtAdjustment.TabIndex = 9;
            // 
            // lblAdjustment
            // 
            this.lblAdjustment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAdjustment.Location = new System.Drawing.Point(344, 502);
            this.lblAdjustment.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblAdjustment.Name = "lblAdjustment";
            this.lblAdjustment.Size = new System.Drawing.Size(50, 18);
            this.lblAdjustment.TabIndex = 8;
            this.lblAdjustment.Text = "Amount:";
            this.lblAdjustment.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblDashboardAmount
            // 
            this.lblDashboardAmount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDashboardAmount.Location = new System.Drawing.Point(3, 439);
            this.lblDashboardAmount.Name = "lblDashboardAmount";
            this.lblDashboardAmount.Size = new System.Drawing.Size(411, 20);
            this.lblDashboardAmount.TabIndex = 7;
            this.lblDashboardAmount.Text = "(dashboard amount)";
            // 
            // lblBudgetApplied
            // 
            this.lblBudgetApplied.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBudgetApplied.Location = new System.Drawing.Point(3, 499);
            this.lblBudgetApplied.Name = "lblBudgetApplied";
            this.lblBudgetApplied.Size = new System.Drawing.Size(279, 20);
            this.lblBudgetApplied.TabIndex = 6;
            this.lblBudgetApplied.Text = "(budget applied)";
            // 
            // lvwDetails
            // 
            this.lvwDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDtlDate,
            this.colDtlNum,
            this.colDtlDescr,
            this.colDtlAmount,
            this.colDtlAccount});
            this.lvwDetails.FullRowSelect = true;
            this.lvwDetails.HideSelection = false;
            this.lvwDetails.Location = new System.Drawing.Point(5, 63);
            this.lvwDetails.Name = "lvwDetails";
            this.lvwDetails.Size = new System.Drawing.Size(446, 373);
            this.lvwDetails.TabIndex = 3;
            this.lvwDetails.UseCompatibleStateImageBehavior = false;
            this.lvwDetails.View = System.Windows.Forms.View.Details;
            // 
            // colDtlDate
            // 
            this.colDtlDate.Text = "Date";
            this.colDtlDate.Width = 64;
            // 
            // colDtlNum
            // 
            this.colDtlNum.Text = "Num";
            this.colDtlNum.Width = 50;
            // 
            // colDtlDescr
            // 
            this.colDtlDescr.Text = "Description";
            this.colDtlDescr.Width = 150;
            // 
            // colDtlAmount
            // 
            this.colDtlAmount.Text = "Amount";
            this.colDtlAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // colDtlAccount
            // 
            this.colDtlAccount.Text = "Account";
            this.colDtlAccount.Width = 100;
            // 
            // lblColumnDate
            // 
            this.lblColumnDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColumnDate.Location = new System.Drawing.Point(3, 40);
            this.lblColumnDate.Name = "lblColumnDate";
            this.lblColumnDate.Size = new System.Drawing.Size(411, 20);
            this.lblColumnDate.TabIndex = 2;
            this.lblColumnDate.Text = "(date)";
            // 
            // lblRowSequence
            // 
            this.lblRowSequence.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRowSequence.Location = new System.Drawing.Point(3, 20);
            this.lblRowSequence.Name = "lblRowSequence";
            this.lblRowSequence.Size = new System.Drawing.Size(449, 20);
            this.lblRowSequence.TabIndex = 1;
            this.lblRowSequence.Text = "(row sequence)";
            // 
            // lblRowLabel
            // 
            this.lblRowLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRowLabel.Location = new System.Drawing.Point(3, 0);
            this.lblRowLabel.Name = "lblRowLabel";
            this.lblRowLabel.Size = new System.Drawing.Size(449, 20);
            this.lblRowLabel.TabIndex = 0;
            this.lblRowLabel.Text = "(row label)";
            // 
            // BudgetDashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1214, 600);
            this.Controls.Add(this.splitContainer);
            this.Name = "BudgetDashboardForm";
            this.Text = "Budget Dashboard";
            ((System.ComponentModel.ISupportInitialize)(this.grdMain)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
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
        private System.Windows.Forms.Button btnSetAdj;
        private System.Windows.Forms.Button btnSubAdj;
        private System.Windows.Forms.Button btnAddAdj;
        private System.Windows.Forms.TextBox txtAdjustment;
        private System.Windows.Forms.Label lblAdjustment;
        private System.Windows.Forms.Label lblGeneratedAmount;
        private System.Windows.Forms.Button btnCancelAdj;
        private System.Windows.Forms.ColumnHeader colDtlNum;
        private System.Windows.Forms.ColumnHeader colDtlAccount;
    }
}