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

    Private mobjCoreRef As AssemblyName
    Private mobjLibRef As AssemblyName
    Private mobjPlugins As List(Of IPlugin)

    Public Shared blnCancelStart As Boolean

    Private Sub CBMainForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim objAccount As Account
        Dim objReg As Register
        Dim strDataPathValue As String

        Try
            blnCancelStart = False
            mobjHostUI = Me

            Dim strUserLicenseStatement As String = "License error"
            If Company.MainLicense.Status = LicenseStatus.Active Then
                strUserLicenseStatement = "Licensed to " + Company.MainLicense.LicensedTo
            ElseIf Company.MainLicense.Status = LicenseStatus.Expired Then
                strUserLicenseStatement = "License to " + Company.MainLicense.LicensedTo + " expired " + Company.MainLicense.ExpirationDate.Value.ToShortDateString()
            ElseIf Company.MainLicense.Status = LicenseStatus.Invalid Then
                strUserLicenseStatement = "License file is invalid"
            ElseIf Company.MainLicense.Status = LicenseStatus.Missing Then
                strUserLicenseStatement = "License file is missing"
            End If

            AddStandardSearchHandlers()
            AddStandardSearchTools()
            AddStandardSearchFilters()
            AddStandardTrxTools()
            LoadPlugins()

            Me.Text = strSoftwareTitle
            frmStartup = New StartupForm
            frmStartup.Init(Me, strUserLicenseStatement)
            frmStartup.Show()
            frmStartup.ShowStatus("Initializing")

            Using objSelectCompanyForm As SelectCompanyForm = New SelectCompanyForm()
                If objSelectCompanyForm.ShowCompanyDialog(mobjHostUI, AddressOf ShowCreateMessage, frmStartup) <> DialogResult.OK Then
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
            mobjSecurity = mobjCompany.SecData
            For Each objPlugin As IPlugin In mobjPlugins
                objPlugin.SetCompany(mobjCompany)
            Next

            Dim objError As CompanyLoadError = CompanyLoader.Load(mobjCompany,
                AddressOf frmStartup.Configure, AddressOf objAuthenticate)
            If Not objError Is Nothing Then
                mobjHostUI.InfoMessageBox(objError.Message)
                frmStartup.Close()
                Me.Close()
                Exit Sub
            End If
            'This gets enabled by Company events fired during account load
            mnuFileSave.Enabled = False

            frmStartup.ShowStatus("Loading main window")

            Me.Text = strSoftwareTitle & " " & My.Application.Info.Version.Major & "." &
                        My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build &
                        " [" & LCase(mobjCompany.DataFolderPath()) & "]"

            If mobjSecurity.NoFile Then
                mnuEnableUserAccounts.Enabled = True
                mnuAddUserAccount.Enabled = False
                mnuDeleteUserAccount.Enabled = False
                mnuChangeCurrentPassword.Enabled = False
                mnuChangeOtherPassword.Enabled = False
                mnuRepairUserAccounts.Enabled = False
            Else
                mnuEnableUserAccounts.Enabled = False
                mnuAddUserAccount.Enabled = mobjSecurity.IsAdministrator
                mnuDeleteUserAccount.Enabled = mobjSecurity.IsAdministrator
                mnuChangeCurrentPassword.Enabled = True
                mnuChangeOtherPassword.Enabled = mobjSecurity.IsAdministrator
                mnuRepairUserAccounts.Enabled = mobjSecurity.IsAdministrator
            End If

            If Company.AnyNonActiveLicenses Then
                Using frm As LicenseListForm = New LicenseListForm()
                    frm.ShowMe(Me)
                End Using
            End If

            For Each objAccount In mobjCompany.Accounts
                For Each objReg In objAccount.Registers
                    If objReg.ShowInitially Then
                        ShowRegister(objReg)
                    End If
                Next
            Next

            frmStartup.Close()

        Catch ex As Exception
            TopException(ex)
            Me.Close()
        End Try
    End Sub

    Private Function objAuthenticate(ByVal objCompany As Company) As CompanyLoadError
        If objCompany.SecData.NoFile Then
            Return Nothing
        End If
        Do
            Dim strLogin As String = ""
            Dim strPassword As String = ""

            Using frmLogin As LoginForm = New LoginForm
                If Not frmLogin.blnGetCredentials(strLogin, strPassword, frmStartup) Then
                    Return New CompanyLoadCanceled()
                End If
            End Using
            If objCompany.SecData.Authenticate(strLogin, strPassword) Then
                Return Nothing
            End If
            mobjHostUI.ErrorMessageBox((New CompanyLoadNotAuthorized()).Message)
        Loop
    End Function

    Private Sub SavedAccount(ByVal strAccountTitle As String) Handles mobjCompany.SavedAccount
        InfoMessageBox("Saved " + strAccountTitle)
    End Sub

    Private Sub ShowCreateMessage(ByVal strMessage As String)
        mobjHostUI.InfoMessageBox(strMessage)
    End Sub

    Private Property FileMenu As MenuBuilder Implements IHostSetup.FileMenu
    Private Property BankImportMenu As MenuBuilder Implements IHostSetup.BankImportMenu
    Private Property CheckImportMenu As MenuBuilder Implements IHostSetup.CheckImportMenu
    Private Property DepositImportMenu As MenuBuilder Implements IHostSetup.DepositImportMenu
    Private Property InvoiceImportMenu As MenuBuilder Implements IHostSetup.InvoiceImportMenu
    Private Property ReportMenu As MenuBuilder Implements IHostSetup.ReportMenu
    Private Property ToolMenu As MenuBuilder Implements IHostSetup.ToolMenu
    Private Property HelpMenu As MenuBuilder Implements IHostSetup.HelpMenu

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

    Private Function MakeTrxForm() As ITrxForm Implements IHostUI.MakeTrxForm
        Return objTrxFormFactory()
    End Function

    Private Function MakeRegisterForm() As IRegisterForm Implements IHostUI.MakeRegisterForm
        Return objRegisterFormFactory()
    End Function

    Private Function MakeSearchForm() As ISearchForm Implements IHostUI.MakeSearchForm
        Return objSearchFormFactory()
    End Function

    Private objSearchHandlers As List(Of ISearchHandler) = New List(Of ISearchHandler)()
    Private objSearchTools As List(Of ISearchTool) = New List(Of ISearchTool)()
    Private objSearchFilters As List(Of ISearchFilter) = New List(Of ISearchFilter)()
    Private objTrxTools As List(Of ITrxTool) = New List(Of ITrxTool)()

    Private Sub AddSearchHandler(ByVal objHandler As ISearchHandler) Implements IHostSetup.AddSearchHandler
        objSearchHandlers.Add(objHandler)
    End Sub

    Private Sub AddSearchTool(ByVal objTool As ISearchTool) Implements IHostSetup.AddSearchTool
        objSearchTools.Add(objTool)
    End Sub

    Private Sub AddSearchFilter(ByVal objFilter As ISearchFilter) Implements IHostSetup.AddSearchFilter
        objSearchFilters.Add(objFilter)
    End Sub

    Private Sub AddTrxTool(ByVal objTool As ITrxTool) Implements IHostSetup.AddTrxTool
        objTrxTools.Add(objTool)
    End Sub

    Private Sub AddStandardSearchHandlers()
        AddSearchHandler(New TrxSearchHandler(Me, "Description", Function(ByVal objTrx As BaseTrx) objTrx.Description))
        AddSearchHandler(New MemoSearchHandler(Me, "Memo"))
        AddSearchHandler(New CategorySearchHandler(Me, "Category"))
        AddSearchHandler(New TrxSearchHandler(Me, "Number", Function(ByVal objTrx As BaseTrx) objTrx.Number))
        AddSearchHandler(New TrxSearchHandler(Me, "Amount", Function(ByVal objTrx As BaseTrx) Utilities.FormatCurrency(objTrx.Amount)))
        AddSearchHandler(New InvoiceSearchHandler(Me, "Invoice #"))
        AddSearchHandler(New PurOrdSearchHandler(Me, "PO #"))
    End Sub

    Private Sub AddStandardSearchTools()
        AddSearchTool(New SearchCombineTool(Me))
        AddSearchTool(New SearchMoveTool(Me))
        AddSearchTool(New SearchExportTool(Me))
        AddSearchTool(New SearchRecategorizeTool(Me))
    End Sub

    Private Sub AddStandardSearchFilters()
        AddSearchFilter(New FilterAll())
        AddSearchFilter(New FilterNonGenerated())
        AddSearchFilter(New FilterFakeOnly())
        AddSearchFilter(New FilterGeneratedOnly())
        AddSearchFilter(New FilterNonReal())
        AddSearchFilter(New FilterNonRealBank())
        AddSearchFilter(New FilterNonImportedBank())
    End Sub

    Private Sub AddStandardTrxTools()
        AddTrxTool(New TrxPrintCheckTool(Me))
        AddTrxTool(New TrxMailingAddressTool(Me))
        AddTrxTool(New TrxCopyAmountTool(Me))
        AddTrxTool(New TrxCopyDateTool(Me))
        AddTrxTool(New TrxCopyInvoiceNumbersTool(Me))
    End Sub

    Public Iterator Function GetSearchHandlers() As IEnumerable(Of ISearchHandler) Implements IHostUI.GetSearchHandlers
        For Each objHandler As ISearchHandler In objSearchHandlers
            Yield objHandler
        Next
    End Function

    Public Iterator Function GetSearchTools() As IEnumerable(Of ISearchTool) Implements IHostUI.GetSearchTools
        For Each objTool As ISearchTool In objSearchTools
            Yield objTool
        Next
    End Function

    Public Iterator Function GetSearchFilters() As IEnumerable(Of ISearchFilter) Implements IHostUI.GetSearchFilters
        For Each objFilter As ISearchFilter In objSearchFilters
            Yield objFilter
        Next
    End Function

    Public Iterator Function GetTrxTools() As IEnumerable(Of ITrxTool) Implements IHostUI.GetTrxTools
        For Each objTool As ITrxTool In objTrxTools
            Yield objTool
        Next
    End Function

    Private Sub LoadPlugins()
        mobjPlugins = New List(Of IPlugin)()

        FileMenu = New MenuBuilder(mnuFile)
        BankImportMenu = New MenuBuilder(mnuImportBank)
        CheckImportMenu = New MenuBuilder(mnuImportChecks)
        DepositImportMenu = New MenuBuilder(mnuImportDeposits)
        InvoiceImportMenu = New MenuBuilder(mnuImportInvoices)
        ReportMenu = New MenuBuilder(mnuRpt)
        ToolMenu = New MenuBuilder(mnuTools)
        HelpMenu = New MenuBuilder(mnuHelp)

        FileMenu.Add(New MenuElementAction("Registers and Accounts", 100, AddressOf mnuFileShowReg_Click))
        Dim saveAction As MenuElementAction = New MenuElementAction("Save", 200, AddressOf mnuFileSave_Click)
        FileMenu.Add(saveAction)
        FileMenu.Add(New MenuElementAction("Plugin List", 300, AddressOf mnuFilePlugins_Click))
        FileMenu.Add(New MenuElementAction("Exit", 400, AddressOf mnuFileExit_Click))

        HelpMenu.Add(New MenuElementAction("Introduction", 1,
                        Sub(sender As Object, e As EventArgs)
                            HelpShowFile("Intro.html")
                        End Sub))
        HelpMenu.Add(New MenuElementAction("Setup and Configuration", 2,
                        Sub(sender As Object, e As EventArgs)
                            HelpShowFile("Setup.html")
                        End Sub))
        HelpMenu.Add(New MenuElementAction("Importing Transactions", 100,
                        Sub(sender As Object, e As EventArgs)
                            HelpShowFile("Importing.html")
                        End Sub))
        HelpMenu.Add(New MenuElementAction("Budgeting Tools", 200,
                        Sub(sender As Object, e As EventArgs)
                            HelpShowFile("Budget.html")
                        End Sub))
        HelpMenu.Add(New MenuElementAction("Reporting and Searching", 300,
                        Sub(sender As Object, e As EventArgs)
                            HelpShowFile("Reporting.html")
                        End Sub))
        HelpMenu.Add(New MenuElementAction("Technical Notes", 10000,
                        Sub(sender As Object, e As EventArgs)
                            HelpShowFile("Technical.html")
                        End Sub))

        mobjCoreRef = GetAssemblyReference(Assembly.GetEntryAssembly(), "Willowsoft.CheckBook.PluginCore")
        If mobjCoreRef Is Nothing Then
            Throw New InvalidDataException("Could not find Willowsoft.CheckBook.PluginCore assembly")
        End If
        mobjLibRef = GetAssemblyReference(Assembly.GetEntryAssembly(), "Willowsoft.CheckBook.Lib")
        If mobjLibRef Is Nothing Then
            Throw New InvalidDataException("Could not find WillowSoft.CheckBook.Lib assembly")
        End If

        LoadPluginsFromAssembly(Assembly.GetEntryAssembly())
        Dim strEntryAssembly As String = Assembly.GetEntryAssembly().Location
        Dim strEntryFolder As String = Path.GetDirectoryName(strEntryAssembly)
        For Each strFile As String In Directory.EnumerateFiles(strEntryFolder, "*.dll")
            Try
                Dim assembly As Assembly = Assembly.LoadFrom(strFile)
                LoadPluginsFromAssembly(assembly)
            Catch ex As TypeLoadException
                mobjHostUI.ErrorMessageBox("Could not load plugin file [" + strFile + "]")
            End Try
        Next
        For Each objPlugin As IPlugin In mobjPlugins
            objPlugin.Register(Me)
        Next

        FileMenu.AddElementsToMenu()
        BankImportMenu.AddElementsToMenu()
        CheckImportMenu.AddElementsToMenu()
        DepositImportMenu.AddElementsToMenu()
        InvoiceImportMenu.AddElementsToMenu()
        ReportMenu.AddElementsToMenu()
        ToolMenu.AddElementsToMenu()
        HelpMenu.AddElementsToMenu()

        mnuFileSave = saveAction.MenuItemControl
    End Sub

    Private Sub LoadPluginsFromAssembly(ByVal objAssembly As Assembly)
        For Each objAttrib As Object In objAssembly.GetCustomAttributes(GetType(PluginAssemblyAttribute), False)
            For Each objType As Type In objAssembly.GetExportedTypes()
                Dim objPluginType As Type = objType.GetInterface("IPlugin")
                If Not objPluginType Is Nothing Then
                    If Not objType.IsAbstract Then
                        'Check version compatibility of CheckBook.PluginCore.
                        Dim objCoreRef As AssemblyName = GetAssemblyReference(objAssembly, "Willowsoft.CheckBook.PluginCore")
                        If objCoreRef Is Nothing Then
                            mobjHostUI.ErrorMessageBox("Plugin [" + objAssembly.FullName +
                                                       "] is missing reference to Willowsoft.CheckBook.PluginCore")
                            Return
                        End If
                        If objCoreRef.Version.Major <> mobjCoreRef.Version.Major Then
                            mobjHostUI.ErrorMessageBox("Plugin [" + objAssembly.FullName +
                                                       "] references wrong major version of Willowsoft.CheckBook.PluginCore")
                            Return
                        End If
                        If objCoreRef.Version.Minor > mobjCoreRef.Version.Minor Then
                            mobjHostUI.ErrorMessageBox("Plugin [" + objAssembly.FullName +
                                                       "] references newer minor version of Willowsoft.CheckBook.PluginCore")
                            Return
                        End If
                        'Check version compatibility of CheckBook.Lib.
                        Dim objLibRef As AssemblyName = GetAssemblyReference(objAssembly, "Willowsoft.CheckBook.Lib")
                        If objLibRef Is Nothing Then
                            mobjHostUI.ErrorMessageBox("Plugin [" + objAssembly.FullName +
                                                       "] is missing reference to Willowsoft.CheckBook.Lib")
                            Return
                        End If
                        If objLibRef.Version.Major <> mobjLibRef.Version.Major Then
                            mobjHostUI.ErrorMessageBox("Plugin [" + objAssembly.FullName +
                                                       "] references wrong major version of Willowsoft.CheckBook.Lib")
                            Return
                        End If
                        If objLibRef.Version.Minor > mobjLibRef.Version.Minor Then
                            mobjHostUI.ErrorMessageBox("Plugin [" + objAssembly.FullName +
                                                       "] references newer minor version of Willowsoft.CheckBook.Lib")
                            Return
                        End If
                        'Create instance of plugin and call its Register() method.
                        Dim objConstructor As ConstructorInfo = objType.GetConstructor({GetType(IHostUI)})
                        If Not objConstructor Is Nothing Then
                            Dim objPlugin As IPlugin = DirectCast(objConstructor.Invoke({mobjHostUI}), IPlugin)
                            mobjPlugins.Add(objPlugin)
                        End If
                    End If
                End If
            Next
        Next
    End Sub

    Private Function GetAssemblyReference(ByVal objAssembly As Assembly, ByVal strRefName As String) As AssemblyName
        'Empirically, GetReferencedAssemblies() returns the references used to compile
        'the assembly, not how those references were satisfied at runtime. For example,
        'if two plugins were compiled against different versions of an assembly then
        'they will have different AssemblyName.Version properties.
        For Each objRefName As AssemblyName In objAssembly.GetReferencedAssemblies()
            If objRefName.Name = strRefName Then
                Return objRefName
            End If
        Next
        Return Nothing
    End Function

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
                CompanySaver.Unload(mobjCompany)
            End If

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub CBMainForm_MdiChildActivate(sender As Object, e As EventArgs) Handles MyBase.MdiChildActivate
        mobjCompany.FireChildFormActivated(Me.ActiveMdiChild)
    End Sub

    Private Function ChooseFile(strWindowCaption As String, strFileType As String, strSettingsKey As String) _
        As String Implements IHostUI.ChooseFile
        Return CommonDialogControlForm.strChooseFile(strWindowCaption, strFileType, strSettingsKey)
    End Function

    Private Function GetCurrentRegister() As Register Implements IHostUI.GetCurrentRegister
        If Not TypeOf Me.ActiveMdiChild Is IRegisterForm Then
            Return Nothing
        End If
        Return CType(Me.ActiveMdiChild, IRegisterForm).Reg
    End Function

    Private Function GetMainForm() As Form Implements IHostUI.GetMainForm
        Return Me
    End Function

    Private ReadOnly Property Company() As Company Implements IHostUI.Company
        Get
            Return mobjCompany
        End Get
    End Property

    Private ReadOnly Property GetChildForms() As IEnumerable(Of Form) Implements IHostUI.GetChildForms
        Get
            Dim frm As System.Windows.Forms.Form
            Dim colResult As List(Of Form)
            colResult = New List(Of Form)
            For Each frm In Me.MdiChildren
                colResult.Add(frm)
            Next frm
            Return colResult
        End Get
    End Property

    Public Sub AddExtraLicense(objLicense As IStandardLicense) Implements IHostSetup.AddExtraLicense
        Company.AddExtraLicense(objLicense)
    End Sub

    Private Function AddNormalTrx(ByVal objTrx As BankTrx,
                                    ByRef datDefaultDate As DateTime, ByVal blnCheckInvoiceNum As Boolean,
                                    ByVal strLogTitle As String) As Boolean Implements IHostUI.AddNormalTrx
        Using frm As ITrxForm = mobjHostUI.MakeTrxForm()
            If frm.AddNormal(Me, objTrx, datDefaultDate, blnCheckInvoiceNum, strLogTitle) Then
                Return True
            End If
            Return False
        End Using
    End Function

    Private Function AddNormalTrxSilent(ByVal objTrx As BankTrx,
                                    ByRef datDefaultDate As DateTime, ByVal blnCheckInvoiceNum As Boolean,
                                    ByVal strLogTitle As String) As Boolean Implements IHostUI.AddNormalTrxSilent
        Using frm As ITrxForm = mobjHostUI.MakeTrxForm()
            If frm.AddNormalSilent(Me, objTrx, datDefaultDate, blnCheckInvoiceNum, strLogTitle) Then
                Return True
            End If
            Return False
        End Using
    End Function

    Private Function AddBudgetTrx(ByVal objReg As Register, ByRef datDefaultDate As DateTime,
                                     ByVal strLogTitle As String) As Boolean Implements IHostUI.AddBudgetTrx
        Using frm As ITrxForm = mobjHostUI.MakeTrxForm()
            If frm.AddBudget(Me, objReg, datDefaultDate, strLogTitle) Then
                Return True
            End If
        End Using
        Return False
    End Function

    Private Function AddTransferTrx(ByVal objReg As Register, ByRef datDefaultDate As DateTime,
                                     ByVal strLogTitle As String) As Boolean Implements IHostUI.AddTransferTrx
        Using frm As ITrxForm = mobjHostUI.MakeTrxForm()
            If frm.AddTransfer(Me, objReg, datDefaultDate, strLogTitle) Then
                Return True
            End If
        End Using
        Return False
    End Function

    Private Function UpdateTrx(ByVal objTrx As BaseTrx, ByRef datDefaultDate As Date,
                                  ByVal strLogTitle As String) As Boolean Implements IHostUI.UpdateTrx
        Using frmEdit As ITrxForm = mobjHostUI.MakeTrxForm()
            If frmEdit.UpdateTrx(Me, objTrx, datDefaultDate, strLogTitle) Then
                Return True
            End If
        End Using
        Return False
    End Function

    Private Sub ShowRegister(ByVal objReg As Register) Implements IHostUI.ShowRegister

        Dim frm As System.Windows.Forms.Form
        Dim frmReg As IRegisterForm

        For Each frm In Me.GetChildForms()
            If TypeOf frm Is IRegisterForm Then
                frmReg = DirectCast(frm, IRegisterForm)
                If frmReg.Reg Is objReg Then
                    frmReg.ShowMeAgain()
                    Exit Sub
                End If
            End If
        Next frm

        frmReg = mobjHostUI.MakeRegisterForm()
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

    Private ReadOnly Property strSoftwareTitle() As String Implements IHostUI.SoftwareName
        Get
            Return "Willow Creek Checkbook"
        End Get
    End Property

    Private ReadOnly Property SplashImagePath() As String Implements IHostUI.SplashImagePath
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
            TopException(ex)
        End Try
    End Sub

    Public Sub mnuListBudgets_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListBudgets.Click
        Dim frm As ListEditorForm

        Try

            frm = New ListEditorForm
            frm.blnShowMe(Me, ListEditorForm.ListType.Budget, mobjCompany.BudgetFilePath(),
                          mobjCompany.Budgets, "Budget List", AddressOf blnEditStringTransElem)

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Public Sub mnuListCategories_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListCategories.Click
        Try
            Using frmListEditor As ListEditorForm = New ListEditorForm()
                Using frmCatEditor As CategoryEditorForm = New CategoryEditorForm()
                    If frmListEditor.blnShowMe(Me, ListEditorForm.ListType.Category, mobjCompany.CategoryFilePath(),
                             mobjCompany.IncExpAccounts, "Category List", AddressOf frmCatEditor.blnShowDialog) Then
                        CompanyLoader.LoadCategories(mobjCompany)
                    End If
                End Using
            End Using

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Function blnEditStringTransElem(ByVal objTransElem As StringTransElement, ByVal blnNew As Boolean) As Boolean
        Dim strNewValue1 As String
        strNewValue1 = InputBox("Name: ", "", objTransElem.Value1)
        If strNewValue1 = "" Or strNewValue1 = objTransElem.Value1 Then
            Return False
        End If
        objTransElem.Value1 = strNewValue1
        Return True
    End Function


    Public Sub mnuListPayees_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListPayees.Click
        Dim frm As PayeeListForm

        Try

            frm = New PayeeListForm
            frm.ShowMe(Me)

            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Public Sub mnuListTrxTypes_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListTrxTypes.Click
        Dim frm As TrxTypeListForm

        Try

            frm = New TrxTypeListForm
            frm.ShowMe(Me)

            Exit Sub
        Catch ex As Exception
            TopException(ex)
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
            TopException(ex)
        End Try
    End Sub

    Public Sub mnuFileShowReg_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)
        Dim frm As ShowRegisterForm
        Dim frm2 As System.Windows.Forms.Form

        Try

            For Each frm2 In Me.GetChildForms()
                If TypeOf frm2 Is ShowRegisterForm Then
                    frm2.Activate()
                    Exit Sub
                End If
            Next frm2

            frm = New ShowRegisterForm
            frm.ShowWindow(Me)

            Exit Sub
        Catch ex As Exception
            TopException(ex)
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
            mobjSecurity.CreateUser(mobjSecurity.AdminLogin, "Administrator")
            mobjSecurity.SetPassword(strPassword)
            mobjSecurity.Save()
            mobjSecurity.FindUser(mobjSecurity.AdminLogin)
            mobjSecurity.PasswordMatches(strPassword)
            mobjHostUI.InfoMessageBox("User logins enabled. Added administrator login """ & mobjSecurity.LoginName & """, with empty password.")
            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub mnuAddUserAccount_Click(sender As Object, e As EventArgs) Handles mnuAddUserAccount.Click
        Try
            mobjSecurity.SaveUserContext()
            AddUserAccount()
            mobjSecurity.RestoreUserContext()
        Catch ex As Exception
            mobjSecurity.RestoreUserContext()
            TopException(ex)
        End Try
    End Sub

    Private Sub AddUserAccount()
        Dim strLogin As String = InputBox("Please enter new login name:", "New Login")
        If strLogin = "" Then
            Exit Sub
        End If
        If mobjSecurity.FindUser(strLogin) Then
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
            TopException(ex)
        End Try
    End Sub

    Private Sub ChangeCurrentPassword()
        Dim strPassword As String = strAskNewPassword(mobjSecurity.LoginName)
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
            TopException(ex)
        End Try
    End Sub

    Private Sub ChangeOtherPassword()
        'Set password on existing user.
        Dim strLogin As String = InputBox("Please enter login name to change password for:", "Existing Login")
        If strLogin = "" Then
            Exit Sub
        End If
        If Not mobjSecurity.FindUser(strLogin) Then
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
            TopException(ex)
        End Try
    End Sub

    Private Sub DeleteUserAccount()
        Dim strDeleteLogin As String = InputBox("Login name to delete:", "Login")
        If strDeleteLogin = "" Then
            mobjSecurity.RestoreUserContext()
            Exit Sub
        End If
        If LCase(strDeleteLogin) = mobjSecurity.AdminLogin Then
            mobjHostUI.InfoMessageBox("Cannot delete the """ + mobjSecurity.AdminLogin + """ login.")
            mobjSecurity.RestoreUserContext()
            Exit Sub
        End If
        If LCase(strDeleteLogin) = LCase(mobjSecurity.LoginName) Then
            mobjHostUI.InfoMessageBox("Cannot delete the login you are currently using.")
            mobjSecurity.RestoreUserContext()
            Exit Sub
        End If
        If Not mobjSecurity.FindUser(strDeleteLogin) Then
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
            TopException(ex)
        End Try
    End Sub

    Private Sub mnuCheckFormat_Click(sender As Object, e As EventArgs) Handles mnuCheckFormat.Click
        Try
            Using frm As CheckFormatEditor = New CheckFormatEditor()
                frm.ShowMe(Me)
            End Using
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub mnuCompanyInformation_Click(sender As Object, e As EventArgs) Handles mnuCompanyInformation.Click
        Try
            Using frm As CompanyInfoEditor = New CompanyInfoEditor()
                frm.ShowMe(mobjCompany)
            End Using
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub mnuFilePlugins_Click(sender As Object, e As EventArgs)
        Try
            Using frm As PluginList = New PluginList()
                frm.ShowMe(Me, mobjPlugins)
            End Using
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub mnuLicensing_Click(sender As Object, e As EventArgs) Handles mnuLicensing.Click
        Try
            'Using frm As LicenseForm = New LicenseForm()
            '    frm.ShowLicense(mobjHostUI, Company.MainLicense)
            'End Using
            Using frm As LicenseListForm = New LicenseListForm()
                frm.ShowMe(mobjHostUI)
            End Using
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

End Class