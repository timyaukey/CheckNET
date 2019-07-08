Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class SearchCombineTool
    Implements ISearchTool

    Private mobjHostUI As IHostUI

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property strTitle As String Implements ISearchTool.strTitle
        Get
            Return "Combine"
        End Get
    End Property

    Public Sub Run(objHostSearchToolUI As IHostSearchToolUI) Implements ISearchTool.Run
        Dim objNewTrx As NormalTrx = Nothing
        Dim objOldTrx As NormalTrx
        Dim colOldTrx As ICollection(Of Trx)
        Dim objOldSplit As TrxSplit
        Dim objStartLogger As ILogGroupStart
        Dim datToday As Date
        Dim datResult As Date

        'Build the new Trx from selected (checked) Trx.
        'Use the first Trx for Trx level data, and clone the splits of all Trx.
        'Keep a collection of the chosen Trx, to delete them at the end.
        colOldTrx = New List(Of Trx)
        For Each objOldTrx In objHostSearchToolUI.objAllSelectedTrx()
            If Not objHostSearchToolUI.blnValidTrxForBulkOperation(objOldTrx, "combined") Then
                Exit Sub
            End If
            'If we do not yet have a new trx, create it.
            If objNewTrx Is Nothing Then
                objNewTrx = New NormalTrx(objHostSearchToolUI.objReg)
                datToday = Today
                objNewTrx.NewStartNormal(True, "", datToday, objOldTrx.strDescription, objOldTrx.strMemo, Trx.TrxStatus.Unreconciled, New TrxGenImportData())
            End If
            'Remember the old Trx to delete later if the new Trx is saved.
            'Remember the Trx object instead of its index because the index may change
            'as the result of saving the new Trx or deleting other old ones.
            colOldTrx.Add(objOldTrx)
            'Clone all the splits in old trx and add them to new trx.
            For Each objOldSplit In objOldTrx.colSplits
                With objOldSplit
                    objNewTrx.AddSplit(.strMemo, .strCategoryKey, .strPONumber, .strInvoiceNum, .datInvoiceDate, .datDueDate, .strTerms, .strBudgetKey, .curAmount)
                End With
            Next objOldSplit
        Next
        If colOldTrx.Count() = 0 Then
            mobjHostUI.ErrorMessageBox("No transactions selected.")
            Exit Sub
        End If

        'Now let them edit it and possibly save it.
        If mobjHostUI.blnAddNormalTrx(objNewTrx, datResult, False, "SearchForm.CombineNew") Then
            'They did not save it.
            mobjHostUI.InfoMessageBox("Canceled.")
            Exit Sub
        End If
        objNewTrx = objHostSearchToolUI.objReg.objNormalTrx(objHostSearchToolUI.objReg.lngCurrentTrxIndex())

        'Now delete old trx.
        'Because we start from the Trx object instead of its index, we don't need
        'to worry if saving the new trx or a prior delete changed the index of a Trx.
        objStartLogger = objHostSearchToolUI.objReg.objLogGroupStart("SearchForm.CombineDelete")
        For Each objOldTrx In colOldTrx
            objOldTrx.Delete(New LogDelete, "SearchForm.CombineDeleteTrx")
        Next
        objHostSearchToolUI.objReg.LogGroupEnd(objStartLogger)

        objHostSearchToolUI.objReg.SetCurrent(objNewTrx.lngIndex)
        objHostSearchToolUI.objReg.RaiseShowCurrent()

    End Sub
End Class
