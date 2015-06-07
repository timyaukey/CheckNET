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
	Public WithEvents mnuActRecon As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuActDepImport As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuActInvImport As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuBankImportOFX As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuActBankImportQIF As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuAct As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuActAdjBudget As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuActFindLiveBudget As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuActRepeatKeys As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuAccount As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuListPayees As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuListCategories As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuListBudgets As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuListTrxTypes As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuList As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuRptCategory As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuRptPayables As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuRpt As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents mnuWindows As System.Windows.Forms.ToolStripMenuItem
	Public WithEvents MainMenu1 As System.Windows.Forms.MenuStrip
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CBMainForm))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.MainMenu1 = New System.Windows.Forms.MenuStrip()
        Me.mnuFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileShowReg = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileSave = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuFileExit = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAct = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuActRecon = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuActDepImport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuActInvImport = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuBankImportOFX = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuActBankImportQIF = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAccount = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuActAdjBudget = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuActFindLiveBudget = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuActRepeatKeys = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuList = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListPayees = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListCategories = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListBudgets = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuListTrxTypes = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRpt = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRptCategory = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuRptPayables = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuWindows = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuActCompuPayImport = New System.Windows.Forms.ToolStripMenuItem()
        Me.MainMenu1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MainMenu1
        '
        Me.MainMenu1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFile, Me.mnuAct, Me.mnuAccount, Me.mnuList, Me.mnuRpt, Me.mnuWindows})
        Me.MainMenu1.Location = New System.Drawing.Point(0, 0)
        Me.MainMenu1.MdiWindowListItem = Me.mnuWindows
        Me.MainMenu1.Name = "MainMenu1"
        Me.MainMenu1.Size = New System.Drawing.Size(1006, 24)
        Me.MainMenu1.TabIndex = 1
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
        'mnuAct
        '
        Me.mnuAct.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuActRecon, Me.mnuActDepImport, Me.mnuActInvImport, Me.mnuBankImportOFX, Me.mnuActBankImportQIF, Me.mnuActCompuPayImport})
        Me.mnuAct.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuAct.Name = "mnuAct"
        Me.mnuAct.Size = New System.Drawing.Size(67, 20)
        Me.mnuAct.Text = "Activities"
        '
        'mnuActRecon
        '
        Me.mnuActRecon.Name = "mnuActRecon"
        Me.mnuActRecon.Size = New System.Drawing.Size(244, 22)
        Me.mnuActRecon.Text = "Reconcile"
        '
        'mnuActDepImport
        '
        Me.mnuActDepImport.Name = "mnuActDepImport"
        Me.mnuActDepImport.Size = New System.Drawing.Size(244, 22)
        Me.mnuActDepImport.Text = "Import Deposit Amounts"
        '
        'mnuActInvImport
        '
        Me.mnuActInvImport.Name = "mnuActInvImport"
        Me.mnuActInvImport.Size = New System.Drawing.Size(244, 22)
        Me.mnuActInvImport.Text = "Import Invoices"
        '
        'mnuBankImportOFX
        '
        Me.mnuBankImportOFX.Name = "mnuBankImportOFX"
        Me.mnuBankImportOFX.Size = New System.Drawing.Size(244, 22)
        Me.mnuBankImportOFX.Text = "Import OFX From Bank"
        '
        'mnuActBankImportQIF
        '
        Me.mnuActBankImportQIF.Name = "mnuActBankImportQIF"
        Me.mnuActBankImportQIF.Size = New System.Drawing.Size(244, 22)
        Me.mnuActBankImportQIF.Text = "Import QIF From Bank"
        '
        'mnuAccount
        '
        Me.mnuAccount.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuActAdjBudget, Me.mnuActFindLiveBudget, Me.mnuActRepeatKeys})
        Me.mnuAccount.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuAccount.Name = "mnuAccount"
        Me.mnuAccount.Size = New System.Drawing.Size(64, 20)
        Me.mnuAccount.Text = "Account"
        Me.mnuAccount.Visible = False
        '
        'mnuActAdjBudget
        '
        Me.mnuActAdjBudget.Name = "mnuActAdjBudget"
        Me.mnuActAdjBudget.Size = New System.Drawing.Size(223, 22)
        Me.mnuActAdjBudget.Text = "Adjust Budgets To Cashflow"
        '
        'mnuActFindLiveBudget
        '
        Me.mnuActFindLiveBudget.Name = "mnuActFindLiveBudget"
        Me.mnuActFindLiveBudget.Size = New System.Drawing.Size(223, 22)
        Me.mnuActFindLiveBudget.Text = "Find Live Budgets"
        '
        'mnuActRepeatKeys
        '
        Me.mnuActRepeatKeys.Name = "mnuActRepeatKeys"
        Me.mnuActRepeatKeys.Size = New System.Drawing.Size(223, 22)
        Me.mnuActRepeatKeys.Text = "Repeat Key List"
        Me.mnuActRepeatKeys.Visible = False
        '
        'mnuList
        '
        Me.mnuList.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuListPayees, Me.mnuListCategories, Me.mnuListBudgets, Me.mnuListTrxTypes})
        Me.mnuList.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuList.Name = "mnuList"
        Me.mnuList.Size = New System.Drawing.Size(42, 20)
        Me.mnuList.Text = "Lists"
        '
        'mnuListPayees
        '
        Me.mnuListPayees.Name = "mnuListPayees"
        Me.mnuListPayees.Size = New System.Drawing.Size(209, 22)
        Me.mnuListPayees.Text = "Memorized Transactions"
        '
        'mnuListCategories
        '
        Me.mnuListCategories.Name = "mnuListCategories"
        Me.mnuListCategories.Size = New System.Drawing.Size(209, 22)
        Me.mnuListCategories.Text = "Categories"
        '
        'mnuListBudgets
        '
        Me.mnuListBudgets.Name = "mnuListBudgets"
        Me.mnuListBudgets.Size = New System.Drawing.Size(209, 22)
        Me.mnuListBudgets.Text = "Budgets"
        '
        'mnuListTrxTypes
        '
        Me.mnuListTrxTypes.Name = "mnuListTrxTypes"
        Me.mnuListTrxTypes.Size = New System.Drawing.Size(209, 22)
        Me.mnuListTrxTypes.Text = "Transaction Import Types"
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
        Me.mnuRptCategory.Size = New System.Drawing.Size(173, 22)
        Me.mnuRptCategory.Text = "Totals By Category"
        '
        'mnuRptPayables
        '
        Me.mnuRptPayables.Name = "mnuRptPayables"
        Me.mnuRptPayables.Size = New System.Drawing.Size(173, 22)
        Me.mnuRptPayables.Text = "Accounts Payable"
        '
        'mnuWindows
        '
        Me.mnuWindows.MergeAction = System.Windows.Forms.MergeAction.Remove
        Me.mnuWindows.Name = "mnuWindows"
        Me.mnuWindows.Size = New System.Drawing.Size(68, 20)
        Me.mnuWindows.Text = "Windows"
        '
        'mnuActCompuPayImport
        '
        Me.mnuActCompuPayImport.Name = "mnuActCompuPayImport"
        Me.mnuActCompuPayImport.Size = New System.Drawing.Size(244, 22)
        Me.mnuActCompuPayImport.Text = "Import Checks From Compupay"
        '
        'CBMainForm
        '
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(1006, 693)
        Me.Controls.Add(Me.MainMenu1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.Location = New System.Drawing.Point(9, 29)
        Me.Name = "CBMainForm"
        Me.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Check Book"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MainMenu1.ResumeLayout(False)
        Me.MainMenu1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents mnuActCompuPayImport As System.Windows.Forms.ToolStripMenuItem
#End Region 
End Class