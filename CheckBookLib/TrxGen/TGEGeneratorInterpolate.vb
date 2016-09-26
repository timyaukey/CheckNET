Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

<XmlRoot("generator")>
Public Class TGEGeneratorInterpolate
    Inherits TGEGeneratorBase
    Implements IFilePersistable

    Public Overrides Function Validate() As String
        Dim result As String
        result = Schedule.Validate()
        If Not result Is Nothing Then
            Return result
        End If
        Return MyBase.Validate()
    End Function

    Public Overrides Sub CleanForSave()
        MyBase.CleanForSave()
    End Sub

    <XmlElement("schedule")>
    <CategoryAttribute("Repeat")>
    <TypeConverter(GetType(ExpandableObjectConverter))>
    <DisplayName("Schedule")>
    Public Property Schedule As TGESchedule

    <XmlElement("sample")>
    <CategoryAttribute("Repeat")>
    <DisplayName("Samples")>
    Public Property Samples As List(Of TGESample)
End Class
