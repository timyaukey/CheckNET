Option Strict On
Option Explicit On

Public Class RegisterSaver
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private mobjReg As Register
    'Append lines for fake non-generated Trx to this Collection.
    Private mcolFakeLines As ICollection(Of String)
    'The Trx currently being saved.
    Private mblnFake As Boolean
    Private mobjTrx As Trx

    Public Sub Save(ByVal objReg_ As Register, ByVal colFakeLines_ As ICollection(Of String))

        Dim lngIndex As Integer

        mobjReg = objReg_
        mcolFakeLines = colFakeLines_
        For lngIndex = 1 To objReg_.lngTrxCount
            SaveTrx(objReg_.objTrx(lngIndex))
        Next
    End Sub

    Private Sub SaveTrx(ByVal objTrx As Trx)
        mobjTrx = objTrx
        mblnFake = mobjTrx.blnFake
        If mobjTrx.blnAutoGenerated Then
            Exit Sub
        End If
        'Do not save ReplicaTrx, because they will be recreated on load.
        Select Case mobjTrx.lngType
            Case Trx.TrxType.Normal
                SaveTrxNormal()
                SaveLine(".T")
            Case Trx.TrxType.Budget
                SaveTrxBudget()
                SaveLine(".T")
            Case Trx.TrxType.Transfer
                SaveTrxTransfer()
                SaveLine(".T")
        End Select
    End Sub

    Private Sub SaveTrxNormal()
        Dim objSplit As TrxSplit

        SaveTrxShared("TN")
        With DirectCast(mobjTrx, NormalTrx)
            If Len(.strNumber) > 0 Then
                SaveLine("N#" & .strNumber)
            End If
            If Len(.strImportKey) > 0 Then
                SaveLine("KI" & .strImportKey)
            End If
            If .curNormalMatchRange <> 0 Then
                SaveLine("MR" & Utilities.strFormatCurrency(.curNormalMatchRange))
            End If
            For Each objSplit In .colSplits
                With objSplit
                    SaveLine("SC" & .strCategoryKey)
                    If Len(.strMemo) > 0 Then
                        SaveLine("SM" & .strMemo)
                    End If
                    If Len(.strPONumber) > 0 Then
                        SaveLine("SP" & .strPONumber)
                    End If
                    If Len(.strInvoiceNum) > 0 Then
                        SaveLine("SN" & .strInvoiceNum)
                    End If
                    If .datInvoiceDate <> System.DateTime.FromOADate(0) Then
                        SaveLine("SI" & Utilities.strFormatDate(.datInvoiceDate))
                    End If
                    If .datDueDate <> System.DateTime.FromOADate(0) Then
                        SaveLine("SD" & Utilities.strFormatDate(.datDueDate))
                    End If
                    If Len(.strTerms) > 0 Then
                        SaveLine("ST" & .strTerms)
                    End If
                    If Len(.strBudgetKey) > 0 Then
                        SaveLine("SB" & .strBudgetKey)
                    End If
                    SaveLine("SA" & Utilities.strFormatCurrency(.curAmount))
                    SaveLine("SZ")
                End With
            Next objSplit
        End With
    End Sub

    Private Sub SaveTrxBudget()
        SaveTrxShared("TB")
        With DirectCast(mobjTrx, BudgetTrx)
            SaveLine("BE" & Utilities.strFormatDate(.datBudgetStarts))
            SaveLine("KB" & .strBudgetKey)
            SaveLine("A$" & Utilities.strFormatCurrency(.curBudgetLimit))
        End With
    End Sub

    Private Sub SaveTrxTransfer()
        SaveTrxShared("TT")
        With DirectCast(mobjTrx, TransferTrx)
            SaveLine("KT" & .strTransferKey)
            SaveLine("A$" & Utilities.strFormatCurrency(.curTransferAmount))
        End With
    End Sub

    Private Sub SaveTrxShared(ByVal strFirstCmd As String)
        With mobjTrx
            SaveLine(strFirstCmd & Mid("URNS", .lngStatus, 1) & .strDescription)
            SaveLine("DT" & Utilities.strFormatDate(.datDate))
            If Len(.strMemo) > 0 Then
                SaveLine("ME" & .strMemo)
            End If
            If .blnAwaitingReview Then
                SaveLine("AR")
            End If
            If .intRepeatSeq <> 0 Then
                SaveLine("RS" & .intRepeatSeq)
            End If
            If Len(.strRepeatKey) > 0 Then
                SaveLine("KR" & .strRepeatKey)
            End If
        End With
    End Sub

    Private Function strConvertRepeatUnit(ByVal lngUnit As Trx.RepeatUnit) As String
        strConvertRepeatUnit = ""
        Select Case lngUnit
            Case Trx.RepeatUnit.Day
                strConvertRepeatUnit = "DAY"
            Case Trx.RepeatUnit.Week
                strConvertRepeatUnit = "WEEK"
            Case Trx.RepeatUnit.Month
                strConvertRepeatUnit = "MONTH"
            Case Else
                gRaiseError("Unrecognized repeat unit: " & lngUnit)
        End Select
    End Function

    Private Sub SaveLine(ByRef strLine As String)
        If mblnFake Then
            mcolFakeLines.Add(strLine)
        Else
            mobjReg.objAccount.SaveLine(strLine)
        End If
    End Sub
End Class