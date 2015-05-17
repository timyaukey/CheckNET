Option Strict Off
Option Explicit On

Imports CheckBookLib

Friend Class RegPropertiesForm
    Inherits System.Windows.Forms.Form

    Private mobjReg As Register

    Public Sub ShowModal(ByVal objReg As Register)
        Dim frm As System.Windows.Forms.Form
        For Each frm In gcolForms()
            'UPGRADE_WARNING: TypeOf has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
            If Not (TypeOf frm Is ShowRegisterForm) And Not (TypeOf frm Is CBMainForm) Then
                MsgBox("Register properties may not be edited if any other windows are open.", MsgBoxStyle.Critical)
                Exit Sub
            End If
        Next frm
        mobjReg = objReg
        txtTitle.Text = mobjReg.strTitle
        chkShowInitially.CheckState = IIf(mobjReg.blnShowInitially, System.Windows.Forms.CheckState.Checked, System.Windows.Forms.CheckState.Unchecked)
        chkNonBank.CheckState = IIf(mobjReg.blnNonBank, System.Windows.Forms.CheckState.Checked, System.Windows.Forms.CheckState.Unchecked)
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
        mobjReg.blnNonBank = (chkNonBank.CheckState = System.Windows.Forms.CheckState.Checked)
        Me.Close()
    End Sub
End Class