Option Strict On
Option Explicit On

Imports CheckBookLib
Imports PluginCore

Public MustInherit Class SearchComparer
    Public MustOverride Function blnCompare(ByVal str1 As String, ByVal str2 As String) As Boolean
End Class

Public Class SearchComparerEqualTo
    Inherits SearchComparer
    Public Overrides Function blnCompare(str1 As String, str2 As String) As Boolean
        Return StrComp(str1, str2, CompareMethod.Text) = 0
    End Function
    Public Overrides Function ToString() As String
        Return "Equal To"
    End Function
End Class

Public Class SearchComparerStartsWith
    Inherits SearchComparer
    Public Overrides Function blnCompare(str1 As String, str2 As String) As Boolean
        Return StrComp(Left(str1, Len(str2)), str2, CompareMethod.Text) = 0
    End Function
    Public Overrides Function ToString() As String
        Return "Starts With"
    End Function
End Class

Public Class SearchComparerContains
    Inherits SearchComparer
    Public Overrides Function blnCompare(str1 As String, str2 As String) As Boolean
        Return InStr(1, str1, str2, CompareMethod.Text) > 0
    End Function
    Public Overrides Function ToString() As String
        Return "Contains"
    End Function
End Class
