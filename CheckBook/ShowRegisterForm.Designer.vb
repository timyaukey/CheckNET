<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class ShowRegisterForm
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
	Public WithEvents cmdRegen As System.Windows.Forms.Button
	Public WithEvents cmdDeleteAccount As System.Windows.Forms.Button
	Public WithEvents cmdDeleteRegister As System.Windows.Forms.Button
	Public WithEvents cmdRegProperties As System.Windows.Forms.Button
	Public WithEvents cmdSetAccountName As System.Windows.Forms.Button
	Public WithEvents cmdNewAccount As System.Windows.Forms.Button
	Public WithEvents cmdNewRegister As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdShowRegister As System.Windows.Forms.Button
	Public WithEvents lstRegisters As System.Windows.Forms.ListBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ShowRegisterForm))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdRegen = New System.Windows.Forms.Button
		Me.cmdDeleteAccount = New System.Windows.Forms.Button
		Me.cmdDeleteRegister = New System.Windows.Forms.Button
		Me.cmdRegProperties = New System.Windows.Forms.Button
		Me.cmdSetAccountName = New System.Windows.Forms.Button
		Me.cmdNewAccount = New System.Windows.Forms.Button
		Me.cmdNewRegister = New System.Windows.Forms.Button
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdShowRegister = New System.Windows.Forms.Button
		Me.lstRegisters = New System.Windows.Forms.ListBox
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "Registers and Accounts"
		Me.ClientSize = New System.Drawing.Size(434, 364)
		Me.Location = New System.Drawing.Point(2, 22)
		Me.MaximizeBox = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
		Me.MinimizeBox = False
		Me.ShowInTaskbar = False
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
		Me.Name = "ShowRegisterForm"
		Me.cmdRegen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRegen.Text = "Regenerate Trans"
		Me.cmdRegen.Enabled = False
		Me.cmdRegen.Size = New System.Drawing.Size(136, 23)
		Me.cmdRegen.Location = New System.Drawing.Point(8, 332)
		Me.cmdRegen.TabIndex = 4
		Me.cmdRegen.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdRegen.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRegen.CausesValidation = True
		Me.cmdRegen.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRegen.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRegen.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRegen.TabStop = True
		Me.cmdRegen.Name = "cmdRegen"
		Me.cmdDeleteAccount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDeleteAccount.Text = "Delete Account"
		Me.cmdDeleteAccount.Enabled = False
		Me.cmdDeleteAccount.Size = New System.Drawing.Size(136, 23)
		Me.cmdDeleteAccount.Location = New System.Drawing.Point(8, 306)
		Me.cmdDeleteAccount.TabIndex = 3
		Me.cmdDeleteAccount.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdDeleteAccount.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDeleteAccount.CausesValidation = True
		Me.cmdDeleteAccount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDeleteAccount.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDeleteAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDeleteAccount.TabStop = True
		Me.cmdDeleteAccount.Name = "cmdDeleteAccount"
		Me.cmdDeleteRegister.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDeleteRegister.Text = "Delete Register"
		Me.cmdDeleteRegister.Enabled = False
		Me.cmdDeleteRegister.Size = New System.Drawing.Size(136, 23)
		Me.cmdDeleteRegister.Location = New System.Drawing.Point(148, 306)
		Me.cmdDeleteRegister.TabIndex = 7
		Me.cmdDeleteRegister.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdDeleteRegister.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDeleteRegister.CausesValidation = True
		Me.cmdDeleteRegister.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDeleteRegister.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDeleteRegister.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDeleteRegister.TabStop = True
		Me.cmdDeleteRegister.Name = "cmdDeleteRegister"
		Me.cmdRegProperties.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdRegProperties.Text = "Register Properties"
		Me.cmdRegProperties.Enabled = False
		Me.cmdRegProperties.Size = New System.Drawing.Size(136, 23)
		Me.cmdRegProperties.Location = New System.Drawing.Point(148, 280)
		Me.cmdRegProperties.TabIndex = 6
		Me.cmdRegProperties.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdRegProperties.BackColor = System.Drawing.SystemColors.Control
		Me.cmdRegProperties.CausesValidation = True
		Me.cmdRegProperties.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdRegProperties.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdRegProperties.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdRegProperties.TabStop = True
		Me.cmdRegProperties.Name = "cmdRegProperties"
		Me.cmdSetAccountName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSetAccountName.Text = "Set Account Title"
		Me.cmdSetAccountName.Enabled = False
		Me.cmdSetAccountName.Size = New System.Drawing.Size(136, 23)
		Me.cmdSetAccountName.Location = New System.Drawing.Point(8, 280)
		Me.cmdSetAccountName.TabIndex = 2
		Me.cmdSetAccountName.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdSetAccountName.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSetAccountName.CausesValidation = True
		Me.cmdSetAccountName.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSetAccountName.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSetAccountName.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSetAccountName.TabStop = True
		Me.cmdSetAccountName.Name = "cmdSetAccountName"
		Me.cmdNewAccount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNewAccount.Text = "New Account"
		Me.cmdNewAccount.Size = New System.Drawing.Size(136, 23)
		Me.cmdNewAccount.Location = New System.Drawing.Point(8, 254)
		Me.cmdNewAccount.TabIndex = 1
		Me.cmdNewAccount.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdNewAccount.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNewAccount.CausesValidation = True
		Me.cmdNewAccount.Enabled = True
		Me.cmdNewAccount.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNewAccount.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNewAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNewAccount.TabStop = True
		Me.cmdNewAccount.Name = "cmdNewAccount"
		Me.cmdNewRegister.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNewRegister.Text = "New Register"
		Me.cmdNewRegister.Enabled = False
		Me.cmdNewRegister.Size = New System.Drawing.Size(136, 23)
		Me.cmdNewRegister.Location = New System.Drawing.Point(148, 254)
		Me.cmdNewRegister.TabIndex = 5
		Me.cmdNewRegister.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdNewRegister.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNewRegister.CausesValidation = True
		Me.cmdNewRegister.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNewRegister.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNewRegister.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNewRegister.TabStop = True
		Me.cmdNewRegister.Name = "cmdNewRegister"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cmdCancel
		Me.cmdCancel.Text = "Close"
		Me.cmdCancel.Size = New System.Drawing.Size(136, 23)
		Me.cmdCancel.Location = New System.Drawing.Point(288, 332)
		Me.cmdCancel.TabIndex = 9
		Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
		Me.cmdCancel.CausesValidation = True
		Me.cmdCancel.Enabled = True
		Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdCancel.TabStop = True
		Me.cmdCancel.Name = "cmdCancel"
		Me.cmdShowRegister.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdShowRegister.Text = "Show Register"
		Me.cmdShowRegister.Enabled = False
		Me.cmdShowRegister.Size = New System.Drawing.Size(136, 23)
		Me.cmdShowRegister.Location = New System.Drawing.Point(148, 332)
		Me.cmdShowRegister.TabIndex = 8
		Me.cmdShowRegister.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdShowRegister.BackColor = System.Drawing.SystemColors.Control
		Me.cmdShowRegister.CausesValidation = True
		Me.cmdShowRegister.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdShowRegister.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdShowRegister.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdShowRegister.TabStop = True
		Me.cmdShowRegister.Name = "cmdShowRegister"
		Me.lstRegisters.Size = New System.Drawing.Size(423, 241)
		Me.lstRegisters.Location = New System.Drawing.Point(5, 5)
		Me.lstRegisters.TabIndex = 0
		Me.lstRegisters.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lstRegisters.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lstRegisters.BackColor = System.Drawing.SystemColors.Window
		Me.lstRegisters.CausesValidation = True
		Me.lstRegisters.Enabled = True
		Me.lstRegisters.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstRegisters.IntegralHeight = True
		Me.lstRegisters.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstRegisters.SelectionMode = System.Windows.Forms.SelectionMode.One
		Me.lstRegisters.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstRegisters.Sorted = False
		Me.lstRegisters.TabStop = True
		Me.lstRegisters.Visible = True
		Me.lstRegisters.MultiColumn = False
		Me.lstRegisters.Name = "lstRegisters"
		Me.Controls.Add(cmdRegen)
		Me.Controls.Add(cmdDeleteAccount)
		Me.Controls.Add(cmdDeleteRegister)
		Me.Controls.Add(cmdRegProperties)
		Me.Controls.Add(cmdSetAccountName)
		Me.Controls.Add(cmdNewAccount)
		Me.Controls.Add(cmdNewRegister)
		Me.Controls.Add(cmdCancel)
		Me.Controls.Add(cmdShowRegister)
		Me.Controls.Add(lstRegisters)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class