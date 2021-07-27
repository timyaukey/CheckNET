Option Strict On
Option Explicit On

Imports System.Xml

Public Class CBXmlNode

    Private mNode As XmlNode

    Public Shared Function Create(ByVal node As XmlNode) As CBXmlNode
        If node Is Nothing Then
            Create = Nothing
        ElseIf TypeOf node Is XmlElement Then
            Create = New CBXmlElement(DirectCast(node, XmlElement))
        ElseIf TypeOf node Is XmlText Then
            Create = New CBXmlText(DirectCast(node, XmlText))
        ElseIf TypeOf node Is XmlDocument Then
            Create = New CBXmlDocument(node)
        Else
            Create = New CBXmlNode(node)
        End If
    End Function

    Public Sub New()
        Me.New(Nothing)
    End Sub

    Public Sub New(ByVal node As XmlNode)
        mNode = node
    End Sub

    Public ReadOnly Property RawNode() As XmlNode
        Get
            RawNode = mNode
        End Get
    End Property

    Public ReadOnly Property ParentNode() As CBXmlNode
        Get
            ParentNode = CBXmlNode.Create(mNode.ParentNode)
        End Get
    End Property

    Public ReadOnly Property ChildNodes() As CBXmlNodeList
        Get
            ChildNodes = New CBXmlNodeList(mNode.ChildNodes)
        End Get
    End Property

    Public Property Text() As String
        Get
            Text = mNode.InnerText
        End Get
        Set(ByVal value As String)
            mNode.InnerText = value
        End Set
    End Property

    Public Function CloneNode(ByVal blnDeepClone As Boolean) As CBXmlNode
        CloneNode = CBXmlNode.Create(mNode.CloneNode(blnDeepClone))
    End Function

    Public Sub AppendChild(ByVal newChild As CBXmlNode)
        mNode.AppendChild(newChild.RawNode)
    End Sub

    Public Sub RemoveChild(ByVal child As CBXmlNode)
        mNode.RemoveChild(child.RawNode)
    End Sub

    Public Sub InsertBefore(ByVal newChild As CBXmlNode, ByVal refChild As CBXmlNode)
        mNode.InsertBefore(newChild.RawNode, refChild.RawNode)
    End Sub

    Public Function SelectSingleNode(ByVal strXPath As String) As CBXmlNode
        SelectSingleNode = CBXmlNode.Create(mNode.SelectSingleNode(strXPath))
    End Function

    Public Function SelectNodes(ByVal strXPath As String) As CBXmlNodeList
        SelectNodes = New CBXmlNodeList(mNode.SelectNodes(strXPath))
    End Function

End Class
