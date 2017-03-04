Option Strict On
Option Explicit On

Imports System.Xml

Public Class VB6XmlParseError
    Private mError As XmlException

    Public Sub New(ByVal parseError As XmlException)
        mError = parseError
    End Sub

    Public ReadOnly Property Line() As Integer
        Get
            Line = mError.LineNumber
        End Get
    End Property

    Public ReadOnly Property Message() As String
        Get
            Message = mError.Message
        End Get
    End Property
End Class
