<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AdjustPersonalBusinessForm
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
        Me.lblStartDate = New System.Windows.Forms.Label()
        Me.ctlStartDate = New System.Windows.Forms.DateTimePicker()
        Me.ctlEndDate = New System.Windows.Forms.DateTimePicker()
        Me.lblEndDate = New System.Windows.Forms.Label()
        Me.btnDeleteAdjustments = New System.Windows.Forms.Button()
        Me.btnRecreateAdjustments = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblStartDate
        '
        Me.lblStartDate.AutoSize = True
        Me.lblStartDate.Location = New System.Drawing.Point(12, 18)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.Size = New System.Drawing.Size(58, 13)
        Me.lblStartDate.TabIndex = 0
        Me.lblStartDate.Text = "Start Date:"
        '
        'ctlStartDate
        '
        Me.ctlStartDate.Location = New System.Drawing.Point(114, 12)
        Me.ctlStartDate.Name = "ctlStartDate"
        Me.ctlStartDate.Size = New System.Drawing.Size(200, 20)
        Me.ctlStartDate.TabIndex = 1
        '
        'ctlEndDate
        '
        Me.ctlEndDate.Location = New System.Drawing.Point(114, 38)
        Me.ctlEndDate.Name = "ctlEndDate"
        Me.ctlEndDate.Size = New System.Drawing.Size(200, 20)
        Me.ctlEndDate.TabIndex = 3
        '
        'lblEndDate
        '
        Me.lblEndDate.AutoSize = True
        Me.lblEndDate.Location = New System.Drawing.Point(12, 44)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.Size = New System.Drawing.Size(55, 13)
        Me.lblEndDate.TabIndex = 2
        Me.lblEndDate.Text = "End Date:"
        '
        'btnDeleteAdjustments
        '
        Me.btnDeleteAdjustments.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDeleteAdjustments.Location = New System.Drawing.Point(143, 235)
        Me.btnDeleteAdjustments.Name = "btnDeleteAdjustments"
        Me.btnDeleteAdjustments.Size = New System.Drawing.Size(184, 23)
        Me.btnDeleteAdjustments.TabIndex = 4
        Me.btnDeleteAdjustments.Text = "Delete Adjustments"
        Me.btnDeleteAdjustments.UseVisualStyleBackColor = True
        '
        'btnRecreateAdjustments
        '
        Me.btnRecreateAdjustments.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecreateAdjustments.Location = New System.Drawing.Point(143, 264)
        Me.btnRecreateAdjustments.Name = "btnRecreateAdjustments"
        Me.btnRecreateAdjustments.Size = New System.Drawing.Size(184, 23)
        Me.btnRecreateAdjustments.TabIndex = 5
        Me.btnRecreateAdjustments.Text = "(Re)create Adjustments"
        Me.btnRecreateAdjustments.UseVisualStyleBackColor = True
        '
        'AdjustPersonalBusinessForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(339, 299)
        Me.Controls.Add(Me.btnRecreateAdjustments)
        Me.Controls.Add(Me.btnDeleteAdjustments)
        Me.Controls.Add(Me.ctlEndDate)
        Me.Controls.Add(Me.lblEndDate)
        Me.Controls.Add(Me.ctlStartDate)
        Me.Controls.Add(Me.lblStartDate)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AdjustPersonalBusinessForm"
        Me.Text = "Adjust Account For Personal Use"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblStartDate As Label
    Friend WithEvents ctlStartDate As DateTimePicker
    Friend WithEvents ctlEndDate As DateTimePicker
    Friend WithEvents lblEndDate As Label
    Friend WithEvents btnDeleteAdjustments As Button
    Friend WithEvents btnRecreateAdjustments As Button
End Class
