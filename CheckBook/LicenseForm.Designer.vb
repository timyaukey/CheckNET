<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LicenseForm
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
        Me.lblLicenseTitle = New System.Windows.Forms.Label()
        Me.txtLicenseTitle = New System.Windows.Forms.TextBox()
        Me.lblLicensedTo = New System.Windows.Forms.Label()
        Me.txtLicensedTo = New System.Windows.Forms.TextBox()
        Me.txtLicenseVersion = New System.Windows.Forms.TextBox()
        Me.lblLicenseVersion = New System.Windows.Forms.Label()
        Me.txtExpirationDate = New System.Windows.Forms.TextBox()
        Me.lblExpirationDate = New System.Windows.Forms.Label()
        Me.txtEmailAddress = New System.Windows.Forms.TextBox()
        Me.lblEmailAddress = New System.Windows.Forms.Label()
        Me.txtDetails = New System.Windows.Forms.TextBox()
        Me.lblDetails = New System.Windows.Forms.Label()
        Me.txtSerialNumber = New System.Windows.Forms.TextBox()
        Me.lblSerialNumber = New System.Windows.Forms.Label()
        Me.txtLicenseStatus = New System.Windows.Forms.TextBox()
        Me.lblLicenseStatus = New System.Windows.Forms.Label()
        Me.btnInstall = New System.Windows.Forms.Button()
        Me.btnRemove = New System.Windows.Forms.Button()
        Me.dlgOpenLicenseFile = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'lblLicenseTitle
        '
        Me.lblLicenseTitle.AutoSize = True
        Me.lblLicenseTitle.Location = New System.Drawing.Point(12, 9)
        Me.lblLicenseTitle.Name = "lblLicenseTitle"
        Me.lblLicenseTitle.Size = New System.Drawing.Size(74, 13)
        Me.lblLicenseTitle.TabIndex = 0
        Me.lblLicenseTitle.Text = "License Type:"
        '
        'txtLicenseTitle
        '
        Me.txtLicenseTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLicenseTitle.Enabled = False
        Me.txtLicenseTitle.Location = New System.Drawing.Point(114, 6)
        Me.txtLicenseTitle.Name = "txtLicenseTitle"
        Me.txtLicenseTitle.ReadOnly = True
        Me.txtLicenseTitle.Size = New System.Drawing.Size(366, 20)
        Me.txtLicenseTitle.TabIndex = 1
        '
        'lblLicensedTo
        '
        Me.lblLicensedTo.AutoSize = True
        Me.lblLicensedTo.Location = New System.Drawing.Point(12, 36)
        Me.lblLicensedTo.Name = "lblLicensedTo"
        Me.lblLicensedTo.Size = New System.Drawing.Size(69, 13)
        Me.lblLicensedTo.TabIndex = 2
        Me.lblLicensedTo.Text = "Licensed To:"
        '
        'txtLicensedTo
        '
        Me.txtLicensedTo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLicensedTo.Enabled = False
        Me.txtLicensedTo.Location = New System.Drawing.Point(114, 33)
        Me.txtLicensedTo.Name = "txtLicensedTo"
        Me.txtLicensedTo.ReadOnly = True
        Me.txtLicensedTo.Size = New System.Drawing.Size(366, 20)
        Me.txtLicensedTo.TabIndex = 3
        '
        'txtLicenseVersion
        '
        Me.txtLicenseVersion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLicenseVersion.Enabled = False
        Me.txtLicenseVersion.Location = New System.Drawing.Point(114, 163)
        Me.txtLicenseVersion.Name = "txtLicenseVersion"
        Me.txtLicenseVersion.ReadOnly = True
        Me.txtLicenseVersion.Size = New System.Drawing.Size(366, 20)
        Me.txtLicenseVersion.TabIndex = 13
        '
        'lblLicenseVersion
        '
        Me.lblLicenseVersion.AutoSize = True
        Me.lblLicenseVersion.Location = New System.Drawing.Point(12, 166)
        Me.lblLicenseVersion.Name = "lblLicenseVersion"
        Me.lblLicenseVersion.Size = New System.Drawing.Size(85, 13)
        Me.lblLicenseVersion.TabIndex = 12
        Me.lblLicenseVersion.Text = "License Version:"
        '
        'txtExpirationDate
        '
        Me.txtExpirationDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtExpirationDate.Enabled = False
        Me.txtExpirationDate.Location = New System.Drawing.Point(114, 85)
        Me.txtExpirationDate.Name = "txtExpirationDate"
        Me.txtExpirationDate.ReadOnly = True
        Me.txtExpirationDate.Size = New System.Drawing.Size(366, 20)
        Me.txtExpirationDate.TabIndex = 7
        '
        'lblExpirationDate
        '
        Me.lblExpirationDate.AutoSize = True
        Me.lblExpirationDate.Location = New System.Drawing.Point(12, 88)
        Me.lblExpirationDate.Name = "lblExpirationDate"
        Me.lblExpirationDate.Size = New System.Drawing.Size(82, 13)
        Me.lblExpirationDate.TabIndex = 6
        Me.lblExpirationDate.Text = "Expiration Date:"
        '
        'txtEmailAddress
        '
        Me.txtEmailAddress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEmailAddress.Enabled = False
        Me.txtEmailAddress.Location = New System.Drawing.Point(114, 59)
        Me.txtEmailAddress.Name = "txtEmailAddress"
        Me.txtEmailAddress.ReadOnly = True
        Me.txtEmailAddress.Size = New System.Drawing.Size(366, 20)
        Me.txtEmailAddress.TabIndex = 5
        '
        'lblEmailAddress
        '
        Me.lblEmailAddress.AutoSize = True
        Me.lblEmailAddress.Location = New System.Drawing.Point(12, 62)
        Me.lblEmailAddress.Name = "lblEmailAddress"
        Me.lblEmailAddress.Size = New System.Drawing.Size(76, 13)
        Me.lblEmailAddress.TabIndex = 4
        Me.lblEmailAddress.Text = "Email Address:"
        '
        'txtDetails
        '
        Me.txtDetails.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtDetails.Enabled = False
        Me.txtDetails.Location = New System.Drawing.Point(114, 111)
        Me.txtDetails.Name = "txtDetails"
        Me.txtDetails.ReadOnly = True
        Me.txtDetails.Size = New System.Drawing.Size(366, 20)
        Me.txtDetails.TabIndex = 9
        '
        'lblDetails
        '
        Me.lblDetails.AutoSize = True
        Me.lblDetails.Location = New System.Drawing.Point(12, 114)
        Me.lblDetails.Name = "lblDetails"
        Me.lblDetails.Size = New System.Drawing.Size(42, 13)
        Me.lblDetails.TabIndex = 8
        Me.lblDetails.Text = "Details:"
        '
        'txtSerialNumber
        '
        Me.txtSerialNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSerialNumber.Enabled = False
        Me.txtSerialNumber.Location = New System.Drawing.Point(114, 137)
        Me.txtSerialNumber.Name = "txtSerialNumber"
        Me.txtSerialNumber.ReadOnly = True
        Me.txtSerialNumber.Size = New System.Drawing.Size(366, 20)
        Me.txtSerialNumber.TabIndex = 11
        '
        'lblSerialNumber
        '
        Me.lblSerialNumber.AutoSize = True
        Me.lblSerialNumber.Location = New System.Drawing.Point(12, 140)
        Me.lblSerialNumber.Name = "lblSerialNumber"
        Me.lblSerialNumber.Size = New System.Drawing.Size(76, 13)
        Me.lblSerialNumber.TabIndex = 10
        Me.lblSerialNumber.Text = "Serial Number:"
        '
        'txtLicenseStatus
        '
        Me.txtLicenseStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtLicenseStatus.Enabled = False
        Me.txtLicenseStatus.Location = New System.Drawing.Point(114, 189)
        Me.txtLicenseStatus.Name = "txtLicenseStatus"
        Me.txtLicenseStatus.ReadOnly = True
        Me.txtLicenseStatus.Size = New System.Drawing.Size(366, 20)
        Me.txtLicenseStatus.TabIndex = 15
        '
        'lblLicenseStatus
        '
        Me.lblLicenseStatus.AutoSize = True
        Me.lblLicenseStatus.Location = New System.Drawing.Point(12, 192)
        Me.lblLicenseStatus.Name = "lblLicenseStatus"
        Me.lblLicenseStatus.Size = New System.Drawing.Size(80, 13)
        Me.lblLicenseStatus.TabIndex = 14
        Me.lblLicenseStatus.Text = "License Status:"
        '
        'btnInstall
        '
        Me.btnInstall.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnInstall.Location = New System.Drawing.Point(268, 215)
        Me.btnInstall.Name = "btnInstall"
        Me.btnInstall.Size = New System.Drawing.Size(103, 23)
        Me.btnInstall.TabIndex = 16
        Me.btnInstall.Text = "Install License"
        Me.btnInstall.UseVisualStyleBackColor = True
        '
        'btnRemove
        '
        Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemove.Location = New System.Drawing.Point(377, 215)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(103, 23)
        Me.btnRemove.TabIndex = 17
        Me.btnRemove.Text = "Remove License"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'dlgOpenLicenseFile
        '
        Me.dlgOpenLicenseFile.Filter = "License files|*.lic|All files|*.*"
        Me.dlgOpenLicenseFile.ShowReadOnly = True
        Me.dlgOpenLicenseFile.Title = "Select License File To Install"
        '
        'LicenseForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(495, 250)
        Me.Controls.Add(Me.btnRemove)
        Me.Controls.Add(Me.btnInstall)
        Me.Controls.Add(Me.txtLicenseStatus)
        Me.Controls.Add(Me.lblLicenseStatus)
        Me.Controls.Add(Me.txtSerialNumber)
        Me.Controls.Add(Me.lblSerialNumber)
        Me.Controls.Add(Me.txtDetails)
        Me.Controls.Add(Me.lblDetails)
        Me.Controls.Add(Me.txtEmailAddress)
        Me.Controls.Add(Me.lblEmailAddress)
        Me.Controls.Add(Me.txtExpirationDate)
        Me.Controls.Add(Me.lblExpirationDate)
        Me.Controls.Add(Me.txtLicenseVersion)
        Me.Controls.Add(Me.lblLicenseVersion)
        Me.Controls.Add(Me.txtLicensedTo)
        Me.Controls.Add(Me.lblLicensedTo)
        Me.Controls.Add(Me.txtLicenseTitle)
        Me.Controls.Add(Me.lblLicenseTitle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LicenseForm"
        Me.ShowInTaskbar = False
        Me.Text = "License Information"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblLicenseTitle As Label
    Friend WithEvents txtLicenseTitle As TextBox
    Friend WithEvents lblLicensedTo As Label
    Friend WithEvents txtLicensedTo As TextBox
    Friend WithEvents txtLicenseVersion As TextBox
    Friend WithEvents lblLicenseVersion As Label
    Friend WithEvents txtExpirationDate As TextBox
    Friend WithEvents lblExpirationDate As Label
    Friend WithEvents txtEmailAddress As TextBox
    Friend WithEvents lblEmailAddress As Label
    Friend WithEvents txtDetails As TextBox
    Friend WithEvents lblDetails As Label
    Friend WithEvents txtSerialNumber As TextBox
    Friend WithEvents lblSerialNumber As Label
    Friend WithEvents txtLicenseStatus As TextBox
    Friend WithEvents lblLicenseStatus As Label
    Friend WithEvents btnInstall As Button
    Friend WithEvents btnRemove As Button
    Friend WithEvents dlgOpenLicenseFile As OpenFileDialog
End Class
