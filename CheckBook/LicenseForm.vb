Option Strict On
Option Explicit On

Public Class LicenseForm
    Private mobjLicense As TamperProofData.IStandardLicense

    Public Sub ShowLicense(ByVal objLicense As TamperProofData.IStandardLicense)
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
            Else
                txtLicenseVersion.Text = ""
                txtLicensedTo.Text = ""
                txtExpirationDate.Text = ""
                txtEmailAddress.Text = ""
                txtDetails.Text = ""
                txtSerialNumber.Text = ""
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
            Case TamperProofData.LicenseStatus.NotLoaded
                strStatus = "Not Loaded"
            Case Else
                strStatus = "Unknown"
        End Select
        txtLicenseStatus.Text = strStatus
    End Sub
End Class