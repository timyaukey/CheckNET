Option Strict On
Option Explicit On

Imports System.Collections.Generic

Public MustInherit Class TamperProofLicense
    Implements ILicense

    Private mcolValues As Dictionary(Of String, String) = Nothing
    Private mlngStatus As LicenseStatus = LicenseStatus.NotLoaded

    Public ReadOnly Property lngStatus As LicenseStatus Implements ILicense.lngStatus
        Get
            Return mlngStatus
        End Get
    End Property

    Public ReadOnly Property strLicensedTo As String Implements ILicense.strLicensedTo
        Get
            Return strGetValue("LicensedTo")
        End Get
    End Property

    Public ReadOnly Property datExpiration As Date? Implements ILicense.datExpiration
        Get
            Dim strValue As String = Nothing
            If mcolValues.TryGetValue("ExpirationDate", strValue) Then
                Return DateTime.Parse(strValue)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public ReadOnly Property strSerialNumber As String Implements ILicense.strSerialNumber
        Get
            Return strGetValue("SerialNumber")
        End Get
    End Property

    Protected ReadOnly Property strGetValue(ByVal strKey As String) As String
        Get
            Dim strValue As String = Nothing
            If mcolValues.TryGetValue(strKey, strValue) Then
                Return strValue
            Else
                Return Nothing
            End If
        End Get
    End Property

    Public ReadOnly Property colValues As Dictionary(Of String, String) Implements ILicense.colValues
        Get
            Return mcolValues
        End Get
    End Property

    Public Sub Load() Implements ILicense.Load
        Dim strLicenseFile As String = System.IO.Path.Combine(Company.strLicenseFolder(), strBaseFileName)
        mcolValues = Nothing
        mlngStatus = LicenseStatus.NotLoaded
        If System.IO.File.Exists(strLicenseFile) Then
            Using licenseStream As System.IO.Stream = New System.IO.FileStream(strLicenseFile, System.IO.FileMode.Open)
                Try
                    mcolValues = Willowsoft.TamperProofData.LicenseReader.Read(licenseStream, objGetValidator())
                    mlngStatus = LicenseStatus.Active
                    Dim datExpiration2 As DateTime? = datExpiration
                    If datExpiration2.HasValue Then
                        If datExpiration2 < DateTime.Today Then
                            mlngStatus = LicenseStatus.Expired
                            Return
                        End If
                    End If
                Catch ex As System.IO.InvalidDataException
                    mlngStatus = LicenseStatus.Invalid
                    Return
                End Try
            End Using
        Else
            mlngStatus = LicenseStatus.Missing
            Return
        End If
    End Sub

    Protected MustOverride Function objGetValidator() As TamperProofData.Validator
    Public MustOverride ReadOnly Property strBaseFileName As String Implements ILicense.strBaseFileName
    Public MustOverride ReadOnly Property strTitle As String Implements ILicense.strTitle
    Public MustOverride ReadOnly Property strAttributeSummary As String Implements ILicense.strAttributeSummary
End Class
