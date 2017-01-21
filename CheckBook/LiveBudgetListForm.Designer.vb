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
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtTargetDate = New System.Windows.Forms.TextBox()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.lvwMatches = New System.Windows.Forms.ListView()
        Me._lvwMatches_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwMatches_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtTargetDate
        '
        Me.txtTargetDate.AcceptsReturn = True
        Me.txtTargetDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtTargetDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtTargetDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtTargetDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtTargetDate.Location = New System.Drawing.Point(80, 8)
        Me.txtTargetDate.MaxLength = 0
        Me.txtTargetDate.Name = "txtTargetDate"
        Me.txtTargetDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtTargetDate.Size = New System.Drawing.Size(64, 20)
        Me.txtTargetDate.TabIndex = 1
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Location = New System.Drawing.Point(550, 8)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(97, 25)
        Me.cmdSearch.TabIndex = 2
        Me.cmdSearch.Text = "Search"
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'lvwMatches
        '
        Me.lvwMatches.BackColor = System.Drawing.SystemColors.Window
        Me.lvwMatches.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwMatches_ColumnHeader_1, Me._lvwMatches_ColumnHeader_2, Me._lvwMatches_ColumnHeader_3, Me._lvwMatches_ColumnHeader_4, Me._lvwMatches_ColumnHeader_5, Me._lvwMatches_ColumnHeader_6})
        Me.lvwMatches.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwMatches.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwMatches.FullRowSelect = True
        Me.lvwMatches.HideSelection = False
        Me.lvwMatches.Location = New System.Drawing.Point(8, 36)
        Me.lvwMatches.Name = "lvwMatches"
        Me.lvwMatches.Size = New System.Drawing.Size(640, 172)
        Me.lvwMatches.TabIndex = 3
        Me.lvwMatches.UseCompatibleStateImageBehavior = False
        Me.lvwMatches.View = System.Windows.Forms.View.Details
        '
        '_lvwMatches_ColumnHeader_1
        '
        Me._lvwMatches_ColumnHeader_1.Text = "Date"
        Me._lvwMatches_ColumnHeader_1.Width = 69
        '
        '_lvwMatches_ColumnHeader_2
        '
        Me._lvwMatches_ColumnHeader_2.Text = "Description"
        Me._lvwMatches_ColumnHeader_2.Width = 219
        '
        '_lvwMatches_ColumnHeader_3
        '
        Me._lvwMatches_ColumnHeader_3.Text = "Budget"
        Me._lvwMatches_ColumnHeader_3.Width = 87
        '
        '_lvwMatches_ColumnHeader_4
        '
        Me._lvwMatches_ColumnHeader_4.Text = "Limit"
        Me._lvwMatches_ColumnHeader_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwMatches_ColumnHeader_4.Width = 80
        '
        '_lvwMatches_ColumnHeader_5
        '
        Me._lvwMatches_ColumnHeader_5.Text = "Applied"
        Me._lvwMatches_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwMatches_ColumnHeader_5.Width = 78
        '
        '_lvwMatches_ColumnHeader_6
        '
        Me._lvwMatches_ColumnHeader_6.Text = "Amount"
        Me._lvwMatches_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwMatches_ColumnHeader_6.Width = 78
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(8, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(68, 18)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Target Date:"
        '
        'LiveBudgetListForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(656, 220)
        Me.Controls.Add(Me.txtTargetDate)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.lvwMatches)
        Me.Controls.Add(Me.Label3)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(2, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LiveBudgetListForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Live Budget Transactions"
        Me.ResumeLayout(False)

    End Sub
#End Region
End Class