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
        Me.MdiParent = CBMainForm
        CBMainForm.Show()
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
	Public WithEvents cmdSetAccountProperties As System.Windows.Forms.Button
	Public WithEvents cmdNewAccount As System.Windows.Forms.Button
	Public WithEvents cmdNewRegister As System.Windows.Forms.Button
	Public WithEvents cmdCancel As System.Windows.Forms.Button
	Public WithEvents cmdShowRegister As System.Windows.Forms.Button
	Public WithEvents lstRegisters As System.Windows.Forms.ListBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdRegen = New System.Windows.Forms.Button()
        Me.cmdDeleteAccount = New System.Windows.Forms.Button()
        Me.cmdDeleteRegister = New System.Windows.Forms.Button()
        Me.cmdRegProperties = New System.Windows.Forms.Button()
        Me.cmdSetAccountProperties = New System.Windows.Forms.Button()
        Me.cmdNewAccount = New System.Windows.Forms.Button()
        Me.cmdNewRegister = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdShowRegister = New System.Windows.Forms.Button()
        Me.lstRegisters = New System.Windows.Forms.ListBox()
        Me.cmdEditGenerators = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'cmdRegen
        '
        Me.cmdRegen.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdRegen.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRegen.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRegen.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRegen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRegen.Location = New System.Drawing.Point(12, 341)
        Me.cmdRegen.Name = "cmdRegen"
        Me.cmdRegen.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRegen.Size = New System.Drawing.Size(171, 23)
        Me.cmdRegen.TabIndex = 4
        Me.cmdRegen.Text = "Regenerate Trans"
        Me.cmdRegen.UseVisualStyleBackColor = False
        '
        'cmdDeleteAccount
        '
        Me.cmdDeleteAccount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdDeleteAccount.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteAccount.Enabled = False
        Me.cmdDeleteAccount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteAccount.Location = New System.Drawing.Point(12, 312)
        Me.cmdDeleteAccount.Name = "cmdDeleteAccount"
        Me.cmdDeleteAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteAccount.Size = New System.Drawing.Size(171, 23)
        Me.cmdDeleteAccount.TabIndex = 3
        Me.cmdDeleteAccount.Text = "Delete Account"
        Me.cmdDeleteAccount.UseVisualStyleBackColor = False
        '
        'cmdDeleteRegister
        '
        Me.cmdDeleteRegister.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdDeleteRegister.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteRegister.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteRegister.Enabled = False
        Me.cmdDeleteRegister.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteRegister.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteRegister.Location = New System.Drawing.Point(189, 370)
        Me.cmdDeleteRegister.Name = "cmdDeleteRegister"
        Me.cmdDeleteRegister.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteRegister.Size = New System.Drawing.Size(171, 23)
        Me.cmdDeleteRegister.TabIndex = 9
        Me.cmdDeleteRegister.Text = "Delete Register"
        Me.cmdDeleteRegister.UseVisualStyleBackColor = False
        '
        'cmdRegProperties
        '
        Me.cmdRegProperties.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdRegProperties.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRegProperties.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRegProperties.Enabled = False
        Me.cmdRegProperties.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRegProperties.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRegProperties.Location = New System.Drawing.Point(189, 283)
        Me.cmdRegProperties.Name = "cmdRegProperties"
        Me.cmdRegProperties.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRegProperties.Size = New System.Drawing.Size(171, 23)
        Me.cmdRegProperties.TabIndex = 6
        Me.cmdRegProperties.Text = "Register Properties"
        Me.cmdRegProperties.UseVisualStyleBackColor = False
        '
        'cmdSetAccountProperties
        '
        Me.cmdSetAccountProperties.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSetAccountProperties.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSetAccountProperties.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSetAccountProperties.Enabled = False
        Me.cmdSetAccountProperties.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSetAccountProperties.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSetAccountProperties.Location = New System.Drawing.Point(12, 283)
        Me.cmdSetAccountProperties.Name = "cmdSetAccountProperties"
        Me.cmdSetAccountProperties.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSetAccountProperties.Size = New System.Drawing.Size(171, 23)
        Me.cmdSetAccountProperties.TabIndex = 2
        Me.cmdSetAccountProperties.Text = "Account Properties"
        Me.cmdSetAccountProperties.UseVisualStyleBackColor = False
        '
        'cmdNewAccount
        '
        Me.cmdNewAccount.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdNewAccount.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewAccount.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewAccount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewAccount.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewAccount.Location = New System.Drawing.Point(12, 254)
        Me.cmdNewAccount.Name = "cmdNewAccount"
        Me.cmdNewAccount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewAccount.Size = New System.Drawing.Size(171, 23)
        Me.cmdNewAccount.TabIndex = 1
        Me.cmdNewAccount.Text = "New Account"
        Me.cmdNewAccount.UseVisualStyleBackColor = False
        '
        'cmdNewRegister
        '
        Me.cmdNewRegister.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdNewRegister.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewRegister.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewRegister.Enabled = False
        Me.cmdNewRegister.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewRegister.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewRegister.Location = New System.Drawing.Point(189, 341)
        Me.cmdNewRegister.Name = "cmdNewRegister"
        Me.cmdNewRegister.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewRegister.Size = New System.Drawing.Size(171, 23)
        Me.cmdNewRegister.TabIndex = 8
        Me.cmdNewRegister.Text = "New Register"
        Me.cmdNewRegister.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(415, 370)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(97, 23)
        Me.cmdCancel.TabIndex = 10
        Me.cmdCancel.Text = "Close"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdShowRegister
        '
        Me.cmdShowRegister.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdShowRegister.BackColor = System.Drawing.SystemColors.Control
        Me.cmdShowRegister.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdShowRegister.Enabled = False
        Me.cmdShowRegister.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdShowRegister.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdShowRegister.Location = New System.Drawing.Point(189, 254)
        Me.cmdShowRegister.Name = "cmdShowRegister"
        Me.cmdShowRegister.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdShowRegister.Size = New System.Drawing.Size(171, 23)
        Me.cmdShowRegister.TabIndex = 5
        Me.cmdShowRegister.Text = "Show Register"
        Me.cmdShowRegister.UseVisualStyleBackColor = False
        '
        'lstRegisters
        '
        Me.lstRegisters.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstRegisters.BackColor = System.Drawing.SystemColors.Window
        Me.lstRegisters.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstRegisters.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstRegisters.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstRegisters.IntegralHeight = False
        Me.lstRegisters.ItemHeight = 14
        Me.lstRegisters.Location = New System.Drawing.Point(12, 5)
        Me.lstRegisters.Name = "lstRegisters"
        Me.lstRegisters.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lstRegisters.Size = New System.Drawing.Size(500, 243)
        Me.lstRegisters.TabIndex = 0
        '
        'cmdEditGenerators
        '
        Me.cmdEditGenerators.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdEditGenerators.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditGenerators.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditGenerators.Enabled = False
        Me.cmdEditGenerators.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditGenerators.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditGenerators.Location = New System.Drawing.Point(189, 312)
        Me.cmdEditGenerators.Name = "cmdEditGenerators"
        Me.cmdEditGenerators.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditGenerators.Size = New System.Drawing.Size(171, 23)
        Me.cmdEditGenerators.TabIndex = 7
        Me.cmdEditGenerators.Text = "Trans Generators"
        Me.cmdEditGenerators.UseVisualStyleBackColor = False
        '
        'ShowRegisterForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(524, 405)
        Me.Controls.Add(Me.cmdEditGenerators)
        Me.Controls.Add(Me.cmdRegen)
        Me.Controls.Add(Me.cmdDeleteAccount)
        Me.Controls.Add(Me.cmdDeleteRegister)
        Me.Controls.Add(Me.cmdRegProperties)
        Me.Controls.Add(Me.cmdSetAccountProperties)
        Me.Controls.Add(Me.cmdNewAccount)
        Me.Controls.Add(Me.cmdNewRegister)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdShowRegister)
        Me.Controls.Add(Me.lstRegisters)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(2, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShowRegisterForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        Me.Text = "Registers and Accounts"
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents cmdEditGenerators As System.Windows.Forms.Button
#End Region 
End Class