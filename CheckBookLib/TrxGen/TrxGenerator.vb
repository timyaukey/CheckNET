Option Strict Off
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
		
		Dim colGenerators As Collection
        Dim objGenerator As ITrxGenerator
		Dim colTrx As Collection
		Dim vntTrxToCreate As Object
		'UPGRADE_WARNING: Arrays in structure datTrxToCreate may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim datTrxToCreate As TrxToCreate
		Dim intSplitIndex As Short
		Dim strError As String
		Dim objLoaded As LoadedRegister
		Dim colReg As Collection
		
		colReg = New Collection
		For	Each objLoaded In objAccount.colLoadedRegisters
			colReg.Add(objLoaded.objReg)
		Next objLoaded
		
		colGenerators = gcolCreateTrxGenerators(objAccount, objReg)
		For	Each objGenerator In colGenerators
            If objGenerator.blnEnabled Then
                objAccount.RaiseLoadStatus("Generate " + objGenerator.strDescription)
                objAccount.objRepeatSummarizer.Define(objGenerator.strRepeatKey, objGenerator.strDescription, True)
                Try
                    colTrx = objGenerator.colCreateTrx(objReg, datRptEndMax)
                    For Each vntTrxToCreate In colTrx
                        datTrxToCreate = vntTrxToCreate

                        'Only create Trx for seq numbers we don't already have in reg.
                        If objReg.objRepeatTrx(datTrxToCreate.strRepeatKey, datTrxToCreate.intRepeatSeq) Is Nothing Then
                            'Create the Trx.
                            strError = gstrCreateOneTrx(datTrxToCreate, objReg, colReg)
                            If strError <> "" Then
                                MsgBox("Error using transaction generator [" & objGenerator.strDescription & "]:" & vbCrLf & strError, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Checkbook")
                                Exit For
                            End If
                        End If
                    Next vntTrxToCreate
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
	
    Public Function gstrCreateOneTrx(ByRef c As TrxToCreate, ByVal objTargetReg As Register, ByVal colRegisters As Collection) As String

        Dim objOtherReg As Register
        Dim objCompareReg As Register
        Dim objTrx As Trx
        Dim objTransferMgr As TransferManager
        Dim vntSplit As Object
        Dim intSplitIndex As Short

        gstrCreateOneTrx = ""

        Select Case c.lngType
            Case Trx.TrxType.glngTRXTYP_NORMAL
                objTrx = New Trx
                If c.intSplits = 0 Then
                    gstrCreateOneTrx = "No splits for normal Trx"
                    Exit Function
                End If
                objTrx.NewStartNormal(objTargetReg, c.strNumber, c.datDate, c.strDescription, c.strMemo, c.lngStatus, c.blnFake, c.curNormalMatchRange, c.blnAwaitingReview, c.blnAutoGenerated, c.intRepeatSeq, c.strImportKey, c.strRepeatKey)
                For intSplitIndex = 1 To c.intSplits
                    With c.adatSplits(intSplitIndex)
                        objTrx.AddSplit(.strMemo, .strCategoryKey, .strPONumber, .strInvoiceNum, .datInvoiceDate, .datDueDate, .strTerms, .strBudgetKey, .curAmount, "")
                    End With
                Next
                objTargetReg.NewLoadEnd(objTrx)
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
                objTrx = New Trx
                objTrx.NewStartBudget(objTargetReg, c.datDate, c.strDescription, c.strMemo, c.blnAwaitingReview, c.blnAutoGenerated, c.intRepeatSeq, c.strRepeatKey, c.curAmount, c.datBudgetEnds, c.strBudgetKey)
                objTargetReg.NewLoadEnd(objTrx)
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
                    objTrx = New Trx
                    objTrx.NewStartTransfer(objTargetReg, c.datDate, c.strDescription, c.strMemo, c.blnFake, c.blnAwaitingReview, c.blnAutoGenerated, c.intRepeatSeq, c.strRepeatKey, c.strTransferKey, c.curAmount)
                    objTargetReg.NewLoadEnd(objTrx)
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
	
	Public Function gdatGenerateSeqTrxForDates(ByVal datNominalStartDate As Date, ByVal vntNominalEndDate As Object, ByVal datRegisterEndDate As Date, ByVal lngRptUnit As Trx.RepeatUnit, ByVal intRptNumber As Short, ByVal curAmount As Decimal, ByVal intStartRepeatSeq As Short) As SequencedTrx()
		
		Dim datTrxDate As Date
		Dim colResults As Collection
		Dim objSeqTrx As SequencedTrx
		Dim intIndex As Short
		Dim intRepeatSeq As Short
		
		datTrxDate = datNominalStartDate
		colResults = New Collection
		intRepeatSeq = intStartRepeatSeq
		
		Do 
			'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            If Not gblnXmlAttributeMissing(vntNominalEndDate) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vntNominalEndDate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If datTrxDate > vntNominalEndDate Then
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
		For	Each objSeqTrx In colResults
			datResults(intIndex) = objSeqTrx
			intIndex = intIndex + 1
		Next objSeqTrx
		gdatGenerateSeqTrxForDates = VB6.CopyArray(datResults)
		
	End Function
	
	'$Description Set the .curAmount values of an array of SequencedTrx
	'   by interpolating between .curAmount values in another array of
	'   SequencedTrx representing sample data. Uses linear interpolation
	'   between the .curAmount values of the pair of samples whose dates
	'   are closest before and the date of each SequencedTrx to create.
	'$Param datNewTrx The SequencedTrx whose .curAmount values will be set.
	'$Param datSamples The SequencedTrx whose elements are used as samples.
	
	Public Sub gInterpolateAmountsFromSamples(ByRef datNewTrx() As SequencedTrx, ByRef datSamples() As SequencedTrx)
		
		Dim intNewIndex As Short
		Dim intSampleIndex As Short
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
			datSampleDate = System.Date.FromOADate(0)
			For intSampleIndex = 1 To UBound(datSamples)
				datPrevSampleDate = datSampleDate
				datSampleDate = datSamples(intSampleIndex).datDate
				'Is this the closest sample after the date of the transaction to create?
				If datSampleDate >= datNewDate Then
					'Is there a sample before it, to make a pair to interpolate between?
					If intSampleIndex > 1 Then
						'Compute the amount by interpolating between the sample amounts.
						dblFraction = (datNewDate.ToOADate - datPrevSampleDate.ToOADate) / (datSampleDate.ToOADate - datPrevSampleDate.ToOADate)
						datNewTrx(intNewIndex).curAmount = System.Math.Round(datSamples(intSampleIndex - 1).curAmount + (datSamples(intSampleIndex).curAmount - datSamples(intSampleIndex - 1).curAmount) * dblFraction, 2)
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
	
    Public Function gcolTrxToCreateFromSeqTrx(ByRef datSeqTrx() As SequencedTrx, ByRef datTrxTemplate As TrxToCreate) As Collection

        Dim colResults As Collection
        Dim intIndex As Short
        'UPGRADE_WARNING: Arrays in structure datTrxTmp may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim datTrxTmp As TrxToCreate

        colResults = New Collection
        For intIndex = 1 To UBound(datSeqTrx)
            If Not datSeqTrx(intIndex).blnSkip Then
                'LSet datTrxTmp = datTrxTemplate
                'UPGRADE_WARNING: Couldn't resolve default property of object datTrxTmp. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                datTrxTmp = gdatCopyTrxToCreate(datTrxTemplate)
                datTrxTmp.datDate = datSeqTrx(intIndex).datDate
                If datTrxTemplate.lngType = Trx.TrxType.glngTRXTYP_NORMAL Then
                    datTrxTmp.adatSplits(1).curAmount = datSeqTrx(intIndex).curAmount
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