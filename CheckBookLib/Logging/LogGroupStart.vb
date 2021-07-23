Option Strict On
Option Explicit On

Public Class LogGroupStart
    Implements ILogGroupStart

    Private mstrTitle As String

    Private Sub ILogGroupStart_Init(ByVal strTitle As String) Implements ILogGroupStart.Init
        mstrTitle = strTitle
    End Sub

    Private ReadOnly Property ILogger_RequiresLog() As Boolean Implements ILogger.RequiresLog
        Get
            ILogger_RequiresLog = False
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements ILogger.WriteLog
        objLog.GroupStart(mstrTitle)
    End Sub
End Class