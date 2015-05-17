<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class StartupForm
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
	Public WithEvents lblMessage As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(StartupForm))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.lblMessage = New System.Windows.Forms.Label
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Text = "Checkbook Loading Status..."
		Me.ClientSize = New System.Drawing.Size(331, 39)
		Me.Location = New System.Drawing.Point(3, 23)
		Me.ControlBox = False
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.Enabled = True
		Me.KeyPreview = False
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "StartupForm"
		Me.lblMessage.Size = New System.Drawing.Size(313, 19)
		Me.lblMessage.Location = New System.Drawing.Point(8, 8)
		Me.lblMessage.TabIndex = 0
		Me.lblMessage.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMessage.BackColor = System.Drawing.SystemColors.Control
		Me.lblMessage.Enabled = True
		Me.lblMessage.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMessage.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMessage.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMessage.UseMnemonic = True
		Me.lblMessage.Visible = True
		Me.lblMessage.AutoSize = False
		Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMessage.Name = "lblMessage"
		Me.Controls.Add(lblMessage)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class