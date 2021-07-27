Option Strict On
Option Explicit On

Public Module TrxGeneratorLoader
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Routines related to creating ITrxGenerator objects from XML files.

    Private mdatNullTrxToCreate As TrxToCreate

    '$Description Create the ITrxGenerator for the specified XML document, and call
    '   Load() for it. Displays a diagnostic error if bad or missing data in XML document.
    '$Returns The ITrxGenerator created if successful, or Nothing.

    Private Function CreateTrxGenerator(ByVal domDoc As CBXmlDocument, ByVal objAccount As Account) As ITrxGenerator

        Dim vntClassName As Object
        Dim strClassName As String
        Dim objGenerator As ITrxGenerator
        Dim strError As String

        CreateTrxGenerator = Nothing
        If domDoc.DocumentElement.Name <> "generator" Then
            ShowTrxGeneratorLoadError(domDoc, "Document element is not <generator>")
            Exit Function
        End If
        vntClassName = domDoc.DocumentElement.GetAttribute("class")
        If gblnXmlAttributeMissing(vntClassName) Then
            ShowTrxGeneratorLoadError(domDoc, "Missing class attribute")
            Exit Function
        End If
        strClassName = CStr(vntClassName)
        If strClassName = "wccheckbook.period" Then
            objGenerator = New TrxGenPeriod
        ElseIf strClassName = "wccheckbook.repeat" Then
            objGenerator = New TrxGenRepeat
        ElseIf strClassName = "wccheckbook.list" Then
            objGenerator = New TrxGenList
        ElseIf strClassName = "wccheckbook.interpolate" Then
            objGenerator = New TrxGenInterpolate
        Else
            ShowTrxGeneratorLoadError(domDoc, "Unrecognized class " & strClassName)
            Exit Function
        End If
        strError = objGenerator.Load(domDoc, objAccount)
        If strError = "" Then
            CreateTrxGenerator = objGenerator
        Else
            ShowTrxGeneratorLoadError(domDoc, strError)
        End If

    End Function

    '$Description Create all ITrxGenerator objects for a register. If a data problem
    '   is encountered will display a message and not create that object.
    '$Returns The Collection of ITrxGenerator objects created.

    Public Function CreateTrxGenerators(ByVal objAccount As Account, ByVal objReg As Register) As ICollection(Of ITrxGenerator)

        Dim strPath As String
        Dim strFullXMLFile As String
        Dim domDoc As CBXmlDocument
        Dim objParseError As CBXmlParseError
        Dim objGenerator As ITrxGenerator
        Dim colResults As ICollection(Of ITrxGenerator)
        Dim strRepeatKeysUsed As String = ""
        Dim strThisRepeatKey As String

        colResults = New List(Of ITrxGenerator)
        strPath = TrxGeneratorPath(objAccount, objReg)
        If IO.Directory.Exists(strPath) Then
            For Each strFullXMLFile In IO.Directory.EnumerateFiles(strPath, "*.gen")
                Try
                    domDoc = New CBXmlDocument
                    domDoc.Load(strFullXMLFile)
                    objParseError = domDoc.ParseError
                    If Not objParseError Is Nothing Then
                        ShowTrxGeneratorLoadError(strFullXMLFile, gstrXMLParseErrorText(objParseError))
                    Else
                        domDoc.SetProperty("SelectionLanguage", "XPath")
                        objGenerator = CreateTrxGenerator(domDoc, objAccount)
                        If Not objGenerator Is Nothing Then
                            strThisRepeatKey = "|" & objGenerator.RepeatKey & "|"
                            If InStr(strRepeatKeysUsed, strThisRepeatKey) = 0 Then
                                colResults.Add(objGenerator)
                                strRepeatKeysUsed = strRepeatKeysUsed & strThisRepeatKey
                            Else
                                ShowTrxGeneratorLoadError(domDoc, "Repeat key used by another generator")
                            End If
                        End If
                    End If
                Catch ex As Exception
                    Throw New Exception("Error loading trx generator [" + strFullXMLFile + "]", ex)
                End Try
            Next
        End If
        Return colResults

    End Function

    '$Description Path to folder with *.gen (transaction generators) for specified 
    '   register in specified account.

    Public Function TrxGeneratorPath(ByVal objAccount As Account, ByVal objReg As Register) As String
        TrxGeneratorPath = objAccount.Company.AccountsFolderPath() & "\" & objAccount.FileNameRoot & ".gen\" & objReg.RegisterKey
    End Function

    '$Description Report an error detected while loading a transaction generator file.

    Public Sub ShowTrxGeneratorLoadError(ByVal domDoc As CBXmlDocument, ByVal strError As String)
        Dim strDescription As String
        Dim vntDescription As Object
        vntDescription = domDoc.DocumentElement.GetAttribute("description")
        If gblnXmlAttributeMissing(vntDescription) Then
            vntDescription = ""
        End If
        If Len(vntDescription) = 0 Then
            strDescription = domDoc.FullPath
        Else
            strDescription = CStr(vntDescription)
        End If
        ShowTrxGeneratorLoadError(strDescription, strError)
    End Sub

    Public Sub ShowTrxGeneratorLoadError(ByVal strDescription As String, ByVal strError As String)
        MsgBox("Error loading transaction generator [" & strDescription & "]:" & vbCrLf & strError, MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Checkbook")
    End Sub

    Public Function GetNextDateInTrxGenSequence(ByVal datStart As Date, ByVal lngUnit As BaseTrx.RepeatUnit, ByVal intNumber As Integer) As Date

        Select Case lngUnit
            Case BaseTrx.RepeatUnit.Day
                GetNextDateInTrxGenSequence = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intNumber, datStart)
            Case BaseTrx.RepeatUnit.Week
                GetNextDateInTrxGenSequence = DateAdd(Microsoft.VisualBasic.DateInterval.WeekOfYear, intNumber, datStart)
            Case BaseTrx.RepeatUnit.Month
                GetNextDateInTrxGenSequence = DateAdd(Microsoft.VisualBasic.DateInterval.Month, intNumber, datStart)
        End Select
    End Function

    Public Function ConvertTrxGenRepeatUnit(ByVal strInput As String) As BaseTrx.RepeatUnit
        Select Case strInput
            Case "DAY"
                ConvertTrxGenRepeatUnit = BaseTrx.RepeatUnit.Day
            Case "WEEK"
                ConvertTrxGenRepeatUnit = BaseTrx.RepeatUnit.Week
            Case "MONTH"
                ConvertTrxGenRepeatUnit = BaseTrx.RepeatUnit.Month
            Case Else
                ConvertTrxGenRepeatUnit = BaseTrx.RepeatUnit.Missing
        End Select
    End Function

    Public Function ConvertTrxGenRepeatCount(ByVal strInput As String) As Short
        Dim intResult As Short

        If IsNumeric(strInput) Then
            intResult = CShort(strInput)
            If intResult < 1 Then
                ConvertTrxGenRepeatCount = 0
            Else
                ConvertTrxGenRepeatCount = intResult
            End If
        Else
            ConvertTrxGenRepeatCount = 0
        End If
    End Function

    '$Description Extract from an XML document the core information which must be
    '   defined for all trx generators. Must be called from ITrxGenerator.strLoad().
    '$Returns A non-empty error message if bad or missing data was encountered,
    '   else an empty string.

    Public Function LoadTrxGeneratorCore(ByVal domDoc As CBXmlDocument, ByRef blnEnabled As Boolean, ByRef strRepeatKey As String, ByRef intStartRepeatSeq As Integer, ByRef strDescription As String, ByVal objAccount As Account) As String

        Dim vntAttrib As Object

        LoadTrxGeneratorCore = ""
        vntAttrib = domDoc.DocumentElement.GetAttribute("enabled")
        If gblnXmlAttributeMissing(vntAttrib) Then
            LoadTrxGeneratorCore = "Missing [enabled] attribute"
            Exit Function
        End If
        If CStr(vntAttrib) <> "true" And CStr(vntAttrib) <> "false" Then
            LoadTrxGeneratorCore = "[enabled] attribute must be ""true"" or ""false"""
            Exit Function
        End If
        blnEnabled = CBool(vntAttrib)

        vntAttrib = domDoc.DocumentElement.GetAttribute("repeatkey")
        If gblnXmlAttributeMissing(vntAttrib) Then
            LoadTrxGeneratorCore = "Missing [repeatkey] attribute"
            Exit Function
        End If
        strRepeatKey = CStr(vntAttrib)
        'If Account.Repeats.intLookupKey(strRepeatKey) = 0 Then
        '    gstrLoadTrxGeneratorCore = "Invalid [repeatkey] attribute"
        '    Exit Function
        'End If

        vntAttrib = domDoc.DocumentElement.GetAttribute("description")
        If gblnXmlAttributeMissing(vntAttrib) Then
            LoadTrxGeneratorCore = "Missing [description] attribute"
            Exit Function
        End If
        If Len(vntAttrib) = 0 Then
            LoadTrxGeneratorCore = "Empty [description] attribute"
            Exit Function
        End If
        strDescription = CStr(vntAttrib)

        vntAttrib = domDoc.DocumentElement.GetAttribute("startseq")
        If gblnXmlAttributeMissing(vntAttrib) Then
            LoadTrxGeneratorCore = "Missing [startseq] attribute"
            Exit Function
        End If
        If Not IsNumeric(vntAttrib) Then
            LoadTrxGeneratorCore = "Invalid [startseq] attribute"
            Exit Function
        End If
        intStartRepeatSeq = CShort(vntAttrib)
    End Function

    '$Description Initialize a DateSequenceParams from attributes of the
    '   XML element matching the arguments.
    '$Param elmParent Parent XML element to the one with the data we want.
    '$Param strChildName Name of the desired child element of elmParent.
    '$Param datParams Initialize this DateSequenceParams.
    '$Param elmChild The XML element found.
    '$Returns A non-empty error message if bad or missing data was encountered,
    '   else an empty string.

    Public Function GetTrxGenDateSequenceParams(ByVal elmParent As CBXmlElement, ByVal strChildName As String, ByRef elmChild As CBXmlElement, ByRef datParams As DateSequenceParams) As String

        Dim vntAttrib As Object

        GetTrxGenDateSequenceParams = ""

        elmChild = DirectCast(elmParent.SelectSingleNode(strChildName), CBXmlElement)
        If elmChild Is Nothing Then
            GetTrxGenDateSequenceParams = "Could not find <" & strChildName & "> element"
            Exit Function
        End If

        vntAttrib = elmChild.GetAttribute("unit")
        If gblnXmlAttributeMissing(vntAttrib) Then
            GetTrxGenDateSequenceParams = "Missing [unit] attribute"
            Exit Function
        End If
        datParams.RptUnit = ConvertTrxGenRepeatUnit(UCase(CStr(vntAttrib)))
        If datParams.RptUnit = BaseTrx.RepeatUnit.Missing Then
            GetTrxGenDateSequenceParams = "Invalid [unit] attribute"
            Exit Function
        End If

        vntAttrib = elmChild.GetAttribute("interval")
        If gblnXmlAttributeMissing(vntAttrib) Then
            GetTrxGenDateSequenceParams = "Missing [interval] attribute"
            Exit Function
        End If
        datParams.RptNumber = ConvertTrxGenRepeatCount(CStr(vntAttrib))
        If datParams.RptNumber = 0 Then
            GetTrxGenDateSequenceParams = "Invalid [interval] attribute"
            Exit Function
        End If

        vntAttrib = elmChild.GetAttribute("startdate")
        If gblnXmlAttributeMissing(vntAttrib) Then
            GetTrxGenDateSequenceParams = "Missing [startdate] attribute"
            Exit Function
        End If
        If Utilities.blnIsValidDate(CStr(vntAttrib)) Then
            datParams.NominalStartDate = CDate(vntAttrib)
        Else
            GetTrxGenDateSequenceParams = "Invalid [startdate] attribute"
            Exit Function
        End If

        vntAttrib = elmChild.GetAttribute("enddate")
        If gblnXmlAttributeMissing(vntAttrib) Then
            datParams.NominalEndDate = vntAttrib
        Else
            If Utilities.blnIsValidDate(CStr(vntAttrib)) Then
                datParams.NominalEndDate = CDate(vntAttrib)
            Else
                GetTrxGenDateSequenceParams = "Invalid [enddate] attribute"
                Exit Function
            End If
        End If

    End Function

    '$Description Create and return an array of SequencedTrx from a set of XML
    '   elements.
    '$Param elmParent The parent element to all the elements to load from.
    '$Param strChildName The name of the child elements to load from.
    '$Param strError A descriptive error message if any problems found,
    '   else an empty string.
    '$Returns The array of SequencedTrx, dimensioned from zero to the number
    '   of SequencedTrx created. The zeroeth element is always unused.
    '   This allows a "for" loop from 1 to UBound(array) to interate the
    '   elements even if no SequencedTrx were created.

    Public Function LoadTrxGenSequencedTrx(ByVal elmParent As CBXmlElement, ByVal strChildName As String, ByVal dblDefaultPercentIncrease As Double, ByVal intStartRepeatSeq As Integer, ByRef strError As String) As SequencedTrx()

        Dim colSeq As CBXmlNodeList
        Dim elmSeq As CBXmlElement
        Dim datResults() As SequencedTrx
        Dim intResultIndex As Integer
        Dim strErrorEnding As String
        Dim intRepeatSeq As Integer

        ReDim LoadTrxGenSequencedTrx(0)
        strError = ""
        intRepeatSeq = intStartRepeatSeq
        colSeq = elmParent.SelectNodes(strChildName)
        If colSeq.Length = 0 Then
            strError = "No [" & strChildName & "] elements found"
            Exit Function
        End If
        ReDim datResults(colSeq.Length)
        intResultIndex = 1
        For Each elmSeq In colSeq
            strErrorEnding = " in [" & strChildName & "] element #" & intResultIndex
            datResults(intResultIndex) = CreateOneSequencedTrx(elmSeq, dblDefaultPercentIncrease, intRepeatSeq, strErrorEnding, strError)
            If strError <> "" Then
                Exit Function
            End If
            intRepeatSeq = intRepeatSeq + 1
            If intResultIndex > 1 Then
                If datResults(intResultIndex).TrxDate <= datResults(intResultIndex - 1).TrxDate Then
                    strError = "Date is not greater than the preceding entry" & strErrorEnding
                    Exit Function
                End If
            End If

            intResultIndex = intResultIndex + 1
        Next elmSeq

        Return DirectCast(datResults.Clone(), SequencedTrx())

    End Function

    '$Description Create one SequencedTrx from attributes of an XML element.

    Public Function CreateOneSequencedTrx(ByVal elmSeq As CBXmlElement, ByVal dblDefaultPercentIncrease As Double, ByVal intRepeatSeq As Integer, ByVal strErrorEnding As String, ByRef strError As String) As SequencedTrx

        Dim vntAttrib As Object
        Dim curAmount As Decimal
        Dim dblPercentIncrease As Double
        Dim datTrxDate As Date
        Dim objSeq As SequencedTrx

        CreateOneSequencedTrx = Nothing
        vntAttrib = elmSeq.GetAttribute("date")
        If gblnXmlAttributeMissing(vntAttrib) Then
            strError = "Missing [date] attribute" & strErrorEnding
            Exit Function
        End If
        If Utilities.blnIsValidDate(CStr(vntAttrib)) Then
            datTrxDate = CDate(vntAttrib)
        Else
            strError = "Invalid [date] attribute" & strErrorEnding
            Exit Function
        End If
        vntAttrib = elmSeq.GetAttribute("increasepercent")
        If gblnXmlAttributeMissing(vntAttrib) Then
            dblPercentIncrease = dblDefaultPercentIncrease
        Else
            If Not Utilities.blnIsValidAmount(CStr(vntAttrib)) Then
                strError = "Invalid [increasepercent] attribute" & strErrorEnding
                Exit Function
            End If
            dblPercentIncrease = CDbl(vntAttrib) / 100.0#
        End If
        vntAttrib = elmSeq.GetAttribute("amount")
        If gblnXmlAttributeMissing(vntAttrib) Then
            strError = "Missing [amount] attribute" & strErrorEnding
            Exit Function
        End If
        If Utilities.blnIsValidAmount(CStr(vntAttrib)) Then
            curAmount = CDec(vntAttrib) * CDec((1.0# + dblPercentIncrease))
        Else
            strError = "Invalid [amount] attribute" & strErrorEnding
            Exit Function
        End If
        objSeq = New SequencedTrx
        objSeq.Init(datTrxDate, curAmount, intRepeatSeq)

        CreateOneSequencedTrx = objSeq

    End Function

    '$Description Split periods and amounts into a longer list of periods and amounts
    '   such that the new list defines a curve which is a "smoother" version of the
    '   curve defined by the original list. Each division only redistributes time
    '   and amount, they do not change either. Does not split periods which are
    '   less than 2 days in length.
    '$Param datFirstPeriodStarts The starting date of the first period.
    '$Param datPeriodEndings The ending dates and amounts of each period.
    '$Param The number of days in the longest period returned.
    '$Returns The new period and amount list.

    Public Function SubdivideSequencedTrx(ByVal datFirstPeriodStarts As Date, ByRef datPeriodEndings() As SequencedTrx, ByRef intLongestOutputPeriod As Integer) As SequencedTrx()

        Dim datResults() As SequencedTrx
        Dim intInPeriods As Integer
        Dim intInIndex As Integer
        Dim intOutIndex As Integer
        Dim datPeriodStarts As Date
        Dim datNewEndDate As Date
        Dim curOldAmount As Decimal
        Dim curDiffBefore As Decimal
        Dim curDiffAfter As Decimal
        Dim curFirstNewAmount As Decimal
        Dim curSecondNewAmount As Decimal
        Dim intFirstHalfDays As Integer
        Dim intOldDays As Integer
        Dim intSecondHalfDays As Integer
        Dim objFirstNew As SequencedTrx
        Dim objSecondNew As SequencedTrx
        Dim curNetMove As Decimal
        Dim dblTemp As Double
        Dim dblMoveScale As Double

        ReDim datResults(UBound(datPeriodEndings) * 2)
        intLongestOutputPeriod = 0
        intInPeriods = UBound(datPeriodEndings)
        datPeriodStarts = datFirstPeriodStarts
        intOutIndex = 1
        For intInIndex = 1 To intInPeriods
            intOldDays = CInt(datPeriodEndings(intInIndex).TrxDate.ToOADate - datPeriodStarts.ToOADate + 1)
            'Can old period be divided?
            If intOldDays > 1 Then
                'Yes - so divide it.
                'Compute length of each sub-period.
                intFirstHalfDays = CInt(intOldDays / 2)
                intSecondHalfDays = intOldDays - intFirstHalfDays
                'Remember length of longest period in output array.
                If intFirstHalfDays > intLongestOutputPeriod Then
                    intLongestOutputPeriod = intFirstHalfDays
                End If
                If intSecondHalfDays > intLongestOutputPeriod Then
                    intLongestOutputPeriod = intSecondHalfDays
                End If
                'Compute ending date of first new sub-period.
                datNewEndDate = System.DateTime.FromOADate(datPeriodStarts.ToOADate + intFirstHalfDays - 1)
                'Compute new sub-period amounts.
                'First divide the amount of the old period proportionally
                'to the number of days in each sub-period.
                curOldAmount = datPeriodEndings(intInIndex).Amount
                curFirstNewAmount = CDec(System.Math.Round(curOldAmount * CDbl(intFirstHalfDays) / CDbl(intOldDays), 2))
                curSecondNewAmount = curOldAmount - curFirstNewAmount
                'Then move amount from one new split to the other based on
                'the difference between the old amount and the amounts of the
                'adjacent old periods to either side of it.
                If intInIndex = 1 Then
                    curDiffBefore = 0
                Else
                    curDiffBefore = curOldAmount - datPeriodEndings(intInIndex - 1).Amount
                End If
                If intInIndex = intInPeriods Then
                    curDiffAfter = 0
                Else
                    curDiffAfter = datPeriodEndings(intInIndex + 1).Amount - curOldAmount
                End If
                If (curDiffBefore + curDiffAfter) = 0 Then
                    curNetMove = 0
                Else
                    dblTemp = System.Math.Abs(curDiffBefore) + System.Math.Abs(curDiffAfter)
                    'dblMoveScale varies from one when before=after down
                    'toward zero as either before or after approaches zero.
                    'This is how we clamp curNetMove to zero when either of
                    'the adjacent splits is equal to the current split in amount.
                    dblMoveScale = System.Math.Abs(curDiffBefore * curDiffAfter) / ((dblTemp * dblTemp) / 4.0#)
                    curNetMove = CDec(System.Math.Round(dblMoveScale * (curDiffBefore + curDiffAfter) / 8.0#, 2))
                End If
                'Move curNetMove from first split to second.
                curFirstNewAmount = curFirstNewAmount - curNetMove
                curSecondNewAmount = curSecondNewAmount + curNetMove
                'Create new SequencedTrx
                objFirstNew = New SequencedTrx
                objFirstNew.Init(datNewEndDate, curFirstNewAmount, 0)
                datResults(intOutIndex) = objFirstNew
                objSecondNew = New SequencedTrx
                objSecondNew.Init(datPeriodEndings(intInIndex).TrxDate, curSecondNewAmount, 0)
                datResults(intOutIndex + 1) = objSecondNew
                intOutIndex = intOutIndex + 2
            Else
                'Period cannot be divided, so copy it unchanged to output.
                datResults(intOutIndex) = datPeriodEndings(intInIndex)
                intOutIndex = intOutIndex + 1
            End If
            'Prepare for next iteration.
            datPeriodStarts = System.DateTime.FromOADate(datPeriodEndings(intInIndex).TrxDate.ToOADate + 1)
        Next

        ReDim Preserve datResults(intOutIndex - 1)

        Return DirectCast(datResults.Clone(), SequencedTrx())

    End Function

    '$Description Initialize a TrxToCreate structure with information extracted from
    '   a <normaltrx>, <budgettrx> or <transfertrx> element which is a child of
    '   the document element. Called by most ITrxGenerator.strLoad() implementations.
    '$Param domDoc The XML document containing the element to read data from.
    '$Param strRepeatKey Set the repeat key in datTrxTemplate to this.
    '$Param curAmount Set amount fields in datTrxTemplate to this.
    '$Param datTrxTemplate The output TrxToCreate.
    '$Returns A non-empty error message if bad or missing data was encountered,
    '   else an empty string.

    Public Function GetTrxGenTemplate(ByVal objCompany As Company, ByRef domDoc As CBXmlDocument, ByVal strRepeatKey As String, ByVal curAmount As Decimal, ByRef datTrxTemplate As TrxToCreate) As String

        Dim elmTrxTpt As CBXmlElement

        'Clear everything.
        'LSet datTrxTemplate = mdatNullTrxToCreate
        datTrxTemplate = CopyTrxToCreate(mdatNullTrxToCreate)

        elmTrxTpt = DirectCast(domDoc.DocumentElement.SelectSingleNode("normaltrx"), CBXmlElement)
        If elmTrxTpt Is Nothing Then
            elmTrxTpt = DirectCast(domDoc.DocumentElement.SelectSingleNode("budgettrx"), CBXmlElement)
            If elmTrxTpt Is Nothing Then
                elmTrxTpt = DirectCast(domDoc.DocumentElement.SelectSingleNode("transfertrx"), CBXmlElement)
                If elmTrxTpt Is Nothing Then
                    GetTrxGenTemplate = "Unable to find <normaltrx>, <budgettrx> or <transfertrx> element"
                    Exit Function
                Else
                    'Set transfer trx fields.
                    GetTrxGenTemplate = GetTrxGenTemplateShared(elmTrxTpt, strRepeatKey, datTrxTemplate)
                End If
            Else
                'Set budget trx fields.
                GetTrxGenTemplate = GetTrxGenTemplateBudget(objCompany, elmTrxTpt, strRepeatKey, curAmount, datTrxTemplate)
            End If
        Else
            'Set normal trx fields.
            GetTrxGenTemplate = GetTrxGenTemplateBank(objCompany, elmTrxTpt, strRepeatKey, curAmount, datTrxTemplate)
        End If

    End Function

    '$Description Set fields of a TrxToCreate structure that are used by a transfer BaseTrx,
    '   from the arguments passed in.

    Public Function GetTrxGenTemplateTransfer(ByVal elmTrxTpt As CBXmlElement, ByVal strRepeatKey As String, ByVal curAmount As Decimal, ByRef datTrxTemplate As TrxToCreate) As String

        Dim vntAttrib As Object

        datTrxTemplate.TrxType = GetType(TransferTrx)
        datTrxTemplate.Status = BaseTrx.TrxStatus.NonBank
        'Amount.
        datTrxTemplate.Amount = curAmount
        'AccountKey of other register.
        vntAttrib = elmTrxTpt.GetAttribute("transferkey")
        If gblnXmlAttributeMissing(vntAttrib) Then
            GetTrxGenTemplateTransfer = "Missing [transferkey] attribute"
            Exit Function
        End If
        datTrxTemplate.TransferKey = CStr(vntAttrib)

        'Set shared fields.
        GetTrxGenTemplateTransfer = GetTrxGenTemplateShared(elmTrxTpt, strRepeatKey, datTrxTemplate)

    End Function

    '$Description Set fields of a TrxToCreate structure that are used by a budget BaseTrx,
    '   from the arguments passed in.

    Public Function GetTrxGenTemplateBudget(ByVal objCompany As Company, ByVal elmTrxTpt As CBXmlElement, ByVal strRepeatKey As String, ByVal curAmount As Decimal, ByRef datTrxTemplate As TrxToCreate) As String

        Dim vntAttrib As Object

        datTrxTemplate.TrxType = GetType(BudgetTrx)
        datTrxTemplate.Status = BaseTrx.TrxStatus.NonBank
        'Budget key.
        vntAttrib = elmTrxTpt.GetAttribute("budgetkey")
        If gblnXmlAttributeMissing(vntAttrib) Then
            GetTrxGenTemplateBudget = "Missing [budgetkey] attribute"
            Exit Function
        End If
        If objCompany.Budgets.intLookupKey(CStr(vntAttrib)) = 0 Then
            GetTrxGenTemplateBudget = "Invalid [budgetkey] attribute"
            Exit Function
        End If
        datTrxTemplate.BudgetKey = CStr(vntAttrib)
        'Budget unit.
        vntAttrib = elmTrxTpt.GetAttribute("budgetunit")
        If gblnXmlAttributeMissing(vntAttrib) Then
            GetTrxGenTemplateBudget = "Missing [budgetunit] attribute"
            Exit Function
        End If
        datTrxTemplate.BudgetUnit = ConvertTrxGenRepeatUnit(UCase(CStr(vntAttrib)))
        If datTrxTemplate.BudgetUnit = BaseTrx.RepeatUnit.Missing Then
            GetTrxGenTemplateBudget = "Invalid [budgetunit] attribute"
            Exit Function
        End If
        'Budget number.
        vntAttrib = elmTrxTpt.GetAttribute("budgetnumber")
        If gblnXmlAttributeMissing(vntAttrib) Then
            GetTrxGenTemplateBudget = "Missing [budgetnumber] attribute"
            Exit Function
        End If
        datTrxTemplate.BudgetNumber = ConvertTrxGenRepeatCount(CStr(vntAttrib))
        If datTrxTemplate.BudgetNumber = 0 Then
            GetTrxGenTemplateBudget = "Invalid [budgetnumber] attribute"
            Exit Function
        End If
        'Amount
        datTrxTemplate.Amount = curAmount

        'Set shared fields.
        GetTrxGenTemplateBudget = GetTrxGenTemplateShared(elmTrxTpt, strRepeatKey, datTrxTemplate)

    End Function

    '$Description Set fields of a TrxToCreate structure that are used by a normal BaseTrx,
    '   from the arguments passed in.

    Public Function GetTrxGenTemplateBank(ByVal objCompany As Company, ByVal elmTrxTpt As CBXmlElement, ByVal strRepeatKey As String, ByVal curAmount As Decimal, ByRef datTrxTemplate As TrxToCreate) As String

        Dim vntAttrib As Object
        Dim datSplit As SplitToCreate = Nothing
        Dim strError As String

        'Set shared fields.
        strError = GetTrxGenTemplateShared(elmTrxTpt, strRepeatKey, datTrxTemplate)
        If strError <> "" Then
            Return strError
        End If

        datTrxTemplate.TrxType = GetType(BankTrx)
        datTrxTemplate.Status = BaseTrx.TrxStatus.Unreconciled
        'Transaction number.
        vntAttrib = elmTrxTpt.GetAttribute("number")
        If gblnXmlAttributeMissing(vntAttrib) Then
            GetTrxGenTemplateBank = "Missing [number] attribute"
            Exit Function
        End If
        If InStr("|pmt|ord|inv|dep|xfr|eft|crm|card|", "|" & LCase(CStr(vntAttrib)) & "|") = 0 Then
            GetTrxGenTemplateBank = "Invalid [number] attribute"
            Exit Function
        End If
        datTrxTemplate.Number = CStr(vntAttrib) '
        'Category key.
        vntAttrib = elmTrxTpt.GetAttribute("catkey")
        If gblnXmlAttributeMissing(vntAttrib) Then
            GetTrxGenTemplateBank = "Missing [catkey] attribute"
            Exit Function
        End If
        If objCompany.Categories.intLookupKey(CStr(vntAttrib)) = 0 Then
            GetTrxGenTemplateBank = "Invalid [catkey] attribute"
            Exit Function
        End If
        datSplit.CategoryKey = CStr(vntAttrib)
        'Budget key.
        vntAttrib = elmTrxTpt.GetAttribute("budgetkey")
        If Not gblnXmlAttributeMissing(vntAttrib) Then
            If objCompany.Budgets.intLookupKey(CStr(vntAttrib)) = 0 Then
                GetTrxGenTemplateBank = "Invalid [budgetkey] attribute"
                Exit Function
            End If
            datSplit.BudgetKey = CStr(vntAttrib)
        End If
        'Amount.
        datSplit.Amount = curAmount
        'No dates.
        datSplit.InvoiceDate = Utilities.datEmpty
        datSplit.DueDate = Utilities.datEmpty
        'Add to splits collection.
        datTrxTemplate.SplitCount = 1
        ReDim datTrxTemplate.Splits(1)
        datTrxTemplate.Splits(1) = datSplit

        Return ""

    End Function

    '$Description Set fields of a TrxToCreate structure that are common to all BaseTrx
    '   types, from the arguments passed in.

    Public Function GetTrxGenTemplateShared(ByVal elmTrxTpt As CBXmlElement, ByVal strRepeatKey As String, ByRef datTrxTemplate As TrxToCreate) As String

        Dim vntAttrib As Object

        GetTrxGenTemplateShared = ""
        'Description.
        vntAttrib = elmTrxTpt.GetAttribute("description")
        If gblnXmlAttributeMissing(vntAttrib) Then
            GetTrxGenTemplateShared = "Missing [description] attribute"
            Exit Function
        End If
        If Len(vntAttrib) = 0 Then
            GetTrxGenTemplateShared = "Empty [description] attribute"
            Exit Function
        End If
        datTrxTemplate.Description = CStr(vntAttrib)
        'Memo.
        vntAttrib = elmTrxTpt.GetAttribute("memo")
        If Not gblnXmlAttributeMissing(vntAttrib) Then
            datTrxTemplate.Memo = CStr(vntAttrib)
        End If
        'Other.
        datTrxTemplate.IsFake = True
        datTrxTemplate.IsAutoGenerated = True
        datTrxTemplate.RepeatKey = strRepeatKey

    End Function

    Public Function CopyTrxToCreate(ByRef datInput As TrxToCreate) As TrxToCreate
        CopyTrxToCreate = datInput
        'Necessary for .NET compatibility, because .NET adatSplits is an object
        'and the code converter doesn't notice so all copies share the same array.
        'By doing it explicitly the code converter treats the copy with value semantics
        'instead of reference semantics.
        'gdatCopyTrxToCreate.adatSplits = VB6.CopyArray(datInput.adatSplits)
        If datInput.Splits Is Nothing Then
            CopyTrxToCreate.Splits = Nothing
        Else
            CopyTrxToCreate.Splits = DirectCast(datInput.Splits.Clone(), SplitToCreate())
        End If
    End Function
End Module