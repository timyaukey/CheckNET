Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class CombinedBalancePlugin
    Inherits ToolPlugin

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
        SortCode = 120
    End Sub

    Public Overrides Sub ClickHandler(sender As Object, e As EventArgs)
        Try
            Dim objReg As Register = HostUI.objGetCurrentRegister()
            If objReg Is Nothing Then
                MsgBox("Please click on the register window you wish to calculate combined personal and business balance for.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            Dim objLiabilityAcct As Account = objReg.objAccount
            If objLiabilityAcct.lngType <> Account.AccountType.Liability Then
                MsgBox("Combined personal and business balance may only be computed for liability accounts.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            Dim objPersonalAcct As Account = objLiabilityAcct.objRelatedAcct1
            If objPersonalAcct Is Nothing Then
                MsgBox("Account does not have a ""Related account #1"" (this is the related personal account)", MsgBoxStyle.Critical)
                Exit Sub
            End If
            Dim strEndDate As String = InputBox("Enter date to compute combined balance for:", "Balance Date", DateTime.Today.ToShortDateString())
            If Not IsDate(strEndDate) Then
                MsgBox("Invalid Date", MsgBoxStyle.Critical)
                Exit Sub
            End If
            Dim datEndDate As DateTime = CDate(strEndDate)
            Dim curCombinedBalance As Decimal = 0
            For Each objLiabilityReg As Register In objLiabilityAcct.colRegisters
                curCombinedBalance = curCombinedBalance + curRegisterBalance(objLiabilityReg, datEndDate)
            Next
            For Each objPersonalReg As Register In objPersonalAcct.colRegisters
                curCombinedBalance = curCombinedBalance + curRegisterBalance(objPersonalReg, datEndDate)
            Next
            MsgBox("Combined personal and business balance as of " & datEndDate.ToShortDateString() & " is " & gstrFormatCurrency(curCombinedBalance) & ".")
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function curRegisterBalance(ByVal objReg As Register, ByVal datEndDate As DateTime) As Decimal
        Dim lngIndex As Integer
        Dim curBalance As Decimal
        For lngIndex = 1 To objReg.lngTrxCount
            Dim objTrx As Trx = objReg.objTrx(lngIndex)
            If objTrx.datDate > datEndDate Then
                Exit For
            End If
            curBalance = curBalance + objTrx.curAmount
        Next
        Return curBalance
    End Function

    Public Overrides Function GetMenuTitle() As String
        Return "Combined Personal and Business Balance"
    End Function
End Class
