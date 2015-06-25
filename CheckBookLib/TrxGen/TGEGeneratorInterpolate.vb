Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

<XmlRoot("generator")>
Public Class TGEGeneratorInterpolate
    Inherits TGEGeneratorBase
    Implements IFilePersistable

    Public Overrides Function Validate() As String
        Validate = Nothing
    End Function

    <XmlElement("schedule")>
    <CategoryAttribute("Schedule")>
    <TypeConverter(GetType(ExpandableObjectConverter))>
    <DisplayName("Schedule")>
    Public Property Schedule As TGESchedule

    <XmlElement("sample")>
    <DisplayName("Samples")>
    Public Property Samples As List(Of TGESample)

    <XmlElement("normaltrx")>
    <CategoryAttribute("Template")>
    <TypeConverter(GetType(ExpandableObjectConverter))>
    <DisplayName("NormalTrx")>
    Public Property NormalTrx As TGENormalTrx

    <XmlElement("budgettrx")>
    <CategoryAttribute("Template")>
    <TypeConverter(GetType(ExpandableObjectConverter))>
    <DisplayName("BudgetTrx")>
    Public Property BudgetTrx As TGEBudgetTrx
End Class
