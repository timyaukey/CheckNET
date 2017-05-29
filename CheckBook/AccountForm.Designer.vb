<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AccountForm
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
        Me.txtAccountName = New System.Windows.Forms.TextBox()
        Me.cboAccountType = New System.Windows.Forms.ComboBox()
        Me.lblAccountName = New System.Windows.Forms.Label()
        Me.lblAccountType = New System.Windows.Forms.Label()
        Me.btnOkay = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblFileName = New System.Windows.Forms.Label()
        Me.txtFileName = New System.Windows.Forms.TextBox()
        Me.lblRelated1 = New System.Windows.Forms.Label()
        Me.cboRelated1 = New System.Windows.Forms.ComboBox()
        Me.lblRelated2 = New System.Windows.Forms.Label()
        Me.cboRelated2 = New System.Windows.Forms.ComboBox()
        Me.lblRelated3 = New System.Windows.Forms.Label()
        Me.cboRelated3 = New System.Windows.Forms.ComboBox()
        Me.lblRelated4 = New System.Windows.Forms.Label()
        Me.cboRelated4 = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'txtAccountName
        '
        Me.txtAccountName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAccountName.Location = New System.Drawing.Point(132, 12)
        Me.txtAccountName.Name = "txtAccountName"
        Me.txtAccountName.Size = New System.Drawing.Size(289, 20)
        Me.txtAccountName.TabIndex = 1
        '
        'cboAccountType
        '
        Me.cboAccountType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAccountType.FormattingEnabled = True
        Me.cboAccountType.Location = New System.Drawing.Point(132, 64)
        Me.cboAccountType.Name = "cboAccountType"
        Me.cboAccountType.Size = New System.Drawing.Size(289, 21)
        Me.cboAccountType.TabIndex = 5
        '
        'lblAccountName
        '
        Me.lblAccountName.AutoSize = True
        Me.lblAccountName.Location = New System.Drawing.Point(12, 15)
        Me.lblAccountName.Name = "lblAccountName"
        Me.lblAccountName.Size = New System.Drawing.Size(78, 13)
        Me.lblAccountName.TabIndex = 0
        Me.lblAccountName.Text = "Account Name"
        '
        'lblAccountType
        '
        Me.lblAccountType.AutoSize = True
        Me.lblAccountType.Location = New System.Drawing.Point(12, 67)
        Me.lblAccountType.Name = "lblAccountType"
        Me.lblAccountType.Size = New System.Drawing.Size(74, 13)
        Me.lblAccountType.TabIndex = 4
        Me.lblAccountType.Text = "Account Type"
        '
        'btnOkay
        '
        Me.btnOkay.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOkay.Location = New System.Drawing.Point(221, 209)
        Me.btnOkay.Name = "btnOkay"
        Me.btnOkay.Size = New System.Drawing.Size(97, 23)
        Me.btnOkay.TabIndex = 6
        Me.btnOkay.Text = "OK"
        Me.btnOkay.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.Location = New System.Drawing.Point(324, 209)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(97, 23)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblFileName
        '
        Me.lblFileName.AutoSize = True
        Me.lblFileName.Location = New System.Drawing.Point(12, 41)
        Me.lblFileName.Name = "lblFileName"
        Me.lblFileName.Size = New System.Drawing.Size(54, 13)
        Me.lblFileName.TabIndex = 2
        Me.lblFileName.Text = "File Name"
        '
        'txtFileName
        '
        Me.txtFileName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFileName.Location = New System.Drawing.Point(132, 38)
        Me.txtFileName.Name = "txtFileName"
        Me.txtFileName.Size = New System.Drawing.Size(289, 20)
        Me.txtFileName.TabIndex = 3
        '
        'lblRelated1
        '
        Me.lblRelated1.AutoSize = True
        Me.lblRelated1.Location = New System.Drawing.Point(12, 94)
        Me.lblRelated1.Name = "lblRelated1"
        Me.lblRelated1.Size = New System.Drawing.Size(103, 13)
        Me.lblRelated1.TabIndex = 8
        Me.lblRelated1.Text = "Related Account #1"
        '
        'cboRelated1
        '
        Me.cboRelated1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRelated1.FormattingEnabled = True
        Me.cboRelated1.Location = New System.Drawing.Point(132, 91)
        Me.cboRelated1.Name = "cboRelated1"
        Me.cboRelated1.Size = New System.Drawing.Size(289, 21)
        Me.cboRelated1.TabIndex = 9
        '
        'lblRelated2
        '
        Me.lblRelated2.AutoSize = True
        Me.lblRelated2.Location = New System.Drawing.Point(12, 121)
        Me.lblRelated2.Name = "lblRelated2"
        Me.lblRelated2.Size = New System.Drawing.Size(103, 13)
        Me.lblRelated2.TabIndex = 10
        Me.lblRelated2.Text = "Related Account #2"
        '
        'cboRelated2
        '
        Me.cboRelated2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRelated2.FormattingEnabled = True
        Me.cboRelated2.Location = New System.Drawing.Point(132, 118)
        Me.cboRelated2.Name = "cboRelated2"
        Me.cboRelated2.Size = New System.Drawing.Size(289, 21)
        Me.cboRelated2.TabIndex = 11
        '
        'lblRelated3
        '
        Me.lblRelated3.AutoSize = True
        Me.lblRelated3.Location = New System.Drawing.Point(12, 148)
        Me.lblRelated3.Name = "lblRelated3"
        Me.lblRelated3.Size = New System.Drawing.Size(103, 13)
        Me.lblRelated3.TabIndex = 12
        Me.lblRelated3.Text = "Related Account #3"
        '
        'cboRelated3
        '
        Me.cboRelated3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRelated3.FormattingEnabled = True
        Me.cboRelated3.Location = New System.Drawing.Point(132, 145)
        Me.cboRelated3.Name = "cboRelated3"
        Me.cboRelated3.Size = New System.Drawing.Size(289, 21)
        Me.cboRelated3.TabIndex = 13
        '
        'lblRelated4
        '
        Me.lblRelated4.AutoSize = True
        Me.lblRelated4.Location = New System.Drawing.Point(12, 175)
        Me.lblRelated4.Name = "lblRelated4"
        Me.lblRelated4.Size = New System.Drawing.Size(103, 13)
        Me.lblRelated4.TabIndex = 14
        Me.lblRelated4.Text = "Related Account #4"
        '
        'cboRelated4
        '
        Me.cboRelated4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboRelated4.FormattingEnabled = True
        Me.cboRelated4.Location = New System.Drawing.Point(132, 172)
        Me.cboRelated4.Name = "cboRelated4"
        Me.cboRelated4.Size = New System.Drawing.Size(289, 21)
        Me.cboRelated4.TabIndex = 15
        '
        'AccountForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(433, 244)
        Me.Controls.Add(Me.lblRelated4)
        Me.Controls.Add(Me.cboRelated4)
        Me.Controls.Add(Me.lblRelated3)
        Me.Controls.Add(Me.cboRelated3)
        Me.Controls.Add(Me.lblRelated2)
        Me.Controls.Add(Me.cboRelated2)
        Me.Controls.Add(Me.lblRelated1)
        Me.Controls.Add(Me.cboRelated1)
        Me.Controls.Add(Me.lblFileName)
        Me.Controls.Add(Me.txtFileName)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOkay)
        Me.Controls.Add(Me.lblAccountType)
        Me.Controls.Add(Me.lblAccountName)
        Me.Controls.Add(Me.cboAccountType)
        Me.Controls.Add(Me.txtAccountName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AccountForm"
        Me.Text = "Account Properties"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtAccountName As TextBox
    Friend WithEvents cboAccountType As ComboBox
    Friend WithEvents lblAccountName As Label
    Friend WithEvents lblAccountType As Label
    Friend WithEvents btnOkay As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents lblFileName As Label
    Friend WithEvents txtFileName As TextBox
    Friend WithEvents lblRelated1 As Label
    Friend WithEvents cboRelated1 As ComboBox
    Friend WithEvents lblRelated2 As Label
    Friend WithEvents cboRelated2 As ComboBox
    Friend WithEvents lblRelated3 As Label
    Friend WithEvents cboRelated3 As ComboBox
    Friend WithEvents lblRelated4 As Label
    Friend WithEvents cboRelated4 As ComboBox
End Class
