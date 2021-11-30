Option Strict On
Option Explicit On

Public MustInherit Class TrxNameSummary
    Public TrxName As String
    Public Balance As Decimal

    Public SummaryCurrentCharges As SubtotalWithDetails
    Public Summary1To30Charges As SubtotalWithDetails
    Public Summary31To60Charges As SubtotalWithDetails
    Public Summary61To90Charges As SubtotalWithDetails
    Public SummaryOver90Charges As SubtotalWithDetails
    Public SummaryFutureCharges As SubtotalWithDetails
    Public SummaryPayments As SubtotalWithDetails

    Public Sub New()
        Balance = 0
        SummaryCurrentCharges = New SubtotalWithDetails()
        Summary1To30Charges = New SubtotalWithDetails()
        Summary31To60Charges = New SubtotalWithDetails()
        Summary61To90Charges = New SubtotalWithDetails()
        SummaryOver90Charges = New SubtotalWithDetails()
        SummaryFutureCharges = New SubtotalWithDetails()
        SummaryPayments = New SubtotalWithDetails()
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
                                    objSummary.AddAmountToCorrectAge(objSplit.Amount, objTrx.TrxDate,
                                        objSplit.DueDateEffective, datAging, objSplit.InvoiceNum)
                                Next
                            Else
                                Dim objReplicaTrx As ReplicaTrx = TryCast(objTrx, ReplicaTrx)
                                If Not objReplicaTrx Is Nothing Then
                                    Dim objSummary As TSummary = GetSummary(colSummary, objDict, objTrx)
                                    objSummary.AddAmountToCorrectAge(objReplicaTrx.Amount, objReplicaTrx.TrxDate,
                                        objReplicaTrx.TrxDate, datAging, objReplicaTrx.InvoiceNum)
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

    Protected Sub AddAmountToCorrectAge(ByVal curAmount As Decimal, ByVal datTrx As Date,
                                        ByVal datDue As Date, ByVal datAging As Date,
                                        ByVal strInvoiceNum As String)
        Me.Balance += curAmount
        If IsCharge(curAmount) Then
            Dim intAge As Integer = CInt(datAging.Subtract(datDue).TotalDays)
            If intAge <= -30 Then
                SummaryFutureCharges.Add(curAmount, strInvoiceNum, datTrx)
            ElseIf intAge <= 0 Then
                SummaryCurrentCharges.Add(curAmount, strInvoiceNum, datTrx)
            ElseIf intAge <= 30 Then
                Summary1To30Charges.Add(curAmount, strInvoiceNum, datTrx)
            ElseIf intAge <= 60 Then
                Summary31To60Charges.Add(curAmount, strInvoiceNum, datTrx)
            ElseIf intAge <= 90 Then
                Summary61To90Charges.Add(curAmount, strInvoiceNum, datTrx)
            Else
                SummaryOver90Charges.Add(curAmount, strInvoiceNum, datTrx)
            End If
        Else
            SummaryPayments.Add(curAmount, strInvoiceNum, datTrx)
        End If
    End Sub

    Private Sub ApplyPayments()
        'First apply payments applied to specific invoices
        ApplyPaymentsToInvoiceNum(SummaryOver90Charges, SummaryPayments)
        ApplyPaymentsToInvoiceNum(Summary61To90Charges, SummaryPayments)
        ApplyPaymentsToInvoiceNum(Summary31To60Charges, SummaryPayments)
        ApplyPaymentsToInvoiceNum(Summary1To30Charges, SummaryPayments)
        ApplyPaymentsToInvoiceNum(SummaryCurrentCharges, SummaryPayments)
        ApplyPaymentsToInvoiceNum(SummaryFutureCharges, SummaryPayments)
        'Then apply to any charge any payments or parts of payments that are still left
        ApplyPaymentsToAnyCharge(SummaryOver90Charges, SummaryPayments)
        ApplyPaymentsToAnyCharge(Summary61To90Charges, SummaryPayments)
        ApplyPaymentsToAnyCharge(Summary31To60Charges, SummaryPayments)
        ApplyPaymentsToAnyCharge(Summary1To30Charges, SummaryPayments)
        ApplyPaymentsToAnyCharge(SummaryCurrentCharges, SummaryPayments)
        ApplyPaymentsToAnyCharge(SummaryFutureCharges, SummaryPayments)
    End Sub

    Private Sub ApplyPaymentsToInvoiceNum(ByVal objCharges As SubtotalWithDetails, ByVal objPayments As SubtotalWithDetails)
        For Each objChargeDetail As SubtotalDetail In objCharges.InvoiceDetails
            For Each objPaymentDetail As SubtotalDetail In objPayments.InvoiceDetails
                If objChargeDetail.InvoiceNum = objPaymentDetail.InvoiceNum Then
                    ApplyPaymentToCharge(objChargeDetail, objPaymentDetail)
                End If
            Next
        Next
    End Sub

    Private Sub ApplyPaymentsToAnyCharge(ByVal objCharges As SubtotalWithDetails, ByVal objPayments As SubtotalWithDetails)
        For Each objPaymentDetail As SubtotalDetail In objPayments.InvoiceDetails
            For Each objChargeDetail As SubtotalDetail In objCharges.InvoiceDetails
                ApplyPaymentToCharge(objChargeDetail, objPaymentDetail)
            Next
            For Each objChargeDetail As SubtotalDetail In objCharges.NonInvoiceDetails
                ApplyPaymentToCharge(objChargeDetail, objPaymentDetail)
            Next
        Next
        For Each objPaymentDetail As SubtotalDetail In objPayments.NonInvoiceDetails
            For Each objChargeDetail As SubtotalDetail In objCharges.InvoiceDetails
                ApplyPaymentToCharge(objChargeDetail, objPaymentDetail)
            Next
            For Each objChargeDetail As SubtotalDetail In objCharges.NonInvoiceDetails
                ApplyPaymentToCharge(objChargeDetail, objPaymentDetail)
            Next
        Next
    End Sub

    ''' <summary>
    ''' Apply as much of a payment as possible to a charge.
    ''' If the charge is larger than or equal to the payment, then apply the entire payment.
    ''' Otherwise apply only the amount needed to pay off the charge.
    ''' </summary>
    ''' <param name="objCharge"></param>
    ''' <param name="objPayment"></param>
    Private Sub ApplyPaymentToCharge(ByVal objCharge As SubtotalDetail, ByVal objPayment As SubtotalDetail)
        Dim curNewCharge As Decimal = objCharge.Amount + objPayment.Amount
        If IsCharge(curNewCharge) Then
            'Charge was NOT fully paid
            objCharge.Amount = curNewCharge
            objPayment.Amount = 0
        Else
            'Charge WAS fully paid
            objPayment.Amount = curNewCharge
            objCharge.Amount = 0
        End If
    End Sub

End Class
