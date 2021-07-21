Option Strict On
Option Explicit On

Public Class FilterNonRealBank
    Implements ISearchFilter

    Public Function blnInclude(objTrx As BaseTrx) As Boolean Implements ISearchFilter.blnInclude
        Return objTrx.blnFake And (objTrx.GetType() Is GetType(BankTrx))
    End Function

    Public Overrides Function ToString() As String
        Return "Fake or Generated Bank Transactions"
    End Function
End Class
