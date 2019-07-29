Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class SearchMoveTool
    Implements ISearchTool

    Private mobjHostUI As IHostUI

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property strTitle As String Implements ISearchTool.strTitle
        Get
            Return "Move"
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return strTitle
    End Function

    Public Sub Run(objHostSearchToolUI As IHostSearchToolUI) Implements ISearchTool.Run
        Dim objTrxSrc As NormalTrx
        Dim objTrxFirst As NormalTrx = Nothing
        Dim colTrx As ICollection(Of NormalTrx)
        Dim strNewDate As String = ""
        Dim objNewReg As Register = Nothing
        Dim datExplicitDate As Date
        Dim blnUseDayOffset As Boolean
        Dim intDayOffset As Integer
        Dim datNewDate As Date
        Dim frmMoveTo As MoveDstForm

        colTrx = New List(Of NormalTrx)
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
        If Not frmMoveTo.blnShowModal(mobjHostUI, objHostSearchToolUI.objReg.objAccount.colRegisters,
                                      objHostSearchToolUI.objReg, strNewDate, objNewReg) Then
            Exit Sub
        End If
        If Utilities.blnIsValidDate(strNewDate) Then
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
        objStartLogger = objHostSearchToolUI.objReg.objLogGroupStart("SearchForm.Move")
        For Each objTrxSrc In colTrx
            If blnUseDayOffset Then
                datNewDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, intDayOffset, objTrxSrc.datDate)
            Else
                datNewDate = datExplicitDate
            End If
            With objTrxSrc
                If objNewReg Is Nothing Then
                    'Changing date, not register.
                    Dim objTrxManager As NormalTrxManager = objTrxSrc.objGetTrxManager()
                    objTrxManager.UpdateStart()
                    objTrxManager.objTrx.datDate = datNewDate
                    objTrxManager.UpdateEnd(New LogMove, "SearchForm.MoveUpdate")
                    If objTrxFirst Is Nothing Then
                        objTrxFirst = objTrxSrc
                    End If
                Else
                    'Changing register, and possibly date.
                    Dim objTrxNew As NormalTrx = New NormalTrx(objNewReg)
                    objTrxNew.NewStartNormal(True, objTrxSrc)
                    objTrxNew.datDate = datNewDate
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
                objHostSearchToolUI.objReg.SetCurrent(objTrxFirst.lngIndex)
                objHostSearchToolUI.objReg.RaiseShowCurrent()
            Else
                objNewReg.SetCurrent(objTrxFirst.lngIndex)
                objNewReg.RaiseShowCurrent()
            End If
        End If
    End Sub
End Class
