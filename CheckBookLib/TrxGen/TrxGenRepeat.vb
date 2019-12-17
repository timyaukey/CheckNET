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

    Public Overrides Function strLoad(ByVal domDoc As VB6XmlDocument, ByVal objAccount As Account) As String

        Dim strError As String
        Dim elmRepeat As VB6XmlElement = Nothing
        Dim vntAttrib As Object

        strError = strLoadCore(domDoc)
        If strError <> "" Then
            Return strError
        End If

        strError = gstrLoadTrxGeneratorCore(domDoc, mblnEnabled, mstrRepeatKey, mintStartRepeatSeq, mstrDescription, objAccount)
        If strError <> "" Then
            Return strError
        End If

        strError = gstrGetDateSequenceParams(domDoc.DocumentElement, "repeat", elmRepeat, mdatSequence)
        If strError <> "" Then
            Return strError
        End If

        vntAttrib = elmRepeat.GetAttribute("amount")
        If gblnXmlAttributeMissing(vntAttrib) Then
            Return "Missing [amount] attribute"
        End If
        If Not Utilities.blnIsValidAmount(CStr(vntAttrib)) Then
            Return "Invalid [amount] attribute"
        End If
        mcurAmount = CDec(vntAttrib)

        Return gstrGetTrxGenTemplate(objAccount.objCompany, domDoc, mstrRepeatKey, mcurAmount, mdatTrxTemplate)
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

        Dim datSeqTrx() As SequencedTrx

        'Get SequencedTrx to create Trx for.
        datSeqTrx = gdatGenerateSeqTrxForDates(mdatSequence.datNominalStartDate, mdatSequence.vntNominalEndDate, datRegisterEndDate, mdatSequence.lngRptUnit, mdatSequence.intRptNumber, mcurAmount, mintStartRepeatSeq)

        'Combine datNewTrx with mdatTrxTemplate to create TrxToCreate
        'array to return.
        Return gcolTrxToCreateFromSeqTrx(datSeqTrx, mdatTrxTemplate)

    End Function
End Class