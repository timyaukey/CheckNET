Option Strict On
Option Explicit On

Public Class TrxCopyInvoiceNumbersTool
    Implements ITrxTool

    Private mobjHostUI As IHostUI

    Public Sub New(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
    End Sub

    Public ReadOnly Property Title As String Implements ITrxTool.Title
        Get
            Return "Copy Invoice Numbers To Clipboard"
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return Title
    End Function

    Public Sub Run(objHostTrxToolUI As IHostTrxToolUI) Implements ITrxTool.Run
        Dim strNumbers As String = ""
        Dim objNormalTrx As BankTrx = objHostTrxToolUI.GetTrxCopy()
        If objNormalTrx Is Nothing Then
            Return
        End If
        For Each objSplit As TrxSplit In objNormalTrx.Splits
            If Not String.IsNullOrEmpty(objSplit.InvoiceNum) Then
                strNumbers = strNumbers & " " & objSplit.InvoiceNum
            End If
        Next
        strNumbers = Trim(strNumbers)
        My.Computer.Clipboard.Clear()
        If Not String.IsNullOrEmpty(strNumbers) Then
            My.Computer.Clipboard.SetText(strNumbers)
        End If
    End Sub
End Class
