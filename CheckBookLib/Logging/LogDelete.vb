Option Strict Off
Option Explicit On
Public Class LogDelete
    Implements _ILogger
    Implements _ILogDelete


    Private mstrTitle As String
    Private mobjOldTrx As Trx
    Private mdatTimestamp As Date

    Private Sub ILogDelete_Init(ByVal strTitle As String, ByVal objOldTrx As Trx) Implements _ILogDelete.Init
        mstrTitle = strTitle
        'This trx is a clone created by EventLog.AddILogDelete().
        mobjOldTrx = objOldTrx
        mdatTimestamp = Now
    End Sub

    Private ReadOnly Property ILogger_blnRequiresLog() As Boolean Implements _ILogger.blnRequiresLog
        Get
            ILogger_blnRequiresLog = True
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements _ILogger.WriteLog
        objLog.EventStart(mstrTitle, mdatTimestamp)
        objLog.WriteTrx("DelTrx", mobjOldTrx)
        objLog.EventEnd()
    End Sub
End Class