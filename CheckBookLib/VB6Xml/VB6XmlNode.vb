Option Strict Off
Option Explicit On

Imports System.Xml

Public Class VB6XmlNode

    Private mNode As XmlNode

    Public Shared Function Create(ByVal node As XmlNode) As VB6XmlNode
        If node Is Nothing Then
            Create = Nothing
        ElseIf TypeOf node Is XmlElement Then
            Create = New VB6XmlElement(node)
        ElseIf TypeOf node Is XmlText Then
            Create = New VB6XmlText(node)
        ElseIf TypeOf node Is XmlDocument Then
            Create = New VB6XmlDocument(node)
        Else
            Create = New VB6XmlNode(node)
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

    Public ReadOnly Property ParentNode() As VB6XmlNode
        Get
            ParentNode = VB6XmlNode.Create(mNode.ParentNode)
        End Get
    End Property

    Public ReadOnly Property ChildNodes() As VB6XmlNodeList
        Get
            ChildNodes = New VB6XmlNodeList(mNode.ChildNodes)
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

    Public Function CloneNode(ByVal blnDeepClone As Boolean) As VB6XmlNode
        CloneNode = VB6XmlNode.Create(mNode.CloneNode(blnDeepClone))
    End Function

    Public Sub AppendChild(ByVal newChild As VB6XmlNode)
        mNode.AppendChild(newChild.RawNode)
    End Sub

    Public Sub RemoveChild(ByVal child As VB6XmlNode)
        mNode.RemoveChild(child.RawNode)
    End Sub

    Public Sub InsertBefore(ByVal newChild As VB6XmlNode, ByVal refChild As VB6XmlNode)
        mNode.InsertBefore(newChild.RawNode, refChild.RawNode)
    End Sub

    Public Function SelectSingleNode(ByVal strXPath As String) As VB6XmlNode
        SelectSingleNode = VB6XmlNode.Create(mNode.SelectSingleNode(strXPath))
    End Function

    Public Function SelectNodes(ByVal strXPath As String) As VB6XmlNodeList
        SelectNodes = New VB6XmlNodeList(mNode.SelectNodes(strXPath))
    End Function

End Class
