Option Strict On
Option Explicit On


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

    Public Overrides Function ToString() As String
        Return strTitle
    End Function

    Public Sub Run(objHostSearchToolUI As IHostSearchToolUI) Implements ISearchTool.Run
        Dim objNewTrx As BankTrx = Nothing
        Dim objOldTrx As BankTrx
        Dim colOldTrx As ICollection(Of BaseTrx)
        Dim objOldSplit As TrxSplit
        Dim objStartLogger As ILogGroupStart
        Dim datToday As Date
        Dim datResult As Date

        'Build the new BaseTrx from selected (checked) BaseTrx.
        'Use the first BaseTrx for BaseTrx level data, and clone the splits of all BaseTrx.
        'Keep a collection of the chosen BaseTrx, to delete them at the end.
        colOldTrx = New List(Of BaseTrx)
        For Each objOldTrx In objHostSearchToolUI.objAllSelectedTrx()
            If Not objHostSearchToolUI.blnValidTrxForBulkOperation(objOldTrx, "combined") Then
                Exit Sub
            End If
            'If we do not yet have a new trx, create it.
            If objNewTrx Is Nothing Then
                objNewTrx = New BankTrx(objHostSearchToolUI.objReg)
                datToday = Today
                objNewTrx.NewStartNormal(True, "", datToday, objOldTrx.Description, objOldTrx.Memo, BaseTrx.TrxStatus.Unreconciled, New TrxGenImportData())
            End If
            'Remember the old BaseTrx to delete later if the new BaseTrx is saved.
            'Remember the BaseTrx object instead of its index because the index may change
            'as the result of saving the new BaseTrx or deleting other old ones.
            colOldTrx.Add(objOldTrx)
            'Clone all the splits in old trx and add them to new trx.
            For Each objOldSplit In objOldTrx.Splits
                With objOldSplit
                    objNewTrx.AddSplit(.Memo, .CategoryKey, .PONumber, .InvoiceNum, .InvoiceDate, .DueDate, .Terms, .BudgetKey, .Amount)
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
        objNewTrx = DirectCast(objHostSearchToolUI.objReg.CurrentTrx, BankTrx)

        'Now delete old trx.
        'Because we start from the BaseTrx object instead of its index, we don't need
        'to worry if saving the new trx or a prior delete changed the index of a BaseTrx.
        objStartLogger = objHostSearchToolUI.objReg.LogGroupStart("SearchForm.CombineDelete")
        For Each objOldTrx In colOldTrx
            objOldTrx.Delete(New LogDelete, "SearchForm.CombineDeleteTrx")
        Next
        objHostSearchToolUI.objReg.LogGroupEnd(objStartLogger)

        'Have to do this because deleting the original trx changes current.
        objHostSearchToolUI.objReg.SetCurrent(objNewTrx)
        objHostSearchToolUI.objReg.FireShowCurrent()

    End Sub
End Class
