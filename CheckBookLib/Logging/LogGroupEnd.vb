Option Strict Off
Option Explicit On
Public Class LogGroupEnd
    Implements _ILogGroupEnd
    Implements _ILogger


    Private Sub ILogGroupEnd_Init() Implements _ILogGroupEnd.Init

    End Sub

    Private ReadOnly Property ILogger_blnRequiresLog() As Boolean Implements _ILogger.blnRequiresLog
        Get
            ILogger_blnRequiresLog = False
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements _ILogger.WriteLog
        objLog.GroupEnd()
    End Sub
End Class