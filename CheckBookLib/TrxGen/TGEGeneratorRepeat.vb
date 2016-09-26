Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

<XmlRoot("generator")>
Public Class TGEGeneratorRepeat
    Inherits TGEGeneratorBase
    Implements IFilePersistable

    Public Overrides Function Validate() As String
        Dim result As String
        result = Repeat.Validate()
        If Not result Is Nothing Then
            Return result
        End If
        Return MyBase.Validate()
    End Function

    Public Overrides Sub CleanForSave()
        MyBase.CleanForSave()
    End Sub

    <XmlElement("repeat")>
    <CategoryAttribute("Repeat")>
    <TypeConverter(GetType(ExpandableObjectConverter))>
    <DisplayName("Schedule")>
    Public Property Repeat As TGERepeat
End Class
