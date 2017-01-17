Option Strict Off
Option Explicit On

Imports CheckBookLib

Public Class ImportHandlerBank
    Implements IImportHandler

    Public Function lngStatusSearch(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register) As Integer Implements IImportHandler.lngStatusSearch
        If objImportedTrx.strImportKey <> "" Then
            Return objReg.lngMatchImportKey(objImportedTrx.strImportKey)
        End If
        Return 0
    End Function

End Class
