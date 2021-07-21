Option Strict On
Option Explicit On

Public Interface IHostSearchToolUI
    Function objAllSelectedTrx() As IEnumerable(Of BaseTrx)
    ReadOnly Property objReg() As Register
    Function blnValidTrxForBulkOperation(ByVal objTrx As BaseTrx, ByVal strOperation As String) As Boolean
End Interface
