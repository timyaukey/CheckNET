Option Strict On
Option Explicit On

Imports System.Collections.Generic

Public Interface ILicense
    Sub Load()
    ReadOnly Property intLicenseVersion() As Integer
    ReadOnly Property strBaseFileName() As String
    ReadOnly Property lngStatus() As LicenseStatus
    ReadOnly Property strTitle() As String
    ReadOnly Property strLicensedTo() As String
    ReadOnly Property datExpiration() As Nullable(Of DateTime)
    ReadOnly Property strSerialNumber() As String
    ReadOnly Property strAttributeSummary() As String
    ReadOnly Property colValues() As Dictionary(Of String, String)
End Interface

Public Enum LicenseStatus
    NotLoaded = 0
    Invalid = 1
    Active = 2
    Expired = 3
    Missing = 4
End Enum
