<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class CBMainForm
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
    Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuImport As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTools As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuListPayees As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuListCategories As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuListBudgets As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuListTrxTypes As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSetup As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRpt As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuWindows As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuMain As System.Windows.Forms.MenuStrip
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CBMainForm))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.mnuMain = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuImport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuImportBank = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuImportChecks = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuImportDeposits = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuImportInvoices = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRpt = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSetup = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListPayees = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListCategories = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListBudgets = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListTrxTypes = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCheckFormat = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuCompanyInformation = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuLicensing = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUserAccounts = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuEnableUserAccounts = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAddUserAccount = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuChangeCurrentPassword = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuChangeOtherPassword = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuDeleteUserAccount = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRepairUserAccounts = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuWindows = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHelp = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'mnuMain
        '
        Me.mnuMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuImport, Me.mnuTools, Me.mnuRpt, Me.mnuSetup, Me.mnuWindows, Me.mnuHelp})
        Me.mnuMain.Location = New System.Drawing.Point(0, 0)
        Me.mnuMain.MdiWindowListItem = Me.mnuWindows
        Me.mnuMain.Name = "mnuMain"
        Me.mnuMain.Size = New System.Drawing.Size(1006, 24)
        Me.mnuMain.TabIndex = 1
        '
        'mnuFile
        '
        Me.mnuFile.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(37, 20)
        Me.mnuFile.Text = "File"
        '
        'mnuImport
        '
        Me.mnuImport.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuImportBank, Me.mnuImportChecks, Me.mnuImportDeposits, Me.mnuImportInvoices})
        Me.mnuImport.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuImport.Name = "mnuImport"
        Me.mnuImport.Size = New System.Drawing.Size(55, 20)
        Me.mnuImport.Text = "Import"
        '
        'mnuImportBank
        '
        Me.mnuImportBank.Name = "mnuImportBank"
        Me.mnuImportBank.Size = New System.Drawing.Size(157, 22)
        Me.mnuImportBank.Text = "Bank Download"
        '
        'mnuImportChecks
        '
        Me.mnuImportChecks.Name = "mnuImportChecks"
        Me.mnuImportChecks.Size = New System.Drawing.Size(157, 22)
        Me.mnuImportChecks.Text = "Checks"
        '
        'mnuImportDeposits
        '
        Me.mnuImportDeposits.Name = "mnuImportDeposits"
        Me.mnuImportDeposits.Size = New System.Drawing.Size(157, 22)
        Me.mnuImportDeposits.Text = "Deposits"
        '
        'mnuImportInvoices
        '
        Me.mnuImportInvoices.Name = "mnuImportInvoices"
        Me.mnuImportInvoices.Size = New System.Drawing.Size(157, 22)
        Me.mnuImportInvoices.Text = "Invoices"
        '
        'mnuTools
        '
        Me.mnuTools.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuTools.Name = "mnuTools"
        Me.mnuTools.Size = New System.Drawing.Size(46, 20)
        Me.mnuTools.Text = "Tools"
        '
        'mnuRpt
        '
        Me.mnuRpt.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuRpt.Name = "mnuRpt"
        Me.mnuRpt.Size = New System.Drawing.Size(59, 20)
        Me.mnuRpt.Text = "Reports"
        '
        'mnuSetup
        '
        Me.mnuSetup.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuListPayees, Me.mnuListCategories, Me.mnuListBudgets, Me.mnuListTrxTypes, Me.mnuCheckFormat, Me.mnuCompanyInformation, Me.mnuLicensing, Me.mnuUserAccounts})
        Me.mnuSetup.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuSetup.Name = "mnuSetup"
        Me.mnuSetup.Size = New System.Drawing.Size(49, 20)
        Me.mnuSetup.Text = "Setup"
        '
        'mnuListPayees
        '
        Me.mnuListPayees.Name = "mnuListPayees"
        Me.mnuListPayees.Size = New System.Drawing.Size(213, 22)
        Me.mnuListPayees.Text = "Memorized Transactions"
        '
        'mnuListCategories
        '
        Me.mnuListCategories.Name = "mnuListCategories"
        Me.mnuListCategories.Size = New System.Drawing.Size(213, 22)
        Me.mnuListCategories.Text = "Categories"
        '
        'mnuListBudgets
        '
        Me.mnuListBudgets.Name = "mnuListBudgets"
        Me.mnuListBudgets.Size = New System.Drawing.Size(213, 22)
        Me.mnuListBudgets.Text = "Budgets"
        '
        'mnuListTrxTypes
        '
        Me.mnuListTrxTypes.Name = "mnuListTrxTypes"
        Me.mnuListTrxTypes.Size = New System.Drawing.Size(213, 22)
        Me.mnuListTrxTypes.Text = "Transaction Import Types"
        '
        'mnuCheckFormat
        '
        Me.mnuCheckFormat.Name = "mnuCheckFormat"
        Me.mnuCheckFormat.Size = New System.Drawing.Size(213, 22)
        Me.mnuCheckFormat.Text = "Check Format"
        '
        'mnuCompanyInformation
        '
        Me.mnuCompanyInformation.Name = "mnuCompanyInformation"
        Me.mnuCompanyInformation.Size = New System.Drawing.Size(213, 22)
        Me.mnuCompanyInformation.Text = "Company Information"
        '
        'mnuLicensing
        '
        Me.mnuLicensing.Name = "mnuLicensing"
        Me.mnuLicensing.Size = New System.Drawing.Size(213, 22)
        Me.mnuLicensing.Text = "Licensing and Registration"
        '
        'mnuUserAccounts
        '
        Me.mnuUserAccounts.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuEnableUserAccounts, Me.mnuAddUserAccount, Me.mnuChangeCurrentPassword, Me.mnuChangeOtherPassword, Me.mnuDeleteUserAccount, Me.mnuRepairUserAccounts})
        Me.mnuUserAccounts.Name = "mnuUserAccounts"
        Me.mnuUserAccounts.Size = New System.Drawing.Size(213, 22)
        Me.mnuUserAccounts.Text = "User Logins"
        '
        'mnuEnableUserAccounts
        '
        Me.mnuEnableUserAccounts.Name = "mnuEnableUserAccounts"
        Me.mnuEnableUserAccounts.Size = New System.Drawing.Size(237, 22)
        Me.mnuEnableUserAccounts.Text = "Enable User Logins"
        '
        'mnuAddUserAccount
        '
        Me.mnuAddUserAccount.Name = "mnuAddUserAccount"
        Me.mnuAddUserAccount.Size = New System.Drawing.Size(237, 22)
        Me.mnuAddUserAccount.Text = "Add User Login"
        '
        'mnuChangeCurrentPassword
        '
        Me.mnuChangeCurrentPassword.Name = "mnuChangeCurrentPassword"
        Me.mnuChangeCurrentPassword.Size = New System.Drawing.Size(237, 22)
        Me.mnuChangeCurrentPassword.Text = "Change Current User Password"
        '
        'mnuChangeOtherPassword
        '
        Me.mnuChangeOtherPassword.Name = "mnuChangeOtherPassword"
        Me.mnuChangeOtherPassword.Size = New System.Drawing.Size(237, 22)
        Me.mnuChangeOtherPassword.Text = "Change Other User Password"
        '
        'mnuDeleteUserAccount
        '
        Me.mnuDeleteUserAccount.Name = "mnuDeleteUserAccount"
        Me.mnuDeleteUserAccount.Size = New System.Drawing.Size(237, 22)
        Me.mnuDeleteUserAccount.Text = "Delete User Login"
        '
        'mnuRepairUserAccounts
        '
        Me.mnuRepairUserAccounts.Name = "mnuRepairUserAccounts"
        Me.mnuRepairUserAccounts.Size = New System.Drawing.Size(237, 22)
        Me.mnuRepairUserAccounts.Text = "Repair User Database"
        '
        'mnuWindows
        '
        Me.mnuWindows.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuWindows.Name = "mnuWindows"
        Me.mnuWindows.Size = New System.Drawing.Size(68, 20)
        Me.mnuWindows.Text = "Windows"
        '
        'mnuHelp
        '
        Me.mnuHelp.Name = "mnuHelp"
        Me.mnuHelp.Size = New System.Drawing.Size(44, 20)
        Me.mnuHelp.Text = "Help"
        '
        'CBMainForm
        '
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(1006, 693)
        Me.Controls.Add(Me.mnuMain)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Location = New System.Drawing.Point(9, 29)
        Me.Name = "CBMainForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Check Book"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.mnuMain.ResumeLayout(False)
        Me.mnuMain.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents mnuImportChecks As ToolStripMenuItem
    Friend WithEvents mnuImportBank As ToolStripMenuItem
    Friend WithEvents mnuImportDeposits As ToolStripMenuItem
    Friend WithEvents mnuImportInvoices As ToolStripMenuItem
    Friend WithEvents mnuUserAccounts As ToolStripMenuItem
    Friend WithEvents mnuEnableUserAccounts As ToolStripMenuItem
    Friend WithEvents mnuAddUserAccount As ToolStripMenuItem
    Friend WithEvents mnuChangeCurrentPassword As ToolStripMenuItem
    Friend WithEvents mnuChangeOtherPassword As ToolStripMenuItem
    Friend WithEvents mnuDeleteUserAccount As ToolStripMenuItem
    Friend WithEvents mnuRepairUserAccounts As ToolStripMenuItem
    Friend WithEvents mnuCheckFormat As ToolStripMenuItem
    Friend WithEvents mnuCompanyInformation As ToolStripMenuItem
    Friend WithEvents mnuHelp As ToolStripMenuItem
    Friend WithEvents mnuLicensing As ToolStripMenuItem
#End Region
End Class