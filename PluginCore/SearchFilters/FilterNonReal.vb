Option Strict On
Option Explicit On

Public Class FilterNonReal
    Implements ISearchFilter

    Public Function IsIncluded(objTrx As BaseTrx) As Boolean Implements ISearchFilter.IsIncluded
        Return objTrx.IsFake
    End Function

    Public Overrides Function ToString() As String
        Return "Fake or Generated"
    End Function
End Class
