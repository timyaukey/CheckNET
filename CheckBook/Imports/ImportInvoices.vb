Option Strict Off
Option Explicit On

Imports System.IO
Imports CheckBookLib

Public Class ImportInvoices
    Implements ITrxImport

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

    Private Function ITrxImport_objNextTrx() As Trx Implements ITrxImport.objNextTrx
        Dim strLine As String
        Dim astrParts() As String

        Dim objTrx As Trx
        Dim datDate As Date
        Dim strDescription As String
        Dim curAmount As Decimal
        Dim strPONumber As String
        Dim strInvNumber As String
        Dim datInvDate As Date
        Dim strTerms As String
        Dim datDueDate As Date
        Dim strDocType As String
        Dim strTrxNum As String
        Dim strCatName As String
        Dim strCatKey As String

        If mintNextIndex > UBound(mastrLines) Then
            'UPGRADE_NOTE: Object ITrxImport_objNextTrx may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            ITrxImport_objNextTrx = Nothing
            Exit Function
        End If

        strLine = mastrLines(mintNextIndex)
        mintNextIndex = mintNextIndex + 1
        'Copying from a Sharepoint custom list via the clipboard can result in extra
        'tab chars in the front of lines. We make the assumption the first field cannot
        'be empty, and therefore a leading tab is one of these extras and must be removed.
        If Left(strLine, 1) = vbTab Then
            strLine = Mid(strLine, 2, Len(strLine) - 1)
        End If
        astrParts = gaSplit(Trim(strLine), vbTab)

        strDescription = astrParts(0)
        strPONumber = astrParts(1)
        If astrParts(2) = "" Then
            datInvDate = System.DateTime.FromOADate(0)
        Else
            datInvDate = CDate(astrParts(2))
        End If
        strInvNumber = astrParts(3)
        strTerms = astrParts(4)
        datDate = CDate(astrParts(5))
        datDueDate = datDate
        curAmount = CDec(astrParts(6))
        strDocType = astrParts(7)
        If strDocType = "Invoice" Then
            strTrxNum = "Inv"
            curAmount = -curAmount
        Else
            strTrxNum = "Crm"
        End If
        strCatName = astrParts(8)
        strCatKey = gobjCategories.strKey(gobjCategories.intLookupValue1(strCatName))

        objTrx = New Trx

        objTrx.NewStartNormal(Nothing, strTrxNum, datDate, strDescription, "", Trx.TrxStatus.glngTRXSTS_UNREC, TrxGenImportData.NewFake(True))
        objTrx.AddSplit("", strCatKey, strPONumber, strInvNumber, datInvDate, datDueDate, strTerms, "", curAmount, "")

        ITrxImport_objNextTrx = objTrx
    End Function

    Private ReadOnly Property ITrxImport_strSource() As String Implements ITrxImport.strSource
        Get
            ITrxImport_strSource = mstrFile
        End Get
    End Property
End Class