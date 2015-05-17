Option Strict Off
Option Explicit On

Imports System.Xml

Public Class VB6XmlText
    Inherits VB6XmlNode

    Private mText As XmlText

    Public Sub New(ByVal text As XmlText)
        MyBase.New(text)
        mText = text
    End Sub
End Class
