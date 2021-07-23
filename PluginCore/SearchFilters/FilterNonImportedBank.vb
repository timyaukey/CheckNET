Option Strict On
Option Explicit On

Public Class FilterNonImportedBank
    Implements ISearchFilter

    Public Function blnInclude(objTrx As BaseTrx) As Boolean Implements ISearchFilter.blnInclude
        If objTrx.GetType() Is GetType(BankTrx) Then
            Dim objBankTrx As BankTrx = (DirectCast(objTrx, BankTrx))
            Return String.IsNullOrEmpty(objBankTrx.ImportKey) And (objBankTrx.Status <> BaseTrx.TrxStatus.Reconciled)
        End If
        Return False
    End Function

    Public Overrides Function ToString() As String
        Return "Non-Imported, Non-Reconciled Bank Trx"
    End Function
End Class
