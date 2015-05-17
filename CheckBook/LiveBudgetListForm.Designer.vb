<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class LiveBudgetListForm
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
	Public WithEvents txtTargetDate As System.Windows.Forms.TextBox
	Public WithEvents cmdSearch As System.Windows.Forms.Button
	Public WithEvents _lvwMatches_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwMatches As System.Windows.Forms.ListView
	Public WithEvents Label3 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(LiveBudgetListForm))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.txtTargetDate = New System.Windows.Forms.TextBox
		Me.cmdSearch = New System.Windows.Forms.Button
		Me.lvwMatches = New System.Windows.Forms.ListView
		Me._lvwMatches_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwMatches_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwMatches_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwMatches_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me._lvwMatches_ColumnHeader_5 = New System.Windows.Forms.ColumnHeader
		Me._lvwMatches_ColumnHeader_6 = New System.Windows.Forms.ColumnHeader
		Me.Label3 = New System.Windows.Forms.Label
		Me.lvwMatches.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Text = "Live Budget Transactions"
		Me.ClientSize = New System.Drawing.Size(656, 220)
		Me.Location = New System.Drawing.Point(2, 22)
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
		Me.Name = "LiveBudgetListForm"
		Me.txtTargetDate.AutoSize = False
		Me.txtTargetDate.Size = New System.Drawing.Size(64, 20)
		Me.txtTargetDate.Location = New System.Drawing.Point(80, 8)
		Me.txtTargetDate.TabIndex = 1
		Me.txtTargetDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtTargetDate.AcceptsReturn = True
		Me.txtTargetDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtTargetDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtTargetDate.CausesValidation = True
		Me.txtTargetDate.Enabled = True
		Me.txtTargetDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtTargetDate.HideSelection = True
		Me.txtTargetDate.ReadOnly = False
		Me.txtTargetDate.Maxlength = 0
		Me.txtTargetDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtTargetDate.MultiLine = False
		Me.txtTargetDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtTargetDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtTargetDate.TabStop = True
		Me.txtTargetDate.Visible = True
		Me.txtTargetDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtTargetDate.Name = "txtTargetDate"
		Me.cmdSearch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdSearch.Text = "Search"
		Me.cmdSearch.Size = New System.Drawing.Size(97, 25)
		Me.cmdSearch.Location = New System.Drawing.Point(550, 8)
		Me.cmdSearch.TabIndex = 2
		Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
		Me.cmdSearch.CausesValidation = True
		Me.cmdSearch.Enabled = True
		Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdSearch.TabStop = True
		Me.cmdSearch.Name = "cmdSearch"
		Me.lvwMatches.Size = New System.Drawing.Size(640, 172)
		Me.lvwMatches.Location = New System.Drawing.Point(8, 36)
		Me.lvwMatches.TabIndex = 3
		Me.lvwMatches.View = System.Windows.Forms.View.Details
		Me.lvwMatches.LabelEdit = False
		Me.lvwMatches.LabelWrap = True
		Me.lvwMatches.HideSelection = False
		Me.lvwMatches.FullRowSelect = True
		Me.lvwMatches.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwMatches.BackColor = System.Drawing.SystemColors.Window
		Me.lvwMatches.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lvwMatches.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwMatches.Name = "lvwMatches"
		Me._lvwMatches_ColumnHeader_1.Text = "Date"
		Me._lvwMatches_ColumnHeader_1.Width = 106
		Me._lvwMatches_ColumnHeader_2.Text = "Description"
		Me._lvwMatches_ColumnHeader_2.Width = 353
		Me._lvwMatches_ColumnHeader_3.Text = "Budget"
		Me._lvwMatches_ColumnHeader_3.Width = 236
		Me._lvwMatches_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwMatches_ColumnHeader_4.Text = "Limit"
		Me._lvwMatches_ColumnHeader_4.Width = 118
		Me._lvwMatches_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwMatches_ColumnHeader_5.Text = "Applied"
		Me._lvwMatches_ColumnHeader_5.Width = 118
		Me._lvwMatches_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwMatches_ColumnHeader_6.Text = "Amount"
		Me._lvwMatches_ColumnHeader_6.Width = 118
		Me.Label3.Text = "Target Date:"
		Me.Label3.Size = New System.Drawing.Size(68, 18)
		Me.Label3.Location = New System.Drawing.Point(8, 10)
		Me.Label3.TabIndex = 0
		Me.Label3.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.Label3.BackColor = System.Drawing.SystemColors.Control
		Me.Label3.Enabled = True
		Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
		Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
		Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.Label3.UseMnemonic = True
		Me.Label3.Visible = True
		Me.Label3.AutoSize = False
		Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.Label3.Name = "Label3"
		Me.Controls.Add(txtTargetDate)
		Me.Controls.Add(cmdSearch)
		Me.Controls.Add(lvwMatches)
		Me.Controls.Add(Label3)
		Me.lvwMatches.Columns.Add(_lvwMatches_ColumnHeader_1)
		Me.lvwMatches.Columns.Add(_lvwMatches_ColumnHeader_2)
		Me.lvwMatches.Columns.Add(_lvwMatches_ColumnHeader_3)
		Me.lvwMatches.Columns.Add(_lvwMatches_ColumnHeader_4)
		Me.lvwMatches.Columns.Add(_lvwMatches_ColumnHeader_5)
		Me.lvwMatches.Columns.Add(_lvwMatches_ColumnHeader_6)
		Me.lvwMatches.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class