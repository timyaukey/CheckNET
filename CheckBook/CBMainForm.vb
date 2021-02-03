Option Strict On
Option Explicit On

Imports System.IO
Imports System.Reflection
Imports VB = Microsoft.VisualBasic

Imports Willowsoft.TamperProofData

Friend Class CBMainForm
    Inherits System.Windows.Forms.Form
    Implements IHostUI
    Implements IHostSetup

    Private frmStartup As StartupForm
    Private mobjSecurity As Security
    Private WithEvents mobjCompany As Company
    Private mobjHostUI As IHostUI
    Private mnuFileSave As ToolStripMenuItem

    Public Shared blnCancelStart As Boolean

    Private Sub CBMainForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim objAccount As Account
        Dim objReg As Register
        Dim strDataPathValue As String

        Try
            blnCancelStart = False
            mobjHostUI = Me

            Dim strUserLicenseStatement As String = "License error"
            If Company.objMainLicense.Status = LicenseStatus.Active Then
                strUserLicenseStatement = "Licensed to " + Company.objMainLicense.LicensedTo
            ElseIf Company.objMainLicense.Status = LicenseStatus.Expired Then
                strUserLicenseStatement = "License to " + Company.objMainLicense.LicensedTo + " expired " + Company.objMainLicense.ExpirationDate.Value.ToShortDateString()
            ElseIf Company.objMainLicense.Status = LicenseStatus.Invalid Then
                strUserLicenseStatement = "License file is invalid"
            ElseIf Company.objMainLicense.Status = LicenseStatus.Missing Then
                strUserLicenseStatement = "License file is missing"
            End If

            LoadPlugins()

            Me.Text = strSoftwareTitle
            frmStartup = New StartupForm
            frmStartup.Init(Me, strUserLicenseStatement)
            frmStartup.Show()
            frmStartup.ShowStatus("Initializing")

            Using objSelectCompanyForm As SelectCompanyForm = New SelectCompanyForm()
                If objSelectCompanyForm.ShowCompanyDialog(mobjHostUI, AddressOf ShowCreateMessage) <> DialogResult.OK Then
                    frmStartup.Close()
                    Me.Close()
                    Exit Sub
                End If
                strDataPathValue = objSelectCompanyForm.strDataPath
            End Using

            'This line, plus the call to CompanyLoader.objLoad(), are the only
            'things that have to happen to load everything for a Company into memory.
            'The rest of this routine is load the UI (this program).
            mobjCompany = New Company(strDataPathValue)
            mobjSecurity = mobjCompany.objSecurity

            Dim objError As CompanyLoadError = CompanyLoader.objLoad(mobjCompany,
                AddressOf frmStartup.Configure, AddressOf objAuthenticate)
            If Not objError Is Nothing Then
                mobjHostUI.InfoMessageBox(objError.strMessage)
                frmStartup.Close()
                Me.Close()
                Exit Sub
            End If

            frmStartup.ShowStatus("Loading main window")

            Me.Text = strSoftwareTitle & " " & My.Application.Info.Version.Major & "." &
                        My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build &
                        " [" & LCase(mobjCompany.strDataPath()) & "]"

            If mobjSecurity.blnNoFile Then
                mnuEnableUserAccounts.Enabled = True
                mnuAddUserAccount.Enabled = False
                mnuDeleteUserAccount.Enabled = False
                mnuChangeCurrentPassword.Enabled = False
                mnuChangeOtherPassword.Enabled = False
                mnuRepairUserAccounts.Enabled = False
            Else
                mnuEnableUserAccounts.Enabled = False
                mnuAddUserAccount.Enabled = mobjSecurity.blnIsAdministrator
                mnuDeleteUserAccount.Enabled = mobjSecurity.blnIsAdministrator
                mnuChangeCurrentPassword.Enabled = True
                mnuChangeOtherPassword.Enabled = mobjSecurity.blnIsAdministrator
                mnuRepairUserAccounts.Enabled = mobjSecurity.blnIsAdministrator
            End If

            If Company.blnAnyNonActiveLicenses Then
                Using frm As LicenseListForm = New LicenseListForm()
                    frm.ShowMe(Me)
                End Using
            End If

            For Each objAccount In mobjCompany.colAccounts
                For Each objReg In objAccount.colRegisters
                    If objReg.blnShowInitially Then
                        ShowRegister(objReg)
                    End If
                Next
            Next

            frmStartup.Close()

        Catch ex As Exception
            gTopException(ex)
            Me.Close()
        End Try
    End Sub

    Private Function objAuthenticate(ByVal objCompany As Company) As CompanyLoadError
        If objCompany.objSecurity.blnNoFile Then
            Return Nothing
        End If
        Do
            Dim strLogin As String = ""
            Dim strPassword As String = ""

            Using frmLogin As LoginForm = New LoginForm
                If Not frmLogin.blnGetCredentials(strLogin, strPassword) Then
                    Return New CompanyLoadCanceled()
                End If
            End Using
            If objCompany.objSecurity.blnAuthenticate(strLogin, strPassword) Then
                Return Nothing
            End If
            mobjHostUI.ErrorMessageBox((New CompanyLoadNotAuthorized()).strMessage)
        Loop
    End Function

    Private Sub SavedAccount(ByVal strAccountTitle As String) Handles mobjCompany.SavedAccount
        InfoMessageBox("Saved " + strAccountTitle)
    End Sub

    Private Sub ShowCreateMessage(ByVal strMessage As String)
        mobjHostUI.InfoMessageBox(strMessage)
    End Sub

    Private Property objFileMenu As MenuBuilder Implements IHostSetup.objFileMenu
    Private Property objBankImportMenu As MenuBuilder Implements IHostSetup.objBankImportMenu
    Private Property objCheckImportMenu As MenuBuilder Implements IHostSetup.objCheckImportMenu
    Private Property objDepositImportMenu As MenuBuilder Implements IHostSetup.objDepositImportMenu
    Private Property objInvoiceImportMenu As MenuBuilder Implements IHostSetup.objInvoiceImportMenu
    Private Property objReportMenu As MenuBuilder Implements IHostSetup.objReportMenu
    Private Property objToolMenu As MenuBuilder Implements IHostSetup.objToolMenu
    Private Property objHelpMenu As MenuBuilder Implements IHostSetup.objHelpMenu

    Private Sub SetTrxFormFactory(ByVal objFactory As Func(Of ITrxForm)) Implements IHostSetup.SetTrxFormFactory
        objTrxFormFactory = objFactory
    End Sub

    Private Sub SetRegisterFormFactory(ByVal objFactory As Func(Of IRegisterForm)) Implements IHostSetup.SetRegisterFormFactory
        objRegisterFormFactory = objFactory
    End Sub

    Private Sub SetSearchFormFactory(ByVal objFactory As Func(Of ISearchForm)) Implements IHostSetup.SetSearchFormFactory
        objSearchFormFactory = objFactory
    End Sub

    Private objTrxFormFactory As Func(Of ITrxForm)
    Private objRegisterFormFactory As Func(Of IRegisterForm)
    Private objSearchFormFactory As Func(Of ISearchForm)

    Private Function objMakeTrxForm() As ITrxForm Implements IHostUI.objMakeTrxForm
        Return objTrxFormFactory()
    End Function

    Private Function objMakeRegisterForm() As IRegisterForm Implements IHostUI.objMakeRegisterForm
        Return objRegisterFormFactory()
    End Function

    Private Function objMakeSearchForm() As ISearchForm Implements IHostUI.objMakeSearchForm
        Return objSearchFormFactory()
    End Function

    Public Iterator Function objSearchHandlers() As IEnumerable(Of ISearchHandler) Implements IHostUI.objSearchHandlers
        Yield New TrxSearchHandler(Me, "Description", Function(ByVal objTrx As Trx) objTrx.strDescription)
        Yield New MemoSearchHandler(Me, "Memo")
        Yield New CategorySearchHandler(Me, "Category")
        Yield New TrxSearchHandler(Me, "Number", Function(ByVal objTrx As Trx) objTrx.strNumber)
        Yield New TrxSearchHandler(Me, "Amount", Function(ByVal objTrx As Trx) Utilities.strFormatCurrency(objTrx.curAmount))
        Yield New InvoiceSearchHandler(Me, "Invoice #")
        Yield New PurOrdSearchHandler(Me, "PO #")
    End Function

    Public Iterator Function objSearchFilters() As IEnumerable(Of ISearchFilter) Implements IHostUI.objSearchFilters
        Yield New FilterAll()
        Yield New FilterNonGenerated()
        Yield New FilterFakeOnly()
        Yield New FilterGeneratedOnly()
        Yield New FilterNonReal()
        Yield New FilterNonRealBank()
        Yield New FilterNonImportedBank()
    End Function

    Public Iterator Function objSearchTools() As IEnumerable(Of ISearchTool) Implements IHostUI.objSearchTools
        Yield New SearchCombineTool(Me)
        Yield New SearchMoveTool(Me)
        Yield New SearchExportTool(Me)
        Yield New SearchRecategorizeTool(Me)
    End Function

    Public Iterator Function objTrxTools() As IEnumerable(Of ITrxTool) Implements IHostUI.objTrxTools
        Yield New TrxPrintCheckTool(Me)
        Yield New TrxMailingAddressTool(Me)
        Yield New TrxCopyAmountTool(Me)
        Yield New TrxCopyDateTool(Me)
        Yield New TrxCopyInvoiceNumbersTool(Me)
    End Function

    Private Sub LoadPlugins()
        objFileMenu = New MenuBuilder(mnuFile)
        objBankImportMenu = New MenuBuilder(mnuImportBank)
        objCheckImportMenu = New MenuBuilder(mnuImportChecks)
        objDepositImportMenu = New MenuBuilder(mnuImportDeposits)
        objInvoiceImportMenu = New MenuBuilder(mnuImportInvoices)
        objReportMenu = New MenuBuilder(mnuRpt)
        objToolMenu = New MenuBuilder(mnuTools)
        objHelpMenu = New MenuBuilder(mnuHelp)

        Dim strPlugInPath As String = System.IO.Path.GetFileName(Me.GetType().Assembly.Location)

        objFileMenu.Add(New MenuElementAction("Registers and Accounts", 100, AddressOf mnuFileShowReg_Click, strPlugInPath))
        Dim saveAction As MenuElementAction = New MenuElementAction("Save", 200, AddressOf mnuFileSave_Click, strPlugInPath)
        objFileMenu.Add(saveAction)
        objFileMenu.Add(New MenuElementAction("Plugin List", 300, AddressOf mnuFilePlugins_Click, strPlugInPath))
        objFileMenu.Add(New MenuElementAction("Exit", 400, AddressOf mnuFileExit_Click, strPlugInPath))

        objHelpMenu.Add(New MenuElementAction("Introduction", 1,
                        Sub(sender As Object, e As EventArgs)
                            HelpShowFile("Intro.html")
                        End Sub, strPlugInPath))
        objHelpMenu.Add(New MenuElementAction("Setup and Configuration", 2,
                        Sub(sender As Object, e As EventArgs)
                            HelpShowFile("Setup.html")
                        End Sub, strPlugInPath))
        objHelpMenu.Add(New MenuElementAction("Importing Transactions", 100,
                        Sub(sender As Object, e As EventArgs)
                            HelpShowFile("Importing.html")
                        End Sub, strPlugInPath))
        objHelpMenu.Add(New MenuElementAction("Budgeting Tools", 200,
                        Sub(sender As Object, e As EventArgs)
                            HelpShowFile("Budget.html")
                        End Sub, strPlugInPath))
        objHelpMenu.Add(New MenuElementAction("Reporting and Searching", 300,
                        Sub(sender As Object, e As EventArgs)
                            HelpShowFile("Reporting.html")
                        End Sub, strPlugInPath))
        objHelpMenu.Add(New MenuElementAction("Technical Notes", 10000,
                        Sub(sender As Object, e As EventArgs)
                            HelpShowFile("Technical.html")
                        End Sub, strPlugInPath))

        LoadPluginsFromAssembly(Assembly.GetEntryAssembly())
        Dim strEntryAssembly As String = Assembly.GetEntryAssembly().Location
        Dim strEntryFolder As String = Path.GetDirectoryName(strEntryAssembly)
        For Each strFile As String In Directory.EnumerateFiles(strEntryFolder, "*.dll")
            Dim assembly As Assembly = Assembly.LoadFrom(strFile)
            LoadPluginsFromAssembly(assembly)
        Next
        objFileMenu.AddElementsToMenu()
        mnuFileSave = saveAction.MenuItemControl
        mnuFileSave.Enabled = False
        objBankImportMenu.AddElementsToMenu()
        objCheckImportMenu.AddElementsToMenu()
        objDepositImportMenu.AddElementsToMenu()
        objInvoiceImportMenu.AddElementsToMenu()
        objReportMenu.AddElementsToMenu()
        objToolMenu.AddElementsToMenu()
        objHelpMenu.AddElementsToMenu()
    End Sub

    Private Sub LoadPluginsFromAssembly(ByVal objAssembly As Assembly)
        For Each objAttrib As Object In objAssembly.GetCustomAttributes(GetType(PluginAssemblyAttribute), False)
            For Each objType As Type In objAssembly.GetExportedTypes()
                Dim objPluginType As Type = objType.GetInterface("IPlugin")
                If Not objPluginType Is Nothing Then
                    If Not objType.IsAbstract Then
                        Dim objConstructor As ConstructorInfo = objType.GetConstructor({GetType(IHostUI)})
                        If Not objConstructor Is Nothing Then
                            Dim objPlugin As IPlugin = DirectCast(objConstructor.Invoke({mobjHostUI}), IPlugin)
                            objPlugin.Register(Me)
                        End If
                    End If
                End If
            Next
        Next
    End Sub

    Private Sub CBMainForm_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        If blnCancelStart Then
            If Not frmStartup Is Nothing Then
                frmStartup.Close()
                frmStartup = Nothing
            End If
            Me.Close()
        End If
    End Sub

    Private Sub CBMainForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try

            If Not mobjCompany Is Nothing Then
                If Not blnCancelStart Then
                    CompanySaver.SaveChangedAccounts(mobjCompany)
                End If
                mobjCompany.Teardown()
                mobjCompany.UnlockCompany()
            End If

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function strChooseFile(strWindowCaption As String, strFileType As String, strSettingsKey As String) _
        As String Implements IHostUI.strChooseFile
        Return CommonDialogControlForm.strChooseFile(strWindowCaption, strFileType, strSettingsKey)
    End Function

    Private Function objGetCurrentRegister() As Register Implements IHostUI.objGetCurrentRegister
        If Not TypeOf Me.ActiveMdiChild Is IRegisterForm Then
            Return Nothing
        End If
        Return CType(Me.ActiveMdiChild, IRegisterForm).objReg
    End Function

    Private Function objGetMainForm() As Form Implements IHostUI.objGetMainForm
        Return Me
    End Function

    Private ReadOnly Property objCompany() As Company Implements IHostUI.objCompany
        Get
            Return mobjCompany
        End Get
    End Property

    Public Sub AddExtraLicense(objLicense As IStandardLicense) Implements IHostSetup.AddExtraLicense
        Company.AddExtraLicense(objLicense)
    End Sub

    Private Function blnAddNormalTrx(ByVal objTrx As NormalTrx,
                                    ByRef datDefaultDate As DateTime, ByVal blnCheckInvoiceNum As Boolean,
                                    ByVal strLogTitle As String) As Boolean Implements IHostUI.blnAddNormalTrx
        Using frm As ITrxForm = mobjHostUI.objMakeTrxForm()
            If frm.blnAddNormal(Me, objTrx, datDefaultDate, blnCheckInvoiceNum, strLogTitle) Then
                Return True
            End If
            Return False
        End Using
    End Function

    Private Function blnAddNormalTrxSilent(ByVal objTrx As NormalTrx,
                                    ByRef datDefaultDate As DateTime, ByVal blnCheckInvoiceNum As Boolean,
                                    ByVal strLogTitle As String) As Boolean Implements IHostUI.blnAddNormalTrxSilent
        Using frm As ITrxForm = mobjHostUI.objMakeTrxForm()
            If frm.blnAddNormalSilent(Me, objTrx, datDefaultDate, blnCheckInvoiceNum, strLogTitle) Then
                Return True
            End If
            Return False
        End Using
    End Function

    Private Function blnAddBudgetTrx(ByVal objReg As Register, ByRef datDefaultDate As DateTime,
                                     ByVal strLogTitle As String) As Boolean Implements IHostUI.blnAddBudgetTrx
        Using frm As ITrxForm = mobjHostUI.objMakeTrxForm()
            If frm.blnAddBudget(Me, objReg, datDefaultDate, strLogTitle) Then
                Return True
            End If
        End Using
        Return False
    End Function

    Private Function blnAddTransferTrx(ByVal objReg As Register, ByRef datDefaultDate As DateTime,
                                     ByVal strLogTitle As String) As Boolean Implements IHostUI.blnAddTransferTrx
        Using frm As ITrxForm = mobjHostUI.objMakeTrxForm()
            If frm.blnAddTransfer(Me, objReg, datDefaultDate, strLogTitle) Then
                Return True
            End If
        End Using
        Return False
    End Function

    Private Function blnUpdateTrx(ByVal objTrx As Trx, ByRef datDefaultDate As Date,
                                  ByVal strLogTitle As String) As Boolean Implements IHostUI.blnUpdateTrx
        Using frmEdit As ITrxForm = mobjHostUI.objMakeTrxForm()
            If frmEdit.blnUpdate(Me, objTrx, datDefaultDate, strLogTitle) Then
                Return True
            End If
        End Using
        Return False
    End Function

    Private Sub ShowRegister(ByVal objReg As Register) Implements IHostUI.ShowRegister

        Dim frm As System.Windows.Forms.Form
        Dim frmReg As IRegisterForm

        For Each frm In gcolForms()
            If TypeOf frm Is IRegisterForm Then
                frmReg = DirectCast(frm, IRegisterForm)
                If frmReg.objReg Is objReg Then
                    frmReg.ShowMeAgain()
                    Exit Sub
                End If
            End If
        Next frm

        frmReg = mobjHostUI.objMakeRegisterForm()
        frmReg.ShowMe(Me, objReg)
    End Sub

    Private Sub InfoMessageBox(ByVal strMessage As String) Implements IHostUI.InfoMessageBox
        MessageBox.Show(strMessage, strSoftwareTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub ErrorMessageBox(ByVal strMessage As String) Implements IHostUI.ErrorMessageBox
        MessageBox.Show(strMessage, strSoftwareTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Function OkCancelMessageBox(ByVal strMessage As String) As DialogResult Implements IHostUI.OkCancelMessageBox
        Dim res As DialogResult = MessageBox.Show(strMessage, strSoftwareTitle, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
        Return res
    End Function

    Private ReadOnly Property strSoftwareTitle() As String Implements IHostUI.strSoftwareName
        Get
            Return "Willow Creek Checkbook"
        End Get
    End Property

    Private ReadOnly Property strSplashImagePath() As String Implements IHostUI.strSplashImagePath
        Get
            Dim objAssembly As Assembly = Assembly.GetEntryAssembly()
            Dim strFolder As String = Path.GetDirectoryName(objAssembly.Location)
            Return Path.Combine(strFolder, "AltSplash.jpg")
        End Get
    End Property

    Private Sub HelpShowFile(ByVal strHtmlFile As String) Implements IHostUI.ShowHelp
        Try
            Dim objProcessInfo As System.Diagnostics.ProcessStartInfo = New ProcessStartInfo()
            Dim strFolder As String = System.IO.Path.GetDirectoryName(Me.GetType().Assembly.Location)
            strFolder = System.IO.Path.Combine(strFolder, "Help")
            objProcessInfo.FileName = System.IO.Path.Combine(strFolder, strHtmlFile)
            If Not System.IO.File.Exists(objProcessInfo.FileName) Then
                mobjHostUI.ErrorMessageBox("Could not find help file " + objProcessInfo.FileName)
                Return
            End If
            objProcessInfo.UseShellExecute = True
            System.Diagnostics.Process.Start(objProcessInfo)
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuListBudgets_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListBudgets.Click
        Dim frm As ListEditorForm

        Try

            frm = New ListEditorForm
            frm.blnShowMe(Me, ListEditorForm.ListType.Budget, mobjCompany.strBudgetPath(),
                          mobjCompany.objBudgets, "Budget List", AddressOf blnEditStringTransElem)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuListCategories_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListCategories.Click
        Try
            Using frmListEditor As ListEditorForm = New ListEditorForm()
                Using frmCatEditor As CategoryEditorForm = New CategoryEditorForm()
                    If frmListEditor.blnShowMe(Me, ListEditorForm.ListType.Category, mobjCompany.strCategoryPath(),
                             mobjCompany.objIncExpAccounts, "Category List", AddressOf frmCatEditor.blnShowDialog) Then
                        CompanyLoader.LoadCategories(mobjCompany)
                    End If
                End Using
            End Using

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function blnEditStringTransElem(ByVal objTransElem As StringTransElement, ByVal blnNew As Boolean) As Boolean
        Dim strNewValue1 As String
        strNewValue1 = InputBox("Name: ", "", objTransElem.strValue1)
        If strNewValue1 = "" Or strNewValue1 = objTransElem.strValue1 Then
            Return False
        End If
        objTransElem.strValue1 = strNewValue1
        Return True
    End Function


    Public Sub mnuListPayees_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListPayees.Click
        Dim frm As PayeeListForm

        Try

            frm = New PayeeListForm
            frm.ShowMe(Me)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuListTrxTypes_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListTrxTypes.Click
        Dim frm As TrxTypeListForm

        Try

            frm = New TrxTypeListForm
            frm.ShowMe(Me)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuFileExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        Me.Close()
    End Sub

    Public Sub mnuFileSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        Try

            CompanySaver.SaveChangedAccounts(mobjCompany)
            mnuFileSave.Enabled = False

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Public Sub mnuFileShowReg_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
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
            frm.ShowWindow(Me)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mobjCompany_SomethingModified() Handles mobjCompany.SomethingModified
        If Not mnuFileSave Is Nothing Then
            mnuFileSave.Enabled = True
        End If
    End Sub

    Private Sub mnuEnableUserAccounts_Click(sender As Object, e As EventArgs) Handles mnuEnableUserAccounts.Click
        Try
            Dim strPassword As String = ""
            mobjSecurity.CreateEmpty()
            mobjSecurity.CreateUser(mobjSecurity.strAdminLogin, "Administrator")
            mobjSecurity.SetPassword(strPassword)
            mobjSecurity.Save()
            mobjSecurity.blnFindUser(mobjSecurity.strAdminLogin)
            mobjSecurity.blnPasswordMatches(strPassword)
            mobjHostUI.InfoMessageBox("User logins enabled. Added administrator login """ & mobjSecurity.strLogin & """, with empty password.")
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mnuAddUserAccount_Click(sender As Object, e As EventArgs) Handles mnuAddUserAccount.Click
        Try
            mobjSecurity.SaveUserContext()
            AddUserAccount()
            mobjSecurity.RestoreUserContext()
        Catch ex As Exception
            mobjSecurity.RestoreUserContext()
            gTopException(ex)
        End Try
    End Sub

    Private Sub AddUserAccount()
        Dim strLogin As String = InputBox("Please enter new login name:", "New Login")
        If strLogin = "" Then
            Exit Sub
        End If
        If mobjSecurity.blnFindUser(strLogin) Then
            mobjHostUI.InfoMessageBox("Login name already exists.")
            Exit Sub
        End If
        Dim strName As String = InputBox("Please enter person's name:", "Person Name")
        If strName = "" Then
            Exit Sub
        End If
        Dim strPassword As String = strAskNewPassword(strLogin)
        If strPassword = Nothing Then
            Exit Sub
        End If
        mobjSecurity.CreateUser(strLogin, strName)
        mobjSecurity.SetPassword(strPassword)
        mobjSecurity.Save()
        mobjHostUI.InfoMessageBox("New login """ & strLogin & """ created.")
    End Sub

    Private Sub mnuChangeCurrentPassword_Click(sender As Object, e As EventArgs) Handles mnuChangeCurrentPassword.Click
        Try
            mobjSecurity.SaveUserContext()
            ChangeCurrentPassword()
            mobjSecurity.RestoreUserContext()
        Catch ex As Exception
            mobjSecurity.RestoreUserContext()
            gTopException(ex)
        End Try
    End Sub

    Private Sub ChangeCurrentPassword()
        Dim strPassword As String = strAskNewPassword(mobjSecurity.strLogin)
        If strPassword = Nothing Then
            Exit Sub
        End If
        mobjSecurity.SetPassword(strPassword)
        mobjSecurity.Save()
        mobjHostUI.InfoMessageBox("Password changed for current user login.")
    End Sub

    Private Sub mnuChangeOtherPassword_Click(sender As Object, e As EventArgs) Handles mnuChangeOtherPassword.Click
        Try
            mobjSecurity.SaveUserContext()
            ChangeOtherPassword()
            mobjSecurity.RestoreUserContext()
        Catch ex As Exception
            mobjSecurity.RestoreUserContext()
            gTopException(ex)
        End Try
    End Sub

    Private Sub ChangeOtherPassword()
        'Set password on existing user.
        Dim strLogin As String = InputBox("Please enter login name to change password for:", "Existing Login")
        If strLogin = "" Then
            Exit Sub
        End If
        If Not mobjSecurity.blnFindUser(strLogin) Then
            mobjHostUI.InfoMessageBox("Login name does not exist.")
            Exit Sub
        End If
        Dim strPassword As String = strAskNewPassword(strLogin)
        If strPassword = Nothing Then
            Exit Sub
        End If
        mobjSecurity.SetPassword(strPassword)
        mobjSecurity.Save()
        mobjHostUI.InfoMessageBox("Password changed for login """ & strLogin & """.")
    End Sub

    Private Function strAskNewPassword(ByVal strLoginName As String) As String
        Dim strPassword1 As String = InputBox("Please enter new password for login """ + strLoginName + """:", "Password")
        Dim strPassword2 As String = InputBox("Please confirm password for login """ + strLoginName + """:", "Password")
        If strPassword1 <> strPassword2 Then
            mobjHostUI.InfoMessageBox("Passwords do not match.")
            Return Nothing
        End If
        If strPassword1 = "" Then
            Return Nothing
        End If
        Return strPassword1
    End Function

    Private Sub mnuDeleteUserAccount_Click(sender As Object, e As EventArgs) Handles mnuDeleteUserAccount.Click
        Try
            mobjSecurity.SaveUserContext()
            DeleteUserAccount()
            mobjSecurity.RestoreUserContext()
        Catch ex As Exception
            mobjSecurity.RestoreUserContext()
            gTopException(ex)
        End Try
    End Sub

    Private Sub DeleteUserAccount()
        Dim strDeleteLogin As String = InputBox("Login name to delete:", "Login")
        If strDeleteLogin = "" Then
            mobjSecurity.RestoreUserContext()
            Exit Sub
        End If
        If LCase(strDeleteLogin) = mobjSecurity.strAdminLogin Then
            mobjHostUI.InfoMessageBox("Cannot delete the """ + mobjSecurity.strAdminLogin + """ login.")
            mobjSecurity.RestoreUserContext()
            Exit Sub
        End If
        If LCase(strDeleteLogin) = LCase(mobjSecurity.strLogin) Then
            mobjHostUI.InfoMessageBox("Cannot delete the login you are currently using.")
            mobjSecurity.RestoreUserContext()
            Exit Sub
        End If
        If Not mobjSecurity.blnFindUser(strDeleteLogin) Then
            mobjHostUI.InfoMessageBox("No such login.")
            mobjSecurity.RestoreUserContext()
            Exit Sub
        End If
        If mobjHostUI.OkCancelMessageBox("Are you sure you want to delete login """ + strDeleteLogin + """?") <> DialogResult.OK Then
            Exit Sub
        End If
        mobjSecurity.DeleteUser()
        mobjSecurity.Save()
        mobjHostUI.InfoMessageBox("Login deleted.")
    End Sub

    Private Sub mnuRepairUserAccounts_Click(sender As Object, e As EventArgs) Handles mnuRepairUserAccounts.Click
        Try
            mobjSecurity.CreateSignatures()
            mobjSecurity.Save()
            mobjHostUI.InfoMessageBox("User login database repaired.")
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mnuCheckFormat_Click(sender As Object, e As EventArgs) Handles mnuCheckFormat.Click
        Try
            Using frm As CheckFormatEditor = New CheckFormatEditor()
                frm.ShowMe(Me)
            End Using
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mnuCompanyInformation_Click(sender As Object, e As EventArgs) Handles mnuCompanyInformation.Click
        Try
            Using frm As CompanyInfoEditor = New CompanyInfoEditor()
                frm.ShowMe(mobjCompany)
            End Using
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mnuFilePlugins_Click(sender As Object, e As EventArgs)
        Try
            Using frm As PluginList = New PluginList()
                frm.ShowMe(Me)
            End Using
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mnuLicensing_Click(sender As Object, e As EventArgs) Handles mnuLicensing.Click
        Try
            'Using frm As LicenseForm = New LicenseForm()
            '    frm.ShowLicense(mobjHostUI, Company.objMainLicense)
            'End Using
            Using frm As LicenseListForm = New LicenseListForm()
                frm.ShowMe(mobjHostUI)
            End Using
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub
End Class