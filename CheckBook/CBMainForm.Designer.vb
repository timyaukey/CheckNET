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
	Public WithEvents mnuFileShowReg As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileSave As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFileExit As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuFile As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuImport As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuActAdjBudget As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuActFindLiveBudget As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuTools As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuListPayees As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuListCategories As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuListBudgets As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuListTrxTypes As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuSetup As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRptCategory As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents mnuRptPayables As System.Windows.Forms.ToolStripMenuItem
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
        Me.mnuFileShowReg = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuImport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuImportBank = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuImportChecks = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuImportDeposits = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuImportDepositsStandard = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuImportInvoices = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuImportInvoicesStandard = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTools = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuActRecon = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuActAdjBudget = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuActFindLiveBudget = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRpt = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRptCategory = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRptPayables = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSetup = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListPayees = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListCategories = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListBudgets = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListTrxTypes = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuWindows = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'mnuMain
        '
        Me.mnuMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuImport, Me.mnuTools, Me.mnuRpt, Me.mnuSetup, Me.mnuWindows})
        Me.mnuMain.Location = New System.Drawing.Point(0, 0)
        Me.mnuMain.MdiWindowListItem = Me.mnuWindows
        Me.mnuMain.Name = "mnuMain"
        Me.mnuMain.Size = New System.Drawing.Size(1006, 24)
        Me.mnuMain.TabIndex = 1
        '
        'mnuFile
        '
        Me.mnuFile.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFileShowReg, Me.mnuFileSave, Me.mnuFileExit})
        Me.mnuFile.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuFile.Name = "mnuFile"
        Me.mnuFile.Size = New System.Drawing.Size(37, 20)
        Me.mnuFile.Text = "File"
        '
        'mnuFileShowReg
        '
        Me.mnuFileShowReg.Name = "mnuFileShowReg"
        Me.mnuFileShowReg.Size = New System.Drawing.Size(197, 22)
        Me.mnuFileShowReg.Text = "Registers and Accounts"
        '
        'mnuFileSave
        '
        Me.mnuFileSave.Enabled = False
        Me.mnuFileSave.Name = "mnuFileSave"
        Me.mnuFileSave.Size = New System.Drawing.Size(197, 22)
        Me.mnuFileSave.Text = "Save"
        '
        'mnuFileExit
        '
        Me.mnuFileExit.Name = "mnuFileExit"
        Me.mnuFileExit.Size = New System.Drawing.Size(197, 22)
        Me.mnuFileExit.Text = "Exit"
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
        Me.mnuImportDeposits.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuImportDepositsStandard})
        Me.mnuImportDeposits.Name = "mnuImportDeposits"
        Me.mnuImportDeposits.Size = New System.Drawing.Size(157, 22)
        Me.mnuImportDeposits.Text = "Deposits"
        '
        'mnuImportDepositsStandard
        '
        Me.mnuImportDepositsStandard.Name = "mnuImportDepositsStandard"
        Me.mnuImportDepositsStandard.Size = New System.Drawing.Size(176, 22)
        Me.mnuImportDepositsStandard.Text = "Standard Clipboard"
        '
        'mnuImportInvoices
        '
        Me.mnuImportInvoices.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuImportInvoicesStandard})
        Me.mnuImportInvoices.Name = "mnuImportInvoices"
        Me.mnuImportInvoices.Size = New System.Drawing.Size(157, 22)
        Me.mnuImportInvoices.Text = "Invoices"
        '
        'mnuImportInvoicesStandard
        '
        Me.mnuImportInvoicesStandard.Name = "mnuImportInvoicesStandard"
        Me.mnuImportInvoicesStandard.Size = New System.Drawing.Size(176, 22)
        Me.mnuImportInvoicesStandard.Text = "Standard Clipboard"
        '
        'mnuTools
        '
        Me.mnuTools.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuActRecon, Me.mnuActAdjBudget, Me.mnuActFindLiveBudget})
        Me.mnuTools.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuTools.Name = "mnuTools"
        Me.mnuTools.Size = New System.Drawing.Size(47, 20)
        Me.mnuTools.Text = "Tools"
        '
        'mnuActRecon
        '
        Me.mnuActRecon.Name = "mnuActRecon"
        Me.mnuActRecon.Size = New System.Drawing.Size(222, 22)
        Me.mnuActRecon.Text = "Reconcile"
        '
        'mnuActAdjBudget
        '
        Me.mnuActAdjBudget.Name = "mnuActAdjBudget"
        Me.mnuActAdjBudget.Size = New System.Drawing.Size(222, 22)
        Me.mnuActAdjBudget.Text = "Adjust Budgets To Cashflow"
        '
        'mnuActFindLiveBudget
        '
        Me.mnuActFindLiveBudget.Name = "mnuActFindLiveBudget"
        Me.mnuActFindLiveBudget.Size = New System.Drawing.Size(222, 22)
        Me.mnuActFindLiveBudget.Text = "Find Live Budgets"
        '
        'mnuRpt
        '
        Me.mnuRpt.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRptCategory, Me.mnuRptPayables})
        Me.mnuRpt.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuRpt.Name = "mnuRpt"
        Me.mnuRpt.Size = New System.Drawing.Size(59, 20)
        Me.mnuRpt.Text = "Reports"
        '
        'mnuRptCategory
        '
        Me.mnuRptCategory.Name = "mnuRptCategory"
        Me.mnuRptCategory.Size = New System.Drawing.Size(172, 22)
        Me.mnuRptCategory.Text = "Totals By Category"
        '
        'mnuRptPayables
        '
        Me.mnuRptPayables.Name = "mnuRptPayables"
        Me.mnuRptPayables.Size = New System.Drawing.Size(172, 22)
        Me.mnuRptPayables.Text = "Accounts Payable"
        '
        'mnuSetup
        '
        Me.mnuSetup.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuListPayees, Me.mnuListCategories, Me.mnuListBudgets, Me.mnuListTrxTypes})
        Me.mnuSetup.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuSetup.Name = "mnuSetup"
        Me.mnuSetup.Size = New System.Drawing.Size(49, 20)
        Me.mnuSetup.Text = "Setup"
        '
        'mnuListPayees
        '
        Me.mnuListPayees.Name = "mnuListPayees"
        Me.mnuListPayees.Size = New System.Drawing.Size(207, 22)
        Me.mnuListPayees.Text = "Memorized Transactions"
        '
        'mnuListCategories
        '
        Me.mnuListCategories.Name = "mnuListCategories"
        Me.mnuListCategories.Size = New System.Drawing.Size(207, 22)
        Me.mnuListCategories.Text = "Categories"
        '
        'mnuListBudgets
        '
        Me.mnuListBudgets.Name = "mnuListBudgets"
        Me.mnuListBudgets.Size = New System.Drawing.Size(207, 22)
        Me.mnuListBudgets.Text = "Budgets"
        '
        'mnuListTrxTypes
        '
        Me.mnuListTrxTypes.Name = "mnuListTrxTypes"
        Me.mnuListTrxTypes.Size = New System.Drawing.Size(207, 22)
        Me.mnuListTrxTypes.Text = "Transaction Import Types"
        '
        'mnuWindows
        '
        Me.mnuWindows.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuWindows.Name = "mnuWindows"
        Me.mnuWindows.Size = New System.Drawing.Size(68, 20)
        Me.mnuWindows.Text = "Windows"
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
    Public WithEvents mnuActRecon As ToolStripMenuItem
    Friend WithEvents mnuImportChecks As ToolStripMenuItem
    Friend WithEvents mnuImportBank As ToolStripMenuItem
    Friend WithEvents mnuImportDeposits As ToolStripMenuItem
    Friend WithEvents mnuImportDepositsStandard As ToolStripMenuItem
    Friend WithEvents mnuImportInvoices As ToolStripMenuItem
    Friend WithEvents mnuImportInvoicesStandard As ToolStripMenuItem
#End Region
End Class