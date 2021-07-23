Option Strict On
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

    Public Sub AutoNewSearch(objImportedTrx As ImportedTrx, objReg As Register, ByRef colMatches As ICollection(Of BankTrx), ByRef blnExactMatch As Boolean) Implements IImportHandler.AutoNewSearch

    End Sub

    Public Function blnAlternateAutoNewHandling(objImportedTrx As ImportedTrx, objReg As Register) As Boolean Implements IImportHandler.blnAlternateAutoNewHandling
        Return False
    End Function

    Public Function strAutoNewValidationError(objImportedTrx As ImportedTrx, ByVal objAccount As Account, blnManualSelectionAllowed As Boolean) As String Implements IImportHandler.strAutoNewValidationError
        Return Nothing
    End Function

    Public Function objStatusSearch(objImportedTrx As ImportedTrx, objReg As Register) As BankTrx Implements IImportHandler.objStatusSearch
        Dim colMatches As ICollection(Of BankTrx) = Nothing
        Dim blnExactMatch As Boolean
        Dim objNormalTrx As BankTrx
        objReg.MatchPayee(objImportedTrx.TrxDate, 7, objImportedTrx.Description, True, colMatches, blnExactMatch)
        If colMatches.Count > 0 Then
            objNormalTrx = Utilities.objFirstElement(colMatches)
            If Not objNormalTrx.IsFake Then
                Return objNormalTrx
            End If
        End If
        Return Nothing
    End Function

    Public Sub BatchUpdate(objImportedTrx As ImportedTrx, objMatchedTrx As BankTrx, ByVal intMultiPartSeqNumber As Integer) Implements IImportHandler.BatchUpdate
        Dim curAmount As Decimal
        If intMultiPartSeqNumber = 0 Then
            curAmount = objImportedTrx.Amount
        Else
            curAmount = 0D
        End If
        objMatchedTrx.Register.ImportUpdateAmount(objMatchedTrx, curAmount)
    End Sub

    Public Sub BatchUpdateSearch(objReg As Register, objImportedTrx As ImportedTrx, colAllMatchedTrx As IEnumerable(Of BankTrx), ByRef colUnusedMatches As ICollection(Of BankTrx), ByRef blnExactMatch As Boolean) Implements IImportHandler.BatchUpdateSearch
        Dim colMatches As ICollection(Of BankTrx) = Nothing
        objReg.MatchPayee(objImportedTrx.TrxDate, 7, objImportedTrx.Description, False, colMatches, blnExactMatch)
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

    Public Sub IndividualSearch(objReg As Register, objImportedTrx As ImportedTrx, blnLooseMatch As Boolean, ByRef colMatches As ICollection(Of BankTrx), ByRef blnExactMatch As Boolean) Implements IImportHandler.IndividualSearch
        objReg.MatchPayee(objImportedTrx.TrxDate, 7, objImportedTrx.Description, False, colMatches, blnExactMatch)
    End Sub

    Public Function blnIndividualUpdate(objImportedTrx As ImportedTrx, objMatchedTrx As BankTrx) As Boolean Implements IImportHandler.blnIndividualUpdate
        objMatchedTrx.Register.ImportUpdateAmount(objMatchedTrx, objImportedTrx.Amount)
        Return True
    End Function
End Class
