Option Strict On
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
    Public Const gintLISTITEM_LOWINDEX As Integer = 0

    Private Class BackupPurgeDay
        Public colInstances As List(Of BackupInstance)
    End Class

    Private Class BackupInstance
        Public datCreate As Date
        Public strName As String
    End Class

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

    Public Sub gAddListSubItem(ByVal objItem As ListViewItem, ByVal intColIndex As Integer, ByVal strValue As String)
        If objItem.SubItems.Count > intColIndex Then
            objItem.SubItems(intColIndex).Text = strValue
        Else
            objItem.SubItems.Insert(intColIndex, New ListViewItem.ListViewSubItem(Nothing, strValue))
        End If
    End Sub

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

    Public Function gblnUserAuthenticated(ByVal objSecurity As Security) As Boolean
        Try
            Dim strLogin As String
            Dim strPassword As String
            Dim frmLogin As LoginForm

            strLogin = ""
            strPassword = ""
            frmLogin = New LoginForm
            If Not frmLogin.blnGetCredentials(strLogin, strPassword) Then
                Return False
            End If

            If Not objSecurity.blnFindUser(strLogin) Then
                MsgBox("Invalid login or password")
                Return False
            End If
            If Not objSecurity.blnPasswordMatches(strPassword) Then
                MsgBox("Invalid login or password")
                Return False
            End If
            If Not objSecurity.blnUserSignatureIsValid Then
                MsgBox("User data is invalid")
                Return False
            End If

            Return True
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Friend Sub gShowRegister(ByVal objAccount As Account, ByVal objReg As Register, ByVal frmStartup As StartupForm)

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
        frmReg.ShowMe(objReg, frmStartup)
    End Sub

    Public Sub gSaveChangedAccounts(ByVal objCompany As Company)
        Dim objAccount As Account
        Dim strBackupFile As String
        For Each objAccount In objCompany.colAccounts
            If objAccount.blnUnsavedChanges Then
                strBackupFile = objCompany.strBackupPath() & "\" & objAccount.strFileNameRoot & "." & Now.ToString("MM$dd$yy$hh$mm")
                If Len(Dir(strBackupFile)) > 0 Then
                    Kill(strBackupFile)
                End If
                Rename(objCompany.strAccountPath() & "\" & objAccount.strFileNameRoot, strBackupFile)
                PurgeAccountBackups(objAccount)
                objAccount.Save(objAccount.strFileNameRoot)
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
        Dim intIndex As Integer
        Dim intBackupsToKeep As Integer
        Dim intAgeInDays As Integer
        Dim colInstances As List(Of BackupInstance)
        Dim colOlderFiles As List(Of String) = New List(Of String)

        Try

            For intIndex = LBound(adatDays) To UBound(adatDays)
                adatDays(intIndex) = New BackupPurgeDay()
                adatDays(intIndex).colInstances = New List(Of BackupInstance)
            Next

            strBackup = Dir(objAccount.objCompany.strBackupPath() & "\" & objAccount.strFileNameRoot & ".*", FileAttribute.Normal)
            Do While strBackup <> ""
                strEncodedDate = Mid(strBackup, InStr(UCase(strBackup), ".ACT.") + 5)
                strParsableDate = "20" & Mid(strEncodedDate, 7, 2) & "/" & Mid(strEncodedDate, 1, 2) & "/" & Mid(strEncodedDate, 4, 2) & " " & Mid(strEncodedDate, 10, 2) & ":" & Mid(strEncodedDate, 13, 2)
                datCreateDate = CDate(strParsableDate)
                intAgeInDays = CInt(DateDiff(Microsoft.VisualBasic.DateInterval.Day, datCreateDate, Today))
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
                Kill(objAccount.objCompany.strBackupPath() & "\" & strBackup)
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
                    Kill(objAccount.objCompany.strBackupPath() & "\" & strBackup)
                Next
            Next

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Function BackupInstanceComparer(ByVal i1 As BackupInstance, ByVal i2 As BackupInstance) As Integer
        Return i1.datCreate.CompareTo(i2.datCreate)
    End Function

    Public Function gblnAskAndCreateAccount(ByVal objCompany As Company) As Boolean
        Dim objAccount As Account
        Dim strFile As String

        objAccount = New Account()
        objAccount.Init(objCompany)
        objAccount.intKey = objCompany.intGetUnusedAccountKey()
        objAccount.lngSubType = Account.SubType.Liability_LoanPayable

        Using frm As AccountForm = New AccountForm()
            If frm.ShowDialog(objAccount, False, False) = DialogResult.OK Then
                strFile = objCompany.strAccountPath() & "\" & objAccount.strFileNameRoot & ".act"
                If Dir(strFile) <> "" Then
                    MsgBox("Account file already exists with that name.", MsgBoxStyle.Critical)
                    Exit Function
                End If
                gCreateAccount(objAccount)
                Return True
            End If
        End Using
        Return False
    End Function

    Public Sub gCreateAccount(ByVal objAccount As Account)

        Dim intFile As Integer
        Dim strFile As String
        Dim objSubTypeMatched As Account.SubTypeDef = Nothing

        For Each objSubType As Account.SubTypeDef In Account.arrSubTypeDefs
            If objSubType.lngSubType = objAccount.lngSubType Then
                objSubTypeMatched = objSubType
            End If
        Next
        If objSubTypeMatched Is Nothing Then
            Throw New Exception("Unrecognized account subtype")
        End If

        strFile = objAccount.objCompany.strAccountPath() & "\" & objAccount.strFileNameRoot & ".act"
        intFile = FreeFile()
        FileOpen(intFile, strFile, OpenMode.Output)

        PrintLine(intFile, "FHCKBK2")
        PrintLine(intFile, "AT" & objAccount.strTitle)
        PrintLine(intFile, "AK" & CStr(objAccount.intKey))
        PrintLine(intFile, "AY" & objSubTypeMatched.strSaveCode)
        PrintLine(intFile, "RK1")
        PrintLine(intFile, "RT" & objAccount.strTitle)
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

        strFile = objAccount.objCompany.strAccountPath() & "\" & objAccount.strFileNameRoot & ".rep"
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

    Public Sub gLoadAccountListBox(ByVal lst As System.Windows.Forms.ListBox, ByVal objCompany As Company)
        Dim objAccount As Account

        With lst
            .Items.Clear()
            For Each objAccount In objCompany.colAccounts
                .Items.Add(objAccount.strTitle)
            Next objAccount
        End With
    End Sub

    Public Function gobjGetSelectedAccountAndUnload(ByVal lst As ListBox, ByVal frm As Form, ByVal objCompany As Company) As Account

        If lst.SelectedIndex = -1 Then
            gobjGetSelectedAccountAndUnload = Nothing
            Exit Function
        End If
        gobjGetSelectedAccountAndUnload = objCompany.colAccounts.Item(lst.SelectedIndex)
        frm.Close()
        System.Windows.Forms.Application.DoEvents()
    End Function

    Public Sub gCreateStandardFolders(ByVal objCompany As Company)
        Try

            If Dir(objCompany.strDataPath(), FileAttribute.Directory) = "" Then
                MkDir(objCompany.strDataPath())
            End If
            If Dir(objCompany.strAccountPath(), FileAttribute.Directory) = "" Then
                MkDir(objCompany.strAccountPath())
            End If
            If Dir(objCompany.strBackupPath(), FileAttribute.Directory) = "" Then
                MkDir(objCompany.strBackupPath())
            End If
            If Dir(objCompany.strReportPath(), FileAttribute.Directory) = "" Then
                MkDir(objCompany.strReportPath())
            End If

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public Sub gCreateStandardFiles(ByVal objCompany As Company)
        Dim strFile As String
        Dim intFile As Integer

        Try

            'Standard category file
            strFile = objCompany.strCategoryPath()
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
            strFile = objCompany.strBudgetPath()
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
            strFile = objCompany.strPayeeFilePath()
            If Dir(strFile, FileAttribute.Normal) = "" Then
                intFile = FreeFile()
                FileOpen(intFile, strFile, OpenMode.Output)
                'Note: Keep the first line up to date!
                PrintLine(intFile, "<Table>")
                PrintLine(intFile, "</Table>")
                FileClose(intFile)
            End If

            'Standard QIF import transaction types file
            strFile = objCompany.strTrxTypeFilePath()
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

    Public Sub gCreateStandardCheckingAccount(ByVal objCompany As Company)
        Try
            MsgBox("Creating first checking account...", MsgBoxStyle.Information)
            Dim objAccount As Account = New Account()
            objAccount.Init(objCompany)
            objAccount.intKey = objCompany.intGetUnusedAccountKey()
            objAccount.lngSubType = Account.SubType.Asset_CheckingAccount
            objAccount.strFileNameRoot = "Main"
            objAccount.strTitle = "Checking Account"
            gCreateAccount(objAccount)
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public Sub gInitPayeeList(ByVal lvwPayees As System.Windows.Forms.ListView)
        With lvwPayees
            .Columns.Clear()
            .Columns.Add("", "Number", 55)
            .Columns.Add("", "Name/Description", 200)
            .Columns.Add("", "Category", 160)
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

        elmNum = DirectCast(elmPayee.SelectSingleNode("Num"), VB6XmlElement)
        If elmNum Is Nothing Then
            strNum = ""
        Else
            strNum = elmNum.Text
        End If
        gDisablePayeeListSorting(lvwPayees)
        objItem = lvwPayees.Items.Add(strNum)
        objItem.Tag = CStr(intIndex)
        If objItem.SubItems.Count > 1 Then
            objItem.SubItems(1).Text = CStr(elmPayee.GetAttribute("Output"))
        Else
            objItem.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, CStr(elmPayee.GetAttribute("Output"))))
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

        elmChild = DirectCast(elmPayee.SelectSingleNode(strChildName), VB6XmlElement)
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

    Public Sub gLoadComboFromStringTranslator(ByVal cbo As System.Windows.Forms.ComboBox, ByVal objList As IStringTranslator, ByVal blnAddEmpty As Boolean)

        Try

            Dim intIndex As Integer
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
                If InStr(gstrShortTermsCatKeys, Company.strEncodeCatKey(objSplit.strCategoryKey)) > 0 Then
                    intDaysBack = 14
                Else
                    intDaysBack = 30
                End If
            End If
            datInvoiceDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, -intDaysBack, datDueDate)
        End If
    End Sub

End Module