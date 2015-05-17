Option Strict Off
Option Explicit On
Public Class LogAdd
    Implements _ILogger
    Implements _ILogAdd


    Private mstrTitle As String
    Private mobjAddTrx As Trx
    Private mdatTimestamp As Date

    Private Sub ILogAdd_Init(ByVal strTitle As String, ByVal objAddTrx As Trx) Implements _ILogAdd.Init
        mstrTitle = strTitle
        'objNewTrx is a clone created by EventLog.AddILogAdd().
        mobjAddTrx = objAddTrx
        mdatTimestamp = Now
    End Sub

    Private ReadOnly Property ILogger_blnRequiresLog() As Boolean Implements _ILogger.blnRequiresLog
        Get
            ILogger_blnRequiresLog = True
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements _ILogger.WriteLog
        objLog.EventStart(mstrTitle, mdatTimestamp)
        objLog.WriteTrx("AddTrx", mobjAddTrx)
        objLog.EventEnd()
    End Sub
End Class