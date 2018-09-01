Option Strict On
Option Explicit On

Public Class Utilities
    'Lower bound of many arrays
    Public Const intLBOUND1 As Short = 1

    Public Const strDateWithTwoDigitYear As String = "MM/dd/yy"

    Public Shared Function Split(ByVal strInput As String, ByVal strSeparator As String) As String()
        Dim sep(1) As String
        sep(0) = strSeparator
        Dim tmp() As String = strInput.Split(sep, StringSplitOptions.None)
        Return tmp
    End Function

End Class
