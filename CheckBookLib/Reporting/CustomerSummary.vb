Option Strict On
Option Explicit On

Public Class CustomerSummary
    Inherits TrxNameSummary

    Protected Overrides Function blnIsCharge(curAmount As Decimal) As Boolean
        Return curAmount > 0
    End Function
End Class
