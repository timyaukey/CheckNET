Option Strict On
Option Explicit On

Public Class TrxGenPeriod
    Inherits TrxGenBase
    Implements ITrxGenerator

    Private mstrDescription As String
    Private mblnEnabled As Boolean
    Private mdatFirstPeriodStarts As Date
    Private mdatPeriodEndings() As SequencedTrx
    Private mdblDefaultPercentIncrease As Double
    Private mdatTrxTemplate As TrxToCreate
    Private mstrRepeatKey As String
    Private mintStartRepeatSeq As Integer
    Private mdblDOWUsage(7) As Double 'vbSunday to vbSaturday

    Public Overrides Function Load(ByVal domDoc As CBXmlDocument, ByVal objAccount As Account) As String

        Dim strError As String
        Dim elmFirst As CBXmlElement
        Dim elmDOWUsage As CBXmlElement
        Dim elmScaling As CBXmlElement
        Dim dblTotalDOWWeights As Double
        Dim intIndex As Integer
        Dim vntAttrib As Object

        strError = LoadCore(domDoc)
        If strError <> "" Then
            Return strError
        End If

        strError = LoadTrxGeneratorCore(domDoc, mblnEnabled, mstrRepeatKey, mintStartRepeatSeq, mstrDescription, objAccount)
        If strError <> "" Then
            Return strError
        End If

        'Load <dowusage> element.
        elmDOWUsage = DirectCast(domDoc.DocumentElement.SelectSingleNode("dowusage"), CBXmlElement)
        If elmDOWUsage Is Nothing Then
            Return "Missing <dowusage> element"
        End If
        dblTotalDOWWeights = 0
        mdblDOWUsage(FirstDayOfWeek.Sunday) = dblGetWeight(elmDOWUsage, "sunday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            Return strError
        End If
        mdblDOWUsage(FirstDayOfWeek.Monday) = dblGetWeight(elmDOWUsage, "monday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            Return strError
        End If
        mdblDOWUsage(FirstDayOfWeek.Tuesday) = dblGetWeight(elmDOWUsage, "tuesday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            Return strError
        End If
        mdblDOWUsage(FirstDayOfWeek.Wednesday) = dblGetWeight(elmDOWUsage, "wednesday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            Return strError
        End If
        mdblDOWUsage(FirstDayOfWeek.Thursday) = dblGetWeight(elmDOWUsage, "thursday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            Return strError
        End If
        mdblDOWUsage(FirstDayOfWeek.Friday) = dblGetWeight(elmDOWUsage, "friday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            Return strError
        End If
        mdblDOWUsage(FirstDayOfWeek.Saturday) = dblGetWeight(elmDOWUsage, "saturday", strError, dblTotalDOWWeights)
        If strError <> "" Then
            Return strError
        End If
        If dblTotalDOWWeights <= 0 Then
            Return "DOW weights must add up to a positive number"
        End If
        For intIndex = Utilities.LowerBound1 To UBound(mdblDOWUsage)
            mdblDOWUsage(intIndex) = mdblDOWUsage(intIndex) / dblTotalDOWWeights
        Next

        'Load first period start date.
        elmFirst = DirectCast(domDoc.DocumentElement.SelectSingleNode("firstperiodstarts"), CBXmlElement)
        If elmFirst Is Nothing Then
            Return "Missing <firstperiodstarts> element"
        End If
        vntAttrib = elmFirst.GetAttribute("date")
        If XMLMisc.IsAttributeMissing(vntAttrib) Then
            Return "Missing [date] attribute of <firstperiodstarts> element"
        End If
        If Not Utilities.IsValidDate(CStr(vntAttrib)) Then
            Return "Invalid [date] attribute of <firstperiodstarts> element"
        End If
        mdatFirstPeriodStarts = CDate(vntAttrib)

        'Load default amount scaling percentage.
        elmScaling = DirectCast(domDoc.DocumentElement.SelectSingleNode("scaling"), CBXmlElement)
        If elmScaling Is Nothing Then
            mdblDefaultPercentIncrease = 0.0#
        Else
            vntAttrib = elmScaling.GetAttribute("increasepercent")
            If XMLMisc.IsAttributeMissing(vntAttrib) Then
                Return "Missing [increasepercent] attribute of <scaling> element"
            End If
            If Utilities.IsValidAmount(CStr(vntAttrib)) Then
                mdblDefaultPercentIncrease = CDbl(vntAttrib) / 100.0#
            Else
                Return "Invalid [increasepercent] attribute of <scaling> element"
            End If
        End If

        'Load period ending dates and amounts.
        mdatPeriodEndings = LoadTrxGenSequencedTrx(domDoc.DocumentElement, "periodends", mdblDefaultPercentIncrease, 0, strError)
        If strError <> "" Then
            Return strError
        End If

        Return GetTrxGenTemplate(objAccount.Company, domDoc, mstrRepeatKey, 0, mdatTrxTemplate)
    End Function

    Private Function dblGetWeight(ByVal elmDOWUsage As CBXmlElement, ByVal strName As String, ByRef strError As String, ByRef dblTotalDOWUsage As Double) As Double

        Dim vntAttrib As Object
        Dim dblResult As Double

        vntAttrib = elmDOWUsage.GetAttribute(strName)
        If XMLMisc.IsAttributeMissing(vntAttrib) Then
            strError = "Missing [" & strName & "] attribute of <dowusage> element"
            Exit Function
        End If
        If Not IsNumeric(vntAttrib) Then
            strError = "Invalid [" & strName & "] attribute of <dowusage> element"
            Exit Function
        End If
        dblResult = CDbl(vntAttrib)
        If dblResult < 0 Then
            strError = "[" & strName & "] attribute of <dowusage> element may not be negative"
            Exit Function
        End If
        dblTotalDOWUsage = dblTotalDOWUsage + dblResult
        dblGetWeight = dblResult
    End Function

    Public Overrides ReadOnly Property Description() As String
        Get
            Return mstrDescription
        End Get
    End Property

    Public Overrides ReadOnly Property IsEnabled() As Boolean
        Get
            Return mblnEnabled
        End Get
    End Property

    Public Overrides ReadOnly Property RepeatKey() As String
        Get
            Return mdatTrxTemplate.RepeatKey
        End Get
    End Property

    Public Overrides Function CreateTrx(ByVal objReg As Register, ByVal datRegisterEndDate As Date) As ICollection(Of TrxToCreate)

        Dim datNewTrx() As SequencedTrx
        Dim intLongestOutputPeriod As Integer
        Dim intIndex As Integer
        Dim intStartIndex As Integer
        Dim intStartDOW As Integer
        Dim curWeekTotal As Decimal
        Dim curWeekRemainder As Decimal
        Dim curDayAmount As Decimal
        Dim intCurrentDOW As Integer
        Dim intRemainderIndex As Integer
        Dim intRepeatSeq As Integer

        'Subdivide periods and totals using a distribution which approximates
        'a smooth curve, and repeat until all periods are one day in length.
        'datNewTrx = VB6.CopyArray(mdatPeriodEndings)
        datNewTrx = DirectCast(mdatPeriodEndings.Clone(), SequencedTrx())
        Do
            datNewTrx = SubdivideSequencedTrx(mdatFirstPeriodStarts, datNewTrx, intLongestOutputPeriod)
            If intLongestOutputPeriod = 1 Then
                Exit Do
            End If
        Loop

        'Assign repeast seq values now that all the SequencedTrx have been generated
        intRepeatSeq = mintStartRepeatSeq
        For intIndex = 1 To UBound(datNewTrx)
            datNewTrx(intIndex).RepeatSeq = intRepeatSeq
            intRepeatSeq = intRepeatSeq + 1
        Next

        'Redistribute amounts among days of the week.
        intStartIndex = 1
        intStartDOW = DatePart(Microsoft.VisualBasic.DateInterval.Weekday, datNewTrx(intStartIndex).TrxDate, FirstDayOfWeek.Sunday)
        Do
            'Is there a full week remaining?
            If (intStartIndex + 6) > UBound(datNewTrx) Then
                Exit Do
            End If
            'Compute curWeekTotal.
            curWeekTotal = 0
            For intIndex = intStartIndex To intStartIndex + 6
                curWeekTotal = curWeekTotal + datNewTrx(intIndex).Amount
            Next
            'Distribute curWeekTotal among days of the week according to weights.
            curWeekRemainder = curWeekTotal
            intRemainderIndex = intStartIndex
            intCurrentDOW = intStartDOW
            For intIndex = intStartIndex To intStartIndex + 6
                curDayAmount = CDec(System.Math.Round(curWeekTotal * mdblDOWUsage(intCurrentDOW), 2))
                If intCurrentDOW = FirstDayOfWeek.Saturday Then
                    intCurrentDOW = FirstDayOfWeek.Sunday
                Else
                    intCurrentDOW = intCurrentDOW + 1
                End If
                datNewTrx(intIndex).Amount = curDayAmount
                curWeekRemainder = curWeekRemainder - curDayAmount
                If curDayAmount <> 0 Then
                    intRemainderIndex = intIndex
                End If
            Next
            'Add any unused amount to the last trx with a non-zero amount.
            'There may be a leftover amount because of rounding.
            datNewTrx(intRemainderIndex).Amount = datNewTrx(intRemainderIndex).Amount + curWeekRemainder
            'Skip trx with zero amount.
            For intIndex = intStartIndex To intStartIndex + 6
                datNewTrx(intIndex).SkipSeqNum = (datNewTrx(intIndex).Amount = 0)
            Next
            'Go to next week.
            intStartIndex = intStartIndex + 7
        Loop

        'Only make BaseTrx after the starting date and on or before the ending date.
        For intIndex = 1 To UBound(datNewTrx)
            If (datNewTrx(intIndex).TrxDate < mdatFirstPeriodStarts) Or (datNewTrx(intIndex).TrxDate > datRegisterEndDate) Then
                'Don't set to "false" in "else", because may have been
                'set to "true" earlier.
                datNewTrx(intIndex).SkipSeqNum = True
            End If
        Next

        'Combine datNewTrx with mdatTrxTemplate to create TrxToCreate
        'array to return.
        Return ConvertSeqTrxToTrxToCreate(datNewTrx, mdatTrxTemplate)

    End Function
End Class