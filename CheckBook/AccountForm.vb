Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class AccountForm
    Private mblnUpdateMode As Boolean

    Public Overloads Function ShowDialog(ByRef strTitle As String, ByRef strFileName As String, ByRef lngAccountType As Account.AccountType,
                                         ByVal blnUpdateMode As Boolean, ByVal blnReadOnly As Boolean) As DialogResult
        mblnUpdateMode = blnUpdateMode
        txtAccountName.Text = strTitle
        txtFileName.Text = strFileName
        cboAccountType.Items.Add(New AccountTypeItem(Account.AccountType.Asset, "Asset"))
        cboAccountType.Items.Add(New AccountTypeItem(Account.AccountType.Liability, "Liability"))
        cboAccountType.Items.Add(New AccountTypeItem(Account.AccountType.Equity, "Equity"))
        For Each objItem As AccountTypeItem In cboAccountType.Items
            If objItem.lngType = lngAccountType Then
                cboAccountType.SelectedItem = objItem
                Exit For
            End If
        Next
        txtAccountName.Enabled = (Not blnReadOnly)
        txtFileName.Enabled = (Not mblnUpdateMode) And (Not blnReadOnly)
        cboAccountType.Enabled = (Not mblnUpdateMode) And (Not blnReadOnly)
        btnOkay.Enabled = (Not blnReadOnly)
        Dim result As DialogResult = Me.ShowDialog()
        If result = DialogResult.OK Then
            strTitle = txtAccountName.Text
            strFileName = txtFileName.Text
            lngAccountType = DirectCast(cboAccountType.SelectedItem, AccountTypeItem).lngType
        End If
        Return result
    End Function

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

    Private Class AccountTypeItem
        Private mlngType As Account.AccountType
        Private mstrName As String

        Public Sub New(ByVal lngType_ As Account.AccountType, ByVal strName_ As String)
            mlngType = lngType_
            mstrName = strName_
        End Sub

        Public ReadOnly Property lngType() As Account.AccountType
            Get
                Return mlngType
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return mstrName
        End Function
    End Class
End Class