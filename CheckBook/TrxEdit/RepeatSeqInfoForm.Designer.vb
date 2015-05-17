<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class RepeatSeqInfoForm
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
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.grdTrx = New System.Windows.Forms.DataGridView
        Me.TrxDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Descr = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Amount = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.SeqNum = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DueDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Type = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.grdTrx, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grdTrx
        '
        Me.grdTrx.AllowUserToAddRows = False
        Me.grdTrx.AllowUserToDeleteRows = False
        Me.grdTrx.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grdTrx.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdTrx.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.TrxDate, Me.Descr, Me.Amount, Me.SeqNum, Me.DueDate, Me.Type})
        Me.grdTrx.Location = New System.Drawing.Point(12, 12)
        Me.grdTrx.Name = "grdTrx"
        Me.grdTrx.ReadOnly = True
        Me.grdTrx.RowHeadersVisible = False
        Me.grdTrx.Size = New System.Drawing.Size(602, 333)
        Me.grdTrx.TabIndex = 0
        '
        'TrxDate
        '
        Me.TrxDate.DataPropertyName = "TrxDate"
        Me.TrxDate.HeaderText = "Date"
        Me.TrxDate.Name = "TrxDate"
        Me.TrxDate.ReadOnly = True
        Me.TrxDate.Width = 65
        '
        'Descr
        '
        Me.Descr.DataPropertyName = "Descr"
        Me.Descr.HeaderText = "Description"
        Me.Descr.Name = "Descr"
        Me.Descr.ReadOnly = True
        Me.Descr.Width = 220
        '
        'Amount
        '
        Me.Amount.DataPropertyName = "Amount"
        Me.Amount.HeaderText = "Amount"
        Me.Amount.Name = "Amount"
        Me.Amount.ReadOnly = True
        Me.Amount.Width = 80
        '
        'SeqNum
        '
        Me.SeqNum.DataPropertyName = "SeqNum"
        Me.SeqNum.HeaderText = "Seq #"
        Me.SeqNum.Name = "SeqNum"
        Me.SeqNum.ReadOnly = True
        Me.SeqNum.Width = 60
        '
        'DueDate
        '
        Me.DueDate.DataPropertyName = "DueDate"
        Me.DueDate.HeaderText = "Due Date"
        Me.DueDate.Name = "DueDate"
        Me.DueDate.ReadOnly = True
        Me.DueDate.Width = 75
        '
        'Type
        '
        Me.Type.DataPropertyName = "Type"
        Me.Type.HeaderText = "Type"
        Me.Type.Name = "Type"
        Me.Type.ReadOnly = True
        Me.Type.Width = 60
        '
        'RepeatSeqInfoForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(626, 357)
        Me.Controls.Add(Me.grdTrx)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(3, 23)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "RepeatSeqInfoForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ShowInTaskbar = False
        Me.Text = "Transactions Using Repeat Key"
        CType(Me.grdTrx, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grdTrx As System.Windows.Forms.DataGridView
    Friend WithEvents TrxDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Descr As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Amount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SeqNum As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DueDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Type As System.Windows.Forms.DataGridViewTextBoxColumn
#End Region
End Class