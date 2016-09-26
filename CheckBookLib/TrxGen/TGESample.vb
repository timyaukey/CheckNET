Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

Public Class TGESample
    Inherits TGEBase
    Implements IFilePersistable

    Public Function Validate() As String Implements IFilePersistable.Validate
        Return Nothing
    End Function

    Public Sub CleanForSave() Implements IFilePersistable.CleanForSave

    End Sub

    <XmlAttribute("amount")>
    Public Property Amount As Decimal

    <XmlAttribute("date")>
    <TypeConverter(GetType(DateConverter))>
    Public Property SampleDate As String

    Public Overrides Function ToString() As String
        Return SampleDate + " " + Amount.ToString("c")
    End Function
End Class
