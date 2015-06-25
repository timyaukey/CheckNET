Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

<XmlRoot("budgettrx")>
Public Class TGEBudgetTrx
    Inherits TGETemplateBase
    Implements IFilePersistable

    Public Function Validate() As String Implements IFilePersistable.Validate
        Validate = Nothing
    End Function

    <XmlAttribute("budgetkey")>
    <DisplayName("Budget")>
    <TypeConverter(GetType(BudgetConverter))>
    <Editor(GetType(BudgetUIEditor), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property BudgetKey As String

    <XmlAttribute("budgetunit")>
    <TypeConverter(GetType(RepeatUnitTypeConverter))>
    Public Property Unit As String

    <XmlAttribute("budgetnumber")>
    Public Property Interval As Integer

    Public Overrides Function ToString() As String
        Return "(expand)"
    End Function
End Class
