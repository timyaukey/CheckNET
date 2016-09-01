Option Strict Off
Option Explicit On

Imports System.IO
Imports CheckBookLib

Public Class ImportByPayee
    Implements ITrxImport

    'An ITrxImport that reads from the clipboard.
    'Each line of text is one Trx.
    'Each line contains three parts separated by a single blank: Date, description, amount.

    Private mobjInput As TextReader
    Private mstrFile As String
    Private mastrLines() As String
    Private mintNextIndex As Short

    Public Sub New(ByVal objInput As TextReader, ByVal strFile As String)
        mobjInput = objInput
        mstrFile = strFile
    End Sub

    Private Function ITrxImport_blnOpenSource(ByVal objAccount_ As Account) As Boolean Implements ITrxImport.blnOpenSource
        Dim strData As String

        strData = mobjInput.ReadToEnd()
        mastrLines = gaSplit(strData, vbNewLine)
        mintNextIndex = LBound(mastrLines)
        ITrxImport_blnOpenSource = True
    End Function

    Private Sub ITrxImport_CloseSource() Implements ITrxImport.CloseSource

    End Sub

    Private Function ITrxImport_objNextTrx() As ImportedTrx Implements ITrxImport.objNextTrx
        Dim objTrx As ImportedTrx
        Dim strLine As String
        Dim astrParts() As String
        Dim datDate As Date
        Dim strDescription As String
        Dim curAmount As Decimal
        Dim datNull As Date

        If mintNextIndex > UBound(mastrLines) Then
            'UPGRADE_NOTE: Object ITrxImport_objNextTrx may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            ITrxImport_objNextTrx = Nothing
            Exit Function
        End If

        strLine = mastrLines(mintNextIndex)
        mintNextIndex = mintNextIndex + 1

        astrParts = gaSplit(strLine, " ")
        If UBound(astrParts) <> 2 Then
            'UPGRADE_NOTE: Object ITrxImport_objNextTrx may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            ITrxImport_objNextTrx = Nothing
            Exit Function
        End If

        datDate = CDate(astrParts(0))
        strDescription = Replace(astrParts(1), "_", " ")
        curAmount = CDec(astrParts(2))

        objTrx = New ImportedTrx

        objTrx.NewStartNormal(Nothing, "", datDate, strDescription, "", Trx.TrxStatus.glngTRXSTS_UNREC, New TrxGenImportData())
        objTrx.AddSplit("", "", "", "", datNull, datNull, "", "", curAmount, "")

        ITrxImport_objNextTrx = objTrx
    End Function

    Private ReadOnly Property ITrxImport_strSource() As String Implements ITrxImport.strSource
        Get
            ITrxImport_strSource = mstrFile
        End Get
    End Property
End Class