﻿Option Strict On
Option Explicit On

Public Class TrxPrintCheckTool
    Implements ITrxTool

    Private mobjHostUI As IHostUI

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property strTitle As String Implements ITrxTool.strTitle
        Get
            Return "Print Check"
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return strTitle
    End Function

    Public Sub Run(ByVal objHostTrxToolUI As IHostTrxToolUI) Implements ITrxTool.Run

        Dim objCheckPrinting As CheckPrinting = New CheckPrinting(mobjHostUI)

        Dim objTestTrx As BankTrx = objHostTrxToolUI.objGetTrxCopy()
        If objTestTrx Is Nothing Then
            Exit Sub
        End If
        If Not objCheckPrinting.IsAllowedToPrintCheck(objTestTrx) Then
            Exit Sub
        End If
        If Not objCheckPrinting.PrepareForFirstCheck() Then
            Exit Sub
        End If

        objHostTrxToolUI.SetNumber(CheckPrinting.NextCheckNumToPrint)
        objHostTrxToolUI.SetDate(Today)
        objHostTrxToolUI.SetFake(False)

        Dim objTrx As BankTrx = objHostTrxToolUI.objGetTrxCopy()
        If objTrx Is Nothing Then
            Exit Sub
        End If

        objHostTrxToolUI.objReg.LogAction("PrintCheck:" & CheckPrinting.NextCheckNumToPrint)
        If objCheckPrinting.PrintCheck(objTrx) Then
            objHostTrxToolUI.SaveAndClose()
        End If

    End Sub

End Class
