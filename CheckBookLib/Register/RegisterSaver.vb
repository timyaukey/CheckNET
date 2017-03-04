Option Strict On
Option Explicit On

Public Class RegisterSaver
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Write lines for non-fake Trx to this file.
    Private mintRealFile As Integer
    'Append lines for fake non-generated Trx to this Collection.
    Private mcolFakeLines As ICollection(Of String)
    'True iff saving a Register to create generated Trx from.
    'Controls how a few Trx properties are saved, like budget ending date,
    'and whether properties specific to repeating Trx are saved (unit type,
    'number of units, etc.)
    Private mblnForGenerating As Boolean
    'The Trx currently being saved.
    Private mblnFake As Boolean
    Private mobjTrx As Trx

    Public Sub Save(ByVal objReg_ As Register, ByVal intRealFile_ As Integer, ByVal colFakeLines_ As ICollection(Of String))

        Dim lngIndex As Integer

        mintRealFile = intRealFile_
        mcolFakeLines = colFakeLines_
        mblnForGenerating = objReg_.blnRepeat
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
        Select Case mobjTrx.lngType
            Case Trx.TrxType.glngTRXTYP_NORMAL
                SaveTrxNormal()
            Case Trx.TrxType.glngTRXTYP_BUDGET
                SaveTrxBudget()
            Case Trx.TrxType.glngTRXTYP_TRANSFER
                SaveTrxTransfer()
        End Select
        SaveLine(".T")
    End Sub

    Private Sub SaveTrxNormal()
        Dim objSplit As TrxSplit

        SaveTrxShared("TN")
        With mobjTrx
            If Len(.strNumber) > 0 Then
                SaveLine("N#" & .strNumber)
            End If
            If Len(.strImportKey) > 0 Then
                SaveLine("KI" & .strImportKey)
            End If
            If .curNormalMatchRange <> 0 Then
                SaveLine("MR" & gstrFormatCurrency(.curNormalMatchRange))
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
                        SaveLine("SI" & gstrFormatDate(.datInvoiceDate))
                    End If
                    If .datDueDate <> System.DateTime.FromOADate(0) Then
                        SaveLine("SD" & gstrFormatDate(.datDueDate))
                    End If
                    If Len(.strTerms) > 0 Then
                        SaveLine("ST" & .strTerms)
                    End If
                    If Len(.strBudgetKey) > 0 Then
                        SaveLine("SB" & .strBudgetKey)
                    End If
                    SaveLine("SA" & gstrFormatCurrency(.curAmount))
                    If Len(.strImageFiles) > 0 Then
                        SaveLine("SF" & .strImageFiles)
                    End If
                    SaveLine("SZ")
                End With
            Next objSplit
        End With
    End Sub

    Private Sub SaveTrxBudget()
        SaveTrxShared("TB")
        With mobjTrx
            If mblnForGenerating Then
                SaveLine("BU" & strConvertRepeatUnit(.lngBudgetPeriodUnit))
                SaveLine("BN" & .intBudgetPeriodNumber)
            Else
                SaveLine("BE" & gstrFormatDate(.datBudgetEnds))
            End If
            SaveLine("KB" & .strBudgetKey)
            SaveLine("A$" & gstrFormatCurrency(.curBudgetLimit))
        End With
    End Sub

    Private Sub SaveTrxTransfer()
        SaveTrxShared("TT")
        With mobjTrx
            SaveLine("KT" & .strTransferKey)
            SaveLine("A$" & gstrFormatCurrency(.curTransferAmount))
        End With
    End Sub

    Private Sub SaveTrxShared(ByVal strFirstCmd As String)
        With mobjTrx
            SaveLine(strFirstCmd & Mid("URNS", .lngStatus, 1) & .strDescription)
            SaveLine("DT" & gstrFormatDate(.datDate))
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
            If mblnForGenerating Then
                SaveLine("GU" & strConvertRepeatUnit(.lngRptUnit))
                SaveLine("GN" & .intRptNumber)
                SaveLine("GE" & gstrFormatDate(.datRptEnd))
            End If
        End With
    End Sub

    Private Function strConvertRepeatUnit(ByVal lngUnit As Trx.RepeatUnit) As String
        strConvertRepeatUnit = ""
        Select Case lngUnit
            Case Trx.RepeatUnit.glngRPTUNT_DAY
                strConvertRepeatUnit = "DAY"
            Case Trx.RepeatUnit.glngRPTUNT_WEEK
                strConvertRepeatUnit = "WEEK"
            Case Trx.RepeatUnit.glngRPTUNT_MONTH
                strConvertRepeatUnit = "MONTH"
            Case Else
                gRaiseError("Unrecognized repeat unit: " & lngUnit)
        End Select
    End Function

    Private Sub SaveLine(ByRef strLine As String)
        If mblnFake Then
            mcolFakeLines.Add(strLine)
        Else
            PrintLine(mintRealFile, strLine)
        End If
    End Sub
End Class