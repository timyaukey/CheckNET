Option Strict Off
Option Explicit On

Imports VB = Microsoft.VisualBasic
Imports CheckBookLib

Public Interface IImportHandler
    Function lngStatusSearch(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register) As Integer

    ReadOnly Property blnAllowNew() As Boolean
    Function strAutoNewValidationError(ByVal objImportedTrx As ImportedTrx, ByVal blnAllowBankNonCard As Boolean) As String
    Function blnAlternateAutoNewHandling(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register) As Boolean
    Sub AutoNewSearch(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register, ByRef colMatches As ICollection(Of Integer), ByRef blnExactMatch As Boolean)

    Sub BatchUpdate(ByVal objMatchedReg As Register, ByVal lngMatchedRegIndex As Integer, ByVal objImportedTrx As ImportedTrx, ByVal objMatchedTrx As Trx, ByVal blnFake As Boolean)
    ReadOnly Property strBatchUpdateFields() As String
    ReadOnly Property blnAllowBatchUpdates() As Boolean
    Sub BatchUpdateSearch(ByVal objReg As Register, ByVal objImportedTrx As ImportedTrx, ByVal colAllMatchedTrx As IEnumerable(Of Trx), ByRef colUnusedMatches As ICollection(Of Integer), ByRef blnExactMatch As Boolean)
End Interface
