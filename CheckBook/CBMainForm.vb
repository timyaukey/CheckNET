Option Strict On
Option Explicit On

Imports System.IO
Imports System.Reflection
Imports VB = Microsoft.VisualBasic
Imports CheckBookLib
Imports ImportPlugins

Friend Class CBMainForm
    Inherits System.Windows.Forms.Form
    Implements IHostUI

    Private frmStartup As StartupForm
    Private mobjSecurity As Security
    Private mblnCancelStart As Boolean
    Private WithEvents mobjCompany As Company

    Private Sub CBMainForm_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Dim objAccount As Account
        Dim objReg As Register
        Dim strDataPathValue As String

        Try
            mblnCancelStart = True

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
                MsgBox("Data files are in use by someone else running this software.")
                frmStartup.Close()
                Exit Sub
            End If

            mobjSecurity = mobjCompany.objSecurity
            mobjSecurity.Load()
            If Not mobjSecurity.blnNoFile Then
                If Not gblnUserAuthenticated(mobjSecurity) Then
                    frmStartup.Close()
                    Exit Sub
                End If
            End If

            mobjCompany.Load(AddressOf frmStartup.Configure)

            frmStartup.ShowStatus("Loading main window")

            LoadPlugins()

            Me.Text = "Willow Creek Checkbook " & My.Application.Info.Version.Major & "." &
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
                        gShowRegister(Me, objReg)
                    End If
                Next
            Next

            frmStartup.Close()

        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub ShowCreateMessage(ByVal strMessage As String)
        MsgBox(strMessage, MsgBoxStyle.Information)
    End Sub

    Private colBankImportPlugins As List(Of IBankImportPlugin) = New List(Of IBankImportPlugin)
    Private colCheckImportPlugins As List(Of ICheckImportPlugin) = New List(Of ICheckImportPlugin)
    Private colDepositImportPlugins As List(Of IDepositImportPlugin) = New List(Of IDepositImportPlugin)
    Private colInvoiceImportPlugins As List(Of IInvoiceImportPlugin) = New List(Of IInvoiceImportPlugin)
    Private colReportPlugins As List(Of IToolPlugin) = New List(Of IToolPlugin)
    Private colToolPlugins As List(Of IToolPlugin) = New List(Of IToolPlugin)

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
        AddPluginsToMenu(mnuRpt, colReportPlugins)
        AddPluginsToMenu(mnuTools, colToolPlugins)
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
            If TypeOf objPlugin Is IBankImportPlugin Then
                colBankImportPlugins.Add(DirectCast(objPlugin, IBankImportPlugin))
            ElseIf TypeOf objPlugin Is ICheckImportPlugin Then
                colCheckImportPlugins.Add(DirectCast(objPlugin, ICheckImportPlugin))
            ElseIf TypeOf objPlugin Is IDepositImportPlugin Then
                colDepositImportPlugins.Add(DirectCast(objPlugin, IDepositImportPlugin))
            ElseIf TypeOf objPlugin Is IInvoiceImportPlugin Then
                colInvoiceImportPlugins.Add(DirectCast(objPlugin, IInvoiceImportPlugin))
            ElseIf TypeOf objPlugin Is IReportPlugin Then
                colReportPlugins.Add(DirectCast(objPlugin, IReportPlugin))
            ElseIf TypeOf objPlugin Is IToolPlugin Then
                colToolPlugins.Add(DirectCast(objPlugin, IToolPlugin))
            Else
                'Plugin is of a type we don't know what to do with.
            End If
        Next
    End Sub

    Private Sub AddPluginsToMenu(Of TPlugin As IToolPlugin)(ByVal objMenu As ToolStripMenuItem, ByVal colPlugins As List(Of TPlugin))
        colPlugins.Sort(AddressOf PluginComparer)
        For Each objPlugin As IToolPlugin In colPlugins
            AddMenuOption(objMenu, objPlugin)
        Next
    End Sub

    Private Function PluginComparer(ByVal objPlugin1 As IToolPlugin, ByVal objPlugin2 As IToolPlugin) As Integer
        Return objPlugin1.SortCode().CompareTo(objPlugin2.SortCode())
    End Function

    Private Sub AddMenuOption(ByVal parent As ToolStripMenuItem, ByVal plugin As IToolPlugin)
        Dim mnuNewItem As ToolStripMenuItem = New ToolStripMenuItem()
        mnuNewItem.Text = plugin.GetMenuTitle()
        AddHandler mnuNewItem.Click, AddressOf plugin.ClickHandler
        parent.DropDownItems.Add(mnuNewItem)
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
                gSaveChangedAccounts(mobjCompany)
            End If
            mobjCompany.Teardown()

            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
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

    Public Function blnAddNormalTrx(ByVal objReg As Register, ByVal objTrx As NormalTrx,
                                    ByVal datDefaultDate As DateTime, ByVal blnCheckInvoiceNum As Boolean,
                                    ByVal strLogTitle As String) As Boolean Implements IHostUI.blnAddNormalTrx
        Using frm As TrxForm = New TrxForm()
            If frm.blnAddNormal(objReg, objTrx, datDefaultDate, blnCheckInvoiceNum, strLogTitle) Then
                Return True
            End If
            Return False
        End Using
    End Function

    Public Function blnAddNormalTrxSilent(ByVal objReg As Register, ByVal objTrx As NormalTrx,
                                    ByVal datDefaultDate As DateTime, ByVal blnCheckInvoiceNum As Boolean,
                                    ByVal strLogTitle As String) As Boolean Implements IHostUI.blnAddNormalTrxSilent
        Using frm As TrxForm = New TrxForm()
            If frm.blnAddNormalSilent(objReg, objTrx, datDefaultDate, blnCheckInvoiceNum, strLogTitle) Then
                Return True
            End If
            Return False
        End Using
    End Function

    Public Sub mnuListBudgets_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles mnuListBudgets.Click
        Dim frm As ListEditorForm

        Try

            frm = New ListEditorForm
            frm.blnShowMe(mobjCompany, ListEditorForm.ListType.Budget, mobjCompany.strBudgetPath(),
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
                    If frmListEditor.blnShowMe(mobjCompany, ListEditorForm.ListType.Category, mobjCompany.strCategoryPath(),
                             mobjCompany.objIncExpAccounts, "Category List", AddressOf frmCatEditor.blnShowDialog) Then
                        mobjCompany.LoadCategories()
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
            frm.ShowMe(mobjCompany)

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
            MsgBox("User logins enabled. Added administrator login """ & mobjSecurity.strLogin & """, with empty password.")
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
            MsgBox("Login name already exists.")
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
        MsgBox("New login """ & strLogin & """ created.")
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
        MsgBox("Password changed for current user login.")
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
            MsgBox("Login name does not exist.")
            Exit Sub
        End If
        Dim strPassword As String = strAskNewPassword(strLogin)
        If strPassword = Nothing Then
            Exit Sub
        End If
        mobjSecurity.SetPassword(strPassword)
        mobjSecurity.Save()
        MsgBox("Password changed for login """ & strLogin & """.")
    End Sub

    Private Function strAskNewPassword(ByVal strLoginName As String) As String
        Dim strPassword1 As String = InputBox("Please enter new password for login """ + strLoginName + """:", "Password")
        Dim strPassword2 As String = InputBox("Please confirm password for login """ + strLoginName + """:", "Password")
        If strPassword1 <> strPassword2 Then
            MsgBox("Passwords do not match.", MsgBoxStyle.Exclamation)
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
            MsgBox("Cannot delete the """ + mobjSecurity.strAdminLogin + """ login.")
            mobjSecurity.RestoreUserContext()
            Exit Sub
        End If
        If LCase(strDeleteLogin) = LCase(mobjSecurity.strLogin) Then
            MsgBox("Cannot delete the login you are currently using.")
            mobjSecurity.RestoreUserContext()
            Exit Sub
        End If
        If Not mobjSecurity.blnFindUser(strDeleteLogin) Then
            MsgBox("No such login.")
            mobjSecurity.RestoreUserContext()
            Exit Sub
        End If
        If MsgBox("Are you sure you want to delete login """ + strDeleteLogin + """?", MsgBoxStyle.Question Or MsgBoxStyle.OkCancel) <> MsgBoxResult.Ok Then
            Exit Sub
        End If
        mobjSecurity.DeleteUser()
        mobjSecurity.Save()
        MsgBox("Login deleted.")
    End Sub

    Private Sub mnuRepairUserAccounts_Click(sender As Object, e As EventArgs) Handles mnuRepairUserAccounts.Click
        Try
            mobjSecurity.CreateSignatures()
            mobjSecurity.Save()
            MsgBox("User login database repaired.")
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub mnuCheckFormat_Click(sender As Object, e As EventArgs) Handles mnuCheckFormat.Click
        Try
            Using frm As CheckFormatEditor = New CheckFormatEditor()
                frm.ShowMe(mobjCompany)
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
End Class