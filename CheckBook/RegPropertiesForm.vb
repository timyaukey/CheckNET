Option Strict On
Option Explicit On

Imports CheckBookLib

Friend Class RegPropertiesForm
    Inherits System.Windows.Forms.Form

    Private mobjReg As Register

    Public Sub ShowModal(ByVal objReg As Register, ByVal blnReadOnly As Boolean)
        Dim frm As System.Windows.Forms.Form
        For Each frm In gcolForms()
            If Not (TypeOf frm Is ShowRegisterForm) And Not (TypeOf frm Is CBMainForm) Then
                MsgBox("Register properties may not be edited if any other windows are open.", MsgBoxStyle.Critical)
                Exit Sub
            End If
        Next frm
        mobjReg = objReg
        txtTitle.Text = mobjReg.strTitle
        txtTitle.Enabled = Not blnReadOnly
        chkShowInitially.CheckState = DirectCast(IIf(mobjReg.blnShowInitially, CheckState.Checked, CheckState.Unchecked), CheckState)
        chkShowInitially.Enabled = Not blnReadOnly
        cmdOkay.Enabled = Not blnReadOnly
        Me.ShowDialog()
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        If Len(Trim(txtTitle.Text)) < 2 Then
            MsgBox("Register title must be at least 2 letters.")
            Exit Sub
        End If
        mobjReg.strTitle = Trim(txtTitle.Text)
        mobjReg.blnShowInitially = (chkShowInitially.CheckState = System.Windows.Forms.CheckState.Checked)
        Me.Close()
    End Sub
End Class