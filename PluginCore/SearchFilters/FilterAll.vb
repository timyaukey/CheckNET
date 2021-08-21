Option Strict On
Option Explicit On

Public Class FilterAll
    Implements ISearchFilter

    Public Function IsIncluded(objTrx As BaseTrx) As Boolean Implements ISearchFilter.IsIncluded
        Return True
    End Function

    Public Overrides Function ToString() As String
        Return "All"
    End Function
End Class
