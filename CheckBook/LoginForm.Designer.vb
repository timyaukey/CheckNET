<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class LoginForm
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
	Public WithEvents txtPassword As System.Windows.Forms.TextBox
	Public WithEvents txtLogin As System.Windows.Forms.TextBox
	Public WithEvents lblPassword As System.Windows.Forms.Label
	Public WithEvents lblLogin As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(LoginForm))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOkay = New System.Windows.Forms.Button
		Me.txtPassword = New System.Windows.Forms.TextBox
		Me.txtLogin = New System.Windows.Forms.TextBox
		Me.lblPassword = New System.Windows.Forms.Label
		Me.lblLogin = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "Enter Login Name and Password"
		Me.ClientSize = New System.Drawing.Size(216, 108)
		Me.Location = New System.Drawing.Point(3, 23)
		Me.ControlBox = False
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.Enabled = True
		Me.KeyPreview = False
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "LoginForm"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cmdCancel
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.Size = New System.Drawing.Size(65, 25)
		Me.cmdCancel.Location = New System.Drawing.Point(138, 70)
		Me.cmdCancel.TabIndex = 5
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
		Me.cmdOkay.Size = New System.Drawing.Size(65, 25)
		Me.cmdOkay.Location = New System.Drawing.Point(70, 70)
		Me.cmdOkay.TabIndex = 4
		Me.cmdOkay.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdOkay.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOkay.CausesValidation = True
		Me.cmdOkay.Enabled = True
		Me.cmdOkay.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOkay.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOkay.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOkay.TabStop = True
		Me.cmdOkay.Name = "cmdOkay"
		Me.txtPassword.AutoSize = False
		Me.txtPassword.Size = New System.Drawing.Size(133, 23)
		Me.txtPassword.IMEMode = System.Windows.Forms.ImeMode.Disable
		Me.txtPassword.Location = New System.Drawing.Point(70, 40)
		Me.txtPassword.PasswordChar = ChrW(42)
		Me.txtPassword.TabIndex = 3
		Me.txtPassword.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtPassword.AcceptsReturn = True
		Me.txtPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPassword.BackColor = System.Drawing.SystemColors.Window
		Me.txtPassword.CausesValidation = True
		Me.txtPassword.Enabled = True
		Me.txtPassword.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPassword.HideSelection = True
		Me.txtPassword.ReadOnly = False
		Me.txtPassword.Maxlength = 0
		Me.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPassword.MultiLine = False
		Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPassword.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPassword.TabStop = True
		Me.txtPassword.Visible = True
		Me.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPassword.Name = "txtPassword"
		Me.txtLogin.AutoSize = False
		Me.txtLogin.Size = New System.Drawing.Size(133, 23)
		Me.txtLogin.Location = New System.Drawing.Point(70, 12)
		Me.txtLogin.TabIndex = 1
		Me.txtLogin.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtLogin.AcceptsReturn = True
		Me.txtLogin.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtLogin.BackColor = System.Drawing.SystemColors.Window
		Me.txtLogin.CausesValidation = True
		Me.txtLogin.Enabled = True
		Me.txtLogin.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtLogin.HideSelection = True
		Me.txtLogin.ReadOnly = False
		Me.txtLogin.Maxlength = 0
		Me.txtLogin.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtLogin.MultiLine = False
		Me.txtLogin.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtLogin.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtLogin.TabStop = True
		Me.txtLogin.Visible = True
		Me.txtLogin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtLogin.Name = "txtLogin"
		Me.lblPassword.Text = "Password:"
		Me.lblPassword.Size = New System.Drawing.Size(53, 21)
		Me.lblPassword.Location = New System.Drawing.Point(12, 40)
		Me.lblPassword.TabIndex = 2
		Me.lblPassword.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblPassword.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPassword.BackColor = System.Drawing.SystemColors.Control
		Me.lblPassword.Enabled = True
		Me.lblPassword.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPassword.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPassword.UseMnemonic = True
		Me.lblPassword.Visible = True
		Me.lblPassword.AutoSize = False
		Me.lblPassword.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPassword.Name = "lblPassword"
		Me.lblLogin.Text = "Login:"
		Me.lblLogin.Size = New System.Drawing.Size(41, 21)
		Me.lblLogin.Location = New System.Drawing.Point(12, 14)
		Me.lblLogin.TabIndex = 0
		Me.lblLogin.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblLogin.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblLogin.BackColor = System.Drawing.SystemColors.Control
		Me.lblLogin.Enabled = True
		Me.lblLogin.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblLogin.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblLogin.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblLogin.UseMnemonic = True
		Me.lblLogin.Visible = True
		Me.lblLogin.AutoSize = False
		Me.lblLogin.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblLogin.Name = "lblLogin"
		Me.Controls.Add(cmdCancel)
		Me.Controls.Add(cmdOkay)
		Me.Controls.Add(txtPassword)
		Me.Controls.Add(txtLogin)
		Me.Controls.Add(lblPassword)
		Me.Controls.Add(lblLogin)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class