Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class PluginList
    Private mobjHostUI As IHostUI

    Public Sub ShowMe(ByVal objHostUI As IHostUI,
            objBankImportMenu As MenuBuilder,
            objCheckImportMenu As MenuBuilder,
            objDepositImportMenu As MenuBuilder,
            objInvoiceImportMenu As MenuBuilder,
            objReportMenu As MenuBuilder,
            objToolMenu As MenuBuilder)
        mobjHostUI = objHostUI
        lvwPlugins.Items.Clear()
        AddPlugins("Bank Import", objBankImportMenu)
        AddPlugins("Check Import", objCheckImportMenu)
        AddPlugins("Deposit Import", objDepositImportMenu)
        AddPlugins("Invoice Import", objInvoiceImportMenu)
        AddPlugins("Reports", objReportMenu)
        AddPlugins("Tools", objToolMenu)
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