Option Strict On
Option Explicit On

Public Class LogAddNull
    Implements ILogAdd

    Private Sub ILogAdd_Init(ByVal strTitle As String, ByVal objAddTrx As BaseTrx) Implements ILogAdd.Init

    End Sub

    Private ReadOnly Property ILogger_blnRequiresLog() As Boolean Implements ILogger.blnRequiresLog
        Get
            ILogger_blnRequiresLog = False
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements ILogger.WriteLog

    End Sub
End Class