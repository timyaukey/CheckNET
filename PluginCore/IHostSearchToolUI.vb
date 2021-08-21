Option Strict On
Option Explicit On

Public Interface IHostSearchToolUI
    Function GetAllSelectedTrx() As IEnumerable(Of BaseTrx)
    ReadOnly Property Reg() As Register
    Function IsValidTrxForBulkOperation(ByVal objTrx As BaseTrx, ByVal strOperation As String) As Boolean
End Interface
