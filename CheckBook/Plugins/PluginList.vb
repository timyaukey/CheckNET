Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class PluginList
    Private mobjHostUI As IHostUI

    Public Sub ShowMe(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
        lvwPlugins.Items.Clear()
        AddPlugins("Bank Import", mobjHostUI.objBankImportMenu)
        AddPlugins("Check Import", mobjHostUI.objCheckImportMenu)
        AddPlugins("Deposit Import", mobjHostUI.objDepositImportMenu)
        AddPlugins("Invoice Import", mobjHostUI.objInvoiceImportMenu)
        AddPlugins("Reports", mobjHostUI.objReportMenu)
        AddPlugins("Tools", mobjHostUI.objToolMenu)
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