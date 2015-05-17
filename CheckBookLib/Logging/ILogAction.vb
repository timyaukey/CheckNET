Option Strict Off
Option Explicit On
Public Interface _ILogAction
	Sub Init(ByVal strTitle As String)
End Interface
Public Class ILogAction
    Implements _ILogAction

    'Implementers must also implement ILogger.

    Public Sub Init(ByVal strTitle As String) Implements _ILogAction.Init

    End Sub
End Class