﻿Option Strict On
Option Explicit On


Public Class ImportHandlerDeposits
    Implements IImportHandler

    Private mobjHostUI As IHostUI

    Public Sub Init(ByVal objHostUI As IHostUI) Implements IImportHandler.Init
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property blnAllowNew As Boolean Implements IImportHandler.blnAllowNew
        Get
            Return False
        End Get
    End Property

    Public Sub AutoNewSearch(objImportedTrx As ImportedTrx, objReg As Register, ByRef colMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean) Implements IImportHandler.AutoNewSearch

    End Sub

    Public Function blnAlternateAutoNewHandling(objImportedTrx As ImportedTrx, objReg As Register) As Boolean Implements IImportHandler.blnAlternateAutoNewHandling
        Return False
    End Function

    Public Function strAutoNewValidationError(objImportedTrx As ImportedTrx, ByVal objAccount As Account, blnManualSelectionAllowed As Boolean) As String Implements IImportHandler.strAutoNewValidationError
        Return Nothing
    End Function

    Public Function objStatusSearch(objImportedTrx As ImportedTrx, objReg As Register) As NormalTrx Implements IImportHandler.objStatusSearch
        Dim colMatches As ICollection(Of NormalTrx) = Nothing
        Dim blnExactMatch As Boolean
        Dim objNormalTrx As NormalTrx
        objReg.MatchPayee(objImportedTrx.datDate, 7, objImportedTrx.strDescription, True, colMatches, blnExactMatch)
        If colMatches.Count > 0 Then
            objNormalTrx = Utilities.objFirstElement(colMatches)
            If Not objNormalTrx.blnFake Then
                Return objNormalTrx
            End If
        End If
        Return Nothing
    End Function

    Public Sub BatchUpdate(objImportedTrx As ImportedTrx, objMatchedTrx As NormalTrx, ByVal intMultiPartSeqNumber As Integer) Implements IImportHandler.BatchUpdate
        Dim curAmount As Decimal
        If intMultiPartSeqNumber = 0 Then
            curAmount = objImportedTrx.curAmount
        Else
            curAmount = 0D
        End If
        objMatchedTrx.objReg.ImportUpdateAmount(objMatchedTrx, curAmount)
    End Sub

    Public Sub BatchUpdateSearch(objReg As Register, objImportedTrx As ImportedTrx, colAllMatchedTrx As IEnumerable(Of NormalTrx), ByRef colUnusedMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean) Implements IImportHandler.BatchUpdateSearch
        Dim colMatches As ICollection(Of NormalTrx) = Nothing
        objReg.MatchPayee(objImportedTrx.datDate, 7, objImportedTrx.strDescription, False, colMatches, blnExactMatch)
        colUnusedMatches = ImportUtilities.colRemoveAlreadyMatched(objReg, colMatches, colAllMatchedTrx)
    End Sub

    Public ReadOnly Property strBatchUpdateFields As String Implements IImportHandler.strBatchUpdateFields
        Get
            Return "updating transaction amounts only"
        End Get
    End Property

    Public ReadOnly Property blnAllowBatchUpdates As Boolean Implements IImportHandler.blnAllowBatchUpdates
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property blnAllowIndividualUpdates As Boolean Implements IImportHandler.blnAllowIndividualUpdates
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property blnAllowMultiPartMatches As Boolean Implements IImportHandler.blnAllowMultiPartMatches
        Get
            Return False
        End Get
    End Property

    Public Sub IndividualSearch(objReg As Register, objImportedTrx As ImportedTrx, blnLooseMatch As Boolean, ByRef colMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean) Implements IImportHandler.IndividualSearch
        objReg.MatchPayee(objImportedTrx.datDate, 7, objImportedTrx.strDescription, False, colMatches, blnExactMatch)
    End Sub

    Public Function blnIndividualUpdate(objImportedTrx As ImportedTrx, objMatchedTrx As NormalTrx) As Boolean Implements IImportHandler.blnIndividualUpdate
        objMatchedTrx.objReg.ImportUpdateAmount(objMatchedTrx, objImportedTrx.curAmount)
        Return True
    End Function
End Class
