Option Strict Off
Option Explicit On
Public Module ErrorHandling

    'Error handling utilities.

    Private Const mlngPASS_THROUGH_ERROR As Integer = 30000
    Private Const mlngRAISE_ERROR As Integer = 30001
    Private mstrSavedErr As String
    Private mlngSavedErr As Integer

    Public Sub gTopException(ByVal ex As Exception)
        Dim ex2 As Exception
        ex2 = ex
        While Not ex2 Is Nothing
            MsgBox(ex2.Message + Environment.NewLine + ex2.StackTrace.ToString())
            ex2 = ex2.InnerException
        End While
    End Sub

    Public Sub gTopErrorTrap(ByVal strRoutine As String)
        MsgBox(strDescription(strRoutine), MsgBoxStyle.Critical)
    End Sub

    Public Sub gNestedErrorTrap(ByVal strRoutine As String)
        Dim strErr As String
        strErr = strDescription(strRoutine)
        Err.Raise(mlngPASS_THROUGH_ERROR, , strErr)
    End Sub

    Private Function strDescription(ByVal strRoutine As String) As String
        If Err.Number = mlngPASS_THROUGH_ERROR Then
            strDescription = Err.Description & vbCrLf & "called from " & strRoutine
        Else
            strDescription = "Unexpected error """ & Err.Description & """" & vbCrLf & "in " & strRoutine
        End If
    End Function

    Public Sub gRaiseError(ByVal strMsg As String)
        Err.Clear()
        Err.Raise(mlngRAISE_ERROR, , strMsg)
    End Sub

    Public Sub gSaveCurrentError()
        mstrSavedErr = Err.Description
        mlngSavedErr = Err.Number
    End Sub

    Public Sub gRestoreCurrentError()
        Err.Number = mlngSavedErr
        Err.Description = mstrSavedErr
    End Sub
End Module