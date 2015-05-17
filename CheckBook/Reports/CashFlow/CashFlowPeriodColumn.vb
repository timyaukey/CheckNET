Option Strict Off
Option Explicit On
Public Class CashFlowPeriodColumn
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Total money allocated to one budget or category in a CashFlowPeriod.
    'Money associated with a budget is only added to a budget column,
    'money not associated with a budget is only added to a category column.
    'Multiple CashFlowPeriodBudget and CashFlowPeriodCategory will likely
    'contribute to this because it summarizes across multiple registers,
    'and a budget can be used by a budget Trx and a Split.

    Public intColIndex As Short
    Public curTotal As Decimal
End Class