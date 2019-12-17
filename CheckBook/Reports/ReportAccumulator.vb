Option Strict On
Option Explicit On

Public Class ReportAccumulator
    Private mcurTotal As Decimal = 0D

    Public Sub Add(ByVal curAmount As Decimal)
        mcurTotal += curAmount
    End Sub

    Public ReadOnly Property curTotal() As Decimal
        Get
            Return mcurTotal
        End Get
    End Property
End Class
