<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class BankImportForm
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
	Public WithEvents chkAllowManualBatchNew As System.Windows.Forms.CheckBox
	Public WithEvents cboDefaultCategory As System.Windows.Forms.ComboBox
	Public WithEvents cmdBatchNew As System.Windows.Forms.Button
	Public WithEvents cmdFindUpdates As System.Windows.Forms.Button
	Public WithEvents cmdBatchUpdates As System.Windows.Forms.Button
	Public WithEvents cmdFindNew As System.Windows.Forms.Button
	Public WithEvents chkLooseMatch As System.Windows.Forms.CheckBox
	Public WithEvents cmdRefreshItems As System.Windows.Forms.Button
	Public WithEvents chkHideCompleted As System.Windows.Forms.CheckBox
	Public WithEvents cmdClose As System.Windows.Forms.Button
	Public WithEvents cmdUpdateExisting As System.Windows.Forms.Button
	Public WithEvents _lvwMatches_CH_Date As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_CH_Number As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_CH_Descr As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_CH_Amount As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_CH_Category As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_CH_Register As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwMatches_CH_Hidden As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwMatches As System.Windows.Forms.ListView
	Public WithEvents cmdRepeatSearch As System.Windows.Forms.Button
	Public WithEvents cmdCreateNew As System.Windows.Forms.Button
	Public WithEvents cboRegister As System.Windows.Forms.ComboBox
	Public WithEvents _lvwTrx_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwTrx As System.Windows.Forms.ListView
	Public WithEvents lblDefaultCategory As System.Windows.Forms.Label
	Public WithEvents lblSearchFor As System.Windows.Forms.Label
	Public WithEvents lblDoubleClickHint As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	Public WithEvents lblReadFrom As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.chkAllowManualBatchNew = New System.Windows.Forms.CheckBox()
        Me.cboDefaultCategory = New System.Windows.Forms.ComboBox()
        Me.cmdBatchNew = New System.Windows.Forms.Button()
        Me.cmdFindUpdates = New System.Windows.Forms.Button()
        Me.cmdBatchUpdates = New System.Windows.Forms.Button()
        Me.cmdFindNew = New System.Windows.Forms.Button()
        Me.chkLooseMatch = New System.Windows.Forms.CheckBox()
        Me.cmdRefreshItems = New System.Windows.Forms.Button()
        Me.chkHideCompleted = New System.Windows.Forms.CheckBox()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.cmdUpdateExisting = New System.Windows.Forms.Button()
        Me.lvwMatches = New System.Windows.Forms.ListView()
        Me._lvwMatches_CH_Date = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_CH_Number = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_CH_Descr = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_CH_Amount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_CH_Category = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_CH_DueDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_CH_Fake = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_CH_Gen = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_CH_Imported = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_CH_Register = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_CH_Hidden = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmdRepeatSearch = New System.Windows.Forms.Button()
        Me.cmdCreateNew = New System.Windows.Forms.Button()
        Me.cboRegister = New System.Windows.Forms.ComboBox()
        Me.lvwTrx = New System.Windows.Forms.ListView()
        Me._lvwTrx_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblDefaultCategory = New System.Windows.Forms.Label()
        Me.lblSearchFor = New System.Windows.Forms.Label()
        Me.lblDoubleClickHint = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblReadFrom = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'chkAllowManualBatchNew
        '
        Me.chkAllowManualBatchNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkAllowManualBatchNew.BackColor = System.Drawing.SystemColors.Control
        Me.chkAllowManualBatchNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkAllowManualBatchNew.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkAllowManualBatchNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAllowManualBatchNew.Location = New System.Drawing.Point(724, 478)
        Me.chkAllowManualBatchNew.Name = "chkAllowManualBatchNew"
        Me.chkAllowManualBatchNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkAllowManualBatchNew.Size = New System.Drawing.Size(281, 16)
        Me.chkAllowManualBatchNew.TabIndex = 21
        Me.chkAllowManualBatchNew.Text = "Allow manual additions to batch new selections"
        Me.chkAllowManualBatchNew.UseVisualStyleBackColor = False
        '
        'cboDefaultCategory
        '
        Me.cboDefaultCategory.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboDefaultCategory.BackColor = System.Drawing.SystemColors.Window
        Me.cboDefaultCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboDefaultCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboDefaultCategory.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboDefaultCategory.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboDefaultCategory.Location = New System.Drawing.Point(830, 496)
        Me.cboDefaultCategory.Name = "cboDefaultCategory"
        Me.cboDefaultCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboDefaultCategory.Size = New System.Drawing.Size(176, 22)
        Me.cboDefaultCategory.TabIndex = 19
        '
        'cmdBatchNew
        '
        Me.cmdBatchNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdBatchNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBatchNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBatchNew.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBatchNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBatchNew.Location = New System.Drawing.Point(248, 493)
        Me.cmdBatchNew.Name = "cmdBatchNew"
        Me.cmdBatchNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBatchNew.Size = New System.Drawing.Size(120, 23)
        Me.cmdBatchNew.TabIndex = 12
        Me.cmdBatchNew.Text = "Do Batch New"
        Me.cmdBatchNew.UseVisualStyleBackColor = False
        '
        'cmdFindUpdates
        '
        Me.cmdFindUpdates.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdFindUpdates.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindUpdates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindUpdates.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindUpdates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindUpdates.Location = New System.Drawing.Point(126, 518)
        Me.cmdFindUpdates.Name = "cmdFindUpdates"
        Me.cmdFindUpdates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindUpdates.Size = New System.Drawing.Size(120, 23)
        Me.cmdFindUpdates.TabIndex = 14
        Me.cmdFindUpdates.Text = "Find Batch Updates"
        Me.cmdFindUpdates.UseVisualStyleBackColor = False
        '
        'cmdBatchUpdates
        '
        Me.cmdBatchUpdates.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdBatchUpdates.BackColor = System.Drawing.SystemColors.Control
        Me.cmdBatchUpdates.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdBatchUpdates.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdBatchUpdates.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdBatchUpdates.Location = New System.Drawing.Point(248, 518)
        Me.cmdBatchUpdates.Name = "cmdBatchUpdates"
        Me.cmdBatchUpdates.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdBatchUpdates.Size = New System.Drawing.Size(120, 23)
        Me.cmdBatchUpdates.TabIndex = 15
        Me.cmdBatchUpdates.Text = "Do Batch Updates"
        Me.cmdBatchUpdates.UseVisualStyleBackColor = False
        '
        'cmdFindNew
        '
        Me.cmdFindNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdFindNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFindNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFindNew.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFindNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFindNew.Location = New System.Drawing.Point(126, 493)
        Me.cmdFindNew.Name = "cmdFindNew"
        Me.cmdFindNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFindNew.Size = New System.Drawing.Size(120, 23)
        Me.cmdFindNew.TabIndex = 11
        Me.cmdFindNew.Text = "Find Batch New"
        Me.cmdFindNew.UseVisualStyleBackColor = False
        '
        'chkLooseMatch
        '
        Me.chkLooseMatch.BackColor = System.Drawing.SystemColors.Control
        Me.chkLooseMatch.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkLooseMatch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkLooseMatch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkLooseMatch.Location = New System.Drawing.Point(82, 25)
        Me.chkLooseMatch.Name = "chkLooseMatch"
        Me.chkLooseMatch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkLooseMatch.Size = New System.Drawing.Size(93, 16)
        Me.chkLooseMatch.TabIndex = 2
        Me.chkLooseMatch.Text = "Loose Match"
        Me.chkLooseMatch.UseVisualStyleBackColor = False
        '
        'cmdRefreshItems
        '
        Me.cmdRefreshItems.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRefreshItems.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRefreshItems.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRefreshItems.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRefreshItems.Location = New System.Drawing.Point(8, 22)
        Me.cmdRefreshItems.Name = "cmdRefreshItems"
        Me.cmdRefreshItems.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRefreshItems.Size = New System.Drawing.Size(64, 18)
        Me.cmdRefreshItems.TabIndex = 1
        Me.cmdRefreshItems.Text = "Refresh"
        Me.cmdRefreshItems.UseVisualStyleBackColor = False
        '
        'chkHideCompleted
        '
        Me.chkHideCompleted.BackColor = System.Drawing.SystemColors.Control
        Me.chkHideCompleted.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkHideCompleted.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHideCompleted.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkHideCompleted.Location = New System.Drawing.Point(184, 25)
        Me.chkHideCompleted.Name = "chkHideCompleted"
        Me.chkHideCompleted.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkHideCompleted.Size = New System.Drawing.Size(137, 16)
        Me.chkHideCompleted.TabIndex = 3
        Me.chkHideCompleted.Text = "Hide Completed Items"
        Me.chkHideCompleted.UseVisualStyleBackColor = False
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.BackColor = System.Drawing.SystemColors.Control
        Me.cmdClose.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdClose.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdClose.Location = New System.Drawing.Point(1014, 518)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdClose.Size = New System.Drawing.Size(79, 23)
        Me.cmdClose.TabIndex = 18
        Me.cmdClose.Text = "Close"
        Me.cmdClose.UseVisualStyleBackColor = False
        '
        'cmdUpdateExisting
        '
        Me.cmdUpdateExisting.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdUpdateExisting.BackColor = System.Drawing.SystemColors.Control
        Me.cmdUpdateExisting.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdUpdateExisting.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUpdateExisting.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdUpdateExisting.Location = New System.Drawing.Point(8, 518)
        Me.cmdUpdateExisting.Name = "cmdUpdateExisting"
        Me.cmdUpdateExisting.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdUpdateExisting.Size = New System.Drawing.Size(104, 23)
        Me.cmdUpdateExisting.TabIndex = 13
        Me.cmdUpdateExisting.Text = "Single Update"
        Me.cmdUpdateExisting.UseVisualStyleBackColor = False
        '
        'lvwMatches
        '
        Me.lvwMatches.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwMatches.BackColor = System.Drawing.SystemColors.Window
        Me.lvwMatches.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwMatches_CH_Date, Me._lvwMatches_CH_Number, Me._lvwMatches_CH_Descr, Me._lvwMatches_CH_Amount, Me._lvwMatches_CH_Category, Me._lvwMatches_CH_DueDate, Me._lvwMatches_CH_Fake, Me._lvwMatches_CH_Gen, Me._lvwMatches_CH_Imported, Me._lvwMatches_CH_Register, Me._lvwMatches_CH_Hidden})
        Me.lvwMatches.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwMatches.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwMatches.FullRowSelect = True
        Me.lvwMatches.GridLines = True
        Me.lvwMatches.HideSelection = False
        Me.lvwMatches.Location = New System.Drawing.Point(5, 318)
        Me.lvwMatches.Name = "lvwMatches"
        Me.lvwMatches.Size = New System.Drawing.Size(1088, 154)
        Me.lvwMatches.TabIndex = 9
        Me.lvwMatches.UseCompatibleStateImageBehavior = False
        Me.lvwMatches.View = System.Windows.Forms.View.Details
        '
        '_lvwMatches_CH_Date
        '
        Me._lvwMatches_CH_Date.Text = "Date"
        Me._lvwMatches_CH_Date.Width = 72
        '
        '_lvwMatches_CH_Number
        '
        Me._lvwMatches_CH_Number.Text = "Number"
        Me._lvwMatches_CH_Number.Width = 65
        '
        '_lvwMatches_CH_Descr
        '
        Me._lvwMatches_CH_Descr.Text = "Description"
        Me._lvwMatches_CH_Descr.Width = 243
        '
        '_lvwMatches_CH_Amount
        '
        Me._lvwMatches_CH_Amount.Text = "Amount"
        Me._lvwMatches_CH_Amount.Width = 63
        '
        '_lvwMatches_CH_Category
        '
        Me._lvwMatches_CH_Category.Text = "Category"
        Me._lvwMatches_CH_Category.Width = 193
        '
        '_lvwMatches_CH_DueDate
        '
        Me._lvwMatches_CH_DueDate.Text = "Due Date"
        Me._lvwMatches_CH_DueDate.Width = 70
        '
        '_lvwMatches_CH_Fake
        '
        Me._lvwMatches_CH_Fake.Text = "Fake"
        Me._lvwMatches_CH_Fake.Width = 40
        '
        '_lvwMatches_CH_Gen
        '
        Me._lvwMatches_CH_Gen.Text = "Gen"
        Me._lvwMatches_CH_Gen.Width = 40
        '
        '_lvwMatches_CH_Imported
        '
        Me._lvwMatches_CH_Imported.Text = "Imported"
        Me._lvwMatches_CH_Imported.Width = 70
        '
        '_lvwMatches_CH_Register
        '
        Me._lvwMatches_CH_Register.Text = "Register"
        Me._lvwMatches_CH_Register.Width = 198
        '
        '_lvwMatches_CH_Hidden
        '
        Me._lvwMatches_CH_Hidden.Text = "Hidden Index"
        Me._lvwMatches_CH_Hidden.Width = 0
        '
        'cmdRepeatSearch
        '
        Me.cmdRepeatSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdRepeatSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdRepeatSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdRepeatSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdRepeatSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdRepeatSearch.Location = New System.Drawing.Point(988, 294)
        Me.cmdRepeatSearch.Name = "cmdRepeatSearch"
        Me.cmdRepeatSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdRepeatSearch.Size = New System.Drawing.Size(104, 23)
        Me.cmdRepeatSearch.TabIndex = 8
        Me.cmdRepeatSearch.Text = "Repeat Search"
        Me.cmdRepeatSearch.UseVisualStyleBackColor = False
        '
        'cmdCreateNew
        '
        Me.cmdCreateNew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdCreateNew.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCreateNew.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCreateNew.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCreateNew.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCreateNew.Location = New System.Drawing.Point(8, 493)
        Me.cmdCreateNew.Name = "cmdCreateNew"
        Me.cmdCreateNew.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCreateNew.Size = New System.Drawing.Size(104, 23)
        Me.cmdCreateNew.TabIndex = 10
        Me.cmdCreateNew.Text = "Single New"
        Me.cmdCreateNew.UseVisualStyleBackColor = False
        '
        'cboRegister
        '
        Me.cboRegister.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRegister.BackColor = System.Drawing.SystemColors.Window
        Me.cboRegister.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboRegister.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboRegister.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboRegister.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboRegister.Location = New System.Drawing.Point(830, 520)
        Me.cboRegister.Name = "cboRegister"
        Me.cboRegister.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboRegister.Size = New System.Drawing.Size(176, 22)
        Me.cboRegister.TabIndex = 17
        '
        'lvwTrx
        '
        Me.lvwTrx.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwTrx.BackColor = System.Drawing.SystemColors.Window
        Me.lvwTrx.CheckBoxes = True
        Me.lvwTrx.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwTrx_ColumnHeader_1, Me._lvwTrx_ColumnHeader_2, Me._lvwTrx_ColumnHeader_3, Me._lvwTrx_ColumnHeader_4, Me._lvwTrx_ColumnHeader_5, Me._lvwTrx_ColumnHeader_6, Me._lvwTrx_ColumnHeader_7, Me._lvwTrx_ColumnHeader_8})
        Me.lvwTrx.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTrx.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTrx.FullRowSelect = True
        Me.lvwTrx.GridLines = True
        Me.lvwTrx.HideSelection = False
        Me.lvwTrx.Location = New System.Drawing.Point(5, 42)
        Me.lvwTrx.Name = "lvwTrx"
        Me.lvwTrx.ShowItemToolTips = True
        Me.lvwTrx.Size = New System.Drawing.Size(1088, 250)
        Me.lvwTrx.TabIndex = 5
        Me.lvwTrx.UseCompatibleStateImageBehavior = False
        Me.lvwTrx.View = System.Windows.Forms.View.Details
        '
        '_lvwTrx_ColumnHeader_1
        '
        Me._lvwTrx_ColumnHeader_1.Text = "Date"
        Me._lvwTrx_ColumnHeader_1.Width = 72
        '
        '_lvwTrx_ColumnHeader_2
        '
        Me._lvwTrx_ColumnHeader_2.Text = "Number"
        Me._lvwTrx_ColumnHeader_2.Width = 65
        '
        '_lvwTrx_ColumnHeader_3
        '
        Me._lvwTrx_ColumnHeader_3.Text = "Description"
        Me._lvwTrx_ColumnHeader_3.Width = 336
        '
        '_lvwTrx_ColumnHeader_4
        '
        Me._lvwTrx_ColumnHeader_4.Text = "Amount"
        Me._lvwTrx_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwTrx_ColumnHeader_4.Width = 63
        '
        '_lvwTrx_ColumnHeader_5
        '
        Me._lvwTrx_ColumnHeader_5.Text = "Category"
        Me._lvwTrx_ColumnHeader_5.Width = 194
        '
        '_lvwTrx_ColumnHeader_6
        '
        Me._lvwTrx_ColumnHeader_6.Text = "Status"
        Me._lvwTrx_ColumnHeader_6.Width = 102
        '
        '_lvwTrx_ColumnHeader_7
        '
        Me._lvwTrx_ColumnHeader_7.Text = "Register"
        Me._lvwTrx_ColumnHeader_7.Width = 220
        '
        '_lvwTrx_ColumnHeader_8
        '
        Me._lvwTrx_ColumnHeader_8.Text = "Hidden Index"
        Me._lvwTrx_ColumnHeader_8.Width = 0
        '
        'lblDefaultCategory
        '
        Me.lblDefaultCategory.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDefaultCategory.BackColor = System.Drawing.SystemColors.Control
        Me.lblDefaultCategory.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDefaultCategory.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDefaultCategory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDefaultCategory.Location = New System.Drawing.Point(724, 500)
        Me.lblDefaultCategory.Name = "lblDefaultCategory"
        Me.lblDefaultCategory.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDefaultCategory.Size = New System.Drawing.Size(101, 15)
        Me.lblDefaultCategory.TabIndex = 20
        Me.lblDefaultCategory.Text = "Default Category:"
        '
        'lblSearchFor
        '
        Me.lblSearchFor.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSearchFor.BackColor = System.Drawing.SystemColors.Control
        Me.lblSearchFor.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblSearchFor.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearchFor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSearchFor.Location = New System.Drawing.Point(558, 296)
        Me.lblSearchFor.Name = "lblSearchFor"
        Me.lblSearchFor.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSearchFor.Size = New System.Drawing.Size(425, 15)
        Me.lblSearchFor.TabIndex = 7
        Me.lblSearchFor.Text = "(search for)"
        Me.lblSearchFor.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblDoubleClickHint
        '
        Me.lblDoubleClickHint.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDoubleClickHint.BackColor = System.Drawing.SystemColors.Control
        Me.lblDoubleClickHint.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblDoubleClickHint.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDoubleClickHint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDoubleClickHint.Location = New System.Drawing.Point(777, 25)
        Me.lblDoubleClickHint.Name = "lblDoubleClickHint"
        Me.lblDoubleClickHint.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblDoubleClickHint.Size = New System.Drawing.Size(312, 52)
        Me.lblDoubleClickHint.TabIndex = 4
        Me.lblDoubleClickHint.Text = "Double-click a row to create a quick transaction from it"
        Me.lblDoubleClickHint.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(5, 295)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(143, 15)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Matches To Current Item:"
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(724, 524)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(101, 15)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Import Into Register:"
        '
        'lblReadFrom
        '
        Me.lblReadFrom.BackColor = System.Drawing.SystemColors.Control
        Me.lblReadFrom.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblReadFrom.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReadFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblReadFrom.Location = New System.Drawing.Point(7, 5)
        Me.lblReadFrom.Name = "lblReadFrom"
        Me.lblReadFrom.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblReadFrom.Size = New System.Drawing.Size(767, 15)
        Me.lblReadFrom.TabIndex = 0
        Me.lblReadFrom.Text = "(read from source file)"
        '
        'BankImportForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1099, 548)
        Me.Controls.Add(Me.chkAllowManualBatchNew)
        Me.Controls.Add(Me.cboDefaultCategory)
        Me.Controls.Add(Me.cmdBatchNew)
        Me.Controls.Add(Me.cmdFindUpdates)
        Me.Controls.Add(Me.cmdBatchUpdates)
        Me.Controls.Add(Me.cmdFindNew)
        Me.Controls.Add(Me.chkLooseMatch)
        Me.Controls.Add(Me.cmdRefreshItems)
        Me.Controls.Add(Me.chkHideCompleted)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.cmdUpdateExisting)
        Me.Controls.Add(Me.lvwMatches)
        Me.Controls.Add(Me.cmdRepeatSearch)
        Me.Controls.Add(Me.cmdCreateNew)
        Me.Controls.Add(Me.cboRegister)
        Me.Controls.Add(Me.lvwTrx)
        Me.Controls.Add(Me.lblDefaultCategory)
        Me.Controls.Add(Me.lblSearchFor)
        Me.Controls.Add(Me.lblDoubleClickHint)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblReadFrom)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(2, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "BankImportForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        Me.Text = "Import Transactions From Bank"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents _lvwMatches_CH_Fake As ColumnHeader
    Friend WithEvents _lvwMatches_CH_Gen As ColumnHeader
    Friend WithEvents _lvwMatches_CH_Imported As ColumnHeader
    Friend WithEvents _lvwMatches_CH_DueDate As ColumnHeader
#End Region
End Class