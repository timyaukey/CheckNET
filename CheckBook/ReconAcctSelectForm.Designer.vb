<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class ReconAcctSelectForm
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
	Public WithEvents lstAccounts As System.Windows.Forms.ListBox
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ReconAcctSelectForm))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdCancel = New System.Windows.Forms.Button
		Me.cmdOkay = New System.Windows.Forms.Button
		Me.lstAccounts = New System.Windows.Forms.ListBox
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "Select Account To Reconcile"
		Me.ClientSize = New System.Drawing.Size(350, 211)
		Me.Location = New System.Drawing.Point(3, 23)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
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
		Me.Name = "ReconAcctSelectForm"
		Me.cmdCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdCancel.Text = "Cancel"
		Me.cmdCancel.Size = New System.Drawing.Size(78, 25)
		Me.cmdCancel.Location = New System.Drawing.Point(264, 180)
		Me.cmdCancel.TabIndex = 2
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
		Me.cmdOkay.Size = New System.Drawing.Size(78, 25)
		Me.cmdOkay.Location = New System.Drawing.Point(183, 180)
		Me.cmdOkay.TabIndex = 1
		Me.cmdOkay.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdOkay.BackColor = System.Drawing.SystemColors.Control
		Me.cmdOkay.CausesValidation = True
		Me.cmdOkay.Enabled = True
		Me.cmdOkay.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdOkay.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdOkay.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdOkay.TabStop = True
		Me.cmdOkay.Name = "cmdOkay"
		Me.lstAccounts.Size = New System.Drawing.Size(340, 173)
		Me.lstAccounts.Location = New System.Drawing.Point(5, 5)
		Me.lstAccounts.TabIndex = 0
		Me.lstAccounts.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lstAccounts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lstAccounts.BackColor = System.Drawing.SystemColors.Window
		Me.lstAccounts.CausesValidation = True
		Me.lstAccounts.Enabled = True
		Me.lstAccounts.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lstAccounts.IntegralHeight = True
		Me.lstAccounts.Cursor = System.Windows.Forms.Cursors.Default
		Me.lstAccounts.SelectionMode = System.Windows.Forms.SelectionMode.One
		Me.lstAccounts.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lstAccounts.Sorted = False
		Me.lstAccounts.TabStop = True
		Me.lstAccounts.Visible = True
		Me.lstAccounts.MultiColumn = False
		Me.lstAccounts.Name = "lstAccounts"
		Me.Controls.Add(cmdCancel)
		Me.Controls.Add(cmdOkay)
		Me.Controls.Add(lstAccounts)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class