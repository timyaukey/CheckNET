Option Strict On
Option Explicit On

Public Class LogDeleteNull
    Implements ILogDelete

    Private Sub ILogDelete_Init(ByVal strTitle As String, ByVal objOldTrx As BaseTrx) Implements ILogDelete.Init

    End Sub

    Private ReadOnly Property ILogger_RequiresLog() As Boolean Implements ILogger.RequiresLog
        Get
            ILogger_RequiresLog = False
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements ILogger.WriteLog

    End Sub
End Class