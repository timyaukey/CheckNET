<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CheckFormatEditor
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CheckFormatEditor))
        Me.prpDetails = New System.Windows.Forms.PropertyGrid()
        Me.lblInstructions = New System.Windows.Forms.Label()
        Me.cmdOkay = New System.Windows.Forms.Button()
        Me.cmdCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'prpDetails
        '
        Me.prpDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.prpDetails.Location = New System.Drawing.Point(12, 47)
        Me.prpDetails.Name = "prpDetails"
        Me.prpDetails.PropertySort = System.Windows.Forms.PropertySort.NoSort
        Me.prpDetails.Size = New System.Drawing.Size(539, 432)
        Me.prpDetails.TabIndex = 0
        Me.prpDetails.ToolbarVisible = False
        '
        'lblInstructions
        '
        Me.lblInstructions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInstructions.Location = New System.Drawing.Point(12, 9)
        Me.lblInstructions.Name = "lblInstructions"
        Me.lblInstructions.Size = New System.Drawing.Size(539, 37)
        Me.lblInstructions.TabIndex = 1
        Me.lblInstructions.Text = resources.GetString("lblInstructions.Text")
        '
        'cmdOkay
        '
        Me.cmdOkay.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOkay.Location = New System.Drawing.Point(343, 485)
        Me.cmdOkay.Name = "cmdOkay"
        Me.cmdOkay.Size = New System.Drawing.Size(101, 23)
        Me.cmdOkay.TabIndex = 2
        Me.cmdOkay.Text = "OK"
        Me.cmdOkay.UseVisualStyleBackColor = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.Location = New System.Drawing.Point(450, 485)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(101, 23)
        Me.cmdCancel.TabIndex = 3
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = True
        '
        'CheckFormatEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(563, 520)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOkay)
        Me.Controls.Add(Me.lblInstructions)
        Me.Controls.Add(Me.prpDetails)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CheckFormatEditor"
        Me.Text = "Check Format Editor"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents prpDetails As PropertyGrid
    Friend WithEvents lblInstructions As Label
    Friend WithEvents cmdOkay As Button
    Friend WithEvents cmdCancel As Button
End Class
