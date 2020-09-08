<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LicenseListForm
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
        Me.lvwLicenses = New System.Windows.Forms.ListView()
        Me.btnManageLicense = New System.Windows.Forms.Button()
        Me.colLicenseTitle = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colLicensedTo = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colExpirationDate = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colLicenseStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SuspendLayout()
        '
        'lvwLicenses
        '
        Me.lvwLicenses.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwLicenses.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colLicenseTitle, Me.colLicensedTo, Me.colExpirationDate, Me.colLicenseStatus})
        Me.lvwLicenses.FullRowSelect = True
        Me.lvwLicenses.GridLines = True
        Me.lvwLicenses.HideSelection = False
        Me.lvwLicenses.Location = New System.Drawing.Point(12, 12)
        Me.lvwLicenses.MultiSelect = False
        Me.lvwLicenses.Name = "lvwLicenses"
        Me.lvwLicenses.Size = New System.Drawing.Size(628, 289)
        Me.lvwLicenses.TabIndex = 0
        Me.lvwLicenses.UseCompatibleStateImageBehavior = False
        Me.lvwLicenses.View = System.Windows.Forms.View.Details
        '
        'btnManageLicense
        '
        Me.btnManageLicense.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnManageLicense.Location = New System.Drawing.Point(440, 312)
        Me.btnManageLicense.Name = "btnManageLicense"
        Me.btnManageLicense.Size = New System.Drawing.Size(200, 23)
        Me.btnManageLicense.TabIndex = 1
        Me.btnManageLicense.Text = "Manage Selected License"
        Me.btnManageLicense.UseVisualStyleBackColor = True
        '
        'colLicenseTitle
        '
        Me.colLicenseTitle.Text = "License Title"
        Me.colLicenseTitle.Width = 200
        '
        'colLicensedTo
        '
        Me.colLicensedTo.Text = "Licensed To"
        Me.colLicensedTo.Width = 200
        '
        'colExpirationDate
        '
        Me.colExpirationDate.Text = "Expiration Date"
        Me.colExpirationDate.Width = 100
        '
        'colLicenseStatus
        '
        Me.colLicenseStatus.Text = "Status"
        Me.colLicenseStatus.Width = 100
        '
        'LicenseListForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(652, 347)
        Me.Controls.Add(Me.btnManageLicense)
        Me.Controls.Add(Me.lvwLicenses)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LicenseListForm"
        Me.ShowInTaskbar = False
        Me.Text = "Software Licenses"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lvwLicenses As ListView
    Friend WithEvents btnManageLicense As Button
    Friend WithEvents colLicenseTitle As ColumnHeader
    Friend WithEvents colLicensedTo As ColumnHeader
    Friend WithEvents colExpirationDate As ColumnHeader
    Friend WithEvents colLicenseStatus As ColumnHeader
End Class
