﻿Option Strict On
Option Explicit On

Public Class TrxCopyDateTool
    Implements ITrxTool

    Private mobjHostUI As IHostUI

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property strTitle As String Implements ITrxTool.strTitle
        Get
            Return "Copy Transaction Date To Clipboard"
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return strTitle
    End Function

    Public Sub Run(objHostTrxToolUI As IHostTrxToolUI) Implements ITrxTool.Run
        Dim objNormalTrx As NormalTrx = objHostTrxToolUI.objGetTrxCopy()
        If objNormalTrx Is Nothing Then
            Return
        End If
        My.Computer.Clipboard.Clear()
        My.Computer.Clipboard.SetText(objNormalTrx.datDate.ToString("MM/dd/yyyy"))
    End Sub
End Class
