Option Strict On
Option Explicit On

Public Class ImportReadException
    Inherits Exception

    Public Sub New(ByVal strMessage As String)
        MyBase.New(strMessage)
    End Sub

End Class
