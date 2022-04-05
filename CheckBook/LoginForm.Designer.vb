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
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdOkay = New System.Windows.Forms.Button()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.txtLogin = New System.Windows.Forms.TextBox()
        Me.lblPassword = New System.Windows.Forms.Label()
        Me.lblLogin = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(183, 70)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(65, 25)
        Me.cmdCancel.TabIndex = 5
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdOkay
        '
        Me.cmdOkay.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOkay.BackColor = System.Drawing.SystemColors.Control
        Me.cmdOkay.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdOkay.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOkay.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdOkay.Location = New System.Drawing.Point(115, 70)
        Me.cmdOkay.Name = "cmdOkay"
        Me.cmdOkay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOkay.Size = New System.Drawing.Size(65, 25)
        Me.cmdOkay.TabIndex = 4
        Me.cmdOkay.Text = "OK"
        Me.cmdOkay.UseVisualStyleBackColor = False
        '
        'txtPassword
        '
        Me.txtPassword.AcceptsReturn = True
        Me.txtPassword.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPassword.BackColor = System.Drawing.SystemColors.Window
        Me.txtPassword.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPassword.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPassword.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPassword.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtPassword.Location = New System.Drawing.Point(86, 40)
        Me.txtPassword.MaxLength = 0
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPassword.Size = New System.Drawing.Size(162, 20)
        Me.txtPassword.TabIndex = 3
        '
        'txtLogin
        '
        Me.txtLogin.AcceptsReturn = True
        Me.txtLogin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLogin.BackColor = System.Drawing.SystemColors.Window
        Me.txtLogin.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtLogin.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtLogin.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtLogin.Location = New System.Drawing.Point(86, 12)
        Me.txtLogin.MaxLength = 0
        Me.txtLogin.Name = "txtLogin"
        Me.txtLogin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLogin.Size = New System.Drawing.Size(162, 20)
        Me.txtLogin.TabIndex = 1
        '
        'lblPassword
        '
        Me.lblPassword.BackColor = System.Drawing.SystemColors.Control
        Me.lblPassword.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPassword.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPassword.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPassword.Location = New System.Drawing.Point(12, 40)
        Me.lblPassword.Name = "lblPassword"
        Me.lblPassword.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPassword.Size = New System.Drawing.Size(68, 21)
        Me.lblPassword.TabIndex = 2
        Me.lblPassword.Text = "Password:"
        '
        'lblLogin
        '
        Me.lblLogin.BackColor = System.Drawing.SystemColors.Control
        Me.lblLogin.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblLogin.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLogin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblLogin.Location = New System.Drawing.Point(12, 14)
        Me.lblLogin.Name = "lblLogin"
        Me.lblLogin.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLogin.Size = New System.Drawing.Size(41, 21)
        Me.lblLogin.TabIndex = 0
        Me.lblLogin.Text = "Login:"
        '
        'LoginForm
        '
        Me.AcceptButton = Me.cmdOkay
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(261, 108)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOkay)
        Me.Controls.Add(Me.txtPassword)
        Me.Controls.Add(Me.txtLogin)
        Me.Controls.Add(Me.lblPassword)
        Me.Controls.Add(Me.lblLogin)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(3, 23)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Enter Login Name and Password"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
#End Region
End Class