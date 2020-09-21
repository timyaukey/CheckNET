Option Strict On
Option Explicit On

Public MustInherit Class TrxNameSummary
    Public strName As String
    Public curBalance As Decimal

    Public objCurrentCharges As DateRangeSummary
    Public obj1To30Charges As DateRangeSummary
    Public obj31To60Charges As DateRangeSummary
    Public obj61To90Charges As DateRangeSummary
    Public objOver90Charges As DateRangeSummary
    Public objFutureCharges As DateRangeSummary
    Public objPayments As DateRangeSummary

    Public Sub New()
        curBalance = 0
        objCurrentCharges = New DateRangeSummary()
        obj1To30Charges = New DateRangeSummary()
        obj31To60Charges = New DateRangeSummary()
        obj61To90Charges = New DateRangeSummary()
        objOver90Charges = New DateRangeSummary()
        objFutureCharges = New DateRangeSummary()
        objPayments = New DateRangeSummary()
    End Sub

    Public Shared Function colScanTrx(Of TSummary As {TrxNameSummary, New})(ByVal objCompany As Company, ByVal datEnd As DateTime,
                                          ByVal datAging As DateTime, ByVal lngSubType As Account.SubType) As List(Of TSummary)
        Dim colSummary As List(Of TSummary) = New List(Of TSummary)()
        Dim objDict As Dictionary(Of String, TSummary) = New Dictionary(Of String, TSummary)()

        For Each objAccount As Account In objCompany.colAccounts
            If objAccount.lngSubType = lngSubType Then
                For Each objReg As Register In objAccount.colRegisters
                    For Each objTrx As Trx In objReg.colDateRange(Of Trx)(New DateTime(1900, 1, 1), datEnd)
                        If Not objTrx.blnFake Then
                            Dim objNormalTrx As NormalTrx = TryCast(objTrx, NormalTrx)
                            If Not objNormalTrx Is Nothing Then
                                Dim objSummary As TSummary = objGetSummary(colSummary, objDict, objTrx)
                                For Each objSplit As TrxSplit In objNormalTrx.colSplits
                                    objSummary.AddAmountToCorrectAge(objSplit.curAmount, objSplit.datDueDateEffective, datAging)
                                Next
                            Else
                                Dim objReplicaTrx As ReplicaTrx = TryCast(objTrx, ReplicaTrx)
                                If Not objReplicaTrx Is Nothing Then
                                    Dim objSummary As TSummary = objGetSummary(colSummary, objDict, objTrx)
                                    objSummary.AddAmountToCorrectAge(objReplicaTrx.curAmount, objReplicaTrx.datDate, datAging)
                                End If
                            End If
                        End If
                    Next
                Next
            End If
        Next
        For Each objSummary As TSummary In colSummary
            objSummary.ApplyPayments()
        Next
        colSummary.Sort(AddressOf intNameComparer)

        Return colSummary
    End Function

    Private Shared Function objGetSummary(Of TSummary As {TrxNameSummary, New})(ByVal colSummary As List(Of TSummary),
                                         ByVal objDict As Dictionary(Of String, TSummary),
                                         ByVal objTrx As Trx) As TSummary
        Dim objSummary As TSummary = Nothing
        Dim strNameKey As String = objTrx.strDescription.ToUpper()
        If strNameKey.Length > 16 Then
            strNameKey = strNameKey.Substring(0, 16)
        End If
        If Not objDict.TryGetValue(strNameKey, objSummary) Then
            objSummary = New TSummary()
            objSummary.strName = objTrx.strDescription
            objSummary.curBalance = 0D
            colSummary.Add(objSummary)
            objDict.Add(strNameKey, objSummary)
        End If
        Return objSummary
    End Function

    Private Shared Function intNameComparer(Of TSummary As TrxNameSummary)(ByVal objSummary1 As TSummary, ByVal objSummary2 As TSummary) As Integer
        Return objSummary1.strName.CompareTo(objSummary2.strName)
    End Function

    Protected MustOverride Function blnIsCharge(ByVal curAmount As Decimal) As Boolean

    Protected Sub AddAmountToCorrectAge(ByVal curAmount As Decimal, ByVal datDue As Date, ByVal datAging As Date)
        Me.curBalance += curAmount
        If blnIsCharge(curAmount) Then
            Dim intAge As Integer = CInt(datAging.Subtract(datDue).TotalDays)
            If intAge <= -30 Then
                objFutureCharges.Add(curAmount)
            ElseIf intAge <= 0 Then
                objCurrentCharges.Add(curAmount)
            ElseIf intAge <= 30 Then
                obj1To30Charges.Add(curAmount)
            ElseIf intAge <= 60 Then
                obj31To60Charges.Add(curAmount)
            ElseIf intAge <= 90 Then
                obj61To90Charges.Add(curAmount)
            Else
                objOver90Charges.Add(curAmount)
            End If
        Else
            objPayments.Add(curAmount)
        End If
    End Sub

    Private Sub ApplyPayments()
        If blnApplyPayments(objOver90Charges, objPayments) Then
            If blnApplyPayments(obj61To90Charges, objPayments) Then
                If blnApplyPayments(obj31To60Charges, objPayments) Then
                    If blnApplyPayments(obj1To30Charges, objPayments) Then
                        If blnApplyPayments(objCurrentCharges, objPayments) Then
                            blnApplyPayments(objFutureCharges, objPayments)
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Function blnApplyPayments(ByVal objCharges As DateRangeSummary, ByVal objPayments As DateRangeSummary) As Boolean
        Dim curNetBalance As Decimal = objCharges.curDateTotal + objPayments.curDateTotal
        If blnIsCharge(curNetBalance) Or curNetBalance = 0 Then
            objCharges.curDateTotal = curNetBalance
            objPayments.curDateTotal = 0
            Return False
        Else
            objPayments.curDateTotal = curNetBalance
            objCharges.curDateTotal = 0
            Return True
        End If
    End Function

End Class
