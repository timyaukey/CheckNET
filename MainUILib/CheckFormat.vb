Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Xml.Serialization

<XmlRoot(ElementName:="Check")>
Public Class CheckFormat
    Public Property Margins As CheckFormatMargins = New CheckFormatMargins()

    <XmlElement(ElementName:="Date")>
    <DisplayName("Check Date")>
    Public Property CheckDate As CheckFormatPosition = New CheckFormatPosition(3.5D, 0.5D)

    <DisplayName("Amount As Number")>
    Public Property ShortAmount As CheckFormatPosition = New CheckFormatPosition(4.5D, 1D)

    <DisplayName("Amount As Words")>
    Public Property LongAmount As CheckFormatPosition = New CheckFormatPosition(0.3D, 2D)

    <DisplayName("Payee Name")>
    Public Property Payee As CheckFormatPosition = New CheckFormatPosition(1D, 1.5D)

    <DisplayName("Mailing Address")>
    Public Property MailingAddress As CheckFormatPosition = New CheckFormatPosition(3D, 2.5D)

    <DisplayName("Memo")>
    Public Property AccountNumber As CheckFormatPosition = New CheckFormatPosition(1D, 3D)

    <DisplayName("Payee Name On Stub")>
    Public Property Payee2 As CheckFormatPosition = New CheckFormatPosition()

    <DisplayName("Amount On Stub")>
    Public Property Amount2 As CheckFormatPosition = New CheckFormatPosition()

    <XmlElement(ElementName:="Date2")>
    <DisplayName("Check Date On Stub")>
    Public Property CheckDate2 As CheckFormatPosition = New CheckFormatPosition()

    <XmlElement(ElementName:="Number2")>
    <DisplayName("Check Number On Stub")>
    Public Property Number2 As CheckFormatPosition = New CheckFormatPosition()

    <DisplayName("Invoice Number List")>
    Public Property InvoiceList1 As CheckFormatTable = New CheckFormatTable()

    <DisplayName("Invoice List On Stub")>
    Public Property InvoiceList2 As CheckFormatTable = New CheckFormatTable()
End Class

<TypeConverter(GetType(ExpandableObjectConverter))>
Public Class CheckFormatPosition
    Public Sub New()
        XPos = 0.0D
        YPos = 0.0D
    End Sub

    Public Sub New(ByVal XPos_ As Decimal, ByVal YPos_ As Decimal)
        XPos = XPos_
        YPos = YPos_
    End Sub

    <XmlAttribute(AttributeName:="x")>
    <DisplayName("Horizontal Position")>
    <Description("Number of inches in from left edge of paper to left edge of text")>
    Public Property XPos As Decimal

    <XmlAttribute(AttributeName:="y")>
    <DisplayName("Vertical Position")>
    <Description("Number of inches down from top edge of paper to top edge of text")>
    Public Property YPos As Decimal

    Public Overrides Function ToString() As String
        Return "x=" + XPos.ToString() + ", y=" + YPos.ToString()
    End Function
End Class

<TypeConverter(GetType(ExpandableObjectConverter))>
Public Class CheckFormatMargins
    <XmlAttribute(AttributeName:="x")>
    <DisplayName("Left Margin")>
    <Description("Leftmost position printer is capable of printing at on page, measured in inches from edge of paper")>
    Public Property XPos As Decimal

    <XmlAttribute(AttributeName:="y")>
    <DisplayName("Top Margin")>
    <Description("Topmost position printer is capable of printing at on page, measured in inches from edge of paper")>
    Public Property YPos As Decimal

    Public Overrides Function ToString() As String
        Return "x=" + XPos.ToString() + ", y=" + YPos.ToString()
    End Function
End Class

<TypeConverter(GetType(ExpandableObjectConverter))>
Public Class CheckFormatTable
    <XmlAttribute(AttributeName:="x")>
    <DisplayName("Horizontal Position")>
    <Description("Number of inches in from left edge of paper to left edge of text")>
    Public Property XPos As Decimal

    <XmlAttribute(AttributeName:="y")>
    <DisplayName("Vertical Position")>
    <Description("Number of inches down from top edge of paper to top edge of text")>
    Public Property YPos As Decimal

    <XmlAttribute(AttributeName:="rows")>
    <DisplayName("Number of Rows")>
    <Description("Max number of rows in table")>
    Public Property Rows As Decimal

    <XmlAttribute(AttributeName:="cols")>
    <DisplayName("Number of Columns")>
    <Description("Max number of columns in table")>
    Public Property Cols As Decimal

    <XmlAttribute(AttributeName:="colwidth")>
    <DisplayName("Column Width")>
    <Description("Column width in inches")>
    Public Property ColWidth As Decimal

    Public Overrides Function ToString() As String
        Return "x=" + XPos.ToString() + ", y=" + YPos.ToString()
    End Function
End Class
