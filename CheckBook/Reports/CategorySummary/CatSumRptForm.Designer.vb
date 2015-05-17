<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class CatSumRptForm
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
	Public WithEvents cmdResultToClipboard As System.Windows.Forms.Button
	Public WithEvents txtStartDate As System.Windows.Forms.TextBox
	Public WithEvents txtEndDate As System.Windows.Forms.TextBox
	Public WithEvents chkIncludeFake As System.Windows.Forms.CheckBox
	Public WithEvents chkIncludeGenerated As System.Windows.Forms.CheckBox
	Public WithEvents lstAccounts As System.Windows.Forms.ListBox
    Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdResultToClipboard = New System.Windows.Forms.Button
        Me.txtStartDate = New System.Windows.Forms.TextBox
        Me.txtEndDate = New System.Windows.Forms.TextBox
        Me.chkIncludeFake = New System.Windows.Forms.CheckBox
        Me.chkIncludeGenerated = New System.Windows.Forms.CheckBox
        Me.lstAccounts = New System.Windows.Forms.ListBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.grdResults = New System.Windows.Forms.DataGridView
        Me.Label = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Amount = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.grdResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdResultToClipboard
        '
        Me.cmdResultToClipboard.BackColor = System.Drawing.SystemColors.Control
        Me.cmdResultToClipboard.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdResultToClipboard.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdResultToClipboard.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdResultToClipboard.Location = New System.Drawing.Point(8, 82)
        Me.cmdResultToClipboard.Name = "cmdResultToClipboard"
        Me.cmdResultToClipboard.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdResultToClipboard.Size = New System.Drawing.Size(133, 23)
        Me.cmdResultToClipboard.TabIndex = 7
        Me.cmdResultToClipboard.Text = "Results To Clipboard"
        Me.cmdResultToClipboard.UseVisualStyleBackColor = False
        '
        'txtStartDate
        '
        Me.txtStartDate.AcceptsReturn = True
        Me.txtStartDate.BackColor = System.Drawing.SystemColors.Control
        Me.txtStartDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartDate.Enabled = False
        Me.txtStartDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartDate.Location = New System.Drawing.Point(345, 7)
        Me.txtStartDate.MaxLength = 0
        Me.txtStartDate.Name = "txtStartDate"
        Me.txtStartDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartDate.Size = New System.Drawing.Size(68, 20)
        Me.txtStartDate.TabIndex = 2
        '
        'txtEndDate
        '
        Me.txtEndDate.AcceptsReturn = True
        Me.txtEndDate.BackColor = System.Drawing.SystemColors.Control
        Me.txtEndDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEndDate.Enabled = False
        Me.txtEndDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEndDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEndDate.Location = New System.Drawing.Point(345, 29)
        Me.txtEndDate.MaxLength = 0
        Me.txtEndDate.Name = "txtEndDate"
        Me.txtEndDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEndDate.Size = New System.Drawing.Size(68, 20)
        Me.txtEndDate.TabIndex = 4
        '
        'chkIncludeFake
        '
        Me.chkIncludeFake.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeFake.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeFake.Enabled = False
        Me.chkIncludeFake.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncludeFake.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeFake.Location = New System.Drawing.Point(264, 51)
        Me.chkIncludeFake.Name = "chkIncludeFake"
        Me.chkIncludeFake.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeFake.Size = New System.Drawing.Size(90, 18)
        Me.chkIncludeFake.TabIndex = 5
        Me.chkIncludeFake.Text = "Include Fake"
        Me.chkIncludeFake.UseVisualStyleBackColor = False
        '
        'chkIncludeGenerated
        '
        Me.chkIncludeGenerated.BackColor = System.Drawing.SystemColors.Control
        Me.chkIncludeGenerated.Cursor = System.Windows.Forms.Cursors.Default
        Me.chkIncludeGenerated.Enabled = False
        Me.chkIncludeGenerated.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkIncludeGenerated.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkIncludeGenerated.Location = New System.Drawing.Point(264, 70)
        Me.chkIncludeGenerated.Name = "chkIncludeGenerated"
        Me.chkIncludeGenerated.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkIncludeGenerated.Size = New System.Drawing.Size(116, 18)
        Me.chkIncludeGenerated.TabIndex = 6
        Me.chkIncludeGenerated.Text = "Include Generated"
        Me.chkIncludeGenerated.UseVisualStyleBackColor = False
        '
        'lstAccounts
        '
        Me.lstAccounts.BackColor = System.Drawing.SystemColors.Control
        Me.lstAccounts.Cursor = System.Windows.Forms.Cursors.Default
        Me.lstAccounts.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstAccounts.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstAccounts.ItemHeight = 14
        Me.lstAccounts.Location = New System.Drawing.Point(5, 8)
        Me.lstAccounts.Name = "lstAccounts"
        Me.lstAccounts.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lstAccounts.Size = New System.Drawing.Size(251, 60)
        Me.lstAccounts.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(264, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(75, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Start Date:"
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(264, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(75, 16)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "End Date:"
        '
        'grdResults
        '
        Me.grdResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdResults.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Label, Me.Amount})
        Me.grdResults.Location = New System.Drawing.Point(5, 111)
        Me.grdResults.Name = "grdResults"
        Me.grdResults.RowHeadersVisible = False
        Me.grdResults.Size = New System.Drawing.Size(408, 330)
        Me.grdResults.TabIndex = 8
        '
        'Label
        '
        Me.Label.DataPropertyName = "Label"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft
        Me.Label.DefaultCellStyle = DataGridViewCellStyle1
        Me.Label.HeaderText = "Category"
        Me.Label.Name = "Label"
        Me.Label.Width = 280
        '
        'Amount
        '
        Me.Amount.DataPropertyName = "Amount"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        Me.Amount.DefaultCellStyle = DataGridViewCellStyle2
        Me.Amount.HeaderText = "Amount"
        Me.Amount.Name = "Amount"
        '
        'CatSumRptForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(419, 445)
        Me.Controls.Add(Me.grdResults)
        Me.Controls.Add(Me.cmdResultToClipboard)
        Me.Controls.Add(Me.txtStartDate)
        Me.Controls.Add(Me.txtEndDate)
        Me.Controls.Add(Me.chkIncludeFake)
        Me.Controls.Add(Me.chkIncludeGenerated)
        Me.Controls.Add(Me.lstAccounts)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(3, 23)
        Me.Name = "CatSumRptForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        Me.Text = "Report of Totals By Category"
        CType(Me.grdResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grdResults As System.Windows.Forms.DataGridView
    Friend WithEvents Label As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Amount As System.Windows.Forms.DataGridViewTextBoxColumn
#End Region
End Class