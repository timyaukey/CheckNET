Option Strict On
Option Explicit On

Public Class DateRangeSummary
    Public curDateTotal As Decimal = 0

    Public Sub Add(ByVal curAmount As Decimal)
        curDateTotal += curAmount
    End Sub
End Class
