Option Strict Off
Option Explicit On

Imports CheckBookLib

Public Class ImportHandlerInvoices
    Implements IImportHandler

    Public Function lngStatusSearch(objImportedTrx As ImportedTrx, objReg As Register) As Integer Implements IImportHandler.lngStatusSearch

        Dim colMatches As ICollection(Of Integer) = Nothing
        objReg.MatchInvoice(objImportedTrx.datDate, 120, objImportedTrx.strDescription, objImportedTrx.objFirstSplit.strInvoiceNum, colMatches)
        If colMatches.Count() > 0 Then
            Return gdatFirstElement(colMatches)
        End If
        Return 0

    End Function
End Class
