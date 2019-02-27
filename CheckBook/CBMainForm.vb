Option Strict On
Option Explicit On

Imports System.IO
Imports System.Reflection
Imports VB = Microsoft.VisualBasic
Imports CheckBookLib
Imports PluginCore
Imports PersistTools
Imports ImportPlugins

Friend Class CBMainForm
    Inherits System.Windows.Forms.Form
    Implements IHostUI

    Private frmStartup As StartupForm
    Private mobjSecurity As Security
    Private mblnCancelStart As Boolean
    Private WithEvents mobjCompany As Company
    Private mobjHostUI As IHostUI

    Private Sub CBMainForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim objAccount As Account
        Dim objReg As Register
        Dim strDataPathValue As String

        Try
            mblnCancelStart = True
            mobjHostUI = Me

            Me.Text = strSoftwareTitle
            frmStartup = New StartupForm
            frmStartup.Show()
            frmStartup.ShowStatus("Initializing")

            strDataPathValue = System.Configuration.ConfigurationManager.AppSettings("DataPath")
            If String.IsNullOrEmpty(strDataPathValue) Then
                strDataPathValue = Company.strExecutableFolder() & "\Data"
            End If

            mobjCompany = New Company(strDataPathValue)

            If Not mobjCompany.blnDataPathExists() Then
                mobjCompany.CreateInitialData(AddressOf ShowCreateMessage)
            End If

            If mobjCompany.blnDataIsLocked() Then
                mobjHostUI.InfoMessageBox("Data files are in use by someone else running this software.")
                frmStartup.Close()
                Exit Sub
            End If

            mobjSecurity = mobjCompany.objSecurity
            mobjSecurity.Load()
            If Not mobjSecurity.blnNoFile Then
                Dim strLogin As String = ""
                Dim strPassword As String = ""

                Using frmLogin As LoginForm = New LoginForm
                    If Not frmLogin.blnGetCredentials(strLogin, strPassword) Then
                        frmStartup.Close()
                        Exit Sub
                    End If
                End Using
                If Not mobjSecurity.blnAuthenticate(strLogin, strPassword) Then
                    ErrorMessageBox("Invalid login or password")
                    frmStartup.Close()
                    Exit Sub
                End If
            End If

            CompanyLoader.Load(mobjCompany, AddressOf frmStartup.Configure)

            frmStartup.ShowStatus("Loading main window")

            LoadPlugins()

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

            mblnCancelStart = False

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
        End Try
    End Sub

    Private Sub SavedAccount(ByVal strAccountTitle As String) Handles mobjCompany.SavedAccount
        InfoMessageBox("Saved " + strAccountTitle)
    End Sub

    Private Sub ShowCreateMessage(ByVal strMessage As String)
        mobjHostUI.InfoMessageBox(strMessage)
    End Sub

    Private Property objBankImportMenu As MenuBuilder Implements IHostUI.objBankImportMenu
    Private Property objCheckImportMenu As MenuBuilder Implements IHostUI.objCheckImportMenu
    Private Property objDepositImportMenu As MenuBuilder Implements IHostUI.objDepositImportMenu
    Private Property objInvoiceImportMenu As MenuBuilder Implements IHostUI.objInvoiceImportMenu
    Private Property objReportMenu As MenuBuilder Implements IHostUI.objReportMenu
    Private Property objToolMenu As MenuBuilder Implements IHostUI.objToolMenu

    Private Sub LoadPlugins()
        objBankImportMenu = New MenuBuilder(mnuImportBank)
        objCheckImportMenu = New MenuBuilder(mnuImportChecks)
        objDepositImportMenu = New MenuBuilder(mnuImportDeposits)
        objInvoiceImportMenu = New MenuBuilder(mnuImportInvoices)
        objReportMenu = New MenuBuilder(mnuRpt)
        objToolMenu = New MenuBuilder(mnuTools)

        LoadPluginsFromAssembly(Assembly.GetEntryAssembly())
        Dim strEntryAssembly As String = Assembly.GetEntryAssembly().Location
        Dim strEntryFolder As String = Path.GetDirectoryName(strEntryAssembly)
        For Each strFile As String In Directory.EnumerateFiles(strEntryFolder, "*.dll")
            Dim assembly As Assembly = Assembly.LoadFrom(strFile)
            LoadPluginsFromAssembly(assembly)
        Next
        objBankImportMenu.AddElementsToMenu()
        objCheckImportMenu.AddElementsToMenu()
        objDepositImportMenu.AddElementsToMenu()
        objInvoiceImportMenu.AddElementsToMenu()
        objReportMenu.AddElementsToMenu()
        objToolMenu.AddElementsToMenu()
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
        For Each objPlugin As IPlugin In objFactory.colGetPlugins(Me)
            objPlugin.Register()
        Next
    End Sub

    Private Sub CBMainForm_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        If mblnCancelStart Then
            If Not frmStartup Is Nothing Then
                frmStartup.Close()
                frmStartup = Nothing
            End If
            Me.Close()
        End If
    End Sub

    Private Sub CBMainForm_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try

            If Not mblnCancelStart Then
                CompanySaver.SaveChangedAccounts(mobjCompany)
            End If
            mobjCompany.Teardown()

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
        If Not TypeOf Me.ActiveMdiChild Is RegisterForm Then
            Return Nothing
        End If
        Return CType(Me.ActiveMdiChild, RegisterForm).objReg
    End Function

    Private Function objGetMainForm() As Form Implements IHostUI.objGetMainForm
        Return Me
    End Function

    Private ReadOnly Property objCompany() As Company Implements IHostUI.objCompany
        Get
            Return mobjCompany
        End Get
    End Property

    Private Function blnAddNormalTrx(ByVal objTrx As NormalTrx,
                                    ByVal datDefaultDate As DateTime, ByVal blnCheckInvoiceNum As Boolean,
                                    ByVal strLogTitle As String) As Boolean Implements IHostUI.blnAddNormalTrx
        Using frm As TrxForm = New TrxForm()
            If frm.blnAddNormal(Me, objTrx, datDefaultDate, blnCheckInvoiceNum, strLogTitle) Then
                Return True
            End If
            Return False
        End Using
    End Function

    Private Function blnAddNormalTrxSilent(ByVal objTrx As NormalTrx,
                                    ByVal datDefaultDate As DateTime, ByVal blnCheckInvoiceNum As Boolean,
                                    ByVal strLogTitle As String) As Boolean Implements IHostUI.blnAddNormalTrxSilent
        Using frm As TrxForm = New TrxForm()
            If frm.blnAddNormalSilent(Me, objTrx, datDefaultDate, blnCheckInvoiceNum, strLogTitle) Then
                Return True
            End If
            Return False
        End Using
    End Function

    Private Function blnAddBudgetTrx(ByVal objReg As Register, ByVal datDefaultDate As DateTime,
                                     ByVal strLogTitle As String) As Boolean Implements IHostUI.blnAddBudgetTrx
        Using frm As TrxForm = New TrxForm()
            If frm.blnAddBudget(Me, objReg, datDefaultDate, strLogTitle) Then
                Return True
            End If
        End Using
        Return False
    End Function

    Private Function blnAddTransferTrx(ByVal objReg As Register, ByVal datDefaultDate As DateTime,
                                     ByVal strLogTitle As String) As Boolean Implements IHostUI.blnAddTransferTrx
        Using frm As TrxForm = New TrxForm()
            If frm.blnAddTransfer(Me, objReg, datDefaultDate, strLogTitle) Then
                Return True
            End If
        End Using
        Return False
    End Function

    Private Function blnUpdateTrx(ByVal objTrx As Trx, ByRef datDefaultDate As Date,
                                  ByVal strLogTitle As String) As Boolean Implements IHostUI.blnUpdateTrx
        Using frmEdit As TrxForm = New TrxForm()
            If frmEdit.blnUpdate(Me, objTrx, datDefaultDate, strLogTitle) Then
                Return True
            End If
        End Using
        Return False
    End Function

    Private Sub ShowRegister(ByVal objReg As Register) Implements IHostUI.ShowRegister

        Dim frm As System.Windows.Forms.Form
        Dim frmReg As RegisterForm

        For Each frm In gcolForms()
            If TypeOf frm Is RegisterForm Then
                frmReg = DirectCast(frm, RegisterForm)
                If frmReg.objReg Is objReg Then
                    frmReg.Show()
                    frmReg.Activate()
                    Exit Sub
                End If
            End If
        Next frm

        frmReg = New RegisterForm
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

    Public Sub mnuFileExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileExit.Click
        Me.Close()
    End Sub

    Public Sub mnuFileSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuFileSave.Click
        Try

            CompanySaver.SaveChangedAccounts(mobjCompany)
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
            frm.ShowWindow(Me)

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mobjCompany_SomethingModified() Handles mobjCompany.SomethingModified
        mnuFileSave.Enabled = True
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

    Private Sub mnuFilePlugins_Click(sender As Object, e As EventArgs) Handles mnuFilePlugins.Click
        Try
            Using frm As PluginList = New PluginList()
                frm.ShowMe(mobjHostUI)
            End Using
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub
End Class