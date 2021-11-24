Option Strict On
Option Explicit On

Imports System.IO
Imports System.Xml.Serialization

Public Class CompanyLoader

    ''' <summary>
    ''' Load all Account objects and other objects belonging to a Company, after
    ''' authenticating the user if the Company requires authentication to access it.
    ''' </summary>
    ''' <param name="objCompany">The Company to load.</param>
    ''' <param name="showAccount">
    ''' This delegate is called every time the CompanyLoader switches to working
    ''' on a different Account. Will be called multiple times for each Account,
    ''' as CompanyLoader goes through different stages of loading the Accounts.
    ''' May also be called with Null/Nothing, indicating that no particular
    ''' Account is in the process of being loaded. The method pointed to by this
    ''' delegate can listen to the Account.LoadStatus event of the Account passed
    ''' to be notified when things happen during the loading process. This
    ''' delegate does not have to do anything - it is purely to allow the UI
    ''' to show some kind of status display.
    ''' </param>
    ''' <param name="authenticator">
    ''' This delegate is called to authenticate the user. Must return Null/Nothing
    ''' if the user successfully authenticates, or no authentication is required
    ''' for the Company. Otherwise returns an object subclassing CompanyLoadError, 
    ''' whose type indicates the specific problem encountered.
    ''' Uses Company.SecData.blnNoFile to determine if authentication is 
    ''' required, and Company.SecData.blnAuthenticate() to authenticate 
    ''' user name and password if that is required.
    ''' </param>
    ''' <returns></returns>
    Public Shared Function Load(ByVal objCompany As Company,
        ByVal showAccount As Action(Of Account),
        ByVal authenticator As Func(Of Company, CompanyLoadError)) As CompanyLoadError

        objCompany.FireBeforeLoad()
        If Not Company.IsDataPathValid(objCompany.DataFolderPath()) Then
            Return New CompanyLoadNotFound()
        End If
        objCompany.SecData.Load()
        Dim objAuthenticatorError As CompanyLoadError = authenticator(objCompany)
        If Not objAuthenticatorError Is Nothing Then
            Return objAuthenticatorError
        End If
        If Not objCompany.TryLockCompany() Then
            Return New CompanyLoadInUse()
        End If
        LoadGlobalLists(objCompany)
        objCompany.LoadMemorizedTrans()
        LoadAccountFiles(objCompany, showAccount)
        objCompany.FireAfterLoad()
        Return Nothing
    End Function

    Public Shared Sub LoadGlobalLists(ByVal objCompany As Company)
        Try

            objCompany.Budgets.LoadFile(objCompany.BudgetFilePath())
            objCompany.IncExpAccounts.LoadFile(objCompany.CategoryFilePath())
            LoadCategories(objCompany)  'Will not include asset, liability and equity accounts, but that's okay at this point.
            FindPlaceholderBudget(objCompany)

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Public Shared Sub LoadCategories(ByVal objCompany As Company)
        Dim intIndex As Integer
        objCompany.Categories.Init()
        For intIndex = 1 To objCompany.IncExpAccounts.ElementCount
            objCompany.Categories.Add(objCompany.IncExpAccounts.GetElement(intIndex))
        Next
        AddAccountTypeToCategories(objCompany, Account.AccountType.Asset)
        AddAccountTypeToCategories(objCompany, Account.AccountType.Liability)
        AddAccountTypeToCategories(objCompany, Account.AccountType.Equity)
        objCompany.BuildShortTermsCatKeys()
    End Sub

    Private Shared Sub AddAccountTypeToCategories(ByVal objCompany As Company, ByVal lngType As Account.AccountType)
        Dim objCats As List(Of StringTransElement) = New List(Of StringTransElement)
        Dim elm As StringTransElement
        Dim strPrefix As String = Account.AccountTypeToLetter(lngType)
        For Each objAccount As Account In objCompany.Accounts
            If objAccount.AcctType = lngType Then
                For Each objReg As Register In objAccount.Registers
                    Dim strKey As String = objReg.CatKey
                    elm = New StringTransElement(objCompany.Categories, strKey, strPrefix + ":" + objReg.Title, " " + objReg.Title)
                    objCats.Add(elm)
                Next
            End If
        Next
        objCats.Sort(AddressOf CategoryComparer)
        For Each elm In objCats
            objCompany.Categories.Add(elm)
        Next
    End Sub

    Private Shared Function CategoryComparer(ByVal cat1 As StringTransElement, ByVal cat2 As StringTransElement) As Integer
        Return cat1.Value1.CompareTo(cat2.Value1)
    End Function

    'Set gstrPlaceholderBudgetKey to the key of the budget whose name
    'is "(budget)", or set it to "---" if there is no such budget.
    Public Shared Sub FindPlaceholderBudget(ByVal objCompany As Company)
        Dim intPlaceholderIndex As Integer
        intPlaceholderIndex = objCompany.Budgets.FindIndexOfValue1("(placeholder)")
        If intPlaceholderIndex > 0 Then
            objCompany.PlaceholderBudgetKey = objCompany.Budgets.GetKey(intPlaceholderIndex)
        Else
            'Don't use empty string, because that's the key used
            'if a split doesn't use a budget.
            objCompany.PlaceholderBudgetKey = "---"
        End If
    End Sub

    Public Shared Sub LoadAccountFiles(ByVal objCompany As Company, ByVal showAccount As Action(Of Account))
        Dim objLoader As AccountLoader
        Dim colLoaders As List(Of AccountLoader) = New List(Of AccountLoader)
        Try
            'Find all ".act" files.
            Dim strFile As String = Dir(objCompany.AccountsFolderPath() & "\*.act")
            Dim intFiles As Integer = 0
            Dim datCutoff As Date
            Dim astrFiles() As String = Nothing

            While strFile <> ""
                intFiles = intFiles + 1
                ReDim Preserve astrFiles(intFiles - 1)
                astrFiles(intFiles - 1) = strFile
                strFile = Dir()
            End While

            'Load real trx, and non-generated fake trx, for all of them.
            For Each strFile In astrFiles
                Dim objAccount As Account = New Account
                objAccount.Init(objCompany)
                objLoader = New AccountLoader(objAccount)
                colLoaders.Add(objLoader)
                showAccount(objAccount)
                objLoader.LoadStart(strFile)
                objCompany.Accounts.Add(objAccount)
                showAccount(Nothing)
            Next strFile

            objCompany.Accounts.Sort(AddressOf AccountComparer)

            'With all Account objects loaded we can add them to the category list.
            datCutoff = objCompany.LastReconciledDate().AddDays(1D)
            LoadCategories(objCompany)

            'Load generated transactions for all of them.
            For Each objLoader In colLoaders
                showAccount(objLoader.Account)
                objLoader.LoadGenerated(datCutoff)
                showAccount(Nothing)
            Next

            'This will merge the generated trx into the sort order.
            'The individual trx should already be sorted, but this will ensure it.
            'This has to happen before objLoader.LoadApply(), or budgets won't be
            'applied properly.
            SortAllRegisters(colLoaders)

            'Call BaseTrx.Apply() for all BaseTrx loaded above.
            'This will create ReplicaTrx.
            For Each objLoader In colLoaders
                showAccount(objLoader.Account)
                objLoader.LoadApply()
                showAccount(Nothing)
            Next

            'This will merge the ReplicaTrx created by objLoader.LoadApply() into the sort order.
            SortAllRegisters(colLoaders)

            'Perform final steps after all BaseTrx exist, including computing running balances.
            For Each objLoader In colLoaders
                showAccount(objLoader.Account)
                objLoader.LoadFinish()
                showAccount(Nothing)
            Next
        Catch ex As Exception
            For Each objLoader In colLoaders
                objLoader.Account.HasUnsavedChanges = False
            Next
            NestedException(ex)
        End Try
    End Sub

    Private Shared Function AccountComparer(ByVal objAcct1 As Account, ByVal objAcct2 As Account) As Integer
        If objAcct1.AcctType <> objAcct2.AcctType Then
            Return objAcct1.AcctType.CompareTo(objAcct2.AcctType)
        End If
        Return objAcct1.Title.CompareTo(objAcct2.Title)
    End Function

    Public Shared Sub RecreateGeneratedTrx(ByVal objCompany As Company, ByVal datRegisterEndDate As Date, ByVal datCutoff As Date)
        Dim objAccount As Account
        Dim objReg As Register

        objCompany.FireBeforeExpensiveOperation()
        For Each objAccount In objCompany.Accounts
            For Each objReg In objAccount.Registers
                'Allow UI to hide all register windows for all accounts before
                'we start any of them, because regenerating can cause ReplicaTrx
                'to be recreated in any Register.
                objReg.FireBeginRegenerating()
            Next objReg
        Next
        'Need to do all accounts, not just the selected account, because there may be many, many
        'accounts and even there are only a few each one can create trx in others through
        'balance sheet categories in trx.
        For Each objAccount In objCompany.Accounts
            Dim objLoader As AccountLoader = New AccountLoader(objAccount)
            objLoader.RecreateGeneratedTrx(datRegisterEndDate, datCutoff)
        Next
        For Each objAccount In objCompany.Accounts
            'Sort all the registers now that all trx have been added to them,
            'and tell all register windows to refresh themselves.
            For Each objReg In objAccount.Registers
                objReg.Sort()
                'Recompute the running balances, because replica trx can be added anywhere.
                objReg.LoadFinish()
                objReg.FireEndRegenerating()
            Next objReg
        Next
        objCompany.FireAfterExpensiveOperation()

    End Sub

    Private Shared Sub SortAllRegisters(ByVal colLoaders As List(Of AccountLoader))
        For Each objLoader In colLoaders
            For Each objReg In objLoader.Account.Registers
                objReg.Sort()
            Next
        Next
    End Sub

End Class

Public MustInherit Class CompanyLoadError
    Private mstrMessage As String

    Public Sub New(ByVal strMessage_ As String)
        mstrMessage = strMessage_
    End Sub

    Public ReadOnly Property Message() As String
        Get
            Return mstrMessage
        End Get
    End Property

End Class

Public Class CompanyLoadNotFound
    Inherits CompanyLoadError

    Public Sub New()
        MyBase.New("Company data files not found")
    End Sub
End Class

Public Class CompanyLoadInUse
    Inherits CompanyLoadError

    Public Sub New()
        MyBase.New("Company data files are being used by someone else")
    End Sub
End Class

Public Class CompanyLoadNotAuthorized
    Inherits CompanyLoadError

    Public Sub New()
        MyBase.New("Invalid login or password")
    End Sub
End Class

Public Class CompanyLoadCanceled
    Inherits CompanyLoadError

    Public Sub New()
        MyBase.New("Operator canceled")
    End Sub
End Class
