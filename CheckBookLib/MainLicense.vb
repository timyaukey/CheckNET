Option Strict On
Option Explicit On

Imports System.Collections.Generic

Public Class MainLicense
    Inherits TamperProofLicense

    Public Overrides ReadOnly Property strBaseFileName As String
        Get
            Return "user.lic"
        End Get
    End Property

    Public Overrides ReadOnly Property strTitle As String
        Get
            Return "Main License"
        End Get
    End Property

    Public Overrides ReadOnly Property strAttributeSummary As String
        Get
            Return Nothing
        End Get
    End Property

    Protected Overrides Function objGetValidator() As Willowsoft.TamperProofData.Validator
        Return New MainLicenseValidator()
    End Function
End Class
