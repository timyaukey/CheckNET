Imports CheckBookLib

Public Class PluginList
    Private mobjHostUI As IHostUI

    Public Sub ShowMe(ByVal objHostUI As IHostUI,
            colBankImportPlugins As List(Of IBankImportPlugin),
            colCheckImportPlugins As List(Of ICheckImportPlugin),
            colDepositImportPlugins As List(Of IDepositImportPlugin),
            colInvoiceImportPlugins As List(Of IInvoiceImportPlugin),
            colReportPlugins As List(Of IReportPlugin),
            colToolPlugins As List(Of IToolPlugin))
        mobjHostUI = objHostUI
        lvwPlugins.Items.Clear()
        AddPlugins(Of IBankImportPlugin)("Bank Import", colBankImportPlugins)
        AddPlugins(Of ICheckImportPlugin)("Check Import", colCheckImportPlugins)
        AddPlugins(Of IDepositImportPlugin)("Deposit Import", colDepositImportPlugins)
        AddPlugins(Of IInvoiceImportPlugin)("Invoice Import", colInvoiceImportPlugins)
        AddPlugins(Of IReportPlugin)("Reports", colReportPlugins)
        AddPlugins(Of IToolPlugin)("Tools", colToolPlugins)
        Me.ShowDialog()
    End Sub

    Private Sub AddPlugins(Of TPlugin As IToolPlugin)(ByVal strType As String, ByVal colPlugins As List(Of TPlugin))
        For Each objPlugin As TPlugin In colPlugins
            Dim item As ListViewItem = New ListViewItem(strType)
            item.SubItems.Add(objPlugin.GetMenuTitle())
            item.SubItems.Add(objPlugin.SortCode)
            item.SubItems.Add(System.IO.Path.GetFileName(objPlugin.GetType().Assembly.Location))
            lvwPlugins.Items.Add(item)
        Next
    End Sub


End Class