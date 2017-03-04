Option Strict On
Option Explicit On

Public Class LogChange
    Implements ILogChange

    Private mstrTitle As String
    Private mobjOldTrx As Trx
    Private mobjNewTrx As Trx
    Private mdatTimestamp As Date

    Private Sub ILogChange_Init(ByVal strTitle As String, ByVal objNewTrx As Trx, ByVal objOldTrx As Trx) Implements ILogChange.Init
        mstrTitle = strTitle
        'Both Trx objects are clones created by EventLog.AddILogChange.
        'objOldTrx should have been cloned before passing to AddILogChange(),
        'but AddILogChange() clones it anyway just for grins.
        mobjOldTrx = objOldTrx
        mobjNewTrx = objNewTrx
        mdatTimestamp = Now
    End Sub

    Private ReadOnly Property ILogger_blnRequiresLog() As Boolean Implements ILogger.blnRequiresLog
        Get
            ILogger_blnRequiresLog = True
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements ILogger.WriteLog
        objLog.EventStart(mstrTitle, mdatTimestamp)
        objLog.WriteTrx("OldTrx", mobjOldTrx)
        objLog.WriteTrx("NewTrx", mobjNewTrx)
        objLog.EventEnd()
    End Sub
End Class