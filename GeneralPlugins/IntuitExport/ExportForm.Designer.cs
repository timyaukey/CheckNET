namespace GeneralPlugins.IntuitExport
{
    partial class ExportForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOkay = new System.Windows.Forms.Button();
            this.ctlStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.ctlEndDate = new System.Windows.Forms.DateTimePicker();
            this.ctlChooseBalSheetTranslator = new System.Windows.Forms.OpenFileDialog();
            this.ctlChooseCatTranslator = new System.Windows.Forms.OpenFileDialog();
            this.btnChooseBalSheetTranslator = new System.Windows.Forms.Button();
            this.btnChooseCatTranslator = new System.Windows.Forms.Button();
            this.lblBalSheetTranslatorFile = new System.Windows.Forms.Label();
            this.lblCatTranslatorFile = new System.Windows.Forms.Label();
            this.btnBalSheetTranslatorHelp = new System.Windows.Forms.Button();
            this.btnCatTranslatorHelp = new System.Windows.Forms.Button();
            this.lblDescriptions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(426, 269);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOkay
            // 
            this.btnOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOkay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOkay.Location = new System.Drawing.Point(345, 269);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(75, 23);
            this.btnOkay.TabIndex = 10;
            this.btnOkay.Text = "Ok";
            this.btnOkay.UseVisualStyleBackColor = true;
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // ctlStartDate
            // 
            this.ctlStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ctlStartDate.Location = new System.Drawing.Point(76, 41);
            this.ctlStartDate.Name = "ctlStartDate";
            this.ctlStartDate.Size = new System.Drawing.Size(112, 20);
            this.ctlStartDate.TabIndex = 1;
            // 
            // lblStartDate
            // 
            this.lblStartDate.AutoSize = true;
            this.lblStartDate.Location = new System.Drawing.Point(12, 47);
            this.lblStartDate.Name = "lblStartDate";
            this.lblStartDate.Size = new System.Drawing.Size(58, 13);
            this.lblStartDate.TabIndex = 0;
            this.lblStartDate.Text = "Start Date:";
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(12, 73);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(55, 13);
            this.lblEndDate.TabIndex = 2;
            this.lblEndDate.Text = "End Date:";
            // 
            // ctlEndDate
            // 
            this.ctlEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ctlEndDate.Location = new System.Drawing.Point(76, 67);
            this.ctlEndDate.Name = "ctlEndDate";
            this.ctlEndDate.Size = new System.Drawing.Size(112, 20);
            this.ctlEndDate.TabIndex = 3;
            // 
            // btnChooseBalSheetTranslator
            // 
            this.btnChooseBalSheetTranslator.Location = new System.Drawing.Point(12, 111);
            this.btnChooseBalSheetTranslator.Name = "btnChooseBalSheetTranslator";
            this.btnChooseBalSheetTranslator.Size = new System.Drawing.Size(232, 23);
            this.btnChooseBalSheetTranslator.TabIndex = 4;
            this.btnChooseBalSheetTranslator.Text = "Choose Balance Sheet Translation File";
            this.btnChooseBalSheetTranslator.UseVisualStyleBackColor = true;
            this.btnChooseBalSheetTranslator.Click += new System.EventHandler(this.btnChooseBalSheetTranslator_Click);
            // 
            // btnChooseCatTranslator
            // 
            this.btnChooseCatTranslator.Location = new System.Drawing.Point(12, 176);
            this.btnChooseCatTranslator.Name = "btnChooseCatTranslator";
            this.btnChooseCatTranslator.Size = new System.Drawing.Size(232, 23);
            this.btnChooseCatTranslator.TabIndex = 7;
            this.btnChooseCatTranslator.Text = "Choose Category Translation File";
            this.btnChooseCatTranslator.UseVisualStyleBackColor = true;
            this.btnChooseCatTranslator.Click += new System.EventHandler(this.btnChooseCatTranslator_Click);
            // 
            // lblBalSheetTranslatorFile
            // 
            this.lblBalSheetTranslatorFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBalSheetTranslatorFile.Location = new System.Drawing.Point(15, 137);
            this.lblBalSheetTranslatorFile.Name = "lblBalSheetTranslatorFile";
            this.lblBalSheetTranslatorFile.Size = new System.Drawing.Size(483, 21);
            this.lblBalSheetTranslatorFile.TabIndex = 6;
            // 
            // lblCatTranslatorFile
            // 
            this.lblCatTranslatorFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCatTranslatorFile.Location = new System.Drawing.Point(15, 202);
            this.lblCatTranslatorFile.Name = "lblCatTranslatorFile";
            this.lblCatTranslatorFile.Size = new System.Drawing.Size(483, 21);
            this.lblCatTranslatorFile.TabIndex = 9;
            // 
            // btnBalSheetTranslatorHelp
            // 
            this.btnBalSheetTranslatorHelp.Location = new System.Drawing.Point(250, 111);
            this.btnBalSheetTranslatorHelp.Name = "btnBalSheetTranslatorHelp";
            this.btnBalSheetTranslatorHelp.Size = new System.Drawing.Size(75, 23);
            this.btnBalSheetTranslatorHelp.TabIndex = 5;
            this.btnBalSheetTranslatorHelp.Text = "Help";
            this.btnBalSheetTranslatorHelp.UseVisualStyleBackColor = true;
            this.btnBalSheetTranslatorHelp.Click += new System.EventHandler(this.btnBalSheetTranslatorHelp_Click);
            // 
            // btnCatTranslatorHelp
            // 
            this.btnCatTranslatorHelp.Location = new System.Drawing.Point(250, 176);
            this.btnCatTranslatorHelp.Name = "btnCatTranslatorHelp";
            this.btnCatTranslatorHelp.Size = new System.Drawing.Size(75, 23);
            this.btnCatTranslatorHelp.TabIndex = 8;
            this.btnCatTranslatorHelp.Text = "Help";
            this.btnCatTranslatorHelp.UseVisualStyleBackColor = true;
            this.btnCatTranslatorHelp.Click += new System.EventHandler(this.btnCatTranslatorHelp_Click);
            // 
            // lblDescriptions
            // 
            this.lblDescriptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDescriptions.Location = new System.Drawing.Point(12, 9);
            this.lblDescriptions.Name = "lblDescriptions";
            this.lblDescriptions.Size = new System.Drawing.Size(489, 22);
            this.lblDescriptions.TabIndex = 12;
            this.lblDescriptions.Text = "Export all non-personal accounts to an Intuit IIF format file, suitable for impor" +
    "ting into QuickBooks.";
            // 
            // ExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 304);
            this.Controls.Add(this.lblDescriptions);
            this.Controls.Add(this.btnCatTranslatorHelp);
            this.Controls.Add(this.btnBalSheetTranslatorHelp);
            this.Controls.Add(this.lblCatTranslatorFile);
            this.Controls.Add(this.lblBalSheetTranslatorFile);
            this.Controls.Add(this.btnChooseCatTranslator);
            this.Controls.Add(this.btnChooseBalSheetTranslator);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.ctlEndDate);
            this.Controls.Add(this.lblStartDate);
            this.Controls.Add(this.ctlStartDate);
            this.Controls.Add(this.btnOkay);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportForm";
            this.Text = "Intuit Export (IIF Format)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOkay;
        private System.Windows.Forms.DateTimePicker ctlStartDate;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker ctlEndDate;
        private System.Windows.Forms.OpenFileDialog ctlChooseBalSheetTranslator;
        private System.Windows.Forms.OpenFileDialog ctlChooseCatTranslator;
        private System.Windows.Forms.Button btnChooseBalSheetTranslator;
        private System.Windows.Forms.Button btnChooseCatTranslator;
        private System.Windows.Forms.Label lblBalSheetTranslatorFile;
        private System.Windows.Forms.Label lblCatTranslatorFile;
        private System.Windows.Forms.Button btnBalSheetTranslatorHelp;
        private System.Windows.Forms.Button btnCatTranslatorHelp;
        private System.Windows.Forms.Label lblDescriptions;
    }
}