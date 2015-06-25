Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

Public MustInherit Class TGEGeneratorBase
    Inherits TGEBase
    Implements IFilePersistable

    <XmlAttribute("class"), ReadOnlyAttribute(True)>
    <DisplayName("Type")>
    <CategoryAttribute("Core")>
    Public Property ClassName As String

    <XmlAttribute("description")>
    <DisplayName("Name")>
    <CategoryAttribute("Core")>
    <Description("Payee name or other description for error messages")>
    Public Property Description As String

    <XmlAttribute("enabled")>
    <CategoryAttribute("Core")>
    <TypeConverter(GetType(BooleanStringTypeConverter))>
    Public Property Enabled As String

    <XmlAttribute("repeatkey")>
    <CategoryAttribute("Core")>
    <DisplayName("TrxGroupKey")>
    Public Property RepeatKey As String

    <XmlAttribute("startseq")>
    <CategoryAttribute("Core")>
    <DisplayName("TrxStartSeq")>
    Public Property StartSeq As Integer

    Public MustOverride Function Validate() As String Implements IFilePersistable.Validate
End Class
