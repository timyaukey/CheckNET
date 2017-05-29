<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AdjustPersonalBusinessForm
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
        Me.lblStartDate = New System.Windows.Forms.Label()
        Me.ctlStartDate = New System.Windows.Forms.DateTimePicker()
        Me.ctlEndDate = New System.Windows.Forms.DateTimePicker()
        Me.lblEndDate = New System.Windows.Forms.Label()
        Me.btnDeleteAdjustments = New System.Windows.Forms.Button()
        Me.btnRecreateAdjustments = New System.Windows.Forms.Button()
        Me.lblPersonalAccount = New System.Windows.Forms.Label()
        Me.lblPersonalPayments = New System.Windows.Forms.Label()
        Me.lblPersonalExpenses = New System.Windows.Forms.Label()
        Me.txtPersonalAccount = New System.Windows.Forms.TextBox()
        Me.txtPersonalPayments = New System.Windows.Forms.TextBox()
        Me.txtPersonalExpenses = New System.Windows.Forms.TextBox()
        Me.txtLoanToAccount = New System.Windows.Forms.TextBox()
        Me.lblLoanToAccount = New System.Windows.Forms.Label()
        Me.lblProgress = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'lblStartDate
        '
        Me.lblStartDate.AutoSize = True
        Me.lblStartDate.Location = New System.Drawing.Point(12, 18)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.Size = New System.Drawing.Size(58, 13)
        Me.lblStartDate.TabIndex = 0
        Me.lblStartDate.Text = "Start Date:"
        '
        'ctlStartDate
        '
        Me.ctlStartDate.Location = New System.Drawing.Point(128, 12)
        Me.ctlStartDate.Name = "ctlStartDate"
        Me.ctlStartDate.Size = New System.Drawing.Size(200, 20)
        Me.ctlStartDate.TabIndex = 1
        '
        'ctlEndDate
        '
        Me.ctlEndDate.Location = New System.Drawing.Point(128, 39)
        Me.ctlEndDate.Name = "ctlEndDate"
        Me.ctlEndDate.Size = New System.Drawing.Size(200, 20)
        Me.ctlEndDate.TabIndex = 3
        '
        'lblEndDate
        '
        Me.lblEndDate.AutoSize = True
        Me.lblEndDate.Location = New System.Drawing.Point(12, 44)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.Size = New System.Drawing.Size(55, 13)
        Me.lblEndDate.TabIndex = 2
        Me.lblEndDate.Text = "End Date:"
        '
        'btnDeleteAdjustments
        '
        Me.btnDeleteAdjustments.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteAdjustments.Location = New System.Drawing.Point(336, 229)
        Me.btnDeleteAdjustments.Name = "btnDeleteAdjustments"
        Me.btnDeleteAdjustments.Size = New System.Drawing.Size(184, 23)
        Me.btnDeleteAdjustments.TabIndex = 12
        Me.btnDeleteAdjustments.Text = "Delete Adjustments"
        Me.btnDeleteAdjustments.UseVisualStyleBackColor = True
        '
        'btnRecreateAdjustments
        '
        Me.btnRecreateAdjustments.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecreateAdjustments.Location = New System.Drawing.Point(336, 258)
        Me.btnRecreateAdjustments.Name = "btnRecreateAdjustments"
        Me.btnRecreateAdjustments.Size = New System.Drawing.Size(184, 23)
        Me.btnRecreateAdjustments.TabIndex = 13
        Me.btnRecreateAdjustments.Text = "(Re)create Adjustments"
        Me.btnRecreateAdjustments.UseVisualStyleBackColor = True
        '
        'lblPersonalAccount
        '
        Me.lblPersonalAccount.AutoSize = True
        Me.lblPersonalAccount.Location = New System.Drawing.Point(12, 68)
        Me.lblPersonalAccount.Name = "lblPersonalAccount"
        Me.lblPersonalAccount.Size = New System.Drawing.Size(94, 13)
        Me.lblPersonalAccount.TabIndex = 4
        Me.lblPersonalAccount.Text = "Personal Account:"
        '
        'lblPersonalPayments
        '
        Me.lblPersonalPayments.AutoSize = True
        Me.lblPersonalPayments.Location = New System.Drawing.Point(12, 120)
        Me.lblPersonalPayments.Name = "lblPersonalPayments"
        Me.lblPersonalPayments.Size = New System.Drawing.Size(100, 13)
        Me.lblPersonalPayments.TabIndex = 8
        Me.lblPersonalPayments.Text = "Personal Payments:"
        '
        'lblPersonalExpenses
        '
        Me.lblPersonalExpenses.AutoSize = True
        Me.lblPersonalExpenses.Location = New System.Drawing.Point(12, 146)
        Me.lblPersonalExpenses.Name = "lblPersonalExpenses"
        Me.lblPersonalExpenses.Size = New System.Drawing.Size(100, 13)
        Me.lblPersonalExpenses.TabIndex = 10
        Me.lblPersonalExpenses.Text = "Personal Expenses:"
        '
        'txtPersonalAccount
        '
        Me.txtPersonalAccount.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPersonalAccount.Location = New System.Drawing.Point(128, 65)
        Me.txtPersonalAccount.Name = "txtPersonalAccount"
        Me.txtPersonalAccount.ReadOnly = True
        Me.txtPersonalAccount.Size = New System.Drawing.Size(392, 20)
        Me.txtPersonalAccount.TabIndex = 5
        '
        'txtPersonalPayments
        '
        Me.txtPersonalPayments.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPersonalPayments.Location = New System.Drawing.Point(128, 117)
        Me.txtPersonalPayments.Name = "txtPersonalPayments"
        Me.txtPersonalPayments.ReadOnly = True
        Me.txtPersonalPayments.Size = New System.Drawing.Size(392, 20)
        Me.txtPersonalPayments.TabIndex = 9
        '
        'txtPersonalExpenses
        '
        Me.txtPersonalExpenses.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPersonalExpenses.Location = New System.Drawing.Point(128, 143)
        Me.txtPersonalExpenses.Name = "txtPersonalExpenses"
        Me.txtPersonalExpenses.ReadOnly = True
        Me.txtPersonalExpenses.Size = New System.Drawing.Size(392, 20)
        Me.txtPersonalExpenses.TabIndex = 11
        '
        'txtLoanToAccount
        '
        Me.txtLoanToAccount.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLoanToAccount.Location = New System.Drawing.Point(128, 91)
        Me.txtLoanToAccount.Name = "txtLoanToAccount"
        Me.txtLoanToAccount.ReadOnly = True
        Me.txtLoanToAccount.Size = New System.Drawing.Size(392, 20)
        Me.txtLoanToAccount.TabIndex = 7
        '
        'lblLoanToAccount
        '
        Me.lblLoanToAccount.AutoSize = True
        Me.lblLoanToAccount.Location = New System.Drawing.Point(12, 94)
        Me.lblLoanToAccount.Name = "lblLoanToAccount"
        Me.lblLoanToAccount.Size = New System.Drawing.Size(93, 13)
        Me.lblLoanToAccount.TabIndex = 6
        Me.lblLoanToAccount.Text = "Loan To Account:"
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(12, 178)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(62, 13)
        Me.lblProgress.TabIndex = 14
        Me.lblProgress.Text = "(progress...)"
        '
        'AdjustPersonalBusinessForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(532, 293)
        Me.Controls.Add(Me.lblProgress)
        Me.Controls.Add(Me.txtLoanToAccount)
        Me.Controls.Add(Me.lblLoanToAccount)
        Me.Controls.Add(Me.txtPersonalExpenses)
        Me.Controls.Add(Me.txtPersonalPayments)
        Me.Controls.Add(Me.txtPersonalAccount)
        Me.Controls.Add(Me.lblPersonalExpenses)
        Me.Controls.Add(Me.lblPersonalPayments)
        Me.Controls.Add(Me.lblPersonalAccount)
        Me.Controls.Add(Me.btnRecreateAdjustments)
        Me.Controls.Add(Me.btnDeleteAdjustments)
        Me.Controls.Add(Me.ctlEndDate)
        Me.Controls.Add(Me.lblEndDate)
        Me.Controls.Add(Me.ctlStartDate)
        Me.Controls.Add(Me.lblStartDate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AdjustPersonalBusinessForm"
        Me.Text = "Adjust Account For Personal Use"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblStartDate As Label
    Friend WithEvents ctlStartDate As DateTimePicker
    Friend WithEvents ctlEndDate As DateTimePicker
    Friend WithEvents lblEndDate As Label
    Friend WithEvents btnDeleteAdjustments As Button
    Friend WithEvents btnRecreateAdjustments As Button
    Friend WithEvents lblPersonalAccount As Label
    Friend WithEvents lblPersonalPayments As Label
    Friend WithEvents lblPersonalExpenses As Label
    Friend WithEvents txtPersonalAccount As TextBox
    Friend WithEvents txtPersonalPayments As TextBox
    Friend WithEvents txtPersonalExpenses As TextBox
    Friend WithEvents txtLoanToAccount As TextBox
    Friend WithEvents lblLoanToAccount As Label
    Friend WithEvents lblProgress As Label
End Class
