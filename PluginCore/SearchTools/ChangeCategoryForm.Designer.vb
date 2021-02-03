<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class ChangeCategoryForm
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
	Public WithEvents cboNewCategory As System.Windows.Forms.ComboBox
	Public WithEvents cboOldCategory As System.Windows.Forms.ComboBox
	Public WithEvents lblNewCategory As System.Windows.Forms.Label
	Public WithEvents lblOldCategory As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ChangeCategoryForm))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOkay = New System.Windows.Forms.Button
		Me.cboNewCategory = New System.Windows.Forms.ComboBox
		Me.cboOldCategory = New System.Windows.Forms.ComboBox
		Me.lblNewCategory = New System.Windows.Forms.Label
		Me.lblOldCategory = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "Select Categories"
		Me.ClientSize = New System.Drawing.Size(467, 126)
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
		Me.Name = "ChangeCategoryForm"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.Size = New System.Drawing.Size(99, 27)
		Me.cmdCancel.Location = New System.Drawing.Point(346, 84)
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
		Me.cmdOkay.Size = New System.Drawing.Size(99, 27)
		Me.cmdOkay.Location = New System.Drawing.Point(238, 84)
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
		Me.cboNewCategory.Size = New System.Drawing.Size(329, 21)
		Me.cboNewCategory.Location = New System.Drawing.Point(116, 44)
		Me.cboNewCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboNewCategory.TabIndex = 3
		Me.cboNewCategory.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cboNewCategory.BackColor = System.Drawing.SystemColors.Window
		Me.cboNewCategory.CausesValidation = True
		Me.cboNewCategory.Enabled = True
		Me.cboNewCategory.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboNewCategory.IntegralHeight = True
		Me.cboNewCategory.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboNewCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboNewCategory.Sorted = False
		Me.cboNewCategory.TabStop = True
		Me.cboNewCategory.Visible = True
		Me.cboNewCategory.Name = "cboNewCategory"
		Me.cboOldCategory.Size = New System.Drawing.Size(329, 21)
		Me.cboOldCategory.Location = New System.Drawing.Point(116, 16)
		Me.cboOldCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cboOldCategory.TabIndex = 1
		Me.cboOldCategory.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cboOldCategory.BackColor = System.Drawing.SystemColors.Window
		Me.cboOldCategory.CausesValidation = True
		Me.cboOldCategory.Enabled = True
		Me.cboOldCategory.ForeColor = System.Drawing.SystemColors.WindowText
		Me.cboOldCategory.IntegralHeight = True
		Me.cboOldCategory.Cursor = System.Windows.Forms.Cursors.Default
		Me.cboOldCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cboOldCategory.Sorted = False
		Me.cboOldCategory.TabStop = True
		Me.cboOldCategory.Visible = True
		Me.cboOldCategory.Name = "cboOldCategory"
		Me.lblNewCategory.Text = "New Category:"
		Me.lblNewCategory.Size = New System.Drawing.Size(85, 21)
		Me.lblNewCategory.Location = New System.Drawing.Point(14, 46)
		Me.lblNewCategory.TabIndex = 2
		Me.lblNewCategory.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblNewCategory.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblNewCategory.BackColor = System.Drawing.SystemColors.Control
		Me.lblNewCategory.Enabled = True
		Me.lblNewCategory.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblNewCategory.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblNewCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblNewCategory.UseMnemonic = True
		Me.lblNewCategory.Visible = True
		Me.lblNewCategory.AutoSize = False
		Me.lblNewCategory.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblNewCategory.Name = "lblNewCategory"
		Me.lblOldCategory.Text = "Old Category:"
		Me.lblOldCategory.Size = New System.Drawing.Size(85, 21)
		Me.lblOldCategory.Location = New System.Drawing.Point(14, 18)
		Me.lblOldCategory.TabIndex = 0
		Me.lblOldCategory.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblOldCategory.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblOldCategory.BackColor = System.Drawing.SystemColors.Control
		Me.lblOldCategory.Enabled = True
		Me.lblOldCategory.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblOldCategory.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblOldCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblOldCategory.UseMnemonic = True
		Me.lblOldCategory.Visible = True
		Me.lblOldCategory.AutoSize = False
		Me.lblOldCategory.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblOldCategory.Name = "lblOldCategory"
		Me.Controls.Add(cmdCancel)
		Me.Controls.Add(cmdOkay)
		Me.Controls.Add(cboNewCategory)
		Me.Controls.Add(cboOldCategory)
		Me.Controls.Add(lblNewCategory)
		Me.Controls.Add(lblOldCategory)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class