Option Strict On
Option Explicit On

Public Interface ILogAdd
    Inherits ILogger
    Sub Init(ByVal strTitle As String, ByVal objNewTrx As BaseTrx)
End Interface
