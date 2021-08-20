Option Strict On
Option Explicit On

Public Class TrxGenRepeat
    Inherits TrxGenBase
    Implements ITrxGenerator

    Private mstrDescription As String
    Private mblnEnabled As Boolean
    Private mcurAmount As Decimal
    Private mdatSequence As DateSequenceParams
    Private mdatTrxTemplate As TrxToCreate
    Private mstrRepeatKey As String
    Private mintStartRepeatSeq As Integer

    Public Overrides Function Load(ByVal domDoc As CBXmlDocument, ByVal objAccount As Account) As String

        Dim strError As String
        Dim elmRepeat As CBXmlElement = Nothing
        Dim vntAttrib As Object

        strError = LoadCore(domDoc)
        If strError <> "" Then
            Return strError
        End If

        strError = LoadTrxGeneratorCore(domDoc, mblnEnabled, mstrRepeatKey, mintStartRepeatSeq, mstrDescription, objAccount)
        If strError <> "" Then
            Return strError
        End If

        strError = GetTrxGenDateSequenceParams(domDoc.DocumentElement, "repeat", elmRepeat, mdatSequence)
        If strError <> "" Then
            Return strError
        End If

        vntAttrib = elmRepeat.GetAttribute("amount")
        If XMLMisc.IsAttributeMissing(vntAttrib) Then
            Return "Missing [amount] attribute"
        End If
        If Not Utilities.IsValidAmount(CStr(vntAttrib)) Then
            Return "Invalid [amount] attribute"
        End If
        mcurAmount = CDec(vntAttrib)

        Return GetTrxGenTemplate(objAccount.Company, domDoc, mstrRepeatKey, mcurAmount, mdatTrxTemplate)
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

        Dim datSeqTrx() As SequencedTrx

        'Get SequencedTrx to create BaseTrx for.
        datSeqTrx = GenerateSeqTrxForDates(mdatSequence.NominalStartDate, mdatSequence.NominalEndDate, datRegisterEndDate, mdatSequence.RptUnit, mdatSequence.RptNumber, mcurAmount, mintStartRepeatSeq)

        'Combine datNewTrx with mdatTrxTemplate to create TrxToCreate
        'array to return.
        Return ConvertSeqTrxToTrxToCreate(datSeqTrx, mdatTrxTemplate)

    End Function
End Class