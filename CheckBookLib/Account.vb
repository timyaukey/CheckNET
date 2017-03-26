Option Strict On
Option Explicit On

Public Class Account
    'The master Everything object.
    Private mobjEverything As Everything
    'Path passed to Load().
    Private mstrFileLoaded As String
    'Account title.
    Private mstrTitle As String
    'Unique number identifying this Account from others loaded at the same time.
    Private mintKey As Integer
    Private mlngType As AccountType
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
    'RegisterLoader object used by LoadRegister().
    'Is Nothing unless LoadRegister() is on the call stack.
    Private WithEvents mobjLoader As RegisterLoader

    Public Enum AccountType
        Unspecified = 0
        Asset = 1
        Liability = 2
        Equity = 3
    End Enum

    'Fired when ChangeMade() is called. Used by clients
    'sensitive to changes in the Account as a whole,
    'for example anything that remembers the index of a Trx
    'in its Register.
    Public Event ChangeMade()

    'Fired multiple times by register loading process.
    Public Event LoadStatus(ByVal strMessage As String)

    '$Description Initialize a new instance.
    '   Must always be the first member used for a new instance.

    Public Sub Init(ByVal objEverything As Everything)
        mobjEverything = objEverything
    End Sub

    Public ReadOnly Property objEverything() As Everything
        Get
            objEverything = mobjEverything
        End Get
    End Property

    Public ReadOnly Property strFileLoaded() As String
        Get
            strFileLoaded = mstrFileLoaded
        End Get
    End Property

    Public Property strTitle() As String
        Get
            If Len(mstrTitle) > 0 Then
                strTitle = mstrTitle
            Else
                strTitle = mstrFileLoaded
            End If
        End Get
        Set(ByVal Value As String)
            mstrTitle = Value
            SetChanged()
        End Set
    End Property

    Public ReadOnly Property intKey() As Integer
        Get
            Return mintKey
        End Get
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
        'UPGRADE_NOTE: Object mobjEverything may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        mobjEverything = Nothing
    End Sub

    '$Description Load a new instance from an account file.
    '   Must always be called immediately after Init().
    '$Param strAcctFile Name of account file, without path.

    Public Sub LoadStart(ByVal strAcctFile As String)

        RaiseEvent LoadStatus("Loading " & strAcctFile)
        mstrFileLoaded = strAcctFile
        mblnUnsavedChanges = False
        mcolRegisters = New List(Of Register)
        LoadIndividual()
        'LoadGenerated()
    End Sub

    Private Sub LoadIndividual()
        Dim intFile As Integer
        Dim strLine As String
        Dim lngLinesRead As Integer
        Dim strRegKey As String = ""
        Dim strRegTitle As String = ""
        Dim blnRegShow As Boolean
        Dim blnAccountPropertiesValidated As Boolean

        Try

            intFile = FreeFile()
            FileOpen(intFile, gstrAccountPath() & "\" & mstrFileLoaded, OpenMode.Input)

            strLine = LineInput(intFile)
            lngLinesRead = lngLinesRead + 1
            'The difference between FHCKBK1 and FHCKBK2 is that FHCKBK2 is hardcoded
            'to use budget file Shared.bud, category file Shared.cat, and repeat group
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
                strLine = LineInput(intFile)
                lngLinesRead = lngLinesRead + 1
                Select Case Left(strLine, 2)
                    Case "AT"
                        mstrTitle = Mid(strLine, 3)
                    Case "AK"
                        Dim intNewKey As Integer = CInt(Mid(strLine, 3))
                        If mobjEverything.blnAccountKeyUsed(intNewKey) Then
                            Throw New Exception("Duplicate use of account key " & intNewKey)
                        End If
                        mintKey = intNewKey
                        mobjEverything.UseAccountKey(mintKey)
                    Case "AY"
                        Select Case Mid(strLine, 3, 1)
                            Case "A"
                                mlngType = AccountType.Asset
                            Case "L"
                                mlngType = AccountType.Liability
                            Case "E"
                                mlngType = AccountType.Equity
                            Case Else
                                Throw New Exception("Unrecognized account type")
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
                        LoadRegister(strLine, False, intFile, lngLinesRead)
                    Case "RF"
                        'Load individual fake Trx into Register.
                        LoadRegister(strLine, True, intFile, lngLinesRead)
                    Case "RR"
                        'Was the repeating register
                        SkipLegacyRegister(intFile)
                    Case ".A"
                        Exit Do
                    Case Else
                        gRaiseError("Unrecognized line in account file: " & strLine)
                End Select
            Loop

            FileClose(intFile)
        Catch ex As Exception
            FileClose(intFile)
            Throw New Exception("Error in Account.LoadIndividual(" & mstrFileLoaded & ";" & lngLinesRead & ")", ex)
        End Try
    End Sub

    Public Sub LoadGenerated()
        Dim objReg As Register

        Try
            Dim datRegisterEndDate As Date = DateAdd(Microsoft.VisualBasic.DateInterval.Day, 45, Today)
            'Create generated Trx.
            'Have to generate for all registers before computing
            'balances or doing any post processing for any of them,
            'because generating a transfer adds Trx to two registers.
            RaiseEvent LoadStatus("Generate for " + mstrTitle)
            For Each objReg In mcolRegisters
                gCreateGeneratedTrx(Me, objReg, datRegisterEndDate)
            Next objReg
        Catch ex As Exception
            Throw New Exception("Error in Account.LoadGenerated(" & mstrFileLoaded & ")", ex)
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
            Throw New Exception("Error in Account.LoadApply(" & mstrFileLoaded & ")", ex)
        End Try
    End Sub

    Public Sub LoadFinish()
        Try

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
            Throw New Exception("Error in Account.LoadFinish(" & mstrFileLoaded & ")", ex)
        End Try
    End Sub

    '$Description Remove all generated Trx from Registers for this account,
    '   then recreate generated Trx through the specified end date.

    Public Sub RecreateGeneratedTrx(ByVal datRegisterEndDate As Date)
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
            gCreateGeneratedTrx(Me, objReg, datRegisterEndDate)
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
        objEverything.FireSomethingModified()
    End Sub

    Private Sub LoadRegister(ByVal strLine As String, ByVal blnFake As Boolean, ByVal intFile As Integer, ByRef lngLinesRead As Integer)

        Dim strSearchRegKey As String
        Dim objReg As Register

        strSearchRegKey = Mid(strLine, 3)
        objReg = objFindReg(strSearchRegKey)
        If objReg Is Nothing Then
            gRaiseError("Register key " & strSearchRegKey & " not found in " & Left(strLine, 2) & " line")
        Else
            mobjLoader = New RegisterLoader
            mobjLoader.LoadFile(objReg, mobjRepeatSummarizer, intFile, blnFake, lngLinesRead)
            mobjLoader = Nothing
        End If
    End Sub

    Private Sub SkipLegacyRegister(ByVal intFile As Integer)
        Dim strLine As String
        Do
            If EOF(intFile) Then
                gRaiseError("End of file encountered skipping legacy RR register")
            End If
            strLine = LineInput(intFile)
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
            objResult.Add(New StringTransElement(objReg.strRegisterKey, objReg.strTitle, objReg.strTitle))
        Next objReg
        objRegisterList = objResult
    End Function

    Private Sub mobjLoader_FindRegister(ByVal strRegisterKey As String, ByRef objReg As Register) Handles mobjLoader.FindRegister
        objReg = objFindReg(strRegisterKey)
    End Sub

    Public Sub Save(ByVal strPath_ As String)
        Dim blnFileOpen As Boolean
        Dim objReg As Register
        Dim strAcctType As String

        Try

            mintSaveFile = FreeFile()
            FileOpen(mintSaveFile, gstrAccountPath() & "\" & strPath_, OpenMode.Output)
            blnFileOpen = True

            SaveLine("FHCKBK2")
            If mstrTitle <> "" Then
                SaveLine("AT" & mstrTitle)
            End If
            SaveLine("AK" & CStr(mintKey))
            Select Case mlngType
                Case AccountType.Asset
                    strAcctType = "A"
                Case AccountType.Liability
                    strAcctType = "L"
                Case AccountType.Equity
                    strAcctType = "E"
                Case Else
                    Throw New Exception("Unrecognized account type")
            End Select
            SaveLine("AY" & strAcctType)
            'Define each register at the top of the file.
            For Each objReg In mcolRegisters
                If Not objReg.blnDeleted Then
                    SaveDefineRegister(objReg)
                End If
            Next objReg
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
        objSaver.Save(objReg, mintSaveFile, colFakeLines)
        SaveLine(".R")

        'Save non-generated fake Trx we saved above.
        SaveLine("RF" & objReg.strRegisterKey)
        For Each strLine In colFakeLines
            SaveLine(strLine)
        Next strLine
        SaveLine(".R")

        'RR line is for repeating register, no longer used.

        objReg.LogSave()
        objReg.WriteEventLog(mstrTitle, mobjRepeats)
    End Sub

    '$Description Write one line to the Save() output file.
    '$Param strLine The line to write.

    Private Sub SaveLine(ByVal strLine As String)
        PrintLine(mintSaveFile, strLine)
    End Sub

    Public Overrides Function ToString() As String
        Return Me.strTitle
    End Function
End Class