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

    Private Structure BackupPurgeDay
        Dim colCreateDates As Collection
        Dim colNames As Collection
    End Structure

    'Type of search used to recognize trx that have already been imported.
    Public Enum ImportStatusSearch
        glngIMPSTATSRCH_BANK = 1 'Bank import key.
        glngIMPSTATSRCH_PAYNONGEN = 2 'Payee name and non-generated trx.
        glngIMPSTATSRCH_VENINV = 3 'Vendor name and invoice number.
    End Enum

    'Type of search used to find Trx matches for batch updates during import.
    Public Enum ImportBatchUpdateSearch
        glngIMPBATUPSR_NONE = 1 'None
        glngIMPBATUPSR_BANK = 2 'Number, or date/description/amount (2 of 3)
        glngIMPBATUPSR_PAYEE = 3 'Exact payee name within date range
    End Enum

    'Type of search used to find Trx matches for batch new Trx creation during import.
    Public Enum ImportBatchNewSearch
        glngIMPBATNWSR_NONE = 1 'Do not allow batch creation of new Trx.
        glngIMPBATNWSR_BANK = 2 'Number, or date/description/amount (2 of 3).
        glngIMPBATNWSR_VENINV = 3 'Vendor name and invoice number.
    End Enum

    'Fields to update during batch import.
    Public Enum ImportBatchUpdateType
        glngIMPBATUPTP_NONE = 1 'Do not allow batch updates during import.
        glngIMPBATUPTP_BANK = 2 'Bank import key ONLY.
        glngIMPBATUPTP_AMOUNT = 3 'Amount only.
    End Enum

    'Type of search used when user clicks on an imported trx.
    Public Enum ImportIndividualSearchType
        glngIMPINDSRTP_BANK = 1 'Number, or date/description/amount (2 of 3)
        glngIMPINDSRTP_PAYEE = 2 'Exact payee name within date range
        glngIMPINDSRTP_VENINV = 3 'Vendor name and invoice number.
    End Enum

    'Fields to update during individual import.
    Public Enum ImportIndividualUpdateType
        glngIMPINDUPTP_NONE = 1 'None.
        glngIMPINDUPTP_BANK = 2 'Bank import key, number, amount.
        glngIMPINDUPTP_AMOUNT = 3 'Amount only.
    End Enum

    'UPGRADE_WARNING: Application will terminate when Sub Main() finishes. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="E08DDC71-66BA-424F-A612-80AF11498FF8"'
    Public Sub Main()
        CBMainForm.Show()
    End Sub

    Private Sub TopError(ByVal strRoutine As String)
        gTopErrorTrap("CBMain." & strRoutine)
    End Sub

    Private Sub NestedError(ByVal strRoutine As String)
        gNestedErrorTrap("CBMain." & strRoutine)
    End Sub

    Public Function gcolForms() As Collection
        Dim frm As System.Windows.Forms.Form
        Dim colResult As Collection
        colResult = New Collection
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

    Public Function gblnDataIsLocked() As Boolean
        On Error GoTo ErrorHandler
        Dim intLockFile As Short
        intLockFile = FreeFile()
        FileOpen(intLockFile, gstrAddPath("LockFile.dat"), OpenMode.Append, OpenAccess.Write, OpenShare.LockWrite)
        gblnDataIsLocked = False
        Exit Function
ErrorHandler:
        If Err.Number = 70 Then
            gblnDataIsLocked = True
            Exit Function
        End If
        Err.Raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function gblnUserAuthenticated() As Boolean
        On Error GoTo ErrorHandler
        Dim strLogin As String
        Dim strPassword As String
        Dim frmLogin As LoginForm

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
ErrorHandler:
        NestedError("gblnUserAuthenticated")
    End Function

    Friend Sub gShowRegister(ByVal objAccount As Account, ByVal objReg As Register, ByVal frmStartup As StartupForm)

        Dim frm As System.Windows.Forms.Form
        Dim frmReg As RegisterForm

        For Each frm In gcolForms()
            'UPGRADE_WARNING: TypeOf has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
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
                strBackupFile = gstrBackupPath() & "\" & objAccount.strFileLoaded & "." & VB6.Format(Now, "mm$dd$yy$hh$mm")
                'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
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
        'Backups older than the upper bound of this array (in days)
        'will be deleted.
        Dim adatDays(30) As BackupPurgeDay
        Dim strBackup As String
        Dim vntOldDate As Object
        Dim strParsableDate As String
        Dim datCreateDate As Date
        Dim strEncodedDate As String
        Dim intIndex As Short
        Dim blnFound As Boolean
        Dim intBackupsToKeep As Short
        Dim intAgeInDays As Short

        On Error GoTo ErrorHandler

        For intIndex = LBound(adatDays) To UBound(adatDays)
            adatDays(intIndex).colCreateDates = New Collection
            adatDays(intIndex).colNames = New Collection
        Next

        'For each day in the last "n" days, build twin collections of backup file
        'names and backup save dates for backups created on those days, for each
        'day sorted in increasing backup save time.
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        strBackup = Dir(gstrBackupPath() & "\" & objAccount.strFileLoaded & ".*", FileAttribute.Normal)
        Do While strBackup <> ""
            strEncodedDate = Mid(strBackup, InStr(UCase(strBackup), ".ACT.") + 5)
            strParsableDate = "20" & Mid(strEncodedDate, 7, 2) & "/" & Mid(strEncodedDate, 1, 2) & "/" & Mid(strEncodedDate, 4, 2) & " " & Mid(strEncodedDate, 10, 2) & ":" & Mid(strEncodedDate, 13, 2)
            datCreateDate = CDate(strParsableDate)
            'UPGRADE_WARNING: DateDiff behavior may be different. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B38EC3F-686D-4B2E-B5A5-9E8E7A762E32"'
            intAgeInDays = DateDiff(Microsoft.VisualBasic.DateInterval.Day, datCreateDate, Today)
            If intAgeInDays <= UBound(adatDays) Then
                blnFound = False
                For intIndex = 1 To adatDays(intAgeInDays).colCreateDates.Count()
                    'UPGRADE_WARNING: Couldn't resolve default property of object adatDays(intAgeInDays).colCreateDates(intIndex). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    If datCreateDate < adatDays(intAgeInDays).colCreateDates.Item(intIndex) Then
                        adatDays(intAgeInDays).colCreateDates.Add(datCreateDate, , intIndex)
                        adatDays(intAgeInDays).colNames.Add(strBackup, , intIndex)
                        blnFound = True
                        Exit For
                    End If
                Next
                If Not blnFound Then
                    adatDays(intAgeInDays).colCreateDates.Add(datCreateDate)
                    adatDays(intAgeInDays).colNames.Add(strBackup)
                End If
            End If
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            strBackup = Dir()
        Loop

        'Delete everything but the "intBackupsToKeep" most recent backups
        'created on each date.
        For intAgeInDays = 0 To UBound(adatDays)
            If intAgeInDays = 0 Then
                'Keep all backups from the current date.
                intBackupsToKeep = 100
            ElseIf intAgeInDays < 5 Then
                intBackupsToKeep = 10
            Else
                intBackupsToKeep = 1
            End If
            For intIndex = 1 To adatDays(intAgeInDays).colNames.Count() - intBackupsToKeep
                'UPGRADE_WARNING: Couldn't resolve default property of object adatDays().colNames(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                strBackup = adatDays(intAgeInDays).colNames.Item(intIndex)
                Kill(gstrBackupPath() & "\" & strBackup)
            Next
        Next

        Exit Sub
ErrorHandler:
        NestedError("PurgeAccountBackups")
    End Sub

    Public Function gblnAskAndCreateAccount() As Boolean
        Dim strFileRoot As String
        Dim strFile As String
        Dim strAcctName As String

        strFileRoot = InputBox("File name for new account:", "New Account")
        If strFileRoot = "" Then
            Exit Function
        End If
        strFile = gstrAccountPath() & "\" & strFileRoot & ".act"
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
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
            Exit Function
        End If
        gobjGetSelectedAccountAndUnload = gcolAccounts.Item(lst.SelectedIndex + 1)
        frm.Close()
        System.Windows.Forms.Application.DoEvents()
    End Function

    Public Sub gCreateStandardFolders()
        On Error GoTo ErrorHandler

        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(gstrDataPath(), FileAttribute.Directory) = "" Then
            MkDir(gstrDataPath())
        End If
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(gstrAccountPath(), FileAttribute.Directory) = "" Then
            MkDir(gstrAccountPath())
        End If
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(gstrBackupPath(), FileAttribute.Directory) = "" Then
            MkDir(gstrBackupPath())
        End If
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(gstrReportPath(), FileAttribute.Directory) = "" Then
            MkDir(gstrReportPath())
        End If

        Exit Sub
ErrorHandler:
        NestedError("CreateStandardFolders")
    End Sub

    Public Sub gCreateStandardFiles()
        Dim strFile As String
        Dim intFile As Short

        On Error GoTo ErrorHandler

        'Standard category file
        strFile = gstrAddPath("Shared.cat")
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
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
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
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
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(strFile, FileAttribute.Normal) = "" Then
            intFile = FreeFile()
            FileOpen(intFile, strFile, OpenMode.Output)
            'Note: Keep the first line up to date!
            PrintLine(intFile, "<Table>")
            PrintLine(intFile, "</Table>")
            FileClose(intFile)
        End If

        'Standard QIF import transaction types file
        'UPGRADE_WARNING: Couldn't resolve default property of object gstrTrxTypeFilePath(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        strFile = gstrTrxTypeFilePath()
        'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
        If Dir(strFile, FileAttribute.Normal) = "" Then
            intFile = FreeFile()
            FileOpen(intFile, strFile, OpenMode.Output)
            'Note: Keep the first line up to date!
            PrintLine(intFile, "<Table>")
            PrintLine(intFile, "</Table>")
            FileClose(intFile)
        End If

        Exit Sub
ErrorHandler:
        NestedError("CreateStandardFiles")
    End Sub

    '$Description Path and name of trx type translation file.

    Public Function gstrTrxTypeFilePath() As Object
        gstrTrxTypeFilePath = gstrAddPath("QIFImportTrxTypes.xml")
    End Function

    Public Sub gInitPayeeList(ByVal lvwPayees As System.Windows.Forms.ListView)
        With lvwPayees
            .Columns.Clear()
            .Columns.Add("", "Number", CInt(VB6.TwipsToPixelsX(800)))
            .Columns.Add("", "Name/Description", CInt(VB6.TwipsToPixelsX(3500)))
            .Columns.Add("", "Category", CInt(VB6.TwipsToPixelsX(3500)))
            .Columns.Add("", "Amount", CInt(VB6.TwipsToPixelsX(1000)))
            .Columns.Add("", "Budget", CInt(VB6.TwipsToPixelsX(2000)))
            .Columns.Add("", "Memo", CInt(VB6.TwipsToPixelsX(3000)))
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
        'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        'UPGRADE_WARNING: Couldn't resolve default property of object elmPayee.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
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
        'UPGRADE_WARNING: Lower bound of collection objItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
        If objItem.SubItems.Count > intSubItem Then
            objItem.SubItems(intSubItem).Text = strText
        Else
            objItem.SubItems.Insert(intSubItem, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, strText))
        End If
    End Sub

    Public Sub gLoadComboFromStringTranslator(ByVal cbo As System.Windows.Forms.ComboBox, ByVal objList As StringTranslator, ByVal blnAddEmpty As Boolean)

        On Error GoTo ErrorHandler

        Dim intIndex As Short
        With cbo
            .Items.Clear()
            If blnAddEmpty Then
                .Items.Add(New VB6.ListBoxItem("", 0))
            End If
            For intIndex = 1 To objList.intElements
                .Items.Add(New VB6.ListBoxItem(objList.strValue1(intIndex), intIndex))
            Next
        End With

        Exit Sub
ErrorHandler:
        NestedError("gLoadComboFromStringTranslator")
    End Sub

    Public Sub gGetSplitDates(ByVal objTrx As Trx, ByVal objSplit As Split_Renamed, ByRef datInvoiceDate As Date, ByRef datDueDate As Date)

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
            'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
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