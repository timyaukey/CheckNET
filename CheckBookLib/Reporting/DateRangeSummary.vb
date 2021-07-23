Option Strict On
Option Explicit On

Public Class DateRangeSummary
    Public DateTotal As Decimal = 0

    Public Sub Add(ByVal curAmount As Decimal)
        DateTotal += curAmount
    End Sub
End Class
