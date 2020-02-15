Option Strict On
Option Explicit On

Public Class FilterAll
    Implements ISearchFilter

    Public Function blnInclude(objTrx As Trx) As Boolean Implements ISearchFilter.blnInclude
        Return True
    End Function

    Public Overrides Function ToString() As String
        Return "All"
    End Function
End Class
