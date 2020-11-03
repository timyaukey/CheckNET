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
    ''' Uses objCompany.objSecurity.blnNoFile to determine if authentication is 
    ''' required, and objCompany.objSecurity.blnAuthenticate() to authenticate 
    ''' user name and password if that is required.
    ''' </param>
    ''' <returns></returns>
    Public Shared Function objLoad(ByVal objCompany As Company,
        ByVal showAccount As Action(Of Account),
        ByVal authenticator As Func(Of Company, CompanyLoadError)) As CompanyLoadError

        If Not Company.blnDataPathIsValid(objCompany.strDataPath()) Then
            Return New CompanyLoadNotFound()
        End If
        objCompany.objSecurity.Load()
        Dim objAuthenticatorError As CompanyLoadError = authenticator(objCompany)
        If Not objAuthenticatorError Is Nothing Then
            Return objAuthenticatorError
        End If
        If Not objCompany.blnTryLockCompany() Then
            Return New CompanyLoadInUse()
        End If
        LoadGlobalLists(objCompany)
        objCompany.LoadTransTable()
        LoadAccountFiles(objCompany, showAccount)
        Return Nothing
    End Function

    Public Shared Sub LoadGlobalLists(ByVal objCompany As Company)
        Try

            objCompany.objBudgets.LoadFile(objCompany.strBudgetPath())
            objCompany.objIncExpAccounts.LoadFile(objCompany.strCategoryPath())
            LoadCategories(objCompany)  'Will not include asset, liability and equity accounts, but that's okay at this point.
            FindPlaceholderBudget(objCompany)

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public Shared Sub LoadCategories(ByVal objCompany As Company)
        Dim intIndex As Integer
        objCompany.objCategories.Init()
        For intIndex = 1 To objCompany.objIncExpAccounts.intElements
            objCompany.objCategories.Add(objCompany.objIncExpAccounts.objElement(intIndex))
        Next
        AddAccountTypeToCategories(objCompany, Account.AccountType.Asset)
        AddAccountTypeToCategories(objCompany, Account.AccountType.Liability)
        AddAccountTypeToCategories(objCompany, Account.AccountType.Equity)
        objCompany.BuildShortTermsCatKeys()
    End Sub

    Private Shared Sub AddAccountTypeToCategories(ByVal objCompany As Company, ByVal lngType As Account.AccountType)
        Dim objCats As List(Of StringTransElement) = New List(Of StringTransElement)
        Dim elm As StringTransElement
        Dim strPrefix As String = Account.strTypeToLetter(lngType)
        For Each objAccount As Account In objCompany.colAccounts
            If objAccount.lngType = lngType Then
                For Each objReg As Register In objAccount.colRegisters
                    Dim strKey As String = objReg.strCatKey
                    elm = New StringTransElement(objCompany.objCategories, strKey, strPrefix + ":" + objReg.strTitle, " " + objReg.strTitle)
                    objCats.Add(elm)
                Next
            End If
        Next
        objCats.Sort(AddressOf intCategoryComparer)
        For Each elm In objCats
            objCompany.objCategories.Add(elm)
        Next
    End Sub

    Private Shared Function intCategoryComparer(ByVal cat1 As StringTransElement, ByVal cat2 As StringTransElement) As Integer
        Return cat1.strValue1.CompareTo(cat2.strValue1)
    End Function

    'Set gstrPlaceholderBudgetKey to the key of the budget whose name
    'is "(budget)", or set it to "---" if there is no such budget.
    Public Shared Sub FindPlaceholderBudget(ByVal objCompany As Company)
        Dim intPlaceholderIndex As Integer
        intPlaceholderIndex = objCompany.objBudgets.intLookupValue1("(placeholder)")
        If intPlaceholderIndex > 0 Then
            objCompany.strPlaceholderBudgetKey = objCompany.objBudgets.strKey(intPlaceholderIndex)
        Else
            'Don't use empty string, because that's the key used
            'if a split doesn't use a budget.
            objCompany.strPlaceholderBudgetKey = "---"
        End If
    End Sub

    Public Shared Sub LoadAccountFiles(ByVal objCompany As Company, ByVal showAccount As Action(Of Account))
        Dim objLoader As AccountLoader
        Dim colLoaders As List(Of AccountLoader) = New List(Of AccountLoader)
        Try
            'Find all ".act" files.
            Dim strFile As String = Dir(objCompany.strAccountPath() & "\*.act")
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
                objCompany.colAccounts.Add(objAccount)
                showAccount(Nothing)
            Next strFile

            objCompany.colAccounts.Sort(AddressOf AccountComparer)

            'With all Account objects loaded we can add them to the category list.
            datCutoff = objCompany.datLastReconciled().AddDays(1D)
            LoadCategories(objCompany)

            'Load generated transactions for all of them.
            For Each objLoader In colLoaders
                showAccount(objLoader.objAccount)
                objLoader.LoadGenerated(datCutoff)
                showAccount(Nothing)
            Next

            'This will merge the generated trx into the sort order.
            'The individual trx should already be sorted, but this will ensure it.
            'This has to happen before objLoader.LoadApply(), or budgets won't be
            'applied properly.
            SortAllRegisters(colLoaders)

            'Call Trx.Apply() for all Trx loaded above.
            'This will create ReplicaTrx.
            For Each objLoader In colLoaders
                showAccount(objLoader.objAccount)
                objLoader.LoadApply()
                showAccount(Nothing)
            Next

            'This will merge the ReplicaTrx created by objLoader.LoadApply() into the sort order.
            SortAllRegisters(colLoaders)

            'Perform final steps after all Trx exist, including computing running balances.
            For Each objLoader In colLoaders
                showAccount(objLoader.objAccount)
                objLoader.LoadFinish()
                showAccount(Nothing)
            Next
        Catch ex As Exception
            For Each objLoader In colLoaders
                objLoader.objAccount.blnUnsavedChanges = False
            Next
            gNestedException(ex)
        End Try
    End Sub

    Private Shared Function AccountComparer(ByVal objAcct1 As Account, ByVal objAcct2 As Account) As Integer
        If objAcct1.lngType <> objAcct2.lngType Then
            Return objAcct1.lngType.CompareTo(objAcct2.lngType)
        End If
        Return objAcct1.strTitle.CompareTo(objAcct2.strTitle)
    End Function

    Public Shared Sub RecreateGeneratedTrx(ByVal objCompany As Company, ByVal datRegisterEndDate As Date, ByVal datCutoff As Date)
        Dim objAccount As Account
        Dim objReg As Register

        For Each objAccount In objCompany.colAccounts
            For Each objReg In objAccount.colRegisters
                'Allow UI to hide all register windows for all accounts before
                'we start any of them, because regenerating can cause ReplicaTrx
                'to be recreated in any Register.
                objReg.FireBeginRegenerating()
            Next objReg
        Next
        'Need to do all accounts, not just the selected account, because there may be many, many
        'accounts and even there are only a few each one can create trx in others through
        'balance sheet categories in trx.
        For Each objAccount In objCompany.colAccounts
            Dim objLoader As AccountLoader = New AccountLoader(objAccount)
            objLoader.RecreateGeneratedTrx(datRegisterEndDate, datCutoff)
        Next
        For Each objAccount In objCompany.colAccounts
            'Tell all register windows to refresh themselves.
            For Each objReg In objAccount.colRegisters
                'Recompute the running balances, because replica trx can be added anywhere.
                objReg.LoadFinish()
                objReg.FireEndRegenerating()
            Next objReg
        Next

    End Sub

    Private Shared Sub SortAllRegisters(ByVal colLoaders As List(Of AccountLoader))
        For Each objLoader In colLoaders
            For Each objReg In objLoader.objAccount.colRegisters
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

    Public ReadOnly Property strMessage() As String
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
