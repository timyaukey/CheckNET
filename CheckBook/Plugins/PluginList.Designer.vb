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
        Me.PluginName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.PluginVersion = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.PluginManufacturer = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.PluginAssembly = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.lblDescription = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.lblInfoURL = New System.Windows.Forms.Label()
        Me.txtInfoURL = New System.Windows.Forms.TextBox()
        Me.lblLicense = New System.Windows.Forms.Label()
        Me.txtLicense = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'lvwPlugins
        '
        Me.lvwPlugins.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwPlugins.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.PluginName, Me.PluginVersion, Me.PluginManufacturer, Me.PluginAssembly})
        Me.lvwPlugins.FullRowSelect = True
        Me.lvwPlugins.GridLines = True
        Me.lvwPlugins.HideSelection = False
        Me.lvwPlugins.Location = New System.Drawing.Point(12, 12)
        Me.lvwPlugins.Name = "lvwPlugins"
        Me.lvwPlugins.Size = New System.Drawing.Size(847, 365)
        Me.lvwPlugins.TabIndex = 0
        Me.lvwPlugins.UseCompatibleStateImageBehavior = False
        Me.lvwPlugins.View = System.Windows.Forms.View.Details
        '
        'PluginName
        '
        Me.PluginName.Text = "Plugin Name"
        Me.PluginName.Width = 200
        '
        'PluginVersion
        '
        Me.PluginVersion.Text = "Version"
        '
        'PluginManufacturer
        '
        Me.PluginManufacturer.Text = "Manufacturer"
        Me.PluginManufacturer.Width = 200
        '
        'PluginAssembly
        '
        Me.PluginAssembly.Text = "Assembly"
        Me.PluginAssembly.Width = 1000
        '
        'lblDescription
        '
        Me.lblDescription.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(9, 380)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(63, 13)
        Me.lblDescription.TabIndex = 1
        Me.lblDescription.Text = "Description:"
        '
        'txtDescription
        '
        Me.txtDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDescription.Location = New System.Drawing.Point(12, 396)
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDescription.Size = New System.Drawing.Size(847, 70)
        Me.txtDescription.TabIndex = 2
        '
        'lblInfoURL
        '
        Me.lblInfoURL.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblInfoURL.AutoSize = True
        Me.lblInfoURL.Location = New System.Drawing.Point(9, 469)
        Me.lblInfoURL.Name = "lblInfoURL"
        Me.lblInfoURL.Size = New System.Drawing.Size(87, 13)
        Me.lblInfoURL.TabIndex = 3
        Me.lblInfoURL.Text = "Information URL:"
        '
        'txtInfoURL
        '
        Me.txtInfoURL.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtInfoURL.Location = New System.Drawing.Point(12, 485)
        Me.txtInfoURL.Name = "txtInfoURL"
        Me.txtInfoURL.ReadOnly = True
        Me.txtInfoURL.Size = New System.Drawing.Size(847, 20)
        Me.txtInfoURL.TabIndex = 4
        '
        'lblLicense
        '
        Me.lblLicense.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLicense.AutoSize = True
        Me.lblLicense.Location = New System.Drawing.Point(9, 508)
        Me.lblLicense.Name = "lblLicense"
        Me.lblLicense.Size = New System.Drawing.Size(47, 13)
        Me.lblLicense.TabIndex = 5
        Me.lblLicense.Text = "License:"
        '
        'txtLicense
        '
        Me.txtLicense.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLicense.Location = New System.Drawing.Point(12, 524)
        Me.txtLicense.Name = "txtLicense"
        Me.txtLicense.ReadOnly = True
        Me.txtLicense.Size = New System.Drawing.Size(847, 20)
        Me.txtLicense.TabIndex = 6
        '
        'PluginList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(871, 562)
        Me.Controls.Add(Me.txtLicense)
        Me.Controls.Add(Me.lblLicense)
        Me.Controls.Add(Me.txtInfoURL)
        Me.Controls.Add(Me.lblInfoURL)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.lvwPlugins)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PluginList"
        Me.Text = "Plugin List"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lvwPlugins As ListView
    Friend WithEvents PluginManufacturer As ColumnHeader
    Friend WithEvents PluginName As ColumnHeader
    Friend WithEvents PluginVersion As ColumnHeader
    Friend WithEvents PluginAssembly As ColumnHeader
    Friend WithEvents lblDescription As Label
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents lblInfoURL As Label
    Friend WithEvents txtInfoURL As TextBox
    Friend WithEvents lblLicense As Label
    Friend WithEvents txtLicense As TextBox
End Class
