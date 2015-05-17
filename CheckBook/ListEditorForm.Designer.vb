<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class ListEditorForm
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
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdChange As System.Windows.Forms.Button
	Public WithEvents cmdNew As System.Windows.Forms.Button
	Public WithEvents cmdDiscard As System.Windows.Forms.Button
	Public WithEvents cmdSave As System.Windows.Forms.Button
	Public WithEvents lstElements As System.Windows.Forms.ListBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ListEditorForm))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdDelete = New System.Windows.Forms.Button
		Me.cmdChange = New System.Windows.Forms.Button
		Me.cmdNew = New System.Windows.Forms.Button
		Me.cmdDiscard = New System.Windows.Forms.Button
		Me.cmdSave = New System.Windows.Forms.Button
		Me.lstElements = New System.Windows.Forms.ListBox
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "List Editor"
		Me.ClientSize = New System.Drawing.Size(563, 382)
		Me.Location = New System.Drawing.Point(3, 23)
		Me.ControlBox = False
		Me.Icon = CType(resources.GetObject("ListEditorForm.Icon"), System.Drawing.Icon)
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
		Me.Name = "ListEditorForm"
		Me.cmdDelete.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDelete.Text = "Delete..."
		Me.cmdDelete.Size = New System.Drawing.Size(135, 25)
		Me.cmdDelete.Location = New System.Drawing.Point(418, 64)
		Me.cmdDelete.TabIndex = 3
		Me.cmdDelete.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDelete.CausesValidation = True
		Me.cmdDelete.Enabled = True
		Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDelete.TabStop = True
		Me.cmdDelete.Name = "cmdDelete"
		Me.cmdChange.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdChange.Text = "Change..."
		Me.cmdChange.Size = New System.Drawing.Size(135, 25)
		Me.cmdChange.Location = New System.Drawing.Point(418, 36)
		Me.cmdChange.TabIndex = 2
		Me.cmdChange.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdChange.BackColor = System.Drawing.SystemColors.Control
		Me.cmdChange.CausesValidation = True
		Me.cmdChange.Enabled = True
		Me.cmdChange.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdChange.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdChange.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdChange.TabStop = True
		Me.cmdChange.Name = "cmdChange"
		Me.cmdNew.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdNew.Text = "New..."
		Me.cmdNew.Size = New System.Drawing.Size(135, 25)
		Me.cmdNew.Location = New System.Drawing.Point(418, 8)
		Me.cmdNew.TabIndex = 1
		Me.cmdNew.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdNew.BackColor = System.Drawing.SystemColors.Control
		Me.cmdNew.CausesValidation = True
		Me.cmdNew.Enabled = True
		Me.cmdNew.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdNew.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdNew.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdNew.TabStop = True
		Me.cmdNew.Name = "cmdNew"
		Me.cmdDiscard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdDiscard.Text = "Discard Changes"
		Me.cmdDiscard.Size = New System.Drawing.Size(135, 25)
		Me.cmdDiscard.Location = New System.Drawing.Point(418, 348)
		Me.cmdDiscard.TabIndex = 5
		Me.cmdDiscard.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdDiscard.BackColor = System.Drawing.SystemColors.Control
		Me.cmdDiscard.CausesValidation = True
		Me.cmdDiscard.Enabled = True
		Me.cmdDiscard.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdDiscard.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdDiscard.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdDiscard.TabStop = True
		Me.cmdDiscard.Name = "cmdDiscard"
		Me.cmdSave.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSave.Text = "Save Changes"
		Me.cmdSave.Size = New System.Drawing.Size(135, 25)
		Me.cmdSave.Location = New System.Drawing.Point(418, 320)
		Me.cmdSave.TabIndex = 4
		Me.cmdSave.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdSave.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSave.CausesValidation = True
		Me.cmdSave.Enabled = True
		Me.cmdSave.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSave.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSave.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSave.TabStop = True
		Me.cmdSave.Name = "cmdSave"
		Me.lstElements.Size = New System.Drawing.Size(399, 371)
		Me.lstElements.Location = New System.Drawing.Point(6, 6)
		Me.lstElements.TabIndex = 0
		Me.lstElements.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lstElements.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lstElements.BackColor = System.Drawing.SystemColors.Window
		Me.lstElements.CausesValidation = True
		Me.lstElements.Enabled = True
		Me.lstElements.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstElements.IntegralHeight = True
		Me.lstElements.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstElements.SelectionMode = System.Windows.Forms.SelectionMode.One
		Me.lstElements.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstElements.Sorted = False
		Me.lstElements.TabStop = True
		Me.lstElements.Visible = True
		Me.lstElements.MultiColumn = False
		Me.lstElements.Name = "lstElements"
		Me.Controls.Add(cmdDelete)
		Me.Controls.Add(cmdChange)
		Me.Controls.Add(cmdNew)
		Me.Controls.Add(cmdDiscard)
		Me.Controls.Add(cmdSave)
		Me.Controls.Add(lstElements)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class