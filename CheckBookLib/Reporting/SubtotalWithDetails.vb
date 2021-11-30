Option Strict On
Option Explicit On

Public Class SubtotalWithDetails
    Public InvoiceDetails As List(Of SubtotalDetail) = New List(Of SubtotalDetail)()
    Public NonInvoiceDetails As List(Of SubtotalDetail) = New List(Of SubtotalDetail)()

    Public Sub Add(ByVal curAmount As Decimal, ByVal strInvoiceNum As String, ByVal datTrxDate As DateTime)
        Dim dtl As SubtotalDetail = New SubtotalDetail()
        dtl.Amount = curAmount
        dtl.TrxDate = datTrxDate
        If String.IsNullOrEmpty(strInvoiceNum) Then
            dtl.InvoiceNum = ""
            NonInvoiceDetails.Add(dtl)
        Else
            dtl.InvoiceNum = strInvoiceNum
            InvoiceDetails.Add(dtl)
        End If
    End Sub

    Public ReadOnly Property Subtotal As Decimal
        Get
            Return SubtotalInvoice + SubtotalNonInvoice
        End Get
    End Property

    Public ReadOnly Property SubtotalInvoice As Decimal
        Get
            Dim amount As Decimal = 0
            For Each dtl As SubtotalDetail In InvoiceDetails
                amount += dtl.Amount
            Next
            Return amount
        End Get
    End Property

    Public ReadOnly Property SubtotalNonInvoice As Decimal
        Get
            Dim amount As Decimal = 0
            For Each dtl As SubtotalDetail In NonInvoiceDetails
                amount += dtl.Amount
            Next
            Return amount
        End Get
    End Property
End Class
