Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

Public MustInherit Class TGEGeneratorBase
    Inherits TGEBase
    Implements IFilePersistable

    <XmlAttribute("description")>
    <DisplayName("Repeat Name")>
    <CategoryAttribute("Core")>
    <Description("The Repeat Name that appears on the transaction form")>
    Public Property Description As String

    <XmlAttribute("enabled")>
    <CategoryAttribute("Core")>
    <TypeConverter(GetType(BooleanStringTypeConverter))>
    Public Property Enabled As String

    <XmlAttribute("class"), ReadOnlyAttribute(True)>
    <DisplayName("Type")>
    <CategoryAttribute("Core")>
    Public Property ClassName As String

    <XmlAttribute("repeatkey")>
    <CategoryAttribute("Repeat")>
    <DisplayName("Repeat Code")>
    <Description("An automatically assigned unique code used internally to identify transactions created from this generator")>
    <ReadOnlyAttribute(False)>
    Public Property RepeatKey As String

    <XmlAttribute("maxdaysold")>
    <DisplayName("Max Days Old")>
    <CategoryAttribute("Core")>
    <Description("If not empty, do not generate any transactions older than this many days. ""0"" means today, ""1"" means yesterday, etc.")>
    Public Property MaxDaysOld As String

    Public Shared Sub SetGroupReadOnly(ByVal blnReadOnly As Boolean)
        Dim pd As PropertyDescriptor = TypeDescriptor.GetProperties(GetType(TGEGeneratorBase)).Item("RepeatKey")
        Dim attr As ReadOnlyAttribute = DirectCast(pd.Attributes.Item(GetType(ReadOnlyAttribute)), ReadOnlyAttribute)
        Dim fieldToChange As System.Reflection.FieldInfo = attr.GetType().GetField("isReadOnly", _
                                       System.Reflection.BindingFlags.NonPublic Or _
                                       System.Reflection.BindingFlags.Instance)
        fieldToChange.SetValue(attr, blnReadOnly)
    End Sub

    <XmlAttribute("startseq")>
    <CategoryAttribute("Repeat")>
    <DisplayName("Starting Sequence Number")>
    Public Property StartSeq As Integer

    <XmlElement("normaltrx")>
    <CategoryAttribute("Templates")>
    <TypeConverter(GetType(ExpandableObjectConverter))>
    <DisplayName("Normal Transaction")>
    Public Property NormalTrx As TGENormalTrx = New TGENormalTrx()

    <XmlElement("budgettrx")>
    <CategoryAttribute("Templates")>
    <TypeConverter(GetType(ExpandableObjectConverter))>
    <DisplayName("Budget Transaction")>
    Public Property BudgetTrx As TGEBudgetTrx = New TGEBudgetTrx()

    Public Overridable Function Validate() As String Implements IFilePersistable.Validate
        Dim result As String
        Dim intTemplateCount As Integer = 0
        If Not NormalTrx Is Nothing Then
            If Not NormalTrx.blnIsEmpty Then
                intTemplateCount = intTemplateCount + 1
            End If
        End If
        If Not BudgetTrx Is Nothing Then
            If Not BudgetTrx.blnIsEmpty() Then
                intTemplateCount = intTemplateCount + 1
            End If
        End If
        If intTemplateCount <> 1 Then
            Return "Either normal or budget template transaction is required, but not both"
        End If
        If Not NormalTrx Is Nothing Then
            If Not NormalTrx.blnIsEmpty() Then
                result = NormalTrx.Validate()
                If Not result Is Nothing Then
                    Return result
                End If
            End If
        End If
        If Not BudgetTrx Is Nothing Then
            If Not BudgetTrx.blnIsEmpty() Then
                result = BudgetTrx.Validate()
                If Not result Is Nothing Then
                    Return result
                End If
                intTemplateCount = intTemplateCount + 1
            End If
        End If
        If String.IsNullOrEmpty(Description) Then
            Return "Name is required"
        End If
        If String.IsNullOrEmpty(RepeatKey) Then
            Return "Group is required"
        End If
        If StartSeq = 0 Then
            Return "Starting sequence number is required"
        End If
        Return Nothing
    End Function

    Public Overridable Sub CleanForSave() Implements IFilePersistable.CleanForSave
        If Not NormalTrx Is Nothing Then
            If NormalTrx.blnIsEmpty() Then
                NormalTrx = Nothing
            End If
        End If
        If Not BudgetTrx Is Nothing Then
            If BudgetTrx.blnIsEmpty() Then
                BudgetTrx = Nothing
            End If
        End If
    End Sub
End Class
