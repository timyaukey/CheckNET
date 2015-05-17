Option Strict Off
Option Explicit On
Public Interface _ILogDelete
	Sub Init(ByVal strTitle As String, ByVal objOldTrx As Trx)
End Interface
Public Class ILogDelete
    Implements _ILogDelete

    'Implementers must also implement ILogger.

    'Called only by EventLog.AddILogDelete(), which must clone the Trx passed.
    Public Sub Init(ByVal strTitle As String, ByVal objOldTrx As Trx) Implements _ILogDelete.Init

    End Sub
End Class