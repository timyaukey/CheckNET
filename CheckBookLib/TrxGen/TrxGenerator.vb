Option Strict On
Option Explicit On

Module TrxGenerator
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Routines related to creating Trx with ITrxGenerator objects.
    'Routines to create ITrxGenerator objects are in TrxGeneratorLoader.bas.

    '$Description Create all generated Trx for a Register.
    '$Param objAccount The Account to which objReg belongs.
    '$Param objReg The Register to generate Trx in.
    '$Param datRptEndMax Latest date to create Trx for.

    Public Sub gCreateGeneratedTrx(ByVal objAccount As Account, ByVal objReg As Register, ByVal datRptEndMax As Date)

        Dim colGenerators As ICollection(Of ITrxGenerator)
        Dim objGenerator As ITrxGenerator
        Dim colTrx As ICollection(Of TrxToCreate)
        Dim datTrxToCreate As TrxToCreate
        Dim strError As String
        Dim objReg2 As Register
        Dim colReg As ICollection(Of Register)

        colReg = New List(Of Register)
        For Each objReg2 In objAccount.colRegisters
            colReg.Add(objReg2)
        Next objReg2

        colGenerators = gcolCreateTrxGenerators(objAccount, objReg)
        For Each objGenerator In colGenerators
            If objGenerator.blnEnabled Then
                objAccount.RaiseLoadStatus("Generate " + objGenerator.strDescription)
                objAccount.objRepeatSummarizer.Define(objGenerator.strRepeatKey, objGenerator.strDescription, True)
                Try
                    colTrx = objGenerator.colCreateTrx(objReg, datRptEndMax)
                    For Each datTrxToCreate In colTrx

                        'Only create Trx for seq numbers we don't already have in reg.
                        If objReg.objRepeatTrx(datTrxToCreate.strRepeatKey, datTrxToCreate.intRepeatSeq) Is Nothing Then
                            'Create the Trx.
                            strError = gstrCreateOneTrx(datTrxToCreate, objReg, colReg)
                            If strError <> "" Then
                                MsgBox("Error using transaction generator [" & objGenerator.strDescription & "]:" & vbCrLf & strError, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Checkbook")
                                Exit For
                            End If
                        End If
                    Next
                Catch ex As Exception
                    Throw New Exception("Error generating trx for [" + objGenerator.strDescription + "]", ex)
                End Try
            End If
        Next objGenerator

    End Sub

    '$Description Create one Trx in the specified Register, from a TrxToCreate.
    '$Param c The TrxToCreate.
    '$Param objTargetReg The Register to create the Trx in.
    '$Param colRegisters All Registers in the same Account as objTargetReg.
    '$Returns A string with a diagnostic message if there was a problem,
    '   else an empty string.

    Public Function gstrCreateOneTrx(ByRef c As TrxToCreate, ByVal objTargetReg As Register, ByVal colRegisters As ICollection(Of Register)) As String

        Dim objOtherReg As Register
        Dim objCompareReg As Register
        Dim objTransferMgr As TransferManager
        Dim intSplitIndex As Integer

        gstrCreateOneTrx = ""

        Select Case c.lngType
            Case Trx.TrxType.glngTRXTYP_NORMAL
                Dim objNormalTrx As NormalTrx = New NormalTrx(objTargetReg)
                If c.intSplits = 0 Then
                    gstrCreateOneTrx = "No splits for normal Trx"
                    Exit Function
                End If
                objNormalTrx.NewStartNormal(True, c.strNumber, c.datDate, c.strDescription, c.strMemo, c.lngStatus, c.blnFake,
                                      c.curNormalMatchRange, c.blnAwaitingReview, c.blnAutoGenerated, c.intRepeatSeq,
                                      c.strImportKey, c.strRepeatKey)
                For intSplitIndex = 1 To c.intSplits
                    With c.adatSplits(intSplitIndex)
                        objNormalTrx.AddSplit(.strMemo, .strCategoryKey, .strPONumber, .strInvoiceNum, .datInvoiceDate, .datDueDate, .strTerms, .strBudgetKey, .curAmount, "")
                    End With
                Next
                objTargetReg.NewLoadEnd(objNormalTrx)
            Case Trx.TrxType.glngTRXTYP_BUDGET
                If c.strBudgetKey = "" Then
                    gstrCreateOneTrx = "No budget key for budget Trx"
                    Exit Function
                End If
                If c.lngBudgetUnit <> Trx.RepeatUnit.glngRPTUNT_MISSING Then
                    If c.intBudgetNumber = 0 Then
                        gstrCreateOneTrx = "Missing budget number"
                        Exit Function
                    End If
                    c.datBudgetEnds = DateAdd(Microsoft.VisualBasic.DateInterval.Day, -1, gdatIncrementDate(c.datDate, c.lngBudgetUnit, c.intBudgetNumber))
                End If
                If c.datBudgetEnds = System.DateTime.FromOADate(0) Then
                    gstrCreateOneTrx = "No budget ending date"
                    Exit Function
                End If
                Dim objBudgetTrx As BudgetTrx = New BudgetTrx(objTargetReg)
                objBudgetTrx.NewStartBudget(True, c.datDate, c.strDescription, c.strMemo, c.blnAwaitingReview, c.blnAutoGenerated, c.intRepeatSeq, c.strRepeatKey, c.curAmount, c.datBudgetEnds, c.strBudgetKey)
                objTargetReg.NewLoadEnd(objBudgetTrx)
            Case Trx.TrxType.glngTRXTYP_TRANSFER
                If c.strTransferKey = "" Then
                    gstrCreateOneTrx = "No register key for other register for transfer Trx"
                    Exit Function
                End If
                If c.blnAutoGenerated Then
                    'UPGRADE_NOTE: Object objOtherReg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
                    objOtherReg = Nothing
                    For Each objCompareReg In colRegisters
                        If objCompareReg.strRegisterKey = c.strTransferKey Then
                            If objCompareReg Is objTargetReg Then
                                gstrCreateOneTrx = "Cannot transfer to the same register in transfer trx"
                                Exit Function
                            End If
                            objOtherReg = objCompareReg
                            Exit For
                        End If
                    Next objCompareReg
                    If objOtherReg Is Nothing Then
                        gstrCreateOneTrx = "Could not find other register in transfer trx"
                        Exit Function
                    End If
                    'Don't have to set repeat properties in this case, because these two
                    'registers are always normal registers. Just for the heck of it though...
                    objTransferMgr = New TransferManager
                    objTransferMgr.LoadGeneratedTransfer(objTargetReg, objOtherReg, c.datDate, c.strDescription, c.strMemo, True, c.curAmount, c.strRepeatKey, c.blnAwaitingReview, c.blnAutoGenerated, c.intRepeatSeq)
                Else
                    Dim objXfrTrx As TransferTrx = New TransferTrx(objTargetReg)
                    objXfrTrx.NewStartTransfer(True, c.datDate, c.strDescription, c.strMemo, c.blnFake, c.blnAwaitingReview, c.blnAutoGenerated, c.intRepeatSeq, c.strRepeatKey, c.strTransferKey, c.curAmount)
                    objTargetReg.NewLoadEnd(objXfrTrx)
                End If
        End Select
    End Function

    '$Description Create and return an array of SequenceTrx for the sequence
    '   of dates specified by the parameters. The result is generally transformed
    '   into a TrxToCreate collection by passing it to gcolTrxToCreateFromSeqTrx(),
    '   usually after modifying it in some way.
    '$Param datNominalStartDate The theoretical start date of the sequence.
    '   Possible dates are generated by counting forward from this date by
    '   the appropriate interval.
    '$Param vntNominalEndDate If not null, no SequencedTrx with dates later than
    '   this will be generated. This date is a characteristic of the sequence,
    '   not the register to which Trx will ultimately be added.
    '$Param datRegisterEndDate The end date out to which generated Trx
    '   are being created in the target Register. The same date is generally
    '   used for all generated sequences in that Register.
    '$Param lngRptUnit The repeat unit used to generate dates.
    '$Param intRptNumber The number of repeat units between generated dates.
    '$Param curAmount The amount to set for each SequencedTrx.
    '$Returns The array of SequencedTrx, dimensioned from zero to the
    '   number of SequencedTrx created, with the zeroeth element being
    '   unused. This allows "for" loops from index 1 to UBound(array)
    '   even if no SequencedTrx created.

    Public Function gdatGenerateSeqTrxForDates(ByVal datNominalStartDate As Date, ByVal vntNominalEndDate As Object, ByVal datRegisterEndDate As Date, ByVal lngRptUnit As Trx.RepeatUnit, ByVal intRptNumber As Integer, ByVal curAmount As Decimal, ByVal intStartRepeatSeq As Integer) As SequencedTrx()

        Dim datTrxDate As Date
        Dim colResults As ICollection(Of SequencedTrx)
        Dim objSeqTrx As SequencedTrx
        Dim intIndex As Integer
        Dim intRepeatSeq As Integer

        datTrxDate = datNominalStartDate
        colResults = New List(Of SequencedTrx)
        intRepeatSeq = intStartRepeatSeq

        Do
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            If Not gblnXmlAttributeMissing(vntNominalEndDate) Then
                If datTrxDate > CDate(vntNominalEndDate) Then
                    Exit Do
                End If
            End If
            If datTrxDate > datRegisterEndDate Then
                Exit Do
            End If
            objSeqTrx = New SequencedTrx
            objSeqTrx.Init(datTrxDate, curAmount, intRepeatSeq)
            colResults.Add(objSeqTrx)
            intRepeatSeq = intRepeatSeq + 1
            datTrxDate = gdatIncrementDate(datTrxDate, lngRptUnit, intRptNumber)
        Loop

        Dim datResults(colResults.Count()) As SequencedTrx
        intIndex = 1
        For Each objSeqTrx In colResults
            datResults(intIndex) = objSeqTrx
            intIndex = intIndex + 1
        Next objSeqTrx
        Return DirectCast(datResults.Clone(), SequencedTrx())

    End Function

    '$Description Set the .curAmount values of an array of SequencedTrx
    '   by interpolating between .curAmount values in another array of
    '   SequencedTrx representing sample data. Uses linear interpolation
    '   between the .curAmount values of the pair of samples whose dates
    '   are closest before and the date of each SequencedTrx to create.
    '$Param datNewTrx The SequencedTrx whose .curAmount values will be set.
    '$Param datSamples The SequencedTrx whose elements are used as samples.

    Public Sub gInterpolateAmountsFromSamples(ByRef datNewTrx() As SequencedTrx, ByRef datSamples() As SequencedTrx)

        Dim intNewIndex As Integer
        Dim intSampleIndex As Integer
        Dim datNewDate As Date
        Dim dblFraction As Double
        Dim datSampleDate As Date
        Dim datPrevSampleDate As Date
        Dim blnAmountSet As Boolean

        'For each trx whose .curAmount value to set.
        For intNewIndex = 1 To UBound(datNewTrx)
            datNewDate = datNewTrx(intNewIndex).datDate
            'Find the sample pair to interpolate between.
            'Assumes the samples are in ascending date order.
            blnAmountSet = False
            datSampleDate = System.DateTime.FromOADate(0)
            For intSampleIndex = 1 To UBound(datSamples)
                datPrevSampleDate = datSampleDate
                datSampleDate = datSamples(intSampleIndex).datDate
                'Is this the closest sample after the date of the transaction to create?
                If datSampleDate >= datNewDate Then
                    'Is there a sample before it, to make a pair to interpolate between?
                    If intSampleIndex > 1 Then
                        'Compute the amount by interpolating between the sample amounts.
                        dblFraction = (datNewDate.ToOADate - datPrevSampleDate.ToOADate) / (datSampleDate.ToOADate - datPrevSampleDate.ToOADate)
                        datNewTrx(intNewIndex).curAmount = CDec(System.Math.Round(datSamples(intSampleIndex - 1).curAmount + (datSamples(intSampleIndex).curAmount - datSamples(intSampleIndex - 1).curAmount) * dblFraction, 2))
                        blnAmountSet = True
                        'We don't need a date before the trx date if the trx date *IS* the
                        'sample date, because in that case we don't have to interpolate
                        'at all. This allows us to create a trx on the date of the first
                        'sample.
                    ElseIf datSampleDate = datNewDate Then
                        datNewTrx(intNewIndex).curAmount = datSamples(intSampleIndex).curAmount
                        blnAmountSet = True
                    End If
                    Exit For
                End If
            Next
            If Not blnAmountSet Then
                'datNewDate is before the earliest sample date or after the latest,
                'so it is impossible to interpolate.
                datNewTrx(intNewIndex).blnSkip = True
            End If
        Next

    End Sub

    '$Description Convert a SequenceTrx array to a TrxToCreate collection.
    '$Param datSeqTrx The array to convert. A TrxToCreate will be created for
    '   each SequencedTrx which is not flagged to skip.
    '$Param datTrxTemplate The template TrxToCreate from which all data other
    '   than the date and amount will be taken.
    '$Returns The new collection of TrxToCreate.

    Public Function gcolTrxToCreateFromSeqTrx(ByRef datSeqTrx() As SequencedTrx, ByRef datTrxTemplate As TrxToCreate) As ICollection(Of TrxToCreate)

        Dim colResults As ICollection(Of TrxToCreate)
        Dim intIndex As Integer
        Dim datTrxTmp As TrxToCreate
        Dim strDueDayOfMonth As String = ""
        Dim intDueDayOfMonth As Integer
        Dim intDueYear As Integer
        Dim intDueMonth As Integer

        colResults = New List(Of TrxToCreate)
        For intIndex = 1 To UBound(datSeqTrx)
            If Not datSeqTrx(intIndex).blnSkip Then
                'LSet datTrxTmp = datTrxTemplate
                datTrxTmp = gdatCopyTrxToCreate(datTrxTemplate)
                datTrxTmp.datDate = datSeqTrx(intIndex).datDate
                If datTrxTemplate.lngType = Trx.TrxType.glngTRXTYP_NORMAL Then
                    datTrxTmp.adatSplits(1).curAmount = datSeqTrx(intIndex).curAmount
                    'If the memo says "due [on |by ][the ]nn???" then use "nn" as the day of the month in split due date.
                    If datTrxTemplate.strMemo <> Nothing Then
                        If datTrxTemplate.strMemo.StartsWith("due ", StringComparison.InvariantCultureIgnoreCase) Then
                            strDueDayOfMonth = datTrxTemplate.strMemo.Substring(4)
                            If strDueDayOfMonth.StartsWith("on ", StringComparison.InvariantCultureIgnoreCase) Then
                                strDueDayOfMonth = strDueDayOfMonth.Substring(3)
                            End If
                            If strDueDayOfMonth.StartsWith("by ", StringComparison.InvariantCultureIgnoreCase) Then
                                strDueDayOfMonth = strDueDayOfMonth.Substring(3)
                            End If
                            If strDueDayOfMonth.StartsWith("the ", StringComparison.InvariantCultureIgnoreCase) Then
                                strDueDayOfMonth = strDueDayOfMonth.Substring(4)
                            End If
                            intDueDayOfMonth = CInt(Val(strDueDayOfMonth))
                            If intDueDayOfMonth > 0 Then
                                intDueYear = datTrxTmp.datDate.Year
                                intDueMonth = datTrxTmp.datDate.Month
                                'If the due day of month is less than the day of month of the transaction that would put
                                'the due date in the past, so assume the due date is in the following month.
                                If intDueDayOfMonth < datTrxTmp.datDate.Day Then
                                    If intDueMonth = 12 Then
                                        intDueYear = intDueYear + 1
                                        intDueMonth = 1
                                    Else
                                        intDueMonth = intDueMonth + 1
                                    End If
                                End If
                                If intDueDayOfMonth > DateTime.DaysInMonth(intDueYear, intDueMonth) Then
                                    intDueDayOfMonth = DateTime.DaysInMonth(intDueYear, intDueMonth)
                                End If
                                datTrxTmp.adatSplits(1).datDueDate = New DateTime(intDueYear, intDueMonth, intDueDayOfMonth)
                            End If
                        End If
                    End If
                Else
                    datTrxTmp.curAmount = datSeqTrx(intIndex).curAmount
                End If
                datTrxTmp.intRepeatSeq = datSeqTrx(intIndex).intRepeatSeq
                colResults.Add(datTrxTmp)
            End If
        Next
        gcolTrxToCreateFromSeqTrx = colResults

    End Function
End Module