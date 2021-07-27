Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

Public Class TGETemplateBase
    Inherits TGEBase

    <XmlAttribute("description")>
    <DisplayName("Description")>
    <Description("Payee name or other transaction description")>
    Public Property Description As String

    <XmlAttribute("number")>
    <DisplayName("Transaction Number")>
    Public Property Number As String

    Public Overridable Function Validate() As String
        If String.IsNullOrEmpty(Number) Then
            Return "Transaction number is required"
        End If
        If String.IsNullOrEmpty(Description) Then
            Return "Description is required"
        End If
        Return Nothing
    End Function

    Public Overridable Function IsEmpty() As Boolean
        Return String.IsNullOrEmpty(Number) And String.IsNullOrEmpty(Description)
    End Function

    Public Overrides Function ToString() As String
        If IsEmpty() Then
            Return "(empty)"
        Else
            Return "(expand)"
        End If
    End Function
End Class
