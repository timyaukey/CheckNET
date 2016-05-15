Option Strict Off
Option Explicit On
Public Class RegisterSaver
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    'Write lines for non-fake Trx to this file.
    Private mintRealFile As Short
    'Append lines for fake non-generated Trx to this Collection.
    Private mcolFakeLines As Collection
    'True iff saving a Register to create generated Trx from.
    'Controls how a few Trx properties are saved, like budget ending date,
    'and whether properties specific to repeating Trx are saved (unit type,
    'number of units, etc.)
    Private mblnForGenerating As Boolean
    'The Trx currently being saved.
    Private mblnFake As Boolean
    Private mobjTrx As Trx

    Public Sub Save(ByVal objReg_ As Register, ByVal intRealFile_ As Short, ByVal colFakeLines_ As Collection)

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
        Dim objSplit As Split_Renamed

        SaveTrxShared("TN")
        With mobjTrx
            If Len(.strNumber) Then
                SaveLine("N#" & .strNumber)
            End If
            If Len(.strImportKey) Then
                SaveLine("KI" & .strImportKey)
            End If
            If .curNormalMatchRange <> 0 Then
                SaveLine("MR" & gstrVB6Format(.curNormalMatchRange, gstrFORMAT_CURRENCY))
            End If
            For Each objSplit In .colSplits
                With objSplit
                    SaveLine("SC" & .strCategoryKey)
                    If Len(.strMemo) Then
                        SaveLine("SM" & .strMemo)
                    End If
                    If Len(.strPONumber) Then
                        SaveLine("SP" & .strPONumber)
                    End If
                    If Len(.strInvoiceNum) Then
                        SaveLine("SN" & .strInvoiceNum)
                    End If
                    If .datInvoiceDate <> System.DateTime.FromOADate(0) Then
                        SaveLine("SI" & gstrVB6Format(.datInvoiceDate, gstrFORMAT_DATE))
                    End If
                    If .datDueDate <> System.DateTime.FromOADate(0) Then
                        SaveLine("SD" & gstrVB6Format(.datDueDate, gstrFORMAT_DATE))
                    End If
                    If Len(.strTerms) Then
                        SaveLine("ST" & .strTerms)
                    End If
                    If Len(.strBudgetKey) Then
                        SaveLine("SB" & .strBudgetKey)
                    End If
                    SaveLine("SA" & gstrVB6Format(.curAmount, gstrFORMAT_CURRENCY))
                    If Len(.strImageFiles) Then
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
                SaveLine("BE" & gstrVB6Format(.datBudgetEnds, gstrFORMAT_DATE))
            End If
            SaveLine("KB" & .strBudgetKey)
            SaveLine("A$" & gstrVB6Format(.curBudgetLimit, gstrFORMAT_CURRENCY))
        End With
    End Sub

    Private Sub SaveTrxTransfer()
        SaveTrxShared("TT")
        With mobjTrx
            SaveLine("KT" & .strTransferKey)
            SaveLine("A$" & gstrVB6Format(.curTransferAmount, gstrFORMAT_CURRENCY))
        End With
    End Sub

    Private Sub SaveTrxShared(ByVal strFirstCmd As String)
        With mobjTrx
            SaveLine(strFirstCmd & Mid("URNS", .lngStatus, 1) & .strDescription)
            SaveLine("DT" & gstrVB6Format(.datDate, gstrFORMAT_DATE))
            If Len(.strMemo) Then
                SaveLine("ME" & .strMemo)
            End If
            If .blnAwaitingReview Then
                SaveLine("AR")
            End If
            If .intRepeatSeq <> 0 Then
                SaveLine("RS" & .intRepeatSeq)
            End If
            If Len(.strRepeatKey) Then
                SaveLine("KR" & .strRepeatKey)
            End If
            If mblnForGenerating Then
                SaveLine("GU" & strConvertRepeatUnit(.lngRptUnit))
                SaveLine("GN" & .intRptNumber)
                SaveLine("GE" & gstrVB6Format(.datRptEnd, gstrFORMAT_DATE))
            End If
        End With
    End Sub

    Private Function strConvertRepeatUnit(ByVal lngUnit As Trx.RepeatUnit) As String
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