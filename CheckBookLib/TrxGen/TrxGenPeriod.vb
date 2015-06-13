Option Strict Off
Option Explicit On
Public Class TrxGenPeriod
    Implements ITrxGenerator
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345


    Private mstrDescription As String
    Private mblnEnabled As Boolean
    Private mdatFirstPeriodStarts As Date
    Private mdatPeriodEndings() As SequencedTrx
    Private mdblDefaultPercentIncrease As Double
    'UPGRADE_WARNING: Arrays in structure mdatTrxTemplate may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
    Private mdatTrxTemplate As TrxToCreate
    Private mstrRepeatKey As String
    Private mintStartRepeatSeq As Short
    'UPGRADE_WARNING: Lower bound of array mdblDOWUsage was changed from gintLBOUND1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
    Private mdblDOWUsage(7) As Double 'vbSunday to vbSaturday

    Public Function ITrxGenerator_strLoad(ByVal domDoc As VB6XmlDocument, ByVal objAccount As Account) As String Implements ITrxGenerator.strLoad

        Dim strError As String
        Dim elmFirst As VB6XmlElement
        Dim elmRepeat As VB6XmlElement
        Dim elmDOWUsage As VB6XmlElement
        Dim elmScaling As VB6XmlElement
        Dim dblTotalDOWWeights As Double
        Dim intIndex As Short
        Dim vntAttrib As Object

        strError = gstrLoadTrxGeneratorCore(domDoc, mblnEnabled, mstrRepeatKey, mintStartRepeatSeq, mstrDescription, objAccount)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If

        'Load <dowusage> element.
        elmDOWUsage = domDoc.DocumentElement.SelectSingleNode("dowusage")
        If elmDOWUsage Is Nothing Then
            ITrxGenerator_strLoad = "Missing <dowusage> element"
            Exit Function
        End If
        dblTotalDOWWeights = 0
        mdblDOWUsage(FirstDayOfWeek.Sunday) = dblGetWeight(elmDOWUsage, "sunday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If
        mdblDOWUsage(FirstDayOfWeek.Monday) = dblGetWeight(elmDOWUsage, "monday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If
        mdblDOWUsage(FirstDayOfWeek.Tuesday) = dblGetWeight(elmDOWUsage, "tuesday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If
        mdblDOWUsage(FirstDayOfWeek.Wednesday) = dblGetWeight(elmDOWUsage, "wednesday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If
        mdblDOWUsage(FirstDayOfWeek.Thursday) = dblGetWeight(elmDOWUsage, "thursday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If
        mdblDOWUsage(FirstDayOfWeek.Friday) = dblGetWeight(elmDOWUsage, "friday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If
        mdblDOWUsage(FirstDayOfWeek.Saturday) = dblGetWeight(elmDOWUsage, "saturday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If
        If dblTotalDOWWeights <= 0 Then
            ITrxGenerator_strLoad = "DOW weights must add up to a positive number"
            Exit Function
        End If
        For intIndex = gintLBOUND1 To UBound(mdblDOWUsage)
            mdblDOWUsage(intIndex) = mdblDOWUsage(intIndex) / dblTotalDOWWeights
        Next

        'Load first period start date.
        elmFirst = domDoc.DocumentElement.SelectSingleNode("firstperiodstarts")
        If elmFirst Is Nothing Then
            ITrxGenerator_strLoad = "Missing <firstperiodstarts> element"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object elmFirst.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmFirst.GetAttribute("date")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            ITrxGenerator_strLoad = "Missing [date] attribute of <firstperiodstarts> element"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Not gblnValidDate(vntAttrib) Then
            ITrxGenerator_strLoad = "Invalid [date] attribute of <firstperiodstarts> element"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        mdatFirstPeriodStarts = CDate(vntAttrib)

        'Load default amount scaling percentage.
        elmScaling = domDoc.DocumentElement.SelectSingleNode("scaling")
        If elmScaling Is Nothing Then
            mdblDefaultPercentIncrease = 0.0#
        Else
            'UPGRADE_WARNING: Couldn't resolve default property of object elmScaling.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vntAttrib = elmScaling.GetAttribute("increasepercent")
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            If gblnXmlAttributeMissing(vntAttrib) Then
                ITrxGenerator_strLoad = "Missing [increasepercent] attribute of <scaling> element"
                Exit Function
            End If
            'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If gblnValidAmount(vntAttrib) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                mdblDefaultPercentIncrease = CDbl(vntAttrib) / 100.0#
            Else
                ITrxGenerator_strLoad = "Invalid [increasepercent] attribute of <scaling> element"
                Exit Function
            End If
        End If

        'Load period ending dates and amounts.
        mdatPeriodEndings = gdatLoadSequencedTrx(domDoc.DocumentElement, "periodends", mdblDefaultPercentIncrease, 0, strError)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If

        ITrxGenerator_strLoad = gstrGetTrxGenTemplate(domDoc, mstrRepeatKey, 0, mdatTrxTemplate)
    End Function

    Private Function dblGetWeight(ByVal elmDOWUsage As VB6XmlElement, ByVal strName As String, ByRef strError As String, ByRef dblTotalDOWUsage As Double) As Double

        Dim vntAttrib As Object
        Dim dblResult As Double

        'UPGRADE_WARNING: Couldn't resolve default property of object elmDOWUsage.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmDOWUsage.GetAttribute(strName)
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            strError = "Missing [" & strName & "] attribute of <dowusage> element"
            Exit Function
        End If
        If Not IsNumeric(vntAttrib) Then
            strError = "Invalid [" & strName & "] attribute of <dowusage> element"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        dblResult = CDbl(vntAttrib)
        If dblResult < 0 Then
            strError = "[" & strName & "] attribute of <dowusage> element may not be negative"
            Exit Function
        End If
        dblTotalDOWUsage = dblTotalDOWUsage + dblResult
        dblGetWeight = dblResult
    End Function

    Public ReadOnly Property ITrxGenerator_strDescription() As String Implements ITrxGenerator.strDescription
        Get
            ITrxGenerator_strDescription = mstrDescription
        End Get
    End Property

    Public ReadOnly Property ITrxGenerator_blnEnabled() As Boolean Implements ITrxGenerator.blnEnabled
        Get
            ITrxGenerator_blnEnabled = mblnEnabled
        End Get
    End Property

    Public ReadOnly Property ITrxGenerator_strRepeatKey() As String Implements ITrxGenerator.strRepeatKey
        Get
            ITrxGenerator_strRepeatKey = mdatTrxTemplate.strRepeatKey
        End Get
    End Property

    Public Function ITrxGenerator_colCreateTrx(ByVal objReg As Register, ByVal datRegisterEndDate As Date) As Collection Implements ITrxGenerator.colCreateTrx

        Dim datNewTrx() As SequencedTrx
        Dim intLongestOutputPeriod As Short
        Dim intIndex As Short
        Dim intStartIndex As Short
        Dim intStartDOW As Short
        Dim curWeekTotal As Decimal
        Dim curWeekRemainder As Decimal
        Dim curDayAmount As Decimal
        Dim intCurrentDOW As Short
        Dim intRemainderIndex As Short
        Dim intRepeatSeq As Short

        'Subdivide periods and totals using a distribution which approximates
        'a smooth curve, and repeat until all periods are one day in length.
        datNewTrx = VB6.CopyArray(mdatPeriodEndings)
        Do
            datNewTrx = gdatSplitSequencedTrx(mdatFirstPeriodStarts, datNewTrx, intLongestOutputPeriod)
            If intLongestOutputPeriod = 1 Then
                Exit Do
            End If
        Loop

        'Assign repeast seq values now that all the SequencedTrx have been generated
        intRepeatSeq = mintStartRepeatSeq
        For intIndex = 1 To UBound(datNewTrx)
            datNewTrx(intIndex).intRepeatSeq = intRepeatSeq
            intRepeatSeq = intRepeatSeq + 1
        Next

        'Redistribute amounts among days of the week.
        intStartIndex = 1
        intStartDOW = DatePart(Microsoft.VisualBasic.DateInterval.Weekday, datNewTrx(intStartIndex).datDate, FirstDayOfWeek.Sunday)
        Do
            'Is there a full week remaining?
            If (intStartIndex + 6) > UBound(datNewTrx) Then
                Exit Do
            End If
            'Compute curWeekTotal.
            curWeekTotal = 0
            For intIndex = intStartIndex To intStartIndex + 6
                curWeekTotal = curWeekTotal + datNewTrx(intIndex).curAmount
            Next
            'Distribute curWeekTotal among days of the week according to weights.
            curWeekRemainder = curWeekTotal
            intRemainderIndex = intStartIndex
            intCurrentDOW = intStartDOW
            For intIndex = intStartIndex To intStartIndex + 6
                curDayAmount = System.Math.Round(curWeekTotal * mdblDOWUsage(intCurrentDOW), 2)
                If intCurrentDOW = FirstDayOfWeek.Saturday Then
                    intCurrentDOW = FirstDayOfWeek.Sunday
                Else
                    intCurrentDOW = intCurrentDOW + 1
                End If
                datNewTrx(intIndex).curAmount = curDayAmount
                curWeekRemainder = curWeekRemainder - curDayAmount
                If curDayAmount <> 0 Then
                    intRemainderIndex = intIndex
                End If
            Next
            'Add any unused amount to the last trx with a non-zero amount.
            'There may be a leftover amount because of rounding.
            datNewTrx(intRemainderIndex).curAmount = datNewTrx(intRemainderIndex).curAmount + curWeekRemainder
            'Skip trx with zero amount.
            For intIndex = intStartIndex To intStartIndex + 6
                datNewTrx(intIndex).blnSkip = (datNewTrx(intIndex).curAmount = 0)
            Next
            'Go to next week.
            intStartIndex = intStartIndex + 7
        Loop

        'Only make Trx after the starting date and on or before the ending date.
        For intIndex = 1 To UBound(datNewTrx)
            If (datNewTrx(intIndex).datDate < mdatFirstPeriodStarts) Or (datNewTrx(intIndex).datDate > datRegisterEndDate) Then
                'Don't set to "false" in "else", because may have been
                'set to "true" earlier.
                datNewTrx(intIndex).blnSkip = True
            End If
        Next

        'Combine datNewTrx with mdatTrxTemplate to create TrxToCreate
        'array to return.
        ITrxGenerator_colCreateTrx = gcolTrxToCreateFromSeqTrx(datNewTrx, mdatTrxTemplate)

    End Function
End Class