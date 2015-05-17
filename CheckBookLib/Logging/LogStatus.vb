Option Strict Off
Option Explicit On
Public Class LogStatus
    Implements _ILogAdd
    Implements _ILogger


    Private mstrTitle As String
    Private mobjNewTrx As Trx
    Private mdatTimestamp As Date

    Private Sub ILogAdd_Init(ByVal strTitle As String, ByVal objNewTrx As Trx) Implements _ILogAdd.Init
        mstrTitle = strTitle
        mobjNewTrx = objNewTrx
        mdatTimestamp = Now
    End Sub

    Private ReadOnly Property ILogger_blnRequiresLog() As Boolean Implements _ILogger.blnRequiresLog
        Get
            ILogger_blnRequiresLog = True
        End Get
    End Property

    Private Sub ILogger_WriteLog(ByVal objLog As EventLog) Implements _ILogger.WriteLog
        objLog.EventStart(mstrTitle, mdatTimestamp)
        objLog.WriteValue("Date", VB6.Format(mobjNewTrx.datDate, gstrFORMAT_DATE))
        objLog.WriteValue("Number", mobjNewTrx.strNumber)
        objLog.WriteValue("Payee", mobjNewTrx.strDescription)
        objLog.WriteValue("Amount", VB6.Format(mobjNewTrx.curAmount, gstrFORMAT_CURRENCY))
        objLog.WriteValue("Status", mobjNewTrx.strStatus)
        objLog.EventEnd()
    End Sub
End Class