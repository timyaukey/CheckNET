Option Strict On
Option Explicit On


Public Class AccountForm
    Private mblnUpdateMode As Boolean
    Private mobjHostUI As IHostUI
    Private mobjCompany As Company

    Public Overloads Function ShowDialog(ByVal objHostUI As IHostUI, ByRef objAccount As Account, ByVal blnUpdateMode As Boolean, ByVal blnReadOnly As Boolean) As DialogResult
        mblnUpdateMode = blnUpdateMode
        mobjHostUI = objHostUI
        mobjCompany = mobjHostUI.objCompany
        txtAccountName.Text = objAccount.Title
        txtFileName.Text = objAccount.FileNameRoot
        For Each objSubType As Account.SubTypeDef In Account.SubTypeDefs
            cboAccountType.Items.Add(objSubType)
        Next
        For Each objItem As Account.SubTypeDef In cboAccountType.Items
            If objItem.lngSubType = objAccount.AcctSubType Then
                cboAccountType.SelectedItem = objItem
                Exit For
            End If
        Next
        txtAccountName.Enabled = (Not blnReadOnly)
        txtFileName.Enabled = (Not mblnUpdateMode) And (Not blnReadOnly)
        cboAccountType.Enabled = (Not blnReadOnly)
        LoadAccountList(cboRelated1, objAccount.RelatedAcct1, blnReadOnly)
        LoadAccountList(cboRelated2, objAccount.RelatedAcct2, blnReadOnly)
        LoadAccountList(cboRelated3, objAccount.RelatedAcct3, blnReadOnly)
        LoadAccountList(cboRelated4, objAccount.RelatedAcct4, blnReadOnly)
        btnOkay.Enabled = (Not blnReadOnly)
        Dim result As DialogResult = Me.ShowDialog()
        If result = DialogResult.OK Then
            objAccount.Title = txtAccountName.Text
            objAccount.AcctSubType = DirectCast(cboAccountType.SelectedItem, Account.SubTypeDef).lngSubType
            objAccount.RelatedAcct1 = DirectCast(cboRelated1.SelectedItem, AccountItem).objAccount
            objAccount.RelatedAcct2 = DirectCast(cboRelated2.SelectedItem, AccountItem).objAccount
            objAccount.RelatedAcct3 = DirectCast(cboRelated3.SelectedItem, AccountItem).objAccount
            objAccount.RelatedAcct4 = DirectCast(cboRelated4.SelectedItem, AccountItem).objAccount
            If Not blnUpdateMode Then
                objAccount.FileNameRoot = txtFileName.Text
            End If
            Return result
        End If
    End Function

    Private Sub LoadAccountList(ByVal ctl As ComboBox, ByVal objSelectedAccount As Account, ByVal blnReadOnly As Boolean)
        Dim objItem As AccountItem
        Dim objSelectedItem As AccountItem
        ctl.Items.Clear()
        objItem = New AccountItem()
        objItem.objAccount = Nothing
        objItem.strTitle = "(none)"
        objSelectedItem = objItem
        ctl.Items.Add(objItem)
        For Each objAccount As Account In mobjCompany.Accounts
            objItem = New AccountItem()
            objItem.objAccount = objAccount
            objItem.strTitle = objAccount.AccountTypeLetter + ":" + objAccount.Title
            ctl.Items.Add(objItem)
            If objAccount Is objSelectedAccount Then
                objSelectedItem = objItem
            End If
        Next
        ctl.SelectedItem = objSelectedItem
        ctl.Enabled = (Not blnReadOnly)
    End Sub

    Private Sub btnOkay_Click(sender As Object, e As EventArgs) Handles btnOkay.Click
        If txtAccountName.Text.Length = 0 Then
            mobjHostUI.InfoMessageBox("Account name is required.")
            Exit Sub
        End If
        If Not mblnUpdateMode Then
            If txtFileName.Text.Length = 0 Then
                mobjHostUI.InfoMessageBox("File name is required.")
                Exit Sub
            End If
            For Each c As Char In ":/\.,!@$%^&()={}[]|<>()*?"
                If txtFileName.Text.Contains(c) Then
                    mobjHostUI.InfoMessageBox("Invalid character in file name.")
                    Exit Sub
                End If
            Next
        End If
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Class AccountItem
        Public objAccount As Account
        Public strTitle As String

        Public Overrides Function ToString() As String
            Return strTitle
        End Function
    End Class

End Class