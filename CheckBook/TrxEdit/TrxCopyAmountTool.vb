Option Strict On
Option Explicit On

Public Class TrxCopyAmountTool
    Implements ITrxTool

    Private mobjHostUI As IHostUI

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property strTitle As String Implements ITrxTool.strTitle
        Get
            Return "Copy Transaction Amount"
        End Get
    End Property

    Public Sub Run(objHostTrxToolUI As IHostTrxToolUI) Implements ITrxTool.Run
        Dim objNormalTrx As NormalTrx = objHostTrxToolUI.objGetTrxCopy()
        If objNormalTrx Is Nothing Then
            Return
        End If
        My.Computer.Clipboard.Clear()
        My.Computer.Clipboard.SetText(Utilities.strFormatCurrency(System.Math.Abs(objNormalTrx.curAmount)))
    End Sub
End Class
