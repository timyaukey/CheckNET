Option Strict On
Option Explicit On

Imports System.Xml

Public Class CBXmlText
    Inherits CBXmlNode

    Private mText As XmlText

    Public Sub New(ByVal text As XmlText)
        MyBase.New(text)
        mText = text
    End Sub
End Class
