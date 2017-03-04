Option Strict On
Option Explicit On

Public Class LogGroupEnd
    Implements ILogGroupEnd

    Private Sub ILogGroupEnd_Init() Implements ILogGroupEnd.Init

    End Sub

    Private ReadOnly Property ILogger_blnRequiresLog() As Boolean Implements ILogger.blnRequiresLog
        Get
            ILogger_blnRequiresLog = False
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements ILogger.WriteLog
        objLog.GroupEnd()
    End Sub
End Class