Option Strict On
Option Explicit On

Imports System.Collections.Generic
Imports Willowsoft.TamperProofData

Public Class FinancialReportsLicense
    Inherits StandardLicenseBase(Of FinancialReportsLicenseValidator)

    Public Overrides ReadOnly Property BaseFileName As String
        Get
            Return "Willowsoft.Checkbook.FinancialReports.lic"
        End Get
    End Property

    Public Overrides ReadOnly Property LicenseTitle As String
        Get
            Return "Willow Creek Financial Reports License"
        End Get
    End Property

    Public Overrides ReadOnly Property AttributeSummary As String
        Get
            Return Nothing
        End Get
    End Property
End Class
