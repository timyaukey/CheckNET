Option Strict On
Option Explicit On

Public Interface ILogAction
    Inherits ILogger
    Sub Init(ByVal strTitle As String)
End Interface
