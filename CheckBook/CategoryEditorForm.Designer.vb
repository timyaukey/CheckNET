<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CategoryEditorForm
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
        Me.btnOkay = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.lblName = New System.Windows.Forms.Label()
        Me.cboType = New System.Windows.Forms.ComboBox()
        Me.lblType = New System.Windows.Forms.Label()
        Me.lblExplanation = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'btnOkay
        '
        Me.btnOkay.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOkay.Location = New System.Drawing.Point(220, 122)
        Me.btnOkay.Name = "btnOkay"
        Me.btnOkay.Size = New System.Drawing.Size(75, 23)
        Me.btnOkay.TabIndex = 0
        Me.btnOkay.Text = "OK"
        Me.btnOkay.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(301, 122)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(56, 12)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(320, 20)
        Me.txtName.TabIndex = 2
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(12, 15)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(38, 13)
        Me.lblName.TabIndex = 3
        Me.lblName.Text = "Name:"
        '
        'cboType
        '
        Me.cboType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.FormattingEnabled = True
        Me.cboType.Location = New System.Drawing.Point(56, 38)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(320, 21)
        Me.cboType.TabIndex = 4
        '
        'lblType
        '
        Me.lblType.AutoSize = True
        Me.lblType.Location = New System.Drawing.Point(12, 41)
        Me.lblType.Name = "lblType"
        Me.lblType.Size = New System.Drawing.Size(34, 13)
        Me.lblType.TabIndex = 5
        Me.lblType.Text = "Type:"
        '
        'lblExplanation
        '
        Me.lblExplanation.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblExplanation.Location = New System.Drawing.Point(56, 66)
        Me.lblExplanation.Name = "lblExplanation"
        Me.lblExplanation.Size = New System.Drawing.Size(320, 53)
        Me.lblExplanation.TabIndex = 6
        Me.lblExplanation.Text = "Category type is used to classify income and expenses on financial statements. If" &
    " no type is chosen it will be treated as ""Operating Expenses""."
        '
        'CategoryEditorForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(388, 157)
        Me.Controls.Add(Me.lblExplanation)
        Me.Controls.Add(Me.lblType)
        Me.Controls.Add(Me.cboType)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOkay)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CategoryEditorForm"
        Me.Text = "Category Information"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnOkay As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents txtName As TextBox
    Friend WithEvents lblName As Label
    Friend WithEvents cboType As ComboBox
    Friend WithEvents lblType As Label
    Friend WithEvents lblExplanation As Label
End Class
