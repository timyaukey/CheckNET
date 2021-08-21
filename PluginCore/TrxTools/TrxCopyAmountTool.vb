Option Strict On
Option Explicit On

Public Class TrxCopyAmountTool
    Implements ITrxTool

    Private mobjHostUI As IHostUI

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property Title As String Implements ITrxTool.Title
        Get
            Return "Copy Transaction Amount To Clipboard"
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Title
    End Function

    Public Sub Run(objHostTrxToolUI As IHostTrxToolUI) Implements ITrxTool.Run
        Dim objNormalTrx As BankTrx = objHostTrxToolUI.GetTrxCopy()
        If objNormalTrx Is Nothing Then
            Return
        End If
        My.Computer.Clipboard.Clear()
        My.Computer.Clipboard.SetText(Utilities.FormatCurrency(System.Math.Abs(objNormalTrx.Amount)))
    End Sub
End Class
