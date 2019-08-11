namespace BudgetDashboard
{
    partial class BudgetSpecsForm
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
            this.ctlStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.lblPeriodDays = new System.Windows.Forms.Label();
            this.txtPeriodDays = new System.Windows.Forms.TextBox();
            this.txtPeriodCount = new System.Windows.Forms.TextBox();
            this.lblPeriodCount = new System.Windows.Forms.Label();
            this.cmdOkay = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cboBudgetType = new System.Windows.Forms.ComboBox();
            this.lblBudgetType = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ctlStartDate
            // 
            this.ctlStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ctlStartDate.Location = new System.Drawing.Point(167, 15);
            this.ctlStartDate.Name = "ctlStartDate";
            this.ctlStartDate.Size = new System.Drawing.Size(117, 20);
            this.ctlStartDate.TabIndex = 0;
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(12, 22);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(92, 13);
            this.lblStartDate.TabIndex = 1;
            this.lblStartDate.Text = "First Period Starts:";
            // 
            // lblPeriodDays
            // 
            this.lblPeriodDays.AutoSize = true;
            this.lblPeriodDays.Location = new System.Drawing.Point(12, 44);
            this.lblPeriodDays.Name = "lblPeriodDays";
            this.lblPeriodDays.Size = new System.Drawing.Size(123, 13);
            this.lblPeriodDays.TabIndex = 2;
            this.lblPeriodDays.Text = "Days Per Budget Period:";
            // 
            // txtPeriodDays
            // 
            this.txtPeriodDays.Location = new System.Drawing.Point(167, 41);
            this.txtPeriodDays.Name = "txtPeriodDays";
            this.txtPeriodDays.Size = new System.Drawing.Size(53, 20);
            this.txtPeriodDays.TabIndex = 3;
            this.txtPeriodDays.Text = "14";
            // 
            // txtPeriodCount
            // 
            this.txtPeriodCount.Location = new System.Drawing.Point(167, 67);
            this.txtPeriodCount.Name = "txtPeriodCount";
            this.txtPeriodCount.Size = new System.Drawing.Size(53, 20);
            this.txtPeriodCount.TabIndex = 5;
            this.txtPeriodCount.Text = "26";
            // 
            // lblPeriodCount
            // 
            this.lblPeriodCount.AutoSize = true;
            this.lblPeriodCount.Location = new System.Drawing.Point(12, 70);
            this.lblPeriodCount.Name = "lblPeriodCount";
            this.lblPeriodCount.Size = new System.Drawing.Size(136, 13);
            this.lblPeriodCount.TabIndex = 4;
            this.lblPeriodCount.Text = "Number Of Budget Periods:";
            // 
            // cmdOkay
            // 
            this.cmdOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOkay.Location = new System.Drawing.Point(192, 141);
            this.cmdOkay.Name = "cmdOkay";
            this.cmdOkay.Size = new System.Drawing.Size(75, 23);
            this.cmdOkay.TabIndex = 6;
            this.cmdOkay.Text = "Ok";
            this.cmdOkay.UseVisualStyleBackColor = true;
            this.cmdOkay.Click += new System.EventHandler(this.cmdOkay_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Location = new System.Drawing.Point(273, 141);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 7;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cboBudgetType
            // 
            this.cboBudgetType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboBudgetType.FormattingEnabled = true;
            this.cboBudgetType.Location = new System.Drawing.Point(167, 93);
            this.cboBudgetType.Name = "cboBudgetType";
            this.cboBudgetType.Size = new System.Drawing.Size(181, 21);
            this.cboBudgetType.TabIndex = 8;
            // 
            // lblBudgetType
            // 
            this.lblBudgetType.AutoSize = true;
            this.lblBudgetType.Location = new System.Drawing.Point(12, 96);
            this.lblBudgetType.Name = "lblBudgetType";
            this.lblBudgetType.Size = new System.Drawing.Size(71, 13);
            this.lblBudgetType.TabIndex = 9;
            this.lblBudgetType.Text = "Budget Type:";
            // 
            // BudgetSpecsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 176);
            this.Controls.Add(this.lblBudgetType);
            this.Controls.Add(this.cboBudgetType);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOkay);
            this.Controls.Add(this.txtPeriodCount);
            this.Controls.Add(this.lblPeriodCount);
            this.Controls.Add(this.txtPeriodDays);
            this.Controls.Add(this.lblPeriodDays);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.ctlStartDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BudgetSpecsForm";
            this.Text = "Budget Dashboard Specs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker ctlStartDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Label lblPeriodDays;
        private System.Windows.Forms.TextBox txtPeriodDays;
        private System.Windows.Forms.TextBox txtPeriodCount;
        private System.Windows.Forms.Label lblPeriodCount;
        private System.Windows.Forms.Button cmdOkay;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.ComboBox cboBudgetType;
        private System.Windows.Forms.Label lblBudgetType;
    }
}