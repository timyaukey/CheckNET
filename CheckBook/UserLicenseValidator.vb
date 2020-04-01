Option Strict On
Option Explicit On

Imports System.Security.Cryptography

Public Class UserLicenseValidator
    Inherits Willowsoft.TamperProofData.Validator

    Protected Overrides Function GetPublicKey() As RSAParameters
        Dim rsap As RSAParameters = New RSAParameters()
        rsap.Exponent = New Byte() {1, 0, 1}
        rsap.Modulus = New Byte() {201, 66, 137, 161, 10, 93, 11, 199, 44, 162, 159, 63, 240, 130, 250, 196,
              205, 147, 253, 174, 47, 53, 253, 167, 174, 47, 94, 145, 243, 91, 180, 188,
              90, 136, 46, 142, 86, 87, 91, 98, 25, 98, 237, 188, 252, 81, 114, 146,
              203, 246, 28, 178, 95, 50, 94, 20, 192, 78, 250, 106, 207, 61, 249, 231,
              10, 155, 15, 50, 101, 78, 148, 21, 73, 49, 184, 93, 174, 170, 216, 117,
              109, 70, 124, 103, 3, 164, 59, 113, 101, 104, 250, 207, 118, 82, 139, 205,
              45, 187, 155, 83, 128, 115, 59, 41, 224, 151, 208, 177, 223, 47, 146, 251,
              1, 248, 102, 245, 161, 215, 248, 200, 134, 237, 210, 153, 247, 186, 206, 57}
        Return rsap
    End Function
End Class
