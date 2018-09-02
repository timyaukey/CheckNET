Option Strict On
Option Explicit On

Imports System.IO
Imports System.Xml.Serialization

''' <summary>
''' The core class of the object model. One instance contains all the data objects
''' used by a company, including the list of general ledger accounts, and a bunch
''' of other non-transactional definition data such as the category list.
''' The simplest usage is to create an instance passing the path to the data file
''' folder, and then call Load() to load all the accounts and everything else.
''' You do not HAVE to call methods that check for files being in use, or authenticate
''' the user.
''' </summary>

Public Class Company

    Public Event SomethingModified()

    Public ReadOnly colAccounts As List(Of Account)
    Public ReadOnly objCategories As CategoryTranslator
    Public ReadOnly objIncExpAccounts As CategoryTranslator
    Public ReadOnly objBudgets As BudgetTranslator
    Public ReadOnly objSecurity As Security
    Public objInfo As CompanyInfo

    'Table with memorized payees.
    Public domTransTable As VB6XmlDocument
    'Above with Output attributes of Payee elements converted to upper case.
    Public domTransTableUCS As VB6XmlDocument

    Private ReadOnly mstrDataPathValue As String
    Private mobjLockFile As System.IO.Stream
    Private mintMaxAccountKey As Integer

    'Category keys of categories which typically have due dates
    '14 days or less after invoice or billing dates. Category
    'keys have "(" and ")" around them.
    Public strShortTermsCatKeys As String

    'Key of budget used as placeholder in fake trx.
    Public strPlaceholderBudgetKey As String

    Public Delegate Sub ShowStartupAccount(ByVal objAccount As Account)
    Public Delegate Sub ShowCreateNewMessage(ByVal strMessage As String)

    Public Sub New(ByVal strDataPathValue As String)
        colAccounts = New List(Of Account)
        objCategories = New CategoryTranslator()
        objIncExpAccounts = New CategoryTranslator()
        objBudgets = New BudgetTranslator()
        objSecurity = New Security(Me)
        mstrDataPathValue = strDataPathValue

        If System.IO.File.Exists(strCompanyInfoPath()) Then
            Dim ser As XmlSerializer = New XmlSerializer(GetType(CompanyInfo))
            Using inputStream As System.IO.FileStream = New IO.FileStream(strCompanyInfoPath(), IO.FileMode.Open)
                objInfo = DirectCast(ser.Deserialize(inputStream), CompanyInfo)
            End Using
        Else
            objInfo = New CompanyInfo()
        End If
    End Sub

    Public Shared Function strExecutableFolder() As String
        Return My.Application.Info.DirectoryPath
    End Function

    Public Function blnDataIsLocked() As Boolean
        Try
            mobjLockFile = New IO.FileStream(strAddPath("LockFile.dat"), IO.FileMode.Append, IO.FileAccess.Write, IO.FileShare.None)
            Return False
        Catch ex As System.IO.IOException
            Return True
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Public Function blnAccountKeyUsed(ByVal intKey As Integer) As Boolean
        For Each act As Account In colAccounts
            If act.intKey = intKey Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function datLastReconciled() As Date
        Dim datResult As Date = New Date(1900, 1, 1)
        For Each act As Account In colAccounts
            For Each reg In act.colRegisters
                For Each objTrx In reg.colAllTrx()
                    If objTrx.lngStatus = Trx.TrxStatus.Reconciled Then
                        If objTrx.datDate > datResult Then
                            datResult = objTrx.datDate
                        End If
                    End If
                Next
            Next
        Next
        Return datResult
    End Function

    Public Sub UseAccountKey(ByVal intKey As Integer)
        If intKey > mintMaxAccountKey Then
            mintMaxAccountKey = intKey
        End If
    End Sub

    Public Function intGetUnusedAccountKey() As Integer
        'We cannot just check colAccounts, because a new Account
        'object is not immediately created when the user creates a new account in the UI.
        'So we need to keep track of account keys created here, by incrementing.
        mintMaxAccountKey += 1
        Return mintMaxAccountKey
    End Function

    Public Sub FireSomethingModified()
        RaiseEvent SomethingModified()
    End Sub

    Public Sub Teardown()
        Dim objAccount As Account
        For Each objAccount In colAccounts
            objAccount.Teardown()
        Next objAccount
    End Sub

    Public Sub Load(ByVal showAccount As ShowStartupAccount)
        LoadGlobalLists()
        LoadTransTable()
        LoadAccountFiles(showAccount)
    End Sub

    Public Sub LoadGlobalLists()
        Try

            objBudgets.LoadFile(strBudgetPath())
            objIncExpAccounts.LoadFile(strCategoryPath())
            LoadCategories()  'Will not include asset, liability and equity accounts, but that's okay at this point.
            FindPlaceholderBudget()

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public Sub LoadCategories()
        Dim intIndex As Integer
        objCategories.Init()
        For intIndex = 1 To objIncExpAccounts.intElements
            objCategories.Add(objIncExpAccounts.objElement(intIndex))
        Next
        AddAccountTypeToCategories(Account.AccountType.Asset)
        AddAccountTypeToCategories(Account.AccountType.Liability)
        AddAccountTypeToCategories(Account.AccountType.Equity)
        BuildShortTermsCatKeys()
    End Sub

    Private Sub AddAccountTypeToCategories(ByVal lngType As Account.AccountType)
        Dim objCats As List(Of StringTransElement) = New List(Of StringTransElement)
        Dim elm As StringTransElement
        Dim strPrefix As String = Account.strTypeToLetter(lngType)
        For Each objAccount As Account In colAccounts
            If objAccount.lngType = lngType Then
                For Each objReg As Register In objAccount.colRegisters
                    Dim strKey As String = objReg.strCatKey
                    elm = New StringTransElement(objCategories, strKey, strPrefix + ":" + objReg.strTitle, " " + objReg.strTitle)
                    objCats.Add(elm)
                Next
            End If
        Next
        objCats.Sort(AddressOf intCategoryComparer)
        For Each elm In objCats
            objCategories.Add(elm)
        Next
    End Sub

    Private Function intCategoryComparer(ByVal cat1 As StringTransElement, ByVal cat2 As StringTransElement) As Integer
        Return cat1.strValue1.CompareTo(cat2.strValue1)
    End Function

    'Set gstrPlaceholderBudgetKey to the key of the budget whose name
    'is "(budget)", or set it to "---" if there is no such budget.
    Public Sub FindPlaceholderBudget()
        Dim intPlaceholderIndex As Integer
        intPlaceholderIndex = objBudgets.intLookupValue1("(placeholder)")
        If intPlaceholderIndex > 0 Then
            strPlaceholderBudgetKey = objBudgets.strKey(intPlaceholderIndex)
        Else
            'Don't use empty string, because that's the key used
            'if a split doesn't use a budget.
            strPlaceholderBudgetKey = "---"
        End If
    End Sub

    'Set gstrShortTermsCatKeys by the heuristic of looking for
    'recognizable strings in the category names.
    Public Sub BuildShortTermsCatKeys()
        Dim intCatIndex As Integer
        Dim strCatName As String
        Dim blnPossibleCredit As Boolean
        Dim blnPossibleUtility As Boolean

        strShortTermsCatKeys = ""
        For intCatIndex = 1 To objCategories.intElements
            strCatName = LCase(objCategories.strValue1(intCatIndex))

            blnPossibleUtility = blnHasWord(strCatName, "util") Or blnHasWord(strCatName, "phone") Or
                blnHasWord(strCatName, "trash") Or blnHasWord(strCatName, "garbage") Or
                blnHasWord(strCatName, "oil") Or blnHasWord(strCatName, "heat") Or
                blnHasWord(strCatName, "electric") Or blnHasWord(strCatName, "cable") Or
                blnHasWord(strCatName, "comcast") Or blnHasWord(strCatName, "web") Or
                blnHasWord(strCatName, "internet") Or blnHasWord(strCatName, "qwest") Or blnHasWord(strCatName, "verizon")

            blnPossibleCredit = blnHasWord(strCatName, "card") Or blnHasWord(strCatName, "bank") Or
                blnHasWord(strCatName, "loan") Or blnHasWord(strCatName, "auto") Or
                blnHasWord(strCatName, "car") Or blnHasWord(strCatName, "truck") Or
                blnHasWord(strCatName, "mortgage") Or blnHasWord(strCatName, "house")

            If blnPossibleCredit Or blnPossibleUtility Then
                strShortTermsCatKeys = strShortTermsCatKeys & strEncodeCatKey(objCategories.strKey(intCatIndex))
            End If
        Next

    End Sub

    '$Description Load list of memorized payees and import translation instructions.

    Public Sub LoadTransTable()
        Try

            Dim strTableFile As String

            strTableFile = strPayeeFilePath()
            domTransTable = domLoadFile(strTableFile)
            CreateTransTableUCS()

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Deep clone gdomTransTable into gdomTransTableUCS, adding
    '   an OutputUCS attribute for each Payee element with an Output attribute.
    '   This routine must be called whenever payee information changes. The
    '   resulting DOM is temporary, and never saved to an XML file.

    Public Sub CreateTransTableUCS()
        Dim colPayees As VB6XmlNodeList
        Dim elmPayee As VB6XmlElement
        Dim vntOutput As Object

        Try

            domTransTableUCS = DirectCast(domTransTable.CloneNode(True), VB6XmlDocument)
            colPayees = domTransTableUCS.DocumentElement.SelectNodes("Payee")
            For Each elmPayee In colPayees
                vntOutput = elmPayee.GetAttribute("Output")
                If Not gblnXmlAttributeMissing(vntOutput) Then
                    elmPayee.SetAttribute("OutputUCS", UCase(CStr(vntOutput)))
                End If
            Next elmPayee

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public Function colFindPayeeMatches(ByRef strRawInput As String) As VB6XmlNodeList
        Dim strInput As String
        Dim strXPath As String

        colFindPayeeMatches = Nothing
        Try

            strInput = UCase(Trim(strRawInput))
            strInput = Replace(strInput, "'", "")
            strXPath = "Payee[substring(@OutputUCS,1," & Len(strInput) & ")='" & strInput & "']"
            colFindPayeeMatches = domTransTableUCS.DocumentElement.SelectNodes(strXPath)

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    '$Description Load an XML file into a new DOM and return it.

    Public Function domLoadFile(ByVal strFile As String) As VB6XmlDocument
        Dim dom As VB6XmlDocument
        Dim objParseError As VB6XmlParseError

        domLoadFile = Nothing
        Try

            dom = New VB6XmlDocument
            With dom
                .Load(strFile)
                objParseError = .ParseError
                If Not objParseError Is Nothing Then
                    gRaiseError("XML parse error loading file: " & gstrXMLParseErrorText(objParseError))
                End If
                .SetProperty("SelectionLanguage", "XPath")
            End With
            domLoadFile = dom

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Public Sub LoadAccountFiles(ByVal showAccount As ShowStartupAccount)
        Try
            'Find all ".act" files.
            Dim strFile As String = Dir(strAccountPath() & "\*.act")
            Dim intFiles As Integer = 0
            Dim datCutoff As Date
            Dim astrFiles() As String = Nothing
            Dim objAccount As Account

            While strFile <> ""
                intFiles = intFiles + 1
                ReDim Preserve astrFiles(intFiles - 1)
                astrFiles(intFiles - 1) = strFile
                strFile = Dir()
            End While

            'Load real trx, and non-generated fake trx, for all of them.
            For Each strFile In astrFiles
                objAccount = New Account
                objAccount.Init(Me)
                showAccount(objAccount)
                objAccount.LoadStart(strFile)
                Me.colAccounts.Add(objAccount)
                showAccount(Nothing)
            Next strFile

            Me.colAccounts.Sort(AddressOf AccountComparer)

            'With all Account objects loaded we can add them to the category list.
            datCutoff = datLastReconciled().AddDays(1D)
            LoadCategories()

            'Load generated transactions for all of them.
            For Each objAccount In colAccounts
                showAccount(objAccount)
                objAccount.LoadGenerated(datCutoff)
                showAccount(Nothing)
            Next

            'Call Trx.Apply() for all Trx loaded above.
            'This will create ReplicaTrx.
            For Each objAccount In colAccounts
                showAccount(objAccount)
                objAccount.LoadApply()
                showAccount(Nothing)
            Next

            'Perform final steps after all Trx exist, including computing running balances.
            For Each objAccount In colAccounts
                showAccount(objAccount)
                objAccount.LoadFinish()
                showAccount(Nothing)
            Next
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Function AccountComparer(ByVal objAcct1 As Account, ByVal objAcct2 As Account) As Integer
        If objAcct1.lngType <> objAcct2.lngType Then
            Return objAcct1.lngType.CompareTo(objAcct2.lngType)
        End If
        Return objAcct1.strTitle.CompareTo(objAcct2.strTitle)
    End Function

    Private Function blnHasWord(ByVal strCatName As String, ByVal strPrefix As String) As Boolean
        blnHasWord = (InStr(strCatName, ":" & strPrefix) > 0) Or (InStr(strCatName, " " & strPrefix) > 0)
    End Function

    Public Shared Function strEncodeCatKey(ByVal strCatKey As String) As String
        Return "(" & strCatKey & ")"
    End Function

    Public Function strDataPath() As String
        Return mstrDataPathValue
    End Function

    Public Function blnDataPathExists() As Boolean
        Return System.IO.Directory.Exists(strDataPath())
    End Function

    Public Function strAddPath(ByVal strBareName As String) As String
        Return strDataPath() & "\" & strBareName
    End Function

    Public Function strTrxTypeFilePath() As String
        Return strAddPath("QIFImportTrxTypes.xml")
    End Function

    Public Function strPayeeFilePath() As String
        Return strAddPath("PayeeList.xml")
    End Function

    Public Function strCategoryPath() As String
        Return strAddPath("Shared.cat")
    End Function

    Public Function strBudgetPath() As String
        Return strAddPath("Shared.bud")
    End Function

    Public Function strCheckFormatPath() As String
        Return strAddPath("CheckFormat.xml")
    End Function

    Public Function strCompanyInfoPath() As String
        Return strAddPath("CompanyInfo.xml")
    End Function

    Public Function strAccountPath() As String
        Return strAddPath("Accounts")
    End Function

    Public Function strReportPath() As String
        Return strAddPath("Reports")
    End Function

    Public Function strBackupPath() As String
        Return strAddPath("Backup")
    End Function

    Public Sub CreateInitialData(ByVal objShowMessage As ShowCreateNewMessage)
        CreateStandardFolders(objShowMessage)
        CreateStandardFiles(objShowMessage)
        Account.CreateStandardChecking(Me, objShowMessage)
    End Sub

    Private Sub CreateStandardFolders(ByVal objShowMessage As ShowCreateNewMessage)
        Try
            If Not Directory.Exists(strDataPath()) Then
                Directory.CreateDirectory(strDataPath())
            End If
            If Not Directory.Exists(strAccountPath()) Then
                Directory.CreateDirectory(strAccountPath())
            End If
            If Not Directory.Exists(strBackupPath()) Then
                Directory.CreateDirectory(strBackupPath())
            End If
            If Not Directory.Exists(strReportPath()) Then
                Directory.CreateDirectory(strReportPath())
            End If

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub CreateStandardFiles(ByVal objShowMessage As ShowCreateNewMessage)
        Try
            'Standard category file
            If Not File.Exists(strCategoryPath()) Then
                objShowMessage("Creating standard category list, which you can edit later...")
                Using objCatWriter As TextWriter = New StreamWriter(strCategoryPath())
                    objCatWriter.WriteLine("Dummy line")
                    objCatWriter.WriteLine("/001/I/Income")
                    objCatWriter.WriteLine("/002/I:Interest/ Interest/Type:OTINC")
                    objCatWriter.WriteLine("/003/I:Wages/ Wages/Type:OTINC")
                    objCatWriter.WriteLine("/004/I:Bonus/ Bonus/Type:OTINC")
                    objCatWriter.WriteLine("/005/I:Other/ Other/Type:OTINC")
                    objCatWriter.WriteLine("/006/I:Sales/ Sales/Type:SALES")
                    objCatWriter.WriteLine("/007/I:Draw/ Draw/Type:OTINC")
                    objCatWriter.WriteLine("/008/I:Gift/ Gift/Type:OTINC")
                    objCatWriter.WriteLine("/009/E/Expense")
                    objCatWriter.WriteLine("/010/E:Cable TV/ Cable TV")
                    objCatWriter.WriteLine("/011/E:Car/ Car")
                    objCatWriter.WriteLine("/012/E:Car:Gasoline/  Gasoline")
                    objCatWriter.WriteLine("/013/E:Car:Payment/  Car Payment")
                    objCatWriter.WriteLine("/014/E:Car:Repair/  Car Repair")
                    objCatWriter.WriteLine("/015/E:Charity/ Charity")
                    objCatWriter.WriteLine("/016/E:Cleaning/ Cleaning")
                    objCatWriter.WriteLine("/017/E:Clothing/ Clothing")
                    objCatWriter.WriteLine("/018/E:Credit Cards/ Credit Cards")
                    objCatWriter.WriteLine("/019/E:Entertainment/ Entertainment")
                    objCatWriter.WriteLine("/020/E:Groceries/ Groceries")
                    objCatWriter.WriteLine("/021/E:Home/ Home")
                    objCatWriter.WriteLine("/022/E:Home:Mortgage/  Mortgage")
                    objCatWriter.WriteLine("/023/E:Home:Repair/  Home Repair")
                    objCatWriter.WriteLine("/024/E:Internet/ Internet")
                    objCatWriter.WriteLine("/025/E:Medical/ Medical")
                    objCatWriter.WriteLine("/026/E:Medical:Insurance/  Insurance")
                    objCatWriter.WriteLine("/027/E:Medical:Office Visits/  Office Visits")
                    objCatWriter.WriteLine("/028/E:Medical:Prescriptions/  Prescriptions")
                    objCatWriter.WriteLine("/029/E:Miscellaneous/ Miscellaneous")
                    objCatWriter.WriteLine("/030/E:Taxes/ Taxes/Type:TAXES")
                    objCatWriter.WriteLine("/031/E:Taxes:Federal Income/  Federal Income/Type:TAXES")
                    objCatWriter.WriteLine("/032/E:Taxes:Local Income/  Local Income/Type:TAXES")
                    objCatWriter.WriteLine("/033/E:Taxes:Property/  Property/Type:TAXES")
                    objCatWriter.WriteLine("/034/E:Taxes:State Income/  State Income/Type:TAXES")
                    objCatWriter.WriteLine("/035/E:Util/ Utilities")
                    objCatWriter.WriteLine("/036/E:Util:Electric/  Electricity")
                    objCatWriter.WriteLine("/037/E:Util:Oil/  Fuel Oil")
                    objCatWriter.WriteLine("/040/E:Util:Natural Gas/  Natural Gas")
                    objCatWriter.WriteLine("/039/E:Util:Phone/  Phone")
                    objCatWriter.WriteLine("/038/E:Util:Water/  Water")
                End Using
            End If

            'Standard budget file
            If Not File.Exists(strBudgetPath()) Then
                objShowMessage("Creating standard budget list, which you can edit later...")
                Using objBudgetWriter As TextWriter = New StreamWriter(strBudgetPath())
                    objBudgetWriter.WriteLine("dummy line")
                    objBudgetWriter.WriteLine("/01/Groceries/Groceries")
                    objBudgetWriter.WriteLine("/02/Clothing/Clothing")
                    objBudgetWriter.WriteLine("/03/Entertainment/Entertainment")
                End Using
            End If

            'Standard payee file
            If Not File.Exists(strPayeeFilePath()) Then
                Using objPayeeWriter As TextWriter = New StreamWriter(strPayeeFilePath())
                    objPayeeWriter.WriteLine("<Table>")
                    objPayeeWriter.WriteLine("</Table>")
                End Using
            End If

            'Standard import transaction types file
            If Not File.Exists(strTrxTypeFilePath()) Then
                Using objTrxTypeWriter As TextWriter = New StreamWriter(strTrxTypeFilePath())
                    objTrxTypeWriter.WriteLine("<Table>")
                    objTrxTypeWriter.WriteLine("</Table>")
                End Using
            End If

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

End Class