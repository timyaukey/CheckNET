Option Strict Off
Option Explicit On
Public Class TrxGenRepeat
    Implements ITrxGenerator
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345


    Private mstrDescription As String
    Private mblnEnabled As Boolean
    Private mcurAmount As Decimal
    Private mdatSequence As DateSequenceParams
    'UPGRADE_WARNING: Arrays in structure mdatTrxTemplate may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
    Private mdatTrxTemplate As TrxToCreate
    Private mstrRepeatKey As String
    Private mintStartRepeatSeq As Short

    Public Function ITrxGenerator_strLoad(ByVal domDoc As VB6XmlDocument, ByVal objAccount As Account) As String Implements ITrxGenerator.strLoad

        Dim strError As String
        Dim elmRepeat As VB6XmlElement = Nothing
        Dim vntAttrib As Object

        strError = gstrLoadTrxGeneratorCore(domDoc, mblnEnabled, mstrRepeatKey, mintStartRepeatSeq, mstrDescription, objAccount)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If

        strError = gstrGetDateSequenceParams(domDoc.DocumentElement, "repeat", elmRepeat, mdatSequence)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object elmRepeat.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmRepeat.GetAttribute("amount")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            ITrxGenerator_strLoad = "Missing [amount] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Not gblnValidAmount(vntAttrib) Then
            ITrxGenerator_strLoad = "Invalid [amount] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        mcurAmount = CDec(vntAttrib)

        ITrxGenerator_strLoad = gstrGetTrxGenTemplate(domDoc, mstrRepeatKey, mcurAmount, mdatTrxTemplate)
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

    Public Function ITrxGenerator_colCreateTrx(ByVal objReg As Register, ByVal datRegisterEndDate As Date) As ICollection(Of TrxToCreate) Implements ITrxGenerator.colCreateTrx

        Dim datSeqTrx() As SequencedTrx

        'Get SequencedTrx to create Trx for.
        datSeqTrx = gdatGenerateSeqTrxForDates(mdatSequence.datNominalStartDate, mdatSequence.vntNominalEndDate, datRegisterEndDate, mdatSequence.lngRptUnit, mdatSequence.intRptNumber, mcurAmount, mintStartRepeatSeq)

        'Combine datNewTrx with mdatTrxTemplate to create TrxToCreate
        'array to return.
        ITrxGenerator_colCreateTrx = gcolTrxToCreateFromSeqTrx(datSeqTrx, mdatTrxTemplate)

    End Function
End Class