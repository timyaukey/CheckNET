Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Xml.Serialization

Public Class CompanyInfo
    <XmlElement(ElementName:="CompanyName")>
    <DisplayName("Company Name")>
    <Category("* General")>
    Public Property CompanyName As String = "Your Company Name"

    <XmlElement("LoansReceivableSummaryOnly")>
    <DisplayName("Loans Receivable - Summary Only")>
    <Category("Balance Sheet Layout")>
    Public Property IsLoansReceivableSummaryOnly As Boolean

    <XmlElement("LoansReceivableSupressZero")>
    <DisplayName("Loans Receivable - Omit Zero Summary")>
    <Category("Balance Sheet Layout")>
    Public Property IsLoansReceivableSuppressZero As Boolean

    <XmlElement("LoansPayableSummaryOnly")>
    <DisplayName("Loans Payable - Summary Only")>
    <Category("Balance Sheet Layout")>
    Public Property IsLoansPayableSummaryOnly As Boolean

    <XmlElement("LoansPayableSupressZero")>
    <DisplayName("Loans Payable - Omit Zero Summary")>
    <Category("Balance Sheet Layout")>
    Public Property IsLoansPayableSuppressZero As Boolean

    <XmlElement("RealPropertySummaryOnly")>
    <DisplayName("Real Property - Summary Only")>
    <Category("Balance Sheet Layout")>
    Public Property IsRealPropertySummaryOnly As Boolean

    <XmlElement("RealPropertySupressZero")>
    <DisplayName("Real Property - Omit Zero Summary")>
    <Category("Balance Sheet Layout")>
    Public Property IsRealPropertySuppressZero As Boolean

    Public Function Clone() As CompanyInfo
        Return DirectCast(Me.MemberwiseClone(), CompanyInfo)
    End Function
End Class
