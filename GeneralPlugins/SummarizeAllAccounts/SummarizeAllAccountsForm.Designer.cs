namespace Willowsoft.CheckBook.GeneralPlugins
{
    partial class SummarizeAllAccountsForm
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
            this.lblEndDate = new System.Windows.Forms.Label();
            this.ctlEndDate = new System.Windows.Forms.DateTimePicker();
            this.lvwAccounts = new System.Windows.Forms.ListView();
            this.colAcctName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRegularBalance = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPersonalBalance = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTotalBalance = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFakeTrx = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSummarize = new System.Windows.Forms.Button();
            this.chkFakeInBalance = new System.Windows.Forms.CheckBox();
            this.lblIncludes = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(12, 15);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(55, 13);
            this.lblEndDate.TabIndex = 0;
            this.lblEndDate.Text = "End Date:";
            // 
            // ctlEndDate
            // 
            this.ctlEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ctlEndDate.Location = new System.Drawing.Point(83, 9);
            this.ctlEndDate.Name = "ctlEndDate";
            this.ctlEndDate.Size = new System.Drawing.Size(122, 20);
            this.ctlEndDate.TabIndex = 1;
            this.ctlEndDate.Value = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            // 
            // lvwAccounts
            // 
            this.lvwAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwAccounts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAcctName,
            this.colRegularBalance,
            this.colPersonalBalance,
            this.colTotalBalance,
            this.colFakeTrx});
            this.lvwAccounts.FullRowSelect = true;
            this.lvwAccounts.GridLines = true;
            this.lvwAccounts.HideSelection = false;
            this.lvwAccounts.Location = new System.Drawing.Point(12, 35);
            this.lvwAccounts.Name = "lvwAccounts";
            this.lvwAccounts.Size = new System.Drawing.Size(712, 348);
            this.lvwAccounts.TabIndex = 3;
            this.lvwAccounts.UseCompatibleStateImageBehavior = false;
            this.lvwAccounts.View = System.Windows.Forms.View.Details;
            // 
            // colAcctName
            // 
            this.colAcctName.Text = "Account Name";
            this.colAcctName.Width = 300;
            // 
            // colRegularBalance
            // 
            this.colRegularBalance.Text = "Regular Balance";
            this.colRegularBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colRegularBalance.Width = 100;
            // 
            // colPersonalBalance
            // 
            this.colPersonalBalance.Text = "Personal Balance";
            this.colPersonalBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colPersonalBalance.Width = 100;
            // 
            // colTotalBalance
            // 
            this.colTotalBalance.Text = "Total Balance";
            this.colTotalBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colTotalBalance.Width = 100;
            // 
            // colFakeTrx
            // 
            this.colFakeTrx.Text = "Fake Trx?";
            this.colFakeTrx.Width = 80;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(636, 389);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSummarize
            // 
            this.btnSummarize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSummarize.Location = new System.Drawing.Point(542, 389);
            this.btnSummarize.Name = "btnSummarize";
            this.btnSummarize.Size = new System.Drawing.Size(88, 23);
            this.btnSummarize.TabIndex = 4;
            this.btnSummarize.Text = "Summarize";
            this.btnSummarize.UseVisualStyleBackColor = true;
            this.btnSummarize.Click += new System.EventHandler(this.btnSummarize_Click);
            // 
            // chkFakeInBalance
            // 
            this.chkFakeInBalance.AutoSize = true;
            this.chkFakeInBalance.Location = new System.Drawing.Point(235, 14);
            this.chkFakeInBalance.Name = "chkFakeInBalance";
            this.chkFakeInBalance.Size = new System.Drawing.Size(228, 17);
            this.chkFakeInBalance.TabIndex = 2;
            this.chkFakeInBalance.Text = "Include fake and generated trx in balances";
            this.chkFakeInBalance.UseVisualStyleBackColor = true;
            // 
            // lblIncludes
            // 
            this.lblIncludes.AutoSize = true;
            this.lblIncludes.Location = new System.Drawing.Point(12, 394);
            this.lblIncludes.Name = "lblIncludes";
            this.lblIncludes.Size = new System.Drawing.Size(214, 13);
            this.lblIncludes.TabIndex = 6;
            this.lblIncludes.Text = "Includes bank and replica transactions only.";
            // 
            // SummarizeAllAccountsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(736, 424);
            this.Controls.Add(this.lblIncludes);
            this.Controls.Add(this.chkFakeInBalance);
            this.Controls.Add(this.btnSummarize);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lvwAccounts);
            this.Controls.Add(this.ctlEndDate);
            this.Controls.Add(this.lblEndDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SummarizeAllAccountsForm";
            this.Text = "Summarize All Accounts";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker ctlEndDate;
        private System.Windows.Forms.ListView lvwAccounts;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSummarize;
        private System.Windows.Forms.ColumnHeader colAcctName;
        private System.Windows.Forms.ColumnHeader colRegularBalance;
        private System.Windows.Forms.ColumnHeader colPersonalBalance;
        private System.Windows.Forms.ColumnHeader colTotalBalance;
        private System.Windows.Forms.ColumnHeader colFakeTrx;
        private System.Windows.Forms.CheckBox chkFakeInBalance;
        private System.Windows.Forms.Label lblIncludes;
    }
}