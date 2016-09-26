Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

<XmlRoot("normaltrx")>
Public Class TGENormalTrx
    Inherits TGETemplateBase
    Implements IFilePersistable

    Public Overrides Function Validate() As String Implements IFilePersistable.Validate
        Dim msg As String
        msg = MyBase.Validate()
        If Not msg Is Nothing Then
            Return msg
        End If
        If String.IsNullOrEmpty(CatKey) Then
            Return "Category is required"
        End If
        Return Nothing
    End Function

    Public Sub CleanForSave() Implements IFilePersistable.CleanForSave

    End Sub

    <XmlAttribute("catkey")>
    <DisplayName("Category")>
    <TypeConverter(GetType(CategoryConverter))>
    <Editor(GetType(CategoryUIEditor), GetType(System.Drawing.Design.UITypeEditor))>
    Public Property CatKey As String

    <XmlAttribute("memo")>
    Public Property Memo As String

    Public Overrides Function blnIsEmpty() As Boolean
        Return MyBase.blnIsEmpty() And String.IsNullOrEmpty(CatKey) And String.IsNullOrEmpty(Memo)
    End Function
End Class
