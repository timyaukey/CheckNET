Option Strict Off
Option Explicit On

Imports System.IO
Imports VB = Microsoft.VisualBasic
Imports CheckBookLib

Friend Class CBMainForm
    Inherits System.Windows.Forms.Form
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private frmStartup As StartupForm
    Private mblnCancelStart As Boolean
    Private WithEvents mobjEverything As Everything

    Private Sub CBMainForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim objAccount As Account
        Dim objLoaded As LoadedRegister
        Dim objReg As Register
        Dim astrFiles() As String = Nothing
        Dim intFiles As Short
        Dim vstrFile As Object
        Dim strSecurityOption As String = ""

        Try


            mblnCancelStart = True

            'If App.PrevInstance Then
            '    MsgBox "Only one copy of this program may run at a time.", vbCritical
            '    Exit Sub
            'End If

            mobjEverything = gobjInitialize()
            gcolAccounts = mobjEverything.colAccounts
            frmStartup = New StartupForm
            frmStartup.Show()
            frmStartup.ShowStatus("Initializing")
            'Look for locally recognized command line options
            Dim intIndex As Short
            Dim strArg As String
            For intIndex = LBound(gstrCmdLinArgs) To UBound(gstrCmdLinArgs)
                strArg = gstrCmdLinArgs(intIndex)
                If strArg = "/rptseq" Then
                    gblnAssignRepeatSeq = True
                    gstrCmdLinArgs(intIndex) = ""
                    MsgBox("Assigning repeat sequence numbers")
                End If
                If VB.Left(strArg, 10) = "/security:" Then
                    strSecurityOption = Mid(strArg, 11)
                    gstrCmdLinArgs(intIndex) = ""
                End If
                If strArg = "/help" Or strArg = "/?" Then
                    MsgBox("Args: //r:(data root folder)" & vbCrLf & "/security:createfile" & vbCrLf & "/security:createuser" & vbCrLf & "/security:setpassword" & vbCrLf & "/security:signfile")
                    frmStartup.Close()
                    Exit Sub
                End If
            Next

            If gblnDataIsLocked() Then
                MsgBox("Data files are in use by someone else running this software.")
                frmStartup.Close()
                Exit Sub
            End If

            If gblnUnrecognizedArgs() Then
                frmStartup.Close()
                Exit Sub
            End If

            If strSecurityOption <> "" Then
                gProcessSecurityOption(strSecurityOption)
                frmStartup.Close()
                Exit Sub
            End If

            gCreateStandardFolders()
            gCreateStandardFiles()
            gLoadGlobalLists()
            gLoadTransTable()

            If Not gblnUserAuthenticated() Then
                frmStartup.Close()
                Exit Sub
            End If

            'Find all ".act" files.
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vstrFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vstrFile = Dir(gstrAccountPath() & "\*.act")
            'UPGRADE_WARNING: Couldn't resolve default property of object vstrFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If vstrFile = "" Then
                MsgBox("Creating first checking account...", MsgBoxStyle.Information)
                gCreateAccount("Main", "Checking Account", "Main Register")
                'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vstrFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                vstrFile = Dir(gstrAccountPath() & "\*.act")
            End If
            'UPGRADE_WARNING: Couldn't resolve default property of object vstrFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            While vstrFile <> ""
                intFiles = intFiles + 1
                ReDim Preserve astrFiles(intFiles - 1)
                'UPGRADE_WARNING: Couldn't resolve default property of object vstrFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                astrFiles(intFiles - 1) = vstrFile
                'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vstrFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                vstrFile = Dir()
            End While
            'Load them all.
            For Each vstrFile In astrFiles
                objAccount = New Account
                objAccount.Init(mobjEverything)
                frmStartup.Configure(objAccount)
                'UPGRADE_WARNING: Couldn't resolve default property of object vstrFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                objAccount.Load(vstrFile)
                gcolAccounts.Add(objAccount)
                frmStartup.Configure(Nothing)
            Next vstrFile

            frmStartup.ShowStatus("Loading main window")

            Me.Text = "Willow Creek Checkbook " & My.Application.Info.Version.Major & "." & _
                My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build & _
                " [" & LCase(gobjSecurity.strLogin) & "] [" & LCase(gstrDataPathValue) & "]"

            mblnCancelStart = False

            For Each objAccount In gcolAccounts
                For Each objLoaded In objAccount.colLoadedRegisters
                    objReg = objLoaded.objReg
                    If objReg.blnShowInitially Then
                        gShowRegister(objAccount, objReg, frmStartup)
                    End If
                Next objLoaded
            Next objAccount

            frmStartup.Close()
            frmStartup = frmStartup

        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    'UPGRADE_WARNING: Form event CBMainForm.Activate has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
    Private Sub CBMainForm_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        If mblnCancelStart Then
            If Not frmStartup Is Nothing Then
                frmStartup.Close()
                'UPGRADE_NOTE: Object frmStartup may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                frmStartup = Nothing
            End If
            Me.Close()
        End If
    End Sub

    Private Sub CBMainForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        On Error GoTo ErrorHandler

        If Not mblnCancelStart Then
            gSaveChangedAccounts()
        End If
        mobjEverything.Teardown()

        Exit Sub
ErrorHandler:
        TopError("MDIForm_Unload")
    End Sub

    Public Sub mnuActBankImportQIF_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuActBankImportQIF.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxImport
        Dim strFile As String
        Dim objFile As TextReader

        On Error GoTo ErrorHandler

        If blnImportFormAlreadyOpen() Then
            Exit Sub
        End If

        strFile = CommonDialogControlForm.strChooseFile("Select Bank Download QIF File To Import", "QIF", "BankQIFPath")
        If strFile <> "" Then
            objFile = New StreamReader(strFile)
            frm = New BankImportAcctSelectForm
            objImport = New ImportBankDownloadQIF(objFile, strFile)
            frm.ShowMe("Import QIF File From Bank", objImport, _
                CBMain.ImportStatusSearch.Bank, _
                CBMain.ImportBatchUpdateSearch.glngIMPBATUPSR_BANK, _
                CBMain.ImportBatchNewSearch.glngIMPBATNWSR_BANK, _
                CBMain.ImportIndividualUpdateType.glngIMPINDUPTP_BANK, _
                CBMain.ImportIndividualSearchType.glngIMPINDSRTP_BANK, _
                CBMain.ImportBatchUpdateType.glngIMPBATUPTP_BANK, False)
        End If

        Exit Sub
ErrorHandler:
        TopError("mnuActBankImportQIF_Click")
    End Sub

    Public Sub mnuBankImportOFX_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuBankImportOFX.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxImport
        Dim strFile As String
        Dim objFile As TextReader

        On Error GoTo ErrorHandler

        If blnImportFormAlreadyOpen() Then
            Exit Sub
        End If

        strFile = CommonDialogControlForm.strChooseFile("Select Bank Download OFX File To Import", "OFX", "BankOFXPath")
        If strFile <> "" Then
            objFile = New StreamReader(strFile)
            frm = New BankImportAcctSelectForm
            objImport = New ImportBankDownloadOFX(objFile, strFile)
            frm.ShowMe("Import OFX File From Bank", objImport, _
                CBMain.ImportStatusSearch.Bank, _
                CBMain.ImportBatchUpdateSearch.glngIMPBATUPSR_BANK, _
                CBMain.ImportBatchNewSearch.glngIMPBATNWSR_BANK, _
                CBMain.ImportIndividualUpdateType.glngIMPINDUPTP_BANK, _
                CBMain.ImportIndividualSearchType.glngIMPINDSRTP_BANK, _
                CBMain.ImportBatchUpdateType.glngIMPBATUPTP_BANK, False)
        End If

        Exit Sub
ErrorHandler:
        TopError("mnuActBankImportOFX_Click")
    End Sub

    Public Sub mnuActDepImport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuActDepImport.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxImport

        On Error GoTo ErrorHandler

        If blnImportFormAlreadyOpen() Then
            Exit Sub
        End If
        frm = New BankImportAcctSelectForm
        objImport = New ImportByPayee(gobjClipboardReader(), "(clipboard)")
        frm.ShowMe("Import Deposit Amounts", objImport, _
            CBMain.ImportStatusSearch.PayeeNonGenerated, _
            CBMain.ImportBatchUpdateSearch.glngIMPBATUPSR_PAYEE, _
            CBMain.ImportBatchNewSearch.glngIMPBATNWSR_NONE, _
            CBMain.ImportIndividualUpdateType.glngIMPINDUPTP_AMOUNT, _
            CBMain.ImportIndividualSearchType.glngIMPINDSRTP_PAYEE, _
            CBMain.ImportBatchUpdateType.glngIMPBATUPTP_AMOUNT, False)

        Exit Sub
ErrorHandler:
        TopError("mnuActBankImport_Click")
    End Sub

    Public Sub mnuActInvImport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuActInvImport.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxImport

        On Error GoTo ErrorHandler

        If blnImportFormAlreadyOpen() Then
            Exit Sub
        End If
        frm = New BankImportAcctSelectForm
        objImport = New ImportInvoices(gobjClipboardReader(), "(clipboard)")
        frm.ShowMe("Import Invoices", objImport, _
            CBMain.ImportStatusSearch.VendorInvoice, _
            CBMain.ImportBatchUpdateSearch.glngIMPBATUPSR_NONE, _
            CBMain.ImportBatchNewSearch.glngIMPBATNWSR_VENINV, _
            CBMain.ImportIndividualUpdateType.glngIMPINDUPTP_NONE, _
            CBMain.ImportIndividualSearchType.glngIMPINDSRTP_VENINV, _
            CBMain.ImportBatchUpdateType.glngIMPBATUPTP_NONE, False)

        Exit Sub
ErrorHandler:
        TopError("mnuActInvImport_Click")
    End Sub

    Private Sub mnuActCheckImport_Click(sender As Object, e As EventArgs) Handles mnuActCheckImport.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxImport
        Dim objSpecs As ImportChecksSpec

        On Error GoTo ErrorHandler

        If blnImportFormAlreadyOpen() Then
            Exit Sub
        End If
        frm = New BankImportAcctSelectForm

        objSpecs = New ImportChecksSpec(1, 0, 2, 3, -1)
        objImport = New ImportChecks(gobjClipboardReader(), "(clipboard)", objSpecs)
        frm.ShowMe("Import Checks", objImport, _
            CBMain.ImportStatusSearch.BillPayment, _
            CBMain.ImportBatchUpdateSearch.glngIMPBATUPSR_BANK, _
            CBMain.ImportBatchNewSearch.glngIMPBATNWSR_BANK, _
            CBMain.ImportIndividualUpdateType.glntIMPINDUPTP_NUMAMT, _
            CBMain.ImportIndividualSearchType.glngIMPINDSRTP_BANK, _
            CBMain.ImportBatchUpdateType.glngIMPBATUPTP_NUMAMT, False)

        Exit Sub
ErrorHandler:
        TopError("mnuActCheckImport_Click")
    End Sub

    Private Sub mnuActCompuPayImport_Click(sender As Object, e As EventArgs) Handles mnuActCompuPayImport.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxImport
        Dim objSpecs As ImportChecksSpec

        On Error GoTo ErrorHandler

        If blnImportFormAlreadyOpen() Then
            Exit Sub
        End If
        frm = New BankImportAcctSelectForm

        objSpecs = New ImportChecksSpec(0, 5, 9, 12, -1)
        objImport = New ImportChecks(gobjClipboardReader(), "(clipboard)", objSpecs)
        frm.ShowMe("Import CompuPay Checks", objImport, _
            CBMain.ImportStatusSearch.BillPayment, _
            CBMain.ImportBatchUpdateSearch.glngIMPBATUPSR_BANK, _
            CBMain.ImportBatchNewSearch.glngIMPBATNWSR_BANK, _
            CBMain.ImportIndividualUpdateType.glntIMPINDUPTP_NUMAMT, _
            CBMain.ImportIndividualSearchType.glngIMPINDSRTP_BANK, _
            CBMain.ImportBatchUpdateType.glngIMPBATUPTP_NUMAMT, False)

        Exit Sub
ErrorHandler:
        TopError("mnuActCompuPayImport_Click")
    End Sub

    Private Sub mnuActOSUImport_Click(sender As Object, e As EventArgs) Handles mnuActOSUImport.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxImport
        Dim objSpecs As ImportChecksSpec

        On Error GoTo ErrorHandler

        If blnImportFormAlreadyOpen() Then
            Exit Sub
        End If
        frm = New BankImportAcctSelectForm

        objSpecs = New ImportChecksSpec(6, 0, 1, 2, -1)
        objImport = New ImportChecks(gobjClipboardReader(), "(clipboard)", objSpecs)
        frm.ShowMe("Import Oregon State Credit Union Checks", objImport, _
            CBMain.ImportStatusSearch.BillPayment, _
            CBMain.ImportBatchUpdateSearch.glngIMPBATUPSR_BANK, _
            CBMain.ImportBatchNewSearch.glngIMPBATNWSR_BANK, _
            CBMain.ImportIndividualUpdateType.glntIMPINDUPTP_NUMAMT, _
            CBMain.ImportIndividualSearchType.glngIMPINDSRTP_BANK, _
            CBMain.ImportBatchUpdateType.glngIMPBATUPTP_NUMAMT, False)

        Exit Sub
ErrorHandler:
        TopError("mnuActOSUImport_Click")
    End Sub

    Private Function blnImportFormAlreadyOpen() As Boolean
        Dim frm As System.Windows.Forms.Form

        For Each frm In gcolForms()
            'UPGRADE_WARNING: TypeOf has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If TypeOf frm Is BankImportForm Then
                MsgBox("An import form is already open, and only one can be open at a time.")
                blnImportFormAlreadyOpen = True
                Exit Function
            End If
        Next frm
        blnImportFormAlreadyOpen = False
    End Function

    Public Sub mnuActAdjBudget_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuActAdjBudget.Click
        Dim frmReg As RegisterForm
        Dim frmAdjust As AdjustBudgetsToCashForm

        On Error GoTo ErrorHandler

        If Not TypeOf Me.ActiveMdiChild Is RegisterForm Then
            MsgBox("Please click on the register window you wish to adjust.", MsgBoxStyle.Critical)
            Exit Sub
        End If
        frmReg = Me.ActiveMdiChild
        If frmReg.objReg.blnRepeat Then
            MsgBox("Not available for repeating transaction registers.", MsgBoxStyle.Critical)
            Exit Sub
        End If
        frmAdjust = New AdjustBudgetsToCashForm
        frmAdjust.ShowModal(frmReg.objReg, gobjBudgets)

        Exit Sub
ErrorHandler:
        TopError("mnuActAdjBudget_Click")
    End Sub

    Public Sub mnuActFindLiveBudget_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuActFindLiveBudget.Click
        Dim frmReg As RegisterForm
        Dim frmLive As LiveBudgetListForm

        On Error GoTo ErrorHandler

        If Not TypeOf Me.ActiveMdiChild Is RegisterForm Then
            MsgBox("Please click on the register window you wish to search for live budgets.", MsgBoxStyle.Critical)
            Exit Sub
        End If
        frmReg = Me.ActiveMdiChild
        frmLive = New LiveBudgetListForm
        frmLive.ShowModal(frmReg.objReg, gobjBudgets)

        Exit Sub
ErrorHandler:
        TopError("mnuActAdjBudget_Click")
    End Sub

    Public Sub mnuActRepeatKeys_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuActRepeatKeys.Click
        Dim frmReg As RegisterForm
        Dim objAccount As Account
        Dim frmList As ListEditorForm

        On Error GoTo ErrorHandler

        'If Not TypeOf Me.ActiveMdiChild Is RegisterForm Then
        '    MsgBox("Please click on a register window for the account you wish edit repeat keys for.", MsgBoxStyle.Critical)
        '    Exit Sub
        'End If
        'frmReg = Me.ActiveMdiChild
        'objAccount = frmReg.objAccount
        'frmList = New ListEditorForm
        'With objAccount
        '    frmList.ShowMe(ListEditorForm.ListType.glngLIST_TYPE_REPEAT, .strRepeatsFile, .objRepeats, "Repeated Transaction Keys", objAccount)
        'End With

        Exit Sub
ErrorHandler:
        TopError("mnuActAdjBudget_Click")
    End Sub

    Public Sub mnuListBudgets_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListBudgets.Click
        Dim frm As ListEditorForm

        On Error GoTo ErrorHandler

        frm = New ListEditorForm
        frm.ShowMe(ListEditorForm.ListType.glngLIST_TYPE_BUDGET, gstrAddPath("Shared.bud"), gobjBudgets, "Budget List", Nothing)

        Exit Sub
ErrorHandler:
        TopError("mnuListBudgets_Click")
    End Sub

    Public Sub mnuListCategories_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListCategories.Click
        Dim frm As ListEditorForm

        On Error GoTo ErrorHandler

        frm = New ListEditorForm
        frm.ShowMe(ListEditorForm.ListType.glngLIST_TYPE_CATEGORY, gstrAddPath("Shared.cat"), gobjCategories, "Category List", Nothing)

        Exit Sub
ErrorHandler:
        TopError("mnuListCategories_Click")
    End Sub

    Public Sub mnuListPayees_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListPayees.Click
        Dim frm As PayeeListForm

        On Error GoTo ErrorHandler

        frm = New PayeeListForm
        frm.ShowMe()

        Exit Sub
ErrorHandler:
        TopError("mnuListPayees_Click")
    End Sub

    Public Sub mnuListTrxTypes_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListTrxTypes.Click
        Dim frm As TrxTypeListForm

        On Error GoTo ErrorHandler

        frm = New TrxTypeListForm
        frm.ShowMe()

        Exit Sub
ErrorHandler:
        TopError("mnuListTrxTypes_Click")
    End Sub

    Public Sub mnuActRecon_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuActRecon.Click
        Dim frm As ReconAcctSelectForm

        On Error GoTo ErrorHandler

        frm = New ReconAcctSelectForm
        frm.ShowDialog()

        Exit Sub
ErrorHandler:
        TopError("mnuActRecon_Click")
    End Sub

    Public Sub mnuFileExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileExit.Click
        Me.Close()
    End Sub

    Public Sub mnuFileSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileSave.Click
        On Error GoTo ErrorHandler

        gSaveChangedAccounts()
        mnuFileSave.Enabled = False

        Exit Sub
ErrorHandler:
        TopError("mnuFileSave_Click")
    End Sub

    Private Sub TopError(ByVal strRoutine As String)
        gTopErrorTrap("CBMainForm." & strRoutine)
    End Sub

    Public Sub mnuFileShowReg_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileShowReg.Click
        Dim frm As ShowRegisterForm
        Dim frm2 As System.Windows.Forms.Form

        On Error GoTo ErrorHandler

        For Each frm2 In gcolForms()
            'UPGRADE_WARNING: TypeOf has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If TypeOf frm2 Is ShowRegisterForm Then
                frm2.Activate()
                Exit Sub
            End If
        Next frm2

        frm = New ShowRegisterForm
        frm.Show()

        Exit Sub
ErrorHandler:
        TopError("mnuFileShowReg_Click")
    End Sub

    Public Sub mnuRptCategory_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuRptCategory.Click
        Dim frmRpt As RptScanSplitsForm

        On Error GoTo ErrorHandler

        frmRpt = New RptScanSplitsForm
        frmRpt.ShowMe(RptScanSplitsForm.SplitReportType.glngSPLTRPT_TOTALS)

        Exit Sub
ErrorHandler:
        TopError("mnuRptCategory_Click")
    End Sub

    Public Sub mnuRptPayables_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuRptPayables.Click
        Dim frmRpt As RptScanSplitsForm

        On Error GoTo ErrorHandler

        frmRpt = New RptScanSplitsForm
        frmRpt.ShowMe(RptScanSplitsForm.SplitReportType.glngSPLTRPT_PAYABLES)

        Exit Sub
ErrorHandler:
        TopError("mnuRptCategory_Click")
    End Sub

    Private Sub mobjEverything_SomethingModified() Handles mobjEverything.SomethingModified
        mnuFileSave.Enabled = True
    End Sub
End Class