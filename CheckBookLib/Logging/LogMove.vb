Option Strict On
Option Explicit On

Public Class LogMove
    Implements ILogChange

    Private mstrTitle As String
    Private mobjOldTrx As BankTrx
    Private mobjNewTrx As BankTrx
    Private mdatTimestamp As Date

    Private Sub ILogChange_Init(ByVal strTitle As String, ByVal objNewTrx As BaseTrx, ByVal objOldTrx As BaseTrx) Implements ILogChange.Init
        mstrTitle = strTitle
        mobjOldTrx = DirectCast(objOldTrx, BankTrx)
        mobjNewTrx = DirectCast(objNewTrx, BankTrx)
        mdatTimestamp = Now
    End Sub

    Private ReadOnly Property ILogger_RequiresLog() As Boolean Implements ILogger.RequiresLog
        Get
            ILogger_RequiresLog = True
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements ILogger.WriteLog
        objLog.EventStart(mstrTitle, mdatTimestamp)
        objLog.WriteValue("OldDate", Utilities.FormatDate(mobjOldTrx.TrxDate))
        objLog.WriteValue("NewDate", Utilities.FormatDate(mobjNewTrx.TrxDate))
        objLog.WriteValue("Number", mobjOldTrx.Number)
        objLog.WriteValue("Payee", mobjOldTrx.Description)
        objLog.WriteValue("DueDate", mobjOldTrx.SummarizeDueDates())
        objLog.WriteValue("Amount", Utilities.FormatCurrency(mobjOldTrx.Amount))
        objLog.WriteValue("CatName", mobjOldTrx.CategoryLabel)
        objLog.EventEnd()
    End Sub
End Class