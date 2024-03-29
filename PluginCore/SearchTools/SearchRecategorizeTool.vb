﻿Option Strict On
Option Explicit On


Public Class SearchRecategorizeTool
    Implements ISearchTool

    Private mobjHostUI As IHostUI

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property Title As String Implements ISearchTool.Title
        Get
            Return "Recategorize"
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Title
    End Function

    Public Sub Run(objHostSearchToolUI As IHostSearchToolUI) Implements ISearchTool.Run
        Dim strOldCatKey As String = ""
        Dim strNewCatKey As String = ""
        Dim colTrx As ICollection(Of BankTrx)
        Dim objCheckedTrx As BaseTrx
        Dim objNormalTrx As BankTrx
        Dim objTrxManager As NormalTrxManager
        Dim colSplits As IEnumerable(Of TrxSplit)
        Dim objSplit As TrxSplit
        Dim strCatKey As String
        Dim lngChgCount As Integer
        Dim objStartLogger As ILogGroupStart

        Using frmArgs As ChangeCategoryForm = New ChangeCategoryForm
            If Not frmArgs.GetCategories(mobjHostUI, strOldCatKey, strNewCatKey) Then
                Exit Sub
            End If
        End Using

        colTrx = New List(Of BankTrx)
        For Each objCheckedTrx In objHostSearchToolUI.GetAllSelectedTrx()
            If objCheckedTrx.GetType() IsNot GetType(BankTrx) Then
                mobjHostUI.ErrorMessageBox("Budgets and transfers may not be recategorized.")
                Exit Sub
            End If
            If objCheckedTrx.IsAutoGenerated Then
                mobjHostUI.ErrorMessageBox("Generated transactions may not be recategorized.")
                Exit Sub
            End If
            colSplits = DirectCast(objCheckedTrx, BankTrx).Splits
            For Each objSplit In colSplits
                With objSplit
                    If objSplit.CategoryKey = strOldCatKey Then
                        colTrx.Add(DirectCast(objCheckedTrx, BankTrx))
                        Exit For
                    End If
                End With
            Next objSplit
        Next
        If colTrx.Count() = 0 Then
            mobjHostUI.ErrorMessageBox("No transactions selected, or none that use the old category.")
            Exit Sub
        End If

        objStartLogger = objHostSearchToolUI.Reg.LogGroupStart("SearchForm.Recategorize")
        For Each objNormalTrx In colTrx
            objTrxManager = New NormalTrxManager(objNormalTrx)
            colSplits = objNormalTrx.Splits
            objTrxManager.UpdateStart()
            objNormalTrx.ClearSplits()
            For Each objSplit In colSplits
                With objSplit
                    strCatKey = objSplit.CategoryKey
                    If strCatKey = strOldCatKey Then
                        strCatKey = strNewCatKey
                    End If
                    objNormalTrx.AddSplit(.Memo, strCatKey, .PONumber, .InvoiceNum, .InvoiceDate, .DueDate, .Terms, .BudgetKey, .Amount)
                End With
            Next objSplit
            objTrxManager.UpdateEnd(New LogChange, "SearchForm.Recategorize")
            lngChgCount = lngChgCount + 1
        Next
        objHostSearchToolUI.Reg.LogGroupEnd(objStartLogger)

        mobjHostUI.InfoMessageBox("Changed category of " & lngChgCount & " transactions.")

    End Sub
End Class
