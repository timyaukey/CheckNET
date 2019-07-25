<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class ExportForm
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdOkay As System.Windows.Forms.Button
	Public WithEvents txtTransDays As System.Windows.Forms.TextBox
	Public WithEvents txtDueDays As System.Windows.Forms.TextBox
	Public WithEvents txtInvDays As System.Windows.Forms.TextBox
	Public WithEvents txtAgingDays As System.Windows.Forms.TextBox
	Public WithEvents txtTransDate As System.Windows.Forms.TextBox
	Public WithEvents txtDueDate As System.Windows.Forms.TextBox
	Public WithEvents txtInvDate As System.Windows.Forms.TextBox
	Public WithEvents txtAgingDate As System.Windows.Forms.TextBox
	Public WithEvents chkIncludeInvDate As System.Windows.Forms.CheckBox
	Public WithEvents chkIncludeDueDate As System.Windows.Forms.CheckBox
	Public WithEvents chkIncludeTransDate As System.Windows.Forms.CheckBox
	Public WithEvents chkIncludeAging As System.Windows.Forms.CheckBox
	Public WithEvents lblMonthly As System.Windows.Forms.Label
	Public WithEvents lblOutputFile As System.Windows.Forms.Label
	Public WithEvents lblInvDays As System.Windows.Forms.Label
	Public WithEvents lblDueDays As System.Windows.Forms.Label
	Public WithEvents lblTransDays As System.Windows.Forms.Label
	Public WithEvents lblAgingDays As System.Windows.Forms.Label
	Public WithEvents lblInvDate As System.Windows.Forms.Label
	Public WithEvents lblDueDate As System.Windows.Forms.Label
	Public WithEvents lblTransDate As System.Windows.Forms.Label
	Public WithEvents lblAgingDate As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ExportForm))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOkay = New System.Windows.Forms.Button
		Me.txtTransDays = New System.Windows.Forms.TextBox
		Me.txtDueDays = New System.Windows.Forms.TextBox
		Me.txtInvDays = New System.Windows.Forms.TextBox
		Me.txtAgingDays = New System.Windows.Forms.TextBox
		Me.txtTransDate = New System.Windows.Forms.TextBox
		Me.txtDueDate = New System.Windows.Forms.TextBox
		Me.txtInvDate = New System.Windows.Forms.TextBox
		Me.txtAgingDate = New System.Windows.Forms.TextBox
		Me.chkIncludeInvDate = New System.Windows.Forms.CheckBox
		Me.chkIncludeDueDate = New System.Windows.Forms.CheckBox
		Me.chkIncludeTransDate = New System.Windows.Forms.CheckBox
		Me.chkIncludeAging = New System.Windows.Forms.CheckBox
		Me.lblMonthly = New System.Windows.Forms.Label
		Me.lblOutputFile = New System.Windows.Forms.Label
		Me.lblInvDays = New System.Windows.Forms.Label
		Me.lblDueDays = New System.Windows.Forms.Label
		Me.lblTransDays = New System.Windows.Forms.Label
		Me.lblAgingDays = New System.Windows.Forms.Label
		Me.lblInvDate = New System.Windows.Forms.Label
		Me.lblDueDate = New System.Windows.Forms.Label
		Me.lblTransDate = New System.Windows.Forms.Label
		Me.lblAgingDate = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "Export Settings"
		Me.ClientSize = New System.Drawing.Size(624, 209)
		Me.Location = New System.Drawing.Point(3, 23)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation
		Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "ExportForm"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cmdCancel
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.Size = New System.Drawing.Size(87, 27)
		Me.cmdCancel.Location = New System.Drawing.Point(516, 162)
		Me.cmdCancel.TabIndex = 22
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
		Me.cmdOkay.Text = "OK"
		Me.AcceptButton = Me.cmdOkay
		Me.cmdOkay.Size = New System.Drawing.Size(87, 27)
		Me.cmdOkay.Location = New System.Drawing.Point(420, 162)
		Me.cmdOkay.TabIndex = 21
		Me.cmdOkay.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdOkay.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOkay.CausesValidation = True
		Me.cmdOkay.Enabled = True
		Me.cmdOkay.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOkay.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOkay.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOkay.TabStop = True
		Me.cmdOkay.Name = "cmdOkay"
		Me.txtTransDays.AutoSize = False
		Me.txtTransDays.Size = New System.Drawing.Size(57, 21)
		Me.txtTransDays.Location = New System.Drawing.Point(544, 42)
		Me.txtTransDays.TabIndex = 9
		Me.txtTransDays.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtTransDays.AcceptsReturn = True
		Me.txtTransDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTransDays.BackColor = System.Drawing.SystemColors.Window
		Me.txtTransDays.CausesValidation = True
		Me.txtTransDays.Enabled = True
		Me.txtTransDays.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTransDays.HideSelection = True
		Me.txtTransDays.ReadOnly = False
		Me.txtTransDays.Maxlength = 0
		Me.txtTransDays.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTransDays.MultiLine = False
		Me.txtTransDays.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTransDays.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTransDays.TabStop = True
		Me.txtTransDays.Visible = True
		Me.txtTransDays.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTransDays.Name = "txtTransDays"
		Me.txtDueDays.AutoSize = False
		Me.txtDueDays.Size = New System.Drawing.Size(57, 21)
		Me.txtDueDays.Location = New System.Drawing.Point(544, 68)
		Me.txtDueDays.TabIndex = 14
		Me.txtDueDays.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtDueDays.AcceptsReturn = True
		Me.txtDueDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDueDays.BackColor = System.Drawing.SystemColors.Window
		Me.txtDueDays.CausesValidation = True
		Me.txtDueDays.Enabled = True
		Me.txtDueDays.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDueDays.HideSelection = True
		Me.txtDueDays.ReadOnly = False
		Me.txtDueDays.Maxlength = 0
		Me.txtDueDays.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDueDays.MultiLine = False
		Me.txtDueDays.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDueDays.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDueDays.TabStop = True
		Me.txtDueDays.Visible = True
		Me.txtDueDays.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDueDays.Name = "txtDueDays"
		Me.txtInvDays.AutoSize = False
		Me.txtInvDays.Size = New System.Drawing.Size(57, 21)
		Me.txtInvDays.Location = New System.Drawing.Point(544, 94)
		Me.txtInvDays.TabIndex = 19
		Me.txtInvDays.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtInvDays.AcceptsReturn = True
		Me.txtInvDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtInvDays.BackColor = System.Drawing.SystemColors.Window
		Me.txtInvDays.CausesValidation = True
		Me.txtInvDays.Enabled = True
		Me.txtInvDays.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtInvDays.HideSelection = True
		Me.txtInvDays.ReadOnly = False
		Me.txtInvDays.Maxlength = 0
		Me.txtInvDays.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtInvDays.MultiLine = False
		Me.txtInvDays.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtInvDays.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtInvDays.TabStop = True
		Me.txtInvDays.Visible = True
		Me.txtInvDays.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtInvDays.Name = "txtInvDays"
		Me.txtAgingDays.AutoSize = False
		Me.txtAgingDays.Size = New System.Drawing.Size(57, 21)
		Me.txtAgingDays.Location = New System.Drawing.Point(544, 16)
		Me.txtAgingDays.TabIndex = 4
		Me.txtAgingDays.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtAgingDays.AcceptsReturn = True
		Me.txtAgingDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAgingDays.BackColor = System.Drawing.SystemColors.Window
		Me.txtAgingDays.CausesValidation = True
		Me.txtAgingDays.Enabled = True
		Me.txtAgingDays.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAgingDays.HideSelection = True
		Me.txtAgingDays.ReadOnly = False
		Me.txtAgingDays.Maxlength = 0
		Me.txtAgingDays.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAgingDays.MultiLine = False
		Me.txtAgingDays.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAgingDays.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAgingDays.TabStop = True
		Me.txtAgingDays.Visible = True
		Me.txtAgingDays.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAgingDays.Name = "txtAgingDays"
		Me.txtTransDate.AutoSize = False
		Me.txtTransDate.Size = New System.Drawing.Size(117, 21)
		Me.txtTransDate.Location = New System.Drawing.Point(322, 42)
		Me.txtTransDate.TabIndex = 7
		Me.txtTransDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtTransDate.AcceptsReturn = True
		Me.txtTransDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTransDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtTransDate.CausesValidation = True
		Me.txtTransDate.Enabled = True
		Me.txtTransDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTransDate.HideSelection = True
		Me.txtTransDate.ReadOnly = False
		Me.txtTransDate.Maxlength = 0
		Me.txtTransDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTransDate.MultiLine = False
		Me.txtTransDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTransDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTransDate.TabStop = True
		Me.txtTransDate.Visible = True
		Me.txtTransDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTransDate.Name = "txtTransDate"
		Me.txtDueDate.AutoSize = False
		Me.txtDueDate.Size = New System.Drawing.Size(117, 21)
		Me.txtDueDate.Location = New System.Drawing.Point(322, 68)
		Me.txtDueDate.TabIndex = 12
		Me.txtDueDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtDueDate.AcceptsReturn = True
		Me.txtDueDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtDueDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtDueDate.CausesValidation = True
		Me.txtDueDate.Enabled = True
		Me.txtDueDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtDueDate.HideSelection = True
		Me.txtDueDate.ReadOnly = False
		Me.txtDueDate.Maxlength = 0
		Me.txtDueDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtDueDate.MultiLine = False
		Me.txtDueDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtDueDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtDueDate.TabStop = True
		Me.txtDueDate.Visible = True
		Me.txtDueDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtDueDate.Name = "txtDueDate"
		Me.txtInvDate.AutoSize = False
		Me.txtInvDate.Size = New System.Drawing.Size(117, 21)
		Me.txtInvDate.Location = New System.Drawing.Point(322, 94)
		Me.txtInvDate.TabIndex = 17
		Me.txtInvDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtInvDate.AcceptsReturn = True
		Me.txtInvDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtInvDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtInvDate.CausesValidation = True
		Me.txtInvDate.Enabled = True
		Me.txtInvDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtInvDate.HideSelection = True
		Me.txtInvDate.ReadOnly = False
		Me.txtInvDate.Maxlength = 0
		Me.txtInvDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtInvDate.MultiLine = False
		Me.txtInvDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtInvDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtInvDate.TabStop = True
		Me.txtInvDate.Visible = True
		Me.txtInvDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtInvDate.Name = "txtInvDate"
		Me.txtAgingDate.AutoSize = False
		Me.txtAgingDate.Size = New System.Drawing.Size(117, 21)
		Me.txtAgingDate.Location = New System.Drawing.Point(322, 16)
		Me.txtAgingDate.TabIndex = 2
		Me.txtAgingDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtAgingDate.AcceptsReturn = True
		Me.txtAgingDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAgingDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtAgingDate.CausesValidation = True
		Me.txtAgingDate.Enabled = True
		Me.txtAgingDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAgingDate.HideSelection = True
		Me.txtAgingDate.ReadOnly = False
		Me.txtAgingDate.Maxlength = 0
		Me.txtAgingDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAgingDate.MultiLine = False
		Me.txtAgingDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAgingDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAgingDate.TabStop = True
		Me.txtAgingDate.Visible = True
		Me.txtAgingDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAgingDate.Name = "txtAgingDate"
		Me.chkIncludeInvDate.Text = "Include invoice date brackets"
		Me.chkIncludeInvDate.Size = New System.Drawing.Size(185, 21)
		Me.chkIncludeInvDate.Location = New System.Drawing.Point(16, 96)
		Me.chkIncludeInvDate.TabIndex = 15
		Me.chkIncludeInvDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.chkIncludeInvDate.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkIncludeInvDate.FlatStyle = System.Windows.Forms.FlatStyle.Standard
		Me.chkIncludeInvDate.BackColor = System.Drawing.SystemColors.Control
		Me.chkIncludeInvDate.CausesValidation = True
		Me.chkIncludeInvDate.Enabled = True
		Me.chkIncludeInvDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkIncludeInvDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkIncludeInvDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkIncludeInvDate.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkIncludeInvDate.TabStop = True
		Me.chkIncludeInvDate.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkIncludeInvDate.Visible = True
		Me.chkIncludeInvDate.Name = "chkIncludeInvDate"
		Me.chkIncludeDueDate.Text = "Include due date brackets"
		Me.chkIncludeDueDate.Size = New System.Drawing.Size(185, 21)
		Me.chkIncludeDueDate.Location = New System.Drawing.Point(16, 70)
		Me.chkIncludeDueDate.TabIndex = 10
		Me.chkIncludeDueDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.chkIncludeDueDate.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkIncludeDueDate.FlatStyle = System.Windows.Forms.FlatStyle.Standard
		Me.chkIncludeDueDate.BackColor = System.Drawing.SystemColors.Control
		Me.chkIncludeDueDate.CausesValidation = True
		Me.chkIncludeDueDate.Enabled = True
		Me.chkIncludeDueDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkIncludeDueDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkIncludeDueDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkIncludeDueDate.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkIncludeDueDate.TabStop = True
		Me.chkIncludeDueDate.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkIncludeDueDate.Visible = True
		Me.chkIncludeDueDate.Name = "chkIncludeDueDate"
		Me.chkIncludeTransDate.Text = "Include transaction date brackets"
		Me.chkIncludeTransDate.Size = New System.Drawing.Size(185, 21)
		Me.chkIncludeTransDate.Location = New System.Drawing.Point(16, 44)
		Me.chkIncludeTransDate.TabIndex = 5
		Me.chkIncludeTransDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.chkIncludeTransDate.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkIncludeTransDate.FlatStyle = System.Windows.Forms.FlatStyle.Standard
		Me.chkIncludeTransDate.BackColor = System.Drawing.SystemColors.Control
		Me.chkIncludeTransDate.CausesValidation = True
		Me.chkIncludeTransDate.Enabled = True
		Me.chkIncludeTransDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkIncludeTransDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkIncludeTransDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkIncludeTransDate.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkIncludeTransDate.TabStop = True
		Me.chkIncludeTransDate.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkIncludeTransDate.Visible = True
		Me.chkIncludeTransDate.Name = "chkIncludeTransDate"
		Me.chkIncludeAging.Text = "Include aging brackets"
		Me.chkIncludeAging.Size = New System.Drawing.Size(133, 21)
		Me.chkIncludeAging.Location = New System.Drawing.Point(16, 18)
		Me.chkIncludeAging.TabIndex = 0
		Me.chkIncludeAging.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.chkIncludeAging.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkIncludeAging.FlatStyle = System.Windows.Forms.FlatStyle.Standard
		Me.chkIncludeAging.BackColor = System.Drawing.SystemColors.Control
		Me.chkIncludeAging.CausesValidation = True
		Me.chkIncludeAging.Enabled = True
		Me.chkIncludeAging.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkIncludeAging.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkIncludeAging.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkIncludeAging.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkIncludeAging.TabStop = True
		Me.chkIncludeAging.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkIncludeAging.Visible = True
		Me.chkIncludeAging.Name = "chkIncludeAging"
		Me.lblMonthly.Text = "Enter ""whole month"", ""half month"" or ""quarter month"" for start date to use part or all of the month for the brackets."
		Me.lblMonthly.Size = New System.Drawing.Size(307, 33)
		Me.lblMonthly.Location = New System.Drawing.Point(12, 156)
		Me.lblMonthly.TabIndex = 23
		Me.lblMonthly.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblMonthly.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMonthly.BackColor = System.Drawing.SystemColors.Control
		Me.lblMonthly.Enabled = True
		Me.lblMonthly.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMonthly.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMonthly.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMonthly.UseMnemonic = True
		Me.lblMonthly.Visible = True
		Me.lblMonthly.AutoSize = False
		Me.lblMonthly.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMonthly.Name = "lblMonthly"
		Me.lblOutputFile.Text = "(output file)"
		Me.lblOutputFile.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblOutputFile.Size = New System.Drawing.Size(589, 19)
		Me.lblOutputFile.Location = New System.Drawing.Point(12, 130)
		Me.lblOutputFile.TabIndex = 20
		Me.lblOutputFile.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOutputFile.BackColor = System.Drawing.SystemColors.Control
		Me.lblOutputFile.Enabled = True
		Me.lblOutputFile.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOutputFile.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOutputFile.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOutputFile.UseMnemonic = True
		Me.lblOutputFile.Visible = True
		Me.lblOutputFile.AutoSize = False
		Me.lblOutputFile.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOutputFile.Name = "lblOutputFile"
		Me.lblInvDays.Text = "Days in bracket:"
		Me.lblInvDays.Size = New System.Drawing.Size(85, 19)
		Me.lblInvDays.Location = New System.Drawing.Point(454, 98)
		Me.lblInvDays.TabIndex = 18
		Me.lblInvDays.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblInvDays.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblInvDays.BackColor = System.Drawing.SystemColors.Control
		Me.lblInvDays.Enabled = True
		Me.lblInvDays.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInvDays.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInvDays.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInvDays.UseMnemonic = True
		Me.lblInvDays.Visible = True
		Me.lblInvDays.AutoSize = False
		Me.lblInvDays.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInvDays.Name = "lblInvDays"
		Me.lblDueDays.Text = "Days in bracket:"
		Me.lblDueDays.Size = New System.Drawing.Size(85, 19)
		Me.lblDueDays.Location = New System.Drawing.Point(454, 72)
		Me.lblDueDays.TabIndex = 13
		Me.lblDueDays.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblDueDays.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDueDays.BackColor = System.Drawing.SystemColors.Control
		Me.lblDueDays.Enabled = True
		Me.lblDueDays.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDueDays.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDueDays.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDueDays.UseMnemonic = True
		Me.lblDueDays.Visible = True
		Me.lblDueDays.AutoSize = False
		Me.lblDueDays.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDueDays.Name = "lblDueDays"
		Me.lblTransDays.Text = "Days in bracket:"
		Me.lblTransDays.Size = New System.Drawing.Size(85, 19)
		Me.lblTransDays.Location = New System.Drawing.Point(454, 46)
		Me.lblTransDays.TabIndex = 8
		Me.lblTransDays.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblTransDays.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTransDays.BackColor = System.Drawing.SystemColors.Control
		Me.lblTransDays.Enabled = True
		Me.lblTransDays.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTransDays.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTransDays.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTransDays.UseMnemonic = True
		Me.lblTransDays.Visible = True
		Me.lblTransDays.AutoSize = False
		Me.lblTransDays.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTransDays.Name = "lblTransDays"
		Me.lblAgingDays.Text = "Days in bracket:"
		Me.lblAgingDays.Size = New System.Drawing.Size(85, 19)
		Me.lblAgingDays.Location = New System.Drawing.Point(454, 20)
		Me.lblAgingDays.TabIndex = 3
		Me.lblAgingDays.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblAgingDays.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAgingDays.BackColor = System.Drawing.SystemColors.Control
		Me.lblAgingDays.Enabled = True
		Me.lblAgingDays.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAgingDays.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAgingDays.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAgingDays.UseMnemonic = True
		Me.lblAgingDays.Visible = True
		Me.lblAgingDays.AutoSize = False
		Me.lblAgingDays.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAgingDays.Name = "lblAgingDays"
		Me.lblInvDate.Text = "Bracket start date:"
		Me.lblInvDate.Size = New System.Drawing.Size(95, 19)
		Me.lblInvDate.Location = New System.Drawing.Point(222, 98)
		Me.lblInvDate.TabIndex = 16
		Me.lblInvDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblInvDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblInvDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblInvDate.Enabled = True
		Me.lblInvDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblInvDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblInvDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblInvDate.UseMnemonic = True
		Me.lblInvDate.Visible = True
		Me.lblInvDate.AutoSize = False
		Me.lblInvDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblInvDate.Name = "lblInvDate"
		Me.lblDueDate.Text = "Bracket start date:"
		Me.lblDueDate.Size = New System.Drawing.Size(95, 19)
		Me.lblDueDate.Location = New System.Drawing.Point(222, 72)
		Me.lblDueDate.TabIndex = 11
		Me.lblDueDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblDueDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblDueDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblDueDate.Enabled = True
		Me.lblDueDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblDueDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblDueDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblDueDate.UseMnemonic = True
		Me.lblDueDate.Visible = True
		Me.lblDueDate.AutoSize = False
		Me.lblDueDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblDueDate.Name = "lblDueDate"
		Me.lblTransDate.Text = "Bracket start date:"
		Me.lblTransDate.Size = New System.Drawing.Size(95, 19)
		Me.lblTransDate.Location = New System.Drawing.Point(222, 46)
		Me.lblTransDate.TabIndex = 6
		Me.lblTransDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblTransDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTransDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblTransDate.Enabled = True
		Me.lblTransDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTransDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTransDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTransDate.UseMnemonic = True
		Me.lblTransDate.Visible = True
		Me.lblTransDate.AutoSize = False
		Me.lblTransDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTransDate.Name = "lblTransDate"
		Me.lblAgingDate.Text = "Aging date:"
		Me.lblAgingDate.Size = New System.Drawing.Size(95, 19)
		Me.lblAgingDate.Location = New System.Drawing.Point(222, 20)
		Me.lblAgingDate.TabIndex = 1
		Me.lblAgingDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblAgingDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblAgingDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblAgingDate.Enabled = True
		Me.lblAgingDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblAgingDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblAgingDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblAgingDate.UseMnemonic = True
		Me.lblAgingDate.Visible = True
		Me.lblAgingDate.AutoSize = False
		Me.lblAgingDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblAgingDate.Name = "lblAgingDate"
		Me.Controls.Add(cmdCancel)
		Me.Controls.Add(cmdOkay)
		Me.Controls.Add(txtTransDays)
		Me.Controls.Add(txtDueDays)
		Me.Controls.Add(txtInvDays)
		Me.Controls.Add(txtAgingDays)
		Me.Controls.Add(txtTransDate)
		Me.Controls.Add(txtDueDate)
		Me.Controls.Add(txtInvDate)
		Me.Controls.Add(txtAgingDate)
		Me.Controls.Add(chkIncludeInvDate)
		Me.Controls.Add(chkIncludeDueDate)
		Me.Controls.Add(chkIncludeTransDate)
		Me.Controls.Add(chkIncludeAging)
		Me.Controls.Add(lblMonthly)
		Me.Controls.Add(lblOutputFile)
		Me.Controls.Add(lblInvDays)
		Me.Controls.Add(lblDueDays)
		Me.Controls.Add(lblTransDays)
		Me.Controls.Add(lblAgingDays)
		Me.Controls.Add(lblInvDate)
		Me.Controls.Add(lblDueDate)
		Me.Controls.Add(lblTransDate)
		Me.Controls.Add(lblAgingDate)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class