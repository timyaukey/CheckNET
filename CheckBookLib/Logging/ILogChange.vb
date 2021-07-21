Option Strict On
Option Explicit On

Public Interface ILogChange
    Inherits ILogger
    Sub Init(ByVal strTitle As String, ByVal objNewTrx As BaseTrx, ByVal objOldTrx As BaseTrx)
End Interface
