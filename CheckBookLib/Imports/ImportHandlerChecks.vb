Option Strict On
Option Explicit On
Imports CheckBookLib

Public Class ImportHandlerChecks
    Implements IImportHandler

    Public ReadOnly Property blnAllowNew As Boolean Implements IImportHandler.blnAllowNew
        Get
            Return True
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
        Return objReg.objMatchPaymentDetails(objImportedTrx.strNumber, objImportedTrx.datDate, 10, objImportedTrx.strDescription, objImportedTrx.curAmount)
    End Function

    Public Sub BatchUpdate(objImportedTrx As ImportedTrx, objMatchedTrx As NormalTrx, ByVal intSeqNumber As Integer) Implements IImportHandler.BatchUpdate
        Dim curAmount As Decimal
        If intSeqNumber = 0 Then
            curAmount = objImportedTrx.curAmount
        Else
            curAmount = 0D
        End If
        objMatchedTrx.objReg.ImportUpdateNumAmt(objMatchedTrx.lngIndex, objImportedTrx.strNumber, curAmount)
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
            Return "updating transaction numbers and amounts"
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
        Dim colExactMatches As ICollection(Of NormalTrx) = Nothing
        Dim lngNumber As Integer
        If IsNumeric(objImportedTrx.strNumber) Then
            lngNumber = CInt(objImportedTrx.strNumber)
        Else
            lngNumber = 0
        End If
        objReg.MatchNormalCore(lngNumber, objImportedTrx.datDate, 60, 120, objImportedTrx.strDescription, objImportedTrx.curAmount,
            objImportedTrx.curMatchMin, objImportedTrx.curMatchMax, blnLooseMatch, colMatches, colExactMatches, blnExactMatch)
        SearchUtilities.PruneToExactMatches(colExactMatches, objImportedTrx.datDate, colMatches, blnExactMatch)
    End Sub

    Public Function blnIndividualUpdate(objImportedTrx As ImportedTrx, objMatchedTrx As NormalTrx) As Boolean Implements IImportHandler.blnIndividualUpdate
        objMatchedTrx.objReg.ImportUpdateNumAmt(objMatchedTrx.lngIndex, objImportedTrx.strNumber, objImportedTrx.curAmount)
        Return True
    End Function
End Class
