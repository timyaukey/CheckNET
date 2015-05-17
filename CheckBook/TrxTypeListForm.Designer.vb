<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class TrxTypeListForm
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
	Public WithEvents cmdMoveDown As System.Windows.Forms.Button
	Public WithEvents cmdMoveUp As System.Windows.Forms.Button
	Public WithEvents cmdDiscardChanges As System.Windows.Forms.Button
	Public WithEvents cmdSaveChanges As System.Windows.Forms.Button
	Public WithEvents cmdDeleteTrxType As System.Windows.Forms.Button
	Public WithEvents cmdNewTrxType As System.Windows.Forms.Button
	Public WithEvents txtAfter As System.Windows.Forms.TextBox
	Public WithEvents txtMinAfter As System.Windows.Forms.TextBox
	Public WithEvents txtBefore As System.Windows.Forms.TextBox
	Public WithEvents txtNumber As System.Windows.Forms.TextBox
	Public WithEvents _lvwTrxTypes_ColumnHeader_1 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrxTypes_ColumnHeader_2 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrxTypes_ColumnHeader_3 As System.Windows.Forms.ColumnHeader
	Public WithEvents _lvwTrxTypes_ColumnHeader_4 As System.Windows.Forms.ColumnHeader
	Public WithEvents lvwTrxTypes As System.Windows.Forms.ListView
	Public WithEvents Label5 As System.Windows.Forms.Label
	Public WithEvents Label11 As System.Windows.Forms.Label
	Public WithEvents Label4 As System.Windows.Forms.Label
	Public WithEvents Label3 As System.Windows.Forms.Label
	Public WithEvents Label2 As System.Windows.Forms.Label
	Public WithEvents Label1 As System.Windows.Forms.Label
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TrxTypeListForm))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdMoveDown = New System.Windows.Forms.Button
        Me.cmdMoveUp = New System.Windows.Forms.Button
        Me.cmdDiscardChanges = New System.Windows.Forms.Button
        Me.cmdSaveChanges = New System.Windows.Forms.Button
        Me.cmdDeleteTrxType = New System.Windows.Forms.Button
        Me.cmdNewTrxType = New System.Windows.Forms.Button
        Me.txtAfter = New System.Windows.Forms.TextBox
        Me.txtMinAfter = New System.Windows.Forms.TextBox
        Me.txtBefore = New System.Windows.Forms.TextBox
        Me.txtNumber = New System.Windows.Forms.TextBox
        Me.lvwTrxTypes = New System.Windows.Forms.ListView
        Me._lvwTrxTypes_ColumnHeader_1 = New System.Windows.Forms.ColumnHeader
        Me._lvwTrxTypes_ColumnHeader_2 = New System.Windows.Forms.ColumnHeader
        Me._lvwTrxTypes_ColumnHeader_3 = New System.Windows.Forms.ColumnHeader
        Me._lvwTrxTypes_ColumnHeader_4 = New System.Windows.Forms.ColumnHeader
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdMoveDown
        '
        Me.cmdMoveDown.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMoveDown.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMoveDown.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMoveDown.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMoveDown.Location = New System.Drawing.Point(568, 338)
        Me.cmdMoveDown.Name = "cmdMoveDown"
        Me.cmdMoveDown.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMoveDown.Size = New System.Drawing.Size(141, 23)
        Me.cmdMoveDown.TabIndex = 12
        Me.cmdMoveDown.Text = "Move Down"
        Me.cmdMoveDown.UseVisualStyleBackColor = False
        '
        'cmdMoveUp
        '
        Me.cmdMoveUp.BackColor = System.Drawing.SystemColors.Control
        Me.cmdMoveUp.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdMoveUp.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMoveUp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdMoveUp.Location = New System.Drawing.Point(568, 312)
        Me.cmdMoveUp.Name = "cmdMoveUp"
        Me.cmdMoveUp.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdMoveUp.Size = New System.Drawing.Size(141, 23)
        Me.cmdMoveUp.TabIndex = 11
        Me.cmdMoveUp.Text = "Move Up"
        Me.cmdMoveUp.UseVisualStyleBackColor = False
        '
        'cmdDiscardChanges
        '
        Me.cmdDiscardChanges.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDiscardChanges.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDiscardChanges.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDiscardChanges.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDiscardChanges.Location = New System.Drawing.Point(568, 420)
        Me.cmdDiscardChanges.Name = "cmdDiscardChanges"
        Me.cmdDiscardChanges.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDiscardChanges.Size = New System.Drawing.Size(141, 23)
        Me.cmdDiscardChanges.TabIndex = 15
        Me.cmdDiscardChanges.Text = "Discard Changes"
        Me.cmdDiscardChanges.UseVisualStyleBackColor = False
        '
        'cmdSaveChanges
        '
        Me.cmdSaveChanges.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSaveChanges.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSaveChanges.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSaveChanges.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSaveChanges.Location = New System.Drawing.Point(568, 446)
        Me.cmdSaveChanges.Name = "cmdSaveChanges"
        Me.cmdSaveChanges.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSaveChanges.Size = New System.Drawing.Size(141, 23)
        Me.cmdSaveChanges.TabIndex = 16
        Me.cmdSaveChanges.Text = "Save Changes"
        Me.cmdSaveChanges.UseVisualStyleBackColor = False
        '
        'cmdDeleteTrxType
        '
        Me.cmdDeleteTrxType.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteTrxType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDeleteTrxType.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDeleteTrxType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteTrxType.Location = New System.Drawing.Point(568, 390)
        Me.cmdDeleteTrxType.Name = "cmdDeleteTrxType"
        Me.cmdDeleteTrxType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDeleteTrxType.Size = New System.Drawing.Size(141, 23)
        Me.cmdDeleteTrxType.TabIndex = 14
        Me.cmdDeleteTrxType.Text = "Delete Type"
        Me.cmdDeleteTrxType.UseVisualStyleBackColor = False
        '
        'cmdNewTrxType
        '
        Me.cmdNewTrxType.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewTrxType.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewTrxType.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewTrxType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewTrxType.Location = New System.Drawing.Point(568, 364)
        Me.cmdNewTrxType.Name = "cmdNewTrxType"
        Me.cmdNewTrxType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewTrxType.Size = New System.Drawing.Size(141, 23)
        Me.cmdNewTrxType.TabIndex = 13
        Me.cmdNewTrxType.Text = "New Type"
        Me.cmdNewTrxType.UseVisualStyleBackColor = False
        '
        'txtAfter
        '
        Me.txtAfter.AcceptsReturn = True
        Me.txtAfter.BackColor = System.Drawing.SystemColors.Window
        Me.txtAfter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAfter.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAfter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtAfter.Location = New System.Drawing.Point(194, 340)
        Me.txtAfter.MaxLength = 0
        Me.txtAfter.Name = "txtAfter"
        Me.txtAfter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtAfter.Size = New System.Drawing.Size(299, 23)
        Me.txtAfter.TabIndex = 5
        '
        'txtMinAfter
        '
        Me.txtMinAfter.AcceptsReturn = True
        Me.txtMinAfter.BackColor = System.Drawing.SystemColors.Window
        Me.txtMinAfter.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMinAfter.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinAfter.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtMinAfter.Location = New System.Drawing.Point(194, 366)
        Me.txtMinAfter.MaxLength = 0
        Me.txtMinAfter.Name = "txtMinAfter"
        Me.txtMinAfter.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMinAfter.Size = New System.Drawing.Size(77, 23)
        Me.txtMinAfter.TabIndex = 7
        '
        'txtBefore
        '
        Me.txtBefore.AcceptsReturn = True
        Me.txtBefore.BackColor = System.Drawing.SystemColors.Window
        Me.txtBefore.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBefore.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtBefore.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBefore.Location = New System.Drawing.Point(194, 314)
        Me.txtBefore.MaxLength = 0
        Me.txtBefore.Name = "txtBefore"
        Me.txtBefore.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBefore.Size = New System.Drawing.Size(299, 23)
        Me.txtBefore.TabIndex = 3
        '
        'txtNumber
        '
        Me.txtNumber.AcceptsReturn = True
        Me.txtNumber.BackColor = System.Drawing.SystemColors.Window
        Me.txtNumber.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtNumber.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNumber.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNumber.Location = New System.Drawing.Point(194, 392)
        Me.txtNumber.MaxLength = 0
        Me.txtNumber.Name = "txtNumber"
        Me.txtNumber.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtNumber.Size = New System.Drawing.Size(61, 23)
        Me.txtNumber.TabIndex = 9
        '
        'lvwTrxTypes
        '
        Me.lvwTrxTypes.BackColor = System.Drawing.SystemColors.Window
        Me.lvwTrxTypes.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me._lvwTrxTypes_ColumnHeader_1, Me._lvwTrxTypes_ColumnHeader_2, Me._lvwTrxTypes_ColumnHeader_3, Me._lvwTrxTypes_ColumnHeader_4})
        Me.lvwTrxTypes.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwTrxTypes.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lvwTrxTypes.FullRowSelect = True
        Me.lvwTrxTypes.HideSelection = False
        Me.lvwTrxTypes.Location = New System.Drawing.Point(4, 99)
        Me.lvwTrxTypes.Name = "lvwTrxTypes"
        Me.lvwTrxTypes.Size = New System.Drawing.Size(709, 204)
        Me.lvwTrxTypes.TabIndex = 1
        Me.lvwTrxTypes.UseCompatibleStateImageBehavior = False
        Me.lvwTrxTypes.View = System.Windows.Forms.View.Details
        '
        '_lvwTrxTypes_ColumnHeader_1
        '
        Me._lvwTrxTypes_ColumnHeader_1.Text = "Starts With"
        Me._lvwTrxTypes_ColumnHeader_1.Width = 220
        '
        '_lvwTrxTypes_ColumnHeader_2
        '
        Me._lvwTrxTypes_ColumnHeader_2.Text = "Ends With"
        Me._lvwTrxTypes_ColumnHeader_2.Width = 200
        '
        '_lvwTrxTypes_ColumnHeader_3
        '
        Me._lvwTrxTypes_ColumnHeader_3.Text = "Min"
        Me._lvwTrxTypes_ColumnHeader_3.Width = 70
        '
        '_lvwTrxTypes_ColumnHeader_4
        '
        Me._lvwTrxTypes_ColumnHeader_4.Text = "Number"
        Me._lvwTrxTypes_ColumnHeader_4.Width = 70
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label5.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(268, 396)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label5.Size = New System.Drawing.Size(257, 47)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "(Enter ""(number)"" to use what remains of the description after trimming as the ch" & _
            "eck number)"
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.SystemColors.Control
        Me.Label11.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label11.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label11.Location = New System.Drawing.Point(8, 4)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(689, 92)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = resources.GetString("Label11.Text")
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.SystemColors.Control
        Me.Label4.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label4.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(22, 342)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label4.Size = New System.Drawing.Size(169, 17)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Description Ends With:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label3
        '
        Me.Label3.BackColor = System.Drawing.SystemColors.Control
        Me.Label3.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label3.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(4, 368)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label3.Size = New System.Drawing.Size(185, 17)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Min. Characters Of Ending To Match:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.SystemColors.Control
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(22, 316)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label2.Size = New System.Drawing.Size(169, 17)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Description Starts With:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(4, 394)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(185, 17)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "Check Number To Use If Matched:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'TrxTypeListForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(717, 476)
        Me.ControlBox = False
        Me.Controls.Add(Me.cmdMoveDown)
        Me.Controls.Add(Me.cmdMoveUp)
        Me.Controls.Add(Me.cmdDiscardChanges)
        Me.Controls.Add(Me.cmdSaveChanges)
        Me.Controls.Add(Me.cmdDeleteTrxType)
        Me.Controls.Add(Me.cmdNewTrxType)
        Me.Controls.Add(Me.txtAfter)
        Me.Controls.Add(Me.txtMinAfter)
        Me.Controls.Add(Me.txtBefore)
        Me.Controls.Add(Me.txtNumber)
        Me.Controls.Add(Me.lvwTrxTypes)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 23)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TrxTypeListForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Transaction Import Type List"
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class