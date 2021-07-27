Option Strict On
Option Explicit On

Public Class TrxGenList
    Inherits TrxGenBase
    Implements ITrxGenerator

    Private mstrDescription As String
    Private mblnEnabled As Boolean
    Private mstrRepeatKey As String
    Private mintStartRepeatSeq As Integer
    Private maudtTrx() As TrxToCreate

    Public Overrides Function Load(ByVal domDoc As CBXmlDocument, ByVal objAccount As Account) As String

        Dim strError As String
        Dim nodeTrx As CBXmlNode
        Dim elmTrx As CBXmlElement
        Dim udtTrx As TrxToCreate = New TrxToCreate()
        Dim datDate As Date
        Dim curAmount As Decimal
        Dim intCount As Integer
        Dim strMsg As String
        Dim blnAddTrx As Boolean
        Dim intNextRepeatSeq As Integer

        strError = LoadCore(domDoc)
        If strError <> "" Then
            Return strError
        End If

        strError = LoadTrxGeneratorCore(domDoc, mblnEnabled, mstrRepeatKey, mintStartRepeatSeq, mstrDescription, objAccount)
        If strError <> "" Then
            Return strError
        End If

        intCount = 0
        intNextRepeatSeq = mintStartRepeatSeq
        ReDim maudtTrx(1)
        For Each nodeTrx In domDoc.DocumentElement.ChildNodes
            If TypeOf nodeTrx Is CBXmlElement Then
                elmTrx = DirectCast(nodeTrx, CBXmlElement)
                blnAddTrx = False
                'To allow budget and xfer trx all that should be necessary
                'is to clone the "If" statement below for the appropriate element
                'name and call the appropriate template reader for it.
                If elmTrx.Name = "normaltrx" Then
                    strMsg = strGetCommonFields(elmTrx, datDate, curAmount)
                    If strMsg <> "" Then
                        Return strMsg
                    End If
                    strMsg = GetTrxGenTemplateBank(objAccount.Company, elmTrx, mstrRepeatKey, curAmount, udtTrx)
                    If strMsg <> "" Then
                        Return strMsg
                    End If
                    blnAddTrx = True
                End If
                If blnAddTrx Then
                    udtTrx.TrxDate = datDate
                    udtTrx.Amount = curAmount
                    udtTrx.RepeatSeq = intNextRepeatSeq
                    intNextRepeatSeq = intNextRepeatSeq + 1
                    intCount = intCount + 1
                    ReDim Preserve maudtTrx(intCount)
                    maudtTrx(intCount) = udtTrx
                End If
            End If
        Next nodeTrx

        Return ""
    End Function

    Private Function strGetCommonFields(ByVal elmTrx As CBXmlElement, ByRef datDate As Date, ByRef curAmount As Decimal) As String

        Dim vntAttrib As Object

        strGetCommonFields = ""
        vntAttrib = elmTrx.GetAttribute("date")
        If gblnXmlAttributeMissing(vntAttrib) Then
            strGetCommonFields = "Missing [date] attribute"
            Exit Function
        End If
        If Not Utilities.blnIsValidDate(CStr(vntAttrib)) Then
            strGetCommonFields = "Invalid [date] attribute"
            Exit Function
        End If
        datDate = CDate(vntAttrib)

        vntAttrib = elmTrx.GetAttribute("amount")
        If gblnXmlAttributeMissing(vntAttrib) Then
            strGetCommonFields = "Missing [amount] attribute"
            Exit Function
        End If
        If Not Utilities.blnIsValidAmount(CStr(vntAttrib)) Then
            strGetCommonFields = "Invalid [amount] attribute"
            Exit Function
        End If
        curAmount = CDec(vntAttrib)
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
            Return mstrRepeatKey
        End Get
    End Property

    Public Overrides Function CreateTrx(ByVal objReg As Register, ByVal datRegisterEndDate As Date) As ICollection(Of TrxToCreate)

        Dim colResults As ICollection(Of TrxToCreate)
        Dim lngIndex As Integer
        Dim lngCount As Integer

        colResults = New List(Of TrxToCreate)
        lngCount = UBound(maudtTrx)
        For lngIndex = Utilities.intLBOUND1 To lngCount
            If maudtTrx(lngIndex).TrxDate <= datRegisterEndDate Then
                colResults.Add(maudtTrx(lngIndex))
            End If
        Next
        Return colResults

    End Function
End Class