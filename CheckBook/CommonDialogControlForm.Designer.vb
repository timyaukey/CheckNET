<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class CommonDialogControlForm
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
	Public ctlDialogOpen As System.Windows.Forms.OpenFileDialog
	Public ctlDialogSave As System.Windows.Forms.SaveFileDialog
	Public ctlDialogFont As System.Windows.Forms.FontDialog
	Public ctlDialogColor As System.Windows.Forms.ColorDialog
	Public ctlDialogPrint As System.Windows.Forms.PrintDialog
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CommonDialogControlForm))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.ctlDialogOpen = New System.Windows.Forms.OpenFileDialog
		Me.ctlDialogSave = New System.Windows.Forms.SaveFileDialog
		Me.ctlDialogFont = New System.Windows.Forms.FontDialog
		Me.ctlDialogColor = New System.Windows.Forms.ColorDialog
		Me.ctlDialogPrint = New System.Windows.Forms.PrintDialog
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "Common Dialog Control Form"
		Me.ClientSize = New System.Drawing.Size(224, 75)
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
		Me.Name = "CommonDialogControlForm"
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class