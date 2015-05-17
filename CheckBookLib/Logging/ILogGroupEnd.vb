Option Strict Off
Option Explicit On
Public Interface _ILogGroupEnd
	Sub Init()
End Interface
Public Class ILogGroupEnd
    Implements _ILogGroupEnd

    'Implementers must also implement ILogger.

    Public Sub Init() Implements _ILogGroupEnd.Init

    End Sub
End Class