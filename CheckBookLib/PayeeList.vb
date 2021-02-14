Option Strict On
Option Explicit On

Imports System.Xml.Serialization

<XmlRoot("Table")>
Public Class PayeeList
    <XmlElement("Payee")>
    Public Payees As List(Of PayeeItem)
End Class
