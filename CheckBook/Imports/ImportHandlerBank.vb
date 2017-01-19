﻿Option Strict Off
Option Explicit On
Imports CheckBook
Imports CheckBookLib

Public Class ImportHandlerBank
    Implements IImportHandler

    Public ReadOnly Property blnAllowNew As Boolean Implements IImportHandler.blnAllowNew
        Get
            Return True
        End Get
    End Property

    Public Sub AutoNewSearch(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register, ByRef colMatches As ICollection(Of Integer), ByRef blnExactMatch As Boolean) Implements IImportHandler.AutoNewSearch
        Dim lngNumber As Integer = 0
        Dim colExactMatches As ICollection(Of Integer) = Nothing
        objReg.MatchCore(lngNumber, objImportedTrx.datDate, 60, objImportedTrx.strDescription, objImportedTrx.curAmount,
                                     objImportedTrx.curMatchMin, objImportedTrx.curMatchMax, False, colMatches, colExactMatches, blnExactMatch)
        objReg.PruneToExactMatches(colExactMatches, objImportedTrx.datDate, colMatches, blnExactMatch)
    End Sub

    Public Function blnAlternateAutoNewHandling(objImportedTrx As ImportedTrx, objReg As Register) As Boolean Implements IImportHandler.blnAlternateAutoNewHandling
        Return False
    End Function

    Public Function strAutoNewValidationError(objImportedTrx As ImportedTrx, blnAllowBankNonCard As Boolean) As String Implements IImportHandler.strAutoNewValidationError
        Dim strTrxNum As String = LCase(objImportedTrx.strNumber)
        If (strTrxNum <> "card") And Not blnAllowBankNonCard Then
            Return "Transaction is not a credit or debit card use"
        End If
        Return Nothing
    End Function

    Public Function lngStatusSearch(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register) As Integer Implements IImportHandler.lngStatusSearch
        If objImportedTrx.strImportKey <> "" Then
            Return objReg.lngMatchImportKey(objImportedTrx.strImportKey)
        End If
        Return 0
    End Function

    Public Sub BatchUpdate(objMatchedReg As Register, lngMatchedRegIndex As Integer, objImportedTrx As ImportedTrx, objMatchedTrx As Trx, blnFake As Boolean) Implements IImportHandler.BatchUpdate
        objMatchedReg.ImportUpdateBank(lngMatchedRegIndex, objImportedTrx.datDate, objMatchedTrx.strNumber, blnFake, objImportedTrx.curAmount, objImportedTrx.strImportKey)
    End Sub

    Public Sub BatchUpdateSearch(objReg As Register, objImportedTrx As ImportedTrx, colAllMatchedTrx As IEnumerable(Of Trx), ByRef colUnusedMatches As ICollection(Of Integer), ByRef blnExactMatch As Boolean) Implements IImportHandler.BatchUpdateSearch
        Dim lngNumber As Integer = Val(objImportedTrx.strNumber)
        Dim colMatches As ICollection(Of Integer) = Nothing
        Dim colExactMatches As ICollection(Of Integer) = Nothing
        objReg.MatchCore(lngNumber, objImportedTrx.datDate, 120, objImportedTrx.strDescription, objImportedTrx.curAmount,
                         objImportedTrx.curMatchMin, objImportedTrx.curMatchMax, False, colMatches, colExactMatches, blnExactMatch)
        objReg.PruneToExactMatches(colExactMatches, objImportedTrx.datDate, colMatches, blnExactMatch)
        colUnusedMatches = ImportUtilities.colRemoveAlreadyMatched(objReg, colMatches, colAllMatchedTrx)
        colUnusedMatches = ImportUtilities.colApplyNarrowMethodForBank(objReg, objImportedTrx, colMatches, blnExactMatch)
    End Sub

    Public ReadOnly Property strBatchUpdateFields As String Implements IImportHandler.strBatchUpdateFields
        Get
            Return "without changing transaction numbers or transaction dates"
        End Get
    End Property

    Public ReadOnly Property blnAllowBatchUpdates As Boolean Implements IImportHandler.blnAllowBatchUpdates
        Get
            Return True
        End Get
    End Property
End Class