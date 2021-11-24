Option Strict On
Option Explicit On

<Assembly: PluginAssembly()>

Public Class BuiltInPlugins
    Inherits PluginBase

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register(ByVal setup As IHostSetup)
        setup.ReportMenu.Add(New MenuElementAction("Totals By Category", 200, AddressOf CategoryReportClickHandler))
        setup.ToolMenu.Add(New MenuElementRegister(HostUI, "Combined Personal and Business Balance", 120, AddressOf CombinedBalanceClickHandler))
        setup.ToolMenu.Add(New MenuElementRegister(HostUI, "Adjust Account For Personal Use", 110, AddressOf PersonalBusinessClickHandler))
        setup.ReportMenu.Add(New MenuElementRegister(HostUI, "Find Live Budgets", 100, AddressOf LiveBudgetClickHandler))
        setup.ToolMenu.Add(New MenuElementAction("Reconcile", 1, AddressOf ReconcileClickHandler))

        MetadataInternal = New PluginMetadata("Built In Tools", "Willow Creek Software",
            Reflection.Assembly.GetExecutingAssembly(), Nothing, "Miscellaneous tools built into the software.", Nothing)
    End Sub

    Private Sub CategoryReportClickHandler(sender As Object, e As EventArgs)
        Try
            Dim frmRpt As RptScanSplitsForm = New RptScanSplitsForm
            frmRpt.ShowMe(RptScanSplitsForm.SplitReportType.Totals, Me.HostUI)
            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub


    Private Sub CombinedBalanceClickHandler(sender As Object, e As RegisterEventArgs)
        Try
            Dim objLiabilityAcct As Account = e.Reg.Account
            If objLiabilityAcct.AcctType <> Account.AccountType.Liability Then
                HostUI.ErrorMessageBox("Combined personal and business balance may only be computed for liability accounts.")
                Exit Sub
            End If
            Dim objPersonalAcct As Account = objLiabilityAcct.RelatedAcct1
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
            For Each objLiabilityReg As Register In objLiabilityAcct.Registers
                curCombinedBalance = curCombinedBalance + objLiabilityReg.EndingBalance(datEndDate)
            Next
            For Each objPersonalReg As Register In objPersonalAcct.Registers
                curCombinedBalance = curCombinedBalance + objPersonalReg.EndingBalance(datEndDate)
            Next
            HostUI.InfoMessageBox("Combined personal and business balance as of " & datEndDate.ToShortDateString() & " is " & Utilities.FormatCurrency(curCombinedBalance) & ".")
            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

    Private Sub PersonalBusinessClickHandler(sender As Object, e As RegisterEventArgs)
        Try
            Dim frmAdjust As AdjustPersonalBusinessForm = New AdjustPersonalBusinessForm
            frmAdjust.ShowModal(HostUI, e.Reg.Account)
            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub


    Private Sub LiveBudgetClickHandler(sender As Object, e As RegisterEventArgs)
        Try
            Dim frmFind As LiveBudgetListForm = New LiveBudgetListForm
            frmFind.ShowModal(HostUI, e.Reg, HostUI.Company.Budgets)
            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub


    Private Sub ReconcileClickHandler(sender As Object, e As EventArgs)
        Try
            Dim frm As ReconAcctSelectForm = New ReconAcctSelectForm
            frm.Init(HostUI)
            frm.ShowDialog()
            Exit Sub
        Catch ex As Exception
            TopException(ex)
        End Try
    End Sub

End Class
