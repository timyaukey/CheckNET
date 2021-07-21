Option Strict On
Option Explicit On

Public Class TrxGenInterpolate
    Inherits TrxGenBase
    Implements ITrxGenerator

    Private mstrDescription As String
    Private mblnEnabled As Boolean
    Private mdatSequence As DateSequenceParams
    Private mdatSamples() As SequencedTrx
    Private mdatTrxTemplate As TrxToCreate
    Private mstrRepeatKey As String
    Private mintStartRepeatSeq As Integer

    Public Overrides Function strLoad(ByVal domDoc As VB6XmlDocument, ByVal objAccount As Account) As String

        Dim strError As String
        Dim elmRepeat As VB6XmlElement = Nothing

        strError = strLoadCore(domDoc)
        If strError <> "" Then
            Return strError
        End If

        strError = gstrLoadTrxGeneratorCore(domDoc, mblnEnabled, mstrRepeatKey, mintStartRepeatSeq, mstrDescription, objAccount)
        If strError <> "" Then
            Return strError
        End If

        strError = gstrGetDateSequenceParams(domDoc.DocumentElement, "schedule", elmRepeat, mdatSequence)
        If strError <> "" Then
            Return strError
        End If

        mdatSamples = gdatLoadSequencedTrx(domDoc.DocumentElement, "sample", 0, 0, strError)
        If strError <> "" Then
            Return strError
        End If

        Return gstrGetTrxGenTemplate(objAccount.objCompany, domDoc, mstrRepeatKey, 0, mdatTrxTemplate)
    End Function

    Public Overrides ReadOnly Property strDescription() As String
        Get
            Return mstrDescription
        End Get
    End Property

    Public Overrides ReadOnly Property blnEnabled() As Boolean
        Get
            Return mblnEnabled
        End Get
    End Property

    Public Overrides ReadOnly Property strRepeatKey() As String
        Get
            Return mdatTrxTemplate.strRepeatKey
        End Get
    End Property

    Public Overrides Function colCreateTrx(ByVal objReg As Register, ByVal datRegisterEndDate As Date) As ICollection(Of TrxToCreate)

        Dim datNewTrx() As SequencedTrx

        'Get SequencedTrx to create BaseTrx for.
        'Their .curAmount values will be initialized to zero.
        datNewTrx = gdatGenerateSeqTrxForDates(mdatSequence.datNominalStartDate, mdatSequence.vntNominalEndDate, datRegisterEndDate, mdatSequence.lngRptUnit, mdatSequence.intRptNumber, 0, mintStartRepeatSeq)

        'Set the .curAmount values for datNewTrx by interpolating from
        'date/amount pairs in mdatSamples, and skip datNewTrx elements
        'outside the sample date range.
        gInterpolateAmountsFromSamples(datNewTrx, mdatSamples)

        'Combine datNewTrx with mdatTrxTemplate to create TrxToCreate
        'array to return.
        Return gcolTrxToCreateFromSeqTrx(datNewTrx, mdatTrxTemplate)

    End Function
End Class