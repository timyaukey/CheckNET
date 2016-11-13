Option Strict Off
Option Explicit On
Public Class RegisterLoader
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Return the Register object for the specified register key, or Nothing.
    Public Event FindRegister(ByVal strRegisterKey As String, ByRef objReg As Register)

    Private mstrLine As String
    Private mlngLinesRead As Integer
    Private mobjReg As Register
    Private mobjRepeatSummarizer As RepeatSummarizer

    'Transaction data.
    Private mstrDescription As String
    Private mlngStatus As Trx.TrxStatus
    Private mlngType As Trx.TrxType
    Private mstrNumber As String
    Private mdatDate As Date
    Private mstrMemo As String
    Private mcurAmount As Decimal
    Private mcurNormalMatchRange As Decimal
    Private mblnAwaitingReview As Boolean
    Private mdatBudgetEnds As Date
    Private mlngBudgetUnit As Trx.RepeatUnit
    Private mintBudgetNumber As Short
    Private mstrImportKey As String
    Private mstrTransferKey As String
    Private mstrBudgetKey As String
    Private mintRepeatSeq As Short
    Private mstrRepeatKey As String
    Private mblnFake As Boolean

    'Generate Trx repeat specifications.
    'Unit the repeat interval is stated in.
    Private mlngRptUnit As Trx.RepeatUnit
    'Number of units in repeat interval.
    Private mintRptNumber As Short
    'Ending date of this repeat series.
    Private mdatRptEnd As Date
    'Max ending date of any repeat series.
    Private mdatRptEndMax As Date

    'Split data.
    Private mstrSMemo As String
    Private mstrSCategoryKey As String
    Private mstrSPONumber As String
    Private mstrSInvoiceNum As String
    Private mdatSInvoiceDate As Date
    Private mdatSDueDate As Date
    Private mstrSTerms As String
    Private mstrSBudgetKey As String
    Private mcurSAmount As Decimal
    Private mstrSImageFiles As String
    Private mcolSplits As Collection

    '$Description Load transactions into an existing register from an open file handle.
    '   Does not clear the existing register contents before starting, so can be used
    '   to load a single Register with data from multiple files.
    '$Param objReg The Register object to load.
    '$Param intFile The open file to load from. Loads transactions until a line starting
    '   with ".R" (end of register) is found. Leaves file open, positioned so the line
    '   "Line Input" will read the line following the ".R".
    '$Param blnFake The blnFake value to construct new transactions with.
    '$Param lngLinesRead The number of lines read from the file. On entry is the number
    '   of lines previously read, on exit is incremented to include lines read by
    '   this method.

    Public Sub LoadFile(ByVal objReg As Register, ByVal objRepeatSummarizer As RepeatSummarizer, _
            ByVal intFile As Short, ByVal blnFake As Boolean, ByVal datRptEndMax_ As Date, ByRef lngLinesRead As Integer)

        Try

            LoadInit(objReg, objRepeatSummarizer, blnFake, datRptEndMax_, lngLinesRead)

            Do
                If EOF(intFile) Then
                    RaiseErrorInLoad("End of file encountered")
                End If
                mstrLine = LineInput(intFile)
                If blnLoadLine(lngLinesRead) Then
                    Exit Sub
                End If
            Loop

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub

    '$Description Used only for unit tests, because in .NET file numbers are local
    '   to the assembly that opens them.

    Public Sub LoadFileUT(ByVal objReg As Register, ByVal objRepeatSummarizer As RepeatSummarizer, ByVal strFile As String, ByVal blnFake As Boolean, ByVal datRptEndMax_ As Date, ByRef lngLinesRead As Integer)

        Dim intFile As Short

        intFile = FreeFile()
        FileOpen(intFile, strFile, OpenMode.Input)
        LoadFile(objReg, objRepeatSummarizer, intFile, False, #1/1/1980#, lngLinesRead)
        FileClose(intFile)

    End Sub

    Private Sub LoadInit(ByVal objReg As Register, ByVal objRepeatSummarizer As RepeatSummarizer, ByVal blnFake As Boolean, ByVal datRptEndMax_ As Date, ByRef lngLinesRead As Integer)

        mobjReg = objReg
        mobjRepeatSummarizer = objRepeatSummarizer
        mblnFake = blnFake
        mdatRptEndMax = datRptEndMax_
        mlngLinesRead = lngLinesRead
        ClearTrxData()
    End Sub

    Private Function blnLoadLine(ByRef lngLinesRead As Integer) As Boolean
        Dim objSplit As Split_Renamed

        blnLoadLine = False
        mlngLinesRead = mlngLinesRead + 1
        lngLinesRead = mlngLinesRead
        'TN, TB or TT line is required, and must precede any line which needs
        'to know the type of transaction.
        Select Case Left(mstrLine, 2)
            Case ".R" 'End of register.
                blnLoadLine = True
                Exit Function
            Case "TN" 'Normal transaction header.
                mlngType = Trx.TrxType.glngTRXTYP_NORMAL
                Select Case Mid(mstrLine, 3, 1)
                    Case "U"
                        mlngStatus = Trx.TrxStatus.glngTRXSTS_UNREC
                    Case "R"
                        mlngStatus = Trx.TrxStatus.glngTRXSTS_RECON
                    Case "S"
                        mlngStatus = Trx.TrxStatus.glngTRXSTS_SELECTED
                    Case Else
                        RaiseErrorInLoad("Normal Trx status may only be U or R")
                End Select
                mstrDescription = Mid(mstrLine, 4)
            Case "TB" 'Budget transaction header.
                mlngType = Trx.TrxType.glngTRXTYP_BUDGET
                If Mid(mstrLine, 3, 1) <> "N" Then
                    RaiseErrorInLoad("Budget Trx status may only be N")
                End If
                mstrDescription = Mid(mstrLine, 4)
            Case "TT" 'Transfer transaction header.
                mlngType = Trx.TrxType.glngTRXTYP_TRANSFER
                If Mid(mstrLine, 3, 1) <> "N" Then
                    RaiseErrorInLoad("Transfer Trx status may only be N")
                End If
                mstrDescription = Mid(mstrLine, 4)
            Case "N#"
                mstrNumber = Mid(mstrLine, 3)
            Case "DT"
                mdatDate = datConvertInput(Mid(mstrLine, 3), "transaction")
            Case "ME"
                mstrMemo = Mid(mstrLine, 3)
            Case "A$"
                If mlngType <> Trx.TrxType.glngTRXTYP_BUDGET And mlngType <> Trx.TrxType.glngTRXTYP_TRANSFER Then
                    RaiseErrorInLoad("A$ line only allowed for budget and transfer Trx")
                End If
                If Not IsNumeric(Mid(mstrLine, 3)) Then
                    RaiseErrorInLoad("Invalid A$ amount")
                End If
                mcurAmount = CDec(Mid(mstrLine, 3))
            Case "MR"
                If mlngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                    RaiseErrorInLoad("MR line only allowed for normal Trx")
                End If
                If Not IsNumeric(Mid(mstrLine, 3)) Then
                    RaiseErrorInLoad("Invalid MR amount")
                End If
                mcurNormalMatchRange = CDec(Mid(mstrLine, 3))
            Case "AR"
                mblnAwaitingReview = True
            Case "BE"
                If mlngType <> Trx.TrxType.glngTRXTYP_BUDGET Then
                    RaiseErrorInLoad("BE line only allowed for budget Trx")
                End If
                mdatBudgetEnds = datConvertInput(Mid(mstrLine, 3), "budget ending")
            Case "BU"
                mlngBudgetUnit = lngConvertRepeatUnit(Mid(mstrLine, 3), "BU line")
            Case "BN"
                mintBudgetNumber = intConvertRepeatCount(Mid(mstrLine, 3), "BN line")
            Case "KI"
                If mlngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                    RaiseErrorInLoad("KI line only allowed for normal Trx")
                End If
                mstrImportKey = Mid(mstrLine, 3)
            Case "RS"
                mintRepeatSeq = Val(Mid(mstrLine, 3))
            Case "KR"
                mstrRepeatKey = Mid(mstrLine, 3)
            Case "KT"
                If mlngType <> Trx.TrxType.glngTRXTYP_TRANSFER Then
                    RaiseErrorInLoad("KT line only allowed for transfer Trx")
                End If
                mstrTransferKey = Mid(mstrLine, 3)
            Case "KB"
                If mlngType <> Trx.TrxType.glngTRXTYP_BUDGET Then
                    RaiseErrorInLoad("KB line only allowed for budget Trx")
                End If
                mstrBudgetKey = Mid(mstrLine, 3)
            Case ".T"
                CreateTrx()
                ClearTrxData()
            Case "GU"
                'This line will be ignored in contexts where it is not used.
                mlngRptUnit = lngConvertRepeatUnit(Mid(mstrLine, 3), "GU line")
            Case "GN"
                'This line will be ignored in contexts where it is not used.
                mintRptNumber = intConvertRepeatCount(Mid(mstrLine, 3), "GN line")
            Case "GE"
                'This line will be ignored in contexts where it is not used.
                mdatRptEnd = datConvertInput(Mid(mstrLine, 3), "repeat sequence end")
            Case "SM"
                If mlngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                    RaiseErrorInLoad("SM only allowed for normal Trx")
                End If
                mstrSMemo = Mid(mstrLine, 3)
            Case "SC"
                If mlngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                    RaiseErrorInLoad("SC only allowed for normal Trx")
                End If
                mstrSCategoryKey = Mid(mstrLine, 3)
            Case "SP"
                If mlngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                    RaiseErrorInLoad("SP only allowed for normal Trx")
                End If
                mstrSPONumber = Mid(mstrLine, 3)
            Case "SN"
                If mlngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                    RaiseErrorInLoad("SN only allowed for normal Trx")
                End If
                mstrSInvoiceNum = Mid(mstrLine, 3)
            Case "SI"
                If mlngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                    RaiseErrorInLoad("SI only allowed for normal Trx")
                End If
                mdatSInvoiceDate = datConvertInput(Mid(mstrLine, 3), "invoice")
            Case "SD"
                If mlngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                    RaiseErrorInLoad("SD only allowed for normal Trx")
                End If
                mdatSDueDate = datConvertInput(Mid(mstrLine, 3), "due")
            Case "ST"
                If mlngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                    RaiseErrorInLoad("ST only allowed for normal Trx")
                End If
                mstrSTerms = Mid(mstrLine, 3)
            Case "SB"
                If mlngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                    RaiseErrorInLoad("SB only allowed for normal Trx")
                End If
                mstrSBudgetKey = Mid(mstrLine, 3)
            Case "SA"
                If mlngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                    RaiseErrorInLoad("SA only allowed for normal Trx")
                End If
                If Not IsNumeric(Mid(mstrLine, 3)) Then
                    RaiseErrorInLoad("Invalid SA amount")
                End If
                mcurSAmount = CDec(Mid(mstrLine, 3))
            Case "SF"
                If mlngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                    RaiseErrorInLoad("SF only allowed for normal Trx")
                End If
                mstrSImageFiles = Mid(mstrLine, 3)
            Case "SZ"
                If mlngType <> Trx.TrxType.glngTRXTYP_NORMAL Then
                    RaiseErrorInLoad("SZ only allowed for normal Trx")
                End If
                objSplit = New Split_Renamed
                objSplit.Init(mstrSMemo, mstrSCategoryKey, mstrSPONumber, mstrSInvoiceNum, mdatSInvoiceDate, mdatSDueDate, mstrSTerms, mstrSBudgetKey, mcurSAmount, mstrSImageFiles)
                mcolSplits.Add(objSplit)
                ClearSplitData()
            Case Else
                RaiseErrorInLoad("Unrecognized line type")
        End Select
    End Function

    Private Sub ClearTrxData()
        mstrDescription = ""
        mlngStatus = Trx.TrxStatus.gintTRXSTS_MISSING
        mlngType = Trx.TrxType.glngTRXTYP_MISSING
        mstrNumber = ""
        mdatDate = System.DateTime.FromOADate(0)
        mstrMemo = ""
        mcurAmount = 0
        mcurNormalMatchRange = 0
        mblnAwaitingReview = False
        mdatBudgetEnds = System.DateTime.FromOADate(0)
        mlngBudgetUnit = Trx.RepeatUnit.glngRPTUNT_MISSING
        mintBudgetNumber = 0
        mstrImportKey = ""
        mstrTransferKey = ""
        mstrBudgetKey = ""
        mintRepeatSeq = 0
        mstrRepeatKey = ""

        mlngRptUnit = Trx.RepeatUnit.glngRPTUNT_MISSING
        mintRptNumber = 0
        mdatRptEnd = System.DateTime.FromOADate(0)

        ClearSplitData()
        mcolSplits = New Collection
    End Sub

    Private Sub ClearSplitData()
        mstrSMemo = ""
        mstrSCategoryKey = ""
        mstrSPONumber = ""
        mstrSInvoiceNum = ""
        mdatSInvoiceDate = System.DateTime.FromOADate(0)
        mdatSDueDate = System.DateTime.FromOADate(0)
        mstrSTerms = ""
        mstrSBudgetKey = ""
        mcurSAmount = 0
        mstrSImageFiles = ""
    End Sub

    Private Sub RaiseError(ByVal strRoutine As String, ByVal strMsg As String)
        gRaiseError("Error in RegisterLoader." & strRoutine & vbCrLf & "Line " & mlngLinesRead & " in register file: " & mstrLine & vbCrLf & strMsg)
    End Sub

    Private Sub RaiseErrorInLoad(ByVal strMsg As String)
        RaiseError("blnLoadLine", strMsg)
    End Sub

    Private Function datConvertInput(ByVal strInput As String, ByVal strContext As String) As Date

        If gblnValidDate(strInput) Then
            datConvertInput = CDate(strInput)
            Exit Function
        End If
        RaiseError("datConvertInput", "Invalid " & strContext & " date")

    End Function

    Private Function lngConvertRepeatUnit(ByVal strInput As String, ByVal strContext As String) As Trx.RepeatUnit

        Dim lngResult As Trx.RepeatUnit
        lngResult = glngConvertRepeatUnit(strInput)
        If lngResult = Trx.RepeatUnit.glngRPTUNT_MISSING Then
            RaiseErrorInLoad("Unrecognized unit name in " & strContext)
        End If
        lngConvertRepeatUnit = lngResult
    End Function

    Private Function intConvertRepeatCount(ByVal strInput As String, ByVal strContext As String) As Short

        Dim intResult As Short

        intResult = gintConvertRepeatCount(strInput)
        If intResult = 0 Then
            RaiseErrorInLoad(strContext & " has non-numeric or non-positive value")
        End If
        intConvertRepeatCount = intResult
    End Function

    Private Sub CreateTrx()
        If mlngType = Trx.TrxType.glngTRXTYP_MISSING Then
            RaiseError("CreateTrx", "No TN, TB or TT line before TZ")
        End If
        If mdatDate = System.DateTime.FromOADate(0) Then
            RaiseError("CreateTrx", "No DT line for Trx")
        End If
        CreateOneTrx(mobjReg, mblnFake)
    End Sub

    Private Sub CreateOneTrx(ByVal objTargetReg As Register, ByVal blnFake As Boolean)

        Dim objTrx As Trx
        Dim objSplit As Split_Renamed
        Dim blnAutoGenerated As Boolean

        Try

            blnAutoGenerated = False

            'Hack to assign initial repeat keys.
            Dim intRepeatKey As Short
            intRepeatKey = Val(mstrRepeatKey)
            If intRepeatKey > 0 And gblnAssignRepeatSeq Then
                mintRepeatSeq = objTargetReg.intGetNextRepeatSeq(intRepeatKey)
            End If

            If mintRepeatSeq > 0 Then
                mobjRepeatSummarizer.Define(mstrRepeatKey, mstrDescription, False)
            End If

            Select Case mlngType
                Case Trx.TrxType.glngTRXTYP_NORMAL
                    objTrx = New Trx
                    If mcolSplits.Count() = 0 Then
                        RaiseError("CreateTrx", "No splits for normal Trx")
                    End If
                    objTrx.NewStartNormal(objTargetReg, mstrNumber, mdatDate, mstrDescription, mstrMemo, mlngStatus, blnFake, _
                                          mcurNormalMatchRange, mblnAwaitingReview, blnAutoGenerated, mintRepeatSeq, _
                                          mstrImportKey, mstrRepeatKey)
                    If objTargetReg.blnRepeat Then
                        objTrx.SetSharedRptProps(mlngRptUnit, mintRptNumber, mdatRptEnd)
                    End If
                    For Each objSplit In mcolSplits
                        With objSplit
                            objTrx.AddSplit(.strMemo, .strCategoryKey, .strPONumber, .strInvoiceNum, .datInvoiceDate, .datDueDate, .strTerms, .strBudgetKey, .curAmount, .strImageFiles)
                        End With
                    Next objSplit
                    objTargetReg.NewLoadEnd(objTrx)
                Case Trx.TrxType.glngTRXTYP_BUDGET
                    If mstrBudgetKey = "" Then
                        RaiseError("CreateTrx", "No KB line for budget Trx")
                    End If
                    If mlngBudgetUnit <> Trx.RepeatUnit.glngRPTUNT_MISSING Then
                        If mintBudgetNumber = 0 Then
                            RaiseError("CreateTrx", "Missing BN line for budget Trx with BU line")
                        End If
                        mdatBudgetEnds = DateAdd(Microsoft.VisualBasic.DateInterval.Day, -1, gdatIncrementDate(mdatDate, mlngBudgetUnit, mintBudgetNumber))
                    End If
                    If mdatBudgetEnds = System.DateTime.FromOADate(0) Then
                        RaiseError("CreateTrx", "No budget ending date")
                    End If
                    objTrx = New Trx
                    objTrx.NewStartBudget(objTargetReg, mdatDate, mstrDescription, mstrMemo, mblnAwaitingReview, blnAutoGenerated, mintRepeatSeq, mstrRepeatKey, mcurAmount, mdatBudgetEnds, mstrBudgetKey)
                    If objTargetReg.blnRepeat Then
                        objTrx.SetSharedRptProps(mlngRptUnit, mintRptNumber, mdatRptEnd)
                        objTrx.SetBudgetRptProps(mlngBudgetUnit, mintBudgetNumber)
                    End If
                    objTargetReg.NewLoadEnd(objTrx)
                Case Trx.TrxType.glngTRXTYP_TRANSFER
                    If mstrTransferKey = "" Then
                        RaiseError("CreateTrx", "No KT line for transfer Trx")
                    End If
                    objTrx = New Trx
                    objTrx.NewStartTransfer(objTargetReg, mdatDate, mstrDescription, mstrMemo, blnFake, mblnAwaitingReview, blnAutoGenerated, mintRepeatSeq, mstrRepeatKey, mstrTransferKey, mcurAmount)
                    If objTargetReg.blnRepeat Then
                        objTrx.SetSharedRptProps(mlngRptUnit, mintRptNumber, mdatRptEnd)
                    End If
                    objTargetReg.NewLoadEnd(objTrx)
            End Select

            Exit Sub
        Catch ex As Exception
            gNestedException(ex)
        End Try
    End Sub
End Class