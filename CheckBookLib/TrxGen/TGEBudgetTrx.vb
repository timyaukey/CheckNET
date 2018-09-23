Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

<XmlRoot("budgettrx")>
Public Class TGEBudgetTrx
    Inherits TGETemplateBase
    Implements IFilePersistable

    Public Overrides Function Validate() As String Implements IFilePersistable.Validate
        Dim msg As String
        msg = MyBase.Validate()
        If Not msg Is Nothing Then
            Return msg
        End If
        If String.IsNullOrEmpty(BudgetKey) Then
            Return "Budget is required"
        End If
        If String.IsNullOrEmpty(Unit) Then
            Return "Unit is required"
        End If
        If Interval = 0 Then
            Return "A non-zero Interval is required"
        End If
        Return Nothing
    End Function

    Public Sub CleanForSave() Implements IFilePersistable.CleanForSave

    End Sub

    <XmlAttribute("budgetkey")>
    <DisplayName("Budget")>
    <TypeConverter(GetType(BudgetConverter))>
    <Editor(GetType(BudgetUIEditor), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property BudgetKey As String

    <XmlAttribute("budgetunit")>
    <TypeConverter(GetType(RepeatUnitTypeConverter))>
    <Description("Unit type used to express the length of the budget period")>
    Public Property Unit As String

    <XmlAttribute("budgetnumber")>
    <Description("The number of units in the budget period")>
    Public Property Interval As Integer

    Public Overrides Function blnIsEmpty() As Boolean
        Return MyBase.blnIsEmpty() And String.IsNullOrEmpty(BudgetKey) And String.IsNullOrEmpty(Unit) And (Interval = 0)
    End Function
End Class
