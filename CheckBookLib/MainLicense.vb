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
            Return "Willow Creek Checkbook License"
        End Get
    End Property

    Public Overrides ReadOnly Property AttributeSummary As String
        Get
            Return Nothing
        End Get
    End Property

    Public Overrides ReadOnly Property LicenseStatement As String
        Get
            Return "Permission is granted to " + Me.LicensedTo + " for unrestricted non-commercial use of the Willow Creek Checkbook software." +
                Environment.NewLine + "asdf" +
                Environment.NewLine + "asdf" +
                Environment.NewLine + "asdf" +
                Environment.NewLine + "asdf" +
                Environment.NewLine + "asdf" +
                Environment.NewLine + "asdf" +
                Environment.NewLine + "asdf" +
                Environment.NewLine + "asdfz."
        End Get
    End Property

    Public Overrides ReadOnly Property LicenseUrl As Uri
        Get
            Return New Uri("http://willowcreekcheckbook.azureweb.com/license")
        End Get
    End Property

    Public Overrides ReadOnly Property ProductUrl As Uri
        Get
            Return New Uri("http://susansgardenandcoffee.com")
        End Get
    End Property
End Class
