Option Strict On
Option Explicit On

Public Class PluginList

    Public Sub ShowMe(ByVal objHostSetup As IHostSetup)
        lvwPlugins.Items.Clear()
        AddPlugins("File", objHostSetup.FileMenu)
        AddPlugins("Bank Import", objHostSetup.BankImportMenu)
        AddPlugins("Check Import", objHostSetup.CheckImportMenu)
        AddPlugins("Deposit Import", objHostSetup.DepositImportMenu)
        AddPlugins("Invoice Import", objHostSetup.InvoiceImportMenu)
        AddPlugins("Reports", objHostSetup.ReportMenu)
        AddPlugins("Tools", objHostSetup.ToolMenu)
        AddPlugins("Help", objHostSetup.HelpMenu)
        Me.ShowDialog()
    End Sub

    Private Sub AddPlugins(ByVal strType As String, ByVal colPlugins As MenuBuilder)
        For Each objPlugin As MenuElementBase In colPlugins.Elements
            Dim item As ListViewItem = New ListViewItem(strType)
            item.SubItems.Add(objPlugin.Title)
            item.SubItems.Add(objPlugin.SortCode.ToString())
            item.SubItems.Add(objPlugin.PluginPath)
            lvwPlugins.Items.Add(item)
        Next
    End Sub


End Class