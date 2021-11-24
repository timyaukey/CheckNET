Option Strict On
Option Explicit On

Public Class PluginList

    Public Sub ShowMe(ByVal objHostSetup As IHostSetup, ByVal objPlugins As IEnumerable(Of IPlugin))
        Dim strName As String
        Dim strVersion As String
        Dim strManufacturer As String
        Dim strPath As String
        lvwPlugins.Items.Clear()
        For Each objPlugin As IPlugin In objPlugins
            strName = "(none)"
            strVersion = ""
            strManufacturer = "(unknown)"
            strPath = ""
            If Not objPlugin.Metadata Is Nothing Then
                If Not String.IsNullOrEmpty(objPlugin.Metadata.PluginName) Then
                    strName = objPlugin.Metadata.PluginName
                End If
                If Not objPlugin.Metadata.Version Is Nothing Then
                    strVersion = objPlugin.Metadata.Version.ToString()
                End If
                If Not String.IsNullOrEmpty(objPlugin.Metadata.Manufacturer) Then
                    strManufacturer = objPlugin.Metadata.Manufacturer
                End If
                If Not objPlugin.Metadata.Assembly Is Nothing Then
                    strPath = objPlugin.Metadata.Assembly.Location
                End If
            End If
            Dim item As ListViewItem = New ListViewItem(strName)
            item.SubItems.Add(strVersion)
            item.SubItems.Add(strManufacturer)
            item.SubItems.Add(strPath)
            item.Tag = objPlugin
            lvwPlugins.Items.Add(item)
        Next
        Me.ShowDialog()
    End Sub

    Private Sub lvwPlugins_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvwPlugins.SelectedIndexChanged
        If lvwPlugins.SelectedItems.Count > 0 Then
            Dim objPlugin As IPlugin = DirectCast(lvwPlugins.SelectedItems(0).Tag, IPlugin)
            Dim objMetadata As PluginMetadata = objPlugin.Metadata
            If Not objMetadata Is Nothing Then
                txtDescription.Text = objMetadata.Description
                If String.IsNullOrEmpty(objMetadata.InfoURL) Then
                    txtInfoURL.Text = "(none)"
                Else
                    txtInfoURL.Text = objMetadata.InfoURL
                End If
                If objMetadata.License Is Nothing Then
                    txtLicense.Text = "(license not needed)"
                Else
                    txtLicense.Text = "Licensed To: " + objMetadata.License.LicensedTo + "; License Type: " +
                        objMetadata.License.LicenseTitle + "; Serial: " + objMetadata.License.SerialNumber
                End If
            End If
        End If
    End Sub
End Class