Option Strict On
Option Explicit On


Public Class SearchMoveTool
    Implements ISearchTool

    Private mobjHostUI As IHostUI

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property Title As String Implements ISearchTool.Title
        Get
            Return "Move"
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Title
    End Function

    Public Sub Run(objHostSearchToolUI As IHostSearchToolUI) Implements ISearchTool.Run
        Dim objTrxSrc As BankTrx
        Dim objTrxFirst As BankTrx = Nothing
        Dim colTrx As ICollection(Of BankTrx)
        Dim strNewDate As String = ""
        Dim objNewReg As Register = Nothing
        Dim datExplicitDate As Date
        Dim blnUseDayOffset As Boolean
        Dim intDayOffset As Integer
        Dim datNewDate As Date
        Dim frmMoveTo As MoveDstForm

        colTrx = New List(Of BankTrx)
        For Each objTrxSrc In objHostSearchToolUI.objAllSelectedTrx()
            If Not objHostSearchToolUI.blnValidTrxForBulkOperation(objTrxSrc, "moved") Then
                Exit Sub
            End If
            colTrx.Add(objTrxSrc)
        Next
        If colTrx.Count() = 0 Then
            mobjHostUI.ErrorMessageBox("No transactions selected.")
            Exit Sub
        End If

        frmMoveTo = New MoveDstForm
        If Not frmMoveTo.ShowModal(mobjHostUI, objHostSearchToolUI.objReg.Account.Registers,
                                      objHostSearchToolUI.objReg, strNewDate, objNewReg) Then
            Exit Sub
        End If
        If Utilities.IsValidDate(strNewDate) Then
            datExplicitDate = CDate(strNewDate)
            blnUseDayOffset = False
        ElseIf IsNumeric(strNewDate) Or strNewDate = "" Then
            intDayOffset = CInt(Val(strNewDate))
            blnUseDayOffset = True
        Else
            'Should never get here.
            mobjHostUI.ErrorMessageBox("Invalid date or number of days.")
            Exit Sub
        End If

        Dim objStartLogger As ILogGroupStart
        objStartLogger = objHostSearchToolUI.objReg.LogGroupStart("SearchForm.Move")
        For Each objTrxSrc In colTrx
            If blnUseDayOffset Then
                datNewDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDayOffset, objTrxSrc.TrxDate)
            Else
                datNewDate = datExplicitDate
            End If
            With objTrxSrc
                If objNewReg Is Nothing Then
                    'Changing date, not register.
                    Dim objTrxManager As NormalTrxManager = New NormalTrxManager(objTrxSrc)
                    objTrxManager.UpdateStart()
                    objTrxManager.Trx.TrxDate = datNewDate
                    objTrxManager.UpdateEnd(New LogMove, "SearchForm.MoveUpdate")
                    If objTrxFirst Is Nothing Then
                        objTrxFirst = objTrxSrc
                    End If
                Else
                    'Changing register, and possibly date.
                    Dim objTrxNew As BankTrx = New BankTrx(objNewReg)
                    objTrxNew.NewStartNormal(True, objTrxSrc)
                    objTrxNew.TrxDate = datNewDate
                    .CopySplits(objTrxNew)
                    objNewReg.NewAddEnd(objTrxNew, New LogAdd, "SearchForm.MoveAdd")
                    If objTrxFirst Is Nothing Then
                        objTrxFirst = objTrxNew
                    End If
                    objTrxSrc.Delete(New LogDelete, "SearchForm.MoveDelete")
                End If
            End With
        Next objTrxSrc
        objHostSearchToolUI.objReg.LogGroupEnd(objStartLogger)

        If Not objTrxFirst Is Nothing Then
            If objNewReg Is Nothing Then
                objHostSearchToolUI.objReg.SetCurrent(objTrxFirst)
                objHostSearchToolUI.objReg.FireShowCurrent()
            Else
                objNewReg.SetCurrent(objTrxFirst)
                objNewReg.FireShowCurrent()
            End If
        End If
    End Sub
End Class
