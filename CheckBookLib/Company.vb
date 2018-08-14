Option Strict On
Option Explicit On

Public Class Company

    Public Event SomethingModified()

    Public ReadOnly strCompanyName As String
    Public ReadOnly colAccounts As List(Of Account)
    Public ReadOnly objCategories As CategoryTranslator
    Public ReadOnly objIncExpAccounts As CategoryTranslator
    Public ReadOnly objBudgets As BudgetTranslator
    Public ReadOnly objSecurity As Security
    Private ReadOnly mstrDataPathValue As String
    Private mobjLockFile As System.IO.Stream
    Private mintMaxAccountKey As Integer

    Public Delegate Sub ShowStartupAccount(ByVal objAccount As Account)

    Public Sub New(ByVal strDataPathValue As String)
        colAccounts = New List(Of Account)
        objCategories = New CategoryTranslator()
        objIncExpAccounts = New CategoryTranslator()
        objBudgets = New BudgetTranslator()
        objSecurity = New Security()
        mstrDataPathValue = strDataPathValue
        gstrDataPathValue = mstrDataPathValue   'Legacy
        strCompanyName = "Schmidt's Garden Center, Inc."
    End Sub

    Public Function blnDataIsLocked() As Boolean
        Try
            mobjLockFile = New IO.FileStream(gstrAddPath("LockFile.dat"), IO.FileMode.Append, IO.FileAccess.Write, IO.FileShare.None)
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
                    If objTrx.lngStatus = Trx.TrxStatus.glngTRXSTS_RECON Then
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

    Public Sub LoadGlobalLists()
        Try

            objBudgets.LoadFile(gstrAddPath("Shared.bud"))
            objIncExpAccounts.LoadFile(gstrAddPath("Shared.cat"))
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
                    elm = New StringTransElement(strKey, strPrefix + ":" + objReg.strTitle, " " + objReg.strTitle)
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
            gstrPlaceholderBudgetKey = objBudgets.strKey(intPlaceholderIndex)
        Else
            'Don't use empty string, because that's the key used
            'if a split doesn't use a budget.
            gstrPlaceholderBudgetKey = "---"
        End If
    End Sub

    'Set gstrShortTermsCatKeys by the heuristic of looking for
    'recognizable strings in the category names.
    Public Sub BuildShortTermsCatKeys()
        Dim intCatIndex As Integer
        Dim strCatName As String
        Dim blnPossibleCredit As Boolean
        Dim blnPossibleUtility As Boolean

        gstrShortTermsCatKeys = ""
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
                gstrShortTermsCatKeys = gstrShortTermsCatKeys & strEncodeCatKey(objCategories.strKey(intCatIndex))
            End If
        Next

    End Sub

    Public Sub LoadAccountFiles(ByVal dlgShowAccount As ShowStartupAccount)
        Try
            'Find all ".act" files.
            Dim strFile As String = Dir(gstrAccountPath() & "\*.act")
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
                dlgShowAccount(objAccount)
                objAccount.LoadStart(strFile)
                Me.colAccounts.Add(objAccount)
                dlgShowAccount(Nothing)
            Next strFile

            Me.colAccounts.Sort(AddressOf AccountComparer)

            'With all Account objects loaded we can add them to the category list.
            datCutoff = datLastReconciled().AddDays(1D)
            LoadCategories()

            'Load generated transactions for all of them.
            For Each objAccount In colAccounts
                dlgShowAccount(objAccount)
                objAccount.LoadGenerated(datCutoff)
                dlgShowAccount(Nothing)
            Next

            'Call Trx.Apply() for all Trx loaded above.
            'This will create ReplicaTrx.
            For Each objAccount In colAccounts
                dlgShowAccount(objAccount)
                objAccount.LoadApply()
                dlgShowAccount(Nothing)
            Next

            'Perform final steps after all Trx exist, including computing running balances.
            For Each objAccount In colAccounts
                dlgShowAccount(objAccount)
                objAccount.LoadFinish()
                dlgShowAccount(Nothing)
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
End Class