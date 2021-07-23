Option Strict On
Option Explicit On

Public Class LogStatus
    Implements ILogAdd

    Private mstrTitle As String
    Private mobjNewTrx As BaseTrx
    Private mdatTimestamp As Date

    Private Sub ILogAdd_Init(ByVal strTitle As String, ByVal objNewTrx As BaseTrx) Implements ILogAdd.Init
        mstrTitle = strTitle
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
        objLog.WriteValue("Date", Utilities.strFormatDate(mobjNewTrx.TrxDate))
        objLog.WriteValue("Number", mobjNewTrx.Number)
        objLog.WriteValue("Payee", mobjNewTrx.Description)
        objLog.WriteValue("Amount", Utilities.strFormatCurrency(mobjNewTrx.Amount))
        objLog.WriteValue("Status", mobjNewTrx.StatusLabel)
        objLog.EventEnd()
    End Sub
End Class