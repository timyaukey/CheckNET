Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Xml.Serialization

Public Class TGETemplateBase
    Inherits TGEBase

    <XmlAttribute("number")>
    Public Property Number As String

    <XmlAttribute("description")>
    <DisplayName("Description")>
    Public Property Description As String
End Class
