Option Strict Off
Option Explicit On
Public Class LogMove
    Implements ILogChange
    Implements ILogger


    Private mstrTitle As String
    Private mobjOldTrx As Trx
    Private mobjNewTrx As Trx
    Private mdatTimestamp As Date

    Private Sub ILogChange_Init(ByVal strTitle As String, ByVal objNewTrx As Trx, ByVal objOldTrx As Trx) Implements ILogChange.Init
        mstrTitle = strTitle
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
        objLog.WriteValue("OldDate", gstrFormatDate(mobjOldTrx.datDate))
        objLog.WriteValue("NewDate", gstrFormatDate(mobjNewTrx.datDate))
        objLog.WriteValue("Number", mobjOldTrx.strNumber)
        objLog.WriteValue("Payee", mobjOldTrx.strDescription)
        objLog.WriteValue("DueDate", gstrSummarizeTrxDueDate(mobjOldTrx))
        objLog.WriteValue("Amount", gstrFormatCurrency(mobjOldTrx.curAmount))
        objLog.WriteValue("CatName", gstrSummarizeTrxCat(mobjOldTrx))
        objLog.EventEnd()
    End Sub
End Class