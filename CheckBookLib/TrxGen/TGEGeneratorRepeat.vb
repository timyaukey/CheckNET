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
        If Not NormalTrx Is Nothing Then
            result = NormalTrx.Validate()
            If Not result Is Nothing Then
                Return result
            End If
        End If
        If Not BudgetTrx Is Nothing Then
            result = BudgetTrx.Validate()
            If Not result Is Nothing Then
                Return result
            End If
        End If
        Return result
    End Function

    <XmlElement("repeat")>
    <CategoryAttribute("Repeat")>
    <TypeConverter(GetType(ExpandableObjectConverter))>
    <DisplayName("Specs")>
    Public Property Repeat As TGERepeat

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
