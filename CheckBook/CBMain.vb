Option Strict Off
Option Explicit On

Imports CheckBookLib

Public Module CBMain
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'UI related stuff.

    Public Const gstrREG_APP As String = "Willow Creek Checkbook"
    Public Const gstrREG_KEY_GENERAL As String = "General"
    'See also: gstrRegkeyRegister().

    'Lowest index in a ListView ListItem collection.
    'Will be 0 in .NET, 1 in VB6.
    Public Const gintLISTITEM_LOWINDEX As Short = 0

    Private Class BackupPurgeDay
        Public colInstances As ICollection(Of BackupInstance)
    End Class

    Private Class BackupInstance
        Public datCreate As Date
        Public strName As String
    End Class

    'Type of search used to recognize trx that have already been imported.
    Public Enum ImportStatusSearch
        Bank = 1 'Bank import key.
        PayeeNonGenerated = 2 'Payee name and non-generated trx.
        VendorInvoice = 3 'Vendor name and invoice number.
        BillPayment = 4 'Check number, or close match on payee, date and amount
    End Enum

    'Type of search used to find Trx matches for batch updates during import.
    Public Enum ImportBatchUpdateSearch
        None = 1 'None
        Bank = 2 'Number, or date/description/amount (2 of 3)
        Payee = 3 'Exact payee name within date range
    End Enum

    'Type of search used to find Trx matches for batch new Trx creation during import.
    Public Enum ImportBatchNewSearch
        None = 1 'Do not allow batch creation of new Trx.
        Bank = 2 'Number, or date/description/amount (2 of 3).
        VendorInvoice = 3 'Vendor name and invoice number.
    End Enum

    'Fields to update during batch import.
    Public Enum ImportBatchUpdateType
        None = 1 'Do not allow batch updates during import.
        Bank = 2 'Bank import key, number, date if not a check, fake->real but not reverse.
        Amount = 3 'Amount only.
        NumberAmount = 4 'Number and amount.
    End Enum

    'Type of search used when user clicks on an imported trx.
    Public Enum ImportIndividualSearchType
        Bank = 1 'Number, or date/description/amount (2 of 3)
        Payee = 2 'Exact payee name within date range
        VendorInvoice = 3 'Vendor name and invoice number.
    End Enum

    'Fields to update during individual import.
    Public Enum ImportIndividualUpdateType
        None = 1 'None.
        Bank = 2 'Bank import key, number, amount.
        Amount = 3 'Amount only.
        NumberAmount = 4 'Number and amount.
    End Enum

    Public Sub Main()
        CBMainForm.Show()
    End Sub

    Public Function gcolForms() As IEnumerable(Of Form)
        Dim frm As System.Windows.Forms.Form
        Dim colResult As List(Of Form)
        colResult = New List(Of Form)
        For Each frm In CBMainForm.MdiChildren
            colResult.Add(frm)
        Next frm
        gcolForms = colResult
    End Function

    Public Function gobjListViewAdd(ByVal lvw As System.Windows.Forms.ListView) As System.Windows.Forms.ListViewItem
        gobjListViewAdd = lvw.Items.Add("")
    End Function

    Public Sub gSetListViewSortColumn(ByVal lvw As System.Windows.Forms.ListView, ByVal intColumn As Short)
        'In .NET the list view will have to have its .ListViewItemSorter set to
        'some object, and the column passed to that object. Copy ListViewSorter.vb
        'into project.
        If intColumn = 0 Then
            lvw.ListViewItemSorter = Nothing
            'UPGRADE_ISSUE: MSComctlLib.ListView property lvw.SortKey was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        Else
            lvw.ListViewItemSorter = New ListViewSorter(intColumn)
            'UPGRADE_ISSUE: MSComctlLib.ListView property lvw.SortKey was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
        End If
    End Sub

    Private objLockFile As System.IO.Stream

    Public Function gblnDataIsLocked() As Boolean
        Try
            objLockFile = New IO.FileStream(gstrAddPath("LockFile.dat"), IO.FileMode.Append, IO.FileAccess.Write, IO.FileShare.None)
            gblnDataIsLocked = False
            Exit Function
        Catch ex As System.IO.IOException
            gblnDataIsLocked = True
            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Public Function gblnUserAuthenticated() As Boolean
        Try
            Dim strLogin As String
            Dim strPassword As String
            Dim frmLogin As LoginForm

            strLogin = ""
            strPassword = ""
            frmLogin = New LoginForm
            If Not frmLogin.blnGetCredentials(strLogin, strPassword) Then
                Exit Function
            End If

            gblnUserAuthenticated = False
            gobjSecurity = New Security
            gobjSecurity.Load(gobjSecurity.strDefaultFileName)
            If Not gobjSecurity.blnFindUser(strLogin) Then
                MsgBox("Invalid login or password")
                Exit Function
            End If
            If Not gobjSecurity.blnPasswordMatches(strPassword) Then
                MsgBox("Invalid login or password")
                Exit Function
            End If
            If Not gobjSecurity.blnUserSignatureIsValid Then
                MsgBox("User data is invalid")
                Exit Function
            End If
            gblnUserAuthenticated = True

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Friend Sub gShowRegister(ByVal objAccount As Account, ByVal objReg As Register, ByVal frmStartup As StartupForm)

        Dim frm As System.Windows.Forms.Form
        Dim frmReg As RegisterForm

        For Each frm In gcolForms()
            If TypeOf frm Is RegisterForm Then
                frmReg = frm
                If frmReg.objReg Is objReg Then
                    frmReg.Show()
                    frmReg.Activate()
                    Exit Sub
                End If
            End If
        Next frm

        frmReg = New RegisterForm
        frmReg.ShowMe(objAccount, objReg, frmStartup)
    End Sub

    Public Sub gSaveChangedAccounts()
        Dim objAccount As Account
        Dim strBackupFile As String
        For Each objAccount In gcolAccounts
            If objAccount.blnUnsavedChanges Then
                strBackupFile = gstrBackupPath() & "\" & objAccount.strFileLoaded & "." & Now.ToString("MM$dd$yy$hh$mm")
                If Len(Dir(strBackupFile)) Then
                    Kill(strBackupFile)
                End If
                Rename(gstrAccountPath() & "\" & objAccount.strFileLoaded, strBackupFile)
                PurgeAccountBackups(objAccount)
                objAccount.Save(objAccount.strFileLoaded)
                MsgBox("Saved " & objAccount.strTitle)
            End If
        Next objAccount
    End Sub

    Private Sub PurgeAccountBackups(ByVal objAccount As Account)
        'Backups older than the upper bound of this array (in days) will be deleted.
        Dim adatDays(30) As BackupPurgeDay
        Dim strBackup As String
        Dim strParsableDate As String
        Dim datCreateDate As Date
        Dim strEncodedDate As String
        Dim intIndex As Short
        Dim intBackupsToKeep As Short
        Dim intAgeInDays As Short
        Dim colInstances As List(Of BackupInstance)
        Dim colOlderFiles As List(Of String) = New List(Of String)

        Try

            For intIndex = LBound(adatDays) To UBound(adatDays)
                adatDays(intIndex) = New BackupPurgeDay()
                adatDays(intIndex).colInstances = New List(Of BackupInstance)
            Next

            strBackup = Dir(gstrBackupPath() & "\" & objAccount.strFileLoaded & ".*", FileAttribute.Normal)
            Do While strBackup <> ""
                strEncodedDate = Mid(strBackup, InStr(UCase(strBackup), ".ACT.") + 5)
                strParsableDate = "20" & Mid(strEncodedDate, 7, 2) & "/" & Mid(strEncodedDate, 1, 2) & "/" & Mid(strEncodedDate, 4, 2) & " " & Mid(strEncodedDate, 10, 2) & ":" & Mid(strEncodedDate, 13, 2)
                datCreateDate = CDate(strParsableDate)
                intAgeInDays = DateDiff(Microsoft.VisualBasic.DateInterval.Day, datCreateDate, Today)
                If intAgeInDays <= UBound(adatDays) Then
                    Dim inst As BackupInstance = New BackupInstance()
                    inst.datCreate = datCreateDate
                    inst.strName = strBackup
                    adatDays(intAgeInDays).colInstances.Add(inst)
                Else
                    colOlderFiles.Add(strBackup)
                End If
                strBackup = Dir()
            Loop

            'Delete the very old backups
            For Each strBackup In colOlderFiles
                Kill(gstrBackupPath() & "\" & strBackup)
            Next

            'Delete everything but the "intBackupsToKeep" most recent backups created on each date.
            For intAgeInDays = 0 To UBound(adatDays)
                If intAgeInDays = 0 Then
                    'Keep all backups from the current date.
                    intBackupsToKeep = 2 ' 100
                ElseIf intAgeInDays < 5 Then
                    intBackupsToKeep = 10
                Else
                    intBackupsToKeep = 1
                End If

                colInstances = adatDays(intAgeInDays).colInstances
                colInstances.Sort(AddressOf BackupInstanceComparer)
                For intIndex = 1 To colInstances.Count() - intBackupsToKeep
                    strBackup = colInstances(intIndex - 1).strName
                    Kill(gstrBackupPath() & "\" & strBackup)
                Next
            Next

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Function BackupInstanceComparer(ByVal i1 As BackupInstance, ByVal i2 As BackupInstance)
        Return i1.datCreate.CompareTo(i2.datCreate)
    End Function

    Public Function gblnAskAndCreateAccount() As Boolean
        Dim strFileRoot As String
        Dim strFile As String
        Dim strAcctName As String

        strFileRoot = InputBox("File name for new account:", "New Account")
        If strFileRoot = "" Then
            Exit Function
        End If
        strFile = gstrAccountPath() & "\" & strFileRoot & ".act"
        If Dir(strFile) <> "" Then
            MsgBox("Account file already exists with that name.", MsgBoxStyle.Critical)
            Exit Function
        End If
        strAcctName = InputBox("Title for new account:", "New Account")
        If strAcctName = "" Then
            Exit Function
        End If
        gCreateAccount(strFileRoot, strAcctName, "Main Register")
        gblnAskAndCreateAccount = True

    End Function

    Public Sub gCreateAccount(ByVal strFileRoot As String, ByVal strAcctTitle As String, ByVal strRegTitle As String)

        Dim intFile As Short
        Dim strFile As String

        strFile = gstrAccountPath() & "\" & strFileRoot & ".act"
        intFile = FreeFile()
        FileOpen(intFile, strFile, OpenMode.Output)

        PrintLine(intFile, "FHCKBK2")
        PrintLine(intFile, "AT" & strAcctTitle)
        PrintLine(intFile, "RK1")
        PrintLine(intFile, "RT" & strRegTitle)
        PrintLine(intFile, "RS")
        PrintLine(intFile, "RI")
        PrintLine(intFile, "RL1")
        PrintLine(intFile, ".R")
        PrintLine(intFile, "RF1")
        PrintLine(intFile, ".R")
        PrintLine(intFile, "RR1")
        PrintLine(intFile, ".R")
        PrintLine(intFile, ".A")

        FileClose(intFile)

        strFile = gstrAccountPath() & "\" & strFileRoot & ".rep"
        intFile = FreeFile()
        FileOpen(intFile, strFile, OpenMode.Output)

        'Note: Keep the first line up to date!
        PrintLine(intFile, "Last used: 14")
        PrintLine(intFile, "/01/Paycheck 1/Paycheck 1")
        PrintLine(intFile, "/02/Paycheck 2/Paycheck 2")
        PrintLine(intFile, "/03/Mortgage/Mortgage")
        PrintLine(intFile, "/04/Grocery Budget/Grocery Budget")
        PrintLine(intFile, "/05/Car Payment 1/Car Payment 1")
        PrintLine(intFile, "/06/Car Payment 2/Car Payment 2")
        PrintLine(intFile, "/07/Credit Card Payment 1/Credit Card Payment 1")
        PrintLine(intFile, "/08/Credit Card Payment 2/Credit Card Payment 2")
        PrintLine(intFile, "/09/Telephone/Telephone")
        PrintLine(intFile, "/10/Electricity/Electricity")
        PrintLine(intFile, "/11/Oil-Natural Gas/Oil-Natural Gas")
        PrintLine(intFile, "/12/Vacation Savings/Vacation Savings")
        PrintLine(intFile, "/13/Fed Income Tax Savings/Fed Income Tax Savings")
        PrintLine(intFile, "/14/State Income Tax Savings/State Income Tax Savings")

        FileClose(intFile)

    End Sub

    Public Sub gLoadAccountListBox(ByVal lst As System.Windows.Forms.ListBox)
        Dim objAccount As Account

        With lst
            .Items.Clear()
            For Each objAccount In gcolAccounts
                .Items.Add(objAccount.strTitle)
            Next objAccount
        End With
    End Sub

    Public Function gobjGetSelectedAccountAndUnload(ByVal lst As System.Windows.Forms.ListBox, ByVal frm As System.Windows.Forms.Form) As Account

        If lst.SelectedIndex = -1 Then
            gobjGetSelectedAccountAndUnload = Nothing
            Exit Function
        End If
        gobjGetSelectedAccountAndUnload = gcolAccounts.Item(lst.SelectedIndex)
        frm.Close()
        System.Windows.Forms.Application.DoEvents()
    End Function

    Public Sub gCreateStandardFolders()
        Try

            If Dir(gstrDataPath(), FileAttribute.Directory) = "" Then
                MkDir(gstrDataPath())
            End If
            If Dir(gstrAccountPath(), FileAttribute.Directory) = "" Then
                MkDir(gstrAccountPath())
            End If
            If Dir(gstrBackupPath(), FileAttribute.Directory) = "" Then
                MkDir(gstrBackupPath())
            End If
            If Dir(gstrReportPath(), FileAttribute.Directory) = "" Then
                MkDir(gstrReportPath())
            End If

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public Sub gCreateStandardFiles()
        Dim strFile As String
        Dim intFile As Short

        Try

            'Standard category file
            strFile = gstrAddPath("Shared.cat")
            If Dir(strFile, FileAttribute.Normal) = "" Then
                MsgBox("Creating standard category list, which you can edit later...", MsgBoxStyle.Information)
                intFile = FreeFile()
                FileOpen(intFile, strFile, OpenMode.Output)
                'Note: Keep the first line up to date!
                PrintLine(intFile, "Last used: 40")
                PrintLine(intFile, "/001/I/Income")
                PrintLine(intFile, "/002/I:Interest/ Interest")
                PrintLine(intFile, "/003/I:Wages/ Wages")
                PrintLine(intFile, "/004/I:Bonus/ Bonus")
                PrintLine(intFile, "/005/I:Other/ Other")
                PrintLine(intFile, "/006/I:Sales/ Sales")
                PrintLine(intFile, "/007/I:Draw/ Draw")
                PrintLine(intFile, "/008/I:Gift/ Gift")
                PrintLine(intFile, "/009/E/Expense")
                PrintLine(intFile, "/010/E:Cable TV/ Cable TV")
                PrintLine(intFile, "/011/E:Car/ Car")
                PrintLine(intFile, "/012/E:Car:Gasoline/  Gasoline")
                PrintLine(intFile, "/013/E:Car:Payment/  Car Payment")
                PrintLine(intFile, "/014/E:Car:Repair/  Car Repair")
                PrintLine(intFile, "/015/E:Charity/ Charity")
                PrintLine(intFile, "/016/E:Cleaning/ Cleaning")
                PrintLine(intFile, "/017/E:Clothing/ Clothing")
                PrintLine(intFile, "/018/E:Credit Cards/ Credit Cards")
                PrintLine(intFile, "/019/E:Entertainment/ Entertainment")
                PrintLine(intFile, "/020/E:Groceries/ Groceries")
                PrintLine(intFile, "/021/E:Home/ Home")
                PrintLine(intFile, "/022/E:Home:Mortgage/  Mortgage")
                PrintLine(intFile, "/023/E:Home:Repair/  Home Repair")
                PrintLine(intFile, "/024/E:Internet/ Internet")
                PrintLine(intFile, "/025/E:Medical/ Medical")
                PrintLine(intFile, "/026/E:Medical:Insurance/  Insurance")
                PrintLine(intFile, "/027/E:Medical:Office Visits/  Office Visits")
                PrintLine(intFile, "/028/E:Medical:Prescriptions/  Prescriptions")
                PrintLine(intFile, "/029/E:Miscellaneous/ Miscellaneous")
                PrintLine(intFile, "/030/E:Taxes/ Taxes")
                PrintLine(intFile, "/031/E:Taxes:Federal Income/  Federal Income")
                PrintLine(intFile, "/032/E:Taxes:Local Income/  Local Income")
                PrintLine(intFile, "/033/E:Taxes:Property/  Property")
                PrintLine(intFile, "/034/E:Taxes:State Income/  State Income")
                PrintLine(intFile, "/035/E:Util/ Utilities")
                PrintLine(intFile, "/036/E:Util:Electric/  Electricity")
                PrintLine(intFile, "/037/E:Util:Oil/  Fuel Oil")
                PrintLine(intFile, "/040/E:Util:Natural Gas/  Natural Gas")
                PrintLine(intFile, "/039/E:Util:Phone/  Phone")
                PrintLine(intFile, "/038/E:Util:Water/  Water")
                FileClose(intFile)
            End If

            'Standard budget file
            strFile = gstrAddPath("Shared.bud")
            If Dir(strFile, FileAttribute.Normal) = "" Then
                MsgBox("Creating standard budget list, which you can edit later...", MsgBoxStyle.Information)
                intFile = FreeFile()
                FileOpen(intFile, strFile, OpenMode.Output)
                'Note: Keep the first line up to date!
                PrintLine(intFile, "Last used: 03")
                PrintLine(intFile, "/01/Groceries/Groceries")
                PrintLine(intFile, "/02/Clothing/Clothing")
                PrintLine(intFile, "/03/Entertainment/Entertainment")
                FileClose(intFile)
            End If

            'Standard payee file
            strFile = gstrPayeeFilePath()
            If Dir(strFile, FileAttribute.Normal) = "" Then
                intFile = FreeFile()
                FileOpen(intFile, strFile, OpenMode.Output)
                'Note: Keep the first line up to date!
                PrintLine(intFile, "<Table>")
                PrintLine(intFile, "</Table>")
                FileClose(intFile)
            End If

            'Standard QIF import transaction types file
            strFile = gstrTrxTypeFilePath()
            If Dir(strFile, FileAttribute.Normal) = "" Then
                intFile = FreeFile()
                FileOpen(intFile, strFile, OpenMode.Output)
                'Note: Keep the first line up to date!
                PrintLine(intFile, "<Table>")
                PrintLine(intFile, "</Table>")
                FileClose(intFile)
            End If

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Path and name of trx type translation file.

    Public Function gstrTrxTypeFilePath() As Object
        gstrTrxTypeFilePath = gstrAddPath("QIFImportTrxTypes.xml")
    End Function

    Public Sub gInitPayeeList(ByVal lvwPayees As System.Windows.Forms.ListView)
        With lvwPayees
            .Columns.Clear()
            .Columns.Add("", "Number", 55)
            .Columns.Add("", "Name/Description", 235)
            .Columns.Add("", "Category", 235)
            .Columns.Add("", "Amount", 65)
            .Columns.Add("", "Budget", 130)
            .Columns.Add("", "Memo", 200)
            .View = System.Windows.Forms.View.Details
            .FullRowSelect = True
            .HideSelection = False
            .Sort()
            .LabelEdit = False
        End With
        gDisablePayeeListSorting(lvwPayees)
    End Sub

    Public Sub gDisablePayeeListSorting(ByVal lvwPayees As System.Windows.Forms.ListView)
        gSetListViewSortColumn(lvwPayees, 0)
    End Sub

    Public Sub gSortPayeeListByName(ByVal lvwPayees As System.Windows.Forms.ListView)
        gSetListViewSortColumn(lvwPayees, 1)
    End Sub

    Public Function gobjCreatePayeeListItem(ByVal elmPayee As VB6XmlElement, ByVal lvwPayees As System.Windows.Forms.ListView, ByVal intIndex As Short) As System.Windows.Forms.ListViewItem

        Dim objItem As System.Windows.Forms.ListViewItem
        Dim elmNum As VB6XmlElement
        Dim strNum As String

        elmNum = elmPayee.SelectSingleNode("Num")
        If elmNum Is Nothing Then
            strNum = ""
        Else
            strNum = elmNum.Text
        End If
        gDisablePayeeListSorting(lvwPayees)
        objItem = lvwPayees.Items.Add(strNum)
        objItem.Tag = CStr(intIndex)
        If objItem.SubItems.Count > 1 Then
            objItem.SubItems(1).Text = elmPayee.GetAttribute("Output")
        Else
            objItem.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, elmPayee.GetAttribute("Output")))
        End If
        SetPayeeSubItem(objItem, 2, elmPayee, "Cat")
        SetPayeeSubItem(objItem, 3, elmPayee, "Amount")
        SetPayeeSubItem(objItem, 4, elmPayee, "Budget")
        SetPayeeSubItem(objItem, 5, elmPayee, "Memo")
        gobjCreatePayeeListItem = objItem
    End Function

    Private Sub SetPayeeSubItem(ByVal objItem As System.Windows.Forms.ListViewItem, ByVal intSubItem As Short, ByVal elmPayee As VB6XmlElement, ByVal strChildName As String)

        Dim elmChild As VB6XmlElement
        Dim strText As String

        elmChild = elmPayee.SelectSingleNode(strChildName)
        If elmChild Is Nothing Then
            strText = ""
        Else
            strText = elmChild.Text
        End If
        If objItem.SubItems.Count > intSubItem Then
            objItem.SubItems(intSubItem).Text = strText
        Else
            objItem.SubItems.Insert(intSubItem, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strText))
        End If
    End Sub

    Public Sub gLoadComboFromStringTranslator(ByVal cbo As System.Windows.Forms.ComboBox, ByVal objList As StringTranslator, ByVal blnAddEmpty As Boolean)

        Try

            Dim intIndex As Short
            With cbo
                .Items.Clear()
                If blnAddEmpty Then
                    .Items.Add(gobjCreateListBoxItem("", 0))
                End If
                For intIndex = 1 To objList.intElements
                    .Items.Add(gobjCreateListBoxItem(objList.strValue1(intIndex), intIndex))
                Next
            End With

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public Sub gLoadMatchNarrowingMethods(ByVal cbo As ComboBox)
        cbo.Items.Clear()
        cbo.Items.Add(gobjCreateListBoxItem("None", ImportMatchNarrowMethod.None))
        cbo.Items.Add(gobjCreateListBoxItem("Closest Date", ImportMatchNarrowMethod.ClosestDate))
        cbo.Items.Add(gobjCreateListBoxItem("Earliest Date", ImportMatchNarrowMethod.EarliestDate))
    End Sub

    Public Sub gGetSplitDates(ByVal objTrx As Trx, ByVal objSplit As TrxSplit, ByRef datInvoiceDate As Date, ByRef datDueDate As Date)

        datDueDate = objSplit.datDueDate
        If datDueDate = System.DateTime.FromOADate(0) Then
            datDueDate = objTrx.datDate
        End If
        datInvoiceDate = objSplit.datInvoiceDate
        Dim intDaysBack As Short
        Dim strTerms As String
        If datInvoiceDate = System.DateTime.FromOADate(0) Then
            'Estimate invoice date from due date.
            strTerms = LCase(objSplit.strTerms)
            strTerms = Replace(strTerms, " ", "")
            If InStr(strTerms, "net10") > 0 Then
                intDaysBack = 10
            ElseIf InStr(strTerms, "net15") > 0 Then
                intDaysBack = 15
            ElseIf InStr(strTerms, "net20") > 0 Then
                intDaysBack = 20
            ElseIf InStr(strTerms, "net25") > 0 Then
                intDaysBack = 25
            Else
                'Is the category one we guessed to have short terms?
                If InStr(gstrShortTermsCatKeys, gstrEncodeCatKey(objSplit.strCategoryKey)) > 0 Then
                    intDaysBack = 14
                Else
                    intDaysBack = 30
                End If
            End If
            datInvoiceDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDaysBack, datDueDate)
        End If
    End Sub

    Public Sub gProcessSecurityOption(ByVal strSecurityOption As String)
        Dim objSec As Security
        Dim strFile As String
        Dim strMasterPassword As String
        Dim strLogin As String
        Dim strName As String
        Dim strPassword As String

        strMasterPassword = InputBox("Please enter master password:", "Master Password")
        'TO DO: Compare to a master password hash in a license file of some kind.
        If strMasterPassword <> "chickenshit" Then
            MsgBox("Invalid master password")
            Exit Sub
        End If

        objSec = New Security

        If strSecurityOption = "createfile" Then
            'Make standard security file.
            strFile = objSec.strMakePath(objSec.strDefaultFileName)
            If Dir(strFile, FileAttribute.Normal) = "" Then
                objSec.CreateEmpty(objSec.strDefaultFileName)
                objSec.CreateUser("admin", "Administrator")
                objSec.SetPassword("")
                objSec.CreateSignatures()
                objSec.Save()
                MsgBox("New security file created, with ""admin"" login and no password.")
                Exit Sub
            Else
                MsgBox("Security file already exists.")
                Exit Sub
            End If
        End If

        If strSecurityOption = "createuser" Then
            'Create new user.
            strLogin = InputBox("Please enter new login name:", "New Login")
            If strLogin = "" Then
                Exit Sub
            End If
            strName = InputBox("Please enter person's name:", "Person Name")
            If strName = "" Then
                Exit Sub
            End If
            objSec.Load(objSec.strDefaultFileName)
            If objSec.blnFindUser(strLogin) Then
                MsgBox("Login name already exists.")
                Exit Sub
            End If
            objSec.CreateUser(strLogin, strName)
            objSec.SetPassword("")
            objSec.CreateSignatures()
            objSec.Save()
            MsgBox("New login """ & strLogin & """ created with no password.")
            Exit Sub
        End If

        If strSecurityOption = "setpassword" Then
            'Set password on existing user.
            strLogin = InputBox("Please enter existing login name:", "Existing Login")
            If strLogin = "" Then
                Exit Sub
            End If
            objSec.Load(objSec.strDefaultFileName)
            If Not objSec.blnFindUser(strLogin) Then
                MsgBox("Login name does not exist.")
                Exit Sub
            End If
            strPassword = InputBox("Please enter new password for """ & strLogin & """:", "New Password")
            If strPassword = "" Then
                Exit Sub
            End If
            objSec.SetPassword(strPassword)
            objSec.CreateSignatures()
            objSec.Save()
            MsgBox("Password set for login """ & strLogin & """.")
            Exit Sub
        End If

        If strSecurityOption = "signfile" Then
            'Sign the security file.
            objSec.Load(objSec.strDefaultFileName)
            objSec.CreateSignatures()
            objSec.Save()
            MsgBox("Security file signed.")
            Exit Sub
        End If

        MsgBox("Unrecognized ""/security:"" option.")

    End Sub
End Module