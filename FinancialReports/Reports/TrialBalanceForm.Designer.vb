<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TrialBalanceForm
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
        Me.ctlEndDate = New System.Windows.Forms.DateTimePicker()
        Me.lblEndDate = New System.Windows.Forms.Label()
        Me.lvwBalanceSheetAccounts = New System.Windows.Forms.ListView()
        Me.lvwBalSheetCol_Title = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwBalSheetCol_Amount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwIncExpAccounts = New System.Windows.Forms.ListView()
        Me.lvwIncExpCol_Title = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lvwIncExpCol_Amount = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnTrialBalance = New System.Windows.Forms.Button()
        Me.lblStartDate = New System.Windows.Forms.Label()
        Me.ctlStartDate = New System.Windows.Forms.DateTimePicker()
        Me.lblBalanceSheetAccounts = New System.Windows.Forms.Label()
        Me.lblIncomeExpenseAccounts = New System.Windows.Forms.Label()
        Me.btnBalanceSheet = New System.Windows.Forms.Button()
        Me.btnIncomeExpenseStatement = New System.Windows.Forms.Button()
        Me.lblResultSummary = New System.Windows.Forms.Label()
        Me.btnPostRetainedEarnings = New System.Windows.Forms.Button()
        Me.btnAccountsPayable = New System.Windows.Forms.Button()
        Me.btnLoansPayable = New System.Windows.Forms.Button()
        Me.btnAccountsReceivable = New System.Windows.Forms.Button()
        Me.btnLoansReceivable = New System.Windows.Forms.Button()
        Me.lblAgingDate = New System.Windows.Forms.Label()
        Me.ctlAgingDate = New System.Windows.Forms.DateTimePicker()
        Me.SuspendLayout()
        '
        'ctlEndDate
        '
        Me.ctlEndDate.Location = New System.Drawing.Point(93, 38)
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
        'lvwBalanceSheetAccounts
        '
        Me.lvwBalanceSheetAccounts.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwBalanceSheetAccounts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.lvwBalSheetCol_Title, Me.lvwBalSheetCol_Amount})
        Me.lvwBalanceSheetAccounts.FullRowSelect = True
        Me.lvwBalanceSheetAccounts.HideSelection = False
        Me.lvwBalanceSheetAccounts.Location = New System.Drawing.Point(12, 120)
        Me.lvwBalanceSheetAccounts.Name = "lvwBalanceSheetAccounts"
        Me.lvwBalanceSheetAccounts.Size = New System.Drawing.Size(537, 146)
        Me.lvwBalanceSheetAccounts.TabIndex = 7
        Me.lvwBalanceSheetAccounts.UseCompatibleStateImageBehavior = False
        Me.lvwBalanceSheetAccounts.View = System.Windows.Forms.View.Details
        '
        'lvwBalSheetCol_Title
        '
        Me.lvwBalSheetCol_Title.Text = "Account Name"
        Me.lvwBalSheetCol_Title.Width = 400
        '
        'lvwBalSheetCol_Amount
        '
        Me.lvwBalSheetCol_Amount.Text = "Amount"
        Me.lvwBalSheetCol_Amount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.lvwBalSheetCol_Amount.Width = 100
        '
        'lvwIncExpAccounts
        '
        Me.lvwIncExpAccounts.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwIncExpAccounts.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.lvwIncExpCol_Title, Me.lvwIncExpCol_Amount})
        Me.lvwIncExpAccounts.FullRowSelect = True
        Me.lvwIncExpAccounts.HideSelection = False
        Me.lvwIncExpAccounts.Location = New System.Drawing.Point(12, 296)
        Me.lvwIncExpAccounts.Name = "lvwIncExpAccounts"
        Me.lvwIncExpAccounts.Size = New System.Drawing.Size(537, 146)
        Me.lvwIncExpAccounts.TabIndex = 9
        Me.lvwIncExpAccounts.UseCompatibleStateImageBehavior = False
        Me.lvwIncExpAccounts.View = System.Windows.Forms.View.Details
        '
        'lvwIncExpCol_Title
        '
        Me.lvwIncExpCol_Title.Text = "Account Name"
        Me.lvwIncExpCol_Title.Width = 400
        '
        'lvwIncExpCol_Amount
        '
        Me.lvwIncExpCol_Amount.Text = "Amount"
        Me.lvwIncExpCol_Amount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.lvwIncExpCol_Amount.Width = 100
        '
        'btnTrialBalance
        '
        Me.btnTrialBalance.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnTrialBalance.Location = New System.Drawing.Point(399, 531)
        Me.btnTrialBalance.Name = "btnTrialBalance"
        Me.btnTrialBalance.Size = New System.Drawing.Size(150, 23)
        Me.btnTrialBalance.TabIndex = 17
        Me.btnTrialBalance.Text = "Trial Balance"
        Me.btnTrialBalance.UseVisualStyleBackColor = True
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
        Me.ctlStartDate.Location = New System.Drawing.Point(93, 12)
        Me.ctlStartDate.Name = "ctlStartDate"
        Me.ctlStartDate.Size = New System.Drawing.Size(200, 20)
        Me.ctlStartDate.TabIndex = 1
        '
        'lblBalanceSheetAccounts
        '
        Me.lblBalanceSheetAccounts.AutoSize = True
        Me.lblBalanceSheetAccounts.Location = New System.Drawing.Point(12, 104)
        Me.lblBalanceSheetAccounts.Name = "lblBalanceSheetAccounts"
        Me.lblBalanceSheetAccounts.Size = New System.Drawing.Size(125, 13)
        Me.lblBalanceSheetAccounts.TabIndex = 6
        Me.lblBalanceSheetAccounts.Text = "Balance Sheet Accounts"
        '
        'lblIncomeExpenseAccounts
        '
        Me.lblIncomeExpenseAccounts.AutoSize = True
        Me.lblIncomeExpenseAccounts.Location = New System.Drawing.Point(12, 280)
        Me.lblIncomeExpenseAccounts.Name = "lblIncomeExpenseAccounts"
        Me.lblIncomeExpenseAccounts.Size = New System.Drawing.Size(136, 13)
        Me.lblIncomeExpenseAccounts.TabIndex = 8
        Me.lblIncomeExpenseAccounts.Text = "Income/Expense Accounts"
        '
        'btnBalanceSheet
        '
        Me.btnBalanceSheet.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnBalanceSheet.Location = New System.Drawing.Point(12, 502)
        Me.btnBalanceSheet.Name = "btnBalanceSheet"
        Me.btnBalanceSheet.Size = New System.Drawing.Size(150, 23)
        Me.btnBalanceSheet.TabIndex = 11
        Me.btnBalanceSheet.Text = "Balance Sheet"
        Me.btnBalanceSheet.UseVisualStyleBackColor = True
        '
        'btnIncomeExpenseStatement
        '
        Me.btnIncomeExpenseStatement.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnIncomeExpenseStatement.Location = New System.Drawing.Point(168, 502)
        Me.btnIncomeExpenseStatement.Name = "btnIncomeExpenseStatement"
        Me.btnIncomeExpenseStatement.Size = New System.Drawing.Size(150, 23)
        Me.btnIncomeExpenseStatement.TabIndex = 12
        Me.btnIncomeExpenseStatement.Text = "Profit and Loss"
        Me.btnIncomeExpenseStatement.UseVisualStyleBackColor = True
        '
        'lblResultSummary
        '
        Me.lblResultSummary.AutoSize = True
        Me.lblResultSummary.Location = New System.Drawing.Point(15, 449)
        Me.lblResultSummary.Name = "lblResultSummary"
        Me.lblResultSummary.Size = New System.Drawing.Size(82, 13)
        Me.lblResultSummary.TabIndex = 10
        Me.lblResultSummary.Text = "(result summary)"
        '
        'btnPostRetainedEarnings
        '
        Me.btnPostRetainedEarnings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPostRetainedEarnings.Location = New System.Drawing.Point(399, 560)
        Me.btnPostRetainedEarnings.Name = "btnPostRetainedEarnings"
        Me.btnPostRetainedEarnings.Size = New System.Drawing.Size(150, 23)
        Me.btnPostRetainedEarnings.TabIndex = 18
        Me.btnPostRetainedEarnings.Text = "Post Retained Earnings"
        Me.btnPostRetainedEarnings.UseVisualStyleBackColor = True
        '
        'btnAccountsPayable
        '
        Me.btnAccountsPayable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAccountsPayable.Location = New System.Drawing.Point(168, 560)
        Me.btnAccountsPayable.Name = "btnAccountsPayable"
        Me.btnAccountsPayable.Size = New System.Drawing.Size(150, 23)
        Me.btnAccountsPayable.TabIndex = 16
        Me.btnAccountsPayable.Text = "Accounts Payable"
        Me.btnAccountsPayable.UseVisualStyleBackColor = True
        '
        'btnLoansPayable
        '
        Me.btnLoansPayable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnLoansPayable.Location = New System.Drawing.Point(12, 560)
        Me.btnLoansPayable.Name = "btnLoansPayable"
        Me.btnLoansPayable.Size = New System.Drawing.Size(150, 23)
        Me.btnLoansPayable.TabIndex = 15
        Me.btnLoansPayable.Text = "Loans Payable"
        Me.btnLoansPayable.UseVisualStyleBackColor = True
        '
        'btnAccountsReceivable
        '
        Me.btnAccountsReceivable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAccountsReceivable.Location = New System.Drawing.Point(168, 531)
        Me.btnAccountsReceivable.Name = "btnAccountsReceivable"
        Me.btnAccountsReceivable.Size = New System.Drawing.Size(150, 23)
        Me.btnAccountsReceivable.TabIndex = 14
        Me.btnAccountsReceivable.Text = "Accounts Receivable"
        Me.btnAccountsReceivable.UseVisualStyleBackColor = True
        '
        'btnLoansReceivable
        '
        Me.btnLoansReceivable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnLoansReceivable.Location = New System.Drawing.Point(12, 531)
        Me.btnLoansReceivable.Name = "btnLoansReceivable"
        Me.btnLoansReceivable.Size = New System.Drawing.Size(150, 23)
        Me.btnLoansReceivable.TabIndex = 13
        Me.btnLoansReceivable.Text = "Loans Receivable"
        Me.btnLoansReceivable.UseVisualStyleBackColor = True
        '
        'lblAgingDate
        '
        Me.lblAgingDate.AutoSize = True
        Me.lblAgingDate.Location = New System.Drawing.Point(12, 70)
        Me.lblAgingDate.Name = "lblAgingDate"
        Me.lblAgingDate.Size = New System.Drawing.Size(63, 13)
        Me.lblAgingDate.TabIndex = 4
        Me.lblAgingDate.Text = "Aging Date:"
        '
        'ctlAgingDate
        '
        Me.ctlAgingDate.Location = New System.Drawing.Point(93, 64)
        Me.ctlAgingDate.Name = "ctlAgingDate"
        Me.ctlAgingDate.Size = New System.Drawing.Size(200, 20)
        Me.ctlAgingDate.TabIndex = 5
        '
        'TrialBalanceForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(561, 595)
        Me.Controls.Add(Me.lblAgingDate)
        Me.Controls.Add(Me.ctlAgingDate)
        Me.Controls.Add(Me.btnLoansReceivable)
        Me.Controls.Add(Me.btnAccountsReceivable)
        Me.Controls.Add(Me.btnAccountsPayable)
        Me.Controls.Add(Me.btnLoansPayable)
        Me.Controls.Add(Me.btnPostRetainedEarnings)
        Me.Controls.Add(Me.lblResultSummary)
        Me.Controls.Add(Me.btnIncomeExpenseStatement)
        Me.Controls.Add(Me.btnBalanceSheet)
        Me.Controls.Add(Me.lblIncomeExpenseAccounts)
        Me.Controls.Add(Me.lblBalanceSheetAccounts)
        Me.Controls.Add(Me.lblStartDate)
        Me.Controls.Add(Me.ctlStartDate)
        Me.Controls.Add(Me.btnTrialBalance)
        Me.Controls.Add(Me.lvwIncExpAccounts)
        Me.Controls.Add(Me.lvwBalanceSheetAccounts)
        Me.Controls.Add(Me.lblEndDate)
        Me.Controls.Add(Me.ctlEndDate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "TrialBalanceForm"
        Me.ShowInTaskbar = False
        Me.Text = "Trial Balance and Statements"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ctlEndDate As DateTimePicker
    Friend WithEvents lblEndDate As Label
    Friend WithEvents lvwBalanceSheetAccounts As ListView
    Friend WithEvents lvwIncExpAccounts As ListView
    Friend WithEvents btnTrialBalance As Button
    Friend WithEvents lvwBalSheetCol_Title As ColumnHeader
    Friend WithEvents lvwBalSheetCol_Amount As ColumnHeader
    Friend WithEvents lvwIncExpCol_Title As ColumnHeader
    Friend WithEvents lvwIncExpCol_Amount As ColumnHeader
    Friend WithEvents lblStartDate As Label
    Friend WithEvents ctlStartDate As DateTimePicker
    Friend WithEvents lblBalanceSheetAccounts As Label
    Friend WithEvents lblIncomeExpenseAccounts As Label
    Friend WithEvents btnBalanceSheet As Button
    Friend WithEvents btnIncomeExpenseStatement As Button
    Friend WithEvents lblResultSummary As Label
    Friend WithEvents btnPostRetainedEarnings As Button
    Friend WithEvents btnAccountsPayable As Button
    Friend WithEvents btnLoansPayable As Button
    Friend WithEvents btnAccountsReceivable As Button
    Friend WithEvents btnLoansReceivable As Button
    Friend WithEvents lblAgingDate As Label
    Friend WithEvents ctlAgingDate As DateTimePicker
End Class
