Option Strict Off
Option Explicit On

Public Module TrxGeneratorLoader
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Routines related to creating ITrxGenerator objects from XML files.

    'UPGRADE_WARNING: Arrays in structure mdatNullTrxToCreate may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
    Private mdatNullTrxToCreate As TrxToCreate

    '$Description Create the ITrxGenerator for the specified XML document, and call
    '   Load() for it. Displays a diagnostic error if bad or missing data in XML document.
    '$Returns The ITrxGenerator created if successful, or Nothing.

    Private Function objCreateTrxGenerator(ByVal domDoc As VB6XmlDocument, ByVal objAccount As Account) As ITrxGenerator

        Dim vntClassName As Object
        Dim strClassName As String
        Dim objGenerator As ITrxGenerator
        Dim strError As String

        If domDoc.DocumentElement.Name <> "generator" Then
            gShowTrxGenLoadError(domDoc, "Document element is not <generator>")
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object domDoc.documentElement.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntClassName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntClassName = domDoc.DocumentElement.GetAttribute("class")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntClassName) Then
            gShowTrxGenLoadError(domDoc, "Missing class attribute")
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntClassName. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
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
            gShowTrxGenLoadError(domDoc, "Unrecognized class " & strClassName)
            Exit Function
        End If
        strError = objGenerator.strLoad(domDoc, objAccount)
        If strError = "" Then
            objCreateTrxGenerator = objGenerator
        Else
            gShowTrxGenLoadError(domDoc, strError)
        End If

    End Function

    '$Description Create all ITrxGenerator objects for a register. If a data problem
    '   is encountered will display a message and not create that object.
    '$Returns The Collection of ITrxGenerator objects created.

    Public Function gcolCreateTrxGenerators(ByVal objAccount As Account, ByVal objReg As Register) As Collection

        Dim strPath As String
        Dim strXMLFile As String
        Dim strFullXMLFile As String
        Dim domDoc As VB6XmlDocument
        Dim objParseError As VB6XmlParseError
        Dim objGenerator As ITrxGenerator
        Dim colResults As Collection
        Dim strRepeatKeysUsed As String
        Dim strThisRepeatKey As String

        colResults = New Collection
        strPath = gstrGeneratorPath(objAccount, objReg)
        strXMLFile = Dir(strPath & "\*.gen")
        While strXMLFile <> ""
            domDoc = New VB6XmlDocument
            strFullXMLFile = strPath & "\" & strXMLFile
            Try

                domDoc.Load(strFullXMLFile)
                objParseError = domDoc.ParseError
                If Not objParseError Is Nothing Then
                    ShowTrxGenLoadError(strFullXMLFile, gstrXMLParseErrorText(objParseError))
                Else
                    domDoc.SetProperty("SelectionLanguage", "XPath")
                    objGenerator = objCreateTrxGenerator(domDoc, objAccount)
                    If Not objGenerator Is Nothing Then
                        strThisRepeatKey = "|" & objGenerator.strRepeatKey & "|"
                        If InStr(strRepeatKeysUsed, strThisRepeatKey) = 0 Then
                            colResults.Add(objGenerator)
                            strRepeatKeysUsed = strRepeatKeysUsed & strThisRepeatKey
                        Else
                            gShowTrxGenLoadError(domDoc, "Repeat key used by another generator")
                        End If
                    End If
                End If
                'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            Catch ex As Exception
                Throw New Exception("Error loading trx generator [" + strFullXMLFile + "]", ex)
            End Try
            strXMLFile = Dir()
        End While
        gcolCreateTrxGenerators = colResults

    End Function

    '$Description Path to folder with *.gen (transaction generators) for specified 
    '   register in specified account.

    Public Function gstrGeneratorPath(ByVal objAccount As Account, ByVal objReg As Register) As String
        gstrGeneratorPath = gstrAccountPath() & "\" & objAccount.strFileLoaded & ".gen\" & objReg.strRegisterKey
    End Function

    '$Description Report an error detected while loading a transaction generator file.

    Public Sub gShowTrxGenLoadError(ByVal domDoc As VB6XmlDocument, ByVal strError As String)
        Dim strDescription As String
        Dim vntDescription As Object
        'UPGRADE_WARNING: Couldn't resolve default property of object domDoc.documentElement.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntDescription. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntDescription = domDoc.DocumentElement.GetAttribute("description")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntDescription) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vntDescription. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vntDescription = ""
        End If
        If Len(vntDescription) = 0 Then
            strDescription = domDoc.FullPath
        Else
            'UPGRADE_WARNING: Couldn't resolve default property of object vntDescription. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            strDescription = CStr(vntDescription)
        End If
        ShowTrxGenLoadError(strDescription, strError)
    End Sub

    Public Sub ShowTrxGenLoadError(ByVal strDescription As String, ByVal strError As String)
        MsgBox("Error loading transaction generator [" & strDescription & "]:" & vbCrLf & strError, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Checkbook")
    End Sub

    Public Function gdatIncrementDate(ByVal datStart As Date, ByVal lngUnit As Trx.RepeatUnit, ByVal intNumber As Short) As Date

        Select Case lngUnit
            Case Trx.RepeatUnit.glngRPTUNT_DAY
                gdatIncrementDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intNumber, datStart)
            Case Trx.RepeatUnit.glngRPTUNT_WEEK
                gdatIncrementDate = DateAdd(Microsoft.VisualBasic.DateInterval.WeekOfYear, intNumber, datStart)
            Case Trx.RepeatUnit.glngRPTUNT_MONTH
                gdatIncrementDate = DateAdd(Microsoft.VisualBasic.DateInterval.Month, intNumber, datStart)
        End Select
    End Function

    Public Function glngConvertRepeatUnit(ByVal strInput As String) As Trx.RepeatUnit
        Select Case strInput
            Case "DAY"
                glngConvertRepeatUnit = Trx.RepeatUnit.glngRPTUNT_DAY
            Case "WEEK"
                glngConvertRepeatUnit = Trx.RepeatUnit.glngRPTUNT_WEEK
            Case "MONTH"
                glngConvertRepeatUnit = Trx.RepeatUnit.glngRPTUNT_MONTH
            Case Else
                glngConvertRepeatUnit = Trx.RepeatUnit.glngRPTUNT_MISSING
        End Select
    End Function

    Public Function gintConvertRepeatCount(ByVal strInput As String) As Short
        Dim intResult As Short

        If IsNumeric(strInput) Then
            intResult = CShort(strInput)
            If intResult < 1 Then
                gintConvertRepeatCount = 0
            Else
                gintConvertRepeatCount = intResult
            End If
        Else
            gintConvertRepeatCount = 0
        End If
    End Function

    '$Description Extract from an XML document the core information which must be
    '   defined for all trx generators. Must be called from ITrxGenerator.strLoad().
    '$Returns A non-empty error message if bad or missing data was encountered,
    '   else an empty string.

    Public Function gstrLoadTrxGeneratorCore(ByVal domDoc As VB6XmlDocument, ByRef blnEnabled As Boolean, ByRef strRepeatKey As String, ByRef intStartRepeatSeq As Short, ByRef strDescription As String, ByVal objAccount As Account) As String

        Dim vntAttrib As Object

        'UPGRADE_WARNING: Couldn't resolve default property of object domDoc.documentElement.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = domDoc.DocumentElement.GetAttribute("enabled")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrLoadTrxGeneratorCore = "Missing [enabled] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If vntAttrib <> "true" And vntAttrib <> "false" Then
            gstrLoadTrxGeneratorCore = "[enabled] attribute must be ""true"" or ""false"""
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        blnEnabled = CBool(vntAttrib)

        'UPGRADE_WARNING: Couldn't resolve default property of object domDoc.documentElement.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = domDoc.DocumentElement.GetAttribute("repeatkey")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrLoadTrxGeneratorCore = "Missing [repeatkey] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        strRepeatKey = CStr(vntAttrib)
        'If objAccount.objRepeats.intLookupKey(strRepeatKey) = 0 Then
        '    gstrLoadTrxGeneratorCore = "Invalid [repeatkey] attribute"
        '    Exit Function
        'End If

        'UPGRADE_WARNING: Couldn't resolve default property of object domDoc.documentElement.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = domDoc.DocumentElement.GetAttribute("description")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrLoadTrxGeneratorCore = "Missing [description] attribute"
            Exit Function
        End If
        If Len(vntAttrib) = 0 Then
            gstrLoadTrxGeneratorCore = "Empty [description] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        strDescription = CStr(vntAttrib)

        'UPGRADE_WARNING: Couldn't resolve default property of object domDoc.documentElement.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = domDoc.DocumentElement.GetAttribute("startseq")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrLoadTrxGeneratorCore = "Missing [startseq] attribute"
            Exit Function
        End If
        If Not IsNumeric(vntAttrib) Then
            gstrLoadTrxGeneratorCore = "Invalid [startseq] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
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

    Public Function gstrGetDateSequenceParams(ByVal elmParent As VB6XmlElement, ByVal strChildName As String, ByRef elmChild As VB6XmlElement, ByRef datParams As DateSequenceParams) As String

        Dim vntAttrib As Object

        elmChild = elmParent.SelectSingleNode(strChildName)
        If elmChild Is Nothing Then
            gstrGetDateSequenceParams = "Could not find <" & strChildName & "> element"
            Exit Function
        End If

        vntAttrib = elmChild.GetAttribute("unit")
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrGetDateSequenceParams = "Missing [unit] attribute"
            Exit Function
        End If
        datParams.lngRptUnit = glngConvertRepeatUnit(UCase(vntAttrib))
        If datParams.lngRptUnit = Trx.RepeatUnit.glngRPTUNT_MISSING Then
            gstrGetDateSequenceParams = "Invalid [unit] attribute"
            Exit Function
        End If

        vntAttrib = elmChild.GetAttribute("interval")
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrGetDateSequenceParams = "Missing [interval] attribute"
            Exit Function
        End If
        datParams.intRptNumber = gintConvertRepeatCount(CStr(vntAttrib))
        If datParams.intRptNumber = 0 Then
            gstrGetDateSequenceParams = "Invalid [interval] attribute"
            Exit Function
        End If

        vntAttrib = elmChild.GetAttribute("startdate")
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrGetDateSequenceParams = "Missing [startdate] attribute"
            Exit Function
        End If
        If gblnValidDate(CStr(vntAttrib)) Then
            datParams.datNominalStartDate = CDate(vntAttrib)
        Else
            gstrGetDateSequenceParams = "Invalid [startdate] attribute"
            Exit Function
        End If

        vntAttrib = elmChild.GetAttribute("enddate")
        If gblnXmlAttributeMissing(vntAttrib) Then
            datParams.vntNominalEndDate = vntAttrib
        Else
            If gblnValidDate(CStr(vntAttrib)) Then
                datParams.vntNominalEndDate = CDate(vntAttrib)
            Else
                gstrGetDateSequenceParams = "Invalid [enddate] attribute"
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

    Public Function gdatLoadSequencedTrx(ByVal elmParent As VB6XmlElement, ByVal strChildName As String, ByVal dblDefaultPercentIncrease As Double, ByVal intStartRepeatSeq As Short, ByRef strError As String) As SequencedTrx()

        Dim colSeq As VB6XmlNodeList
        Dim elmSeq As VB6XmlElement
        Dim datResults() As SequencedTrx
        Dim intResultIndex As Short
        Dim strErrorEnding As String
        Dim intRepeatSeq As Short

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
            datResults(intResultIndex) = gdatCreateOneSequencedTrx(elmSeq, dblDefaultPercentIncrease, intRepeatSeq, strErrorEnding, strError)
            If strError <> "" Then
                Exit Function
            End If
            intRepeatSeq = intRepeatSeq + 1
            If intResultIndex > 1 Then
                If datResults(intResultIndex).datDate <= datResults(intResultIndex - 1).datDate Then
                    strError = "Date is not greater than the preceding entry" & strErrorEnding
                    Exit Function
                End If
            End If

            intResultIndex = intResultIndex + 1
        Next elmSeq

        gdatLoadSequencedTrx = VB6.CopyArray(datResults)

    End Function

    '$Description Create one SequencedTrx from attributes of an XML element.

    Public Function gdatCreateOneSequencedTrx(ByVal elmSeq As VB6XmlElement, ByVal dblDefaultPercentIncrease As Double, ByVal intRepeatSeq As Short, ByVal strErrorEnding As String, ByRef strError As String) As SequencedTrx

        Dim vntAttrib As Object
        Dim curAmount As Decimal
        Dim dblPercentIncrease As Double
        Dim datTrxDate As Date
        Dim objSeq As SequencedTrx

        'UPGRADE_WARNING: Couldn't resolve default property of object elmSeq.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmSeq.GetAttribute("date")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            strError = "Missing [date] attribute" & strErrorEnding
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If gblnValidDate(vntAttrib) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            datTrxDate = CDate(vntAttrib)
        Else
            strError = "Invalid [date] attribute" & strErrorEnding
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object elmSeq.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmSeq.GetAttribute("increasepercent")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            dblPercentIncrease = dblDefaultPercentIncrease
        Else
            'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If Not gblnValidAmount(vntAttrib) Then
                strError = "Invalid [increasepercent] attribute" & strErrorEnding
                Exit Function
            End If
            'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            dblPercentIncrease = CDbl(vntAttrib) / 100.0#
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object elmSeq.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmSeq.GetAttribute("amount")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            strError = "Missing [amount] attribute" & strErrorEnding
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If gblnValidAmount(vntAttrib) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            curAmount = CDec(vntAttrib) * (1.0# + dblPercentIncrease)
        Else
            strError = "Invalid [amount] attribute" & strErrorEnding
            Exit Function
        End If
        objSeq = New SequencedTrx
        objSeq.Init(datTrxDate, curAmount, intRepeatSeq)

        gdatCreateOneSequencedTrx = objSeq

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

    Public Function gdatSplitSequencedTrx(ByVal datFirstPeriodStarts As Date, ByRef datPeriodEndings() As SequencedTrx, ByRef intLongestOutputPeriod As Short) As SequencedTrx()

        Dim datResults() As SequencedTrx
        Dim intInPeriods As Short
        Dim intInIndex As Short
        Dim intOutIndex As Short
        Dim datPeriodStarts As Date
        Dim datNewEndDate As Date
        Dim curOldAmount As Decimal
        Dim curDiffBefore As Decimal
        Dim curDiffAfter As Decimal
        Dim curFirstNewAmount As Decimal
        Dim curSecondNewAmount As Decimal
        Dim intFirstHalfDays As Short
        Dim intOldDays As Short
        Dim intSecondHalfDays As Short
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
            intOldDays = datPeriodEndings(intInIndex).datDate.ToOADate - datPeriodStarts.ToOADate + 1
            'Can old period be divided?
            If intOldDays > 1 Then
                'Yes - so divide it.
                'Compute length of each sub-period.
                intFirstHalfDays = intOldDays / 2
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
                curOldAmount = datPeriodEndings(intInIndex).curAmount
                curFirstNewAmount = System.Math.Round(curOldAmount * CDbl(intFirstHalfDays) / CDbl(intOldDays), 2)
                curSecondNewAmount = curOldAmount - curFirstNewAmount
                'Then move amount from one new split to the other based on
                'the difference between the old amount and the amounts of the
                'adjacent old periods to either side of it.
                If intInIndex = 1 Then
                    curDiffBefore = 0
                Else
                    curDiffBefore = curOldAmount - datPeriodEndings(intInIndex - 1).curAmount
                End If
                If intInIndex = intInPeriods Then
                    curDiffAfter = 0
                Else
                    curDiffAfter = datPeriodEndings(intInIndex + 1).curAmount - curOldAmount
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
                    curNetMove = System.Math.Round(dblMoveScale * (curDiffBefore + curDiffAfter) / 8.0#, 2)
                End If
                'Move curNetMove from first split to second.
                curFirstNewAmount = curFirstNewAmount - curNetMove
                curSecondNewAmount = curSecondNewAmount + curNetMove
                'Create new SequencedTrx
                objFirstNew = New SequencedTrx
                objFirstNew.Init(datNewEndDate, curFirstNewAmount, 0)
                datResults(intOutIndex) = objFirstNew
                objSecondNew = New SequencedTrx
                objSecondNew.Init(datPeriodEndings(intInIndex).datDate, curSecondNewAmount, 0)
                datResults(intOutIndex + 1) = objSecondNew
                intOutIndex = intOutIndex + 2
            Else
                'Period cannot be divided, so copy it unchanged to output.
                datResults(intOutIndex) = datPeriodEndings(intInIndex)
                intOutIndex = intOutIndex + 1
            End If
            'Prepare for next iteration.
            datPeriodStarts = System.DateTime.FromOADate(datPeriodEndings(intInIndex).datDate.ToOADate + 1)
        Next

        ReDim Preserve datResults(intOutIndex - 1)

        gdatSplitSequencedTrx = VB6.CopyArray(datResults)

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

    Public Function gstrGetTrxGenTemplate(ByRef domDoc As VB6XmlDocument, ByVal strRepeatKey As String, ByVal curAmount As Decimal, ByRef datTrxTemplate As TrxToCreate) As String

        Dim elmTrxTpt As VB6XmlElement

        'Clear everything.
        'LSet datTrxTemplate = mdatNullTrxToCreate
        'UPGRADE_WARNING: Couldn't resolve default property of object datTrxTemplate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        datTrxTemplate = gdatCopyTrxToCreate(mdatNullTrxToCreate)

        elmTrxTpt = domDoc.DocumentElement.SelectSingleNode("normaltrx")
        If elmTrxTpt Is Nothing Then
            elmTrxTpt = domDoc.DocumentElement.SelectSingleNode("budgettrx")
            If elmTrxTpt Is Nothing Then
                elmTrxTpt = domDoc.DocumentElement.SelectSingleNode("transfertrx")
                If elmTrxTpt Is Nothing Then
                    gstrGetTrxGenTemplate = "Unable to find <normaltrx>, <budgettrx> or <transfertrx> element"
                    Exit Function
                Else
                    'Set transfer trx fields.
                    gstrGetTrxGenTemplate = gstrGetTrxGenTemplateShared(elmTrxTpt, strRepeatKey, datTrxTemplate)
                End If
            Else
                'Set budget trx fields.
                gstrGetTrxGenTemplate = gstrGetTrxGenTemplateBudget(elmTrxTpt, strRepeatKey, curAmount, datTrxTemplate)
            End If
        Else
            'Set normal trx fields.
            gstrGetTrxGenTemplate = gstrGetTrxGenTemplateNormal(elmTrxTpt, strRepeatKey, curAmount, datTrxTemplate)
        End If

    End Function

    '$Description Set fields of a TrxToCreate structure that are used by a transfer Trx,
    '   from the arguments passed in.

    Public Function gstrGetTrxGenTemplateTransfer(ByVal elmTrxTpt As VB6XmlElement, ByVal strRepeatKey As String, ByVal curAmount As Decimal, ByRef datTrxTemplate As TrxToCreate) As String

        Dim vntAttrib As Object

        datTrxTemplate.lngType = Trx.TrxType.glngTRXTYP_TRANSFER
        datTrxTemplate.lngStatus = Trx.TrxStatus.glngTRXSTS_NONBANK
        'Amount.
        datTrxTemplate.curAmount = curAmount
        'Key of other register.
        'UPGRADE_WARNING: Couldn't resolve default property of object elmTrxTpt.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmTrxTpt.GetAttribute("transferkey")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrGetTrxGenTemplateTransfer = "Missing [transferkey] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        datTrxTemplate.strTransferKey = CStr(vntAttrib)

        'Set shared fields.
        gstrGetTrxGenTemplateTransfer = gstrGetTrxGenTemplateShared(elmTrxTpt, strRepeatKey, datTrxTemplate)

    End Function

    '$Description Set fields of a TrxToCreate structure that are used by a budget Trx,
    '   from the arguments passed in.

    Public Function gstrGetTrxGenTemplateBudget(ByVal elmTrxTpt As VB6XmlElement, ByVal strRepeatKey As String, ByVal curAmount As Decimal, ByRef datTrxTemplate As TrxToCreate) As String

        Dim vntAttrib As Object

        datTrxTemplate.lngType = Trx.TrxType.glngTRXTYP_BUDGET
        datTrxTemplate.lngStatus = Trx.TrxStatus.glngTRXSTS_NONBANK
        'Budget key.
        'UPGRADE_WARNING: Couldn't resolve default property of object elmTrxTpt.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmTrxTpt.GetAttribute("budgetkey")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrGetTrxGenTemplateBudget = "Missing [budgetkey] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If gobjBudgets.intLookupKey(vntAttrib) = 0 Then
            gstrGetTrxGenTemplateBudget = "Invalid [budgetkey] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        datTrxTemplate.strBudgetKey = CStr(vntAttrib)
        'Budget unit.
        'UPGRADE_WARNING: Couldn't resolve default property of object elmTrxTpt.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmTrxTpt.GetAttribute("budgetunit")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrGetTrxGenTemplateBudget = "Missing [budgetunit] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        datTrxTemplate.lngBudgetUnit = glngConvertRepeatUnit(UCase(vntAttrib))
        If datTrxTemplate.lngBudgetUnit = Trx.RepeatUnit.glngRPTUNT_MISSING Then
            gstrGetTrxGenTemplateBudget = "Invalid [budgetunit] attribute"
            Exit Function
        End If
        'Budget number.
        'UPGRADE_WARNING: Couldn't resolve default property of object elmTrxTpt.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmTrxTpt.GetAttribute("budgetnumber")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrGetTrxGenTemplateBudget = "Missing [budgetnumber] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        datTrxTemplate.intBudgetNumber = gintConvertRepeatCount(vntAttrib)
        If datTrxTemplate.intBudgetNumber = 0 Then
            gstrGetTrxGenTemplateBudget = "Invalid [budgetnumber] attribute"
            Exit Function
        End If
        'Amount
        datTrxTemplate.curAmount = curAmount

        'Set shared fields.
        gstrGetTrxGenTemplateBudget = gstrGetTrxGenTemplateShared(elmTrxTpt, strRepeatKey, datTrxTemplate)

    End Function

    '$Description Set fields of a TrxToCreate structure that are used by a normal Trx,
    '   from the arguments passed in.

    Public Function gstrGetTrxGenTemplateNormal(ByVal elmTrxTpt As VB6XmlElement, ByVal strRepeatKey As String, ByVal curAmount As Decimal, ByRef datTrxTemplate As TrxToCreate) As String

        Dim vntAttrib As Object
        Dim datSplit As SplitToCreate

        datTrxTemplate.lngType = Trx.TrxType.glngTRXTYP_NORMAL
        datTrxTemplate.lngStatus = Trx.TrxStatus.glngTRXSTS_UNREC
        'Transaction number.
        vntAttrib = elmTrxTpt.GetAttribute("number")
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrGetTrxGenTemplateNormal = "Missing [number] attribute"
            Exit Function
        End If
        If InStr("|pmt|ord|inv|dep|xfr|eft|crm|card|", "|" & LCase(vntAttrib) & "|") = 0 Then
            gstrGetTrxGenTemplateNormal = "Invalid [number] attribute"
            Exit Function
        End If
        datTrxTemplate.strNumber = CStr(vntAttrib) '
        'Category key.
        vntAttrib = elmTrxTpt.GetAttribute("catkey")
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrGetTrxGenTemplateNormal = "Missing [catkey] attribute"
            Exit Function
        End If
        If gobjCategories.intLookupKey(vntAttrib) = 0 Then
            gstrGetTrxGenTemplateNormal = "Invalid [catkey] attribute"
            Exit Function
        End If
        datSplit.strCategoryKey = CStr(vntAttrib)
        'Budget key.
        vntAttrib = elmTrxTpt.GetAttribute("budgetkey")
        If Not gblnXmlAttributeMissing(vntAttrib) Then
            If gobjBudgets.intLookupKey(vntAttrib) = 0 Then
                gstrGetTrxGenTemplateNormal = "Invalid [budgetkey] attribute"
                Exit Function
            End If
            datSplit.strBudgetKey = CStr(vntAttrib)
        End If
        'Amount.
        datSplit.curAmount = curAmount
        'No dates.
        datSplit.datInvoiceDate = System.DateTime.FromOADate(0)
        datSplit.datDueDate = System.DateTime.FromOADate(0)
        'Add to splits collection.
        datTrxTemplate.intSplits = 1
        ReDim datTrxTemplate.adatSplits(1)
        datTrxTemplate.adatSplits(1) = datSplit

        'Set shared fields.
        gstrGetTrxGenTemplateNormal = gstrGetTrxGenTemplateShared(elmTrxTpt, strRepeatKey, datTrxTemplate)

    End Function

    '$Description Set fields of a TrxToCreate structure that are common to all Trx
    '   types, from the arguments passed in.

    Public Function gstrGetTrxGenTemplateShared(ByVal elmTrxTpt As VB6XmlElement, ByVal strRepeatKey As String, ByRef datTrxTemplate As TrxToCreate) As String

        Dim vntAttrib As Object

        'Description.
        'UPGRADE_WARNING: Couldn't resolve default property of object elmTrxTpt.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmTrxTpt.GetAttribute("description")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            gstrGetTrxGenTemplateShared = "Missing [description] attribute"
            Exit Function
        End If
        If Len(vntAttrib) = 0 Then
            gstrGetTrxGenTemplateShared = "Empty [description] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        datTrxTemplate.strDescription = CStr(vntAttrib)
        'Memo.
        'UPGRADE_WARNING: Couldn't resolve default property of object elmTrxTpt.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmTrxTpt.GetAttribute("memo")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If Not gblnXmlAttributeMissing(vntAttrib) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            datTrxTemplate.strMemo = CStr(vntAttrib)
        End If
        'Other.
        datTrxTemplate.blnFake = True
        datTrxTemplate.blnAutoGenerated = True
        datTrxTemplate.strRepeatKey = strRepeatKey

    End Function

    Public Function gdatCopyTrxToCreate(ByRef datInput As TrxToCreate) As TrxToCreate
        'UPGRADE_WARNING: Couldn't resolve default property of object gdatCopyTrxToCreate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        gdatCopyTrxToCreate = datInput
        'Necessary for .NET compatibility, because .NET adatSplits is an object
        'and the code converter doesn't notice so all copies share the same array.
        'By doing it explicitly the code converter treats the copy with value semantics
        'instead of reference semantics.
        gdatCopyTrxToCreate.adatSplits = VB6.CopyArray(datInput.adatSplits)
    End Function
End Module