Option Strict On
Option Explicit On

Imports System.IO

''' <summary>
''' Represents one general ledger account and all the transactions in it,
''' like a checking account or a loan account. The most important member
''' is colRegisters, which is a collection of the Register objects containing
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
    Private mintRelatedKey1 As Integer
    Private mintRelatedKey2 As Integer
    Private mintRelatedKey3 As Integer
    Private mintRelatedKey4 As Integer
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
    'Account has unsaved changes.
    Private mblnUnsavedChanges As Boolean
    'File number for Save().
    Private mintSaveFile As Integer
    'File for Load().
    Private mobjLoadFile As System.IO.TextReader
    'RegisterLoader object used by LoadRegister().
    'Is Nothing unless LoadRegister() is on the call stack.
    Private WithEvents mobjLoader As RegisterLoader

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

    Public Shared arrSubTypeDefs() As SubTypeDef =
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
        New SubTypeDef() With {.lngType = AccountType.Personal, .lngSubType = SubType.Personal_Other, .strName = "Pesonal - Other", .strSaveCode = "POT"}
    }

    'Fired when ChangeMade() is called. Used by clients
    'sensitive to changes in the Account as a whole,
    'for example anything that remembers the index of a Trx
    'in its Register.
    Public Event ChangeMade()

    'Fired multiple times by register loading process.
    Public Event LoadStatus(ByVal strMessage As String)

    '$Description Initialize a new instance.
    '   Must always be the first member used for a new instance.

    Public Sub Init(ByVal objCompany As Company)
        mobjCompany = objCompany
    End Sub

    Public ReadOnly Property objCompany() As Company
        Get
            objCompany = mobjCompany
        End Get
    End Property

    Public Property strFileNameRoot() As String
        Get
            strFileNameRoot = mstrFileNameRoot
        End Get
        Set(value As String)
            mstrFileNameRoot = value
        End Set
    End Property

    Public Property strTitle() As String
        Get
            If Len(mstrTitle) > 0 Then
                strTitle = mstrTitle
            Else
                strTitle = mstrFileNameRoot
            End If
        End Get
        Set(ByVal Value As String)
            mstrTitle = Value
            SetChanged()
        End Set
    End Property

    Public Property intKey() As Integer
        Get
            Return mintKey
        End Get
        Set(value As Integer)
            mintKey = value
            SetChanged()
        End Set
    End Property

    Public Property lngType() As AccountType
        Get
            Return mlngType
        End Get
        Set(value As AccountType)
            mlngType = value
            SetChanged()
        End Set
    End Property

    Public Property lngSubType() As SubType
        Get
            Return mlngSubType
        End Get
        Set(value As SubType)
            mlngSubType = value
            SetChanged()
        End Set
    End Property

    Public Property objRelatedAcct1() As Account
        Get
            Return mobjRelatedAcct1
        End Get
        Set(value As Account)
            mobjRelatedAcct1 = value
            SetChanged()
        End Set
    End Property

    Public Property objRelatedAcct2() As Account
        Get
            Return mobjRelatedAcct2
        End Get
        Set(value As Account)
            mobjRelatedAcct2 = value
            SetChanged()
        End Set
    End Property

    Public Property objRelatedAcct3() As Account
        Get
            Return mobjRelatedAcct3
        End Get
        Set(value As Account)
            mobjRelatedAcct3 = value
            SetChanged()
        End Set
    End Property

    Public Property objRelatedAcct4() As Account
        Get
            Return mobjRelatedAcct4
        End Get
        Set(value As Account)
            mobjRelatedAcct4 = value
            SetChanged()
        End Set
    End Property

    Public ReadOnly Property strType() As String
        Get
            Return strTypeToLetter(lngType)
        End Get
    End Property

    Public Shared Function strTypeToLetter(ByVal lngType_ As AccountType) As String
        Select Case lngType_
            Case AccountType.Asset : Return "A"
            Case AccountType.Liability : Return "L"
            Case AccountType.Equity : Return "Q"
            Case AccountType.Personal : Return "P"
            Case Else
                Throw New Exception("Invalid account type")
        End Select
    End Function

    Public ReadOnly Property colRegisters() As List(Of Register)
        Get
            Return mcolRegisters
        End Get
    End Property

    Public ReadOnly Property objRepeats() As SimpleStringTranslator
        Get
            objRepeats = mobjRepeats
        End Get
    End Property

    Public ReadOnly Property objRepeatSummarizer() As RepeatSummarizer
        Get
            objRepeatSummarizer = mobjRepeatSummarizer
        End Get
    End Property

    'Public ReadOnly Property strRepeatsFile() As String
    '    Get
    '        strRepeatsFile = mstrRepeatsFile
    '    End Get
    'End Property

    Public ReadOnly Property blnUnsavedChanges() As Boolean
        Get
            blnUnsavedChanges = mblnUnsavedChanges
        End Get
    End Property

    Public Sub RaiseLoadStatus(ByVal strMessage As String)
        RaiseEvent LoadStatus(strMessage)
    End Sub

    Public Sub Teardown()
        'UPGRADE_NOTE: Object mobjCompany may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        mobjCompany = Nothing
    End Sub

    '$Description Load a new instance from an account file.
    '   Must always be called immediately after Init().
    '$Param strAcctFile Name of account file, without path.

    Public Sub LoadStart(ByVal strAcctFile As String)

        RaiseEvent LoadStatus("Loading " & strAcctFile)
        mstrFileNameRoot = strAcctFile
        mblnUnsavedChanges = False
        mcolRegisters = New List(Of Register)
        LoadIndividual()
        'LoadGenerated()
    End Sub

    Private Sub LoadIndividual()
        Dim strLine As String
        Dim lngLinesRead As Integer
        Dim strRegKey As String = ""
        Dim strRegTitle As String = ""
        Dim blnRegShow As Boolean
        Dim blnAccountPropertiesValidated As Boolean

        Try

            OpenInputFile(mobjCompany.strAccountPath() & "\" & mstrFileNameRoot)

            strLine = strReadLine()
            lngLinesRead = lngLinesRead + 1
            'The difference between FHCKBK1 and FHCKBK2 is that FHCKBK2 is hardcoded
            'to use a specific budget file name and category file name, and repeat group
            'file (acctfilename).rep instead of getting the file names from FC, FB and FR
            'lines in the .act file.
            If strLine <> "FHCKBK2" Then
                gRaiseError("Invalid header line")
            End If

            'mobjRepeats = New StringTranslator()
            mobjRepeatSummarizer = New RepeatSummarizer()
            'mstrRepeatsFile = gstrAccountPath() & "\" & Replace(LCase(strAcctFile), ".act", ".rep")
            'mobjRepeats.LoadFile(mstrRepeatsFile)
            mlngType = AccountType.Unspecified

            Do
                strLine = strReadLine()
                lngLinesRead = lngLinesRead + 1
                Select Case Left(strLine, 2)
                    Case "AT"
                        mstrTitle = Mid(strLine, 3)
                    Case "AK"
                        Dim intNewKey As Integer = CInt(Mid(strLine, 3))
                        If mobjCompany.blnAccountKeyUsed(intNewKey) Then
                            Throw New Exception("Duplicate use of account key " & intNewKey)
                        End If
                        mintKey = intNewKey
                        mobjCompany.UseAccountKey(mintKey)
                    Case "AY"
                        Dim objSubTypeMatched As SubTypeDef = Nothing
                        For Each objSubType In arrSubTypeDefs
                            If objSubType.strSaveCode = Mid(strLine, 3) Then
                                objSubTypeMatched = objSubType
                            End If
                        Next
                        If objSubTypeMatched Is Nothing Then
                            Throw New Exception("Unrecognized account subtype")
                        End If
                        mlngType = objSubTypeMatched.lngType
                        mlngSubType = objSubTypeMatched.lngSubType
                    Case "AR"
                        Select Case Mid(strLine, 3, 1)
                            Case "1"
                                mintRelatedKey1 = intLoadParseRelatedAccountKey(strLine)
                            Case "2"
                                mintRelatedKey2 = intLoadParseRelatedAccountKey(strLine)
                            Case "3"
                                mintRelatedKey3 = intLoadParseRelatedAccountKey(strLine)
                            Case "4"
                                mintRelatedKey4 = intLoadParseRelatedAccountKey(strLine)
                            Case Else
                                Throw New Exception("Invalid related account selector")
                        End Select
                    Case "RK"
                        If Not blnAccountPropertiesValidated Then
                            'The lines that set these properties come before the first "RK" line.
                            If mintKey = 0 Then
                                Throw New Exception("Account key not specified")
                            End If
                            If mlngType = AccountType.Unspecified Then
                                Throw New Exception("Account type not specified")
                            End If
                            blnAccountPropertiesValidated = True
                        End If
                        strRegKey = Mid(strLine, 3)
                    Case "RT"
                        strRegTitle = Mid(strLine, 3)
                    Case "RS"
                        blnRegShow = True
                    Case "RN"
                        'Ignore this for backward compatibility until all .act files are resaved without it.
                        'blnRegNonBank = True
                    Case "RI"
                        CreateRegister(strRegKey, strRegTitle, blnRegShow)
                        strRegKey = ""
                        strRegTitle = ""
                        blnRegShow = False
                    Case "RL"
                        'Load individual non-fake Trx into Register.
                        LoadRegister(strLine, False, lngLinesRead)
                    Case "RF"
                        'Load individual fake Trx into Register.
                        LoadRegister(strLine, True, lngLinesRead)
                    Case "RR"
                        'Was the repeating register
                        SkipLegacyRegister()
                    Case ".A"
                        Exit Do
                    Case Else
                        gRaiseError("Unrecognized line in account file: " & strLine)
                End Select
            Loop
        Catch ex As Exception
            Throw New Exception("Error in Account.LoadIndividual(" & mstrFileNameRoot & ";" & lngLinesRead & ")", ex)
        Finally
            CloseInputFile()
        End Try
    End Sub

    Public Sub OpenInputFile(ByVal strFileName As String)
        mobjLoadFile = New StreamReader(strFileName)
    End Sub

    Public Sub CloseInputFile()
        mobjLoadFile.Close()
    End Sub

    Friend Function strReadLine() As String
        Return mobjLoadFile.ReadLine()
    End Function

    Private Function intLoadParseRelatedAccountKey(ByVal strLine As String) As Integer
        Dim intRelatedKey As Integer
        If Int32.TryParse(strLine.Substring(4), intRelatedKey) Then
            Return intRelatedKey
        End If
        Throw New Exception("Invalid related account key")
    End Function

    Public Sub LoadGenerated(ByVal datCutoff As Date)
        Dim objReg As Register

        Try
            Dim datRegisterEndDate As Date = DateAdd(Microsoft.VisualBasic.DateInterval.Day, 45, Today)
            'Create generated Trx.
            'Have to generate for all registers before computing
            'balances or doing any post processing for any of them,
            'because generating a transfer adds Trx to two registers.
            RaiseEvent LoadStatus("Generate for " + mstrTitle)
            For Each objReg In mcolRegisters
                gCreateGeneratedTrx(Me, objReg, datRegisterEndDate, datCutoff)
            Next objReg
        Catch ex As Exception
            Throw New Exception("Error in Account.LoadGenerated(" & mstrFileNameRoot & ")", ex)
        End Try
    End Sub

    Public Sub LoadApply()
        Try
            RaiseEvent LoadStatus("Apply for " + mstrTitle)
            For Each objReg As Register In mcolRegisters
                objReg.LoadApply()
            Next

            Exit Sub
        Catch ex As Exception
            Throw New Exception("Error in Account.LoadApply(" & mstrFileNameRoot & ")", ex)
        End Try
    End Sub

    Public Sub LoadFinish()
        Try
            'This has to happen after all Account objects are loaded, not when the "AR" lines are read.
            mobjRelatedAcct1 = objLoadResolveRelatedAccount(mintRelatedKey1)
            mobjRelatedAcct2 = objLoadResolveRelatedAccount(mintRelatedKey2)
            mobjRelatedAcct3 = objLoadResolveRelatedAccount(mintRelatedKey3)
            mobjRelatedAcct4 = objLoadResolveRelatedAccount(mintRelatedKey4)

            'Construct repeat key StringTranslator from actual transaction
            'data and info in .GEN files.
            mobjRepeats = mobjRepeatSummarizer.BuildStringTranslator()

            'Call LoadPostProcessing after everything has been loaded.
            RaiseEvent LoadStatus("Finish " + mstrTitle)
            For Each objReg As Register In mcolRegisters
                objReg.LoadFinish()
            Next

            Exit Sub
        Catch ex As Exception
            Throw New Exception("Error in Account.LoadFinish(" & mstrFileNameRoot & ")", ex)
        End Try
    End Sub

    Private Function objLoadResolveRelatedAccount(ByVal intRelatedKey As Integer) As Account
        If intRelatedKey = 0 Then
            Return Nothing
        End If
        For Each objAccount As Account In mobjCompany.colAccounts
            If objAccount.intKey = intRelatedKey Then
                Return objAccount
            End If
        Next
        Throw New Exception("Related account key " + intRelatedKey.ToString() + " not found")
    End Function

    '$Description Remove all generated Trx from Registers for this account,
    '   then recreate generated Trx through the specified end date.

    Public Sub RecreateGeneratedTrx(ByVal datRegisterEndDate As Date, ByVal datCutoff As Date)
        Dim objReg As Register

        'Purge generated Trx and clear all budget allocations for each register.
        For Each objReg In mcolRegisters
            objReg.FireHideTrx()
            objReg.PurgeGenerated()
        Next objReg

        'Generate all Trx.
        'Have to generate for all registers before computing
        'balances or doing any post processing for any of them,
        'because generating a transfer adds Trx to two registers.
        'This only takes 5 to 10 percent of the total time spent
        'in this routine. The rest is divided fairly evenly between
        'LoadPostProcessing() and FireRedisplayTrx().
        For Each objReg In mcolRegisters
            gCreateGeneratedTrx(Me, objReg, datRegisterEndDate, datCutoff)
        Next objReg

        'In case trx generators have been edited.
        mobjRepeats = mobjRepeatSummarizer.BuildStringTranslator()

        'Compute budgets and balances.
        For Each objReg In mcolRegisters
            objReg.LoadApply()
        Next objReg

    End Sub

    Public Sub CreateRegister(ByVal strRegKey As String, ByVal strRegTitle As String, ByVal blnRegShow As Boolean)

        Dim objReg As Register

        If strRegKey = "" Then
            gRaiseError("Missing RK line before RI line")
        End If
        If strRegTitle = "" Then
            gRaiseError("Missing RT line before RI line")
        End If
        If Not objFindReg(strRegKey) Is Nothing Then
            gRaiseError("Reg key already used in RI line")
        End If
        objReg = New Register
        objReg.Init(Me, strRegTitle, strRegKey, blnRegShow, 32, DateAdd(Microsoft.VisualBasic.DateInterval.Day, -1, Today))
        mcolRegisters.Add(objReg)
    End Sub

    Public Sub SetChanged()
        mblnUnsavedChanges = True
        RaiseEvent ChangeMade()
        objCompany.FireSomethingModified()
    End Sub

    Private Sub LoadRegister(ByVal strLine As String, ByVal blnFake As Boolean, ByRef lngLinesRead As Integer)

        Dim strSearchRegKey As String
        Dim objReg As Register

        strSearchRegKey = Mid(strLine, 3)
        objReg = objFindReg(strSearchRegKey)
        If objReg Is Nothing Then
            gRaiseError("Register key " & strSearchRegKey & " not found in " & Left(strLine, 2) & " line")
        Else
            mobjLoader = New RegisterLoader
            mobjLoader.LoadFile(objReg, mobjRepeatSummarizer, blnFake, lngLinesRead)
            mobjLoader = Nothing
        End If
    End Sub

    Private Sub SkipLegacyRegister()
        Dim strLine As String
        Do
            strLine = strReadLine()
            If strLine = ".R" Then
                Exit Sub
            End If
        Loop
    End Sub

    Public Function objFindReg(ByVal strRegisterKey As String) As Register
        Dim objReg As Register
        For Each objReg In mcolRegisters
            If objReg.strRegisterKey = strRegisterKey Then
                objFindReg = objReg
                Exit Function
            End If
        Next objReg
        objFindReg = Nothing
    End Function

    Public Function objRegisterList() As SimpleStringTranslator
        Dim objReg As Register
        Dim objResult As SimpleStringTranslator

        objResult = New SimpleStringTranslator
        For Each objReg In mcolRegisters
            objResult.Add(New StringTransElement(objResult, objReg.strRegisterKey, objReg.strTitle, objReg.strTitle))
        Next objReg
        objRegisterList = objResult
    End Function

    Private Sub mobjLoader_FindRegister(ByVal strRegisterKey As String, ByRef objReg As Register) Handles mobjLoader.FindRegister
        objReg = objFindReg(strRegisterKey)
    End Sub

    Public Sub Save(ByVal strPath_ As String)
        Dim blnFileOpen As Boolean
        Dim objReg As Register
        Dim objSubTypeMatched As SubTypeDef = Nothing

        Try

            mintSaveFile = FreeFile()
            FileOpen(mintSaveFile, mobjCompany.strAccountPath() & "\" & strPath_, OpenMode.Output)
            blnFileOpen = True

            SaveLine("FHCKBK2")
            If mstrTitle <> "" Then
                SaveLine("AT" & mstrTitle)
            End If
            SaveLine("AK" & CStr(mintKey))
            For Each objSubType As SubTypeDef In Account.arrSubTypeDefs
                If objSubType.lngSubType = mlngSubType Then
                    objSubTypeMatched = objSubType
                End If
            Next
            If objSubTypeMatched Is Nothing Then
                Throw New Exception("Could not match account subtype in save for " + mstrTitle)
            End If
            SaveLine("AY" & objSubTypeMatched.strSaveCode)
            'Define each register at the top of the file.
            For Each objReg In mcolRegisters
                If Not objReg.blnDeleted Then
                    SaveDefineRegister(objReg)
                End If
            Next objReg
            SaveRelatedAcct(objRelatedAcct1, "1")
            SaveRelatedAcct(objRelatedAcct2, "2")
            SaveRelatedAcct(objRelatedAcct3, "3")
            SaveRelatedAcct(objRelatedAcct4, "4")
            'Save the transactions for each register.
            For Each objReg In mcolRegisters
                If Not objReg.blnDeleted Then
                    SaveLoadedRegister(objReg)
                End If
            Next objReg
            SaveLine(".A")

            FileClose(mintSaveFile)
            blnFileOpen = False
            mblnUnsavedChanges = False

            Exit Sub
        Catch ex As Exception
            If blnFileOpen Then
                FileClose(mintSaveFile)
            End If
            gNestedException(ex)
        End Try
    End Sub

    Private Sub SaveRelatedAcct(ByVal objRelatedAccount As Account, ByVal strSelector As String)
        If Not objRelatedAccount Is Nothing Then
            SaveLine("AR" + strSelector + " " + objRelatedAccount.intKey.ToString())
        End If
    End Sub

    Private Sub SaveDefineRegister(ByVal objReg As Register)
        With objReg
            SaveLine("RK" & .strRegisterKey)
            SaveLine("RT" & .strTitle)
            If .blnShowInitially Then
                SaveLine("RS")
            End If
            SaveLine("RI")
        End With
    End Sub

    '$Description Save one Register for Save(). Writes real, fake non-generated
    '   and fake generated Trx for LoadedRegister.

    Private Sub SaveLoadedRegister(ByVal objReg As Register)
        Dim objSaver As RegisterSaver
        Dim colFakeLines As ICollection(Of String)
        Dim strLine As String

        objSaver = New RegisterSaver

        'Output the non-fake Trx, and remember the non-generated fake.
        colFakeLines = New List(Of String)
        SaveLine("RL" & objReg.strRegisterKey)
        objSaver.Save(objReg, colFakeLines)
        SaveLine(".R")

        'Save non-generated fake Trx we saved above.
        SaveLine("RF" & objReg.strRegisterKey)
        For Each strLine In colFakeLines
            SaveLine(strLine)
        Next strLine
        SaveLine(".R")

        'RR line is for repeating register, no longer used.

        objReg.LogSave()
        objReg.WriteEventLog(System.IO.Path.GetFileNameWithoutExtension(mstrFileNameRoot), mobjRepeats)
    End Sub

    '$Description Write one line to the Save() output file.
    '$Param strLine The line to write.

    Friend Sub SaveLine(ByVal strLine As String)
        PrintLine(mintSaveFile, strLine)
    End Sub

    Public Overrides Function ToString() As String
        Return Me.strTitle
    End Function

    Public Shared Sub CreateStandardChecking(ByVal objCompany As Company, ByVal objShowMessage As Company.ShowCreateNewMessage)
        Try
            objShowMessage("Creating first checking account...")
            Dim objAccount As Account = New Account()
            objAccount.Init(objCompany)
            objAccount.intKey = objCompany.intGetUnusedAccountKey()
            objAccount.lngSubType = Account.SubType.Asset_CheckingAccount
            objAccount.strFileNameRoot = "Main"
            objAccount.strTitle = "Checking Account"
            objAccount.Create()
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    Public Sub Create()
        Try
            Dim objSubTypeMatched As Account.SubTypeDef = Nothing

            For Each objSubType As Account.SubTypeDef In Account.arrSubTypeDefs
                If objSubType.lngSubType = lngSubType Then
                    objSubTypeMatched = objSubType
                End If
            Next
            If objSubTypeMatched Is Nothing Then
                Throw New Exception("Unrecognized account subtype")
            End If

            Dim strAcctFile As String = mobjCompany.strAccountPath() & "\" & strFileNameRoot & ".act"
            Using objAcctWriter As TextWriter = New StreamWriter(strAcctFile)
                objAcctWriter.WriteLine("FHCKBK2")
                objAcctWriter.WriteLine("AT" & strTitle)
                objAcctWriter.WriteLine("AK" & CStr(intKey))
                objAcctWriter.WriteLine("AY" & objSubTypeMatched.strSaveCode)
                objAcctWriter.WriteLine("RK1")
                objAcctWriter.WriteLine("RT" & strTitle)
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

            Dim strRepeatFile As String = mobjCompany.strAccountPath() & "\" & strFileNameRoot & ".rep"
            Using objRepeatWriter As TextWriter = New StreamWriter(strRepeatFile)
                objRepeatWriter.WriteLine("dummy line")
            End Using
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

End Class