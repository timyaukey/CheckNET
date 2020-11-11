Option Strict On
Option Explicit On

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
        Dim strDate As String
        Dim datDate As Date
        Dim strDescription As String
        Dim strAmount As String
        Dim curAmount As Decimal
        Dim datNull As Date

        If mintNextIndex > UBound(mastrLines) Then
            objNextTrx = Nothing
            Exit Function
        End If

        strLine = mastrLines(mintNextIndex)
        mintNextIndex = mintNextIndex + 1

        astrParts = Utilities.Split(strLine, vbTab)
        If astrParts.Length < 3 Then
            Throw New ImportReadException("Input line has less than three tab delimited fields: " + strLine)
        End If

        strDate = Trim(astrParts(0))
        If Not Utilities.blnIsValidDate(strDate) Then
            Throw New ImportReadException("Invalid deposit date in column 1")
        End If
        datDate = CDate(strDate)

        strDescription = Trim(astrParts(1))
        If String.IsNullOrEmpty(strDescription) Then
            Throw New ImportReadException("Description is required in column 2")
        End If

        strAmount = Trim(astrParts(2))
        If Not Utilities.blnIsValidAmount(strAmount) Then
            Throw New ImportReadException("Invalid deposit amount in column 3")
        End If
        curAmount = CDec(strAmount)

        objTrx = New ImportedTrx(Nothing)

        datNull = Utilities.datEmpty
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