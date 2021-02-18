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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PayeeListForm))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtAccount = New System.Windows.Forms.TextBox()
        Me.txtZip = New System.Windows.Forms.TextBox()
        Me.txtState = New System.Windows.Forms.TextBox()
        Me.txtCity = New System.Windows.Forms.TextBox()
        Me.txtAddress2 = New System.Windows.Forms.TextBox()
        Me.txtAddress1 = New System.Windows.Forms.TextBox()
        Me.cmdDiscardChanges = New System.Windows.Forms.Button()
        Me.cmdSaveChanges = New System.Windows.Forms.Button()
        Me.cmdDeletePayee = New System.Windows.Forms.Button()
        Me.cmdNewPayee = New System.Windows.Forms.Button()
        Me.cboBudget = New System.Windows.Forms.ComboBox()
        Me.cboCategory = New System.Windows.Forms.ComboBox()
        Me.txtMemo = New System.Windows.Forms.TextBox()
        Me.txtAmount = New System.Windows.Forms.TextBox()
        Me.txtPayee = New System.Windows.Forms.TextBox()
        Me.txtNumber = New System.Windows.Forms.TextBox()
        Me.lvwPayees = New System.Windows.Forms.ListView()
        Me.fraImport = New System.Windows.Forms.GroupBox()
        Me.cboNarrowMethod = New System.Windows.Forms.ComboBox()
        Me.lblMatchMethod = New System.Windows.Forms.Label()
        Me.txtMaxAmount = New System.Windows.Forms.TextBox()
        Me.txtMinAmount = New System.Windows.Forms.TextBox()
        Me.txtBank = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkAllowAutoBatchNew = New System.Windows.Forms.CheckBox()
        Me.chkAllowAutoBatchUpdate = New System.Windows.Forms.CheckBox()
        Me.fraImport.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtAccount
        '
        Me.txtAccount.AcceptsReturn = True
        Me.txtAccount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAccount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAccount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAccount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAccount.Location = New System.Drawing.Point(580, 354)
        Me.txtAccount.MaxLength = 0
        Me.txtAccount.Name = "txtAccount"
        Me.txtAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAccount.Size = New System.Drawing.Size(77, 20)
        Me.txtAccount.TabIndex = 23
        '
        'txtZip
        '
        Me.txtZip.AcceptsReturn = True
        Me.txtZip.BackColor = System.Drawing.SystemColors.Window
        Me.txtZip.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtZip.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtZip.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtZip.Location = New System.Drawing.Point(416, 380)
        Me.txtZip.MaxLength = 0
        Me.txtZip.Name = "txtZip"
        Me.txtZip.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtZip.Size = New System.Drawing.Size(81, 20)
        Me.txtZip.TabIndex = 13
        '
        'txtState
        '
        Me.txtState.AcceptsReturn = True
        Me.txtState.BackColor = System.Drawing.SystemColors.Window
        Me.txtState.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtState.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtState.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtState.Location = New System.Drawing.Point(374, 380)
        Me.txtState.MaxLength = 0
        Me.txtState.Name = "txtState"
        Me.txtState.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtState.Size = New System.Drawing.Size(39, 20)
        Me.txtState.TabIndex = 12
        '
        'txtCity
        '
        Me.txtCity.AcceptsReturn = True
        Me.txtCity.BackColor = System.Drawing.SystemColors.Window
        Me.txtCity.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtCity.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCity.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCity.Location = New System.Drawing.Point(184, 380)
        Me.txtCity.MaxLength = 0
        Me.txtCity.Name = "txtCity"
        Me.txtCity.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtCity.Size = New System.Drawing.Size(187, 20)
        Me.txtCity.TabIndex = 11
        '
        'txtAddress2
        '
        Me.txtAddress2.AcceptsReturn = True
        Me.txtAddress2.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress2.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress2.Location = New System.Drawing.Point(374, 354)
        Me.txtAddress2.MaxLength = 0
        Me.txtAddress2.Name = "txtAddress2"
        Me.txtAddress2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress2.Size = New System.Drawing.Size(123, 20)
        Me.txtAddress2.TabIndex = 9
        '
        'txtAddress1
        '
        Me.txtAddress1.AcceptsReturn = True
        Me.txtAddress1.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddress1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddress1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddress1.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddress1.Location = New System.Drawing.Point(184, 354)
        Me.txtAddress1.MaxLength = 0
        Me.txtAddress1.Name = "txtAddress1"
        Me.txtAddress1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddress1.Size = New System.Drawing.Size(187, 20)
        Me.txtAddress1.TabIndex = 8
        '
        'cmdDiscardChanges
        '
        Me.cmdDiscardChanges.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDiscardChanges.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDiscardChanges.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDiscardChanges.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDiscardChanges.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDiscardChanges.Location = New System.Drawing.Point(531, 596)
        Me.cmdDiscardChanges.Name = "cmdDiscardChanges"
        Me.cmdDiscardChanges.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDiscardChanges.Size = New System.Drawing.Size(141, 23)
        Me.cmdDiscardChanges.TabIndex = 27
        Me.cmdDiscardChanges.Text = "Discard Changes"
        Me.cmdDiscardChanges.UseVisualStyleBackColor = False
        '
        'cmdSaveChanges
        '
        Me.cmdSaveChanges.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSaveChanges.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSaveChanges.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSaveChanges.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSaveChanges.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSaveChanges.Location = New System.Drawing.Point(531, 624)
        Me.cmdSaveChanges.Name = "cmdSaveChanges"
        Me.cmdSaveChanges.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSaveChanges.Size = New System.Drawing.Size(141, 23)
        Me.cmdSaveChanges.TabIndex = 28
        Me.cmdSaveChanges.Text = "Save Changes"
        Me.cmdSaveChanges.UseVisualStyleBackColor = False
        '
        'cmdDeletePayee
        '
        Me.cmdDeletePayee.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdDeletePayee.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeletePayee.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeletePayee.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeletePayee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeletePayee.Location = New System.Drawing.Point(531, 568)
        Me.cmdDeletePayee.Name = "cmdDeletePayee"
        Me.cmdDeletePayee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeletePayee.Size = New System.Drawing.Size(141, 23)
        Me.cmdDeletePayee.TabIndex = 26
        Me.cmdDeletePayee.Text = "Delete Transaction"
        Me.cmdDeletePayee.UseVisualStyleBackColor = False
        '
        'cmdNewPayee
        '
        Me.cmdNewPayee.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdNewPayee.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewPayee.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewPayee.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewPayee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewPayee.Location = New System.Drawing.Point(531, 540)
        Me.cmdNewPayee.Name = "cmdNewPayee"
        Me.cmdNewPayee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewPayee.Size = New System.Drawing.Size(141, 23)
        Me.cmdNewPayee.TabIndex = 25
        Me.cmdNewPayee.Text = "New Transaction"
        Me.cmdNewPayee.UseVisualStyleBackColor = False
        '
        'cboBudget
        '
        Me.cboBudget.BackColor = System.Drawing.SystemColors.Window
        Me.cboBudget.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboBudget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboBudget.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboBudget.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboBudget.Location = New System.Drawing.Point(184, 456)
        Me.cboBudget.Name = "cboBudget"
        Me.cboBudget.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboBudget.Size = New System.Drawing.Size(313, 22)
        Me.cboBudget.TabIndex = 19
        '
        'cboCategory
        '
        Me.cboCategory.BackColor = System.Drawing.SystemColors.Window
        Me.cboCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCategory.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboCategory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboCategory.Location = New System.Drawing.Point(184, 432)
        Me.cboCategory.Name = "cboCategory"
        Me.cboCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboCategory.Size = New System.Drawing.Size(313, 22)
        Me.cboCategory.TabIndex = 17
        '
        'txtMemo
        '
        Me.txtMemo.AcceptsReturn = True
        Me.txtMemo.BackColor = System.Drawing.SystemColors.Window
        Me.txtMemo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMemo.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMemo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMemo.Location = New System.Drawing.Point(184, 406)
        Me.txtMemo.MaxLength = 0
        Me.txtMemo.Name = "txtMemo"
        Me.txtMemo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMemo.Size = New System.Drawing.Size(313, 20)
        Me.txtMemo.TabIndex = 15
        '
        'txtAmount
        '
        Me.txtAmount.AcceptsReturn = True
        Me.txtAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAmount.Location = New System.Drawing.Point(580, 328)
        Me.txtAmount.MaxLength = 0
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAmount.Size = New System.Drawing.Size(77, 20)
        Me.txtAmount.TabIndex = 21
        '
        'txtPayee
        '
        Me.txtPayee.AcceptsReturn = True
        Me.txtPayee.BackColor = System.Drawing.SystemColors.Window
        Me.txtPayee.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPayee.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPayee.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPayee.Location = New System.Drawing.Point(184, 328)
        Me.txtPayee.MaxLength = 0
        Me.txtPayee.Name = "txtPayee"
        Me.txtPayee.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPayee.Size = New System.Drawing.Size(313, 20)
        Me.txtPayee.TabIndex = 6
        '
        'txtNumber
        '
        Me.txtNumber.AcceptsReturn = True
        Me.txtNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNumber.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNumber.Location = New System.Drawing.Point(73, 328)
        Me.txtNumber.MaxLength = 0
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNumber.Size = New System.Drawing.Size(49, 20)
        Me.txtNumber.TabIndex = 4
        '
        'lvwPayees
        '
        Me.lvwPayees.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwPayees.BackColor = System.Drawing.SystemColors.Window
        Me.lvwPayees.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwPayees.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwPayees.HideSelection = False
        Me.lvwPayees.LabelEdit = True
        Me.lvwPayees.Location = New System.Drawing.Point(4, 92)
        Me.lvwPayees.Name = "lvwPayees"
        Me.lvwPayees.Size = New System.Drawing.Size(668, 228)
        Me.lvwPayees.TabIndex = 2
        Me.lvwPayees.UseCompatibleStateImageBehavior = False
        '
        'fraImport
        '
        Me.fraImport.BackColor = System.Drawing.SystemColors.Control
        Me.fraImport.Controls.Add(Me.chkAllowAutoBatchUpdate)
        Me.fraImport.Controls.Add(Me.chkAllowAutoBatchNew)
        Me.fraImport.Controls.Add(Me.cboNarrowMethod)
        Me.fraImport.Controls.Add(Me.lblMatchMethod)
        Me.fraImport.Controls.Add(Me.txtMaxAmount)
        Me.fraImport.Controls.Add(Me.txtMinAmount)
        Me.fraImport.Controls.Add(Me.txtBank)
        Me.fraImport.Controls.Add(Me.Label10)
        Me.fraImport.Controls.Add(Me.Label9)
        Me.fraImport.Controls.Add(Me.Label7)
        Me.fraImport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.fraImport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraImport.Location = New System.Drawing.Point(12, 497)
        Me.fraImport.Name = "fraImport"
        Me.fraImport.Padding = New System.Windows.Forms.Padding(0)
        Me.fraImport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.fraImport.Size = New System.Drawing.Size(478, 150)
        Me.fraImport.TabIndex = 24
        Me.fraImport.TabStop = False
        Me.fraImport.Text = "Information Used When Importing Transactions"
        '
        'cboNarrowMethod
        '
        Me.cboNarrowMethod.BackColor = System.Drawing.SystemColors.Window
        Me.cboNarrowMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboNarrowMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboNarrowMethod.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboNarrowMethod.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboNarrowMethod.Location = New System.Drawing.Point(171, 73)
        Me.cboNarrowMethod.Name = "cboNarrowMethod"
        Me.cboNarrowMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboNarrowMethod.Size = New System.Drawing.Size(299, 22)
        Me.cboNarrowMethod.TabIndex = 7
        '
        'lblMatchMethod
        '
        Me.lblMatchMethod.BackColor = System.Drawing.SystemColors.Control
        Me.lblMatchMethod.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMatchMethod.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMatchMethod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMatchMethod.Location = New System.Drawing.Point(20, 76)
        Me.lblMatchMethod.Name = "lblMatchMethod"
        Me.lblMatchMethod.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMatchMethod.Size = New System.Drawing.Size(145, 19)
        Me.lblMatchMethod.TabIndex = 6
        Me.lblMatchMethod.Text = "Match Narrowing Method:"
        Me.lblMatchMethod.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'txtMaxAmount
        '
        Me.txtMaxAmount.AcceptsReturn = True
        Me.txtMaxAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtMaxAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMaxAmount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMaxAmount.Location = New System.Drawing.Point(393, 47)
        Me.txtMaxAmount.MaxLength = 0
        Me.txtMaxAmount.Name = "txtMaxAmount"
        Me.txtMaxAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMaxAmount.Size = New System.Drawing.Size(77, 20)
        Me.txtMaxAmount.TabIndex = 5
        '
        'txtMinAmount
        '
        Me.txtMinAmount.AcceptsReturn = True
        Me.txtMinAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtMinAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMinAmount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMinAmount.Location = New System.Drawing.Point(171, 47)
        Me.txtMinAmount.MaxLength = 0
        Me.txtMinAmount.Name = "txtMinAmount"
        Me.txtMinAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMinAmount.Size = New System.Drawing.Size(77, 20)
        Me.txtMinAmount.TabIndex = 3
        '
        'txtBank
        '
        Me.txtBank.AcceptsReturn = True
        Me.txtBank.BackColor = System.Drawing.SystemColors.Window
        Me.txtBank.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBank.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBank.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBank.Location = New System.Drawing.Point(171, 21)
        Me.txtBank.MaxLength = 0
        Me.txtBank.Name = "txtBank"
        Me.txtBank.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBank.Size = New System.Drawing.Size(299, 20)
        Me.txtBank.TabIndex = 1
        '
        'Label10
        '
        Me.Label10.BackColor = System.Drawing.SystemColors.Control
        Me.Label10.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label10.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(268, 50)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label10.Size = New System.Drawing.Size(119, 17)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "Max Amount To Match:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label9
        '
        Me.Label9.BackColor = System.Drawing.SystemColors.Control
        Me.Label9.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label9.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(42, 50)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label9.Size = New System.Drawing.Size(119, 17)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Min Amount To Match:"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label7
        '
        Me.Label7.BackColor = System.Drawing.SystemColors.Control
        Me.Label7.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label7.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label7.Location = New System.Drawing.Point(34, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label7.Size = New System.Drawing.Size(127, 17)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Name Used By Bank:"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label15
        '
        Me.Label15.BackColor = System.Drawing.SystemColors.Control
        Me.Label15.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label15.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label15.Location = New System.Drawing.Point(512, 356)
        Me.Label15.Name = "Label15"
        Me.Label15.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label15.Size = New System.Drawing.Size(65, 17)
        Me.Label15.TabIndex = 22
        Me.Label15.Text = "Account #:"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label14
        '
        Me.Label14.BackColor = System.Drawing.SystemColors.Control
        Me.Label14.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label14.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label14.Location = New System.Drawing.Point(66, 382)
        Me.Label14.Name = "Label14"
        Me.Label14.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label14.Size = New System.Drawing.Size(115, 17)
        Me.Label14.TabIndex = 10
        Me.Label14.Text = "City/State/Zip:"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label12
        '
        Me.Label12.BackColor = System.Drawing.SystemColors.Control
        Me.Label12.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label12.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(70, 356)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label12.Size = New System.Drawing.Size(111, 17)
        Me.Label12.TabIndex = 7
        Me.Label12.Text = "Address Line 1/2:"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.SystemColors.Control
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(8, 4)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(664, 31)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = resources.GetString("Label11.Text")
        '
        'Label8
        '
        Me.Label8.BackColor = System.Drawing.SystemColors.Control
        Me.Label8.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label8.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(8, 40)
        Me.Label8.Name = "Label8"
        Me.Label8.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label8.Size = New System.Drawing.Size(664, 49)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = resources.GetString("Label8.Text")
        '
        'Label6
        '
        Me.Label6.BackColor = System.Drawing.SystemColors.Control
        Me.Label6.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label6.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(128, 458)
        Me.Label6.Name = "Label6"
        Me.Label6.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label6.Size = New System.Drawing.Size(53, 17)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "Budget:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(128, 434)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(53, 17)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Category:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(138, 408)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(43, 17)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Memo:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(519, 331)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(58, 16)
        Me.Label3.TabIndex = 20
        Me.Label3.Text = "Amount:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(138, 330)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(43, 17)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Name:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(10, 330)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(57, 18)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Number:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkAllowAutoBatchNew
        '
        Me.chkAllowAutoBatchNew.AutoSize = True
        Me.chkAllowAutoBatchNew.Location = New System.Drawing.Point(171, 101)
        Me.chkAllowAutoBatchNew.Name = "chkAllowAutoBatchNew"
        Me.chkAllowAutoBatchNew.Size = New System.Drawing.Size(227, 18)
        Me.chkAllowAutoBatchNew.TabIndex = 8
        Me.chkAllowAutoBatchNew.Text = "Allow auto selection by ""Find Batch New"""
        Me.chkAllowAutoBatchNew.UseVisualStyleBackColor = True
        '
        'chkAllowAutoBatchUpdate
        '
        Me.chkAllowAutoBatchUpdate.AutoSize = True
        Me.chkAllowAutoBatchUpdate.Location = New System.Drawing.Point(171, 125)
        Me.chkAllowAutoBatchUpdate.Name = "chkAllowAutoBatchUpdate"
        Me.chkAllowAutoBatchUpdate.Size = New System.Drawing.Size(244, 18)
        Me.chkAllowAutoBatchUpdate.TabIndex = 9
        Me.chkAllowAutoBatchUpdate.Text = "Allow auto selection by ""Find Batch Updates"""
        Me.chkAllowAutoBatchUpdate.UseVisualStyleBackColor = True
        '
        'PayeeListForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(684, 659)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtAccount)
        Me.Controls.Add(Me.txtZip)
        Me.Controls.Add(Me.txtState)
        Me.Controls.Add(Me.txtCity)
        Me.Controls.Add(Me.txtAddress2)
        Me.Controls.Add(Me.txtAddress1)
        Me.Controls.Add(Me.cmdDiscardChanges)
        Me.Controls.Add(Me.cmdSaveChanges)
        Me.Controls.Add(Me.cmdDeletePayee)
        Me.Controls.Add(Me.cmdNewPayee)
        Me.Controls.Add(Me.cboBudget)
        Me.Controls.Add(Me.cboCategory)
        Me.Controls.Add(Me.txtMemo)
        Me.Controls.Add(Me.txtAmount)
        Me.Controls.Add(Me.txtPayee)
        Me.Controls.Add(Me.txtNumber)
        Me.Controls.Add(Me.lvwPayees)
        Me.Controls.Add(Me.fraImport)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 23)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PayeeListForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Memorized Transaction List"
        Me.fraImport.ResumeLayout(False)
        Me.fraImport.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents cboNarrowMethod As System.Windows.Forms.ComboBox
    Public WithEvents lblMatchMethod As System.Windows.Forms.Label
    Friend WithEvents chkAllowAutoBatchUpdate As CheckBox
    Friend WithEvents chkAllowAutoBatchNew As CheckBox
#End Region
End Class