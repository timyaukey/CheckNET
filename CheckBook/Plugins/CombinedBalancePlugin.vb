Option Strict On
Option Explicit On


Public Class CombinedBalancePlugin
    Inherits PluginBase

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register(ByVal setup As IHostSetup)
        setup.objToolMenu.Add(New MenuElementAction("Combined Personal and Business Balance", 120, AddressOf ClickHandler, GetPluginPath()))
    End Sub

    Private Sub ClickHandler(sender As Object, e As EventArgs)
        Try
            Dim objReg As Register = HostUI.objGetCurrentRegister()
            If objReg Is Nothing Then
                HostUI.ErrorMessageBox("Please click on the register window you wish to calculate combined personal and business balance for.")
                Exit Sub
            End If
            Dim objLiabilityAcct As Account = objReg.objAccount
            If objLiabilityAcct.lngType <> Account.AccountType.Liability Then
                HostUI.ErrorMessageBox("Combined personal and business balance may only be computed for liability accounts.")
                Exit Sub
            End If
            Dim objPersonalAcct As Account = objLiabilityAcct.objRelatedAcct1
            If objPersonalAcct Is Nothing Then
                HostUI.ErrorMessageBox("Account does not have a ""Related account #1"" (this is the related personal account)")
                Exit Sub
            End If
            Dim strEndDate As String = InputBox("Enter date to compute combined balance for:", "Balance Date", DateTime.Today.ToShortDateString())
            If Not IsDate(strEndDate) Then
                HostUI.ErrorMessageBox("Invalid Date")
                Exit Sub
            End If
            Dim datEndDate As DateTime = CDate(strEndDate)
            Dim curCombinedBalance As Decimal = 0
            For Each objLiabilityReg As Register In objLiabilityAcct.colRegisters
                curCombinedBalance = curCombinedBalance + objLiabilityReg.curEndingBalance(datEndDate)
            Next
            For Each objPersonalReg As Register In objPersonalAcct.colRegisters
                curCombinedBalance = curCombinedBalance + objPersonalReg.curEndingBalance(datEndDate)
            Next
            HostUI.InfoMessageBox("Combined personal and business balance as of " & datEndDate.ToShortDateString() & " is " & Utilities.strFormatCurrency(curCombinedBalance) & ".")
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub
End Class
