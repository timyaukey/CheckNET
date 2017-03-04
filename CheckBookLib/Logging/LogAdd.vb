Option Strict On
Option Explicit On

Public Class LogAdd
    Implements ILogAdd

    Private mstrTitle As String
    Private mobjAddTrx As Trx
    Private mdatTimestamp As Date

    Private Sub ILogAdd_Init(ByVal strTitle As String, ByVal objAddTrx As Trx) Implements ILogAdd.Init
        mstrTitle = strTitle
        'objNewTrx is a clone created by EventLog.AddILogAdd().
        mobjAddTrx = objAddTrx
        mdatTimestamp = Now
    End Sub

    Private ReadOnly Property ILogger_blnRequiresLog() As Boolean Implements ILogger.blnRequiresLog
        Get
            ILogger_blnRequiresLog = True
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements ILogger.WriteLog
        objLog.EventStart(mstrTitle, mdatTimestamp)
        objLog.WriteTrx("AddTrx", mobjAddTrx)
        objLog.EventEnd()
    End Sub
End Class