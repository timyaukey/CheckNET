Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

Public Class TGERepeat
    Inherits TGEBase
    Implements IFilePersistable

    Public Function Validate() As String Implements IFilePersistable.Validate
        Validate = Nothing
    End Function

    <XmlAttribute("unit")>
    <TypeConverter(GetType(RepeatUnitTypeConverter))>
    Public Property Unit As String

    <XmlAttribute("interval")>
    Public Property Interval As Integer

    <XmlAttribute("startdate")>
    <TypeConverter(GetType(DateConverter))>
    Public Property StartDate As String

    <XmlAttribute("enddate")>
    <TypeConverter(GetType(DateConverter))>
    Public Property EndDate As String

    <XmlAttribute("amount")>
    Public Property Amount As Decimal

    Public Overrides Function ToString() As String
        Return "(expand)"
    End Function
End Class
