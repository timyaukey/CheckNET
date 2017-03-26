Option Strict On
Option Explicit On

Imports System.IO
Imports System.Reflection
Imports VB = Microsoft.VisualBasic
Imports CheckBookLib

Friend Class CBMainForm
    Inherits System.Windows.Forms.Form
    Implements IHostUI

    Private frmStartup As StartupForm
    Private mblnCancelStart As Boolean
    Private WithEvents mobjCompany As Company

    Private Sub CBMainForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim objAccount As Account
        Dim objReg As Register
        Dim astrFiles() As String = Nothing
        Dim intFiles As Integer
        Dim strFile As String
        Dim strSecurityOption As String = ""

        Try
            mblnCancelStart = True

            mobjCompany = gobjInitialize()
            frmStartup = New StartupForm
            frmStartup.Show()
            frmStartup.ShowStatus("Initializing")

            LoadPlugins()

            'Look for locally recognized command line options
            Dim intIndex As Integer
            Dim strArg As String
            For intIndex = LBound(gstrCmdLinArgs) To UBound(gstrCmdLinArgs)
                strArg = gstrCmdLinArgs(intIndex)
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

            Me.Text = "Willow Creek Checkbook " & My.Application.Info.Version.Major & "." &
                My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build &
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
            mobjCompany.LoadGlobalLists()
            gLoadTransTable()

            If Not gblnUserAuthenticated() Then
                frmStartup.Close()
                Exit Sub
            End If

            'Find all ".act" files.
            strFile = Dir(gstrAccountPath() & "\*.act")
            If strFile = "" Then
                MsgBox("Creating first checking account...", MsgBoxStyle.Information)
                gCreateAccount("Main", "Checking Account", "Main Register", mobjCompany.intGetUnusedAccountKey(), Account.AccountType.Asset)
                strFile = Dir(gstrAccountPath() & "\*.act")
            End If
            While strFile <> ""
                intFiles = intFiles + 1
                ReDim Preserve astrFiles(intFiles - 1)
                astrFiles(intFiles - 1) = strFile
                strFile = Dir()
            End While

            'Load real trx, and non-generated fake trx, for all of them.
            For Each strFile In astrFiles
                objAccount = New Account
                objAccount.Init(mobjCompany)
                frmStartup.Configure(objAccount)
                objAccount.LoadStart(strFile)
                mobjCompany.colAccounts.Add(objAccount)
                frmStartup.Configure(Nothing)
            Next strFile

            'With all Account objects loaded we can add them to the category list.
            mobjCompany.LoadCategories()

            'Load generated transactions for all of them.
            For Each objAccount In mobjCompany.colAccounts
                frmStartup.Configure(objAccount)
                objAccount.LoadGenerated()
                frmStartup.Configure(Nothing)
            Next

            'Call Trx.Apply() for all Trx loaded above.
            'This will create ReplicaTrx.
            For Each objAccount In mobjCompany.colAccounts
                frmStartup.Configure(objAccount)
                objAccount.LoadApply()
                frmStartup.Configure(Nothing)
            Next

            'Perform final steps after all Trx exist, including computing running balances.
            For Each objAccount In mobjCompany.colAccounts
                frmStartup.Configure(objAccount)
                objAccount.LoadFinish()
                frmStartup.Configure(Nothing)
            Next

            frmStartup.ShowStatus("Loading main window")

            mblnCancelStart = False

            For Each objAccount In mobjCompany.colAccounts
                frmStartup.Configure(objAccount)
                For Each objReg In objAccount.colRegisters
                    If objReg.blnShowInitially Then
                        gShowRegister(objAccount, objReg, frmStartup)
                    End If
                Next
                frmStartup.Configure(Nothing)
            Next

            frmStartup.Close()
            frmStartup = frmStartup

        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private colBankImportPlugins As List(Of ToolPlugin) = New List(Of ToolPlugin)
    Private colCheckImportPlugins As List(Of ToolPlugin) = New List(Of ToolPlugin)
    Private colDepositImportPlugins As List(Of ToolPlugin) = New List(Of ToolPlugin)
    Private colInvoiceImportPlugins As List(Of ToolPlugin) = New List(Of ToolPlugin)
    Private colToolPlugins As List(Of ToolPlugin) = New List(Of ToolPlugin)
    Private colReportPlugins As List(Of ToolPlugin) = New List(Of ToolPlugin)

    Private Sub LoadPlugins()
        LoadPluginsFromAssembly(Assembly.GetEntryAssembly())
        Dim strEntryAssembly As String = Assembly.GetEntryAssembly().Location
        Dim strEntryFolder As String = Path.GetDirectoryName(strEntryAssembly)
        For Each strFile As String In Directory.EnumerateFiles(strEntryFolder, "*.dll")
            Dim assembly As Assembly = Assembly.LoadFrom(strFile)
            LoadPluginsFromAssembly(assembly)
        Next
        AddPluginsToMenu(mnuImportBank, colBankImportPlugins)
        AddPluginsToMenu(mnuImportChecks, colCheckImportPlugins)
        AddPluginsToMenu(mnuImportDeposits, colDepositImportPlugins)
        AddPluginsToMenu(mnuImportInvoices, colInvoiceImportPlugins)
        AddPluginsToMenu(mnuTools, colToolPlugins)
        AddPluginsToMenu(mnuRpt, colReportPlugins)
    End Sub

    Private Sub LoadPluginsFromAssembly(ByVal objAssembly As Assembly)
        For Each objAttrib As Object In objAssembly.GetCustomAttributes(GetType(PluginFactoryAttribute), False)
            Dim objPluginFactoryAttr As PluginFactoryAttribute = CType(objAttrib, PluginFactoryAttribute)
            Dim objFactoryType As Type = objPluginFactoryAttr.objFactoryType
            Dim objFactory As IPluginFactory = DirectCast(System.Activator.CreateInstance(objFactoryType), IPluginFactory)
            LoadPluginsFromFactory(objFactory)
        Next
    End Sub

    Private Sub LoadPluginsFromFactory(ByVal objFactory As IPluginFactory)
        For Each objPlugin As ToolPlugin In objFactory.colGetPlugins(Me)
            If TypeOf objPlugin Is BankImportPlugin Then
                colBankImportPlugins.Add(objPlugin)
            ElseIf TypeOf objPlugin Is CheckImportPlugin Then
                colCheckImportPlugins.Add(objPlugin)
            ElseIf TypeOf objPlugin Is DepositImportPlugin Then
                colDepositImportPlugins.Add(objPlugin)
            ElseIf TypeOf objPlugin Is InvoiceImportPlugin Then
                colInvoiceImportPlugins.Add(objPlugin)
            ElseIf TypeOf objPlugin Is ReportPlugin Then
                colReportPlugins.Add(objPlugin)
            Else
                colToolPlugins.Add(objPlugin)
            End If
        Next
    End Sub

    Private Sub AddPluginsToMenu(ByVal objMenu As ToolStripMenuItem, ByVal colPlugins As List(Of ToolPlugin))
        colPlugins.Sort(AddressOf PluginComparer)
        For Each objPlugin As ToolPlugin In colPlugins
            AddMenuOption(objMenu, objPlugin)
        Next
    End Sub

    Private Function PluginComparer(ByVal objPlugin1 As ToolPlugin, ByVal objPlugin2 As ToolPlugin) As Integer
        Return objPlugin1.SortCode.CompareTo(objPlugin2.SortCode)
    End Function

    Private Sub AddMenuOption(ByVal parent As ToolStripMenuItem, ByVal plugin As ToolPlugin)
        Dim mnuNewItem As ToolStripMenuItem = New ToolStripMenuItem()
        mnuNewItem.Text = plugin.GetMenuTitle()
        AddHandler mnuNewItem.Click, AddressOf plugin.ClickHandler
        parent.DropDownItems.Add(mnuNewItem)
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
                gSaveChangedAccounts(mobjCompany)
            End If
            mobjCompany.Teardown()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function blnImportFormAlreadyOpen() As Boolean Implements IHostUI.blnImportFormAlreadyOpen
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

    Public Sub OpenImportForm(strWindowCaption As String, objHandler As IImportHandler,
        objReader As ITrxReader) Implements IHostUI.OpenImportForm

        Using frm As BankImportAcctSelectForm = New BankImportAcctSelectForm()
            frm.ShowMe(mobjCompany, strWindowCaption, objHandler, objReader, Me)
        End Using
    End Sub

    Public Function strChooseFile(strWindowCaption As String, strFileType As String, strSettingsKey As String) _
        As String Implements IHostUI.strChooseFile
        Return CommonDialogControlForm.strChooseFile(strWindowCaption, strFileType, strSettingsKey)
    End Function

    Public Function objGetCurrentRegister() As Register Implements IHostUI.objGetCurrentRegister
        If Not TypeOf Me.ActiveMdiChild Is RegisterForm Then
            Return Nothing
        End If
        Return CType(Me.ActiveMdiChild, RegisterForm).objReg
    End Function

    Public Function objGetMainForm() As Form Implements IHostUI.objGetMainForm
        Return Me
    End Function

    Public ReadOnly Property objCompany() As Company Implements IHostUI.objCompany
        Get
            Return mobjCompany
        End Get
    End Property

    Public Sub mnuListBudgets_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListBudgets.Click
        Dim frm As ListEditorForm

        Try

            frm = New ListEditorForm
            frm.blnShowMe(mobjCompany, ListEditorForm.ListType.glngLIST_TYPE_BUDGET, gstrAddPath("Shared.bud"), mobjCompany.objBudgets, "Budget List")

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuListCategories_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListCategories.Click
        Dim frm As ListEditorForm

        Try
            frm = New ListEditorForm
            If frm.blnShowMe(mobjCompany, ListEditorForm.ListType.glngLIST_TYPE_CATEGORY, gstrAddPath("Shared.cat"),
                             mobjCompany.objIncExpAccounts, "Category List") Then
                mobjCompany.LoadCategories()
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuListPayees_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListPayees.Click
        Dim frm As PayeeListForm

        Try

            frm = New PayeeListForm
            frm.ShowMe(mobjCompany)

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

    Public Sub mnuFileExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileExit.Click
        Me.Close()
    End Sub

    Public Sub mnuFileSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileSave.Click
        Try

            gSaveChangedAccounts(mobjCompany)
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
            frm.ShowWindow(mobjCompany)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mobjCompany_SomethingModified() Handles mobjCompany.SomethingModified
        mnuFileSave.Enabled = True
    End Sub
End Class