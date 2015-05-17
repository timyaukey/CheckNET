Option Strict Off
Option Explicit On
Public Interface _ILogger
	Sub WriteLog(ByVal objLog As EventLog)
	ReadOnly Property blnRequiresLog As Boolean
End Interface
Public Class ILogger
    Implements _ILogger

    Public Sub WriteLog(ByVal objLog As EventLog) Implements _ILogger.WriteLog

    End Sub

    Public ReadOnly Property blnRequiresLog() As Boolean Implements _ILogger.blnRequiresLog
        Get

        End Get
    End Property
End Class