Option Strict On
Option Explicit On

Imports CheckBookLib

Public Interface IImportHandler
    Function objStatusSearch(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register) As NormalTrx

    ReadOnly Property blnAllowNew() As Boolean
    Function strAutoNewValidationError(ByVal objImportedTrx As ImportedTrx, ByVal objAccount As Account, ByVal blnManualSelectionAllowed As Boolean) As String
    Function blnAlternateAutoNewHandling(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register) As Boolean
    Sub AutoNewSearch(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register,
        ByRef colMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean)

    Sub Init(ByVal objHostUI As IHostUI)

    ReadOnly Property strBatchUpdateFields() As String
    ReadOnly Property blnAllowBatchUpdates() As Boolean
    ReadOnly Property blnAllowMultiPartMatches() As Boolean

    'Copy objMatchedTrx from objImportedTrx with fields appropriate to the implementation.
    'If intSeqNumber=0 then follow the normal update logic on trx amount, but if intSeqNumber>0 then
    'objMatchTrx is an extra trx in a multi-part match and its amount must be set to zero. intSeqNumber will never
    'be >0 unless blnAllowMultiPartMatches is true.
    Sub BatchUpdate(ByVal objImportedTrx As ImportedTrx, ByVal objMatchedTrx As NormalTrx, ByVal intSeqNumber As Integer)

    Sub BatchUpdateSearch(ByVal objReg As Register, ByVal objImportedTrx As ImportedTrx,
        ByVal colAllMatchedTrx As IEnumerable(Of NormalTrx), ByRef colUnusedMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean)

    ReadOnly Property blnAllowIndividualUpdates() As Boolean

    Sub IndividualSearch(ByVal objReg As Register, ByVal objImportedTrx As ImportedTrx,
        ByVal blnLooseMatch As Boolean, ByRef colMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean)

    Function blnIndividualUpdate(ByVal objImportedTrx As ImportedTrx, ByVal objMatchedTrx As NormalTrx) As Boolean
End Interface
