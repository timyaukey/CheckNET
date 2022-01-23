namespace Willowsoft.CheckBook.GeneralPlugins.CalculateInterest
{
    partial class CalculateInterestForm
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
            this.ctlEndDate = new System.Windows.Forms.DateTimePicker();
            this.ctlStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.lvwRegisters = new System.Windows.Forms.ListView();
            this.colRegTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblRegisters = new System.Windows.Forms.Label();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboInterestCategory = new System.Windows.Forms.ComboBox();
            this.lblInterestCategory = new System.Windows.Forms.Label();
            this.lblInterestType = new System.Windows.Forms.Label();
            this.cboInterestType = new System.Windows.Forms.ComboBox();
            this.lblExplanation = new System.Windows.Forms.Label();
            this.lblAnnualRate = new System.Windows.Forms.Label();
            this.txtAnnualRate = new System.Windows.Forms.TextBox();
            this.btnSearchStarting = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ctlEndDate
            // 
            this.ctlEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ctlEndDate.Location = new System.Drawing.Point(146, 38);
            this.ctlEndDate.Name = "ctlEndDate";
            this.ctlEndDate.Size = new System.Drawing.Size(100, 20);
            this.ctlEndDate.TabIndex = 3;
            // 
            // ctlStartDate
            // 
            this.ctlStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ctlStartDate.Location = new System.Drawing.Point(146, 12);
            this.ctlStartDate.Name = "ctlStartDate";
            this.ctlStartDate.Size = new System.Drawing.Size(100, 20);
            this.ctlStartDate.TabIndex = 1;
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(12, 44);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(107, 13);
            this.lblEndDate.TabIndex = 2;
            this.lblEndDate.Text = "Ending Interest Date:";
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(12, 18);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(110, 13);
            this.lblStartDate.TabIndex = 0;
            this.lblStartDate.Text = "Starting Interest Date:";
            // 
            // lvwRegisters
            // 
            this.lvwRegisters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwRegisters.CheckBoxes = true;
            this.lvwRegisters.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colRegTitle});
            this.lvwRegisters.HideSelection = false;
            this.lvwRegisters.Location = new System.Drawing.Point(12, 217);
            this.lvwRegisters.Name = "lvwRegisters";
            this.lvwRegisters.Size = new System.Drawing.Size(608, 152);
            this.lvwRegisters.TabIndex = 12;
            this.lvwRegisters.UseCompatibleStateImageBehavior = false;
            this.lvwRegisters.View = System.Windows.Forms.View.Details;
            // 
            // colRegTitle
            // 
            this.colRegTitle.Text = "Registers In Current Account and Any Related Personal Account";
            this.colRegTitle.Width = 581;
            // 
            // lblRegisters
            // 
            this.lblRegisters.AutoSize = true;
            this.lblRegisters.Location = new System.Drawing.Point(12, 201);
            this.lblRegisters.Name = "lblRegisters";
            this.lblRegisters.Size = new System.Drawing.Size(256, 13);
            this.lblRegisters.TabIndex = 11;
            this.lblRegisters.Text = "Check the registers to include in balance calculation:";
            // 
            // btnCalculate
            // 
            this.btnCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCalculate.Location = new System.Drawing.Point(382, 375);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(116, 23);
            this.btnCalculate.TabIndex = 13;
            this.btnCalculate.Text = "Calculate Interest";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(504, 375);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(116, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cboInterestCategory
            // 
            this.cboInterestCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInterestCategory.FormattingEnabled = true;
            this.cboInterestCategory.Location = new System.Drawing.Point(146, 90);
            this.cboInterestCategory.Name = "cboInterestCategory";
            this.cboInterestCategory.Size = new System.Drawing.Size(352, 21);
            this.cboInterestCategory.TabIndex = 7;
            // 
            // lblInterestCategory
            // 
            this.lblInterestCategory.AutoSize = true;
            this.lblInterestCategory.Location = new System.Drawing.Point(12, 93);
            this.lblInterestCategory.Name = "lblInterestCategory";
            this.lblInterestCategory.Size = new System.Drawing.Size(90, 13);
            this.lblInterestCategory.TabIndex = 6;
            this.lblInterestCategory.Text = "Interest Category:";
            // 
            // lblInterestType
            // 
            this.lblInterestType.AutoSize = true;
            this.lblInterestType.Location = new System.Drawing.Point(12, 120);
            this.lblInterestType.Name = "lblInterestType";
            this.lblInterestType.Size = new System.Drawing.Size(72, 13);
            this.lblInterestType.TabIndex = 8;
            this.lblInterestType.Text = "Interest Type:";
            // 
            // cboInterestType
            // 
            this.cboInterestType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInterestType.FormattingEnabled = true;
            this.cboInterestType.Location = new System.Drawing.Point(146, 117);
            this.cboInterestType.Name = "cboInterestType";
            this.cboInterestType.Size = new System.Drawing.Size(352, 21);
            this.cboInterestType.TabIndex = 9;
            this.cboInterestType.SelectedIndexChanged += new System.EventHandler(this.cboInterestType_SelectedIndexChanged);
            // 
            // lblExplanation
            // 
            this.lblExplanation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblExplanation.Location = new System.Drawing.Point(143, 141);
            this.lblExplanation.Name = "lblExplanation";
            this.lblExplanation.Size = new System.Drawing.Size(477, 47);
            this.lblExplanation.TabIndex = 10;
            this.lblExplanation.Text = "(explanation)";
            // 
            // lblAnnualRate
            // 
            this.lblAnnualRate.AutoSize = true;
            this.lblAnnualRate.Location = new System.Drawing.Point(12, 67);
            this.lblAnnualRate.Name = "lblAnnualRate";
            this.lblAnnualRate.Size = new System.Drawing.Size(107, 13);
            this.lblAnnualRate.TabIndex = 4;
            this.lblAnnualRate.Text = "Annual Interest Rate:";
            // 
            // txtAnnualRate
            // 
            this.txtAnnualRate.Location = new System.Drawing.Point(146, 64);
            this.txtAnnualRate.Name = "txtAnnualRate";
            this.txtAnnualRate.Size = new System.Drawing.Size(57, 20);
            this.txtAnnualRate.TabIndex = 5;
            this.txtAnnualRate.Text = "18.0";
            // 
            // btnSearchStarting
            // 
            this.btnSearchStarting.Location = new System.Drawing.Point(252, 9);
            this.btnSearchStarting.Name = "btnSearchStarting";
            this.btnSearchStarting.Size = new System.Drawing.Size(150, 23);
            this.btnSearchStarting.TabIndex = 15;
            this.btnSearchStarting.Text = "Search For Starting Date";
            this.btnSearchStarting.UseVisualStyleBackColor = true;
            this.btnSearchStarting.Click += new System.EventHandler(this.btnSearchStarting_Click);
            // 
            // CalculateInterestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 410);
            this.Controls.Add(this.btnSearchStarting);
            this.Controls.Add(this.txtAnnualRate);
            this.Controls.Add(this.lblAnnualRate);
            this.Controls.Add(this.lblExplanation);
            this.Controls.Add(this.lblInterestType);
            this.Controls.Add(this.cboInterestType);
            this.Controls.Add(this.lblInterestCategory);
            this.Controls.Add(this.cboInterestCategory);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.lblRegisters);
            this.Controls.Add(this.lvwRegisters);
            this.Controls.Add(this.ctlEndDate);
            this.Controls.Add(this.ctlStartDate);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.lblStartDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalculateInterestForm";
            this.Text = "Calculate Interest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker ctlEndDate;
        private System.Windows.Forms.DateTimePicker ctlStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.ListView lvwRegisters;
        private System.Windows.Forms.Label lblRegisters;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ColumnHeader colRegTitle;
        private System.Windows.Forms.ComboBox cboInterestCategory;
        private System.Windows.Forms.Label lblInterestCategory;
        private System.Windows.Forms.Label lblInterestType;
        private System.Windows.Forms.ComboBox cboInterestType;
        private System.Windows.Forms.Label lblExplanation;
        private System.Windows.Forms.Label lblAnnualRate;
        private System.Windows.Forms.TextBox txtAnnualRate;
        private System.Windows.Forms.Button btnSearchStarting;
    }
}