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

    Public Overrides Function Load(ByVal domDoc As CBXmlDocument, ByVal objAccount As Account) As String

        Dim strError As String
        Dim elmRepeat As CBXmlElement = Nothing

        strError = LoadCore(domDoc)
        If strError <> "" Then
            Return strError
        End If

        strError = LoadTrxGeneratorCore(domDoc, mblnEnabled, mstrRepeatKey, mintStartRepeatSeq, mstrDescription, objAccount)
        If strError <> "" Then
            Return strError
        End If

        strError = GetTrxGenDateSequenceParams(domDoc.DocumentElement, "schedule", elmRepeat, mdatSequence)
        If strError <> "" Then
            Return strError
        End If

        mdatSamples = LoadTrxGenSequencedTrx(domDoc.DocumentElement, "sample", 0, 0, strError)
        If strError <> "" Then
            Return strError
        End If

        Return GetTrxGenTemplate(objAccount.Company, domDoc, mstrRepeatKey, 0, mdatTrxTemplate)
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

        'Get SequencedTrx to create BaseTrx for.
        'Their .curAmount values will be initialized to zero.
        datNewTrx = GenerateSeqTrxForDates(mdatSequence.NominalStartDate, mdatSequence.NominalEndDate, datRegisterEndDate, mdatSequence.RptUnit, mdatSequence.RptNumber, 0, mintStartRepeatSeq)

        'Set the .curAmount values for datNewTrx by interpolating from
        'date/amount pairs in mdatSamples, and skip datNewTrx elements
        'outside the sample date range.
        InterpolateGeneratedTrxAmountsFromSamples(datNewTrx, mdatSamples)

        'Combine datNewTrx with mdatTrxTemplate to create TrxToCreate
        'array to return.
        Return ConvertSeqTrxToTrxToCreate(datNewTrx, mdatTrxTemplate)

    End Function
End Class