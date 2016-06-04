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
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AdjustBudgetsToCashForm))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdAdjust = New System.Windows.Forms.Button()
        Me.lvwResults = New System.Windows.Forms.ListView()
        Me._lvwResults_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwResults_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwResults_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwResults_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.txtPrefix = New System.Windows.Forms.TextBox()
        Me.txtMinBal = New System.Windows.Forms.TextBox()
        Me.txtStartingDate = New System.Windows.Forms.TextBox()
        Me._txtPercent_4 = New System.Windows.Forms.TextBox()
        Me._cboBudget_4 = New System.Windows.Forms.ComboBox()
        Me._txtPercent_3 = New System.Windows.Forms.TextBox()
        Me._cboBudget_3 = New System.Windows.Forms.ComboBox()
        Me._txtPercent_2 = New System.Windows.Forms.TextBox()
        Me._cboBudget_2 = New System.Windows.Forms.ComboBox()
        Me._txtPercent_1 = New System.Windows.Forms.TextBox()
        Me._cboBudget_1 = New System.Windows.Forms.ComboBox()
        Me._txtPercent_5 = New System.Windows.Forms.TextBox()
        Me._cboBudget_5 = New System.Windows.Forms.ComboBox()
        Me.lblPrefix = New System.Windows.Forms.Label()
        Me.lblMinBal = New System.Windows.Forms.Label()
        Me.lblStartingDate = New System.Windows.Forms.Label()
        Me._lblBudgetNumber_4 = New System.Windows.Forms.Label()
        Me._lblBudgetNumber_3 = New System.Windows.Forms.Label()
        Me._lblBudgetNumber_2 = New System.Windows.Forms.Label()
        Me._lblBudgetNumber_1 = New System.Windows.Forms.Label()
        Me.lblPercent = New System.Windows.Forms.Label()
        Me._lblBudgetNumber_5 = New System.Windows.Forms.Label()
        Me.lblBudget = New System.Windows.Forms.Label()
        Me.lblExplanation = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'cmdAdjust
        '
        Me.cmdAdjust.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdjust.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdjust.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdjust.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdjust.Location = New System.Drawing.Point(19, 239)
        Me.cmdAdjust.Name = "cmdAdjust"
        Me.cmdAdjust.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAdjust.Size = New System.Drawing.Size(356, 25)
        Me.cmdAdjust.TabIndex = 23
        Me.cmdAdjust.Text = "Adjust Selected Budgets"
        Me.cmdAdjust.UseVisualStyleBackColor = False
        '
        'lvwResults
        '
        Me.lvwResults.BackColor = System.Drawing.SystemColors.Window
        Me.lvwResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwResults_ColumnHeader_1, Me._lvwResults_ColumnHeader_2, Me._lvwResults_ColumnHeader_3, Me._lvwResults_ColumnHeader_4})
        Me.lvwResults.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwResults.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwResults.HideSelection = False
        Me.lvwResults.Location = New System.Drawing.Point(17, 270)
        Me.lvwResults.Name = "lvwResults"
        Me.lvwResults.Size = New System.Drawing.Size(436, 222)
        Me.lvwResults.TabIndex = 24
        Me.lvwResults.UseCompatibleStateImageBehavior = False
        Me.lvwResults.View = System.Windows.Forms.View.Details
        '
        '_lvwResults_ColumnHeader_1
        '
        Me._lvwResults_ColumnHeader_1.Text = "Date"
        Me._lvwResults_ColumnHeader_1.Width = 50
        '
        '_lvwResults_ColumnHeader_2
        '
        Me._lvwResults_ColumnHeader_2.Text = "Budget"
        Me._lvwResults_ColumnHeader_2.Width = 200
        '
        '_lvwResults_ColumnHeader_3
        '
        Me._lvwResults_ColumnHeader_3.Text = "Amount"
        Me._lvwResults_ColumnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        '_lvwResults_ColumnHeader_4
        '
        Me._lvwResults_ColumnHeader_4.Text = "Used"
        Me._lvwResults_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtPrefix
        '
        Me.txtPrefix.AcceptsReturn = True
        Me.txtPrefix.BackColor = System.Drawing.SystemColors.Window
        Me.txtPrefix.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtPrefix.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPrefix.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPrefix.Location = New System.Drawing.Point(231, 208)
        Me.txtPrefix.MaxLength = 0
        Me.txtPrefix.Name = "txtPrefix"
        Me.txtPrefix.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPrefix.Size = New System.Drawing.Size(143, 20)
        Me.txtPrefix.TabIndex = 22
        '
        'txtMinBal
        '
        Me.txtMinBal.AcceptsReturn = True
        Me.txtMinBal.BackColor = System.Drawing.SystemColors.Window
        Me.txtMinBal.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMinBal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinBal.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMinBal.Location = New System.Drawing.Point(231, 186)
        Me.txtMinBal.MaxLength = 0
        Me.txtMinBal.Name = "txtMinBal"
        Me.txtMinBal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMinBal.Size = New System.Drawing.Size(54, 20)
        Me.txtMinBal.TabIndex = 20
        '
        'txtStartingDate
        '
        Me.txtStartingDate.AcceptsReturn = True
        Me.txtStartingDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtStartingDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartingDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartingDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartingDate.Location = New System.Drawing.Point(231, 165)
        Me.txtStartingDate.MaxLength = 0
        Me.txtStartingDate.Name = "txtStartingDate"
        Me.txtStartingDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartingDate.Size = New System.Drawing.Size(54, 20)
        Me.txtStartingDate.TabIndex = 18
        '
        '_txtPercent_4
        '
        Me._txtPercent_4.AcceptsReturn = True
        Me._txtPercent_4.BackColor = System.Drawing.SystemColors.Window
        Me._txtPercent_4.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtPercent_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtPercent_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtPercent_4.Location = New System.Drawing.Point(231, 110)
        Me._txtPercent_4.MaxLength = 0
        Me._txtPercent_4.Name = "_txtPercent_4"
        Me._txtPercent_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtPercent_4.Size = New System.Drawing.Size(54, 20)
        Me._txtPercent_4.TabIndex = 13
        '
        '_cboBudget_4
        '
        Me._cboBudget_4.BackColor = System.Drawing.SystemColors.Window
        Me._cboBudget_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboBudget_4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cboBudget_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboBudget_4.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboBudget_4.Location = New System.Drawing.Point(36, 110)
        Me._cboBudget_4.Name = "_cboBudget_4"
        Me._cboBudget_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboBudget_4.Size = New System.Drawing.Size(172, 22)
        Me._cboBudget_4.TabIndex = 12
        '
        '_txtPercent_3
        '
        Me._txtPercent_3.AcceptsReturn = True
        Me._txtPercent_3.BackColor = System.Drawing.SystemColors.Window
        Me._txtPercent_3.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtPercent_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtPercent_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtPercent_3.Location = New System.Drawing.Point(231, 88)
        Me._txtPercent_3.MaxLength = 0
        Me._txtPercent_3.Name = "_txtPercent_3"
        Me._txtPercent_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtPercent_3.Size = New System.Drawing.Size(54, 20)
        Me._txtPercent_3.TabIndex = 10
        '
        '_cboBudget_3
        '
        Me._cboBudget_3.BackColor = System.Drawing.SystemColors.Window
        Me._cboBudget_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboBudget_3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cboBudget_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboBudget_3.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboBudget_3.Location = New System.Drawing.Point(36, 88)
        Me._cboBudget_3.Name = "_cboBudget_3"
        Me._cboBudget_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboBudget_3.Size = New System.Drawing.Size(172, 22)
        Me._cboBudget_3.TabIndex = 9
        '
        '_txtPercent_2
        '
        Me._txtPercent_2.AcceptsReturn = True
        Me._txtPercent_2.BackColor = System.Drawing.SystemColors.Window
        Me._txtPercent_2.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtPercent_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtPercent_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtPercent_2.Location = New System.Drawing.Point(231, 66)
        Me._txtPercent_2.MaxLength = 0
        Me._txtPercent_2.Name = "_txtPercent_2"
        Me._txtPercent_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtPercent_2.Size = New System.Drawing.Size(54, 20)
        Me._txtPercent_2.TabIndex = 7
        '
        '_cboBudget_2
        '
        Me._cboBudget_2.BackColor = System.Drawing.SystemColors.Window
        Me._cboBudget_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboBudget_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cboBudget_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboBudget_2.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboBudget_2.Location = New System.Drawing.Point(36, 66)
        Me._cboBudget_2.Name = "_cboBudget_2"
        Me._cboBudget_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboBudget_2.Size = New System.Drawing.Size(172, 22)
        Me._cboBudget_2.TabIndex = 6
        '
        '_txtPercent_1
        '
        Me._txtPercent_1.AcceptsReturn = True
        Me._txtPercent_1.BackColor = System.Drawing.SystemColors.Window
        Me._txtPercent_1.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtPercent_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtPercent_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtPercent_1.Location = New System.Drawing.Point(231, 45)
        Me._txtPercent_1.MaxLength = 0
        Me._txtPercent_1.Name = "_txtPercent_1"
        Me._txtPercent_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtPercent_1.Size = New System.Drawing.Size(54, 20)
        Me._txtPercent_1.TabIndex = 4
        '
        '_cboBudget_1
        '
        Me._cboBudget_1.BackColor = System.Drawing.SystemColors.Window
        Me._cboBudget_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboBudget_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cboBudget_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboBudget_1.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboBudget_1.Location = New System.Drawing.Point(36, 45)
        Me._cboBudget_1.Name = "_cboBudget_1"
        Me._cboBudget_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboBudget_1.Size = New System.Drawing.Size(172, 22)
        Me._cboBudget_1.TabIndex = 3
        '
        '_txtPercent_5
        '
        Me._txtPercent_5.AcceptsReturn = True
        Me._txtPercent_5.BackColor = System.Drawing.SystemColors.Window
        Me._txtPercent_5.Cursor = System.Windows.Forms.Cursors.IBeam
        Me._txtPercent_5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._txtPercent_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me._txtPercent_5.Location = New System.Drawing.Point(231, 131)
        Me._txtPercent_5.MaxLength = 0
        Me._txtPercent_5.Name = "_txtPercent_5"
        Me._txtPercent_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._txtPercent_5.Size = New System.Drawing.Size(54, 20)
        Me._txtPercent_5.TabIndex = 16
        '
        '_cboBudget_5
        '
        Me._cboBudget_5.BackColor = System.Drawing.SystemColors.Window
        Me._cboBudget_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._cboBudget_5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._cboBudget_5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._cboBudget_5.ForeColor = System.Drawing.SystemColors.WindowText
        Me._cboBudget_5.Location = New System.Drawing.Point(36, 131)
        Me._cboBudget_5.Name = "_cboBudget_5"
        Me._cboBudget_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._cboBudget_5.Size = New System.Drawing.Size(172, 22)
        Me._cboBudget_5.TabIndex = 15
        '
        'lblPrefix
        '
        Me.lblPrefix.BackColor = System.Drawing.SystemColors.Control
        Me.lblPrefix.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPrefix.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrefix.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPrefix.Location = New System.Drawing.Point(91, 210)
        Me.lblPrefix.Name = "lblPrefix"
        Me.lblPrefix.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPrefix.Size = New System.Drawing.Size(131, 18)
        Me.lblPrefix.TabIndex = 21
        Me.lblPrefix.Text = "Transaction Name Prefix:"
        '
        'lblMinBal
        '
        Me.lblMinBal.BackColor = System.Drawing.SystemColors.Control
        Me.lblMinBal.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblMinBal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMinBal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMinBal.Location = New System.Drawing.Point(91, 189)
        Me.lblMinBal.Name = "lblMinBal"
        Me.lblMinBal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMinBal.Size = New System.Drawing.Size(136, 17)
        Me.lblMinBal.TabIndex = 19
        Me.lblMinBal.Text = "Minimum Balance:"
        '
        'lblStartingDate
        '
        Me.lblStartingDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartingDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartingDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartingDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartingDate.Location = New System.Drawing.Point(91, 167)
        Me.lblStartingDate.Name = "lblStartingDate"
        Me.lblStartingDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartingDate.Size = New System.Drawing.Size(136, 18)
        Me.lblStartingDate.TabIndex = 17
        Me.lblStartingDate.Text = "Earliest Date To Adjust:"
        '
        '_lblBudgetNumber_4
        '
        Me._lblBudgetNumber_4.BackColor = System.Drawing.SystemColors.Control
        Me._lblBudgetNumber_4.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBudgetNumber_4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBudgetNumber_4.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBudgetNumber_4.Location = New System.Drawing.Point(17, 114)
        Me._lblBudgetNumber_4.Name = "_lblBudgetNumber_4"
        Me._lblBudgetNumber_4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBudgetNumber_4.Size = New System.Drawing.Size(16, 16)
        Me._lblBudgetNumber_4.TabIndex = 11
        Me._lblBudgetNumber_4.Text = "4."
        '
        '_lblBudgetNumber_3
        '
        Me._lblBudgetNumber_3.BackColor = System.Drawing.SystemColors.Control
        Me._lblBudgetNumber_3.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBudgetNumber_3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBudgetNumber_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBudgetNumber_3.Location = New System.Drawing.Point(17, 93)
        Me._lblBudgetNumber_3.Name = "_lblBudgetNumber_3"
        Me._lblBudgetNumber_3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBudgetNumber_3.Size = New System.Drawing.Size(16, 16)
        Me._lblBudgetNumber_3.TabIndex = 8
        Me._lblBudgetNumber_3.Text = "3."
        '
        '_lblBudgetNumber_2
        '
        Me._lblBudgetNumber_2.BackColor = System.Drawing.SystemColors.Control
        Me._lblBudgetNumber_2.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBudgetNumber_2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBudgetNumber_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBudgetNumber_2.Location = New System.Drawing.Point(17, 71)
        Me._lblBudgetNumber_2.Name = "_lblBudgetNumber_2"
        Me._lblBudgetNumber_2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBudgetNumber_2.Size = New System.Drawing.Size(16, 16)
        Me._lblBudgetNumber_2.TabIndex = 5
        Me._lblBudgetNumber_2.Text = "2."
        '
        '_lblBudgetNumber_1
        '
        Me._lblBudgetNumber_1.BackColor = System.Drawing.SystemColors.Control
        Me._lblBudgetNumber_1.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBudgetNumber_1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBudgetNumber_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBudgetNumber_1.Location = New System.Drawing.Point(17, 50)
        Me._lblBudgetNumber_1.Name = "_lblBudgetNumber_1"
        Me._lblBudgetNumber_1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBudgetNumber_1.Size = New System.Drawing.Size(16, 16)
        Me._lblBudgetNumber_1.TabIndex = 2
        Me._lblBudgetNumber_1.Text = "1."
        '
        'lblPercent
        '
        Me.lblPercent.BackColor = System.Drawing.SystemColors.Control
        Me.lblPercent.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblPercent.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercent.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPercent.Location = New System.Drawing.Point(233, 9)
        Me.lblPercent.Name = "lblPercent"
        Me.lblPercent.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPercent.Size = New System.Drawing.Size(105, 41)
        Me.lblPercent.TabIndex = 1
        Me.lblPercent.Text = "% of Cash To Use For Each Budget"
        '
        '_lblBudgetNumber_5
        '
        Me._lblBudgetNumber_5.BackColor = System.Drawing.SystemColors.Control
        Me._lblBudgetNumber_5.Cursor = System.Windows.Forms.Cursors.Default
        Me._lblBudgetNumber_5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._lblBudgetNumber_5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._lblBudgetNumber_5.Location = New System.Drawing.Point(17, 136)
        Me._lblBudgetNumber_5.Name = "_lblBudgetNumber_5"
        Me._lblBudgetNumber_5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me._lblBudgetNumber_5.Size = New System.Drawing.Size(16, 16)
        Me._lblBudgetNumber_5.TabIndex = 14
        Me._lblBudgetNumber_5.Text = "5."
        '
        'lblBudget
        '
        Me.lblBudget.BackColor = System.Drawing.SystemColors.Control
        Me.lblBudget.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblBudget.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBudget.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblBudget.Location = New System.Drawing.Point(39, 28)
        Me.lblBudget.Name = "lblBudget"
        Me.lblBudget.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBudget.Size = New System.Drawing.Size(109, 16)
        Me.lblBudget.TabIndex = 0
        Me.lblBudget.Text = "Budgets To Adjust"
        '
        'lblExplanation
        '
        Me.lblExplanation.Location = New System.Drawing.Point(332, 50)
        Me.lblExplanation.Name = "lblExplanation"
        Me.lblExplanation.Size = New System.Drawing.Size(213, 145)
        Me.lblExplanation.TabIndex = 25
        Me.lblExplanation.Text = resources.GetString("lblExplanation.Text")
        '
        'AdjustBudgetsToCashForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(557, 510)
        Me.Controls.Add(Me.lblExplanation)
        Me.Controls.Add(Me.cmdAdjust)
        Me.Controls.Add(Me.lvwResults)
        Me.Controls.Add(Me.txtPrefix)
        Me.Controls.Add(Me.txtMinBal)
        Me.Controls.Add(Me.txtStartingDate)
        Me.Controls.Add(Me._txtPercent_4)
        Me.Controls.Add(Me._cboBudget_4)
        Me.Controls.Add(Me._txtPercent_3)
        Me.Controls.Add(Me._cboBudget_3)
        Me.Controls.Add(Me._txtPercent_2)
        Me.Controls.Add(Me._cboBudget_2)
        Me.Controls.Add(Me._txtPercent_1)
        Me.Controls.Add(Me._cboBudget_1)
        Me.Controls.Add(Me._txtPercent_5)
        Me.Controls.Add(Me._cboBudget_5)
        Me.Controls.Add(Me.lblPrefix)
        Me.Controls.Add(Me.lblMinBal)
        Me.Controls.Add(Me.lblStartingDate)
        Me.Controls.Add(Me._lblBudgetNumber_4)
        Me.Controls.Add(Me._lblBudgetNumber_3)
        Me.Controls.Add(Me._lblBudgetNumber_2)
        Me.Controls.Add(Me._lblBudgetNumber_1)
        Me.Controls.Add(Me.lblPercent)
        Me.Controls.Add(Me._lblBudgetNumber_5)
        Me.Controls.Add(Me.lblBudget)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Location = New System.Drawing.Point(2, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AdjustBudgetsToCashForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblExplanation As System.Windows.Forms.Label
#End Region 
End Class