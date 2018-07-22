<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class ReconcileForm
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
	Public WithEvents cmdLater As System.Windows.Forms.Button
	Public WithEvents cmdFinish As System.Windows.Forms.Button
	Public WithEvents txtEndingBalance As System.Windows.Forms.TextBox
	Public WithEvents txtClearedBalance As System.Windows.Forms.TextBox
	Public WithEvents txtStartingBalance As System.Windows.Forms.TextBox
	Public WithEvents _lvwTrx_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_5 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_6 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_7 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_8 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_9 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrx_ColumnHeader_10 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwTrx As System.Windows.Forms.ListView
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.cmdLater = New System.Windows.Forms.Button()
        Me.cmdFinish = New System.Windows.Forms.Button()
        Me.txtEndingBalance = New System.Windows.Forms.TextBox()
        Me.txtClearedBalance = New System.Windows.Forms.TextBox()
        Me.txtStartingBalance = New System.Windows.Forms.TextBox()
        Me.lvwTrx = New System.Windows.Forms.ListView()
        Me._lvwTrx_ColumnHeader_1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_6 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_7 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_8 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_9 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me._lvwTrx_ColumnHeader_10 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnSelectThroughDate = New System.Windows.Forms.Button()
        Me.txtSelectThroughDate = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'cmdCancel
        '
        Me.cmdCancel.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCancel.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdCancel.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCancel.Location = New System.Drawing.Point(493, 495)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdCancel.Size = New System.Drawing.Size(114, 23)
        Me.cmdCancel.TabIndex = 11
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'cmdLater
        '
        Me.cmdLater.BackColor = System.Drawing.SystemColors.Control
        Me.cmdLater.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdLater.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdLater.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdLater.Location = New System.Drawing.Point(493, 468)
        Me.cmdLater.Name = "cmdLater"
        Me.cmdLater.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdLater.Size = New System.Drawing.Size(114, 23)
        Me.cmdLater.TabIndex = 10
        Me.cmdLater.Text = "Finish Later"
        Me.cmdLater.UseVisualStyleBackColor = False
        '
        'cmdFinish
        '
        Me.cmdFinish.BackColor = System.Drawing.SystemColors.Control
        Me.cmdFinish.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdFinish.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdFinish.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdFinish.Location = New System.Drawing.Point(493, 442)
        Me.cmdFinish.Name = "cmdFinish"
        Me.cmdFinish.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdFinish.Size = New System.Drawing.Size(114, 23)
        Me.cmdFinish.TabIndex = 9
        Me.cmdFinish.Text = "Finish"
        Me.cmdFinish.UseVisualStyleBackColor = False
        '
        'txtEndingBalance
        '
        Me.txtEndingBalance.AcceptsReturn = True
        Me.txtEndingBalance.BackColor = System.Drawing.SystemColors.Window
        Me.txtEndingBalance.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtEndingBalance.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEndingBalance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEndingBalance.Location = New System.Drawing.Point(111, 495)
        Me.txtEndingBalance.MaxLength = 0
        Me.txtEndingBalance.Name = "txtEndingBalance"
        Me.txtEndingBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtEndingBalance.Size = New System.Drawing.Size(80, 20)
        Me.txtEndingBalance.TabIndex = 6
        Me.txtEndingBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtClearedBalance
        '
        Me.txtClearedBalance.AcceptsReturn = True
        Me.txtClearedBalance.BackColor = System.Drawing.SystemColors.Control
        Me.txtClearedBalance.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtClearedBalance.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtClearedBalance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtClearedBalance.Location = New System.Drawing.Point(111, 468)
        Me.txtClearedBalance.MaxLength = 0
        Me.txtClearedBalance.Name = "txtClearedBalance"
        Me.txtClearedBalance.ReadOnly = True
        Me.txtClearedBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtClearedBalance.Size = New System.Drawing.Size(80, 20)
        Me.txtClearedBalance.TabIndex = 4
        Me.txtClearedBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStartingBalance
        '
        Me.txtStartingBalance.AcceptsReturn = True
        Me.txtStartingBalance.BackColor = System.Drawing.SystemColors.Control
        Me.txtStartingBalance.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtStartingBalance.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtStartingBalance.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtStartingBalance.Location = New System.Drawing.Point(111, 442)
        Me.txtStartingBalance.MaxLength = 0
        Me.txtStartingBalance.Name = "txtStartingBalance"
        Me.txtStartingBalance.ReadOnly = True
        Me.txtStartingBalance.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtStartingBalance.Size = New System.Drawing.Size(80, 20)
        Me.txtStartingBalance.TabIndex = 2
        Me.txtStartingBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lvwTrx
        '
        Me.lvwTrx.BackColor = System.Drawing.SystemColors.Window
        Me.lvwTrx.CheckBoxes = True
        Me.lvwTrx.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwTrx_ColumnHeader_1, Me._lvwTrx_ColumnHeader_2, Me._lvwTrx_ColumnHeader_3, Me._lvwTrx_ColumnHeader_4, Me._lvwTrx_ColumnHeader_5, Me._lvwTrx_ColumnHeader_6, Me._lvwTrx_ColumnHeader_7, Me._lvwTrx_ColumnHeader_8, Me._lvwTrx_ColumnHeader_9, Me._lvwTrx_ColumnHeader_10})
        Me.lvwTrx.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTrx.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTrx.FullRowSelect = True
        Me.lvwTrx.HideSelection = False
        Me.lvwTrx.Location = New System.Drawing.Point(7, 7)
        Me.lvwTrx.Name = "lvwTrx"
        Me.lvwTrx.Size = New System.Drawing.Size(599, 423)
        Me.lvwTrx.TabIndex = 0
        Me.lvwTrx.UseCompatibleStateImageBehavior = False
        Me.lvwTrx.View = System.Windows.Forms.View.Details
        '
        '_lvwTrx_ColumnHeader_1
        '
        Me._lvwTrx_ColumnHeader_1.Text = ""
        Me._lvwTrx_ColumnHeader_1.Width = 26
        '
        '_lvwTrx_ColumnHeader_2
        '
        Me._lvwTrx_ColumnHeader_2.Text = "Date"
        Me._lvwTrx_ColumnHeader_2.Width = 59
        '
        '_lvwTrx_ColumnHeader_3
        '
        Me._lvwTrx_ColumnHeader_3.Text = "Number"
        Me._lvwTrx_ColumnHeader_3.Width = 61
        '
        '_lvwTrx_ColumnHeader_4
        '
        Me._lvwTrx_ColumnHeader_4.Text = "Description"
        Me._lvwTrx_ColumnHeader_4.Width = 228
        '
        '_lvwTrx_ColumnHeader_5
        '
        Me._lvwTrx_ColumnHeader_5.Text = "Amount"
        Me._lvwTrx_ColumnHeader_5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me._lvwTrx_ColumnHeader_5.Width = 67
        '
        '_lvwTrx_ColumnHeader_6
        '
        Me._lvwTrx_ColumnHeader_6.Text = "Imported"
        Me._lvwTrx_ColumnHeader_6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        '_lvwTrx_ColumnHeader_7
        '
        Me._lvwTrx_ColumnHeader_7.Text = "Bank Date"
        Me._lvwTrx_ColumnHeader_7.Width = 118
        '
        '_lvwTrx_ColumnHeader_8
        '
        Me._lvwTrx_ColumnHeader_8.Text = "HiddenSortableDate"
        Me._lvwTrx_ColumnHeader_8.Width = 1
        '
        '_lvwTrx_ColumnHeader_9
        '
        Me._lvwTrx_ColumnHeader_9.Text = "HiddenSortableNumber"
        Me._lvwTrx_ColumnHeader_9.Width = 1
        '
        '_lvwTrx_ColumnHeader_10
        '
        Me._lvwTrx_ColumnHeader_10.Text = "ArrayIndex"
        Me._lvwTrx_ColumnHeader_10.Width = 1
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(8, 497)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(97, 18)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Ending Balance:"
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(8, 471)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(97, 18)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Cleared Balance:"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(8, 444)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(97, 18)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Starting Balance:"
        '
        'btnSelectThroughDate
        '
        Me.btnSelectThroughDate.BackColor = System.Drawing.SystemColors.Control
        Me.btnSelectThroughDate.Cursor = System.Windows.Forms.Cursors.Default
        Me.btnSelectThroughDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectThroughDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSelectThroughDate.Location = New System.Drawing.Point(234, 442)
        Me.btnSelectThroughDate.Name = "btnSelectThroughDate"
        Me.btnSelectThroughDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.btnSelectThroughDate.Size = New System.Drawing.Size(129, 23)
        Me.btnSelectThroughDate.TabIndex = 7
        Me.btnSelectThroughDate.Text = "Select Through Date"
        Me.btnSelectThroughDate.UseVisualStyleBackColor = False
        '
        'txtSelectThroughDate
        '
        Me.txtSelectThroughDate.AcceptsReturn = True
        Me.txtSelectThroughDate.BackColor = System.Drawing.SystemColors.Window
        Me.txtSelectThroughDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtSelectThroughDate.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSelectThroughDate.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtSelectThroughDate.Location = New System.Drawing.Point(369, 443)
        Me.txtSelectThroughDate.MaxLength = 0
        Me.txtSelectThroughDate.Name = "txtSelectThroughDate"
        Me.txtSelectThroughDate.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSelectThroughDate.Size = New System.Drawing.Size(70, 20)
        Me.txtSelectThroughDate.TabIndex = 8
        Me.txtSelectThroughDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ReconcileForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(614, 532)
        Me.Controls.Add(Me.txtSelectThroughDate)
        Me.Controls.Add(Me.btnSelectThroughDate)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdLater)
        Me.Controls.Add(Me.cmdFinish)
        Me.Controls.Add(Me.txtEndingBalance)
        Me.Controls.Add(Me.txtClearedBalance)
        Me.Controls.Add(Me.txtStartingBalance)
        Me.Controls.Add(Me.lvwTrx)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(2, 22)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ReconcileForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Reconcile Account"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public WithEvents btnSelectThroughDate As Button
    Public WithEvents txtSelectThroughDate As TextBox
#End Region
End Class