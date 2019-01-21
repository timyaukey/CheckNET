<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PluginList
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lvwPlugins = New System.Windows.Forms.ListView()
        Me.PluginType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.PluginName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.PluginSort = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.PluginAssembly = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SuspendLayout()
        '
        'lvwPlugins
        '
        Me.lvwPlugins.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwPlugins.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.PluginType, Me.PluginName, Me.PluginSort, Me.PluginAssembly})
        Me.lvwPlugins.Location = New System.Drawing.Point(12, 12)
        Me.lvwPlugins.Name = "lvwPlugins"
        Me.lvwPlugins.Size = New System.Drawing.Size(742, 394)
        Me.lvwPlugins.TabIndex = 0
        Me.lvwPlugins.UseCompatibleStateImageBehavior = False
        Me.lvwPlugins.View = System.Windows.Forms.View.Details
        '
        'PluginType
        '
        Me.PluginType.Text = "Type"
        Me.PluginType.Width = 100
        '
        'PluginName
        '
        Me.PluginName.Text = "Plugin Name"
        Me.PluginName.Width = 300
        '
        'PluginSort
        '
        Me.PluginSort.Text = "Sort Code"
        '
        'PluginAssembly
        '
        Me.PluginAssembly.Text = "Assembly"
        Me.PluginAssembly.Width = 250
        '
        'PluginList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(766, 418)
        Me.Controls.Add(Me.lvwPlugins)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PluginList"
        Me.Text = "Plugin List"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lvwPlugins As ListView
    Friend WithEvents PluginType As ColumnHeader
    Friend WithEvents PluginName As ColumnHeader
    Friend WithEvents PluginSort As ColumnHeader
    Friend WithEvents PluginAssembly As ColumnHeader
End Class
