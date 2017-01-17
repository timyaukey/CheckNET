Option Strict Off
Option Explicit On

Imports CheckBookLib

Public Class ImportHandlerChecks
    Implements IImportHandler

    Public Function lngStatusSearch(objImportedTrx As ImportedTrx, objReg As Register) As Integer Implements IImportHandler.lngStatusSearch
        Return objReg.lngMatchPaymentDetails(objImportedTrx.strNumber, objImportedTrx.datDate, 10, objImportedTrx.strDescription, objImportedTrx.curAmount)
    End Function
End Class
