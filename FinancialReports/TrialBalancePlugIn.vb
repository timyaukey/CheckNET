Option Strict On
Option Explicit On

<Assembly: PluginAssembly()>

Public Class TrialBalancePlugIn
    Inherits PluginBase

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register(ByVal setup As IHostSetup)
        setup.ReportMenu.Add(New MenuElementAction("Financial Statements", 400, AddressOf ClickHandler))
        setup.HelpMenu.Add(New MenuElementAction("Financial Statements", 350, AddressOf HelpHandler))
        Dim objLicense As Willowsoft.TamperProofData.IStandardLicense = New FinancialReportsLicense()
        objLicense.Load(Company.LicenseFolderPath)
        setup.AddExtraLicense(objLicense)
        MetadataInternal = New PluginMetadata("Financial Reports", "Willow Creek Software",
            System.Reflection.Assembly.GetExecutingAssembly(), Nothing,
            "A full set of standard financial reports including balance sheet, profit and loss, and more.",
            objLicense)
    End Sub

    Private Sub ClickHandler(sender As Object, e As EventArgs)
        Dim frm As TrialBalanceForm = New TrialBalanceForm()
        frm.ShowWindow(Me.HostUI)
    End Sub

    Private Sub HelpHandler(sender As Object, e As EventArgs)
        HostUI.ShowHelp("FinancialStatements.html")
    End Sub

End Class
