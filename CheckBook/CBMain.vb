Option Strict On
Option Explicit On


''' <summary>
''' Static methods related to WinForm user interface management.
''' </summary>

Public Module CBMain

    Public Sub Main()
        CBMainForm.Show()
    End Sub

    Public Function gcolForms() As IEnumerable(Of Form)
        Dim frm As System.Windows.Forms.Form
        Dim colResult As List(Of Form)
        colResult = New List(Of Form)
        For Each frm In CBMainForm.MdiChildren
            colResult.Add(frm)
        Next frm
        gcolForms = colResult
    End Function

    Public Function gblnAskAndCreateAccount(ByVal objHostUI As IHostUI) As Boolean
        Dim objAccount As Account
        Dim strFile As String

        objAccount = New Account()
        objAccount.Init(objHostUI.Company)
        objAccount.AccountKey = objHostUI.Company.GetUnusedAccountKey()
        objAccount.AcctSubType = Account.SubType.Liability_LoanPayable

        Using frm As AccountForm = New AccountForm()
            If frm.ShowDialog(objHostUI, objAccount, False, False) = DialogResult.OK Then
                strFile = objHostUI.Company.AccountsFolderPath() & "\" & objAccount.FileNameRoot & ".act"
                If Dir(strFile) <> "" Then
                    objHostUI.ErrorMessageBox("Account file already exists with that name.")
                    Exit Function
                End If
                objAccount.Create()
                Return True
            End If
        End Using
        Return False
    End Function

End Module