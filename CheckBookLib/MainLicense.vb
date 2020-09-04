Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Willowsoft.TamperProofData

Public Class MainLicense
    Inherits StandardLicenseBase(Of MainLicenseValidator)

    Public Overrides ReadOnly Property BaseFileName As String
        Get
            Return "Willowsoft.Checkbook.Main.lic"
        End Get
    End Property

    Public Overrides ReadOnly Property LicenseTitle As String
        Get
            Return "Main License"
        End Get
    End Property

    Public Overrides ReadOnly Property AttributeSummary As String
        Get
            Return Nothing
        End Get
    End Property
End Class
