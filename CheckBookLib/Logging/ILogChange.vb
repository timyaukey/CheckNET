Option Strict Off
Option Explicit On
Public Interface ILogChange
    Sub Init(ByVal strTitle As String, ByVal objNewTrx As Trx, ByVal objOldTrx As Trx)
End Interface
