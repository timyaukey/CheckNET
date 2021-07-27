Option Strict On
Option Explicit On

Imports System.Xml

Public Class CBXmlNodeList
    Inherits System.Collections.Generic.List(Of CBXmlNode)

    Public Sub New(ByVal list As XmlNodeList)
        Dim node As XmlNode
        For Each node In list
            Me.Add(CBXmlNode.Create(node))
        Next
    End Sub

    Public ReadOnly Property Length() As Integer
        Get
            Length = Me.Count
        End Get
    End Property
End Class
