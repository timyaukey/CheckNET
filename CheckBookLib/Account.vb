Option Strict Off
Option Explicit On
Public Class Account
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'The master Everything object.
    Private mobjEverything As Everything
    'Path passed to Load().
    Private mstrFileLoaded As String
    'Account title.
    Private mstrTitle As String
    'Repeat key list for account.
    Private mobjRepeats As StringTranslator
    Private mobjRepeatSummarizer As RepeatSummarizer
    'File mobjRepeats was loaded from.
    'Private mstrRepeatsFile As String
    'All LoadedRegister objects for account, whether or not
    'displayed in any UI.
    Private mcolLoadedRegisters As Collection
    'Account has unsaved changes.
    Private mblnUnsavedChanges As Boolean
    'File number for Save().
    Private mintSaveFile As Short
    'RegisterLoader object used by LoadRegister().
    'Is Nothing unless LoadRegister() is on the call stack.
    Private WithEvents mobjLoader As RegisterLoader

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
            If Len(mstrTitle) Then
                strTitle = mstrTitle
            Else
                strTitle = mstrFileLoaded
            End If
        End Get
        Set(ByVal Value As String)
            mstrTitle = Value
            gSetAccountChanged(Me)
        End Set
    End Property

    Public ReadOnly Property colLoadedRegisters() As Collection
        Get
            colLoadedRegisters = mcolLoadedRegisters
        End Get
    End Property

    Public ReadOnly Property objRepeats() As StringTranslator
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

    Public Sub Load(ByVal strAcctFile As String)
        Dim intFile As Short
        Dim strLine As String
        Dim lngLinesRead As Integer
        Dim strRegKey As String = ""
        Dim strRegTitle As String = ""
        Dim blnRegShow As Boolean
        Dim blnRegNonBank As Boolean
        Dim objLoaded As LoadedRegister
        Dim blnFileOpen As Boolean
        Dim datRegisterEndDate As Date

        Try

            mstrFileLoaded = strAcctFile
            mblnUnsavedChanges = False
            datRegisterEndDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, 45, Today)
            mcolLoadedRegisters = New Collection
            RaiseEvent LoadStatus("Loading " & strAcctFile)
            intFile = FreeFile()
            FileOpen(intFile, gstrAccountPath() & "\" & strAcctFile, OpenMode.Input)
            blnFileOpen = True

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

            Do
                strLine = LineInput(intFile)
                lngLinesRead = lngLinesRead + 1
                Select Case Left(strLine, 2)
                    Case "AT"
                        mstrTitle = Mid(strLine, 3)
                    Case "RK"
                        strRegKey = Mid(strLine, 3)
                    Case "RT"
                        strRegTitle = Mid(strLine, 3)
                    Case "RS"
                        blnRegShow = True
                    Case "RN"
                        blnRegNonBank = True
                    Case "RI"
                        CreateRegister(strRegKey, strRegTitle, blnRegShow, blnRegNonBank)
                        strRegKey = ""
                        strRegTitle = ""
                        blnRegShow = False
                        blnRegNonBank = False
                    Case "RL"
                        'Load individual non-fake Trx into Register.
                        LoadRegister(strLine, False, intFile, datRegisterEndDate, lngLinesRead)
                    Case "RF"
                        'Load individual fake Trx into Register.
                        LoadRegister(strLine, True, intFile, datRegisterEndDate, lngLinesRead)
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
            blnFileOpen = False

            'Create generated Trx.
            'Have to generate for all registers before computing
            'balances or doing any post processing for any of them,
            'because generating a transfer adds Trx to two registers.
            RaiseEvent LoadStatus("Creating generated transactions")
            For Each objLoaded In mcolLoadedRegisters
                gCreateGeneratedTrx(Me, objLoaded.objReg, datRegisterEndDate)
            Next objLoaded

            'Construct repeat key StringTranslator from actual transaction
            'data and info in .GEN files.
            mobjRepeats = mobjRepeatSummarizer.BuildStringTranslator()

            'Call LoadPostProcessing after everything has been loaded.
            RaiseEvent LoadStatus("Load postprocessing")
            For Each objLoaded In mcolLoadedRegisters
                With objLoaded.objReg
                    .LoadPostProcessing()
                    'If .datOldestFakeNormal < DateAdd("d", -10, Date) And _
                    ''    .datOldestFakeNormal <> 0 Then
                    '    MsgBox "NOTE: The oldest fake normal transaction in register """ & _
                    ''        .strTitle & """ is suspiciously old, dated " & _
                    ''        Format$(.datOldestFakeNormal, gstrFORMAT_DATE) & ".", _
                    ''        vbInformation
                    'End If
                End With
            Next objLoaded

            RaiseEvent LoadStatus("Load complete")

            Exit Sub

        Catch ex As Exception
            If blnFileOpen Then
                FileClose(intFile)
            End If
            Throw New Exception("Error in Account.Load(" & strAcctFile & ";" & lngLinesRead & ")", ex)
        End Try
    End Sub

    '$Description Remove all generated Trx from Registers for this account,
    '   then recreate generated Trx through the specified end date.

    Public Sub RecreateGeneratedTrx(ByVal datRegisterEndDate As Date)
        Dim objLoaded As LoadedRegister

        'Purge generated Trx and clear all budget allocations for each register.
        For Each objLoaded In mcolLoadedRegisters
            objLoaded.objReg.FireHideTrx()
            objLoaded.objReg.PurgeGenerated()
        Next objLoaded

        'Generate all Trx.
        'Have to generate for all registers before computing
        'balances or doing any post processing for any of them,
        'because generating a transfer adds Trx to two registers.
        'This only takes 5 to 10 percent of the total time spent
        'in this routine. The rest is divided fairly evenly between
        'LoadPostProcessing() and FireRedisplayTrx().
        For Each objLoaded In mcolLoadedRegisters
            gCreateGeneratedTrx(Me, objLoaded.objReg, datRegisterEndDate)
        Next objLoaded

        'Compute budgets and balances.
        For Each objLoaded In mcolLoadedRegisters
            objLoaded.objReg.LoadPostProcessing()
        Next objLoaded

        'Tell all register windows to refresh themselves.
        For Each objLoaded In mcolLoadedRegisters
            objLoaded.objReg.FireRedisplayTrx()
        Next objLoaded

    End Sub

    Public Sub CreateRegister(ByVal strRegKey As String, ByVal strRegTitle As String, ByVal blnRegShow As Boolean, ByVal blnRegNonBank As Boolean)

        Dim objReg As Register
        Dim objLoaded As LoadedRegister

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
        objReg.Init(strRegTitle, strRegKey, blnRegShow, blnRegNonBank, 32, DateAdd(Microsoft.VisualBasic.DateInterval.Day, -1, Today), False)
        objLoaded = New LoadedRegister
        objLoaded.Init(Me, objReg)
        mcolLoadedRegisters.Add(objLoaded)
    End Sub

    Private Sub LoadRegister(ByVal strLine As String, ByVal blnFake As Boolean, ByVal intFile As Short, ByVal datRptEndMax As Date, ByRef lngLinesRead As Integer)

        Dim strSearchRegKey As String
        Dim objLoaded As LoadedRegister

        strSearchRegKey = Mid(strLine, 3)
        objLoaded = objFindReg(strSearchRegKey)
        If objLoaded Is Nothing Then
            gRaiseError("Register key " & strSearchRegKey & " not found in " & Left(strLine, 2) & " line")
        Else
            mobjLoader = New RegisterLoader
            mobjLoader.LoadFile(objLoaded.objReg, mobjRepeatSummarizer, intFile, blnFake, datRptEndMax, lngLinesRead)
            'UPGRADE_NOTE: Object mobjLoader may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            mobjLoader = Nothing
        End If
    End Sub

    Private Sub SkipLegacyRegister(ByVal intFile As Short)
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

    Public Function objFindReg(ByVal strRegisterKey As String) As LoadedRegister
        Dim objLoaded As LoadedRegister
        For Each objLoaded In mcolLoadedRegisters
            If objLoaded.objReg.strRegisterKey = strRegisterKey Then
                objFindReg = objLoaded
                Exit Function
            End If
        Next objLoaded
        objFindReg = Nothing
    End Function

    Public Function objRegisterList() As StringTranslator
        Dim objLoaded As LoadedRegister
        Dim objReg As Register
        Dim objResult As StringTranslator

        objResult = New StringTranslator
        For Each objLoaded In mcolLoadedRegisters
            objReg = objLoaded.objReg
            objResult.Add(objReg.strRegisterKey, objReg.strTitle, objReg.strTitle)
        Next objLoaded
        objRegisterList = objResult
    End Function

    'UPGRADE_NOTE: ChangeMade was upgraded to ChangeMade_Renamed. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Public Sub ChangeMade_Renamed()
        mblnUnsavedChanges = True
        RaiseEvent ChangeMade()
    End Sub

    Private Sub mobjLoader_FindRegister(ByVal strRegisterKey As String, ByRef objReg As Register) Handles mobjLoader.FindRegister
        objReg = objFindReg(strRegisterKey).objReg
    End Sub

    Public Sub Save(ByVal strPath_ As String)
        Dim blnFileOpen As Boolean
        Dim objLoaded As LoadedRegister

        On Error GoTo ErrorHandler

        mintSaveFile = FreeFile()
        FileOpen(mintSaveFile, gstrAccountPath() & "\" & strPath_, OpenMode.Output)
        blnFileOpen = True

        SaveLine("FHCKBK2")
        If mstrTitle <> "" Then
            SaveLine("AT" & mstrTitle)
        End If
        'Define each register at the top of the file.
        For Each objLoaded In mcolLoadedRegisters
            If Not objLoaded.blnDeleted Then
                SaveDefineRegister(objLoaded)
            End If
        Next objLoaded
        'Save the transactions for each register.
        For Each objLoaded In mcolLoadedRegisters
            If Not objLoaded.blnDeleted Then
                SaveLoadedRegister(objLoaded)
            End If
        Next objLoaded
        SaveLine(".A")

        FileClose(mintSaveFile)
        blnFileOpen = False
        mblnUnsavedChanges = False

        Exit Sub
ErrorHandler:
        If blnFileOpen Then
            FileClose(mintSaveFile)
        End If
        gNestedErrorTrap("Account.Save(" & strPath_ & ")")
    End Sub

    Private Sub SaveDefineRegister(ByVal objLoaded As LoadedRegister)
        With objLoaded.objReg
            SaveLine("RK" & .strRegisterKey)
            SaveLine("RT" & .strTitle)
            If .blnShowInitially Then
                SaveLine("RS")
            End If
            If .blnNonBank Then
                SaveLine("RN")
            End If
            SaveLine("RI")
        End With
    End Sub

    '$Description Save one LoadedRegister for Save(). Writes real, fake non-generated
    '   and fake generated Trx for LoadedRegister.

    Private Sub SaveLoadedRegister(ByVal objLoaded As LoadedRegister)
        Dim objSaver As RegisterSaver
        Dim colFakeLines As Collection
        Dim vstrLine As Object
        Dim objReg As Register

        objReg = objLoaded.objReg
        objSaver = New RegisterSaver

        'Output the non-fake Trx, and remember the non-generated fake.
        colFakeLines = New Collection
        SaveLine("RL" & objReg.strRegisterKey)
        objSaver.Save(objReg, mintSaveFile, colFakeLines)
        SaveLine(".R")

        'Save non-generated fake Trx we saved above.
        SaveLine("RF" & objReg.strRegisterKey)
        For Each vstrLine In colFakeLines
            'UPGRADE_WARNING: Couldn't resolve default property of object vstrLine. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            SaveLine(vstrLine)
        Next vstrLine
        SaveLine(".R")

        'RR line is for repeating register, no longer used.

        objLoaded.objReg.LogSave()
        objLoaded.objReg.WriteEventLog(mstrTitle, mobjRepeats)
    End Sub

    '$Description Write one line to the Save() output file.
    '$Param strLine The line to write.

    Private Sub SaveLine(ByVal strLine As String)
        PrintLine(mintSaveFile, strLine)
    End Sub
End Class