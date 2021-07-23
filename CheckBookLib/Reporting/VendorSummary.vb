Option Strict On
Option Explicit On

Public Class VendorSummary
    Inherits TrxNameSummary

    Protected Overrides Function IsCharge(curAmount As Decimal) As Boolean
        Return curAmount < 0
    End Function
End Class
