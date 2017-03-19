Option Strict On
Option Explicit On

Public Class ImportHandlerInvoices
    Implements IImportHandler

    Public ReadOnly Property blnAllowNew As Boolean Implements IImportHandler.blnAllowNew
        Get
            Return True
        End Get
    End Property

    Public Sub AutoNewSearch(objImportedTrx As ImportedTrx, objReg As Register, ByRef colMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean) Implements IImportHandler.AutoNewSearch
        objReg.MatchInvoice(objImportedTrx.datDate, 120, objImportedTrx.strDescription, objImportedTrx.objFirstSplit.strInvoiceNum, colMatches)
        blnExactMatch = True
    End Sub

    Public Function blnAlternateAutoNewHandling(objImportedTrx As ImportedTrx, objReg As Register) As Boolean Implements IImportHandler.blnAlternateAutoNewHandling
        Dim objImportedSplit As TrxSplit
        Dim colPOMatches As ICollection(Of NormalTrx) = Nothing
        Dim vlngMatchedTrxIndex As Integer
        Dim objMatchedTrx As NormalTrx
        Dim objMatchedSplit As TrxSplit
        Dim strPONumber As String
        'Check if we are importing an invoice that can be matched to a purchase order.
        'If this happens we update an existing Trx by adding a split rather than creating
        'a new Trx as would normally be the case in this method.
        If objImportedTrx.lngSplits > 0 Then
            objImportedSplit = objImportedTrx.objFirstSplit
            strPONumber = objImportedSplit.strPONumber
            If LCase(strPONumber) = "none" Then
                strPONumber = ""
            End If
            If strPONumber <> "" Then
                objReg.MatchPONumber(objImportedTrx.datDate, 14, objImportedTrx.strDescription, strPONumber, colPOMatches)
                'There should be only one matching Trx, but we'll check all matches
                'and use the first one with a split with no invoice number. That split
                'represents the uninvoiced part of the purchase order due on that date.
                For Each objMatchedTrx In colPOMatches
                    For Each objMatchedSplit In objMatchedTrx.colSplits
                        If objMatchedSplit.strPONumber = strPONumber And objMatchedSplit.strInvoiceNum = "" Then
                            'Add the imported Trx as a new split in objMatchedTrx,
                            'and reduce the amount of objMatchedSplit by the same amount
                            'so the total amount of objMatchedTrx does not change.
                            objReg.ImportUpdatePurchaseOrder(vlngMatchedTrxIndex, objMatchedSplit, objImportedSplit)
                            Return True
                        End If
                    Next objMatchedSplit
                Next
            End If
        End If
        Return False
    End Function

    Public Function strAutoNewValidationError(objImportedTrx As ImportedTrx, blnAllowBankNonCard As Boolean) As String Implements IImportHandler.strAutoNewValidationError
        Dim strTrxNum As String = LCase(objImportedTrx.strNumber)
        If strTrxNum <> "inv" And strTrxNum <> "crm" Then
            Return "Transaction is not an invoice or credit memo"
        End If
        Return Nothing
    End Function

    Public Function objStatusSearch(objImportedTrx As ImportedTrx, objReg As Register) As NormalTrx Implements IImportHandler.objStatusSearch
        Dim colMatches As ICollection(Of NormalTrx) = Nothing
        objReg.MatchInvoice(objImportedTrx.datDate, 120, objImportedTrx.strDescription, objImportedTrx.objFirstSplit.strInvoiceNum, colMatches)
        If colMatches.Count() > 0 Then
            Return gdatFirstElement(colMatches)
        End If
        Return Nothing
    End Function

    Public Sub BatchUpdate(objImportedTrx As ImportedTrx, objMatchedTrx As NormalTrx) Implements IImportHandler.BatchUpdate
        'Do nothing for invoices.
    End Sub

    Public Sub BatchUpdateSearch(objReg As Register, objImportedTrx As ImportedTrx, colImportMatchedTrx As IEnumerable(Of NormalTrx), ByRef colUnusedMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean) Implements IImportHandler.BatchUpdateSearch
        'Do nothing
    End Sub

    Public ReadOnly Property strBatchUpdateFields As String Implements IImportHandler.strBatchUpdateFields
        Get
            Return ""  'Never finds anything to update, so not needed.
        End Get
    End Property

    Public ReadOnly Property blnAllowBatchUpdates As Boolean Implements IImportHandler.blnAllowBatchUpdates
        Get
            Return False
        End Get
    End Property

    Public ReadOnly Property blnAllowIndividualUpdates As Boolean Implements IImportHandler.blnAllowIndividualUpdates
        Get
            Return False
        End Get
    End Property

    Public Sub IndividualSearch(objReg As Register, objImportedTrx As ImportedTrx, blnLooseMatch As Boolean, ByRef colMatches As ICollection(Of NormalTrx), ByRef blnExactMatch As Boolean) Implements IImportHandler.IndividualSearch
        objReg.MatchInvoice(objImportedTrx.datDate, 120, objImportedTrx.strDescription, objImportedTrx.objFirstSplit.strInvoiceNum, colMatches)
        blnExactMatch = True
    End Sub

    Public Function blnIndividualUpdate(objImportedTrx As ImportedTrx, objMatchedTrx As NormalTrx) As Boolean Implements IImportHandler.blnIndividualUpdate
        'Do nothing
        Return False
    End Function
End Class
