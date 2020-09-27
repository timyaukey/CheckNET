Option Strict On
Option Explicit On

Public Class LicenseListForm
    Private mobjHostUI As IHostUI

    Public Sub ShowMe(ByVal objHostUI As IHostUI)
        mobjHostUI = objHostUI
        ShowLicenses()
        Me.ShowDialog()
    End Sub

    Private Sub ShowLicenses()
        lvwLicenses.Items.Clear()
        ShowLicense(Company.objMainLicense)
        For Each objLicense As Willowsoft.TamperProofData.IStandardLicense In Company.colExtraLicenses
            ShowLicense(objLicense)
        Next
        If Company.blnAnyNonActiveLicenses Then
            lblLicenseStatus.Text = "WARNING: One or more licenses are missing, expired, or invalid for some other reason."
            lblLicenseStatus.Visible = True
        Else
            lblLicenseStatus.Visible = False
        End If
    End Sub

    Private Sub ShowLicense(ByVal objLicense As Willowsoft.TamperProofData.IStandardLicense)
        Dim item As ListViewItem = New ListViewItem(objLicense.LicenseTitle)
        If objLicense.Status = TamperProofData.LicenseStatus.Active Or
           objLicense.Status = TamperProofData.LicenseStatus.Expired Then
            item.SubItems.Add(objLicense.LicensedTo)
            Dim strExpirationDate As String
            If objLicense.ExpirationDate.HasValue Then
                strExpirationDate = objLicense.ExpirationDate.Value.ToShortDateString()
            Else
                strExpirationDate = "(none)"
            End If
            item.SubItems.Add(strExpirationDate)
        Else
            item.SubItems.Add("")
            item.SubItems.Add("")
        End If
        Dim strLicenseStatus As String
        Select Case objLicense.Status
            Case TamperProofData.LicenseStatus.Active
                strLicenseStatus = "Active"
            Case TamperProofData.LicenseStatus.Expired
                strLicenseStatus = "Expired"
            Case TamperProofData.LicenseStatus.Invalid
                strLicenseStatus = "Invalid"
            Case TamperProofData.LicenseStatus.Missing
                strLicenseStatus = "Missing"
            Case Else
                strLicenseStatus = "Unknown"
        End Select
        item.SubItems.Add(strLicenseStatus)
        item.Tag = objLicense
        lvwLicenses.Items.Add(item)
    End Sub

    Private Sub btnManageLicense_Click(sender As Object, e As EventArgs) Handles btnManageLicense.Click
        Try
            If lvwLicenses.SelectedItems.Count = 0 Then
                mobjHostUI.InfoMessageBox("Select a license first")
                Return
            End If
            Dim objLicense As Willowsoft.TamperProofData.IStandardLicense =
                DirectCast(lvwLicenses.SelectedItems(0).Tag, Willowsoft.TamperProofData.IStandardLicense)
            Using frm As LicenseForm = New LicenseForm()
                frm.ShowLicense(mobjHostUI, objLicense)
                ShowLicenses()
            End Using
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
End Class