Option Strict On
Option Explicit On

Imports System.Xml

Public Class CBXmlDocument
    Inherits CBXmlNode

    Private mDoc As XmlDocument
    Private mstrFullPath As String
    Private mParseError As CBXmlParseError

    Public Sub New()
        Me.New(New XmlDocument())
    End Sub

    Public Sub New(ByVal doc As XmlNode)
        MyBase.New(doc)
        mDoc = DirectCast(doc, XmlDocument)
    End Sub

    Public Function CreateElement(ByVal strElementName As String) As CBXmlElement
        Return DirectCast(CBXmlNode.Create(mDoc.CreateElement(strElementName)), CBXmlElement)
    End Function

    Public Sub Load(ByVal strFullPath As String)
        mstrFullPath = strFullPath
        mParseError = Nothing
        Try
            mDoc.Load(strFullPath)
        Catch ex As XmlException
            mParseError = New CBXmlParseError(ex)
        End Try
    End Sub

    Public Sub LoadXml(ByVal strXml As String)
        mDoc.LoadXml(strXml)
    End Sub

    Public Sub Save(ByVal strFullPath As String)
        mDoc.Save(strFullPath)
    End Sub

    Public ReadOnly Property ParseError() As CBXmlParseError
        Get
            ParseError = mParseError
        End Get
    End Property

    Public ReadOnly Property FullPath() As String
        Get
            FullPath = mstrFullPath
        End Get
    End Property

    Public Sub SetProperty(ByVal strName As String, ByVal strValue As String)
        'mDoc.setProperty(strName, strValue)
    End Sub

    Public ReadOnly Property DocumentElement() As CBXmlElement
        Get
            DocumentElement = New CBXmlElement(mDoc.DocumentElement)
        End Get
    End Property

    Public Function CreateTextNode(ByVal strText As String) As CBXmlText
        CreateTextNode = New CBXmlText(mDoc.CreateTextNode(strText))
    End Function
End Class
