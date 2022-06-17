Option Strict On
Option Explicit On

Imports System.IO
Imports System.Xml.Serialization

Public Class AccountLoader
    Private mobjAccount As Account
    Private mobjCompany As Company
    'RegisterLoader object used by LoadRegister().
    'Is Nothing unless LoadRegister() is on the call stack.
    Private WithEvents mobjLoader As RegisterLoader
    Private mobjLoadFile As TextReader

    Private mintRelatedKey1 As Integer
    Private mintRelatedKey2 As Integer
    Private mintRelatedKey3 As Integer
    Private mintRelatedKey4 As Integer

    Public Sub New(ByVal objAccount As Account)
        mobjAccount = objAccount
        mobjCompany = mobjAccount.Company
    End Sub

    Public ReadOnly Property Account As Account
        Get
            Return mobjAccount
        End Get
    End Property

    '$Description Load a new instance from an account file,
    '   up until the contents of the first register.
    '   Followed by a call to LoadRegisters() to load the registers.
    '   Must always be called immediately after Init().
    '$Param strAcctFile Name of account file, without path.

    Friend Sub LoadStart(ByVal strAcctFile As String)

        Dim strLine As String
        Dim lngLinesRead As Integer
        Dim strRegKey As String = ""
        Dim strRegTitle As String = ""
        Dim blnRegShow As Boolean
        Dim blnAccountPropertiesValidated As Boolean

        Try
            mobjAccount.FileNameRoot = strAcctFile
            mobjAccount.RaiseLoadStatus("Initializing " & mobjAccount.FileNameRoot)
            mobjLoadFile = New StreamReader(mobjCompany.AccountsFolderPath() & "\" & mobjAccount.FileNameRoot)

            strLine = mobjLoadFile.ReadLine()
            lngLinesRead = lngLinesRead + 1
            'The difference between FHCKBK1 and FHCKBK2 is that FHCKBK2 is hardcoded
            'to use a specific budget file name and category file name, and repeat group
            'file (acctfilename).rep instead of getting the file names from FC, FB and FR
            'lines in the .act file.
            If strLine <> "FHCKBK2" Then
                RaiseErrorMsg("Invalid header line")
            End If

            mobjAccount.InitForLoad()

            Do
                strLine = mobjLoadFile.ReadLine()
                lngLinesRead = lngLinesRead + 1
                Select Case Left(strLine, 2)
                    Case "AT"
                        mobjAccount.Title = Mid(strLine, 3)
                    Case "AK"
                        Dim intNewKey As Integer = CInt(Mid(strLine, 3))
                        If mobjCompany.IsAccountKeyUsed(intNewKey) Then
                            Throw New Exception("Duplicate use of account key " & intNewKey)
                        End If
                        mobjAccount.AccountKey = intNewKey
                        mobjCompany.UseAccountKey(mobjAccount.AccountKey)
                    Case "AY"
                        Dim objSubTypeMatched As Account.SubTypeDef = Nothing
                        For Each objSubType In Account.SubTypeDefs
                            If objSubType.SaveCode = Mid(strLine, 3) Then
                                objSubTypeMatched = objSubType
                            End If
                        Next
                        If objSubTypeMatched Is Nothing Then
                            Throw New Exception("Unrecognized account subtype")
                        End If
                        mobjAccount.AcctType = objSubTypeMatched.AcctType
                        mobjAccount.AcctSubType = objSubTypeMatched.AcctSubType
                    Case "AR"
                        Select Case Mid(strLine, 3, 1)
                            Case "1"
                                mintRelatedKey1 = LoadParseRelatedAccountKey(strLine)
                            Case "2"
                                mintRelatedKey2 = LoadParseRelatedAccountKey(strLine)
                            Case "3"
                                mintRelatedKey3 = LoadParseRelatedAccountKey(strLine)
                            Case "4"
                                mintRelatedKey4 = LoadParseRelatedAccountKey(strLine)
                            Case Else
                                Throw New Exception("Invalid related account selector")
                        End Select
                    Case "RK"
                        If Not blnAccountPropertiesValidated Then
                            'The lines that set these properties come before the first "RK" line.
                            If mobjAccount.AccountKey = 0 Then
                                Throw New Exception("Account key not specified")
                            End If
                            If mobjAccount.AcctType = Account.AccountType.Unspecified Then
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
                        mobjAccount.CreateRegister(strRegKey, strRegTitle, blnRegShow)
                        strRegKey = ""
                        strRegTitle = ""
                        blnRegShow = False
                    Case "RL"
                        'Load individual non-fake BaseTrx into Register.
                        'Handled by LoadRegisters().
                        Exit Do
                    Case "RF"
                        'Load individual fake BaseTrx into Register.
                        'Handled by LoadRegisters().
                        Exit Do
                    Case "RR"
                        'Was the repeating register
                        'Handled by LoadRegisters().
                        Exit Do
                    Case ".A"
                        Exit Do
                    Case Else
                        RaiseErrorMsg("Unrecognized line in account file: " & strLine)
                End Select
            Loop
        Catch ex As Exception
            Throw New Exception("Error in Account.LoadIndividual(" & mobjAccount.FileNameRoot & ";" & lngLinesRead & ")", ex)
        Finally
            mobjLoadFile.Close()
            mobjLoadFile = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Load the register contents of an ACT file. Must recognize and skip
    ''' the lines previously loaded by LoadStart().
    ''' </summary>
    Friend Sub LoadRegisters()
        Dim strLine As String
        Dim lngLinesRead As Integer

        Try
            mobjAccount.RaiseLoadStatus("Loading " & mobjAccount.FileNameRoot)
            mobjLoadFile = New StreamReader(mobjCompany.AccountsFolderPath() & "\" & mobjAccount.FileNameRoot)

            strLine = mobjLoadFile.ReadLine()
            lngLinesRead = lngLinesRead + 1
            'The difference between FHCKBK1 and FHCKBK2 is that FHCKBK2 is hardcoded
            'to use a specific budget file name and category file name, and repeat group
            'file (acctfilename).rep instead of getting the file names from FC, FB and FR
            'lines in the .act file.
            If strLine <> "FHCKBK2" Then
                RaiseErrorMsg("Invalid header line")
            End If

            Do
                strLine = mobjLoadFile.ReadLine()
                lngLinesRead = lngLinesRead + 1
                Select Case Left(strLine, 2)
                    Case "AT"
                    Case "AK"
                    Case "AY"
                    Case "AR"
                    Case "RK"
                    Case "RT"
                    Case "RS"
                    Case "RN"
                    Case "RI"
                    Case "RL"
                        'Load individual non-fake BaseTrx into Register.
                        LoadRegister(strLine, False, lngLinesRead)
                    Case "RF"
                        'Load individual fake BaseTrx into Register.
                        LoadRegister(strLine, True, lngLinesRead)
                    Case "RR"
                        'Was the repeating register
                        SkipLegacyRegister()
                    Case ".A"
                        Exit Do
                    Case Else
                        RaiseErrorMsg("Unrecognized line in account file: " & strLine)
                End Select
            Loop
        Catch ex As Exception
            Throw New Exception("Error in Account.LoadIndividual(" & mobjAccount.FileNameRoot & ";" & lngLinesRead & ")", ex)
        Finally
            mobjLoadFile.Close()
            mobjLoadFile = Nothing
        End Try
    End Sub

    Private Function LoadParseRelatedAccountKey(ByVal strLine As String) As Integer
        Dim intRelatedKey As Integer
        If Int32.TryParse(strLine.Substring(4), intRelatedKey) Then
            Return intRelatedKey
        End If
        Throw New Exception("Invalid related account key")
    End Function

    Friend Sub LoadApply()
        Try
            mobjAccount.RaiseLoadStatus("Apply for " + mobjAccount.Title)
            For Each objReg As Register In mobjAccount.Registers
                objReg.LoadApply()
            Next

            Exit Sub
        Catch ex As Exception
            Throw New Exception("Error in AccountLoader.LoadApply(" & mobjAccount.FileNameRoot & ")", ex)
        End Try
    End Sub

    Friend Sub MakeReplicas()
        Try
            mobjAccount.RaiseLoadStatus("Replicas for " + mobjAccount.Title)
            For Each objReg As Register In mobjAccount.Registers
                objReg.MakeReplicas()
            Next

            Exit Sub
        Catch ex As Exception
            Throw New Exception("Error in AccountLoader.MakeReplicas(" & mobjAccount.FileNameRoot & ")", ex)
        End Try
    End Sub

    Friend Sub LoadFinish()
        Try
            'This has to happen after all Account objects are loaded, not when the "AR" lines are read.
            mobjAccount.RelatedAcct1 = LoadResolveRelatedAccount(mintRelatedKey1)
            mobjAccount.RelatedAcct2 = LoadResolveRelatedAccount(mintRelatedKey2)
            mobjAccount.RelatedAcct3 = LoadResolveRelatedAccount(mintRelatedKey3)
            mobjAccount.RelatedAcct4 = LoadResolveRelatedAccount(mintRelatedKey4)

            'Construct repeat key StringTranslator from actual transaction
            'data and info in .GEN files.
            mobjAccount.BuildRepeatList()

            'Call LoadPostProcessing after everything has been loaded.
            mobjAccount.RaiseLoadStatus("Finish " + mobjAccount.Title)
            For Each objReg As Register In mobjAccount.Registers
                objReg.LoadFinish()
                objReg.EndLoading()
            Next
            mobjAccount.HasUnsavedChanges = False

            Exit Sub
        Catch ex As Exception
            Throw New Exception("Error in Account.LoadFinish(" & mobjAccount.FileNameRoot & ")", ex)
        End Try
    End Sub

    Private Function LoadResolveRelatedAccount(ByVal intRelatedKey As Integer) As Account
        If intRelatedKey = 0 Then
            Return Nothing
        End If
        For Each objAccount As Account In mobjCompany.Accounts
            If objAccount.AccountKey = intRelatedKey Then
                Return objAccount
            End If
        Next
        Throw New Exception("Related account key " + intRelatedKey.ToString() + " not found")
    End Function

    '$Description Remove all generated BaseTrx from Registers for this account,
    '   then recreate generated BaseTrx through the specified end date.

    Public Sub RecreateGeneratedTrx(ByVal datRegisterEndDate As Date, ByVal datCutoff As Date)
        Dim objReg As Register
        Dim blnAccountChanged As Boolean = False

        'Purge generated BaseTrx and clear all budget allocations for each register.
        For Each objReg In mobjAccount.Registers
            objReg.PurgeGenerated(blnAccountChanged)
        Next objReg

        'Generate all BaseTrx.
        'Have to generate for all registers before computing
        'balances or doing any post processing for any of them,
        'because generating a transfer adds BaseTrx to two registers.
        'This only takes 5 to 10 percent of the total time spent
        'in this routine. The rest is divided fairly evenly between
        'LoadPostProcessing() and FireRedisplayTrx().
        For Each objReg In mobjAccount.Registers
            CreateAllGeneratedTrx(mobjAccount, objReg, datRegisterEndDate, datCutoff, blnAccountChanged)
        Next objReg

        'In case trx generators have been edited.
        mobjAccount.BuildRepeatList()

        'Compute budgets and balances.
        For Each objReg In mobjAccount.Registers
            objReg.LoadApply()
        Next objReg

        If blnAccountChanged Then
            mobjAccount.SetChanged()
        End If

    End Sub

    Private Sub LoadRegister(ByVal strLine As String, ByVal blnFake As Boolean, ByRef lngLinesRead As Integer)

        Dim strSearchRegKey As String
        Dim objReg As Register

        strSearchRegKey = Mid(strLine, 3)
        objReg = mobjAccount.FindRegister(strSearchRegKey)
        If objReg Is Nothing Then
            RaiseErrorMsg("Register key " & strSearchRegKey & " not found in " & Left(strLine, 2) & " line")
        Else
            objReg.BeginLoading()
            mobjLoader = New RegisterLoader
            mobjLoader.LoadFile(mobjLoadFile, objReg, mobjAccount.RepeatSummarizer, blnFake, lngLinesRead)
            mobjLoader = Nothing
        End If
    End Sub

    Private Sub SkipLegacyRegister()
        Dim strLine As String
        Do
            strLine = mobjLoadFile.ReadLine()
            If strLine = ".R" Then
                Exit Sub
            End If
        Loop
    End Sub

    Private Sub mobjLoader_FindRegister(ByVal strRegisterKey As String, ByRef objReg As Register) Handles mobjLoader.FindRegister
        objReg = mobjAccount.FindRegister(strRegisterKey)
    End Sub

End Class
