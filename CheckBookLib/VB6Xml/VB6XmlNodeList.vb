Option Strict Off
Option Explicit On

Imports System.Xml

Public Class VB6XmlNodeList
    Inherits System.Collections.Generic.List(Of VB6XmlNode)

    Public Sub New(ByVal list As XmlNodeList)
        Dim node As XmlNode
        For Each node In list
            Me.Add(VB6XmlNode.Create(node))
        Next
    End Sub

    Public ReadOnly Property Length() As Integer
        Get
            Length = Me.Count
        End Get
    End Property
End Class
