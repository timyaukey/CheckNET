Option Strict On
Option Explicit On

Public Class LogSave
    Implements ILogAction

    Private mstrTitle As String
    Private mdatTimestamp As Date

    Private Sub ILogAction_Init(ByVal strTitle As String) Implements ILogAction.Init
        mstrTitle = strTitle
        mdatTimestamp = Now
    End Sub

    Private ReadOnly Property ILogger_blnRequiresLog() As Boolean Implements ILogger.blnRequiresLog
        Get
            ILogger_blnRequiresLog = False
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements ILogger.WriteLog
        objLog.EventStart(mstrTitle, mdatTimestamp)
        objLog.EventEnd()
    End Sub
End Class