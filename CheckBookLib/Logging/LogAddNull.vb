Option Strict On
Option Explicit On

Public Class LogAddNull
    Implements ILogAdd

    Private Sub ILogAdd_Init(ByVal strTitle As String, ByVal objAddTrx As BaseTrx) Implements ILogAdd.Init

    End Sub

    Private ReadOnly Property ILogger_RequiresLog() As Boolean Implements ILogger.RequiresLog
        Get
            ILogger_RequiresLog = False
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements ILogger.WriteLog

    End Sub
End Class