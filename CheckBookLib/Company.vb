Option Strict On
Option Explicit On

Public Class Company

    Public Event SomethingModified()

    Public ReadOnly strCompanyName As String
    Public ReadOnly colAccounts As List(Of Account)
    Public ReadOnly objCategories As CategoryTranslator
    Public ReadOnly objIncExpAccounts As CategoryTranslator
    Public ReadOnly objBudgets As BudgetTranslator

    Dim mintMaxAccountKey As Integer

    Public Sub New()
        colAccounts = New List(Of Account)
        objCategories = New CategoryTranslator()
        objIncExpAccounts = New CategoryTranslator()
        objBudgets = New BudgetTranslator()
        strCompanyName = "Schmidt's Garden Center, Inc."
    End Sub

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

    Private Function blnHasWord(ByVal strCatName As String, ByVal strPrefix As String) As Boolean
        blnHasWord = (InStr(strCatName, ":" & strPrefix) > 0) Or (InStr(strCatName, " " & strPrefix) > 0)
    End Function

    Public Shared Function strEncodeCatKey(ByVal strCatKey As String) As String
        Return "(" & strCatKey & ")"
    End Function
End Class