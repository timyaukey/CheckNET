Option Strict On
Option Explicit On

Public Class LogChange
    Implements ILogChange

    Private mstrTitle As String
    Private mobjOldTrx As BaseTrx
    Private mobjNewTrx As BaseTrx
    Private mdatTimestamp As Date

    Private Sub ILogChange_Init(ByVal strTitle As String, ByVal objNewTrx As BaseTrx, ByVal objOldTrx As BaseTrx) Implements ILogChange.Init
        mstrTitle = strTitle
        'Both BaseTrx objects are clones created by EventLog.AddILogChange.
        'objOldTrx should have been cloned before passing to AddILogChange(),
        'but AddILogChange() clones it anyway just for grins.
        mobjOldTrx = objOldTrx
        mobjNewTrx = objNewTrx
        mdatTimestamp = Now
    End Sub

    Private ReadOnly Property ILogger_RequiresLog() As Boolean Implements ILogger.RequiresLog
        Get
            ILogger_RequiresLog = True
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements ILogger.WriteLog
        objLog.EventStart(mstrTitle, mdatTimestamp)
        objLog.WriteTrx("OldTrx", mobjOldTrx)
        objLog.WriteTrx("NewTrx", mobjNewTrx)
        objLog.EventEnd()
    End Sub
End Class