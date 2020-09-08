Option Strict On
Option Explicit On

Public Class LicenseForm
    Private mobjHostUI As IHostUI
    Private mobjLicense As TamperProofData.IStandardLicense

    Public Sub ShowLicense(ByVal objHostUI As IHostUI, ByVal objLicense As TamperProofData.IStandardLicense)
        mobjHostUI = objHostUI
        mobjLicense = objLicense
        SetControlValues()
        Me.ShowDialog()
    End Sub

    Private Sub SetControlValues()
        Try
            txtLicenseTitle.Text = mobjLicense.LicenseTitle
            If mobjLicense.Status = TamperProofData.LicenseStatus.Active Or
               mobjLicense.Status = TamperProofData.LicenseStatus.Expired Then
                txtLicenseVersion.Text = mobjLicense.LicenseVersion.ToString()
                txtLicensedTo.Text = If(String.IsNullOrEmpty(mobjLicense.LicensedTo), "(missing)", mobjLicense.LicensedTo)
                If mobjLicense.ExpirationDate.HasValue Then
                    txtExpirationDate.Text = mobjLicense.ExpirationDate.Value.ToShortDateString()
                Else
                    txtExpirationDate.Text = "(none)"
                End If
                txtEmailAddress.Text = If(String.IsNullOrEmpty(mobjLicense.EmailAddress), "", mobjLicense.EmailAddress)
                txtDetails.Text = If(String.IsNullOrEmpty(mobjLicense.AttributeSummary), "", mobjLicense.AttributeSummary)
                txtSerialNumber.Text = If(String.IsNullOrEmpty(mobjLicense.SerialNumber), "", mobjLicense.SerialNumber)
                btnInstall.Enabled = False
                btnRemove.Enabled = True
            Else
                txtLicenseVersion.Text = ""
                txtLicensedTo.Text = ""
                txtExpirationDate.Text = ""
                txtEmailAddress.Text = ""
                txtDetails.Text = ""
                txtSerialNumber.Text = ""
                btnInstall.Enabled = True
                btnRemove.Enabled = False
            End If
        Catch

        End Try
        Dim strStatus As String
        Select Case mobjLicense.Status
            Case TamperProofData.LicenseStatus.Active
                strStatus = "Active"
            Case TamperProofData.LicenseStatus.Expired
                strStatus = "Expired"
            Case TamperProofData.LicenseStatus.Invalid
                strStatus = "Invalid"
            Case TamperProofData.LicenseStatus.Missing
                strStatus = "Missing"
            Case Else
                strStatus = "Unknown"
        End Select
        txtLicenseStatus.Text = strStatus
    End Sub

    Private Sub btnInstall_Click(sender As Object, e As EventArgs) Handles btnInstall.Click
        Try
            Dim result As DialogResult = dlgOpenLicenseFile.ShowDialog()
            If result <> DialogResult.OK Then
                mobjHostUI.InfoMessageBox("License file not installed.")
                Return
            End If
            System.IO.File.Copy(dlgOpenLicenseFile.FileName, strLicenseFilePath(), True)
            mobjLicense.Load(Company.strLicenseFolder())
            SetControlValues()
            If mobjLicense.Status = TamperProofData.LicenseStatus.Active Then
                mobjHostUI.InfoMessageBox("License file successfully installed.")
                Return
            ElseIf mobjLicense.Status = TamperProofData.LicenseStatus.Expired Then
                mobjHostUI.ErrorMessageBox("This license file is expired.")
                Return
            Else
                mobjHostUI.ErrorMessageBox("Invalid or inaccessible license file.")
                Return
            End If
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Try
            If mobjHostUI.OkCancelMessageBox("Are you sure you want to remove this license?") = DialogResult.OK Then
                System.IO.File.Delete(strLicenseFilePath())
                mobjLicense.Load(Company.strLicenseFolder())
                SetControlValues()
                mobjHostUI.InfoMessageBox("License removed.")
            End If
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub

    Private Function strLicenseFilePath() As String
        Return System.IO.Path.Combine(Company.strLicenseFolder(), mobjLicense.BaseFileName)
    End Function
End Class