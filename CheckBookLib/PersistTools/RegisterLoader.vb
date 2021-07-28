Option Strict On
Option Explicit On

Imports System.IO

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
    Private mlngStatus As BaseTrx.TrxStatus
    Private mobjTrxType As Type
    Private mstrNumber As String
    Private mdatDate As Date
    Private mstrMemo As String
    Private mcurAmount As Decimal
    Private mcurNormalMatchRange As Decimal
    Private mblnAwaitingReview As Boolean
    Private mdatBudgetStarts As Date
    Private mstrImportKey As String
    Private mstrTransferKey As String
    Private mstrBudgetKey As String
    Private mintRepeatSeq As Integer
    Private mstrRepeatKey As String
    Private mblnFake As Boolean

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
    Private mcolSplits As ICollection(Of TrxSplit)

    '$Description Load transactions into an existing register from an open file handle.
    '   Does not clear the existing register contents before starting, so can be used
    '   to load a single Register with data from multiple files.
    '$Param objReg The Register object to load.
    '$Param blnFake The blnFake value to construct new transactions with.
    '$Param lngLinesRead The number of lines read from the file. On entry is the number
    '   of lines previously read, on exit is incremented to include lines read by
    '   this method.

    Public Sub LoadFile(ByVal objLoadFile As TextReader, ByVal objReg As Register, ByVal objRepeatSummarizer As RepeatSummarizer,
            ByVal blnFake As Boolean, ByRef lngLinesRead As Integer)

        Try

            LoadInit(objReg, objRepeatSummarizer, blnFake, lngLinesRead)

            Do
                mstrLine = objLoadFile.ReadLine()
                If LoadLine(lngLinesRead) Then
                    Exit Sub
                End If
            Loop

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub

    Private Sub LoadInit(ByVal objReg As Register, ByVal objRepeatSummarizer As RepeatSummarizer, ByVal blnFake As Boolean, ByRef lngLinesRead As Integer)

        mobjReg = objReg
        mobjRepeatSummarizer = objRepeatSummarizer
        mblnFake = blnFake
        mlngLinesRead = lngLinesRead
        ClearTrxData()
    End Sub

    Private Function LoadLine(ByRef lngLinesRead As Integer) As Boolean
        Dim objSplit As TrxSplit

        LoadLine = False
        mlngLinesRead = mlngLinesRead + 1
        lngLinesRead = mlngLinesRead
        'TN, TB or TT line is required, and must precede any line which needs
        'to know the type of transaction.
        Select Case Left(mstrLine, 2)
            Case ".R" 'End of register.
                LoadLine = True
                Exit Function
            Case "TN" 'Normal transaction header.
                mobjTrxType = GetType(BankTrx)
                Select Case Mid(mstrLine, 3, 1)
                    Case "U"
                        mlngStatus = BaseTrx.TrxStatus.Unreconciled
                    Case "R"
                        mlngStatus = BaseTrx.TrxStatus.Reconciled
                    Case "S"
                        mlngStatus = BaseTrx.TrxStatus.Selected
                    Case Else
                        RaiseErrorInLoad("Normal Trx status may only be U or R")
                End Select
                mstrDescription = Mid(mstrLine, 4)
            Case "TB" 'Budget transaction header.
                mobjTrxType = GetType(BudgetTrx)
                If Mid(mstrLine, 3, 1) <> "N" Then
                    RaiseErrorInLoad("Budget Trx status may only be N")
                End If
                mstrDescription = Mid(mstrLine, 4)
            Case "TT" 'Transfer transaction header.
                mobjTrxType = GetType(TransferTrx)
                If Mid(mstrLine, 3, 1) <> "N" Then
                    RaiseErrorInLoad("Transfer Trx status may only be N")
                End If
                mstrDescription = Mid(mstrLine, 4)
            Case "N#"
                mstrNumber = Mid(mstrLine, 3)
            Case "DT"
                mdatDate = ConvertInputDate(Mid(mstrLine, 3), "transaction")
            Case "ME"
                mstrMemo = Mid(mstrLine, 3)
            Case "A$"
                If mobjTrxType <> GetType(BudgetTrx) And mobjTrxType <> GetType(TransferTrx) Then
                    RaiseErrorInLoad("A$ line only allowed for budget and transfer Trx")
                End If
                If Not IsNumeric(Mid(mstrLine, 3)) Then
                    RaiseErrorInLoad("Invalid A$ amount")
                End If
                mcurAmount = CDec(Mid(mstrLine, 3))
            Case "MR"
                If mobjTrxType <> GetType(BankTrx) Then
                    RaiseErrorInLoad("MR line only allowed for normal Trx")
                End If
                If Not IsNumeric(Mid(mstrLine, 3)) Then
                    RaiseErrorInLoad("Invalid MR amount")
                End If
                mcurNormalMatchRange = CDec(Mid(mstrLine, 3))
            Case "AR"
                mblnAwaitingReview = True
            Case "BE"
                If mobjTrxType <> GetType(BudgetTrx) Then
                    RaiseErrorInLoad("BE line only allowed for budget Trx")
                End If
                mdatBudgetStarts = ConvertInputDate(Mid(mstrLine, 3), "budget ending")
            Case "KI"
                If mobjTrxType <> GetType(BankTrx) Then
                    RaiseErrorInLoad("KI line only allowed for normal Trx")
                End If
                mstrImportKey = Mid(mstrLine, 3)
            Case "RS"
                mintRepeatSeq = CInt(Val(Mid(mstrLine, 3)))
            Case "KR"
                mstrRepeatKey = Mid(mstrLine, 3)
            Case "KT"
                If mobjTrxType <> GetType(TransferTrx) Then
                    RaiseErrorInLoad("KT line only allowed for transfer Trx")
                End If
                mstrTransferKey = Mid(mstrLine, 3)
            Case "KB"
                If mobjTrxType <> GetType(BudgetTrx) Then
                    RaiseErrorInLoad("KB line only allowed for budget Trx")
                End If
                mstrBudgetKey = Mid(mstrLine, 3)
            Case ".T"
                CreateTrx()
                ClearTrxData()
            'Case "GU"
            '    'This line will be ignored in contexts where it is not used.
            '    mlngRptUnit = lngConvertRepeatUnit(Mid(mstrLine, 3), "GU line")
            'Case "GN"
            '    'This line will be ignored in contexts where it is not used.
            '    mintRptNumber = intConvertRepeatCount(Mid(mstrLine, 3), "GN line")
            'Case "GE"
            '    'This line will be ignored in contexts where it is not used.
            '    mdatRptEnd = datConvertInput(Mid(mstrLine, 3), "repeat sequence end")
            Case "SM"
                If mobjTrxType <> GetType(BankTrx) Then
                    RaiseErrorInLoad("SM only allowed for normal Trx")
                End If
                mstrSMemo = Mid(mstrLine, 3)
            Case "SC"
                If mobjTrxType <> GetType(BankTrx) Then
                    RaiseErrorInLoad("SC only allowed for normal Trx")
                End If
                mstrSCategoryKey = Mid(mstrLine, 3)
            Case "SP"
                If mobjTrxType <> GetType(BankTrx) Then
                    RaiseErrorInLoad("SP only allowed for normal Trx")
                End If
                mstrSPONumber = Mid(mstrLine, 3)
            Case "SN"
                If mobjTrxType <> GetType(BankTrx) Then
                    RaiseErrorInLoad("SN only allowed for normal Trx")
                End If
                mstrSInvoiceNum = Mid(mstrLine, 3)
            Case "SI"
                If mobjTrxType <> GetType(BankTrx) Then
                    RaiseErrorInLoad("SI only allowed for normal Trx")
                End If
                mdatSInvoiceDate = ConvertInputDate(Mid(mstrLine, 3), "invoice")
            Case "SD"
                If mobjTrxType <> GetType(BankTrx) Then
                    RaiseErrorInLoad("SD only allowed for normal Trx")
                End If
                mdatSDueDate = ConvertInputDate(Mid(mstrLine, 3), "due")
            Case "ST"
                If mobjTrxType <> GetType(BankTrx) Then
                    RaiseErrorInLoad("ST only allowed for normal Trx")
                End If
                mstrSTerms = Mid(mstrLine, 3)
            Case "SB"
                If mobjTrxType <> GetType(BankTrx) Then
                    RaiseErrorInLoad("SB only allowed for normal Trx")
                End If
                mstrSBudgetKey = Mid(mstrLine, 3)
            Case "SA"
                If mobjTrxType <> GetType(BankTrx) Then
                    RaiseErrorInLoad("SA only allowed for normal Trx")
                End If
                If Not IsNumeric(Mid(mstrLine, 3)) Then
                    RaiseErrorInLoad("Invalid SA amount")
                End If
                mcurSAmount = CDec(Mid(mstrLine, 3))
            Case "SF"
                'Ignore this. Used to be file list, and a few old trx still have this data field.
            Case "SZ"
                If mobjTrxType <> GetType(BankTrx) Then
                    RaiseErrorInLoad("SZ only allowed for normal Trx")
                End If
                objSplit = New TrxSplit
                objSplit.Init(mstrSMemo, mstrSCategoryKey, mstrSPONumber, mstrSInvoiceNum, mdatSInvoiceDate, mdatSDueDate, mstrSTerms, mstrSBudgetKey, mcurSAmount)
                mcolSplits.Add(objSplit)
                ClearSplitData()
            Case Else
                RaiseErrorInLoad("Unrecognized line type")
        End Select
    End Function

    Private Sub ClearTrxData()
        mstrDescription = ""
        mlngStatus = BaseTrx.TrxStatus.Missing
        mobjTrxType = Nothing
        mstrNumber = ""
        mdatDate = Utilities.datEmpty
        mstrMemo = ""
        mcurAmount = 0
        mcurNormalMatchRange = 0
        mblnAwaitingReview = False
        mdatBudgetStarts = Utilities.datEmpty
        mstrImportKey = ""
        mstrTransferKey = ""
        mstrBudgetKey = ""
        mintRepeatSeq = 0
        mstrRepeatKey = ""

        ClearSplitData()
        mcolSplits = New List(Of TrxSplit)
    End Sub

    Private Sub ClearSplitData()
        mstrSMemo = ""
        mstrSCategoryKey = ""
        mstrSPONumber = ""
        mstrSInvoiceNum = ""
        mdatSInvoiceDate = Utilities.datEmpty
        mdatSDueDate = Utilities.datEmpty
        mstrSTerms = ""
        mstrSBudgetKey = ""
        mcurSAmount = 0
    End Sub

    Private Sub RaiseError(ByVal strRoutine As String, ByVal strMsg As String)
        RaiseErrorMsg("Error in RegisterLoader." & strRoutine & vbCrLf & "Line " & mlngLinesRead & " in register file: " & mstrLine & vbCrLf & strMsg)
    End Sub

    Private Sub RaiseErrorInLoad(ByVal strMsg As String)
        RaiseError("blnLoadLine", strMsg)
    End Sub

    Private Function ConvertInputDate(ByVal strInput As String, ByVal strContext As String) As Date

        Dim datOutput As Date
        If Utilities.blnTryParseUniversalDate(strInput, datOutput) Then
            Return datOutput
        End If
        'If Utilities.blnIsValidDate(strInput) Then
        '    datConvertInput = CDate(strInput)
        '    Exit Function
        'End If
        RaiseError("datConvertInput", "Invalid " & strContext & " date")

    End Function

    Private Function ConvertRepeatUnit(ByVal strInput As String, ByVal strContext As String) As BaseTrx.RepeatUnit

        Dim lngResult As BaseTrx.RepeatUnit
        lngResult = ConvertTrxGenRepeatUnit(strInput)
        If lngResult = BaseTrx.RepeatUnit.Missing Then
            RaiseErrorInLoad("Unrecognized unit name in " & strContext)
        End If
        ConvertRepeatUnit = lngResult
    End Function

    Private Function ConvertRepeatCount(ByVal strInput As String, ByVal strContext As String) As Short

        Dim intResult As Short

        intResult = ConvertTrxGenRepeatCount(strInput)
        If intResult = 0 Then
            RaiseErrorInLoad(strContext & " has non-numeric or non-positive value")
        End If
        ConvertRepeatCount = intResult
    End Function

    Private Sub CreateTrx()
        If mobjTrxType Is Nothing Then
            RaiseError("CreateTrx", "No TN, TB or TT line before TZ")
        End If
        If mdatDate = Utilities.datEmpty Then
            RaiseError("CreateTrx", "No DT line for Trx")
        End If
        CreateOneTrx(mobjReg, mblnFake)
    End Sub

    Private Sub CreateOneTrx(ByVal objTargetReg As Register, ByVal blnFake As Boolean)

        Dim objSplit As TrxSplit
        Dim blnAutoGenerated As Boolean

        Try

            blnAutoGenerated = False

            'Hack to assign initial repeat keys.
            Dim intRepeatKey As Integer
            intRepeatKey = CInt(Val(mstrRepeatKey))

            If mintRepeatSeq > 0 Then
                mobjRepeatSummarizer.Define(mstrRepeatKey, mstrDescription, False)
            End If

            Select Case mobjTrxType
                Case GetType(BankTrx)
                    Dim objNormalTrx As BankTrx = New BankTrx(objTargetReg)
                    If mcolSplits.Count() = 0 Then
                        RaiseError("CreateTrx", "No splits for normal Trx")
                    End If
                    objNormalTrx.NewStartNormal(True, mstrNumber, mdatDate, mstrDescription, mstrMemo, mlngStatus, blnFake,
                                          mcurNormalMatchRange, mblnAwaitingReview, blnAutoGenerated, mintRepeatSeq,
                                          mstrImportKey, mstrRepeatKey)
                    For Each objSplit In mcolSplits
                        With objSplit
                            objNormalTrx.AddSplit(.Memo, .CategoryKey, .PONumber, .InvoiceNum, .InvoiceDate, .DueDate, .Terms, .BudgetKey, .Amount)
                        End With
                    Next objSplit
                    objTargetReg.NewLoadEnd(objNormalTrx)
                Case GetType(BudgetTrx)
                    If mstrBudgetKey = "" Then
                        RaiseError("CreateTrx", "No KB line for budget Trx")
                    End If
                    If mdatBudgetStarts = Utilities.datEmpty Then
                        RaiseError("CreateTrx", "No budget starting date")
                    End If
                    Dim objBudgetTrx As BudgetTrx = New BudgetTrx(objTargetReg)
                    objBudgetTrx.NewStartBudget(True, mdatDate, mstrDescription, mstrMemo, mblnAwaitingReview, blnAutoGenerated, mintRepeatSeq, mstrRepeatKey, mcurAmount, mdatBudgetStarts, mstrBudgetKey)
                    objTargetReg.NewLoadEnd(objBudgetTrx)
                Case GetType(TransferTrx)
                    If mstrTransferKey = "" Then
                        RaiseError("CreateTrx", "No KT line for transfer Trx")
                    End If
                    Dim objXfrTrx As TransferTrx = New TransferTrx(objTargetReg)
                    objXfrTrx.NewStartTransfer(True, mdatDate, mstrDescription, mstrMemo, blnFake, mblnAwaitingReview, blnAutoGenerated, mintRepeatSeq, mstrRepeatKey, mstrTransferKey, mcurAmount)
                    objTargetReg.NewLoadEnd(objXfrTrx)
            End Select

            Exit Sub
        Catch ex As Exception
            NestedException(ex)
        End Try
    End Sub
End Class