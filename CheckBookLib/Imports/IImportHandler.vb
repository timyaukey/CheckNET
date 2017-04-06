﻿Option Strict On
Option Explicit On

Public Interface IImportHandler
    Function objStatusSearch(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register) As NormalTrx

    ReadOnly Property blnAllowNew() As Boolean
    Function strAutoNewValidationError(ByVal objImportedTrx As ImportedTrx, ByVal objAccount As Account, ByVal blnManualSelectionAllowed As Boolean) As String
    Function blnAlternateAutoNewHandling(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register) As Boolean
    Sub AutoNewSearch(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register,
        ByRef colMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean)

    ReadOnly Property strBatchUpdateFields() As String
    ReadOnly Property blnAllowBatchUpdates() As Boolean
    Sub BatchUpdate(ByVal objImportedTrx As ImportedTrx, ByVal objMatchedTrx As NormalTrx)
    Sub BatchUpdateSearch(ByVal objReg As Register, ByVal objImportedTrx As ImportedTrx,
        ByVal colAllMatchedTrx As IEnumerable(Of NormalTrx), ByRef colUnusedMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean)

    ReadOnly Property blnAllowIndividualUpdates() As Boolean
    Sub IndividualSearch(ByVal objReg As Register, ByVal objImportedTrx As ImportedTrx,
        ByVal blnLooseMatch As Boolean, ByRef colMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean)
    Function blnIndividualUpdate(ByVal objImportedTrx As ImportedTrx, ByVal objMatchedTrx As NormalTrx) As Boolean
End Interface
