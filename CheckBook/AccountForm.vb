﻿Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class AccountForm
    Private mblnUpdateMode As Boolean
    Private mobjCompany As Company

    Public Overloads Function ShowDialog(ByRef objAccount As Account, ByVal blnUpdateMode As Boolean, ByVal blnReadOnly As Boolean) As DialogResult
        mblnUpdateMode = blnUpdateMode
        mobjCompany = objAccount.objCompany
        txtAccountName.Text = objAccount.strTitle
        txtFileName.Text = objAccount.strFileNameRoot
        For Each objSubType As Account.SubTypeDef In Account.arrSubTypeDefs
            cboAccountType.Items.Add(objSubType)
        Next
        For Each objItem As Account.SubTypeDef In cboAccountType.Items
            If objItem.lngSubType = objAccount.lngSubType Then
                cboAccountType.SelectedItem = objItem
                Exit For
            End If
        Next
        txtAccountName.Enabled = (Not blnReadOnly)
        txtFileName.Enabled = (Not mblnUpdateMode) And (Not blnReadOnly)
        cboAccountType.Enabled = (Not blnReadOnly)
        LoadAccountList(cboRelated1, objAccount.objRelatedAcct1, blnReadOnly)
        LoadAccountList(cboRelated2, objAccount.objRelatedAcct2, blnReadOnly)
        LoadAccountList(cboRelated3, objAccount.objRelatedAcct3, blnReadOnly)
        LoadAccountList(cboRelated4, objAccount.objRelatedAcct4, blnReadOnly)
        btnOkay.Enabled = (Not blnReadOnly)
        Dim result As DialogResult = Me.ShowDialog()
        If result = DialogResult.OK Then
            objAccount.strTitle = txtAccountName.Text
            objAccount.lngSubType = DirectCast(cboAccountType.SelectedItem, Account.SubTypeDef).lngSubType
            objAccount.objRelatedAcct1 = DirectCast(cboRelated1.SelectedItem, AccountItem).objAccount
            objAccount.objRelatedAcct2 = DirectCast(cboRelated2.SelectedItem, AccountItem).objAccount
            objAccount.objRelatedAcct3 = DirectCast(cboRelated3.SelectedItem, AccountItem).objAccount
            objAccount.objRelatedAcct4 = DirectCast(cboRelated4.SelectedItem, AccountItem).objAccount
            If Not blnUpdateMode Then
                objAccount.strFileNameRoot = txtFileName.Text
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
        For Each objAccount As Account In mobjCompany.colAccounts
            objItem = New AccountItem()
            objItem.objAccount = objAccount
            objItem.strTitle = objAccount.strType + ":" + objAccount.strTitle
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
            MsgBox("Account name is required.")
            Exit Sub
        End If
        If Not mblnUpdateMode Then
            If txtFileName.Text.Length = 0 Then
                MsgBox("File name is required.")
                Exit Sub
            End If
            For Each c As Char In ":/\.,!@$%^&()={}[]|<>()*?"
                If txtFileName.Text.Contains(c) Then
                    MsgBox("Invalid character in file name.")
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