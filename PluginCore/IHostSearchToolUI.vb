Option Strict On
Option Explicit On

Public Interface IHostSearchToolUI
    Function objAllSelectedTrx() As IEnumerable(Of Trx)
    ReadOnly Property objReg() As Register
    Function blnValidTrxForBulkOperation(ByVal objTrx As Trx, ByVal strOperation As String) As Boolean
End Interface
