Option Strict Off
Option Explicit On

Imports CheckBookLib

Public Class ImportByPayee
    Implements _ITrxImport

    'An ITrxImport that reads from the clipboard.
    'Each line of text is one Trx.
    'Each line contains three parts separated by a single blank: Date, description, amount.


    Private mastrLines() As String
    Private mintNextIndex As Short

    Private Function ITrxImport_blnOpenSource(ByVal objAccount_ As Account) As Boolean Implements _ITrxImport.blnOpenSource
        Dim strData As String

        'UPGRADE_ISSUE: Clipboard method Clipboard.GetText was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="076C26E5-B7A9-4E77-B69C-B4448DF39E58"'
        strData = Trim(My.Computer.Clipboard.GetText())
        mastrLines = gaSplit(strData, vbNewLine)
        mintNextIndex = LBound(mastrLines)
        ITrxImport_blnOpenSource = True
    End Function

    Private Sub ITrxImport_CloseSource() Implements _ITrxImport.CloseSource

    End Sub

    Private Function ITrxImport_objNextTrx() As Trx Implements _ITrxImport.objNextTrx
        Dim objTrx As Trx
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

        objTrx = New Trx

        objTrx.NewStartNormal(Nothing, "", datDate, strDescription, "", Trx.TrxStatus.glngTRXSTS_UNREC, False, 0.0#, False, False, 0, "", "")
        objTrx.AddSplit("", "", "", "", datNull, datNull, "", "", curAmount, "")

        ITrxImport_objNextTrx = objTrx
    End Function

    Private ReadOnly Property ITrxImport_strSource() As String Implements _ITrxImport.strSource
        Get
            ITrxImport_strSource = "(clipboard)"
        End Get
    End Property
End Class