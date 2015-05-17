Option Strict Off
Option Explicit On
Public Interface _ILogChange
	Sub Init(ByVal strTitle As String, ByVal objNewTrx As Trx, ByVal objOldTrx As Trx)
End Interface
Public Class ILogChange
    Implements _ILogChange

    'Implementers must also implement ILogger.

    'Called only by EventLog.AddILogChange(), which must clone the Trx passed.
    Public Sub Init(ByVal strTitle As String, ByVal objNewTrx As Trx, ByVal objOldTrx As Trx) Implements _ILogChange.Init

    End Sub
End Class