<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class MoveDstForm
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
	Public WithEvents cboRegister As System.Windows.Forms.ComboBox
	Public WithEvents txtDateOrDays As System.Windows.Forms.TextBox
	Public WithEvents lblDescription As System.Windows.Forms.Label
	Public WithEvents lblRegister As System.Windows.Forms.Label
	Public WithEvents lblDateOrDays As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.cmdOkay = New System.Windows.Forms.Button
        Me.cboRegister = New System.Windows.Forms.ComboBox
        Me.txtDateOrDays = New System.Windows.Forms.TextBox
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblRegister = New System.Windows.Forms.Label
        Me.lblDateOrDays = New System.Windows.Forms.Label
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
        Me.cmdCancel.Location = New System.Drawing.Point(245, 174)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(79, 25)
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
        Me.cmdOkay.Location = New System.Drawing.Point(159, 174)
        Me.cmdOkay.Name = "cmdOkay"
        Me.cmdOkay.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdOkay.Size = New System.Drawing.Size(79, 25)
        Me.cmdOkay.TabIndex = 4
        Me.cmdOkay.Text = "OK"
        Me.cmdOkay.UseVisualStyleBackColor = False
        '
        'cboRegister
        '
        Me.cboRegister.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRegister.BackColor = System.Drawing.SystemColors.Window
        Me.cboRegister.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRegister.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRegister.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRegister.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRegister.Location = New System.Drawing.Point(10, 142)
        Me.cboRegister.Name = "cboRegister"
        Me.cboRegister.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRegister.Size = New System.Drawing.Size(312, 22)
        Me.cboRegister.TabIndex = 3
        '
        'txtDateOrDays
        '
        Me.txtDateOrDays.AcceptsReturn = True
        Me.txtDateOrDays.BackColor = System.Drawing.SystemColors.Window
        Me.txtDateOrDays.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtDateOrDays.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDateOrDays.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtDateOrDays.Location = New System.Drawing.Point(10, 92)
        Me.txtDateOrDays.MaxLength = 0
        Me.txtDateOrDays.Name = "txtDateOrDays"
        Me.txtDateOrDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtDateOrDays.Size = New System.Drawing.Size(81, 21)
        Me.txtDateOrDays.TabIndex = 1
        '
        'lblDescription
        '
        Me.lblDescription.BackColor = System.Drawing.SystemColors.Control
        Me.lblDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDescription.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDescription.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDescription.Location = New System.Drawing.Point(12, 12)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDescription.Size = New System.Drawing.Size(243, 29)
        Me.lblDescription.TabIndex = 6
        Me.lblDescription.Text = "Will move the selected transactions to a different date and/or different register" & _
            "."
        '
        'lblRegister
        '
        Me.lblRegister.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRegister.BackColor = System.Drawing.SystemColors.Control
        Me.lblRegister.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblRegister.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRegister.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblRegister.Location = New System.Drawing.Point(12, 126)
        Me.lblRegister.Name = "lblRegister"
        Me.lblRegister.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblRegister.Size = New System.Drawing.Size(308, 15)
        Me.lblRegister.TabIndex = 2
        Me.lblRegister.Text = "Move to this register, or leave blank to keep in same register:"
        '
        'lblDateOrDays
        '
        Me.lblDateOrDays.BackColor = System.Drawing.SystemColors.Control
        Me.lblDateOrDays.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDateOrDays.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDateOrDays.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDateOrDays.Location = New System.Drawing.Point(12, 62)
        Me.lblDateOrDays.Name = "lblDateOrDays"
        Me.lblDateOrDays.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDateOrDays.Size = New System.Drawing.Size(275, 29)
        Me.lblDateOrDays.TabIndex = 0
        Me.lblDateOrDays.Text = "Move to this date, or by this number of days (use a negative number to move to an" & _
            " earlier date):"
        '
        'MoveDstForm
        '
        Me.AcceptButton = Me.cmdOkay
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(337, 212)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOkay)
        Me.Controls.Add(Me.cboRegister)
        Me.Controls.Add(Me.txtDateOrDays)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.lblRegister)
        Me.Controls.Add(Me.lblDateOrDays)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 23)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "MoveDstForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Move Selected Transactions To"
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class