<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class AdjustBudgetsToCashForm
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
	Public WithEvents cmdAdjust As System.Windows.Forms.Button
	Public WithEvents _lvwResults_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwResults_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwResults_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwResults_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwResults As System.Windows.Forms.ListView
	Public WithEvents txtPrefix As System.Windows.Forms.TextBox
	Public WithEvents txtMinBal As System.Windows.Forms.TextBox
	Public WithEvents txtStartingDate As System.Windows.Forms.TextBox
	Public WithEvents _txtPercent_4 As System.Windows.Forms.TextBox
	Public WithEvents _cboBudget_4 As System.Windows.Forms.ComboBox
	Public WithEvents _txtPercent_3 As System.Windows.Forms.TextBox
	Public WithEvents _cboBudget_3 As System.Windows.Forms.ComboBox
	Public WithEvents _txtPercent_2 As System.Windows.Forms.TextBox
	Public WithEvents _cboBudget_2 As System.Windows.Forms.ComboBox
	Public WithEvents _txtPercent_1 As System.Windows.Forms.TextBox
	Public WithEvents _cboBudget_1 As System.Windows.Forms.ComboBox
	Public WithEvents _txtPercent_5 As System.Windows.Forms.TextBox
	Public WithEvents _cboBudget_5 As System.Windows.Forms.ComboBox
	Public WithEvents lblPrefix As System.Windows.Forms.Label
	Public WithEvents lblMinBal As System.Windows.Forms.Label
	Public WithEvents lblStartingDate As System.Windows.Forms.Label
	Public WithEvents _lblBudgetNumber_4 As System.Windows.Forms.Label
	Public WithEvents _lblBudgetNumber_3 As System.Windows.Forms.Label
	Public WithEvents _lblBudgetNumber_2 As System.Windows.Forms.Label
	Public WithEvents _lblBudgetNumber_1 As System.Windows.Forms.Label
	Public WithEvents lblPercent As System.Windows.Forms.Label
	Public WithEvents _lblBudgetNumber_5 As System.Windows.Forms.Label
	Public WithEvents lblBudget As System.Windows.Forms.Label
	Public WithEvents cboBudget As Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray
	Public WithEvents lblBudgetNumber As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
	Public WithEvents txtPercent As Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
		Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(AdjustBudgetsToCashForm))
		Me.components = New System.ComponentModel.Container()
		Me.ToolTip1 = New System.Windows.Forms.ToolTip(components)
		Me.cmdAdjust = New System.Windows.Forms.Button
		Me.lvwResults = New System.Windows.Forms.ListView
		Me._lvwResults_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
		Me._lvwResults_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
		Me.txtPrefix = New System.Windows.Forms.TextBox
		Me.txtMinBal = New System.Windows.Forms.TextBox
		Me.txtStartingDate = New System.Windows.Forms.TextBox
		Me._txtPercent_4 = New System.Windows.Forms.TextBox
		Me._cboBudget_4 = New System.Windows.Forms.ComboBox
		Me._txtPercent_3 = New System.Windows.Forms.TextBox
		Me._cboBudget_3 = New System.Windows.Forms.ComboBox
		Me._txtPercent_2 = New System.Windows.Forms.TextBox
		Me._cboBudget_2 = New System.Windows.Forms.ComboBox
		Me._txtPercent_1 = New System.Windows.Forms.TextBox
		Me._cboBudget_1 = New System.Windows.Forms.ComboBox
		Me._txtPercent_5 = New System.Windows.Forms.TextBox
		Me._cboBudget_5 = New System.Windows.Forms.ComboBox
		Me.lblPrefix = New System.Windows.Forms.Label
		Me.lblMinBal = New System.Windows.Forms.Label
		Me.lblStartingDate = New System.Windows.Forms.Label
		Me._lblBudgetNumber_4 = New System.Windows.Forms.Label
		Me._lblBudgetNumber_3 = New System.Windows.Forms.Label
		Me._lblBudgetNumber_2 = New System.Windows.Forms.Label
		Me._lblBudgetNumber_1 = New System.Windows.Forms.Label
		Me.lblPercent = New System.Windows.Forms.Label
		Me._lblBudgetNumber_5 = New System.Windows.Forms.Label
		Me.lblBudget = New System.Windows.Forms.Label
		Me.cboBudget = New Microsoft.VisualBasic.Compatibility.VB6.ComboBoxArray(components)
		Me.lblBudgetNumber = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(components)
		Me.txtPercent = New Microsoft.VisualBasic.Compatibility.VB6.TextBoxArray(components)
		Me.lvwResults.SuspendLayout()
		Me.SuspendLayout()
		Me.ToolTip1.Active = True
		CType(Me.cboBudget, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.lblBudgetNumber, System.ComponentModel.ISupportInitialize).BeginInit()
		CType(Me.txtPercent, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
		Me.Text = "Form1"
		Me.ClientSize = New System.Drawing.Size(457, 491)
		Me.Location = New System.Drawing.Point(2, 22)
		Me.MaximizeBox = False
		Me.MinimizeBox = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.BackColor = System.Drawing.SystemColors.Control
		Me.ControlBox = True
		Me.Enabled = True
		Me.KeyPreview = False
		Me.Cursor = System.Windows.Forms.Cursors.Default
		Me.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.ShowInTaskbar = True
		Me.HelpButton = False
		Me.WindowState = System.Windows.Forms.FormWindowState.Normal
		Me.Name = "AdjustBudgetsToCashForm"
		Me.cmdAdjust.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		Me.cmdAdjust.Text = "Adjust Selected Budgets"
		Me.cmdAdjust.Size = New System.Drawing.Size(356, 25)
		Me.cmdAdjust.Location = New System.Drawing.Point(12, 221)
		Me.cmdAdjust.TabIndex = 23
		Me.cmdAdjust.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.cmdAdjust.BackColor = System.Drawing.SystemColors.Control
		Me.cmdAdjust.CausesValidation = True
		Me.cmdAdjust.Enabled = True
		Me.cmdAdjust.ForeColor = System.Drawing.SystemColors.ControlText
		Me.cmdAdjust.Cursor = System.Windows.Forms.Cursors.Default
		Me.cmdAdjust.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.cmdAdjust.TabStop = True
		Me.cmdAdjust.Name = "cmdAdjust"
		Me.lvwResults.Size = New System.Drawing.Size(436, 222)
		Me.lvwResults.Location = New System.Drawing.Point(10, 252)
		Me.lvwResults.TabIndex = 24
		Me.lvwResults.View = System.Windows.Forms.View.Details
		Me.lvwResults.LabelEdit = False
		Me.lvwResults.LabelWrap = True
		Me.lvwResults.HideSelection = False
		Me.lvwResults.ForeColor = System.Drawing.SystemColors.WindowText
		Me.lvwResults.BackColor = System.Drawing.SystemColors.Window
		Me.lvwResults.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lvwResults.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.lvwResults.Name = "lvwResults"
		Me._lvwResults_ColumnHeader_1.Text = "Date"
		Me._lvwResults_ColumnHeader_1.Width = 106
		Me._lvwResults_ColumnHeader_2.Text = "Budget"
		Me._lvwResults_ColumnHeader_2.Width = 353
		Me._lvwResults_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwResults_ColumnHeader_3.Text = "Amount"
		Me._lvwResults_ColumnHeader_3.Width = 118
		Me._lvwResults_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
		Me._lvwResults_ColumnHeader_4.Text = "Used"
		Me._lvwResults_ColumnHeader_4.Width = 118
		Me.txtPrefix.AutoSize = False
		Me.txtPrefix.Size = New System.Drawing.Size(143, 20)
		Me.txtPrefix.Location = New System.Drawing.Point(224, 190)
		Me.txtPrefix.TabIndex = 22
		Me.txtPrefix.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtPrefix.AcceptsReturn = True
		Me.txtPrefix.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtPrefix.BackColor = System.Drawing.SystemColors.Window
		Me.txtPrefix.CausesValidation = True
		Me.txtPrefix.Enabled = True
		Me.txtPrefix.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtPrefix.HideSelection = True
		Me.txtPrefix.ReadOnly = False
		Me.txtPrefix.Maxlength = 0
		Me.txtPrefix.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtPrefix.MultiLine = False
		Me.txtPrefix.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtPrefix.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtPrefix.TabStop = True
		Me.txtPrefix.Visible = True
		Me.txtPrefix.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtPrefix.Name = "txtPrefix"
		Me.txtMinBal.AutoSize = False
		Me.txtMinBal.Size = New System.Drawing.Size(54, 20)
		Me.txtMinBal.Location = New System.Drawing.Point(224, 168)
		Me.txtMinBal.TabIndex = 20
		Me.txtMinBal.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtMinBal.AcceptsReturn = True
		Me.txtMinBal.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtMinBal.BackColor = System.Drawing.SystemColors.Window
		Me.txtMinBal.CausesValidation = True
		Me.txtMinBal.Enabled = True
		Me.txtMinBal.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtMinBal.HideSelection = True
		Me.txtMinBal.ReadOnly = False
		Me.txtMinBal.Maxlength = 0
		Me.txtMinBal.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtMinBal.MultiLine = False
		Me.txtMinBal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtMinBal.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtMinBal.TabStop = True
		Me.txtMinBal.Visible = True
		Me.txtMinBal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtMinBal.Name = "txtMinBal"
		Me.txtStartingDate.AutoSize = False
		Me.txtStartingDate.Size = New System.Drawing.Size(54, 20)
		Me.txtStartingDate.Location = New System.Drawing.Point(224, 147)
		Me.txtStartingDate.TabIndex = 18
		Me.txtStartingDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.txtStartingDate.AcceptsReturn = True
		Me.txtStartingDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me.txtStartingDate.BackColor = System.Drawing.SystemColors.Window
		Me.txtStartingDate.CausesValidation = True
		Me.txtStartingDate.Enabled = True
		Me.txtStartingDate.ForeColor = System.Drawing.SystemColors.WindowText
		Me.txtStartingDate.HideSelection = True
		Me.txtStartingDate.ReadOnly = False
		Me.txtStartingDate.Maxlength = 0
		Me.txtStartingDate.Cursor = System.Windows.Forms.Cursors.IBeam
		Me.txtStartingDate.MultiLine = False
		Me.txtStartingDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.txtStartingDate.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me.txtStartingDate.TabStop = True
		Me.txtStartingDate.Visible = True
		Me.txtStartingDate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me.txtStartingDate.Name = "txtStartingDate"
		Me._txtPercent_4.AutoSize = False
		Me._txtPercent_4.Size = New System.Drawing.Size(54, 20)
		Me._txtPercent_4.Location = New System.Drawing.Point(224, 92)
		Me._txtPercent_4.TabIndex = 13
		Me._txtPercent_4.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._txtPercent_4.AcceptsReturn = True
		Me._txtPercent_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtPercent_4.BackColor = System.Drawing.SystemColors.Window
		Me._txtPercent_4.CausesValidation = True
		Me._txtPercent_4.Enabled = True
		Me._txtPercent_4.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtPercent_4.HideSelection = True
		Me._txtPercent_4.ReadOnly = False
		Me._txtPercent_4.Maxlength = 0
		Me._txtPercent_4.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtPercent_4.MultiLine = False
		Me._txtPercent_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtPercent_4.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtPercent_4.TabStop = True
		Me._txtPercent_4.Visible = True
		Me._txtPercent_4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtPercent_4.Name = "_txtPercent_4"
		Me._cboBudget_4.Size = New System.Drawing.Size(172, 20)
		Me._cboBudget_4.Location = New System.Drawing.Point(29, 92)
		Me._cboBudget_4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me._cboBudget_4.TabIndex = 12
		Me._cboBudget_4.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._cboBudget_4.BackColor = System.Drawing.SystemColors.Window
		Me._cboBudget_4.CausesValidation = True
		Me._cboBudget_4.Enabled = True
		Me._cboBudget_4.ForeColor = System.Drawing.SystemColors.WindowText
		Me._cboBudget_4.IntegralHeight = True
		Me._cboBudget_4.Cursor = System.Windows.Forms.Cursors.Default
		Me._cboBudget_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cboBudget_4.Sorted = False
		Me._cboBudget_4.TabStop = True
		Me._cboBudget_4.Visible = True
		Me._cboBudget_4.Name = "_cboBudget_4"
		Me._txtPercent_3.AutoSize = False
		Me._txtPercent_3.Size = New System.Drawing.Size(54, 20)
		Me._txtPercent_3.Location = New System.Drawing.Point(224, 70)
		Me._txtPercent_3.TabIndex = 10
		Me._txtPercent_3.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._txtPercent_3.AcceptsReturn = True
		Me._txtPercent_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtPercent_3.BackColor = System.Drawing.SystemColors.Window
		Me._txtPercent_3.CausesValidation = True
		Me._txtPercent_3.Enabled = True
		Me._txtPercent_3.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtPercent_3.HideSelection = True
		Me._txtPercent_3.ReadOnly = False
		Me._txtPercent_3.Maxlength = 0
		Me._txtPercent_3.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtPercent_3.MultiLine = False
		Me._txtPercent_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtPercent_3.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtPercent_3.TabStop = True
		Me._txtPercent_3.Visible = True
		Me._txtPercent_3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtPercent_3.Name = "_txtPercent_3"
		Me._cboBudget_3.Size = New System.Drawing.Size(172, 20)
		Me._cboBudget_3.Location = New System.Drawing.Point(29, 70)
		Me._cboBudget_3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me._cboBudget_3.TabIndex = 9
		Me._cboBudget_3.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._cboBudget_3.BackColor = System.Drawing.SystemColors.Window
		Me._cboBudget_3.CausesValidation = True
		Me._cboBudget_3.Enabled = True
		Me._cboBudget_3.ForeColor = System.Drawing.SystemColors.WindowText
		Me._cboBudget_3.IntegralHeight = True
		Me._cboBudget_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._cboBudget_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cboBudget_3.Sorted = False
		Me._cboBudget_3.TabStop = True
		Me._cboBudget_3.Visible = True
		Me._cboBudget_3.Name = "_cboBudget_3"
		Me._txtPercent_2.AutoSize = False
		Me._txtPercent_2.Size = New System.Drawing.Size(54, 20)
		Me._txtPercent_2.Location = New System.Drawing.Point(224, 48)
		Me._txtPercent_2.TabIndex = 7
		Me._txtPercent_2.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._txtPercent_2.AcceptsReturn = True
		Me._txtPercent_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtPercent_2.BackColor = System.Drawing.SystemColors.Window
		Me._txtPercent_2.CausesValidation = True
		Me._txtPercent_2.Enabled = True
		Me._txtPercent_2.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtPercent_2.HideSelection = True
		Me._txtPercent_2.ReadOnly = False
		Me._txtPercent_2.Maxlength = 0
		Me._txtPercent_2.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtPercent_2.MultiLine = False
		Me._txtPercent_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtPercent_2.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtPercent_2.TabStop = True
		Me._txtPercent_2.Visible = True
		Me._txtPercent_2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtPercent_2.Name = "_txtPercent_2"
		Me._cboBudget_2.Size = New System.Drawing.Size(172, 20)
		Me._cboBudget_2.Location = New System.Drawing.Point(29, 48)
		Me._cboBudget_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me._cboBudget_2.TabIndex = 6
		Me._cboBudget_2.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._cboBudget_2.BackColor = System.Drawing.SystemColors.Window
		Me._cboBudget_2.CausesValidation = True
		Me._cboBudget_2.Enabled = True
		Me._cboBudget_2.ForeColor = System.Drawing.SystemColors.WindowText
		Me._cboBudget_2.IntegralHeight = True
		Me._cboBudget_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._cboBudget_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cboBudget_2.Sorted = False
		Me._cboBudget_2.TabStop = True
		Me._cboBudget_2.Visible = True
		Me._cboBudget_2.Name = "_cboBudget_2"
		Me._txtPercent_1.AutoSize = False
		Me._txtPercent_1.Size = New System.Drawing.Size(54, 20)
		Me._txtPercent_1.Location = New System.Drawing.Point(224, 27)
		Me._txtPercent_1.TabIndex = 4
		Me._txtPercent_1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._txtPercent_1.AcceptsReturn = True
		Me._txtPercent_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtPercent_1.BackColor = System.Drawing.SystemColors.Window
		Me._txtPercent_1.CausesValidation = True
		Me._txtPercent_1.Enabled = True
		Me._txtPercent_1.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtPercent_1.HideSelection = True
		Me._txtPercent_1.ReadOnly = False
		Me._txtPercent_1.Maxlength = 0
		Me._txtPercent_1.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtPercent_1.MultiLine = False
		Me._txtPercent_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtPercent_1.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtPercent_1.TabStop = True
		Me._txtPercent_1.Visible = True
		Me._txtPercent_1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtPercent_1.Name = "_txtPercent_1"
		Me._cboBudget_1.Size = New System.Drawing.Size(172, 20)
		Me._cboBudget_1.Location = New System.Drawing.Point(29, 27)
		Me._cboBudget_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me._cboBudget_1.TabIndex = 3
		Me._cboBudget_1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._cboBudget_1.BackColor = System.Drawing.SystemColors.Window
		Me._cboBudget_1.CausesValidation = True
		Me._cboBudget_1.Enabled = True
		Me._cboBudget_1.ForeColor = System.Drawing.SystemColors.WindowText
		Me._cboBudget_1.IntegralHeight = True
		Me._cboBudget_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._cboBudget_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cboBudget_1.Sorted = False
		Me._cboBudget_1.TabStop = True
		Me._cboBudget_1.Visible = True
		Me._cboBudget_1.Name = "_cboBudget_1"
		Me._txtPercent_5.AutoSize = False
		Me._txtPercent_5.Size = New System.Drawing.Size(54, 20)
		Me._txtPercent_5.Location = New System.Drawing.Point(224, 113)
		Me._txtPercent_5.TabIndex = 16
		Me._txtPercent_5.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._txtPercent_5.AcceptsReturn = True
		Me._txtPercent_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
		Me._txtPercent_5.BackColor = System.Drawing.SystemColors.Window
		Me._txtPercent_5.CausesValidation = True
		Me._txtPercent_5.Enabled = True
		Me._txtPercent_5.ForeColor = System.Drawing.SystemColors.WindowText
		Me._txtPercent_5.HideSelection = True
		Me._txtPercent_5.ReadOnly = False
		Me._txtPercent_5.Maxlength = 0
		Me._txtPercent_5.Cursor = System.Windows.Forms.Cursors.IBeam
		Me._txtPercent_5.MultiLine = False
		Me._txtPercent_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._txtPercent_5.ScrollBars = System.Windows.Forms.ScrollBars.None
		Me._txtPercent_5.TabStop = True
		Me._txtPercent_5.Visible = True
		Me._txtPercent_5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
		Me._txtPercent_5.Name = "_txtPercent_5"
		Me._cboBudget_5.Size = New System.Drawing.Size(172, 20)
		Me._cboBudget_5.Location = New System.Drawing.Point(29, 113)
		Me._cboBudget_5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me._cboBudget_5.TabIndex = 15
		Me._cboBudget_5.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._cboBudget_5.BackColor = System.Drawing.SystemColors.Window
		Me._cboBudget_5.CausesValidation = True
		Me._cboBudget_5.Enabled = True
		Me._cboBudget_5.ForeColor = System.Drawing.SystemColors.WindowText
		Me._cboBudget_5.IntegralHeight = True
		Me._cboBudget_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._cboBudget_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._cboBudget_5.Sorted = False
		Me._cboBudget_5.TabStop = True
		Me._cboBudget_5.Visible = True
		Me._cboBudget_5.Name = "_cboBudget_5"
		Me.lblPrefix.Text = "Transaction Name Prefix:"
		Me.lblPrefix.Size = New System.Drawing.Size(128, 16)
		Me.lblPrefix.Location = New System.Drawing.Point(92, 192)
		Me.lblPrefix.TabIndex = 21
		Me.lblPrefix.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblPrefix.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPrefix.BackColor = System.Drawing.SystemColors.Control
		Me.lblPrefix.Enabled = True
		Me.lblPrefix.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPrefix.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPrefix.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPrefix.UseMnemonic = True
		Me.lblPrefix.Visible = True
		Me.lblPrefix.AutoSize = False
		Me.lblPrefix.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPrefix.Name = "lblPrefix"
		Me.lblMinBal.Text = "Minimum Balance:"
		Me.lblMinBal.Size = New System.Drawing.Size(128, 16)
		Me.lblMinBal.Location = New System.Drawing.Point(92, 171)
		Me.lblMinBal.TabIndex = 19
		Me.lblMinBal.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblMinBal.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblMinBal.BackColor = System.Drawing.SystemColors.Control
		Me.lblMinBal.Enabled = True
		Me.lblMinBal.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblMinBal.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblMinBal.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblMinBal.UseMnemonic = True
		Me.lblMinBal.Visible = True
		Me.lblMinBal.AutoSize = False
		Me.lblMinBal.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblMinBal.Name = "lblMinBal"
		Me.lblStartingDate.Text = "Earliest Budget To Adjust:"
		Me.lblStartingDate.Size = New System.Drawing.Size(128, 16)
		Me.lblStartingDate.Location = New System.Drawing.Point(92, 149)
		Me.lblStartingDate.TabIndex = 17
		Me.lblStartingDate.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblStartingDate.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblStartingDate.BackColor = System.Drawing.SystemColors.Control
		Me.lblStartingDate.Enabled = True
		Me.lblStartingDate.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblStartingDate.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblStartingDate.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblStartingDate.UseMnemonic = True
		Me.lblStartingDate.Visible = True
		Me.lblStartingDate.AutoSize = False
		Me.lblStartingDate.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblStartingDate.Name = "lblStartingDate"
		Me._lblBudgetNumber_4.Text = "4."
		Me._lblBudgetNumber_4.Size = New System.Drawing.Size(16, 16)
		Me._lblBudgetNumber_4.Location = New System.Drawing.Point(10, 96)
		Me._lblBudgetNumber_4.TabIndex = 11
		Me._lblBudgetNumber_4.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._lblBudgetNumber_4.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblBudgetNumber_4.BackColor = System.Drawing.SystemColors.Control
		Me._lblBudgetNumber_4.Enabled = True
		Me._lblBudgetNumber_4.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblBudgetNumber_4.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblBudgetNumber_4.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblBudgetNumber_4.UseMnemonic = True
		Me._lblBudgetNumber_4.Visible = True
		Me._lblBudgetNumber_4.AutoSize = False
		Me._lblBudgetNumber_4.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblBudgetNumber_4.Name = "_lblBudgetNumber_4"
		Me._lblBudgetNumber_3.Text = "3."
		Me._lblBudgetNumber_3.Size = New System.Drawing.Size(16, 16)
		Me._lblBudgetNumber_3.Location = New System.Drawing.Point(10, 75)
		Me._lblBudgetNumber_3.TabIndex = 8
		Me._lblBudgetNumber_3.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._lblBudgetNumber_3.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblBudgetNumber_3.BackColor = System.Drawing.SystemColors.Control
		Me._lblBudgetNumber_3.Enabled = True
		Me._lblBudgetNumber_3.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblBudgetNumber_3.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblBudgetNumber_3.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblBudgetNumber_3.UseMnemonic = True
		Me._lblBudgetNumber_3.Visible = True
		Me._lblBudgetNumber_3.AutoSize = False
		Me._lblBudgetNumber_3.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblBudgetNumber_3.Name = "_lblBudgetNumber_3"
		Me._lblBudgetNumber_2.Text = "2."
		Me._lblBudgetNumber_2.Size = New System.Drawing.Size(16, 16)
		Me._lblBudgetNumber_2.Location = New System.Drawing.Point(10, 53)
		Me._lblBudgetNumber_2.TabIndex = 5
		Me._lblBudgetNumber_2.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._lblBudgetNumber_2.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblBudgetNumber_2.BackColor = System.Drawing.SystemColors.Control
		Me._lblBudgetNumber_2.Enabled = True
		Me._lblBudgetNumber_2.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblBudgetNumber_2.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblBudgetNumber_2.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblBudgetNumber_2.UseMnemonic = True
		Me._lblBudgetNumber_2.Visible = True
		Me._lblBudgetNumber_2.AutoSize = False
		Me._lblBudgetNumber_2.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblBudgetNumber_2.Name = "_lblBudgetNumber_2"
		Me._lblBudgetNumber_1.Text = "1."
		Me._lblBudgetNumber_1.Size = New System.Drawing.Size(16, 16)
		Me._lblBudgetNumber_1.Location = New System.Drawing.Point(10, 32)
		Me._lblBudgetNumber_1.TabIndex = 2
		Me._lblBudgetNumber_1.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._lblBudgetNumber_1.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblBudgetNumber_1.BackColor = System.Drawing.SystemColors.Control
		Me._lblBudgetNumber_1.Enabled = True
		Me._lblBudgetNumber_1.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblBudgetNumber_1.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblBudgetNumber_1.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblBudgetNumber_1.UseMnemonic = True
		Me._lblBudgetNumber_1.Visible = True
		Me._lblBudgetNumber_1.AutoSize = False
		Me._lblBudgetNumber_1.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblBudgetNumber_1.Name = "_lblBudgetNumber_1"
		Me.lblPercent.Text = "% of Available Cash"
		Me.lblPercent.Size = New System.Drawing.Size(102, 16)
		Me.lblPercent.Location = New System.Drawing.Point(226, 10)
		Me.lblPercent.TabIndex = 1
		Me.lblPercent.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblPercent.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblPercent.BackColor = System.Drawing.SystemColors.Control
		Me.lblPercent.Enabled = True
		Me.lblPercent.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblPercent.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblPercent.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblPercent.UseMnemonic = True
		Me.lblPercent.Visible = True
		Me.lblPercent.AutoSize = False
		Me.lblPercent.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblPercent.Name = "lblPercent"
		Me._lblBudgetNumber_5.Text = "5."
		Me._lblBudgetNumber_5.Size = New System.Drawing.Size(16, 16)
		Me._lblBudgetNumber_5.Location = New System.Drawing.Point(10, 118)
		Me._lblBudgetNumber_5.TabIndex = 14
		Me._lblBudgetNumber_5.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me._lblBudgetNumber_5.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me._lblBudgetNumber_5.BackColor = System.Drawing.SystemColors.Control
		Me._lblBudgetNumber_5.Enabled = True
		Me._lblBudgetNumber_5.ForeColor = System.Drawing.SystemColors.ControlText
		Me._lblBudgetNumber_5.Cursor = System.Windows.Forms.Cursors.Default
		Me._lblBudgetNumber_5.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me._lblBudgetNumber_5.UseMnemonic = True
		Me._lblBudgetNumber_5.Visible = True
		Me._lblBudgetNumber_5.AutoSize = False
		Me._lblBudgetNumber_5.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me._lblBudgetNumber_5.Name = "_lblBudgetNumber_5"
		Me.lblBudget.Text = "Budget To Adjust"
		Me.lblBudget.Size = New System.Drawing.Size(109, 16)
		Me.lblBudget.Location = New System.Drawing.Point(32, 10)
		Me.lblBudget.TabIndex = 0
		Me.lblBudget.Font = New System.Drawing.Font("Arial", 8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.lblBudget.TextAlign = System.Drawing.ContentAlignment.TopLeft
		Me.lblBudget.BackColor = System.Drawing.SystemColors.Control
		Me.lblBudget.Enabled = True
		Me.lblBudget.ForeColor = System.Drawing.SystemColors.ControlText
		Me.lblBudget.Cursor = System.Windows.Forms.Cursors.Default
		Me.lblBudget.RightToLeft = System.Windows.Forms.RightToLeft.No
		Me.lblBudget.UseMnemonic = True
		Me.lblBudget.Visible = True
		Me.lblBudget.AutoSize = False
		Me.lblBudget.BorderStyle = System.Windows.Forms.BorderStyle.None
		Me.lblBudget.Name = "lblBudget"
		Me.Controls.Add(cmdAdjust)
		Me.Controls.Add(lvwResults)
		Me.Controls.Add(txtPrefix)
		Me.Controls.Add(txtMinBal)
		Me.Controls.Add(txtStartingDate)
		Me.Controls.Add(_txtPercent_4)
		Me.Controls.Add(_cboBudget_4)
		Me.Controls.Add(_txtPercent_3)
		Me.Controls.Add(_cboBudget_3)
		Me.Controls.Add(_txtPercent_2)
		Me.Controls.Add(_cboBudget_2)
		Me.Controls.Add(_txtPercent_1)
		Me.Controls.Add(_cboBudget_1)
		Me.Controls.Add(_txtPercent_5)
		Me.Controls.Add(_cboBudget_5)
		Me.Controls.Add(lblPrefix)
		Me.Controls.Add(lblMinBal)
		Me.Controls.Add(lblStartingDate)
		Me.Controls.Add(_lblBudgetNumber_4)
		Me.Controls.Add(_lblBudgetNumber_3)
		Me.Controls.Add(_lblBudgetNumber_2)
		Me.Controls.Add(_lblBudgetNumber_1)
		Me.Controls.Add(lblPercent)
		Me.Controls.Add(_lblBudgetNumber_5)
		Me.Controls.Add(lblBudget)
		Me.lvwResults.Columns.Add(_lvwResults_ColumnHeader_1)
		Me.lvwResults.Columns.Add(_lvwResults_ColumnHeader_2)
		Me.lvwResults.Columns.Add(_lvwResults_ColumnHeader_3)
		Me.lvwResults.Columns.Add(_lvwResults_ColumnHeader_4)
		Me.cboBudget.SetIndex(_cboBudget_4, CType(4, Short))
		Me.cboBudget.SetIndex(_cboBudget_3, CType(3, Short))
		Me.cboBudget.SetIndex(_cboBudget_2, CType(2, Short))
		Me.cboBudget.SetIndex(_cboBudget_1, CType(1, Short))
		Me.cboBudget.SetIndex(_cboBudget_5, CType(5, Short))
		Me.lblBudgetNumber.SetIndex(_lblBudgetNumber_4, CType(4, Short))
		Me.lblBudgetNumber.SetIndex(_lblBudgetNumber_3, CType(3, Short))
		Me.lblBudgetNumber.SetIndex(_lblBudgetNumber_2, CType(2, Short))
		Me.lblBudgetNumber.SetIndex(_lblBudgetNumber_1, CType(1, Short))
		Me.lblBudgetNumber.SetIndex(_lblBudgetNumber_5, CType(5, Short))
		Me.txtPercent.SetIndex(_txtPercent_4, CType(4, Short))
		Me.txtPercent.SetIndex(_txtPercent_3, CType(3, Short))
		Me.txtPercent.SetIndex(_txtPercent_2, CType(2, Short))
		Me.txtPercent.SetIndex(_txtPercent_1, CType(1, Short))
		Me.txtPercent.SetIndex(_txtPercent_5, CType(5, Short))
		CType(Me.txtPercent, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.lblBudgetNumber, System.ComponentModel.ISupportInitialize).EndInit()
		CType(Me.cboBudget, System.ComponentModel.ISupportInitialize).EndInit()
		Me.lvwResults.ResumeLayout(False)
		Me.ResumeLayout(False)
		Me.PerformLayout()
	End Sub
#End Region 
End Class