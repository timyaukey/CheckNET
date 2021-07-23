Option Strict On
Option Explicit On

Public Class LogAction
    Implements ILogAction

    Private mstrTitle As String
    Private mdatTimestamp As Date

    Private Sub ILogAction_Init(ByVal strTitle As String) Implements ILogAction.Init
        mstrTitle = strTitle
        mdatTimestamp = Now
    End Sub

    Private ReadOnly Property ILogger_RequiresLog() As Boolean Implements ILogger.RequiresLog
        Get
            ILogger_RequiresLog = True
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements ILogger.WriteLog
        objLog.EventStart(mstrTitle, mdatTimestamp)
        objLog.EventEnd()
    End Sub
End Class