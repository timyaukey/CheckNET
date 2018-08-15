Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Xml.Serialization

Public Class CompanyInfo
    <XmlElement(ElementName:="CompanyName")>
    <DisplayName("Company Name")>
    <Category("* General")>
    Public Property strCompanyName As String = "Your Company Name"

    <XmlElement("LoansReceivableSummaryOnly")>
    <DisplayName("Loans Receivable - Summary Only")>
    <Category("Balance Sheet Layout")>
    Public Property blnLoansReceivableSummaryOnly As Boolean

    <XmlElement("LoansReceivableSupressZero")>
    <DisplayName("Loans Receivable - Omit Zero Summary")>
    <Category("Balance Sheet Layout")>
    Public Property blnLoansReceivableSuppressZero As Boolean

    <XmlElement("LoansPayableSummaryOnly")>
    <DisplayName("Loans Payable - Summary Only")>
    <Category("Balance Sheet Layout")>
    Public Property blnLoansPayableSummaryOnly As Boolean

    <XmlElement("LoansPayableSupressZero")>
    <DisplayName("Loans Payable - Omit Zero Summary")>
    <Category("Balance Sheet Layout")>
    Public Property blnLoansPayableSuppressZero As Boolean

    <XmlElement("RealPropertySummaryOnly")>
    <DisplayName("Real Property - Summary Only")>
    <Category("Balance Sheet Layout")>
    Public Property blnRealPropertySummaryOnly As Boolean

    <XmlElement("RealPropertySupressZero")>
    <DisplayName("Real Property - Omit Zero Summary")>
    <Category("Balance Sheet Layout")>
    Public Property blnRealPropertySuppressZero As Boolean

    Public Function Clone() As CompanyInfo
        Return DirectCast(Me.MemberwiseClone(), CompanyInfo)
    End Function
End Class
