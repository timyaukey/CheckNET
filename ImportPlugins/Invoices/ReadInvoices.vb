Option Strict On
Option Explicit On

Imports System.IO

Public Class ReadInvoices
    Implements ITrxReader

    Private mobjCompany As Company
    Private mobjInput As TextReader
    Private mstrFile As String
    Private mastrLines() As String
    Private mintNextIndex As Integer
    Private mobjAccount As Account

    Public Sub New(ByVal objCompany As Company, ByVal objInput As TextReader, ByVal strFile As String)
        mobjCompany = objCompany
        mobjInput = objInput
        mstrFile = strFile
    End Sub

    Private Function blnOpenSource(ByVal objAccount_ As Account) As Boolean Implements ITrxReader.blnOpenSource
        Dim strData As String

        strData = mobjInput.ReadToEnd()
        mastrLines = Utilities.Split(strData, vbNewLine)
        mintNextIndex = LBound(mastrLines)
        mobjAccount = objAccount_
        blnOpenSource = True
    End Function

    Private Sub CloseSource() Implements ITrxReader.CloseSource

    End Sub

    Private Function objNextTrx() As ImportedTrx Implements ITrxReader.objNextTrx
        Dim strLine As String
        Dim astrParts() As String

        Dim objTrx As ImportedTrx
        Dim datDate As Date
        Dim strDate As String
        Dim strDescription As String
        Dim curAmount As Decimal
        Dim strAmount As String
        Dim strPONumber As String
        Dim strInvNumber As String
        Dim datInvDate As Date
        Dim strTerms As String
        Dim datDueDate As Date
        Dim strDueDate As String
        Dim strDocType As String
        Dim strTrxNum As String
        Dim intCatIdx As Integer
        Dim strCatName As String
        Dim strCatKey As String
        Dim blnUseFakeInvoices As Boolean

        If mintNextIndex > UBound(mastrLines) Then
            objNextTrx = Nothing
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
        strLine = Trim(strLine)
        astrParts = Utilities.Split(strLine, vbTab)
        If astrParts.Length < 9 Then
            Throw New ImportReadException("Input line has less than nine tab delimited fields: " + strLine)
        End If

        strDescription = Trim(astrParts(0))
        If String.IsNullOrEmpty(strDescription) Then
            Throw New ImportReadException("Description/vendor is required in column 1")
        End If
        strPONumber = Trim(astrParts(1))
        strDate = Trim(astrParts(2))
        If Not Utilities.blnIsValidDate(strDate) Then
            Throw New ImportReadException("Invalid invoice date in column 3")
        End If
        datInvDate = CDate(strDate)
        datDate = datInvDate
        strInvNumber = Trim(astrParts(3))
        strTerms = Trim(astrParts(4))
        strDueDate = Trim(astrParts(5))
        If String.IsNullOrEmpty(strDueDate) Then
            datDueDate = Utilities.datEmpty
        Else
            If Not Utilities.blnIsValidDate(strDueDate) Then
                Throw New ImportReadException("Invalid due date in column 6")
            End If
            datDueDate = CDate(strDueDate)
        End If
        strAmount = Trim(astrParts(6))
        If Not Utilities.blnIsValidAmount(strAmount) Then
            Throw New ImportReadException("Invalid amount in column 7")
        End If
        curAmount = CDec(strAmount)
        strDocType = Trim(astrParts(7))
        If Left(strDocType, 2).ToLower() = "in" Then
            strTrxNum = "Inv"
            curAmount = -curAmount
        ElseIf Left(strDocType, 2).ToLower() = "cr" Then
            strTrxNum = "Crm"
        Else
            Throw New ImportReadException("Invalid document type in column 8")
        End If
        strCatName = Trim(astrParts(8))
        intCatIdx = mobjCompany.objCategories.intLookupValue1(strCatName)
        If intCatIdx = 0 Then
            Throw New ImportReadException("Unrecognized category name - " + strCatName)
        End If
        strCatKey = mobjCompany.objCategories.strKey(intCatIdx)

        objTrx = New ImportedTrx(Nothing)
        blnUseFakeInvoices = (mobjAccount.lngSubType <> Account.SubType.Liability_AccountsPayable)
        objTrx.NewStartNormal(False, strTrxNum, datDate, strDescription, "", BaseTrx.TrxStatus.Unreconciled,
                              TrxGenImportData.NewFake(blnUseFakeInvoices))
        objTrx.AddSplit("", strCatKey, strPONumber, strInvNumber, datInvDate, datDueDate, strTerms, "", curAmount)

        objNextTrx = objTrx
    End Function

    Private ReadOnly Property strSource() As String Implements ITrxReader.strSource
        Get
            strSource = mstrFile
        End Get
    End Property
End Class