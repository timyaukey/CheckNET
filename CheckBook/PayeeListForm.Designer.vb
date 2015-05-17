<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class PayeeListForm
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
	Public WithEvents txtAccount As System.Windows.Forms.TextBox
	Public WithEvents txtZip As System.Windows.Forms.TextBox
	Public WithEvents txtState As System.Windows.Forms.TextBox
	Public WithEvents txtCity As System.Windows.Forms.TextBox
	Public WithEvents txtAddress2 As System.Windows.Forms.TextBox
	Public WithEvents txtAddress1 As System.Windows.Forms.TextBox
	Public WithEvents cmdDiscardChanges As System.Windows.Forms.Button
	Public WithEvents cmdSaveChanges As System.Windows.Forms.Button
	Public WithEvents cmdDeletePayee As System.Windows.Forms.Button
	Public WithEvents cmdNewPayee As System.Windows.Forms.Button
	Public WithEvents cboBudget As System.Windows.Forms.ComboBox
	Public WithEvents cboCategory As System.Windows.Forms.ComboBox
	Public WithEvents txtMemo As System.Windows.Forms.TextBox
	Public WithEvents txtAmount As System.Windows.Forms.TextBox
	Public WithEvents txtPayee As System.Windows.Forms.TextBox
	Public WithEvents txtNumber As System.Windows.Forms.TextBox
	Public WithEvents lvwPayees As System.Windows.Forms.ListView
	Public WithEvents txtMaxAmount As System.Windows.Forms.TextBox
	Public WithEvents txtMinAmount As System.Windows.Forms.TextBox
	Public WithEvents txtBank As System.Windows.Forms.TextBox
	Public WithEvents Label10 As System.Windows.Forms.Label
	Public WithEvents Label9 As System.Windows.Forms.Label
	Public WithEvents Label7 As System.Windows.Forms.Label
	Public WithEvents fraImport As System.Windows.Forms.GroupBox
	Public WithEvents Label15 As System.Windows.Forms.Label
	Public WithEvents Label14 As System.Windows.Forms.Label
	Public WithEvents Label13 As System.Windows.Forms.Label
	Public WithEvents Label12 As System.Windows.Forms.Label
	Public WithEvents Label11 As System.Windows.Forms.Label
	Public WithEvents Label8 As System.Windows.Forms.Label
	Public WithEvents Label6 As System.Windows.Forms.Label
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(PayeeListForm))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.txtAccount = New System.Windows.Forms.TextBox
		Me.txtZip = New System.Windows.Forms.TextBox
		Me.txtState = New System.Windows.Forms.TextBox
		Me.txtCity = New System.Windows.Forms.TextBox
		Me.txtAddress2 = New System.Windows.Forms.TextBox
		Me.txtAddress1 = New System.Windows.Forms.TextBox
		Me.cmdDiscardChanges = New System.Windows.Forms.Button
		Me.cmdSaveChanges = New System.Windows.Forms.Button
		Me.cmdDeletePayee = New System.Windows.Forms.Button
		Me.cmdNewPayee = New System.Windows.Forms.Button
		Me.cboBudget = New System.Windows.Forms.ComboBox
		Me.cboCategory = New System.Windows.Forms.ComboBox
		Me.txtMemo = New System.Windows.Forms.TextBox
		Me.txtAmount = New System.Windows.Forms.TextBox
		Me.txtPayee = New System.Windows.Forms.TextBox
		Me.txtNumber = New System.Windows.Forms.TextBox
		Me.lvwPayees = New System.Windows.Forms.ListView
		Me.fraImport = New System.Windows.Forms.GroupBox
		Me.txtMaxAmount = New System.Windows.Forms.TextBox
		Me.txtMinAmount = New System.Windows.Forms.TextBox
		Me.txtBank = New System.Windows.Forms.TextBox
		Me.Label10 = New System.Windows.Forms.Label
		Me.Label9 = New System.Windows.Forms.Label
		Me.Label7 = New System.Windows.Forms.Label
		Me.Label15 = New System.Windows.Forms.Label
		Me.Label14 = New System.Windows.Forms.Label
		Me.Label13 = New System.Windows.Forms.Label
		Me.Label12 = New System.Windows.Forms.Label
		Me.Label11 = New System.Windows.Forms.Label
		Me.Label8 = New System.Windows.Forms.Label
		Me.Label6 = New System.Windows.Forms.Label
		Me.Label5 = New System.Windows.Forms.Label
		Me.Label4 = New System.Windows.Forms.Label
		Me.Label3 = New System.Windows.Forms.Label
		Me.Label2 = New System.Windows.Forms.Label
		Me.Label1 = New System.Windows.Forms.Label
		Me.fraImport.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "Memorized Transaction List"
		Me.ClientSize = New System.Drawing.Size(717, 629)
		Me.Location = New System.Drawing.Point(3, 23)
		Me.ControlBox = False
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.Enabled = True
		Me.KeyPreview = False
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "PayeeListForm"
		Me.txtAccount.AutoSize = False
		Me.txtAccount.Size = New System.Drawing.Size(77, 23)
		Me.txtAccount.Location = New System.Drawing.Point(580, 372)
		Me.txtAccount.TabIndex = 12
		Me.txtAccount.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtAccount.AcceptsReturn = True
		Me.txtAccount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAccount.BackColor = System.Drawing.SystemColors.Window
		Me.txtAccount.CausesValidation = True
		Me.txtAccount.Enabled = True
		Me.txtAccount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAccount.HideSelection = True
		Me.txtAccount.ReadOnly = False
		Me.txtAccount.Maxlength = 0
		Me.txtAccount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAccount.MultiLine = False
		Me.txtAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAccount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAccount.TabStop = True
		Me.txtAccount.Visible = True
		Me.txtAccount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAccount.Name = "txtAccount"
		Me.txtZip.AutoSize = False
		Me.txtZip.Size = New System.Drawing.Size(81, 23)
		Me.txtZip.Location = New System.Drawing.Point(416, 424)
		Me.txtZip.TabIndex = 18
		Me.txtZip.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtZip.AcceptsReturn = True
		Me.txtZip.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtZip.BackColor = System.Drawing.SystemColors.Window
		Me.txtZip.CausesValidation = True
		Me.txtZip.Enabled = True
		Me.txtZip.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtZip.HideSelection = True
		Me.txtZip.ReadOnly = False
		Me.txtZip.Maxlength = 0
		Me.txtZip.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtZip.MultiLine = False
		Me.txtZip.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtZip.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtZip.TabStop = True
		Me.txtZip.Visible = True
		Me.txtZip.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtZip.Name = "txtZip"
		Me.txtState.AutoSize = False
		Me.txtState.Size = New System.Drawing.Size(39, 23)
		Me.txtState.Location = New System.Drawing.Point(374, 424)
		Me.txtState.TabIndex = 17
		Me.txtState.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtState.AcceptsReturn = True
		Me.txtState.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtState.BackColor = System.Drawing.SystemColors.Window
		Me.txtState.CausesValidation = True
		Me.txtState.Enabled = True
		Me.txtState.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtState.HideSelection = True
		Me.txtState.ReadOnly = False
		Me.txtState.Maxlength = 0
		Me.txtState.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtState.MultiLine = False
		Me.txtState.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtState.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtState.TabStop = True
		Me.txtState.Visible = True
		Me.txtState.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtState.Name = "txtState"
		Me.txtCity.AutoSize = False
		Me.txtCity.Size = New System.Drawing.Size(187, 23)
		Me.txtCity.Location = New System.Drawing.Point(184, 424)
		Me.txtCity.TabIndex = 16
		Me.txtCity.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtCity.AcceptsReturn = True
		Me.txtCity.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtCity.BackColor = System.Drawing.SystemColors.Window
		Me.txtCity.CausesValidation = True
		Me.txtCity.Enabled = True
		Me.txtCity.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtCity.HideSelection = True
		Me.txtCity.ReadOnly = False
		Me.txtCity.Maxlength = 0
		Me.txtCity.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtCity.MultiLine = False
		Me.txtCity.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtCity.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtCity.TabStop = True
		Me.txtCity.Visible = True
		Me.txtCity.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtCity.Name = "txtCity"
		Me.txtAddress2.AutoSize = False
		Me.txtAddress2.Size = New System.Drawing.Size(313, 23)
		Me.txtAddress2.Location = New System.Drawing.Point(184, 398)
		Me.txtAddress2.TabIndex = 14
		Me.txtAddress2.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtAddress2.AcceptsReturn = True
		Me.txtAddress2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAddress2.BackColor = System.Drawing.SystemColors.Window
		Me.txtAddress2.CausesValidation = True
		Me.txtAddress2.Enabled = True
		Me.txtAddress2.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAddress2.HideSelection = True
		Me.txtAddress2.ReadOnly = False
		Me.txtAddress2.Maxlength = 0
		Me.txtAddress2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAddress2.MultiLine = False
		Me.txtAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAddress2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAddress2.TabStop = True
		Me.txtAddress2.Visible = True
		Me.txtAddress2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAddress2.Name = "txtAddress2"
		Me.txtAddress1.AutoSize = False
		Me.txtAddress1.Size = New System.Drawing.Size(313, 23)
		Me.txtAddress1.Location = New System.Drawing.Point(184, 372)
		Me.txtAddress1.TabIndex = 10
		Me.txtAddress1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtAddress1.AcceptsReturn = True
		Me.txtAddress1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAddress1.BackColor = System.Drawing.SystemColors.Window
		Me.txtAddress1.CausesValidation = True
		Me.txtAddress1.Enabled = True
		Me.txtAddress1.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAddress1.HideSelection = True
		Me.txtAddress1.ReadOnly = False
		Me.txtAddress1.Maxlength = 0
		Me.txtAddress1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAddress1.MultiLine = False
		Me.txtAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAddress1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAddress1.TabStop = True
		Me.txtAddress1.Visible = True
		Me.txtAddress1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAddress1.Name = "txtAddress1"
		Me.cmdDiscardChanges.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDiscardChanges.Text = "Discard Changes"
		Me.cmdDiscardChanges.Size = New System.Drawing.Size(141, 23)
		Me.cmdDiscardChanges.Location = New System.Drawing.Point(568, 566)
		Me.cmdDiscardChanges.TabIndex = 34
		Me.cmdDiscardChanges.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdDiscardChanges.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDiscardChanges.CausesValidation = True
		Me.cmdDiscardChanges.Enabled = True
		Me.cmdDiscardChanges.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDiscardChanges.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDiscardChanges.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDiscardChanges.TabStop = True
		Me.cmdDiscardChanges.Name = "cmdDiscardChanges"
		Me.cmdSaveChanges.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSaveChanges.Text = "Save Changes"
		Me.cmdSaveChanges.Size = New System.Drawing.Size(141, 23)
		Me.cmdSaveChanges.Location = New System.Drawing.Point(568, 594)
		Me.cmdSaveChanges.TabIndex = 35
		Me.cmdSaveChanges.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdSaveChanges.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSaveChanges.CausesValidation = True
		Me.cmdSaveChanges.Enabled = True
		Me.cmdSaveChanges.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSaveChanges.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSaveChanges.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSaveChanges.TabStop = True
		Me.cmdSaveChanges.Name = "cmdSaveChanges"
		Me.cmdDeletePayee.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDeletePayee.Text = "Delete Transaction"
		Me.cmdDeletePayee.Size = New System.Drawing.Size(141, 23)
		Me.cmdDeletePayee.Location = New System.Drawing.Point(568, 538)
		Me.cmdDeletePayee.TabIndex = 33
		Me.cmdDeletePayee.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdDeletePayee.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDeletePayee.CausesValidation = True
		Me.cmdDeletePayee.Enabled = True
		Me.cmdDeletePayee.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDeletePayee.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDeletePayee.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDeletePayee.TabStop = True
		Me.cmdDeletePayee.Name = "cmdDeletePayee"
		Me.cmdNewPayee.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNewPayee.Text = "New Transaction"
		Me.cmdNewPayee.Size = New System.Drawing.Size(141, 23)
		Me.cmdNewPayee.Location = New System.Drawing.Point(568, 510)
		Me.cmdNewPayee.TabIndex = 32
		Me.cmdNewPayee.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdNewPayee.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNewPayee.CausesValidation = True
		Me.cmdNewPayee.Enabled = True
		Me.cmdNewPayee.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNewPayee.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNewPayee.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNewPayee.TabStop = True
		Me.cmdNewPayee.Name = "cmdNewPayee"
		Me.cboBudget.Size = New System.Drawing.Size(313, 21)
		Me.cboBudget.Location = New System.Drawing.Point(184, 500)
		Me.cboBudget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboBudget.TabIndex = 24
		Me.cboBudget.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cboBudget.BackColor = System.Drawing.SystemColors.Window
		Me.cboBudget.CausesValidation = True
		Me.cboBudget.Enabled = True
		Me.cboBudget.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboBudget.IntegralHeight = True
		Me.cboBudget.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboBudget.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboBudget.Sorted = False
		Me.cboBudget.TabStop = True
		Me.cboBudget.Visible = True
		Me.cboBudget.Name = "cboBudget"
		Me.cboCategory.Size = New System.Drawing.Size(313, 21)
		Me.cboCategory.Location = New System.Drawing.Point(184, 476)
		Me.cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboCategory.TabIndex = 22
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
		Me.cboCategory.Visible = True
		Me.cboCategory.Name = "cboCategory"
		Me.txtMemo.AutoSize = False
		Me.txtMemo.Size = New System.Drawing.Size(313, 23)
		Me.txtMemo.Location = New System.Drawing.Point(184, 450)
		Me.txtMemo.TabIndex = 20
		Me.txtMemo.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtMemo.AcceptsReturn = True
		Me.txtMemo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMemo.BackColor = System.Drawing.SystemColors.Window
		Me.txtMemo.CausesValidation = True
		Me.txtMemo.Enabled = True
		Me.txtMemo.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMemo.HideSelection = True
		Me.txtMemo.ReadOnly = False
		Me.txtMemo.Maxlength = 0
		Me.txtMemo.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMemo.MultiLine = False
		Me.txtMemo.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMemo.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMemo.TabStop = True
		Me.txtMemo.Visible = True
		Me.txtMemo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMemo.Name = "txtMemo"
		Me.txtAmount.AutoSize = False
		Me.txtAmount.Size = New System.Drawing.Size(77, 23)
		Me.txtAmount.Location = New System.Drawing.Point(580, 346)
		Me.txtAmount.TabIndex = 8
		Me.txtAmount.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtAmount.AcceptsReturn = True
		Me.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtAmount.CausesValidation = True
		Me.txtAmount.Enabled = True
		Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtAmount.HideSelection = True
		Me.txtAmount.ReadOnly = False
		Me.txtAmount.Maxlength = 0
		Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtAmount.MultiLine = False
		Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtAmount.TabStop = True
		Me.txtAmount.Visible = True
		Me.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtAmount.Name = "txtAmount"
		Me.txtPayee.AutoSize = False
		Me.txtPayee.Size = New System.Drawing.Size(313, 23)
		Me.txtPayee.Location = New System.Drawing.Point(184, 346)
		Me.txtPayee.TabIndex = 6
		Me.txtPayee.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtPayee.AcceptsReturn = True
		Me.txtPayee.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPayee.BackColor = System.Drawing.SystemColors.Window
		Me.txtPayee.CausesValidation = True
		Me.txtPayee.Enabled = True
		Me.txtPayee.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPayee.HideSelection = True
		Me.txtPayee.ReadOnly = False
		Me.txtPayee.Maxlength = 0
		Me.txtPayee.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPayee.MultiLine = False
		Me.txtPayee.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPayee.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPayee.TabStop = True
		Me.txtPayee.Visible = True
		Me.txtPayee.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPayee.Name = "txtPayee"
		Me.txtNumber.AutoSize = False
		Me.txtNumber.Size = New System.Drawing.Size(61, 23)
		Me.txtNumber.Location = New System.Drawing.Point(58, 346)
		Me.txtNumber.TabIndex = 4
		Me.txtNumber.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtNumber.AcceptsReturn = True
		Me.txtNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtNumber.BackColor = System.Drawing.SystemColors.Window
		Me.txtNumber.CausesValidation = True
		Me.txtNumber.Enabled = True
		Me.txtNumber.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtNumber.HideSelection = True
		Me.txtNumber.ReadOnly = False
		Me.txtNumber.Maxlength = 0
		Me.txtNumber.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtNumber.MultiLine = False
		Me.txtNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtNumber.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtNumber.TabStop = True
		Me.txtNumber.Visible = True
		Me.txtNumber.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtNumber.Name = "txtNumber"
		Me.lvwPayees.Size = New System.Drawing.Size(709, 245)
		Me.lvwPayees.Location = New System.Drawing.Point(4, 92)
		Me.lvwPayees.TabIndex = 2
		Me.lvwPayees.LabelWrap = True
		Me.lvwPayees.HideSelection = True
		Me.lvwPayees.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwPayees.BackColor = System.Drawing.SystemColors.Window
		Me.lvwPayees.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lvwPayees.LabelEdit = True
		Me.lvwPayees.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwPayees.Name = "lvwPayees"
		Me.fraImport.Text = "Info Used To Find Memorized Transactions When Importing From Bank"
		Me.fraImport.Size = New System.Drawing.Size(445, 83)
		Me.fraImport.Location = New System.Drawing.Point(46, 532)
		Me.fraImport.TabIndex = 25
		Me.fraImport.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.fraImport.BackColor = System.Drawing.SystemColors.Control
		Me.fraImport.Enabled = True
		Me.fraImport.ForeColor = System.Drawing.SystemColors.ControlText
		Me.fraImport.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.fraImport.Visible = True
		Me.fraImport.Padding = New System.Windows.Forms.Padding(0)
		Me.fraImport.Name = "fraImport"
		Me.txtMaxAmount.AutoSize = False
		Me.txtMaxAmount.Size = New System.Drawing.Size(77, 23)
		Me.txtMaxAmount.Location = New System.Drawing.Point(358, 50)
		Me.txtMaxAmount.TabIndex = 31
		Me.txtMaxAmount.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtMaxAmount.AcceptsReturn = True
		Me.txtMaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMaxAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtMaxAmount.CausesValidation = True
		Me.txtMaxAmount.Enabled = True
		Me.txtMaxAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMaxAmount.HideSelection = True
		Me.txtMaxAmount.ReadOnly = False
		Me.txtMaxAmount.Maxlength = 0
		Me.txtMaxAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMaxAmount.MultiLine = False
		Me.txtMaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMaxAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMaxAmount.TabStop = True
		Me.txtMaxAmount.Visible = True
		Me.txtMaxAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMaxAmount.Name = "txtMaxAmount"
		Me.txtMinAmount.AutoSize = False
		Me.txtMinAmount.Size = New System.Drawing.Size(77, 23)
		Me.txtMinAmount.Location = New System.Drawing.Point(136, 50)
		Me.txtMinAmount.TabIndex = 29
		Me.txtMinAmount.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtMinAmount.AcceptsReturn = True
		Me.txtMinAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMinAmount.BackColor = System.Drawing.SystemColors.Window
		Me.txtMinAmount.CausesValidation = True
		Me.txtMinAmount.Enabled = True
		Me.txtMinAmount.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMinAmount.HideSelection = True
		Me.txtMinAmount.ReadOnly = False
		Me.txtMinAmount.Maxlength = 0
		Me.txtMinAmount.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMinAmount.MultiLine = False
		Me.txtMinAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMinAmount.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMinAmount.TabStop = True
		Me.txtMinAmount.Visible = True
		Me.txtMinAmount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMinAmount.Name = "txtMinAmount"
		Me.txtBank.AutoSize = False
		Me.txtBank.Size = New System.Drawing.Size(299, 23)
		Me.txtBank.Location = New System.Drawing.Point(136, 22)
		Me.txtBank.TabIndex = 27
		Me.txtBank.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtBank.AcceptsReturn = True
		Me.txtBank.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtBank.BackColor = System.Drawing.SystemColors.Window
		Me.txtBank.CausesValidation = True
		Me.txtBank.Enabled = True
		Me.txtBank.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtBank.HideSelection = True
		Me.txtBank.ReadOnly = False
		Me.txtBank.Maxlength = 0
		Me.txtBank.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtBank.MultiLine = False
		Me.txtBank.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtBank.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtBank.TabStop = True
		Me.txtBank.Visible = True
		Me.txtBank.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtBank.Name = "txtBank"
		Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label10.Text = "Max Amount To Match:"
		Me.Label10.Size = New System.Drawing.Size(119, 17)
		Me.Label10.Location = New System.Drawing.Point(236, 52)
		Me.Label10.TabIndex = 30
		Me.Label10.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label10.BackColor = System.Drawing.SystemColors.Control
		Me.Label10.Enabled = True
		Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label10.UseMnemonic = True
		Me.Label10.Visible = True
		Me.Label10.AutoSize = False
		Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label10.Name = "Label10"
		Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label9.Text = "Min Amount To Match:"
		Me.Label9.Size = New System.Drawing.Size(119, 17)
		Me.Label9.Location = New System.Drawing.Point(14, 52)
		Me.Label9.TabIndex = 28
		Me.Label9.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label9.BackColor = System.Drawing.SystemColors.Control
		Me.Label9.Enabled = True
		Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label9.UseMnemonic = True
		Me.Label9.Visible = True
		Me.Label9.AutoSize = False
		Me.Label9.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label9.Name = "Label9"
		Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label7.Text = "Name Used By Bank:"
		Me.Label7.Size = New System.Drawing.Size(127, 17)
		Me.Label7.Location = New System.Drawing.Point(6, 24)
		Me.Label7.TabIndex = 26
		Me.Label7.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label7.BackColor = System.Drawing.SystemColors.Control
		Me.Label7.Enabled = True
		Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label7.UseMnemonic = True
		Me.Label7.Visible = True
		Me.Label7.AutoSize = False
		Me.Label7.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label7.Name = "Label7"
		Me.Label15.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label15.Text = "Account #:"
		Me.Label15.Size = New System.Drawing.Size(65, 17)
		Me.Label15.Location = New System.Drawing.Point(512, 374)
		Me.Label15.TabIndex = 11
		Me.Label15.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label15.BackColor = System.Drawing.SystemColors.Control
		Me.Label15.Enabled = True
		Me.Label15.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label15.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label15.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label15.UseMnemonic = True
		Me.Label15.Visible = True
		Me.Label15.AutoSize = False
		Me.Label15.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label15.Name = "Label15"
		Me.Label14.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label14.Text = "City/State/Zip:"
		Me.Label14.Size = New System.Drawing.Size(115, 17)
		Me.Label14.Location = New System.Drawing.Point(66, 426)
		Me.Label14.TabIndex = 15
		Me.Label14.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label14.BackColor = System.Drawing.SystemColors.Control
		Me.Label14.Enabled = True
		Me.Label14.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label14.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label14.UseMnemonic = True
		Me.Label14.Visible = True
		Me.Label14.AutoSize = False
		Me.Label14.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label14.Name = "Label14"
		Me.Label13.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label13.Text = "Address Line 2:"
		Me.Label13.Size = New System.Drawing.Size(113, 17)
		Me.Label13.Location = New System.Drawing.Point(68, 400)
		Me.Label13.TabIndex = 13
		Me.Label13.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label13.BackColor = System.Drawing.SystemColors.Control
		Me.Label13.Enabled = True
		Me.Label13.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label13.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label13.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label13.UseMnemonic = True
		Me.Label13.Visible = True
		Me.Label13.AutoSize = False
		Me.Label13.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label13.Name = "Label13"
		Me.Label12.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label12.Text = "Address Line 1:"
		Me.Label12.Size = New System.Drawing.Size(111, 17)
		Me.Label12.Location = New System.Drawing.Point(70, 374)
		Me.Label12.TabIndex = 9
		Me.Label12.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label12.BackColor = System.Drawing.SystemColors.Control
		Me.Label12.Enabled = True
		Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label12.UseMnemonic = True
		Me.Label12.Visible = True
		Me.Label12.AutoSize = False
		Me.Label12.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label12.Name = "Label12"
		Me.Label11.Text = "Memorized transactions are used to fill in information for you when you press ^S after entering a few letters of the description while entering a transaction, and to translate transaction information imported from the bank."
		Me.Label11.Size = New System.Drawing.Size(689, 31)
		Me.Label11.Location = New System.Drawing.Point(8, 4)
		Me.Label11.TabIndex = 0
		Me.Label11.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label11.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label11.BackColor = System.Drawing.SystemColors.Control
		Me.Label11.Enabled = True
		Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label11.UseMnemonic = True
		Me.Label11.Visible = True
		Me.Label11.AutoSize = False
		Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label11.Name = "Label11"
		Me.Label8.Text = "Click on a memorized transaction in the list below, then edit it in the boxes at the bottom. Then click any button or another transaction to copy your changes back up to the list. Click ""Save Changes"" to make your accumulated changes permanent and close this dialog, or click ""Discard Changes"" to discard your accumulated changes and close this dialog."
		Me.Label8.Size = New System.Drawing.Size(689, 49)
		Me.Label8.Location = New System.Drawing.Point(8, 40)
		Me.Label8.TabIndex = 1
		Me.Label8.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label8.BackColor = System.Drawing.SystemColors.Control
		Me.Label8.Enabled = True
		Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label8.UseMnemonic = True
		Me.Label8.Visible = True
		Me.Label8.AutoSize = False
		Me.Label8.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label8.Name = "Label8"
		Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label6.Text = "Budget:"
		Me.Label6.Size = New System.Drawing.Size(53, 17)
		Me.Label6.Location = New System.Drawing.Point(128, 502)
		Me.Label6.TabIndex = 23
		Me.Label6.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label6.BackColor = System.Drawing.SystemColors.Control
		Me.Label6.Enabled = True
		Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label6.UseMnemonic = True
		Me.Label6.Visible = True
		Me.Label6.AutoSize = False
		Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label6.Name = "Label6"
		Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label5.Text = "Category:"
		Me.Label5.Size = New System.Drawing.Size(53, 17)
		Me.Label5.Location = New System.Drawing.Point(128, 478)
		Me.Label5.TabIndex = 21
		Me.Label5.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label5.BackColor = System.Drawing.SystemColors.Control
		Me.Label5.Enabled = True
		Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label5.UseMnemonic = True
		Me.Label5.Visible = True
		Me.Label5.AutoSize = False
		Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label5.Name = "Label5"
		Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label4.Text = "Memo:"
		Me.Label4.Size = New System.Drawing.Size(43, 17)
		Me.Label4.Location = New System.Drawing.Point(138, 452)
		Me.Label4.TabIndex = 19
		Me.Label4.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label4.BackColor = System.Drawing.SystemColors.Control
		Me.Label4.Enabled = True
		Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label4.UseMnemonic = True
		Me.Label4.Visible = True
		Me.Label4.AutoSize = False
		Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label4.Name = "Label4"
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label3.Text = "Amount:"
		Me.Label3.Size = New System.Drawing.Size(45, 17)
		Me.Label3.Location = New System.Drawing.Point(532, 348)
		Me.Label3.TabIndex = 7
		Me.Label3.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
		Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label2.Text = "Name:"
		Me.Label2.Size = New System.Drawing.Size(43, 17)
		Me.Label2.Location = New System.Drawing.Point(138, 348)
		Me.Label2.TabIndex = 5
		Me.Label2.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
		Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
		Me.Label1.Text = "Number:"
		Me.Label1.Size = New System.Drawing.Size(45, 17)
		Me.Label1.Location = New System.Drawing.Point(10, 348)
		Me.Label1.TabIndex = 3
		Me.Label1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
		Me.Controls.Add(txtAccount)
		Me.Controls.Add(txtZip)
		Me.Controls.Add(txtState)
		Me.Controls.Add(txtCity)
		Me.Controls.Add(txtAddress2)
		Me.Controls.Add(txtAddress1)
		Me.Controls.Add(cmdDiscardChanges)
		Me.Controls.Add(cmdSaveChanges)
		Me.Controls.Add(cmdDeletePayee)
		Me.Controls.Add(cmdNewPayee)
		Me.Controls.Add(cboBudget)
		Me.Controls.Add(cboCategory)
		Me.Controls.Add(txtMemo)
		Me.Controls.Add(txtAmount)
		Me.Controls.Add(txtPayee)
		Me.Controls.Add(txtNumber)
		Me.Controls.Add(lvwPayees)
		Me.Controls.Add(fraImport)
		Me.Controls.Add(Label15)
		Me.Controls.Add(Label14)
		Me.Controls.Add(Label13)
		Me.Controls.Add(Label12)
		Me.Controls.Add(Label11)
		Me.Controls.Add(Label8)
		Me.Controls.Add(Label6)
		Me.Controls.Add(Label5)
		Me.Controls.Add(Label4)
		Me.Controls.Add(Label3)
		Me.Controls.Add(Label2)
		Me.Controls.Add(Label1)
		Me.fraImport.Controls.Add(txtMaxAmount)
		Me.fraImport.Controls.Add(txtMinAmount)
		Me.fraImport.Controls.Add(txtBank)
		Me.fraImport.Controls.Add(Label10)
		Me.fraImport.Controls.Add(Label9)
		Me.fraImport.Controls.Add(Label7)
		Me.fraImport.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class