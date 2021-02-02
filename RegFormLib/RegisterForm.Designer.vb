<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class RegisterForm
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
	Public WithEvents cmdSearch As System.Windows.Forms.Button
	Public WithEvents cmdNewXfer As System.Windows.Forms.Button
	Public WithEvents cmdNewBudget As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
	Public WithEvents cmdEdit As System.Windows.Forms.Button
	Public WithEvents cmdNewNormal As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RegisterForm))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdNewXfer = New System.Windows.Forms.Button()
        Me.cmdNewBudget = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdEdit = New System.Windows.Forms.Button()
        Me.cmdNewNormal = New System.Windows.Forms.Button()
        Me.grdReg = New System.Windows.Forms.DataGridView()
        CType(Me.grdReg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdSearch
        '
        Me.cmdSearch.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdSearch.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Location = New System.Drawing.Point(441, 472)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdSearch.Size = New System.Drawing.Size(62, 23)
        Me.cmdSearch.TabIndex = 6
        Me.cmdSearch.Text = "&Search"
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdNewXfer
        '
        Me.cmdNewXfer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdNewXfer.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewXfer.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewXfer.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewXfer.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewXfer.Location = New System.Drawing.Point(193, 472)
        Me.cmdNewXfer.Name = "cmdNewXfer"
        Me.cmdNewXfer.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewXfer.Size = New System.Drawing.Size(90, 23)
        Me.cmdNewXfer.TabIndex = 3
        Me.cmdNewXfer.Text = "New &Xfer"
        Me.cmdNewXfer.UseVisualStyleBackColor = False
        '
        'cmdNewBudget
        '
        Me.cmdNewBudget.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdNewBudget.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewBudget.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewBudget.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewBudget.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewBudget.Location = New System.Drawing.Point(99, 472)
        Me.cmdNewBudget.Name = "cmdNewBudget"
        Me.cmdNewBudget.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewBudget.Size = New System.Drawing.Size(90, 23)
        Me.cmdNewBudget.TabIndex = 2
        Me.cmdNewBudget.Text = "New &Budget"
        Me.cmdNewBudget.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdDelete.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Location = New System.Drawing.Point(373, 472)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdDelete.Size = New System.Drawing.Size(62, 23)
        Me.cmdDelete.TabIndex = 5
        Me.cmdDelete.Text = "&Delete"
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'cmdEdit
        '
        Me.cmdEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdEdit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEdit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdEdit.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEdit.Location = New System.Drawing.Point(307, 472)
        Me.cmdEdit.Name = "cmdEdit"
        Me.cmdEdit.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdEdit.Size = New System.Drawing.Size(62, 23)
        Me.cmdEdit.TabIndex = 4
        Me.cmdEdit.Text = "&Edit"
        Me.cmdEdit.UseVisualStyleBackColor = False
        '
        'cmdNewNormal
        '
        Me.cmdNewNormal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdNewNormal.BackColor = System.Drawing.SystemColors.Control
        Me.cmdNewNormal.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdNewNormal.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNewNormal.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdNewNormal.Location = New System.Drawing.Point(5, 472)
        Me.cmdNewNormal.Name = "cmdNewNormal"
        Me.cmdNewNormal.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cmdNewNormal.Size = New System.Drawing.Size(90, 23)
        Me.cmdNewNormal.TabIndex = 1
        Me.cmdNewNormal.Text = "&New Trans"
        Me.cmdNewNormal.UseVisualStyleBackColor = False
        '
        'grdReg
        '
        Me.grdReg.AllowUserToAddRows = False
        Me.grdReg.AllowUserToDeleteRows = False
        Me.grdReg.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdReg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdReg.Location = New System.Drawing.Point(12, 12)
        Me.grdReg.Name = "grdReg"
        Me.grdReg.ReadOnly = True
        Me.grdReg.RowHeadersWidth = 24
        Me.grdReg.Size = New System.Drawing.Size(994, 454)
        Me.grdReg.TabIndex = 7
        Me.grdReg.VirtualMode = True
        '
        'RegisterForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(1018, 500)
        Me.Controls.Add(Me.grdReg)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.cmdNewXfer)
        Me.Controls.Add(Me.cmdNewBudget)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.cmdEdit)
        Me.Controls.Add(Me.cmdNewNormal)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Location = New System.Drawing.Point(3, 23)
        Me.Name = "RegisterForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds
        Me.Text = "Register"
        CType(Me.grdReg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grdReg As System.Windows.Forms.DataGridView
#End Region 
End Class