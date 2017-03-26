Option Strict On
Option Explicit On

Public Class TrxGenRepeat
    Implements ITrxGenerator
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345


    Private mstrDescription As String
    Private mblnEnabled As Boolean
    Private mcurAmount As Decimal
    Private mdatSequence As DateSequenceParams
    Private mdatTrxTemplate As TrxToCreate
    Private mstrRepeatKey As String
    Private mintStartRepeatSeq As Integer

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

        vntAttrib = elmRepeat.GetAttribute("amount")
        If gblnXmlAttributeMissing(vntAttrib) Then
            ITrxGenerator_strLoad = "Missing [amount] attribute"
            Exit Function
        End If
        If Not gblnValidAmount(CStr(vntAttrib)) Then
            ITrxGenerator_strLoad = "Invalid [amount] attribute"
            Exit Function
        End If
        mcurAmount = CDec(vntAttrib)

        ITrxGenerator_strLoad = gstrGetTrxGenTemplate(objAccount.objCompany, domDoc, mstrRepeatKey, mcurAmount, mdatTrxTemplate)
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