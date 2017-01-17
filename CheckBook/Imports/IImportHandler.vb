Option Strict Off
Option Explicit On

Imports VB = Microsoft.VisualBasic
Imports CheckBookLib

Public Interface IImportHandler
    Function lngStatusSearch(ByVal objImportedTrx As ImportedTrx, ByVal objReg As Register) As Integer
End Interface
