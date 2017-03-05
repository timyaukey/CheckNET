Option Strict On
Option Explicit On

Public Class TrxGenList
    Implements ITrxGenerator
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345


    Private mstrDescription As String
    Private mblnEnabled As Boolean
    Private mstrRepeatKey As String
    Private mintStartRepeatSeq As Integer
    Private maudtTrx() As TrxToCreate

    Public Function ITrxGenerator_strLoad(ByVal domDoc As VB6XmlDocument, ByVal objAccount As Account) As String Implements ITrxGenerator.strLoad

        Dim strError As String
        Dim nodeTrx As VB6XmlNode
        Dim elmTrx As VB6XmlElement
        Dim udtTrx As TrxToCreate = New TrxToCreate()
        Dim datDate As Date
        Dim curAmount As Decimal
        Dim intCount As Integer
        Dim strMsg As String
        Dim blnAddTrx As Boolean
        Dim intNextRepeatSeq As Integer

        strError = gstrLoadTrxGeneratorCore(domDoc, mblnEnabled, mstrRepeatKey, mintStartRepeatSeq, mstrDescription, objAccount)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If

        intCount = 0
        intNextRepeatSeq = mintStartRepeatSeq
        ReDim maudtTrx(1)
        For Each nodeTrx In domDoc.DocumentElement.ChildNodes
            If TypeOf nodeTrx Is VB6XmlElement Then
                elmTrx = DirectCast(nodeTrx, VB6XmlElement)
                blnAddTrx = False
                'To allow budget and xfer trx all that should be necessary
                'is to clone the "If" statement below for the appropriate element
                'name and call the appropriate template reader for it.
                If elmTrx.Name = "normaltrx" Then
                    strMsg = strGetCommonFields(elmTrx, datDate, curAmount)
                    If strMsg <> "" Then
                        ITrxGenerator_strLoad = strMsg
                        Exit Function
                    End If
                    strMsg = gstrGetTrxGenTemplateNormal(elmTrx, mstrRepeatKey, curAmount, udtTrx)
                    If strMsg <> "" Then
                        ITrxGenerator_strLoad = strMsg
                        Exit Function
                    End If
                    blnAddTrx = True
                End If
                If blnAddTrx Then
                    udtTrx.datDate = datDate
                    udtTrx.curAmount = curAmount
                    udtTrx.intRepeatSeq = intNextRepeatSeq
                    intNextRepeatSeq = intNextRepeatSeq + 1
                    intCount = intCount + 1
                    ReDim Preserve maudtTrx(intCount)
                    maudtTrx(intCount) = udtTrx
                End If
            End If
        Next nodeTrx

        ITrxGenerator_strLoad = ""
    End Function

    Private Function strGetCommonFields(ByVal elmTrx As VB6XmlElement, ByRef datDate As Date, ByRef curAmount As Decimal) As String

        Dim vntAttrib As Object

        strGetCommonFields = ""
        vntAttrib = elmTrx.GetAttribute("date")
        If gblnXmlAttributeMissing(vntAttrib) Then
            strGetCommonFields = "Missing [date] attribute"
            Exit Function
        End If
        If Not gblnValidDate(CStr(vntAttrib)) Then
            strGetCommonFields = "Invalid [date] attribute"
            Exit Function
        End If
        datDate = CDate(vntAttrib)

        vntAttrib = elmTrx.GetAttribute("amount")
        If gblnXmlAttributeMissing(vntAttrib) Then
            strGetCommonFields = "Missing [amount] attribute"
            Exit Function
        End If
        If Not gblnValidAmount(CStr(vntAttrib)) Then
            strGetCommonFields = "Invalid [amount] attribute"
            Exit Function
        End If
        curAmount = CDec(vntAttrib)
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
            ITrxGenerator_strRepeatKey = mstrRepeatKey
        End Get
    End Property

    Public Function ITrxGenerator_colCreateTrx(ByVal objReg As Register, ByVal datRegisterEndDate As Date) As ICollection(Of TrxToCreate) Implements ITrxGenerator.colCreateTrx

        Dim colResults As ICollection(Of TrxToCreate)
        Dim lngIndex As Integer
        Dim lngCount As Integer

        colResults = New List(Of TrxToCreate)
        lngCount = UBound(maudtTrx)
        For lngIndex = gintLBOUND1 To lngCount
            If maudtTrx(lngIndex).datDate <= datRegisterEndDate Then
                colResults.Add(maudtTrx(lngIndex))
            End If
        Next
        ITrxGenerator_colCreateTrx = colResults

    End Function
End Class