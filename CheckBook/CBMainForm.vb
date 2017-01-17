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
        Dim objReg As Register
        Dim astrFiles() As String = Nothing
        Dim intFiles As Short
        Dim vstrFile As Object
        Dim strSecurityOption As String = ""

        Try
            mblnCancelStart = True

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

            Me.Text = "Willow Creek Checkbook " & My.Application.Info.Version.Major & "." & _
                My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build & _
                " [" & LCase(gstrDataPathValue) & "]"

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
            vstrFile = Dir(gstrAccountPath() & "\*.act")
            If vstrFile = "" Then
                MsgBox("Creating first checking account...", MsgBoxStyle.Information)
                gCreateAccount("Main", "Checking Account", "Main Register")
                vstrFile = Dir(gstrAccountPath() & "\*.act")
            End If
            While vstrFile <> ""
                intFiles = intFiles + 1
                ReDim Preserve astrFiles(intFiles - 1)
                astrFiles(intFiles - 1) = vstrFile
                vstrFile = Dir()
            End While
            'Load them all.
            For Each vstrFile In astrFiles
                objAccount = New Account
                objAccount.Init(mobjEverything)
                frmStartup.Configure(objAccount)
                objAccount.Load(vstrFile)
                gcolAccounts.Add(objAccount)
                frmStartup.Configure(Nothing)
            Next vstrFile

            frmStartup.ShowStatus("Loading main window")

            mblnCancelStart = False

            For Each objAccount In gcolAccounts
                For Each objReg In objAccount.colRegisters
                    If objReg.blnShowInitially Then
                        gShowRegister(objAccount, objReg, frmStartup)
                    End If
                Next objReg
            Next objAccount

            frmStartup.Close()
            frmStartup = frmStartup

        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

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
        Try

            If Not mblnCancelStart Then
                gSaveChangedAccounts()
            End If
            mobjEverything.Teardown()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuActBankImportQIF_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuActBankImportQIF.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxReader
        Dim strFile As String
        Dim objFile As TextReader

        Try

            If blnImportFormAlreadyOpen() Then
                Exit Sub
            End If

            strFile = CommonDialogControlForm.strChooseFile("Select Bank Download QIF File To Import", "QIF", "BankQIFPath")
            If strFile <> "" Then
                objFile = New StreamReader(strFile)
                frm = New BankImportAcctSelectForm
                objImport = New ImportBankDownloadQIF(objFile, strFile)
                frm.ShowMe("Import QIF File From Bank", New ImportHandlerBank(), objImport,
                    CBMain.ImportBatchUpdateSearch.Bank,
                    CBMain.ImportIndividualUpdateType.Bank,
                    CBMain.ImportIndividualSearchType.Bank,
                    CBMain.ImportBatchUpdateType.Bank, False)
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuBankImportOFX_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuBankImportOFX.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxReader
        Dim strFile As String
        Dim objFile As TextReader

        Try

            If blnImportFormAlreadyOpen() Then
                Exit Sub
            End If

            strFile = CommonDialogControlForm.strChooseFile("Select Bank Download OFX File To Import", "OFX", "BankOFXPath")
            If strFile <> "" Then
                objFile = New StreamReader(strFile)
                frm = New BankImportAcctSelectForm
                objImport = New ImportBankDownloadOFX(objFile, strFile)
                frm.ShowMe("Import OFX File From Bank", New ImportHandlerBank(), objImport,
                    CBMain.ImportBatchUpdateSearch.Bank,
                    CBMain.ImportIndividualUpdateType.Bank,
                    CBMain.ImportIndividualSearchType.Bank,
                    CBMain.ImportBatchUpdateType.Bank, False)
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuActDepImport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuActDepImport.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxReader

        Try

            If blnImportFormAlreadyOpen() Then
                Exit Sub
            End If
            frm = New BankImportAcctSelectForm
            objImport = New ImportByPayee(gobjClipboardReader(), "(clipboard)")
            frm.ShowMe("Import Deposit Amounts", New ImportHandlerDeposits(), objImport,
                CBMain.ImportBatchUpdateSearch.Payee,
                CBMain.ImportIndividualUpdateType.Amount,
                CBMain.ImportIndividualSearchType.Payee,
                CBMain.ImportBatchUpdateType.Amount, False)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuActInvImport_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuActInvImport.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxReader

        Try

            If blnImportFormAlreadyOpen() Then
                Exit Sub
            End If
            frm = New BankImportAcctSelectForm
            objImport = New ImportInvoices(gobjClipboardReader(), "(clipboard)")
            frm.ShowMe("Import Invoices", New ImportHandlerInvoices(), objImport,
                CBMain.ImportBatchUpdateSearch.None,
                CBMain.ImportIndividualUpdateType.None,
                CBMain.ImportIndividualSearchType.VendorInvoice,
                CBMain.ImportBatchUpdateType.None, False)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mnuActCheckImport_Click(sender As Object, e As EventArgs) Handles mnuActCheckImport.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxReader
        Dim objSpecs As ImportChecksSpec

        Try

            If blnImportFormAlreadyOpen() Then
                Exit Sub
            End If
            frm = New BankImportAcctSelectForm

            objSpecs = New ImportChecksSpec(1, 0, 2, 3, -1)
            objImport = New ImportChecks(gobjClipboardReader(), "(clipboard)", objSpecs)
            frm.ShowMe("Import Checks", New ImportHandlerChecks(), objImport,
                CBMain.ImportBatchUpdateSearch.Bank,
                CBMain.ImportIndividualUpdateType.NumberAmount,
                CBMain.ImportIndividualSearchType.Bank,
                CBMain.ImportBatchUpdateType.NumberAmount, False)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mnuActCompuPayImport_Click(sender As Object, e As EventArgs) Handles mnuActCompuPayImport.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxReader
        Dim objSpecs As ImportChecksSpec

        Try

            If blnImportFormAlreadyOpen() Then
                Exit Sub
            End If
            frm = New BankImportAcctSelectForm

            objSpecs = New ImportChecksSpec(0, 5, 9, 12, -1)
            objImport = New ImportChecks(gobjClipboardReader(), "(clipboard)", objSpecs)
            frm.ShowMe("Import CompuPay Checks", New ImportHandlerChecks(), objImport,
                CBMain.ImportBatchUpdateSearch.Bank,
                CBMain.ImportIndividualUpdateType.NumberAmount,
                CBMain.ImportIndividualSearchType.Bank,
                CBMain.ImportBatchUpdateType.NumberAmount, False)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mnuActOSUImport_Click(sender As Object, e As EventArgs) Handles mnuActOSUImport.Click
        Dim frm As BankImportAcctSelectForm
        Dim objImport As ITrxReader
        Dim objSpecs As ImportChecksSpec

        Try

            If blnImportFormAlreadyOpen() Then
                Exit Sub
            End If
            frm = New BankImportAcctSelectForm

            objSpecs = New ImportChecksSpec(6, 0, 1, 2, -1)
            objImport = New ImportChecks(gobjClipboardReader(), "(clipboard)", objSpecs)
            frm.ShowMe("Import Oregon State Credit Union Checks", New ImportHandlerChecks(), objImport,
                CBMain.ImportBatchUpdateSearch.Bank,
                CBMain.ImportIndividualUpdateType.NumberAmount,
                CBMain.ImportIndividualSearchType.Bank,
                CBMain.ImportBatchUpdateType.NumberAmount, False)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function blnImportFormAlreadyOpen() As Boolean
        Dim frm As System.Windows.Forms.Form

        For Each frm In gcolForms()
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

        Try

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
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuActFindLiveBudget_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuActFindLiveBudget.Click
        Dim frmReg As RegisterForm
        Dim frmLive As LiveBudgetListForm

        Try

            If Not TypeOf Me.ActiveMdiChild Is RegisterForm Then
                MsgBox("Please click on the register window you wish to search for live budgets.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            frmReg = Me.ActiveMdiChild
            frmLive = New LiveBudgetListForm
            frmLive.ShowModal(frmReg.objReg, gobjBudgets)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuActRepeatKeys_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuActRepeatKeys.Click
        'Dim frmReg As RegisterForm
        'Dim objAccount As Account
        'Dim frmList As ListEditorForm

        Try

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
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuListBudgets_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListBudgets.Click
        Dim frm As ListEditorForm

        Try

            frm = New ListEditorForm
            frm.ShowMe(ListEditorForm.ListType.glngLIST_TYPE_BUDGET, gstrAddPath("Shared.bud"), gobjBudgets, "Budget List", Nothing)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuListCategories_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListCategories.Click
        Dim frm As ListEditorForm

        Try

            frm = New ListEditorForm
            frm.ShowMe(ListEditorForm.ListType.glngLIST_TYPE_CATEGORY, gstrAddPath("Shared.cat"), gobjCategories, "Category List", Nothing)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuListPayees_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListPayees.Click
        Dim frm As PayeeListForm

        Try

            frm = New PayeeListForm
            frm.ShowMe()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuListTrxTypes_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListTrxTypes.Click
        Dim frm As TrxTypeListForm

        Try

            frm = New TrxTypeListForm
            frm.ShowMe()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuActRecon_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuActRecon.Click
        Dim frm As ReconAcctSelectForm

        Try

            frm = New ReconAcctSelectForm
            frm.ShowDialog()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuFileExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileExit.Click
        Me.Close()
    End Sub

    Public Sub mnuFileSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileSave.Click
        Try

            gSaveChangedAccounts()
            mnuFileSave.Enabled = False

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuFileShowReg_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileShowReg.Click
        Dim frm As ShowRegisterForm
        Dim frm2 As System.Windows.Forms.Form

        Try

            For Each frm2 In gcolForms()
                If TypeOf frm2 Is ShowRegisterForm Then
                    frm2.Activate()
                    Exit Sub
                End If
            Next frm2

            frm = New ShowRegisterForm
            frm.Show()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuRptCategory_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuRptCategory.Click
        Dim frmRpt As RptScanSplitsForm

        Try

            frmRpt = New RptScanSplitsForm
            frmRpt.ShowMe(RptScanSplitsForm.SplitReportType.glngSPLTRPT_TOTALS)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuRptPayables_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuRptPayables.Click
        Dim frmRpt As RptScanSplitsForm

        Try

            frmRpt = New RptScanSplitsForm
            frmRpt.ShowMe(RptScanSplitsForm.SplitReportType.glngSPLTRPT_PAYABLES)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mobjEverything_SomethingModified() Handles mobjEverything.SomethingModified
        mnuFileSave.Enabled = True
    End Sub
End Class