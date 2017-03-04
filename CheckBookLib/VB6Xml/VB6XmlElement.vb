Option Strict On
Option Explicit On

Imports System.Xml

Public Class VB6XmlElement
    Inherits VB6XmlNode

    Private mElement As XmlElement

    Public Sub New(ByVal element As XmlElement)
        MyBase.New(element)
        mElement = element
    End Sub

    Public ReadOnly Property RawElement() As XmlElement
        Get
            RawElement = mElement
        End Get
    End Property

    Public Function GetAttribute(ByVal strName As String) As Object
        GetAttribute = mElement.GetAttribute(strName)
    End Function

    Public Sub SetAttribute(ByVal strName As String, ByVal strValue As String)
        mElement.SetAttribute(strName, strValue)
    End Sub

    Public Sub RemoveAttribute(ByVal strName As String)
        mElement.RemoveAttribute(strName)
    End Sub

    Public ReadOnly Property Name() As String
        Get
            Name = mElement.Name
        End Get
    End Property

End Class
