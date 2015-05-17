Option Strict Off
Option Explicit On
Public Class LogGroupStart
    Implements _ILogGroupStart
    Implements _ILogger


    Private mstrTitle As String

    Private Sub ILogGroupStart_Init(ByVal strTitle As String) Implements _ILogGroupStart.Init
        mstrTitle = strTitle
    End Sub

    Private ReadOnly Property ILogger_blnRequiresLog() As Boolean Implements _ILogger.blnRequiresLog
        Get
            ILogger_blnRequiresLog = False
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements _ILogger.WriteLog
        objLog.GroupStart(mstrTitle)
    End Sub
End Class