Option Strict On
Option Explicit On

Imports System.IO

''' <summary>
''' Represents one general ledger account and all the transactions in it,
''' like a checking account or a loan account. The most important member
''' is Registers, which is a collection of the Register objects containing
''' the transactions in this account. Most commonly there is one register
''' per account.
''' </summary>

Public Class Account
    'The master Company object.
    Private mobjCompany As Company
    'Path passed to Load().
    Private mstrFileNameRoot As String
    'Account title.
    Private mstrTitle As String
    'Unique number identifying this Account from others loaded at the same time.
    Private mintKey As Integer
    Private mlngType As AccountType
    Private mlngSubType As SubType
    Private mobjRelatedAcct1 As Account
    Private mobjRelatedAcct2 As Account
    Private mobjRelatedAcct3 As Account
    Private mobjRelatedAcct4 As Account
    'Repeat key list for account.
    Private mobjRepeats As SimpleStringTranslator
    Private mobjRepeatSummarizer As RepeatSummarizer
    'File mobjRepeats was loaded from.
    'Private mstrRepeatsFile As String
    'All LoadedRegister objects for account, whether or not
    'displayed in any UI.
    Private mcolRegisters As List(Of Register)
    Private mdatLastReconciled As DateTime
    'Account has unsaved changes.
    Private mblnUnsavedChanges As Boolean

    Public Enum AccountType
        Unspecified = 0
        Asset = 1
        Liability = 2
        Equity = 3
        Personal = 4
    End Enum

    Public Enum SubType
        Unspecified = 0
        Asset_CheckingAccount = 100
        Asset_SavingsAccount = 101
        Asset_AccountsReceivable = 102
        Asset_Inventory = 103
        Asset_LoanReceivable = 104
        Asset_RealProperty = 105
        Asset_OtherProperty = 106
        Asset_Investment = 107
        Asset_Other = 199
        Liability_LoanPayable = 200
        Liability_AccountsPayable = 201
        Liability_Taxes = 202
        Liability_Other = 299
        Equity_RetainedEarnings = 302
        Equity_Stock = 303
        Equity_Capital = 399
        Personal_LiabilityLoan = 400
        Personal_AssetLoan = 401
        Personal_BankAccount = 402
        Personal_Other = 499
    End Enum

    Public Class SubTypeDef
        Public lngType As AccountType
        Public lngSubType As SubType
        Public strName As String
        Public strSaveCode As String

        Public Overrides Function ToString() As String
            Return strName
        End Function
    End Class

    Public Shared SubTypeDefs() As SubTypeDef =
    {
        New SubTypeDef() With {.lngType = AccountType.Asset, .lngSubType = SubType.Asset_CheckingAccount, .strName = "Asset - Checking Account", .strSaveCode = "A"},
        New SubTypeDef() With {.lngType = AccountType.Asset, .lngSubType = SubType.Asset_SavingsAccount, .strName = "Asset - Savings Account", .strSaveCode = "ASA"},
        New SubTypeDef() With {.lngType = AccountType.Asset, .lngSubType = SubType.Asset_AccountsReceivable, .strName = "Asset - Accounts Receivable", .strSaveCode = "AAR"},
        New SubTypeDef() With {.lngType = AccountType.Asset, .lngSubType = SubType.Asset_Inventory, .strName = "Asset - Inventory", .strSaveCode = "AIV"},
        New SubTypeDef() With {.lngType = AccountType.Asset, .lngSubType = SubType.Asset_LoanReceivable, .strName = "Asset - Loan Receivable", .strSaveCode = "ALR"},
        New SubTypeDef() With {.lngType = AccountType.Asset, .lngSubType = SubType.Asset_RealProperty, .strName = "Asset - Real Property", .strSaveCode = "ARP"},
        New SubTypeDef() With {.lngType = AccountType.Asset, .lngSubType = SubType.Asset_OtherProperty, .strName = "Asset - Other Property", .strSaveCode = "AOP"},
        New SubTypeDef() With {.lngType = AccountType.Asset, .lngSubType = SubType.Asset_Investment, .strName = "Asset - Investment", .strSaveCode = "AIN"},
        New SubTypeDef() With {.lngType = AccountType.Asset, .lngSubType = SubType.Asset_Other, .strName = "Asset - Other", .strSaveCode = "AOT"},
        New SubTypeDef() With {.lngType = AccountType.Liability, .lngSubType = SubType.Liability_LoanPayable, .strName = "Liability - Loan Payable", .strSaveCode = "L"},
        New SubTypeDef() With {.lngType = AccountType.Liability, .lngSubType = SubType.Liability_AccountsPayable, .strName = "Liability - Accounts Payable", .strSaveCode = "LAP"},
        New SubTypeDef() With {.lngType = AccountType.Liability, .lngSubType = SubType.Liability_Taxes, .strName = "Liability - Taxes", .strSaveCode = "LTX"},
        New SubTypeDef() With {.lngType = AccountType.Liability, .lngSubType = SubType.Liability_Other, .strName = "Liability - Other", .strSaveCode = "LOT"},
        New SubTypeDef() With {.lngType = AccountType.Equity, .lngSubType = SubType.Equity_RetainedEarnings, .strName = "Equity - Retained Earnings", .strSaveCode = "ERE"},
        New SubTypeDef() With {.lngType = AccountType.Equity, .lngSubType = SubType.Equity_Stock, .strName = "Equity - Stock", .strSaveCode = "EST"},
        New SubTypeDef() With {.lngType = AccountType.Equity, .lngSubType = SubType.Equity_Capital, .strName = "Equity - Capital", .strSaveCode = "E"},
        New SubTypeDef() With {.lngType = AccountType.Personal, .lngSubType = SubType.Personal_LiabilityLoan, .strName = "Personal - Liability Loan", .strSaveCode = "PLL"},
        New SubTypeDef() With {.lngType = AccountType.Personal, .lngSubType = SubType.Personal_AssetLoan, .strName = "Personal - Asset Loan", .strSaveCode = "PAL"},
        New SubTypeDef() With {.lngType = AccountType.Personal, .lngSubType = SubType.Personal_BankAccount, .strName = "Personal - Bank Account", .strSaveCode = "PBA"},
        New SubTypeDef() With {.lngType = AccountType.Personal, .lngSubType = SubType.Personal_Other, .strName = "Personal - Other", .strSaveCode = "POT"}
    }

    'Fired when ChangeMade() is called. Used by clients
    'sensitive to changes in the Account as a whole,
    'for example anything that remembers the index of a BaseTrx
    'in its Register.
    Public Event ChangeMade()

    'Fired multiple times by register loading process.
    Public Event LoadStatus(ByVal strMessage As String)

    Public Sub RaiseLoadStatus(ByVal strMessage As String)
        RaiseEvent LoadStatus(strMessage)
    End Sub

    '$Description Initialize a new instance.
    '   Must always be the first member used for a new instance.

    Public Sub Init(ByVal objCompany As Company)
        mobjCompany = objCompany
    End Sub

    Public ReadOnly Property Company() As Company
        Get
            Return mobjCompany
        End Get
    End Property

    Public Property FileNameRoot() As String
        Get
            Return mstrFileNameRoot
        End Get
        Set(value As String)
            mstrFileNameRoot = value
        End Set
    End Property

    Public Property Title() As String
        Get
            If Len(mstrTitle) > 0 Then
                Return mstrTitle
            Else
                Return mstrFileNameRoot
            End If
        End Get
        Set(ByVal Value As String)
            mstrTitle = Value
            SetChanged()
        End Set
    End Property

    Public Property AccountKey() As Integer
        Get
            Return mintKey
        End Get
        Set(value As Integer)
            mintKey = value
            SetChanged()
        End Set
    End Property

    Public Property AcctType() As AccountType
        Get
            Return mlngType
        End Get
        Set(value As AccountType)
            mlngType = value
            SetChanged()
        End Set
    End Property

    Public Property AcctSubType() As SubType
        Get
            Return mlngSubType
        End Get
        Set(value As SubType)
            mlngSubType = value
            SetChanged()
        End Set
    End Property

    Public Property RelatedAcct1() As Account
        Get
            Return mobjRelatedAcct1
        End Get
        Set(value As Account)
            mobjRelatedAcct1 = value
            SetChanged()
        End Set
    End Property

    Public Property RelatedAcct2() As Account
        Get
            Return mobjRelatedAcct2
        End Get
        Set(value As Account)
            mobjRelatedAcct2 = value
            SetChanged()
        End Set
    End Property

    Public Property RelatedAcct3() As Account
        Get
            Return mobjRelatedAcct3
        End Get
        Set(value As Account)
            mobjRelatedAcct3 = value
            SetChanged()
        End Set
    End Property

    Public Property RelatedAcct4() As Account
        Get
            Return mobjRelatedAcct4
        End Get
        Set(value As Account)
            mobjRelatedAcct4 = value
            SetChanged()
        End Set
    End Property

    Public ReadOnly Property AccountTypeLetter() As String
        Get
            Return AccountTypeToLetter(AcctType)
        End Get
    End Property

    Public ReadOnly Property LastReconciledDate() As DateTime
        Get
            Return mdatLastReconciled
        End Get
    End Property

    Public Shared Function AccountTypeToLetter(ByVal lngType_ As AccountType) As String
        Select Case lngType_
            Case AccountType.Asset : Return "A"
            Case AccountType.Liability : Return "L"
            Case AccountType.Equity : Return "Q"
            Case AccountType.Personal : Return "P"
            Case Else
                Throw New Exception("Invalid account type")
        End Select
    End Function

    Public ReadOnly Property Registers() As List(Of Register)
        Get
            Return mcolRegisters
        End Get
    End Property

    Public ReadOnly Property Repeats() As SimpleStringTranslator
        Get
            Return mobjRepeats
        End Get
    End Property

    Public Sub BuildRepeatList()
        mobjRepeats = mobjRepeatSummarizer.BuildStringTranslator()
    End Sub

    Public ReadOnly Property RepeatSummarizer() As RepeatSummarizer
        Get
            Return mobjRepeatSummarizer
        End Get
    End Property

    Public Property HasUnsavedChanges() As Boolean
        Get
            Return mblnUnsavedChanges
        End Get
        Set(value As Boolean)
            mblnUnsavedChanges = value
        End Set
    End Property

    Friend Sub Teardown()
        mobjCompany = Nothing
    End Sub

    Public Sub CreateRegister(ByVal strRegKey As String, ByVal strRegTitle As String, ByVal blnRegShow As Boolean)

        Dim objReg As Register

        If strRegKey = "" Then
            gRaiseError("Missing RK line before RI line")
        End If
        If strRegTitle = "" Then
            gRaiseError("Missing RT line before RI line")
        End If
        If Not FindRegister(strRegKey) Is Nothing Then
            gRaiseError("Reg key already used in RI line")
        End If
        objReg = New Register
        objReg.Init(Me, strRegTitle, strRegKey, blnRegShow, 128)
        mcolRegisters.Add(objReg)
    End Sub

    Public Sub SetChanged()
        mblnUnsavedChanges = True
        RaiseEvent ChangeMade()
        Company.FireSomethingModified()
    End Sub

    Public Function FindRegister(ByVal strRegisterKey As String) As Register
        Dim objReg As Register
        For Each objReg In mcolRegisters
            If objReg.RegisterKey = strRegisterKey Then
                FindRegister = objReg
                Exit Function
            End If
        Next objReg
        FindRegister = Nothing
    End Function

    Public Function RegisterList() As SimpleStringTranslator
        Dim objReg As Register
        Dim objResult As SimpleStringTranslator

        objResult = New SimpleStringTranslator
        For Each objReg In mcolRegisters
            objResult.Add(New StringTransElement(objResult, objReg.RegisterKey, objReg.Title, objReg.Title))
        Next objReg
        RegisterList = objResult
    End Function

    Public Sub SetLastReconciledDate()
        mdatLastReconciled = DateTime.MinValue
        For Each reg In mcolRegisters
            For Each objTrx As BankTrx In reg.GetAllTrx(Of BankTrx)()
                If objTrx.lngStatus = BaseTrx.TrxStatus.Reconciled Then
                    If objTrx.datDate > mdatLastReconciled Then
                        mdatLastReconciled = objTrx.datDate
                    End If
                End If
            Next
        Next
    End Sub

    Public Sub InitForLoad()
        mblnUnsavedChanges = False
        mcolRegisters = New List(Of Register)
        'mobjRepeats = New StringTranslator()
        mobjRepeatSummarizer = New RepeatSummarizer()
        'mstrRepeatsFile = gstrAccountPath() & "\" & Replace(LCase(strAcctFile), ".act", ".rep")
        'mobjRepeats.LoadFile(mstrRepeatsFile)
        mlngType = Account.AccountType.Unspecified
    End Sub

    Public Overrides Function ToString() As String
        Return Me.Title
    End Function

    Public Shared Sub CreateStandardChecking(ByVal objCompany As Company, ByVal objShowMessage As Action(Of String))
        Try
            objShowMessage("Creating first checking account...")
            Dim objAccount As Account = New Account()
            objAccount.Init(objCompany)
            objAccount.AccountKey = objCompany.GetUnusedAccountKey()
            objAccount.AcctSubType = Account.SubType.Asset_CheckingAccount
            objAccount.FileNameRoot = "Main"
            objAccount.Title = "Checking Account"
            objAccount.Create()
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public Sub Create()
        Try
            Dim objSubTypeMatched As Account.SubTypeDef = Nothing

            For Each objSubType As Account.SubTypeDef In Account.SubTypeDefs
                If objSubType.lngSubType = AcctSubType Then
                    objSubTypeMatched = objSubType
                End If
            Next
            If objSubTypeMatched Is Nothing Then
                Throw New Exception("Unrecognized account subtype")
            End If

            Dim strAcctFile As String = mobjCompany.AccountsFolderPath() & "\" & FileNameRoot & ".act"
            Using objAcctWriter As TextWriter = New StreamWriter(strAcctFile)
                objAcctWriter.WriteLine("FHCKBK2")
                objAcctWriter.WriteLine("AT" & Title)
                objAcctWriter.WriteLine("AK" & CStr(AccountKey))
                objAcctWriter.WriteLine("AY" & objSubTypeMatched.strSaveCode)
                objAcctWriter.WriteLine("RK1")
                objAcctWriter.WriteLine("RT" & Title)
                objAcctWriter.WriteLine("RS")
                objAcctWriter.WriteLine("RI")
                objAcctWriter.WriteLine("RL1")
                objAcctWriter.WriteLine(".R")
                objAcctWriter.WriteLine("RF1")
                objAcctWriter.WriteLine(".R")
                objAcctWriter.WriteLine("RR1")
                objAcctWriter.WriteLine(".R")
                objAcctWriter.WriteLine(".A")
            End Using

            Dim strRepeatFile As String = mobjCompany.AccountsFolderPath() & "\" & FileNameRoot & ".rep"
            Using objRepeatWriter As TextWriter = New StreamWriter(strRepeatFile)
                objRepeatWriter.WriteLine("dummy line")
            End Using
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

End Class