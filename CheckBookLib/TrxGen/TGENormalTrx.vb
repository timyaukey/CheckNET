Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

<XmlRoot("normaltrx")>
Public Class TGENormalTrx
    Inherits TGETemplateBase
    Implements IFilePersistable

    Public Function Validate() As String Implements IFilePersistable.Validate
        Validate = Nothing
    End Function

    <XmlAttribute("catkey")>
    <DisplayName("Category")>
    <TypeConverter(GetType(CategoryConverter))>
    <Editor(GetType(CategoryUIEditor), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property CatKey As String

    <XmlAttribute("memo")>
    Public Property Memo As String

    Public Overrides Function ToString() As String
        Return "(expand)"
    End Function
End Class
