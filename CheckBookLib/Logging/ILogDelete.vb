Option Strict On
Option Explicit On

Public Interface ILogDelete
    Inherits ILogger
    Sub Init(ByVal strTitle As String, ByVal objOldTrx As Trx)
End Interface
