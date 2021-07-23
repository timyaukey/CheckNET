Option Strict On
Option Explicit On

Public MustInherit Class TrxNameSummary
    Public TrxName As String
    Public Balance As Decimal

    Public SummaryCurrentCharges As DateRangeSummary
    Public Summary1To30Charges As DateRangeSummary
    Public Summary31To60Charges As DateRangeSummary
    Public Summary61To90Charges As DateRangeSummary
    Public SummaryOver90Charges As DateRangeSummary
    Public SummaryFutureCharges As DateRangeSummary
    Public SummaryPayments As DateRangeSummary

    Public Sub New()
        Balance = 0
        SummaryCurrentCharges = New DateRangeSummary()
        Summary1To30Charges = New DateRangeSummary()
        Summary31To60Charges = New DateRangeSummary()
        Summary61To90Charges = New DateRangeSummary()
        SummaryOver90Charges = New DateRangeSummary()
        SummaryFutureCharges = New DateRangeSummary()
        SummaryPayments = New DateRangeSummary()
    End Sub

    Public Shared Function ScanTrx(Of TSummary As {TrxNameSummary, New})(ByVal objCompany As Company, ByVal datEnd As DateTime,
                                          ByVal datAging As DateTime, ByVal lngSubType As Account.SubType) As List(Of TSummary)
        Dim colSummary As List(Of TSummary) = New List(Of TSummary)()
        Dim objDict As Dictionary(Of String, TSummary) = New Dictionary(Of String, TSummary)()

        For Each objAccount As Account In objCompany.Accounts
            If objAccount.AcctSubType = lngSubType Then
                For Each objReg As Register In objAccount.Registers
                    For Each objTrx As BaseTrx In objReg.GetDateRange(Of BaseTrx)(New DateTime(1900, 1, 1), datEnd)
                        If Not objTrx.IsFake Then
                            Dim objNormalTrx As BankTrx = TryCast(objTrx, BankTrx)
                            If Not objNormalTrx Is Nothing Then
                                Dim objSummary As TSummary = GetSummary(colSummary, objDict, objTrx)
                                For Each objSplit As TrxSplit In objNormalTrx.Splits
                                    objSummary.AddAmountToCorrectAge(objSplit.Amount, objSplit.DueDateEffective, datAging)
                                Next
                            Else
                                Dim objReplicaTrx As ReplicaTrx = TryCast(objTrx, ReplicaTrx)
                                If Not objReplicaTrx Is Nothing Then
                                    Dim objSummary As TSummary = GetSummary(colSummary, objDict, objTrx)
                                    objSummary.AddAmountToCorrectAge(objReplicaTrx.Amount, objReplicaTrx.TrxDate, datAging)
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
        colSummary.Sort(AddressOf NameComparer)

        Return colSummary
    End Function

    Private Shared Function GetSummary(Of TSummary As {TrxNameSummary, New})(ByVal colSummary As List(Of TSummary),
                                         ByVal objDict As Dictionary(Of String, TSummary),
                                         ByVal objTrx As BaseTrx) As TSummary
        Dim objSummary As TSummary = Nothing
        Dim strNameKey As String = objTrx.Description.ToUpper()
        If strNameKey.Length > 16 Then
            strNameKey = strNameKey.Substring(0, 16)
        End If
        If Not objDict.TryGetValue(strNameKey, objSummary) Then
            objSummary = New TSummary()
            objSummary.TrxName = objTrx.Description
            objSummary.Balance = 0D
            colSummary.Add(objSummary)
            objDict.Add(strNameKey, objSummary)
        End If
        Return objSummary
    End Function

    Private Shared Function NameComparer(Of TSummary As TrxNameSummary)(ByVal objSummary1 As TSummary, ByVal objSummary2 As TSummary) As Integer
        Return objSummary1.TrxName.CompareTo(objSummary2.TrxName)
    End Function

    Protected MustOverride Function IsCharge(ByVal curAmount As Decimal) As Boolean

    Protected Sub AddAmountToCorrectAge(ByVal curAmount As Decimal, ByVal datDue As Date, ByVal datAging As Date)
        Me.Balance += curAmount
        If IsCharge(curAmount) Then
            Dim intAge As Integer = CInt(datAging.Subtract(datDue).TotalDays)
            If intAge <= -30 Then
                SummaryFutureCharges.Add(curAmount)
            ElseIf intAge <= 0 Then
                SummaryCurrentCharges.Add(curAmount)
            ElseIf intAge <= 30 Then
                Summary1To30Charges.Add(curAmount)
            ElseIf intAge <= 60 Then
                Summary31To60Charges.Add(curAmount)
            ElseIf intAge <= 90 Then
                Summary61To90Charges.Add(curAmount)
            Else
                SummaryOver90Charges.Add(curAmount)
            End If
        Else
            SummaryPayments.Add(curAmount)
        End If
    End Sub

    Private Sub ApplyPayments()
        If ApplyPayments(SummaryOver90Charges, SummaryPayments) Then
            If ApplyPayments(Summary61To90Charges, SummaryPayments) Then
                If ApplyPayments(Summary31To60Charges, SummaryPayments) Then
                    If ApplyPayments(Summary1To30Charges, SummaryPayments) Then
                        If ApplyPayments(SummaryCurrentCharges, SummaryPayments) Then
                            ApplyPayments(SummaryFutureCharges, SummaryPayments)
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Function ApplyPayments(ByVal objCharges As DateRangeSummary, ByVal objPayments As DateRangeSummary) As Boolean
        Dim curNetBalance As Decimal = objCharges.DateTotal + objPayments.DateTotal
        If IsCharge(curNetBalance) Or curNetBalance = 0 Then
            objCharges.DateTotal = curNetBalance
            objPayments.DateTotal = 0
            Return False
        Else
            objPayments.DateTotal = curNetBalance
            objCharges.DateTotal = 0
            Return True
        End If
    End Function

End Class
