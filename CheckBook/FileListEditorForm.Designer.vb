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
        Me.btnNewFile = New System.Windows.Forms.Button()
        Me.cboNewType = New System.Windows.Forms.ComboBox()
        Me.lblNewName = New System.Windows.Forms.Label()
        Me.lblNewType = New System.Windows.Forms.Label()
        Me.txtNewName = New System.Windows.Forms.TextBox()
        Me.btnDeleteFile = New System.Windows.Forms.Button()
        Me.btnRenameFile = New System.Windows.Forms.Button()
        Me.btnEditFile = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lvwFiles
        '
        Me.lvwFiles.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwFiles.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colName, Me.colDate})
        Me.lvwFiles.FullRowSelect = True
        Me.lvwFiles.HideSelection = False
        Me.lvwFiles.Location = New System.Drawing.Point(12, 12)
        Me.lvwFiles.Name = "lvwFiles"
        Me.lvwFiles.Size = New System.Drawing.Size(569, 324)
        Me.lvwFiles.TabIndex = 0
        Me.lvwFiles.UseCompatibleStateImageBehavior = False
        Me.lvwFiles.View = System.Windows.Forms.View.Details
        '
        'colName
        '
        Me.colName.Text = "File Name"
        Me.colName.Width = 400
        '
        'colDate
        '
        Me.colDate.Text = "Last Modified"
        Me.colDate.Width = 140
        '
        'btnNewFile
        '
        Me.btnNewFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnNewFile.Location = New System.Drawing.Point(459, 371)
        Me.btnNewFile.Name = "btnNewFile"
        Me.btnNewFile.Size = New System.Drawing.Size(122, 23)
        Me.btnNewFile.TabIndex = 6
        Me.btnNewFile.Text = "New File"
        Me.btnNewFile.UseVisualStyleBackColor = True
        '
        'cboNewType
        '
        Me.cboNewType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cboNewType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboNewType.FormattingEnabled = True
        Me.cboNewType.Location = New System.Drawing.Point(100, 368)
        Me.cboNewType.Name = "cboNewType"
        Me.cboNewType.Size = New System.Drawing.Size(210, 21)
        Me.cboNewType.TabIndex = 4
        '
        'lblNewName
        '
        Me.lblNewName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblNewName.AutoSize = True
        Me.lblNewName.Location = New System.Drawing.Point(12, 347)
        Me.lblNewName.Name = "lblNewName"
        Me.lblNewName.Size = New System.Drawing.Size(82, 13)
        Me.lblNewName.TabIndex = 1
        Me.lblNewName.Text = "New File Name:"
        '
        'lblNewType
        '
        Me.lblNewType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblNewType.AutoSize = True
        Me.lblNewType.Location = New System.Drawing.Point(12, 371)
        Me.lblNewType.Name = "lblNewType"
        Me.lblNewType.Size = New System.Drawing.Size(78, 13)
        Me.lblNewType.TabIndex = 3
        Me.lblNewType.Text = "New File Type:"
        '
        'txtNewName
        '
        Me.txtNewName.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtNewName.Location = New System.Drawing.Point(100, 342)
        Me.txtNewName.Name = "txtNewName"
        Me.txtNewName.Size = New System.Drawing.Size(210, 20)
        Me.txtNewName.TabIndex = 2
        '
        'btnDeleteFile
        '
        Me.btnDeleteFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteFile.Location = New System.Drawing.Point(459, 429)
        Me.btnDeleteFile.Name = "btnDeleteFile"
        Me.btnDeleteFile.Size = New System.Drawing.Size(122, 23)
        Me.btnDeleteFile.TabIndex = 8
        Me.btnDeleteFile.Text = "Delete File"
        Me.btnDeleteFile.UseVisualStyleBackColor = True
        '
        'btnRenameFile
        '
        Me.btnRenameFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRenameFile.Location = New System.Drawing.Point(459, 400)
        Me.btnRenameFile.Name = "btnRenameFile"
        Me.btnRenameFile.Size = New System.Drawing.Size(122, 23)
        Me.btnRenameFile.TabIndex = 7
        Me.btnRenameFile.Text = "Rename File"
        Me.btnRenameFile.UseVisualStyleBackColor = True
        '
        'btnEditFile
        '
        Me.btnEditFile.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnEditFile.Location = New System.Drawing.Point(459, 342)
        Me.btnEditFile.Name = "btnEditFile"
        Me.btnEditFile.Size = New System.Drawing.Size(122, 23)
        Me.btnEditFile.TabIndex = 5
        Me.btnEditFile.Text = "Edit File"
        Me.btnEditFile.UseVisualStyleBackColor = True
        '
        'FileListEditorForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(593, 464)
        Me.Controls.Add(Me.btnEditFile)
        Me.Controls.Add(Me.btnRenameFile)
        Me.Controls.Add(Me.btnDeleteFile)
        Me.Controls.Add(Me.txtNewName)
        Me.Controls.Add(Me.lblNewType)
        Me.Controls.Add(Me.lblNewName)
        Me.Controls.Add(Me.cboNewType)
        Me.Controls.Add(Me.btnNewFile)
        Me.Controls.Add(Me.lvwFiles)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FileListEditorForm"
        Me.Text = "FileListEditorForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lvwFiles As System.Windows.Forms.ListView
    Friend WithEvents colName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDate As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnNewFile As System.Windows.Forms.Button
    Friend WithEvents cboNewType As System.Windows.Forms.ComboBox
    Friend WithEvents lblNewName As System.Windows.Forms.Label
    Friend WithEvents lblNewType As System.Windows.Forms.Label
    Friend WithEvents txtNewName As System.Windows.Forms.TextBox
    Friend WithEvents btnDeleteFile As System.Windows.Forms.Button
    Friend WithEvents btnRenameFile As System.Windows.Forms.Button
    Friend WithEvents btnEditFile As System.Windows.Forms.Button
End Class
