Option Strict Off
Option Explicit On

Imports System.Xml

Public Class VB6XmlDocument
    Inherits VB6XmlNode

    Private mDoc As XmlDocument
    Private mstrFullPath As String
    Private mParseError As VB6XmlParseError

    Public Sub New()
        Me.New(New XmlDocument())
    End Sub

    Public Sub New(ByVal doc As XmlNode)
        MyBase.New(doc)
        mDoc = doc
    End Sub

    Public Function CreateElement(ByVal strElementName As String) As VB6XmlElement
        CreateElement = VB6XmlNode.Create(mDoc.CreateElement(strElementName))
    End Function

    Public Sub Load(ByVal strFullPath As String)
        mstrFullPath = strFullPath
        mParseError = Nothing
        Try
            mDoc.Load(strFullPath)
        Catch ex As XmlException
            mParseError = New VB6XmlParseError(ex)
        End Try
    End Sub

    Public Sub LoadXml(ByVal strXml As String)
        mDoc.LoadXml(strXml)
    End Sub

    Public Sub Save(ByVal strFullPath As String)
        mDoc.Save(strFullPath)
    End Sub

    Public ReadOnly Property ParseError() As VB6XmlParseError
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

    Public ReadOnly Property DocumentElement() As VB6XmlElement
        Get
            DocumentElement = New VB6XmlElement(mDoc.DocumentElement)
        End Get
    End Property

    Public Function CreateTextNode(ByVal strText As String) As VB6XmlText
        CreateTextNode = New VB6XmlText(mDoc.CreateTextNode(strText))
    End Function
End Class
