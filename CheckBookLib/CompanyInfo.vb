Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Xml.Serialization

Public Class CompanyInfo
    <XmlElement(ElementName:="CompanyName")>
    <DisplayName("Company Name")>
    Public Property strCompanyName As String = "Your Company Name"

    Public Function Clone() As CompanyInfo
        Return DirectCast(Me.MemberwiseClone(), CompanyInfo)
    End Function
End Class
