Option Strict Off
Option Explicit On
Public Interface _ILogAdd
	Sub Init(ByVal strTitle As String, ByVal objNewTrx As Trx)
End Interface
Public Class ILogAdd
    Implements _ILogAdd

    'Implementers must also implement ILogger.

    'Called only by EventLog.AddILogAdd(), which must clone the Trx passed.
    Public Sub Init(ByVal strTitle As String, ByVal objNewTrx As Trx) Implements _ILogAdd.Init

    End Sub
End Class