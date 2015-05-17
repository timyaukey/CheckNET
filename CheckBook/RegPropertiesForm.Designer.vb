<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class RegPropertiesForm
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
	Public WithEvents chkNonBank As System.Windows.Forms.CheckBox
	Public WithEvents chkShowInitially As System.Windows.Forms.CheckBox
	Public WithEvents txtTitle As System.Windows.Forms.TextBox
	Public WithEvents lblTitle As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(RegPropertiesForm))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOkay = New System.Windows.Forms.Button
		Me.chkNonBank = New System.Windows.Forms.CheckBox
		Me.chkShowInitially = New System.Windows.Forms.CheckBox
		Me.txtTitle = New System.Windows.Forms.TextBox
		Me.lblTitle = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "Register Properties"
		Me.ClientSize = New System.Drawing.Size(307, 158)
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
		Me.Name = "RegPropertiesForm"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.CancelButton = Me.cmdCancel
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.Size = New System.Drawing.Size(79, 27)
		Me.cmdCancel.Location = New System.Drawing.Point(214, 122)
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
		Me.cmdOkay.Size = New System.Drawing.Size(79, 27)
		Me.cmdOkay.Location = New System.Drawing.Point(128, 122)
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
		Me.chkNonBank.Text = "Non-Bank Transactions Only"
		Me.chkNonBank.Size = New System.Drawing.Size(169, 21)
		Me.chkNonBank.Location = New System.Drawing.Point(10, 88)
		Me.chkNonBank.TabIndex = 3
		Me.chkNonBank.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.chkNonBank.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkNonBank.FlatStyle = System.Windows.Forms.FlatStyle.Standard
		Me.chkNonBank.BackColor = System.Drawing.SystemColors.Control
		Me.chkNonBank.CausesValidation = True
		Me.chkNonBank.Enabled = True
		Me.chkNonBank.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkNonBank.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkNonBank.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkNonBank.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkNonBank.TabStop = True
		Me.chkNonBank.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkNonBank.Visible = True
		Me.chkNonBank.Name = "chkNonBank"
		Me.chkShowInitially.Text = "Show Initially"
		Me.chkShowInitially.Size = New System.Drawing.Size(169, 21)
		Me.chkShowInitially.Location = New System.Drawing.Point(10, 64)
		Me.chkShowInitially.TabIndex = 2
		Me.chkShowInitially.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.chkShowInitially.CheckAlign = System.Drawing.ContentAlignment.MiddleLeft
		Me.chkShowInitially.FlatStyle = System.Windows.Forms.FlatStyle.Standard
		Me.chkShowInitially.BackColor = System.Drawing.SystemColors.Control
		Me.chkShowInitially.CausesValidation = True
		Me.chkShowInitially.Enabled = True
		Me.chkShowInitially.ForeColor = System.Drawing.SystemColors.ControlText
		Me.chkShowInitially.Cursor = System.Windows.Forms.Cursors.Default
		Me.chkShowInitially.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.chkShowInitially.Appearance = System.Windows.Forms.Appearance.Normal
		Me.chkShowInitially.TabStop = True
		Me.chkShowInitially.CheckState = System.Windows.Forms.CheckState.Unchecked
		Me.chkShowInitially.Visible = True
		Me.chkShowInitially.Name = "chkShowInitially"
		Me.txtTitle.AutoSize = False
		Me.txtTitle.Size = New System.Drawing.Size(285, 23)
		Me.txtTitle.Location = New System.Drawing.Point(8, 34)
		Me.txtTitle.TabIndex = 1
		Me.txtTitle.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtTitle.AcceptsReturn = True
		Me.txtTitle.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTitle.BackColor = System.Drawing.SystemColors.Window
		Me.txtTitle.CausesValidation = True
		Me.txtTitle.Enabled = True
		Me.txtTitle.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTitle.HideSelection = True
		Me.txtTitle.ReadOnly = False
		Me.txtTitle.Maxlength = 0
		Me.txtTitle.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTitle.MultiLine = False
		Me.txtTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTitle.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTitle.TabStop = True
		Me.txtTitle.Visible = True
		Me.txtTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTitle.Name = "txtTitle"
		Me.lblTitle.Text = "Title:"
		Me.lblTitle.Size = New System.Drawing.Size(107, 19)
		Me.lblTitle.Location = New System.Drawing.Point(10, 10)
		Me.lblTitle.TabIndex = 0
		Me.lblTitle.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblTitle.BackColor = System.Drawing.SystemColors.Control
		Me.lblTitle.Enabled = True
		Me.lblTitle.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblTitle.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblTitle.UseMnemonic = True
		Me.lblTitle.Visible = True
		Me.lblTitle.AutoSize = False
		Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblTitle.Name = "lblTitle"
		Me.Controls.Add(cmdCancel)
		Me.Controls.Add(cmdOkay)
		Me.Controls.Add(chkNonBank)
		Me.Controls.Add(chkShowInitially)
		Me.Controls.Add(txtTitle)
		Me.Controls.Add(lblTitle)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class