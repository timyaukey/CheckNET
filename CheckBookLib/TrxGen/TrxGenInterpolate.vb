Option Strict Off
Option Explicit On
Public Class TrxGenInterpolate
    Implements _ITrxGenerator
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345


    Private mstrDescription As String
    Private mblnEnabled As Boolean
    Private mdatSequence As ITrxGenerator.DateSequenceParams
    Private mdatSamples() As SequencedTrx
    'UPGRADE_WARNING: Arrays in structure mdatTrxTemplate may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
    Private mdatTrxTemplate As ITrxGenerator.TrxToCreate
    Private mstrRepeatKey As String
    Private mintStartRepeatSeq As Short

    Public Function ITrxGenerator_strLoad(ByVal domDoc As VB6XmlDocument, ByVal objAccount As Account) As String Implements _ITrxGenerator.strLoad

        Dim strError As String
        Dim elmRepeat As VB6XmlElement

        strError = gstrLoadTrxGeneratorCore(domDoc, mblnEnabled, mstrRepeatKey, mintStartRepeatSeq, mstrDescription, objAccount)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If

        strError = gstrGetDateSequenceParams(domDoc.DocumentElement, "schedule", elmRepeat, mdatSequence)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If

        mdatSamples = gdatLoadSequencedTrx(domDoc.DocumentElement, "sample", 0, 0, strError)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If

        ITrxGenerator_strLoad = gstrGetTrxGenTemplate(domDoc, mstrRepeatKey, 0, mdatTrxTemplate)
    End Function

    Public ReadOnly Property ITrxGenerator_strDescription() As String Implements _ITrxGenerator.strDescription
        Get
            ITrxGenerator_strDescription = mstrDescription
        End Get
    End Property

    Public ReadOnly Property ITrxGenerator_blnEnabled() As Boolean Implements _ITrxGenerator.blnEnabled
        Get
            ITrxGenerator_blnEnabled = mblnEnabled
        End Get
    End Property

    Public ReadOnly Property ITrxGenerator_strRepeatKey() As String Implements _ITrxGenerator.strRepeatKey
        Get
            ITrxGenerator_strRepeatKey = mdatTrxTemplate.strRepeatKey
        End Get
    End Property

    Public Function ITrxGenerator_colCreateTrx(ByVal objReg As Register, ByVal datRegisterEndDate As Date) As Collection Implements _ITrxGenerator.colCreateTrx

        Dim datNewTrx() As SequencedTrx

        'Get SequencedTrx to create Trx for.
        'Their .curAmount values will be initialized to zero.
        datNewTrx = gdatGenerateSeqTrxForDates(mdatSequence.datNominalStartDate, mdatSequence.vntNominalEndDate, datRegisterEndDate, mdatSequence.lngRptUnit, mdatSequence.intRptNumber, 0, mintStartRepeatSeq)

        'Set the .curAmount values for datNewTrx by interpolating from
        'date/amount pairs in mdatSamples, and skip datNewTrx elements
        'outside the sample date range.
        gInterpolateAmountsFromSamples(datNewTrx, mdatSamples)

        'Combine datNewTrx with mdatTrxTemplate to create TrxToCreate
        'array to return.
        ITrxGenerator_colCreateTrx = gcolTrxToCreateFromSeqTrx(datNewTrx, mdatTrxTemplate)

    End Function
End Class