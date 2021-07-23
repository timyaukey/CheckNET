Option Strict On
Option Explicit On

Public Interface ILogger
    Sub WriteLog(ByVal objLog As EventLog)
    ReadOnly Property RequiresLog As Boolean
End Interface
