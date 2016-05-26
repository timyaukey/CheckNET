Option Strict Off
Option Explicit On
Public Class TrxGenList
    Implements ITrxGenerator
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345


    Private mstrDescription As String
    Private mblnEnabled As Boolean
    Private mstrRepeatKey As String
    Private mintStartRepeatSeq As Short
    Private maudtTrx() As TrxToCreate

    Public Function ITrxGenerator_strLoad(ByVal domDoc As VB6XmlDocument, ByVal objAccount As Account) As String Implements ITrxGenerator.strLoad

        Dim strError As String
        Dim nodeTrx As VB6XmlNode
        Dim elmTrx As VB6XmlElement
        'UPGRADE_WARNING: Arrays in structure udtTrx may need to be initialized before they can be used. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="814DF224-76BD-4BB4-BFFB-EA359CB9FC48"'
        Dim udtTrx As TrxToCreate = New TrxToCreate()
        Dim datDate As Date
        Dim curAmount As Decimal
        Dim intCount As Short
        Dim strMsg As String
        Dim blnAddTrx As Boolean
        Dim intNextRepeatSeq As Short

        strError = gstrLoadTrxGeneratorCore(domDoc, mblnEnabled, mstrRepeatKey, mintStartRepeatSeq, mstrDescription, objAccount)
        If strError <> "" Then
            ITrxGenerator_strLoad = strError
            Exit Function
        End If

        intCount = 0
        intNextRepeatSeq = mintStartRepeatSeq
        'UPGRADE_WARNING: Lower bound of array maudtTrx was changed from gintLBOUND1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
        ReDim maudtTrx(1)
        For Each nodeTrx In domDoc.DocumentElement.ChildNodes
            'UPGRADE_WARNING: TypeOf has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If TypeOf nodeTrx Is VB6XmlElement Then
                elmTrx = nodeTrx
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
                    'UPGRADE_WARNING: Lower bound of array maudtTrx was changed from gintLBOUND1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
                    ReDim Preserve maudtTrx(intCount)
                    'UPGRADE_WARNING: Couldn't resolve default property of object maudtTrx(intCount). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                    maudtTrx(intCount) = udtTrx
                End If
            End If
        Next nodeTrx

        ITrxGenerator_strLoad = ""
    End Function

    Private Function strGetCommonFields(ByVal elmTrx As VB6XmlElement, ByRef datDate As Date, ByRef curAmount As Decimal) As String

        Dim vntAttrib As Object

        strGetCommonFields = ""
        'UPGRADE_WARNING: Couldn't resolve default property of object elmTrx.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmTrx.GetAttribute("date")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            strGetCommonFields = "Missing [date] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Not gblnValidDate(vntAttrib) Then
            strGetCommonFields = "Invalid [date] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        datDate = CDate(vntAttrib)

        'UPGRADE_WARNING: Couldn't resolve default property of object elmTrx.getAttribute(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        vntAttrib = elmTrx.GetAttribute("amount")
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
        If gblnXmlAttributeMissing(vntAttrib) Then
            strGetCommonFields = "Missing [amount] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        If Not gblnValidAmount(vntAttrib) Then
            strGetCommonFields = "Invalid [amount] attribute"
            Exit Function
        End If
        'UPGRADE_WARNING: Couldn't resolve default property of object vntAttrib. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
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

    Public Function ITrxGenerator_colCreateTrx(ByVal objReg As Register, ByVal datRegisterEndDate As Date) As Collection Implements ITrxGenerator.colCreateTrx

        Dim colResults As Collection
        Dim lngIndex As Integer
        Dim lngCount As Integer

        colResults = New Collection
        lngCount = UBound(maudtTrx)
        For lngIndex = gintLBOUND1 To lngCount
            If maudtTrx(lngIndex).datDate <= datRegisterEndDate Then
                colResults.Add(maudtTrx(lngIndex))
            End If
        Next
        ITrxGenerator_colCreateTrx = colResults

    End Function
End Class