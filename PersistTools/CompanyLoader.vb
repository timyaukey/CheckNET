Option Strict On
Option Explicit On

Imports System.IO
Imports System.Xml.Serialization

Public Class CompanyLoader

    Public Shared Sub Load(ByVal objCompany As Company, ByVal showAccount As Company.ShowStartupAccount)
        LoadGlobalLists(objCompany)
        objCompany.LoadTransTable()
        LoadAccountFiles(objCompany, showAccount)
    End Sub

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

    Public Shared Sub LoadAccountFiles(ByVal objCompany As Company, ByVal showAccount As Company.ShowStartupAccount)
        Try
            'Find all ".act" files.
            Dim strFile As String = Dir(objCompany.strAccountPath() & "\*.act")
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
                objAccount.Init(objCompany)
                showAccount(objAccount)
                objAccount.LoadStart(strFile)
                objCompany.colAccounts.Add(objAccount)
                showAccount(Nothing)
            Next strFile

            objCompany.colAccounts.Sort(AddressOf AccountComparer)

            'With all Account objects loaded we can add them to the category list.
            datCutoff = objCompany.datLastReconciled().AddDays(1D)
            LoadCategories(objCompany)

            'Load generated transactions for all of them.
            For Each objAccount In objCompany.colAccounts
                showAccount(objAccount)
                objAccount.LoadGenerated(datCutoff)
                showAccount(Nothing)
            Next

            'Call Trx.Apply() for all Trx loaded above.
            'This will create ReplicaTrx.
            For Each objAccount In objCompany.colAccounts
                showAccount(objAccount)
                objAccount.LoadApply()
                showAccount(Nothing)
            Next

            'Perform final steps after all Trx exist, including computing running balances.
            For Each objAccount In objCompany.colAccounts
                showAccount(objAccount)
                objAccount.LoadFinish()
                showAccount(Nothing)
            Next
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Private Shared Function AccountComparer(ByVal objAcct1 As Account, ByVal objAcct2 As Account) As Integer
        If objAcct1.lngType <> objAcct2.lngType Then
            Return objAcct1.lngType.CompareTo(objAcct2.lngType)
        End If
        Return objAcct1.strTitle.CompareTo(objAcct2.strTitle)
    End Function

End Class
