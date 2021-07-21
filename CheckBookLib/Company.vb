Option Strict On
Option Explicit On

Imports System.IO
Imports System.Xml.Serialization

Imports Willowsoft.TamperProofData

<Assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Willowsoft.CheckBook.UnitTests")>

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

    Public ReadOnly Accounts As List(Of Account)
    Public ReadOnly Categories As CategoryTranslator
    Public ReadOnly IncExpAccounts As CategoryTranslator
    Public ReadOnly Budgets As BudgetTranslator
    Public ReadOnly SecData As Security
    Public Info As CompanyInfo

    'Table with memorized payees.
    Public MemorizedTransXml As VB6XmlDocument
    'Above with Output attributes of Payee elements converted to upper case.
    Public MemorizedTransXmlUCS As VB6XmlDocument
    'Same as MemorizedTransXml, but strongly typed.
    Public MemorizedTrans As PayeeList


    Private ReadOnly mDataFolderPath As String
    Private mMaxAccountKey As Integer
    Private mLockRefreshThread As Threading.Thread

    'Category keys of categories which typically have due dates
    '14 days or less after invoice or billing dates. Category
    'keys have "(" and ")" around them.
    Public ShortTermsCatKeys As String

    'Key of budget used as placeholder in fake trx.
    Public PlaceholderBudgetKey As String

    Public Shared MainLicense As IStandardLicense = LoadMainLicenseFile()
    Private Shared mExtraLicenses As List(Of IStandardLicense) = New List(Of IStandardLicense)()

    Public Sub New(ByVal strDataPathValue As String)
        Accounts = New List(Of Account)
        Categories = New CategoryTranslator()
        IncExpAccounts = New CategoryTranslator()
        Budgets = New BudgetTranslator()
        SecData = New Security(Me)
        mDataFolderPath = strDataPathValue

        If System.IO.File.Exists(CompanyInfoFilePath()) Then
            Dim ser As XmlSerializer = New XmlSerializer(GetType(CompanyInfo))
            Using inputStream As System.IO.FileStream = New IO.FileStream(CompanyInfoFilePath(), IO.FileMode.Open)
                Info = DirectCast(ser.Deserialize(inputStream), CompanyInfo)
            End Using
        Else
            Info = New CompanyInfo()
        End If
    End Sub

    Public Shared Function ExecutableFolder() As String
        Return My.Application.Info.DirectoryPath
    End Function

    Public Function TryLockCompany() As Boolean
        Dim objLockInfo As FileInfo = New FileInfo(CompanyLockFilePath())
        If Not objLockInfo.Exists Then
            LockCompany()
            Return True
        End If
        'A lock file that has not been modified in this many seconds is assumed to be abandoned,
        'and is treated as if it does not exist. The extra time beyond the refresh interval is
        'to allow for delays in propagating the lock file through Dropbox or other file sharing service.
        'This is vulnerable to delays in file synchronization, especially those caused by mass file
        'updates causing a long backlog of files to sync up, so even 30 minutes may not be long enough.
        Dim dblLockExpirationSeconds As Double = CDbl(mLockRefreshSeconds) + (30D * 60D)
        If DateTime.UtcNow.Subtract(objLockInfo.LastWriteTimeUtc).TotalSeconds > dblLockExpirationSeconds Then
            LockCompany()
            Return True
        End If
        Return False
    End Function

    'Recreate the lock file this often, which keeps the last modified timestamp current.
    Private mLockRefreshSeconds As Integer = 5 * 60

    Private Sub LockCompany()
        WriteCompanyLockFile()
        mLockRefreshThread = New Threading.Thread(AddressOf RefreshLockLoop)
        mLockRefreshThread.IsBackground = True
        mLockRefreshThread.Start()
    End Sub

    Private Sub RefreshLockLoop()
        Try
            Do
                Threading.Thread.Sleep(mLockRefreshSeconds * 1000)
                WriteCompanyLockFile()
            Loop
        Catch ex As Threading.ThreadInterruptedException

        End Try
    End Sub

    Private Sub WriteCompanyLockFile()
        Using objLockWriter As TextWriter = New StreamWriter(CompanyLockFilePath())
            'Nothing actually checks this in this version of the software,
            'but we might check to see if the computer name changes as an indication
            'that another process somewhere else wrote its own lock file because
            'of a race condition.
            objLockWriter.WriteLine(ComputerName())
        End Using
    End Sub

    Private ReadOnly Property CompanyLockFilePath() As String
        Get
            Return AddNameToDataPath("LockFile.dat")
        End Get
    End Property

    Private ReadOnly Property ComputerName() As String
        Get
            Return Environment.MachineName
        End Get
    End Property

    Public Sub UnlockCompany()
        If mLockRefreshThread IsNot Nothing Then
            'Only delete the lock file if we created one.
            'This method may be called in various shutdown scenarios,
            'including when the software was not able to lock the company.
            mLockRefreshThread.Interrupt()
            mLockRefreshThread.Join()
            mLockRefreshThread = Nothing
            Dim objLockInfo = New FileInfo(CompanyLockFilePath())
            If objLockInfo.Exists Then
                objLockInfo.Delete()
            End If
        End If
    End Sub


    Public Function IsAccountKeyUsed(ByVal intKey As Integer) As Boolean
        For Each act As Account In Accounts
            If act.Key = intKey Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function LastReconciledDate() As Date
        Dim datResult As DateTime = DateTime.MinValue
        For Each act As Account In Accounts
            act.SetLastReconciledDate()
            If act.LastReconciledDate > datResult Then
                datResult = act.LastReconciledDate
            End If
        Next
        Return datResult
    End Function

    Public Sub UseAccountKey(ByVal intKey As Integer)
        If intKey > mMaxAccountKey Then
            mMaxAccountKey = intKey
        End If
    End Sub

    Public Function GetUnusedAccountKey() As Integer
        'We cannot just check Accounts, because a new Account
        'object is not immediately created when the user creates a new account in the UI.
        'So we need to keep track of account keys created here, by incrementing.
        mMaxAccountKey += 1
        Return mMaxAccountKey
    End Function

    Public Sub FireSomethingModified()
        RaiseEvent SomethingModified()
    End Sub

    Public Sub Teardown()
        Dim objAccount As Account
        For Each objAccount In Accounts
            objAccount.Teardown()
        Next objAccount
    End Sub

    'Set gstrShortTermsCatKeys by the heuristic of looking for
    'recognizable strings in the category names.
    Public Sub BuildShortTermsCatKeys()
        Dim intCatIndex As Integer
        Dim strCatName As String
        Dim blnPossibleCredit As Boolean
        Dim blnPossibleUtility As Boolean

        ShortTermsCatKeys = ""
        For intCatIndex = 1 To Categories.intElements
            strCatName = LCase(Categories.strValue1(intCatIndex))

            blnPossibleUtility = CatNameHasWord(strCatName, "util") Or CatNameHasWord(strCatName, "phone") Or
                CatNameHasWord(strCatName, "trash") Or CatNameHasWord(strCatName, "garbage") Or
                CatNameHasWord(strCatName, "oil") Or CatNameHasWord(strCatName, "heat") Or
                CatNameHasWord(strCatName, "electric") Or CatNameHasWord(strCatName, "cable") Or
                CatNameHasWord(strCatName, "comcast") Or CatNameHasWord(strCatName, "web") Or
                CatNameHasWord(strCatName, "internet") Or CatNameHasWord(strCatName, "qwest") Or CatNameHasWord(strCatName, "verizon")

            blnPossibleCredit = CatNameHasWord(strCatName, "card") Or CatNameHasWord(strCatName, "bank") Or
                CatNameHasWord(strCatName, "loan") Or CatNameHasWord(strCatName, "auto") Or
                CatNameHasWord(strCatName, "car") Or CatNameHasWord(strCatName, "truck") Or
                CatNameHasWord(strCatName, "mortgage") Or CatNameHasWord(strCatName, "house")

            If blnPossibleCredit Or blnPossibleUtility Then
                ShortTermsCatKeys = ShortTermsCatKeys & EncodeCatKey(Categories.strKey(intCatIndex))
            End If
        Next

    End Sub

    '$Description Load list of memorized payees and import translation instructions.

    Public Sub LoadMemorizedTrans()
        Try

            Dim strTableFile As String

            strTableFile = MemorizedTransFilePath()
            MemorizedTransXml = LoadXmlFile(strTableFile)
            CreateMemorizedTransXmlUCS()
            LoadMemorizedTransNew()

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public Sub LoadMemorizedTransNew()
        Dim strTableFile As String
        Dim objPayeeSerializer As XmlSerializer

        strTableFile = MemorizedTransFilePath()
        objPayeeSerializer = New XmlSerializer(GetType(PayeeList))
        Using objReader As TextReader = New StreamReader(strTableFile)
            MemorizedTrans = DirectCast(objPayeeSerializer.Deserialize(objReader), PayeeList)
        End Using

        'Used only for testing, to validate I can generate XML from it.
        Using objStringWriter As StringWriter = New StringWriter()
            Dim objSer2 As XmlSerializer = New XmlSerializer(GetType(PayeeList))
            Dim objNames As XmlSerializerNamespaces = New XmlSerializerNamespaces()
            objNames.Add("", "")
            objSer2.Serialize(objStringWriter, MemorizedTrans, objNames)
        End Using
    End Sub

    '$Description Deep clone gdomTransTable into gdomTransTableUCS, adding
    '   an OutputUCS attribute for each Payee element with an Output attribute.
    '   This routine must be called whenever payee information changes. The
    '   resulting DOM is temporary, and never saved to an XML file.

    Public Sub CreateMemorizedTransXmlUCS()
        Dim colPayees As VB6XmlNodeList
        Dim elmPayee As VB6XmlElement
        Dim vntOutput As Object

        Try

            MemorizedTransXmlUCS = DirectCast(MemorizedTransXml.CloneNode(True), VB6XmlDocument)
            colPayees = MemorizedTransXmlUCS.DocumentElement.SelectNodes("Payee")
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

    Public Function FindPayeeMatches(ByRef strRawInput As String) As VB6XmlNodeList
        Dim strInput As String
        Dim strXPath As String

        FindPayeeMatches = Nothing
        Try

            strInput = UCase(Trim(strRawInput))
            strInput = Replace(strInput, "'", "")
            strXPath = "Payee[substring(@OutputUCS,1," & Len(strInput) & ")='" & strInput & "']"
            FindPayeeMatches = MemorizedTransXmlUCS.DocumentElement.SelectNodes(strXPath)

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    '$Description Load an XML file into a new DOM and return it.

    Public Function LoadXmlFile(ByVal strFile As String) As VB6XmlDocument
        Dim dom As VB6XmlDocument
        Dim objParseError As VB6XmlParseError

        LoadXmlFile = Nothing
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
            LoadXmlFile = dom

            Exit Function
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Function

    Public Event SavedAccount(ByVal strAccountTitle As String)

    Public Sub FireSavedAccount(ByVal strAccountTitle As String)
        RaiseEvent SavedAccount(strAccountTitle)
    End Sub

    Private Function CatNameHasWord(ByVal strCatName As String, ByVal strPrefix As String) As Boolean
        CatNameHasWord = (InStr(strCatName, ":" & strPrefix) > 0) Or (InStr(strCatName, " " & strPrefix) > 0)
    End Function

    Public Shared Function EncodeCatKey(ByVal strCatKey As String) As String
        Return "(" & strCatKey & ")"
    End Function

    Public Function DataFolderPath() As String
        Return mDataFolderPath
    End Function

    Public Shared Function IsDataPathValid(ByVal strPath As String) As Boolean
        If Not System.IO.Directory.Exists(strPath) Then
            Return False
        End If
        If Not System.IO.Directory.Exists(Company.AccountsFolderPath(strPath)) Then
            Return False
        End If
        If Not System.IO.File.Exists(Company.CategoryFilePath(strPath)) Then
            Return False
        End If
        If Not System.IO.File.Exists(Company.BudgetFilePath(strPath)) Then
            Return False
        End If
        Return True
    End Function

    Public Function AddNameToDataPath(ByVal strBareName As String) As String
        Return System.IO.Path.Combine(DataFolderPath(), strBareName)
    End Function

    Public Function TrxTypeFilePath() As String
        Return AddNameToDataPath("QIFImportTrxTypes.xml")
    End Function

    Public Function MemorizedTransFilePath() As String
        Return AddNameToDataPath("PayeeList.xml")
    End Function

    Public Function CategoryFilePath() As String
        Return CategoryFilePath(DataFolderPath())
    End Function

    Public Shared Function CategoryFilePath(ByVal strDataPath As String) As String
        Return System.IO.Path.Combine(strDataPath, "Shared.cat")
    End Function

    Public Function BudgetFilePath() As String
        Return BudgetFilePath(DataFolderPath())
    End Function

    Public Shared Function BudgetFilePath(ByVal strDataPath As String) As String
        Return System.IO.Path.Combine(strDataPath, "Shared.bud")
    End Function

    Public Function CheckFormatFilePath() As String
        Return AddNameToDataPath("CheckFormat.xml")
    End Function

    Public Function CompanyInfoFilePath() As String
        Return AddNameToDataPath("CompanyInfo.xml")
    End Function

    Public Function AccountsFolderPath() As String
        Return AccountsFolderPath(DataFolderPath())
    End Function

    Public Shared Function AccountsFolderPath(ByVal strDataPath As String) As String
        Return System.IO.Path.Combine(strDataPath, "Accounts")
    End Function

    Public Function ReportsFolderPath() As String
        Return AddNameToDataPath("Reports")
    End Function

    Public Function BackupsFolderPath() As String
        Return AddNameToDataPath("Backup")
    End Function

    Public Sub CreateInitialData(ByVal objShowMessage As Action(Of String))
        CreateStandardFolders(objShowMessage)
        CreateStandardFiles(objShowMessage)
        Account.CreateStandardChecking(Me, objShowMessage)
    End Sub

    Private Sub CreateStandardFolders(ByVal objShowMessage As Action(Of String))
        Try
            If Not Directory.Exists(DataFolderPath()) Then
                Directory.CreateDirectory(DataFolderPath())
            End If
            If Not Directory.Exists(AccountsFolderPath()) Then
                Directory.CreateDirectory(AccountsFolderPath())
            End If
            If Not Directory.Exists(BackupsFolderPath()) Then
                Directory.CreateDirectory(BackupsFolderPath())
            End If
            If Not Directory.Exists(ReportsFolderPath()) Then
                Directory.CreateDirectory(ReportsFolderPath())
            End If

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Sub CreateStandardFiles(ByVal objShowMessage As Action(Of String))
        Try
            'Standard category file
            If Not File.Exists(CategoryFilePath()) Then
                objShowMessage("Creating standard category list, which you can edit later...")
                Using objCatWriter As TextWriter = New StreamWriter(CategoryFilePath())
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
            If Not File.Exists(BudgetFilePath()) Then
                objShowMessage("Creating standard budget list, which you can edit later...")
                Using objBudgetWriter As TextWriter = New StreamWriter(BudgetFilePath())
                    objBudgetWriter.WriteLine("dummy line")
                    objBudgetWriter.WriteLine("/01/Groceries/Groceries")
                    objBudgetWriter.WriteLine("/02/Clothing/Clothing")
                    objBudgetWriter.WriteLine("/03/Entertainment/Entertainment")
                End Using
            End If

            'Standard payee file
            If Not File.Exists(MemorizedTransFilePath()) Then
                Using objPayeeWriter As TextWriter = New StreamWriter(MemorizedTransFilePath())
                    objPayeeWriter.WriteLine("<Table>")
                    objPayeeWriter.WriteLine("</Table>")
                End Using
            End If

            'Standard import transaction types file
            If Not File.Exists(TrxTypeFilePath()) Then
                Using objTrxTypeWriter As TextWriter = New StreamWriter(TrxTypeFilePath())
                    objTrxTypeWriter.WriteLine("<Table>")
                    objTrxTypeWriter.WriteLine("</Table>")
                End Using
            End If

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public Shared Function DefaultRootFolder(ByVal strSoftwareTitle As String) As String
        Return System.IO.Path.Combine(System.Environment.GetFolderPath(
            Environment.SpecialFolder.CommonApplicationData), strSoftwareTitle)
    End Function

    Public Shared Function LicenseFolderPath() As String
        Return System.IO.Path.Combine(System.Environment.GetFolderPath(
            Environment.SpecialFolder.CommonApplicationData), "WCCheckbookLicenses")
    End Function

    Private Shared Function LoadMainLicenseFile() As IStandardLicense
        Dim objLicense As IStandardLicense
        objLicense = New MainLicense()
        objLicense.Load(LicenseFolderPath())
        Return objLicense
    End Function

    Public Shared Sub AddExtraLicense(ByVal objLicense As IStandardLicense)
        mExtraLicenses.Add(objLicense)
    End Sub

    Public Shared ReadOnly Iterator Property ExtraLicenses() As IEnumerable(Of IStandardLicense)
        Get
            For Each objLicense As IStandardLicense In mExtraLicenses
                Yield objLicense
            Next
        End Get
    End Property

    Public Shared ReadOnly Property AnyNonActiveLicenses() As Boolean
        Get
            If MainLicense.Status <> LicenseStatus.Active Then
                Return True
            End If
            For Each objLicense As IStandardLicense In mExtraLicenses
                If objLicense.Status <> LicenseStatus.Active Then
                    Return True
                End If
            Next
            Return False
        End Get
    End Property

    Public ReadOnly Property AnyCriticalOperationFailed() As Boolean
        Get
            For Each objAcct As Account In Accounts
                For Each objReg In objAcct.Registers
                    objReg.CheckIfInCriticalOperation()
                    If objReg.blnCriticalOperationFailed Then
                        Return True
                    End If
                Next
            Next
            Return False
        End Get
    End Property

End Class