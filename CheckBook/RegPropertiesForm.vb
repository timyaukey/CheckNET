Option Strict On
Option Explicit On


Friend Class RegPropertiesForm
    Inherits System.Windows.Forms.Form

    Private mobjHostUI As IHostUI
    Private mobjReg As Register

    Public Sub ShowModal(ByVal objHostUI As IHostUI, ByVal objReg As Register, ByVal blnReadOnly As Boolean)
        mobjHostUI = objHostUI
        Dim frm As System.Windows.Forms.Form
        For Each frm In gcolForms()
            If Not (TypeOf frm Is ShowRegisterForm) And Not (TypeOf frm Is CBMainForm) Then
                mobjHostUI.ErrorMessageBox("Register properties may not be edited if any other windows are open.")
                Exit Sub
            End If
        Next frm
        mobjReg = objReg
        txtTitle.Text = mobjReg.Title
        txtTitle.Enabled = Not blnReadOnly
        chkShowInitially.CheckState = DirectCast(IIf(mobjReg.ShowInitially, CheckState.Checked, CheckState.Unchecked), CheckState)
        chkShowInitially.Enabled = Not blnReadOnly
        cmdOkay.Enabled = Not blnReadOnly
        Me.ShowDialog()
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        If Len(Trim(txtTitle.Text)) < 2 Then
            mobjHostUI.InfoMessageBox("Register title must be at least 2 letters.")
            Exit Sub
        End If
        mobjReg.Title = Trim(txtTitle.Text)
        mobjReg.ShowInitially = (chkShowInitially.CheckState = System.Windows.Forms.CheckState.Checked)
        Me.Close()
    End Sub
End Class