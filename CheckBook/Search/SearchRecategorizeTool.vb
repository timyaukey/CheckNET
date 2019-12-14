﻿Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class SearchRecategorizeTool
    Implements ISearchTool

    Private mobjHostUI As IHostUI

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property strTitle As String Implements ISearchTool.strTitle
        Get
            Return "Recategorize"
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return strTitle
    End Function

    Public Sub Run(objHostSearchToolUI As IHostSearchToolUI) Implements ISearchTool.Run
        Dim strOldCatKey As String = ""
        Dim strNewCatKey As String = ""
        Dim colTrx As ICollection(Of NormalTrx)
        Dim objCheckedTrx As Trx
        Dim objNormalTrx As NormalTrx
        Dim objTrxManager As NormalTrxManager
        Dim colSplits As IEnumerable(Of TrxSplit)
        Dim objSplit As TrxSplit
        Dim strCatKey As String
        Dim lngChgCount As Integer
        Dim objStartLogger As ILogGroupStart

        Using frmArgs As ChangeCategoryForm = New ChangeCategoryForm
            If Not frmArgs.blnGetCategories(mobjHostUI, strOldCatKey, strNewCatKey) Then
                Exit Sub
            End If
        End Using

        colTrx = New List(Of NormalTrx)
        For Each objCheckedTrx In objHostSearchToolUI.objAllSelectedTrx()
            If objCheckedTrx.GetType() IsNot GetType(NormalTrx) Then
                mobjHostUI.ErrorMessageBox("Budgets and transfers may not be recategorized.")
                Exit Sub
            End If
            If objCheckedTrx.blnAutoGenerated Then
                mobjHostUI.ErrorMessageBox("Generated transactions may not be recategorized.")
                Exit Sub
            End If
            colSplits = DirectCast(objCheckedTrx, NormalTrx).colSplits
            For Each objSplit In colSplits
                With objSplit
                    If objSplit.strCategoryKey = strOldCatKey Then
                        colTrx.Add(DirectCast(objCheckedTrx, NormalTrx))
                        Exit For
                    End If
                End With
            Next objSplit
        Next
        If colTrx.Count() = 0 Then
            mobjHostUI.ErrorMessageBox("No transactions selected, or none that use the old category.")
            Exit Sub
        End If

        objStartLogger = objHostSearchToolUI.objReg.objLogGroupStart("SearchForm.Recategorize")
        For Each objNormalTrx In colTrx
            objTrxManager = objNormalTrx.objGetTrxManager()
            colSplits = objNormalTrx.colSplits
            objTrxManager.UpdateStart()
            objNormalTrx.ClearSplits()
            For Each objSplit In colSplits
                With objSplit
                    strCatKey = objSplit.strCategoryKey
                    If strCatKey = strOldCatKey Then
                        strCatKey = strNewCatKey
                    End If
                    objNormalTrx.AddSplit(.strMemo, strCatKey, .strPONumber, .strInvoiceNum, .datInvoiceDate, .datDueDate, .strTerms, .strBudgetKey, .curAmount)
                End With
            Next objSplit
            objTrxManager.UpdateEnd(New LogChange, "SearchForm.Recategorize")
            lngChgCount = lngChgCount + 1
        Next
        objHostSearchToolUI.objReg.LogGroupEnd(objStartLogger)

        mobjHostUI.InfoMessageBox("Changed category of " & lngChgCount & " transactions.")

    End Sub
End Class
