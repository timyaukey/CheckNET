﻿Option Strict On
Option Explicit On

Public Class ImportHandlerBank
    Implements IImportHandler

    Public ReadOnly Property blnAllowNew As Boolean Implements IImportHandler.blnAllowNew
        Get
            Return True
        End Get
    End Property

    Public Sub AutoNewSearch(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register, ByRef colMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean) Implements IImportHandler.AutoNewSearch
        Dim lngNumber As Integer = 0
        Dim colExactMatches As ICollection(Of NormalTrx) = Nothing
        objReg.MatchNormalCore(lngNumber, objImportedTrx.datDate, 60, 60, objImportedTrx.strDescription, objImportedTrx.curAmount,
                                     objImportedTrx.curMatchMin, objImportedTrx.curMatchMax, False, colMatches, colExactMatches, blnExactMatch)
        SearchUtilities.PruneToExactMatches(colExactMatches, objImportedTrx.datDate, colMatches, blnExactMatch)
    End Sub

    Public Function blnAlternateAutoNewHandling(objImportedTrx As ImportedTrx, objReg As Register) As Boolean Implements IImportHandler.blnAlternateAutoNewHandling
        Return False
    End Function

    Public Function strAutoNewValidationError(objImportedTrx As ImportedTrx, ByVal objAccount As Account, blnManualSelectionAllowed As Boolean) As String Implements IImportHandler.strAutoNewValidationError
        If blnManualSelectionAllowed Then
            Return Nothing
        End If
        Dim intCompareLen As Integer = 8
        Dim strImportName As String = objImportedTrx.strDescription
        For Each objReg As Register In objAccount.colRegisters
            For Each objTrx As Trx In objReg.colDbgRepeatTrx.Values
                If String.Compare(objTrx.strDescription, 0, strImportName, 0, intCompareLen, True) = 0 Then
                    Return "There is a repeating transaction with a similar name"
                End If
            Next
        Next
        Return Nothing
    End Function

    Public Function objStatusSearch(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register) As NormalTrx Implements IImportHandler.objStatusSearch
        If objImportedTrx.strImportKey <> "" Then
            Return objReg.objMatchImportKey(objImportedTrx.strImportKey)
        End If
        Return Nothing
    End Function

    Public Sub BatchUpdate(objImportedTrx As ImportedTrx, objMatchedTrx As NormalTrx) Implements IImportHandler.BatchUpdate
        objMatchedTrx.objReg.ImportUpdateBank(objMatchedTrx.lngIndex, objImportedTrx.datDate, objMatchedTrx.strNumber, objImportedTrx.curAmount, objImportedTrx.strImportKey)
    End Sub

    Public Sub BatchUpdateSearch(objReg As Register, objImportedTrx As ImportedTrx, colAllMatchedTrx As IEnumerable(Of NormalTrx), ByRef colUnusedMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean) Implements IImportHandler.BatchUpdateSearch
        Dim lngNumber As Integer = CType(Val(objImportedTrx.strNumber), Integer)
        Dim colMatches As ICollection(Of NormalTrx) = Nothing
        Dim colExactMatches As ICollection(Of NormalTrx) = Nothing
        objReg.MatchNormalCore(lngNumber, objImportedTrx.datDate, 120, 120, objImportedTrx.strDescription, objImportedTrx.curAmount,
                         objImportedTrx.curMatchMin, objImportedTrx.curMatchMax, False, colMatches, colExactMatches, blnExactMatch)
        SearchUtilities.PruneToExactMatches(colExactMatches, objImportedTrx.datDate, colMatches, blnExactMatch)
        colUnusedMatches = ImportUtilities.colRemoveAlreadyMatched(objReg, colMatches, colAllMatchedTrx)
        colUnusedMatches = ImportUtilities.colApplyNarrowMethod(objReg, objImportedTrx, colUnusedMatches, blnExactMatch)
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

    Public ReadOnly Property blnAllowIndividualUpdates As Boolean Implements IImportHandler.blnAllowIndividualUpdates
        Get
            Return True
        End Get
    End Property

    Public Sub IndividualSearch(objReg As Register, objImportedTrx As ImportedTrx, blnLooseMatch As Boolean, ByRef colMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean) Implements IImportHandler.IndividualSearch
        Dim colExactMatches As ICollection(Of NormalTrx) = Nothing
        Dim lngNumber As Integer
        If IsNumeric(objImportedTrx.strNumber) Then
            lngNumber = CInt(objImportedTrx.strNumber)
        Else
            lngNumber = 0
        End If
        objReg.MatchNormalCore(lngNumber, objImportedTrx.datDate, 90, 120, objImportedTrx.strDescription, objImportedTrx.curAmount,
            objImportedTrx.curMatchMin, objImportedTrx.curMatchMax, blnLooseMatch, colMatches, colExactMatches, blnExactMatch)
        SearchUtilities.PruneToNonImportedExactMatches(colExactMatches, objImportedTrx.datDate, colMatches, blnExactMatch)
    End Sub

    Public Function blnIndividualUpdate(objImportedTrx As ImportedTrx, objMatchedTrx As NormalTrx) As Boolean Implements IImportHandler.blnIndividualUpdate
        Dim blnPreserveNumAmt As Boolean
        Dim strNewNumber As String
        Dim curNewAmount As Decimal
        blnPreserveNumAmt = (Not objMatchedTrx.blnFake) And objImportedTrx.blnFake
        If (objImportedTrx.curAmount <> objMatchedTrx.curAmount) And Not blnPreserveNumAmt Then
            If MsgBox("NOTE: The amount of the imported transaction is " & "different than the amount of the match you selected. " &
                "Updating the matched transaction will change its amount to " & "equal the amount of the import." & vbCrLf & vbCrLf &
                "Do you really want to do this?", MsgBoxStyle.OkCancel Or MsgBoxStyle.DefaultButton2) <> MsgBoxResult.Ok Then
                MsgBox("Update cancelled.", MsgBoxStyle.Information)
                Return False
            End If
        End If
        strNewNumber = objImportedTrx.strNumber
        curNewAmount = objImportedTrx.curAmount
        If blnPreserveNumAmt Then
            strNewNumber = objMatchedTrx.strNumber
            curNewAmount = objMatchedTrx.curAmount
        End If
        objMatchedTrx.objReg.ImportUpdateBank(objMatchedTrx.lngIndex, objImportedTrx.datDate, strNewNumber, curNewAmount, objImportedTrx.strImportKey)
        Return True
    End Function
End Class
