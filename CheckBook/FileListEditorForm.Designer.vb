<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FileListEditorForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lvwFiles = New System.Windows.Forms.ListView()
        Me.colName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SuspendLayout()
        '
        'lvwFiles
        '
        Me.lvwFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colName, Me.colDate, Me.colSize})
        Me.lvwFiles.FullRowSelect = True
        Me.lvwFiles.Location = New System.Drawing.Point(12, 12)
        Me.lvwFiles.Name = "lvwFiles"
        Me.lvwFiles.Size = New System.Drawing.Size(558, 395)
        Me.lvwFiles.TabIndex = 0
        Me.lvwFiles.UseCompatibleStateImageBehavior = False
        Me.lvwFiles.View = System.Windows.Forms.View.Details
        '
        'colName
        '
        Me.colName.Text = "File Name"
        Me.colName.Width = 301
        '
        'colDate
        '
        Me.colDate.Text = "Last Modified"
        Me.colDate.Width = 140
        '
        'colSize
        '
        Me.colSize.Text = "Size"
        Me.colSize.Width = 80
        '
        'FileListEditorForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(582, 485)
        Me.Controls.Add(Me.lvwFiles)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FileListEditorForm"
        Me.Text = "FileListEditorForm"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvwFiles As System.Windows.Forms.ListView
    Friend WithEvents colName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDate As System.Windows.Forms.ColumnHeader
    Friend WithEvents colSize As System.Windows.Forms.ColumnHeader
End Class
