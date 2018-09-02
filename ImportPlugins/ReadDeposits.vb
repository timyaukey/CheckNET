Option Strict On
Option Explicit On

Imports CheckBookLib
Imports System.IO

Public Class ReadDeposits
    Implements ITrxReader

    'An ITrxReader that reads from the clipboard.
    'Each line of text is one Trx.
    'Each line contains three parts separated by a single blank: Date, description, amount.

    Private mobjInput As TextReader
    Private mstrFile As String
    Private mastrLines() As String
    Private mintNextIndex As Integer

    Public Sub New(ByVal objInput As TextReader, ByVal strFile As String)
        mobjInput = objInput
        mstrFile = strFile
    End Sub

    Private Function blnOpenSource(ByVal objAccount_ As Account) As Boolean Implements ITrxReader.blnOpenSource
        Dim strData As String

        strData = mobjInput.ReadToEnd()
        mastrLines = Utilities.Split(strData, vbNewLine)
        mintNextIndex = LBound(mastrLines)
        blnOpenSource = True
    End Function

    Private Sub CloseSource() Implements ITrxReader.CloseSource

    End Sub

    Private Function objNextTrx() As ImportedTrx Implements ITrxReader.objNextTrx
        Dim objTrx As ImportedTrx
        Dim strLine As String
        Dim astrParts() As String
        Dim datDate As Date
        Dim strDescription As String
        Dim curAmount As Decimal
        Dim datNull As Date

        If mintNextIndex > UBound(mastrLines) Then
            'UPGRADE_NOTE: Object ITrxImport_objNextTrx may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            objNextTrx = Nothing
            Exit Function
        End If

        strLine = mastrLines(mintNextIndex)
        mintNextIndex = mintNextIndex + 1

        astrParts = Utilities.Split(strLine, " ")
        If UBound(astrParts) <> 2 Then
            'UPGRADE_NOTE: Object ITrxImport_objNextTrx may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            objNextTrx = Nothing
            Exit Function
        End If

        datDate = CDate(astrParts(0))
        strDescription = Replace(astrParts(1), "_", " ")
        curAmount = CDec(astrParts(2))

        objTrx = New ImportedTrx(Nothing)

        objTrx.NewStartNormal(False, "", datDate, strDescription, "", Trx.TrxStatus.Unreconciled, New TrxGenImportData())
        objTrx.AddSplit("", "", "", "", datNull, datNull, "", "", curAmount)

        objNextTrx = objTrx
    End Function

    Private ReadOnly Property strSource() As String Implements ITrxReader.strSource
        Get
            strSource = mstrFile
        End Get
    End Property
End Class