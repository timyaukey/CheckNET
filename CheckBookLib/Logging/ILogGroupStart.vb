Option Strict Off
Option Explicit On
Public Interface _ILogGroupStart
	Sub Init(ByVal strTitle As String)
End Interface
Public Class ILogGroupStart
    Implements _ILogGroupStart

    'Implementers must also implement ILogger.

    Public Sub Init(ByVal strTitle As String) Implements _ILogGroupStart.Init

    End Sub
End Class