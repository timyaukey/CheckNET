Option Strict On
Option Explicit On

Public Class PluginList

    Public Sub ShowMe(ByVal objHostSetup As IHostSetup)
        lvwPlugins.Items.Clear()
        AddPlugins("Bank Import", objHostSetup.objBankImportMenu)
        AddPlugins("Check Import", objHostSetup.objCheckImportMenu)
        AddPlugins("Deposit Import", objHostSetup.objDepositImportMenu)
        AddPlugins("Invoice Import", objHostSetup.objInvoiceImportMenu)
        AddPlugins("Reports", objHostSetup.objReportMenu)
        AddPlugins("Tools", objHostSetup.objToolMenu)
        AddPlugins("Help", objHostSetup.objHelpMenu)
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