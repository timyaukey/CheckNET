<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class RptScanSplitsForm
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		'This form is an MDI child.
		'This code simulates the VB6 
		' functionality of automatically
		' loading and showing an MDI
		' child's parent.
		Me.MDIParent = CheckBook.CBMainForm
		CheckBook.CBMainForm.Show
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents txtReportDate As System.Windows.Forms.TextBox
	Public WithEvents cboCategory As System.Windows.Forms.ComboBox
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOkay As System.Windows.Forms.Button
	Public WithEvents chkIncludeGenerated As System.Windows.Forms.CheckBox
	Public WithEvents chkIncludeFake As System.Windows.Forms.CheckBox
	Public WithEvents txtEndDate As System.Windows.Forms.TextBox
	Public WithEvents txtStartDate As System.Windows.Forms.TextBox
	Public WithEvents lstAccounts As System.Windows.Forms.ListBox
	Public WithEvents lblReportDate As System.Windows.Forms.Label
	Public WithEvents lblCategory As System.Windows.Forms.Label
	Public WithEvents lblProgress As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(RptScanSplitsForm))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.txtReportDate = New System.Windows.Forms.TextBox
		Me.cboCategory = New System.Windows.Forms.ComboBox
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOkay = New System.Windows.Forms.Button
		Me.chkIncludeGenerated = New System.Windows.Forms.CheckBox
		Me.chkIncludeFake = New System.Windows.Forms.CheckBox
		Me.txtEndDate = New System.Windows.Forms.TextBox
		Me.txtStartDate = New System.Windows.Forms.TextBox
		Me.lstAccounts = New System.Windows.Forms.ListBox
		Me.lblReportDate = New System.Windows.Forms.Label
		Me.lblCategory = New System.Windows.Forms.Label
		Me.lblProgress = New System.Windows.Forms.Label
		Me.Label3 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Text = "Report Title"
		Me.ClientSize = New System.Drawing.Size(336, 406)
		Me.Location = New System.Drawing.Point(2, 22)
		Me.MaximizeBox = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
		Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.MinimizeBox = True
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "RptScanSplitsForm"
		Me.txtReportDate.AutoSize = False
		Me.txtReportDate.Size = New System.Drawing.Size(68, 20)
		Me.txtReportDate.Location = New System.Drawing.Point(82, 226)
		Me.txtReportDate.TabIndex = 5
		Me.txtReportDate.Visible = False
		Me.txtReportDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtReportDate.AcceptsReturn = True
		Me.txtReportDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtReportDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtReportDate.CausesValidation = True
		Me.txtReportDate.Enabled = True
		Me.txtReportDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtReportDate.HideSelection = True
		Me.txtReportDate.ReadOnly = False
		Me.txtReportDate.Maxlength = 0
		Me.txtReportDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtReportDate.MultiLine = False
		Me.txtReportDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtReportDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtReportDate.TabStop = True
		Me.txtReportDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtReportDate.Name = "txtReportDate"
		Me.cboCategory.Size = New System.Drawing.Size(315, 21)
		Me.cboCategory.Location = New System.Drawing.Point(10, 198)
		Me.cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboCategory.TabIndex = 3
		Me.cboCategory.Visible = False
		Me.cboCategory.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cboCategory.BackColor = System.Drawing.SystemColors.Window
		Me.cboCategory.CausesValidation = True
		Me.cboCategory.Enabled = True
		Me.cboCategory.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboCategory.IntegralHeight = True
		Me.cboCategory.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboCategory.Sorted = False
		Me.cboCategory.TabStop = True
		Me.cboCategory.Name = "cboCategory"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cmdCancel
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.Size = New System.Drawing.Size(78, 25)
		Me.cmdCancel.Location = New System.Drawing.Point(248, 370)
		Me.cmdCancel.TabIndex = 14
		Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdOkay.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdOkay.Text = "Okay"
		Me.AcceptButton = Me.cmdOkay
		Me.cmdOkay.Size = New System.Drawing.Size(78, 25)
		Me.cmdOkay.Location = New System.Drawing.Point(166, 370)
		Me.cmdOkay.TabIndex = 13
		Me.cmdOkay.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdOkay.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOkay.CausesValidation = True
		Me.cmdOkay.Enabled = True
		Me.cmdOkay.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOkay.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOkay.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOkay.TabStop = True
		Me.cmdOkay.Name = "cmdOkay"
		Me.chkIncludeGenerated.Text = "Include Generated Transactions"
		Me.chkIncludeGenerated.Size = New System.Drawing.Size(188, 18)
		Me.chkIncludeGenerated.Location = New System.Drawing.Point(10, 325)
		Me.chkIncludeGenerated.TabIndex = 11
		Me.chkIncludeGenerated.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.chkIncludeGenerated.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkIncludeGenerated.FlatStyle = System.Windows.Forms.FlatStyle.Standard
		Me.chkIncludeGenerated.BackColor = System.Drawing.SystemColors.Control
		Me.chkIncludeGenerated.CausesValidation = True
		Me.chkIncludeGenerated.Enabled = True
		Me.chkIncludeGenerated.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkIncludeGenerated.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkIncludeGenerated.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkIncludeGenerated.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkIncludeGenerated.TabStop = True
		Me.chkIncludeGenerated.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkIncludeGenerated.Visible = True
		Me.chkIncludeGenerated.Name = "chkIncludeGenerated"
		Me.chkIncludeFake.Text = "Include Fake Transactions"
		Me.chkIncludeFake.Size = New System.Drawing.Size(179, 18)
		Me.chkIncludeFake.Location = New System.Drawing.Point(10, 303)
		Me.chkIncludeFake.TabIndex = 10
		Me.chkIncludeFake.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.chkIncludeFake.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkIncludeFake.FlatStyle = System.Windows.Forms.FlatStyle.Standard
		Me.chkIncludeFake.BackColor = System.Drawing.SystemColors.Control
		Me.chkIncludeFake.CausesValidation = True
		Me.chkIncludeFake.Enabled = True
		Me.chkIncludeFake.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkIncludeFake.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkIncludeFake.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkIncludeFake.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkIncludeFake.TabStop = True
		Me.chkIncludeFake.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkIncludeFake.Visible = True
		Me.chkIncludeFake.Name = "chkIncludeFake"
		Me.txtEndDate.AutoSize = False
		Me.txtEndDate.Size = New System.Drawing.Size(68, 20)
		Me.txtEndDate.Location = New System.Drawing.Point(82, 274)
		Me.txtEndDate.TabIndex = 9
		Me.txtEndDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtEndDate.AcceptsReturn = True
		Me.txtEndDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtEndDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtEndDate.CausesValidation = True
		Me.txtEndDate.Enabled = True
		Me.txtEndDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtEndDate.HideSelection = True
		Me.txtEndDate.ReadOnly = False
		Me.txtEndDate.Maxlength = 0
		Me.txtEndDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtEndDate.MultiLine = False
		Me.txtEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtEndDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtEndDate.TabStop = True
		Me.txtEndDate.Visible = True
		Me.txtEndDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtEndDate.Name = "txtEndDate"
		Me.txtStartDate.AutoSize = False
		Me.txtStartDate.Size = New System.Drawing.Size(68, 20)
		Me.txtStartDate.Location = New System.Drawing.Point(82, 250)
		Me.txtStartDate.TabIndex = 7
		Me.txtStartDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtStartDate.AcceptsReturn = True
		Me.txtStartDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtStartDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtStartDate.CausesValidation = True
		Me.txtStartDate.Enabled = True
		Me.txtStartDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtStartDate.HideSelection = True
		Me.txtStartDate.ReadOnly = False
		Me.txtStartDate.Maxlength = 0
		Me.txtStartDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtStartDate.MultiLine = False
		Me.txtStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtStartDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtStartDate.TabStop = True
		Me.txtStartDate.Visible = True
		Me.txtStartDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtStartDate.Name = "txtStartDate"
		Me.lstAccounts.Size = New System.Drawing.Size(316, 150)
		Me.lstAccounts.Location = New System.Drawing.Point(10, 29)
		Me.lstAccounts.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
		Me.lstAccounts.TabIndex = 1
		Me.lstAccounts.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lstAccounts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lstAccounts.BackColor = System.Drawing.SystemColors.Window
		Me.lstAccounts.CausesValidation = True
		Me.lstAccounts.Enabled = True
		Me.lstAccounts.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstAccounts.IntegralHeight = True
		Me.lstAccounts.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstAccounts.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstAccounts.Sorted = False
		Me.lstAccounts.TabStop = True
		Me.lstAccounts.Visible = True
		Me.lstAccounts.MultiColumn = False
		Me.lstAccounts.Name = "lstAccounts"
		Me.lblReportDate.Text = "Report Date:"
		Me.lblReportDate.Size = New System.Drawing.Size(68, 18)
		Me.lblReportDate.Location = New System.Drawing.Point(10, 229)
		Me.lblReportDate.TabIndex = 4
		Me.lblReportDate.Visible = False
		Me.lblReportDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblReportDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblReportDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblReportDate.Enabled = True
		Me.lblReportDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblReportDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblReportDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblReportDate.UseMnemonic = True
		Me.lblReportDate.AutoSize = False
		Me.lblReportDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblReportDate.Name = "lblReportDate"
		Me.lblCategory.Text = "Select Category To Report On:"
		Me.lblCategory.Size = New System.Drawing.Size(200, 18)
		Me.lblCategory.Location = New System.Drawing.Point(10, 180)
		Me.lblCategory.TabIndex = 2
		Me.lblCategory.Visible = False
		Me.lblCategory.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblCategory.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblCategory.BackColor = System.Drawing.SystemColors.Control
		Me.lblCategory.Enabled = True
		Me.lblCategory.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblCategory.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblCategory.UseMnemonic = True
		Me.lblCategory.AutoSize = False
		Me.lblCategory.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblCategory.Name = "lblCategory"
		Me.lblProgress.ForeColor = System.Drawing.Color.Red
		Me.lblProgress.Size = New System.Drawing.Size(318, 18)
		Me.lblProgress.Location = New System.Drawing.Point(7, 349)
		Me.lblProgress.TabIndex = 12
		Me.lblProgress.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblProgress.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblProgress.BackColor = System.Drawing.SystemColors.Control
		Me.lblProgress.Enabled = True
		Me.lblProgress.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblProgress.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblProgress.UseMnemonic = True
		Me.lblProgress.Visible = True
		Me.lblProgress.AutoSize = False
		Me.lblProgress.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblProgress.Name = "lblProgress"
		Me.Label3.Text = "End Date:"
		Me.Label3.Size = New System.Drawing.Size(68, 18)
		Me.Label3.Location = New System.Drawing.Point(10, 277)
		Me.Label3.TabIndex = 8
		Me.Label3.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label3.BackColor = System.Drawing.SystemColors.Control
		Me.Label3.Enabled = True
		Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.UseMnemonic = True
		Me.Label3.Visible = True
		Me.Label3.AutoSize = False
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label3.Name = "Label3"
		Me.Label2.Text = "Start Date:"
		Me.Label2.Size = New System.Drawing.Size(68, 18)
		Me.Label2.Location = New System.Drawing.Point(10, 253)
		Me.Label2.TabIndex = 6
		Me.Label2.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label2.BackColor = System.Drawing.SystemColors.Control
		Me.Label2.Enabled = True
		Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label2.UseMnemonic = True
		Me.Label2.Visible = True
		Me.Label2.AutoSize = False
		Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label2.Name = "Label2"
		Me.Label1.Text = "Select Account(s) To Report On:"
		Me.Label1.Size = New System.Drawing.Size(200, 18)
		Me.Label1.Location = New System.Drawing.Point(10, 10)
		Me.Label1.TabIndex = 0
		Me.Label1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label1.BackColor = System.Drawing.SystemColors.Control
		Me.Label1.Enabled = True
		Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label1.UseMnemonic = True
		Me.Label1.Visible = True
		Me.Label1.AutoSize = False
		Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label1.Name = "Label1"
		Me.Controls.Add(txtReportDate)
		Me.Controls.Add(cboCategory)
		Me.Controls.Add(cmdCancel)
		Me.Controls.Add(cmdOkay)
		Me.Controls.Add(chkIncludeGenerated)
		Me.Controls.Add(chkIncludeFake)
		Me.Controls.Add(txtEndDate)
		Me.Controls.Add(txtStartDate)
		Me.Controls.Add(lstAccounts)
		Me.Controls.Add(lblReportDate)
		Me.Controls.Add(lblCategory)
		Me.Controls.Add(lblProgress)
		Me.Controls.Add(Label3)
		Me.Controls.Add(Label2)
		Me.Controls.Add(Label1)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class