namespace Willowsoft.CheckBook.GeneralPlugins
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
            this.SuspendLayout();
            // 
            // ctlEndDate
            // 
            this.ctlEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ctlEndDate.Location = new System.Drawing.Point(146, 38);
            this.ctlEndDate.Name = "ctlEndDate";
            this.ctlEndDate.Size = new System.Drawing.Size(100, 20);
            this.ctlEndDate.TabIndex = 7;
            // 
            // ctlStartDate
            // 
            this.ctlStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ctlStartDate.Location = new System.Drawing.Point(146, 12);
            this.ctlStartDate.Name = "ctlStartDate";
            this.ctlStartDate.Size = new System.Drawing.Size(100, 20);
            this.ctlStartDate.TabIndex = 5;
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(12, 44);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(107, 13);
            this.lblEndDate.TabIndex = 6;
            this.lblEndDate.Text = "Ending Interest Date:";
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(12, 18);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(110, 13);
            this.lblStartDate.TabIndex = 4;
            this.lblStartDate.Text = "Starting Interest Date:";
            // 
            // CalculateInterestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 323);
            this.Controls.Add(this.ctlEndDate);
            this.Controls.Add(this.ctlStartDate);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.lblStartDate);
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
    }
}