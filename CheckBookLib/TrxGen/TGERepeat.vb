Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

Public Class TGERepeat
    Inherits TGEBase
    Implements IFilePersistable

    Public Function Validate() As String Implements IFilePersistable.Validate
        If String.IsNullOrEmpty(Unit) Then
            Return "Unit is necessary"
        End If
        If Interval = 0 Then
            Return "Interval is necessary"
        End If
        If String.IsNullOrEmpty(StartDate) Then
            Return "Start date is necessary"
        End If
        If String.IsNullOrEmpty(EndDate) Then
            Return "End date is necessary"
        End If
        If Amount = 0.0# Then
            Return "Amount is necessary"
        End If
        Return Nothing
    End Function

    Public Sub CleanForSave() Implements IFilePersistable.CleanForSave

    End Sub

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
