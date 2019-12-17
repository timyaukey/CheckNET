namespace Willowsoft.CheckBook.GeneralPlugins
{
    partial class RenumberChecksForm
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
            this.lblStartDate = new System.Windows.Forms.Label();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.lblStartNumber = new System.Windows.Forms.Label();
            this.lblEndNumber = new System.Windows.Forms.Label();
            this.lblAddNumber = new System.Windows.Forms.Label();
            this.txtAddNumber = new System.Windows.Forms.TextBox();
            this.ctlStartDate = new System.Windows.Forms.DateTimePicker();
            this.ctlEndDate = new System.Windows.Forms.DateTimePicker();
            this.txtStartNumber = new System.Windows.Forms.TextBox();
            this.txtEndNumber = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnRenumber = new System.Windows.Forms.Button();
            this.lblProgress = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(12, 18);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(140, 13);
            this.lblStartDate.TabIndex = 0;
            this.lblStartDate.Text = "Starting Date To Renumber:";
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(12, 44);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(137, 13);
            this.lblEndDate.TabIndex = 2;
            this.lblEndDate.Text = "Ending Date To Renumber:";
            // 
            // lblStartNumber
            // 
            this.lblStartNumber.AutoSize = true;
            this.lblStartNumber.Location = new System.Drawing.Point(12, 67);
            this.lblStartNumber.Name = "lblStartNumber";
            this.lblStartNumber.Size = new System.Drawing.Size(188, 13);
            this.lblStartNumber.TabIndex = 4;
            this.lblStartNumber.Text = "Starting Check Number To Renumber:";
            // 
            // lblEndNumber
            // 
            this.lblEndNumber.AutoSize = true;
            this.lblEndNumber.Location = new System.Drawing.Point(12, 93);
            this.lblEndNumber.Name = "lblEndNumber";
            this.lblEndNumber.Size = new System.Drawing.Size(185, 13);
            this.lblEndNumber.TabIndex = 6;
            this.lblEndNumber.Text = "Ending Check Number To Renumber:";
            // 
            // lblAddNumber
            // 
            this.lblAddNumber.AutoSize = true;
            this.lblAddNumber.Location = new System.Drawing.Point(12, 119);
            this.lblAddNumber.Name = "lblAddNumber";
            this.lblAddNumber.Size = new System.Drawing.Size(202, 13);
            this.lblAddNumber.TabIndex = 8;
            this.lblAddNumber.Text = "Amount To Add To Each Check Number:";
            // 
            // txtAddNumber
            // 
            this.txtAddNumber.Location = new System.Drawing.Point(220, 116);
            this.txtAddNumber.Name = "txtAddNumber";
            this.txtAddNumber.Size = new System.Drawing.Size(100, 20);
            this.txtAddNumber.TabIndex = 9;
            // 
            // ctlStartDate
            // 
            this.ctlStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ctlStartDate.Location = new System.Drawing.Point(220, 12);
            this.ctlStartDate.Name = "ctlStartDate";
            this.ctlStartDate.Size = new System.Drawing.Size(100, 20);
            this.ctlStartDate.TabIndex = 1;
            // 
            // ctlEndDate
            // 
            this.ctlEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ctlEndDate.Location = new System.Drawing.Point(220, 38);
            this.ctlEndDate.Name = "ctlEndDate";
            this.ctlEndDate.Size = new System.Drawing.Size(100, 20);
            this.ctlEndDate.TabIndex = 3;
            // 
            // txtStartNumber
            // 
            this.txtStartNumber.Location = new System.Drawing.Point(220, 64);
            this.txtStartNumber.Name = "txtStartNumber";
            this.txtStartNumber.Size = new System.Drawing.Size(100, 20);
            this.txtStartNumber.TabIndex = 5;
            // 
            // txtEndNumber
            // 
            this.txtEndNumber.Location = new System.Drawing.Point(220, 90);
            this.txtEndNumber.Name = "txtEndNumber";
            this.txtEndNumber.Size = new System.Drawing.Size(100, 20);
            this.txtEndNumber.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(245, 181);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnRenumber
            // 
            this.btnRenumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRenumber.Location = new System.Drawing.Point(131, 181);
            this.btnRenumber.Name = "btnRenumber";
            this.btnRenumber.Size = new System.Drawing.Size(108, 23);
            this.btnRenumber.TabIndex = 10;
            this.btnRenumber.Text = "Renumber";
            this.btnRenumber.UseVisualStyleBackColor = true;
            this.btnRenumber.Click += new System.EventHandler(this.btnRenumber_Click);
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(12, 147);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(53, 13);
            this.lblProgress.TabIndex = 12;
            this.lblProgress.Text = "(progress)";
            // 
            // RenumberChecksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 216);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.btnRenumber);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtEndNumber);
            this.Controls.Add(this.txtStartNumber);
            this.Controls.Add(this.ctlEndDate);
            this.Controls.Add(this.ctlStartDate);
            this.Controls.Add(this.txtAddNumber);
            this.Controls.Add(this.lblAddNumber);
            this.Controls.Add(this.lblEndNumber);
            this.Controls.Add(this.lblStartNumber);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.lblStartDate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RenumberChecksForm";
            this.Text = "Renumber Checks";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.Label lblStartNumber;
        private System.Windows.Forms.Label lblEndNumber;
        private System.Windows.Forms.Label lblAddNumber;
        private System.Windows.Forms.TextBox txtAddNumber;
        private System.Windows.Forms.DateTimePicker ctlStartDate;
        private System.Windows.Forms.DateTimePicker ctlEndDate;
        private System.Windows.Forms.TextBox txtStartNumber;
        private System.Windows.Forms.TextBox txtEndNumber;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnRenumber;
        private System.Windows.Forms.Label lblProgress;
    }
}