Option Strict On
Option Explicit On

Imports System.IO

Public Class RegisterSaver
    '2345667890123456789012345678901234567890123456789012345678901234567890123456789012345

    Private mobjReg As Register
    'Append lines for fake non-generated BaseTrx to this Collection.
    Private mcolFakeLines As ICollection(Of String)
    'The BaseTrx currently being saved.
    Private mblnFake As Boolean
    Private mobjTrx As BaseTrx
    Private mobjSaveFile As StreamWriter

    Public Sub Save(ByVal objSaveFile_ As StreamWriter, ByVal objReg_ As Register, ByVal colFakeLines_ As ICollection(Of String))
        mobjSaveFile = objSaveFile_
        mobjReg = objReg_
        mcolFakeLines = colFakeLines_
        For Each objTrx As BaseTrx In objReg_.GetAllTrx(Of BaseTrx)()
            SaveTrx(objTrx)
        Next
    End Sub

    Private Sub SaveTrx(ByVal objTrx As BaseTrx)
        mobjTrx = objTrx
        mblnFake = mobjTrx.IsFake
        If mobjTrx.IsAutoGenerated Then
            Exit Sub
        End If
        'Do not save ReplicaTrx, because they will be recreated on load.
        If TypeOf mobjTrx Is BankTrx Then
            SaveTrxNormal()
            SaveLine(".T")
        ElseIf TypeOf mobjTrx Is BudgetTrx Then
            SaveTrxBudget()
            SaveLine(".T")
        ElseIf TypeOf mobjTrx Is TransferTrx Then
            SaveTrxTransfer()
            SaveLine(".T")
        End If
    End Sub

    Private Sub SaveTrxNormal()
        Dim objSplit As TrxSplit

        SaveTrxShared("TN")
        With DirectCast(mobjTrx, BankTrx)
            If Len(.Number) > 0 Then
                SaveLine("N#" & .Number)
            End If
            If Len(.ImportKey) > 0 Then
                SaveLine("KI" & .ImportKey)
            End If
            If .NormalMatchRange <> 0 Then
                SaveLine("MR" & Utilities.strFormatCurrency(.NormalMatchRange))
            End If
            For Each objSplit In .Splits
                With objSplit
                    SaveLine("SC" & .CategoryKey)
                    If Len(.Memo) > 0 Then
                        SaveLine("SM" & .Memo)
                    End If
                    If Len(.PONumber) > 0 Then
                        SaveLine("SP" & .PONumber)
                    End If
                    If Len(.InvoiceNum) > 0 Then
                        SaveLine("SN" & .InvoiceNum)
                    End If
                    If .InvoiceDate <> Utilities.datEmpty Then
                        SaveLine("SI" & Utilities.strFormatDate(.InvoiceDate))
                    End If
                    If .DueDate <> Utilities.datEmpty Then
                        SaveLine("SD" & Utilities.strFormatDate(.DueDate))
                    End If
                    If Len(.Terms) > 0 Then
                        SaveLine("ST" & .Terms)
                    End If
                    If Len(.BudgetKey) > 0 Then
                        SaveLine("SB" & .BudgetKey)
                    End If
                    SaveLine("SA" & Utilities.strFormatCurrency(.Amount))
                    SaveLine("SZ")
                End With
            Next objSplit
        End With
    End Sub

    Private Sub SaveTrxBudget()
        SaveTrxShared("TB")
        With DirectCast(mobjTrx, BudgetTrx)
            SaveLine("BE" & Utilities.strFormatDate(.BudgetStarts))
            SaveLine("KB" & .BudgetKey)
            SaveLine("A$" & Utilities.strFormatCurrency(.BudgetLimit))
        End With
    End Sub

    Private Sub SaveTrxTransfer()
        SaveTrxShared("TT")
        With DirectCast(mobjTrx, TransferTrx)
            SaveLine("KT" & .TransferKey)
            SaveLine("A$" & Utilities.strFormatCurrency(.TransferAmount))
        End With
    End Sub

    Private Sub SaveTrxShared(ByVal strFirstCmd As String)
        With mobjTrx
            SaveLine(strFirstCmd & Mid("URNS", .Status, 1) & .Description)
            SaveLine("DT" & Utilities.strFormatDate(.TrxDate))
            If Len(.Memo) > 0 Then
                SaveLine("ME" & .Memo)
            End If
            If .IsAwaitingReview Then
                SaveLine("AR")
            End If
            If .RepeatSeq <> 0 Then
                SaveLine("RS" & .RepeatSeq)
            End If
            If Len(.RepeatKey) > 0 Then
                SaveLine("KR" & .RepeatKey)
            End If
        End With
    End Sub

    Private Function ConvertRepeatUnit(ByVal lngUnit As BaseTrx.RepeatUnit) As String
        ConvertRepeatUnit = ""
        Select Case lngUnit
            Case BaseTrx.RepeatUnit.Day
                ConvertRepeatUnit = "DAY"
            Case BaseTrx.RepeatUnit.Week
                ConvertRepeatUnit = "WEEK"
            Case BaseTrx.RepeatUnit.Month
                ConvertRepeatUnit = "MONTH"
            Case Else
                RaiseErrorMsg("Unrecognized repeat unit: " & lngUnit)
        End Select
    End Function

    Private Sub SaveLine(ByRef strLine As String)
        If mblnFake Then
            mcolFakeLines.Add(strLine)
        Else
            mobjSaveFile.WriteLine(strLine)
        End If
    End Sub
End Class