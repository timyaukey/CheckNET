Option Strict On
Option Explicit On

Public Module ErrorHandling

    'Error handling utilities.

    Public Sub gTopException(ByVal ex As Exception)
        Dim ex2 As Exception
        ex2 = ex
        While Not ex2 Is Nothing
            MsgBox(ex2.Message + Environment.NewLine + ex2.StackTrace.ToString())
            ex2 = ex2.InnerException
        End While
    End Sub

    Public Sub gNestedException(ByVal ex As Exception)
        Throw ex
    End Sub

    Public Sub gRaiseError(ByVal strMsg As String)
        Throw New Exception(strMsg)
    End Sub
End Module