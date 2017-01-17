Option Strict Off
Option Explicit On
Imports CheckBook
Imports CheckBookLib

Public Class ImportHandlerInvoices
    Implements IImportHandler

    Public ReadOnly Property blnAllowNew As Boolean Implements IImportHandler.blnAllowNew
        Get
            Return True
        End Get
    End Property

    Public Sub AutoNewSearch(objImportedTrx As ImportedTrx, objReg As Register, ByRef colMatches As ICollection(Of Integer), ByRef blnExactMatch As Boolean) Implements IImportHandler.AutoNewSearch
        objReg.MatchInvoice(objImportedTrx.datDate, 120, objImportedTrx.strDescription, objImportedTrx.objFirstSplit.strInvoiceNum, colMatches)
        blnExactMatch = True
    End Sub

    Public Function blnAlternateAutoNewHandling(objImportedTrx As ImportedTrx, objReg As Register) As Boolean Implements IImportHandler.blnAlternateAutoNewHandling
        Dim objImportedSplit As TrxSplit
        Dim colPOMatches As ICollection(Of Integer) = Nothing
        Dim vlngMatchedTrxIndex As Object
        Dim objMatchedTrx As Trx
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
                For Each vlngMatchedTrxIndex In colPOMatches
                    objMatchedTrx = objReg.objTrx(vlngMatchedTrxIndex)
                    For Each objMatchedSplit In objMatchedTrx.colSplits
                        If objMatchedSplit.strPONumber = strPONumber And objMatchedSplit.strInvoiceNum = "" Then
                            'Add the imported Trx as a new split in objMatchedTrx,
                            'and reduce the amount of objMatchedSplit by the same amount
                            'so the total amount of objMatchedTrx does not change.
                            objReg.ImportUpdatePurchaseOrder(vlngMatchedTrxIndex, objMatchedSplit, objImportedSplit)
                            Return True
                        End If
                    Next objMatchedSplit
                Next vlngMatchedTrxIndex
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

    Public Function lngStatusSearch(objImportedTrx As ImportedTrx, objReg As Register) As Integer Implements IImportHandler.lngStatusSearch
        Dim colMatches As ICollection(Of Integer) = Nothing
        objReg.MatchInvoice(objImportedTrx.datDate, 120, objImportedTrx.strDescription, objImportedTrx.objFirstSplit.strInvoiceNum, colMatches)
        If colMatches.Count() > 0 Then
            Return gdatFirstElement(colMatches)
        End If
        Return 0
    End Function

End Class
