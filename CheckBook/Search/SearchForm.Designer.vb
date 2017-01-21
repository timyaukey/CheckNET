<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class SearchForm
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
		'This form is an MDI child.
		'This code simulates the VB6 
		' functionality of automatically
		' loading and showing an MDI
		' child's parent.
		Me.MDIParent = CheckBook.CBMainForm
		CheckBook.CBMainForm.Show
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
	Public WithEvents cmdNewNormal As System.Windows.Forms.Button
	Public WithEvents cmdEditTrx As System.Windows.Forms.Button
	Public WithEvents cmdRecategorize As System.Windows.Forms.Button
	Public WithEvents chkIncludeGenerated As System.Windows.Forms.CheckBox
	Public WithEvents cmdSelect As System.Windows.Forms.Button
	Public WithEvents cmdExport As System.Windows.Forms.Button
	Public WithEvents txtAddMultAmount As System.Windows.Forms.TextBox
	Public WithEvents cmdMultTotalBy As System.Windows.Forms.Button
	Public WithEvents cmdAddToTotal As System.Windows.Forms.Button
	Public WithEvents cmdTotalToClipboard As System.Windows.Forms.Button
	Public WithEvents cmdClearTotal As System.Windows.Forms.Button
	Public WithEvents cmdMove As System.Windows.Forms.Button
	Public WithEvents cmdCombine As System.Windows.Forms.Button
	Public WithEvents txtEndDate As System.Windows.Forms.TextBox
	Public WithEvents cmdSearch As System.Windows.Forms.Button
	Public WithEvents txtSearchFor As System.Windows.Forms.TextBox
	Public WithEvents txtStartDate As System.Windows.Forms.TextBox
	Public WithEvents cboSearchType As System.Windows.Forms.ComboBox
	Public WithEvents cboSearchIn As System.Windows.Forms.ComboBox
	Public WithEvents _lvwMatches_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_11 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_12 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_ColumnHeader_13 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwMatches As System.Windows.Forms.ListView
	Public WithEvents cboSearchCats As System.Windows.Forms.ComboBox
	Public WithEvents lblAddMultAmout As System.Windows.Forms.Label
	Public WithEvents lblTotalDollars As System.Windows.Forms.Label
	Public WithEvents lblEndDate As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents lblStartDate As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdNewNormal = New System.Windows.Forms.Button()
        Me.cmdEditTrx = New System.Windows.Forms.Button()
        Me.cmdRecategorize = New System.Windows.Forms.Button()
        Me.chkIncludeGenerated = New System.Windows.Forms.CheckBox()
        Me.cmdSelect = New System.Windows.Forms.Button()
        Me.cmdExport = New System.Windows.Forms.Button()
        Me.txtAddMultAmount = New System.Windows.Forms.TextBox()
        Me.cmdMultTotalBy = New System.Windows.Forms.Button()
        Me.cmdAddToTotal = New System.Windows.Forms.Button()
        Me.cmdTotalToClipboard = New System.Windows.Forms.Button()
        Me.cmdClearTotal = New System.Windows.Forms.Button()
        Me.cmdMove = New System.Windows.Forms.Button()
        Me.cmdCombine = New System.Windows.Forms.Button()
        Me.txtEndDate = New System.Windows.Forms.TextBox()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.txtSearchFor = New System.Windows.Forms.TextBox()
        Me.txtStartDate = New System.Windows.Forms.TextBox()
        Me.cboSearchType = New System.Windows.Forms.ComboBox()
        Me.cboSearchIn = New System.Windows.Forms.ComboBox()
        Me.lvwMatches = New System.Windows.Forms.ListView()
        Me._lvwMatches_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_11 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cboSearchCats = New System.Windows.Forms.ComboBox()
        Me.lblAddMultAmout = New System.Windows.Forms.Label()
        Me.lblTotalDollars = New System.Windows.Forms.Label()
        Me.lblEndDate = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblStartDate = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.chkShowAllSplits = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'cmdNewNormal
        '
        Me.cmdNewNormal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdNewNormal.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewNormal.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewNormal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewNormal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewNormal.Location = New System.Drawing.Point(166, 475)
        Me.cmdNewNormal.Name = "cmdNewNormal"
        Me.cmdNewNormal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewNormal.Size = New System.Drawing.Size(108, 25)
        Me.cmdNewNormal.TabIndex = 23
        Me.cmdNewNormal.Text = "New Transaction"
        Me.cmdNewNormal.UseVisualStyleBackColor = False
        '
        'cmdEditTrx
        '
        Me.cmdEditTrx.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdEditTrx.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditTrx.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEditTrx.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEditTrx.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditTrx.Location = New System.Drawing.Point(280, 475)
        Me.cmdEditTrx.Name = "cmdEditTrx"
        Me.cmdEditTrx.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEditTrx.Size = New System.Drawing.Size(107, 25)
        Me.cmdEditTrx.TabIndex = 24
        Me.cmdEditTrx.Text = "Edit"
        Me.cmdEditTrx.UseVisualStyleBackColor = False
        '
        'cmdRecategorize
        '
        Me.cmdRecategorize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdRecategorize.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRecategorize.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRecategorize.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRecategorize.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRecategorize.Location = New System.Drawing.Point(587, 475)
        Me.cmdRecategorize.Name = "cmdRecategorize"
        Me.cmdRecategorize.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRecategorize.Size = New System.Drawing.Size(95, 25)
        Me.cmdRecategorize.TabIndex = 25
        Me.cmdRecategorize.Text = "Recategorize"
        Me.cmdRecategorize.UseVisualStyleBackColor = False
        '
        'chkIncludeGenerated
        '
        Me.chkIncludeGenerated.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeGenerated.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeGenerated.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncludeGenerated.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeGenerated.Location = New System.Drawing.Point(87, 79)
        Me.chkIncludeGenerated.Name = "chkIncludeGenerated"
        Me.chkIncludeGenerated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeGenerated.Size = New System.Drawing.Size(181, 22)
        Me.chkIncludeGenerated.TabIndex = 7
        Me.chkIncludeGenerated.Text = "Include generated transactions"
        Me.chkIncludeGenerated.UseVisualStyleBackColor = False
        '
        'cmdSelect
        '
        Me.cmdSelect.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSelect.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSelect.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSelect.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSelect.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSelect.Location = New System.Drawing.Point(12, 475)
        Me.cmdSelect.Name = "cmdSelect"
        Me.cmdSelect.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSelect.Size = New System.Drawing.Size(119, 25)
        Me.cmdSelect.TabIndex = 22
        Me.cmdSelect.Text = "(Un)Select All"
        Me.cmdSelect.UseVisualStyleBackColor = False
        '
        'cmdExport
        '
        Me.cmdExport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExport.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExport.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExport.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExport.Location = New System.Drawing.Point(687, 475)
        Me.cmdExport.Name = "cmdExport"
        Me.cmdExport.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdExport.Size = New System.Drawing.Size(95, 25)
        Me.cmdExport.TabIndex = 26
        Me.cmdExport.Text = "Export"
        Me.cmdExport.UseVisualStyleBackColor = False
        '
        'txtAddMultAmount
        '
        Me.txtAddMultAmount.AcceptsReturn = True
        Me.txtAddMultAmount.BackColor = System.Drawing.SystemColors.Window
        Me.txtAddMultAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAddMultAmount.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAddMultAmount.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAddMultAmount.Location = New System.Drawing.Point(440, 77)
        Me.txtAddMultAmount.MaxLength = 0
        Me.txtAddMultAmount.Name = "txtAddMultAmount"
        Me.txtAddMultAmount.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAddMultAmount.Size = New System.Drawing.Size(73, 20)
        Me.txtAddMultAmount.TabIndex = 14
        '
        'cmdMultTotalBy
        '
        Me.cmdMultTotalBy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMultTotalBy.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMultTotalBy.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMultTotalBy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMultTotalBy.Location = New System.Drawing.Point(440, 52)
        Me.cmdMultTotalBy.Name = "cmdMultTotalBy"
        Me.cmdMultTotalBy.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMultTotalBy.Size = New System.Drawing.Size(115, 23)
        Me.cmdMultTotalBy.TabIndex = 12
        Me.cmdMultTotalBy.Text = "Multiply Total By"
        Me.cmdMultTotalBy.UseVisualStyleBackColor = False
        '
        'cmdAddToTotal
        '
        Me.cmdAddToTotal.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddToTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAddToTotal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddToTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddToTotal.Location = New System.Drawing.Point(440, 26)
        Me.cmdAddToTotal.Name = "cmdAddToTotal"
        Me.cmdAddToTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdAddToTotal.Size = New System.Drawing.Size(115, 23)
        Me.cmdAddToTotal.TabIndex = 10
        Me.cmdAddToTotal.Text = "Add To Total"
        Me.cmdAddToTotal.UseVisualStyleBackColor = False
        '
        'cmdTotalToClipboard
        '
        Me.cmdTotalToClipboard.BackColor = System.Drawing.SystemColors.Control
        Me.cmdTotalToClipboard.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdTotalToClipboard.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdTotalToClipboard.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdTotalToClipboard.Location = New System.Drawing.Point(322, 52)
        Me.cmdTotalToClipboard.Name = "cmdTotalToClipboard"
        Me.cmdTotalToClipboard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdTotalToClipboard.Size = New System.Drawing.Size(115, 23)
        Me.cmdTotalToClipboard.TabIndex = 11
        Me.cmdTotalToClipboard.Text = "Total To Clipboard"
        Me.cmdTotalToClipboard.UseVisualStyleBackColor = False
        '
        'cmdClearTotal
        '
        Me.cmdClearTotal.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClearTotal.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClearTotal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClearTotal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClearTotal.Location = New System.Drawing.Point(322, 26)
        Me.cmdClearTotal.Name = "cmdClearTotal"
        Me.cmdClearTotal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClearTotal.Size = New System.Drawing.Size(115, 23)
        Me.cmdClearTotal.TabIndex = 9
        Me.cmdClearTotal.Text = "Clear Total"
        Me.cmdClearTotal.UseVisualStyleBackColor = False
        '
        'cmdMove
        '
        Me.cmdMove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdMove.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMove.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMove.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMove.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMove.Location = New System.Drawing.Point(787, 475)
        Me.cmdMove.Name = "cmdMove"
        Me.cmdMove.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMove.Size = New System.Drawing.Size(95, 25)
        Me.cmdMove.TabIndex = 27
        Me.cmdMove.Text = "Move"
        Me.cmdMove.UseVisualStyleBackColor = False
        '
        'cmdCombine
        '
        Me.cmdCombine.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCombine.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCombine.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCombine.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCombine.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCombine.Location = New System.Drawing.Point(887, 475)
        Me.cmdCombine.Name = "cmdCombine"
        Me.cmdCombine.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCombine.Size = New System.Drawing.Size(95, 25)
        Me.cmdCombine.TabIndex = 28
        Me.cmdCombine.Text = "Combine"
        Me.cmdCombine.UseVisualStyleBackColor = False
        '
        'txtEndDate
        '
        Me.txtEndDate.AcceptsReturn = True
        Me.txtEndDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEndDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtEndDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEndDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEndDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEndDate.Location = New System.Drawing.Point(907, 59)
        Me.txtEndDate.MaxLength = 0
        Me.txtEndDate.Name = "txtEndDate"
        Me.txtEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEndDate.Size = New System.Drawing.Size(73, 20)
        Me.txtEndDate.TabIndex = 20
        '
        'cmdSearch
        '
        Me.cmdSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Location = New System.Drawing.Point(886, 10)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(97, 23)
        Me.cmdSearch.TabIndex = 16
        Me.cmdSearch.Text = "Search"
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'txtSearchFor
        '
        Me.txtSearchFor.AcceptsReturn = True
        Me.txtSearchFor.BackColor = System.Drawing.SystemColors.Window
        Me.txtSearchFor.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSearchFor.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearchFor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSearchFor.Location = New System.Drawing.Point(87, 58)
        Me.txtSearchFor.MaxLength = 0
        Me.txtSearchFor.Name = "txtSearchFor"
        Me.txtSearchFor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSearchFor.Size = New System.Drawing.Size(220, 20)
        Me.txtSearchFor.TabIndex = 5
        '
        'txtStartDate
        '
        Me.txtStartDate.AcceptsReturn = True
        Me.txtStartDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtStartDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtStartDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartDate.Location = New System.Drawing.Point(907, 37)
        Me.txtStartDate.MaxLength = 0
        Me.txtStartDate.Name = "txtStartDate"
        Me.txtStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartDate.Size = New System.Drawing.Size(73, 20)
        Me.txtStartDate.TabIndex = 18
        '
        'cboSearchType
        '
        Me.cboSearchType.BackColor = System.Drawing.SystemColors.Window
        Me.cboSearchType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSearchType.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSearchType.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSearchType.Location = New System.Drawing.Point(87, 34)
        Me.cboSearchType.Name = "cboSearchType"
        Me.cboSearchType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSearchType.Size = New System.Drawing.Size(160, 22)
        Me.cboSearchType.TabIndex = 3
        '
        'cboSearchIn
        '
        Me.cboSearchIn.BackColor = System.Drawing.SystemColors.Window
        Me.cboSearchIn.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSearchIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSearchIn.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSearchIn.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSearchIn.Location = New System.Drawing.Point(87, 10)
        Me.cboSearchIn.Name = "cboSearchIn"
        Me.cboSearchIn.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSearchIn.Size = New System.Drawing.Size(160, 22)
        Me.cboSearchIn.TabIndex = 1
        '
        'lvwMatches
        '
        Me.lvwMatches.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwMatches.BackColor = System.Drawing.SystemColors.Window
        Me.lvwMatches.CheckBoxes = True
        Me.lvwMatches.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwMatches_ColumnHeader_1, Me._lvwMatches_ColumnHeader_2, Me._lvwMatches_ColumnHeader_3, Me._lvwMatches_ColumnHeader_4, Me._lvwMatches_ColumnHeader_5, Me._lvwMatches_ColumnHeader_6, Me._lvwMatches_ColumnHeader_7, Me._lvwMatches_ColumnHeader_8, Me._lvwMatches_ColumnHeader_9, Me._lvwMatches_ColumnHeader_10, Me._lvwMatches_ColumnHeader_11, Me._lvwMatches_ColumnHeader_12, Me._lvwMatches_ColumnHeader_13})
        Me.lvwMatches.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwMatches.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwMatches.FullRowSelect = True
        Me.lvwMatches.GridLines = True
        Me.lvwMatches.HideSelection = False
        Me.lvwMatches.Location = New System.Drawing.Point(10, 102)
        Me.lvwMatches.Name = "lvwMatches"
        Me.lvwMatches.Size = New System.Drawing.Size(973, 366)
        Me.lvwMatches.TabIndex = 21
        Me.lvwMatches.UseCompatibleStateImageBehavior = False
        Me.lvwMatches.View = System.Windows.Forms.View.Details
        '
        '_lvwMatches_ColumnHeader_1
        '
        Me._lvwMatches_ColumnHeader_1.Text = "Date"
        Me._lvwMatches_ColumnHeader_1.Width = 75
        '
        '_lvwMatches_ColumnHeader_2
        '
        Me._lvwMatches_ColumnHeader_2.Text = "Number"
        Me._lvwMatches_ColumnHeader_2.Width = 64
        '
        '_lvwMatches_ColumnHeader_3
        '
        Me._lvwMatches_ColumnHeader_3.Text = "Description"
        Me._lvwMatches_ColumnHeader_3.Width = 171
        '
        '_lvwMatches_ColumnHeader_4
        '
        Me._lvwMatches_ColumnHeader_4.Text = "Amount"
        Me._lvwMatches_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwMatches_ColumnHeader_4.Width = 66
        '
        '_lvwMatches_ColumnHeader_5
        '
        Me._lvwMatches_ColumnHeader_5.Text = "Avail"
        Me._lvwMatches_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwMatches_ColumnHeader_5.Width = 57
        '
        '_lvwMatches_ColumnHeader_6
        '
        Me._lvwMatches_ColumnHeader_6.Text = "Category"
        Me._lvwMatches_ColumnHeader_6.Width = 99
        '
        '_lvwMatches_ColumnHeader_7
        '
        Me._lvwMatches_ColumnHeader_7.Text = "PO#"
        Me._lvwMatches_ColumnHeader_7.Width = 57
        '
        '_lvwMatches_ColumnHeader_8
        '
        Me._lvwMatches_ColumnHeader_8.Text = "Invoice#"
        Me._lvwMatches_ColumnHeader_8.Width = 84
        '
        '_lvwMatches_ColumnHeader_9
        '
        Me._lvwMatches_ColumnHeader_9.Text = "Inv. Date"
        Me._lvwMatches_ColumnHeader_9.Width = 70
        '
        '_lvwMatches_ColumnHeader_10
        '
        Me._lvwMatches_ColumnHeader_10.Text = "Due Date"
        Me._lvwMatches_ColumnHeader_10.Width = 70
        '
        '_lvwMatches_ColumnHeader_11
        '
        Me._lvwMatches_ColumnHeader_11.Text = "Terms"
        Me._lvwMatches_ColumnHeader_11.Width = 59
        '
        '_lvwMatches_ColumnHeader_12
        '
        Me._lvwMatches_ColumnHeader_12.Text = "Status"
        Me._lvwMatches_ColumnHeader_12.Width = 64
        '
        '_lvwMatches_ColumnHeader_13
        '
        Me._lvwMatches_ColumnHeader_13.Text = "Hidden Index"
        Me._lvwMatches_ColumnHeader_13.Width = 0
        '
        'cboSearchCats
        '
        Me.cboSearchCats.BackColor = System.Drawing.SystemColors.Window
        Me.cboSearchCats.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboSearchCats.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSearchCats.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSearchCats.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboSearchCats.Location = New System.Drawing.Point(98, 46)
        Me.cboSearchCats.Name = "cboSearchCats"
        Me.cboSearchCats.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboSearchCats.Size = New System.Drawing.Size(220, 22)
        Me.cboSearchCats.TabIndex = 6
        '
        'lblAddMultAmout
        '
        Me.lblAddMultAmout.BackColor = System.Drawing.SystemColors.Control
        Me.lblAddMultAmout.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblAddMultAmout.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAddMultAmout.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblAddMultAmout.Location = New System.Drawing.Point(344, 79)
        Me.lblAddMultAmout.Name = "lblAddMultAmout"
        Me.lblAddMultAmout.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblAddMultAmout.Size = New System.Drawing.Size(90, 18)
        Me.lblAddMultAmout.TabIndex = 13
        Me.lblAddMultAmout.Text = "Add/Multiply By:"
        Me.lblAddMultAmout.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblTotalDollars
        '
        Me.lblTotalDollars.BackColor = System.Drawing.SystemColors.Control
        Me.lblTotalDollars.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblTotalDollars.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalDollars.ForeColor = System.Drawing.Color.Red
        Me.lblTotalDollars.Location = New System.Drawing.Point(322, 8)
        Me.lblTotalDollars.Name = "lblTotalDollars"
        Me.lblTotalDollars.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTotalDollars.Size = New System.Drawing.Size(232, 14)
        Me.lblTotalDollars.TabIndex = 8
        '
        'lblEndDate
        '
        Me.lblEndDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEndDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblEndDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblEndDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEndDate.Location = New System.Drawing.Point(833, 61)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblEndDate.Size = New System.Drawing.Size(68, 18)
        Me.lblEndDate.TabIndex = 19
        Me.lblEndDate.Text = "End Date:"
        Me.lblEndDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(10, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(70, 18)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Search For:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblStartDate
        '
        Me.lblStartDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStartDate.BackColor = System.Drawing.SystemColors.Control
        Me.lblStartDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblStartDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStartDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStartDate.Location = New System.Drawing.Point(833, 39)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblStartDate.Size = New System.Drawing.Size(68, 18)
        Me.lblStartDate.TabIndex = 17
        Me.lblStartDate.Text = "Start Date:"
        Me.lblStartDate.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(8, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(72, 18)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Search Type:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(12, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(68, 18)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Search In:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'chkShowAllSplits
        '
        Me.chkShowAllSplits.BackColor = System.Drawing.SystemColors.Control
        Me.chkShowAllSplits.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkShowAllSplits.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkShowAllSplits.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkShowAllSplits.Location = New System.Drawing.Point(587, 79)
        Me.chkShowAllSplits.Name = "chkShowAllSplits"
        Me.chkShowAllSplits.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkShowAllSplits.Size = New System.Drawing.Size(238, 22)
        Me.chkShowAllSplits.TabIndex = 15
        Me.chkShowAllSplits.Text = "Show all splits for matched transactions"
        Me.chkShowAllSplits.UseVisualStyleBackColor = False
        '
        'SearchForm
        '
        Me.AcceptButton = Me.cmdSearch
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(995, 508)
        Me.Controls.Add(Me.chkShowAllSplits)
        Me.Controls.Add(Me.cmdNewNormal)
        Me.Controls.Add(Me.cmdEditTrx)
        Me.Controls.Add(Me.cmdRecategorize)
        Me.Controls.Add(Me.chkIncludeGenerated)
        Me.Controls.Add(Me.cmdSelect)
        Me.Controls.Add(Me.cmdExport)
        Me.Controls.Add(Me.txtAddMultAmount)
        Me.Controls.Add(Me.cmdMultTotalBy)
        Me.Controls.Add(Me.cmdAddToTotal)
        Me.Controls.Add(Me.cmdTotalToClipboard)
        Me.Controls.Add(Me.cmdClearTotal)
        Me.Controls.Add(Me.cmdMove)
        Me.Controls.Add(Me.cmdCombine)
        Me.Controls.Add(Me.txtEndDate)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.txtSearchFor)
        Me.Controls.Add(Me.txtStartDate)
        Me.Controls.Add(Me.cboSearchType)
        Me.Controls.Add(Me.cboSearchIn)
        Me.Controls.Add(Me.lvwMatches)
        Me.Controls.Add(Me.cboSearchCats)
        Me.Controls.Add(Me.lblAddMultAmout)
        Me.Controls.Add(Me.lblTotalDollars)
        Me.Controls.Add(Me.lblEndDate)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblStartDate)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(7, 27)
        Me.Name = "SearchForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        Me.Text = "Search"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public WithEvents chkShowAllSplits As CheckBox
#End Region
End Class