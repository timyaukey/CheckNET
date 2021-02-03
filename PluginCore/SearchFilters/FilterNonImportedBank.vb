Option Strict On
Option Explicit On

Public Class FilterNonImportedBank
    Implements ISearchFilter

    Public Function blnInclude(objTrx As Trx) As Boolean Implements ISearchFilter.blnInclude
        If objTrx.GetType() Is GetType(NormalTrx) Then
            Dim objBankTrx As NormalTrx = (DirectCast(objTrx, NormalTrx))
            Return String.IsNullOrEmpty(objBankTrx.strImportKey) And (objBankTrx.lngStatus <> Trx.TrxStatus.Reconciled)
        End If
        Return False
    End Function

    Public Overrides Function ToString() As String
        Return "Non-Imported, Non-Reconciled Bank Trx"
    End Function
End Class
