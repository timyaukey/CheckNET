Option Strict Off
Option Explicit On

Imports CheckBookLib

Friend Class MoveDstForm
    Inherits System.Windows.Forms.Form

    Private mblnSuccess As Boolean
    Private mcolLoadedRegisters As Collection
    Private mobjOldReg As Register
    Private mstrNewDate As String
    Private mobjNewReg As Register

    Public Function blnShowModal(ByVal colLoadedRegisters As Collection, ByVal objOldReg As Register, ByRef strNewDate As String, ByRef objNewReg As Register) As Boolean

        Dim objLoadedReg As LoadedRegister
        Dim intRegIdx As Short

        mblnSuccess = False
        mcolLoadedRegisters = colLoadedRegisters
        mobjOldReg = objOldReg
        cboRegister.Items.Add(gobjCreateListBoxItem("", -1))
        For intRegIdx = 1 To mcolLoadedRegisters.Count()
            objLoadedReg = mcolLoadedRegisters.Item(intRegIdx)
            cboRegister.Items.Add(gobjCreateListBoxItem(objLoadedReg.objReg.strTitle, intRegIdx))
        Next
        cboRegister.SelectedIndex = 0
        Me.ShowDialog()
        blnShowModal = mblnSuccess
        If mblnSuccess Then
            strNewDate = mstrNewDate
            objNewReg = mobjNewReg
        End If
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdOkay_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOkay.Click
        Dim strDateOrDays As String
        Dim objNewLoadedReg As LoadedRegister
        Dim objNewReg As Register

        strDateOrDays = Trim(txtDateOrDays.Text)
        If strDateOrDays = "" And cboRegister.SelectedIndex < 1 Then
            MsgBox("At least one of the inputs must be specified.")
            Exit Sub
        End If
        If strDateOrDays <> "" Then
            If Not (gblnValidDate(strDateOrDays) Or IsNumeric(strDateOrDays)) Then
                MsgBox("Invalid date or number of days.")
                Exit Sub
            End If
        End If
        If cboRegister.SelectedIndex > 0 Then
            objNewLoadedReg = mcolLoadedRegisters.Item(VB6.GetItemData(cboRegister, cboRegister.SelectedIndex))
            objNewReg = objNewLoadedReg.objReg
            If objNewReg Is mobjOldReg Then
                MsgBox("You may not choose the same register (select no register " & "to leave in the same register.")
                Exit Sub
            End If
        Else
            'UPGRADE_NOTE: Object objNewReg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            objNewReg = Nothing
        End If

        mstrNewDate = strDateOrDays
        mobjNewReg = objNewReg
        mblnSuccess = True

        Me.Close()
    End Sub
End Class