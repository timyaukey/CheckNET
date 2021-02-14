Option Strict On
Option Explicit On

Imports System.Xml.Serialization

Public Class PayeeItem
    Public Sub New()
        'Setting default values causes these to be serialized
        'as empty strings if no other value is set for them.
        mOutput = ""
        Input = ""
        Address1 = ""
        Address2 = ""
        City = ""
        State = ""
        Zip = ""
        Account = ""
        Num = ""
        Amount = ""
        Memo = ""
        Cat = ""
        Budget = ""
        NarrowMethod = ""
    End Sub

    <XmlAttribute>
    Public Property Output As String
        Get
            Return mOutput
        End Get
        Set(value As String)
            mOutput = value
            OutputUCS = mOutput.ToUpperInvariant()
        End Set
    End Property
    Private mOutput As String
    <XmlIgnore>
    Public Property OutputUCS As String
    <XmlAttribute>
    Public Property Input As String
    Public Property Address1 As String
    Public Property Address2 As String
    Public Property City As String
    Public Property State As String
    Public Property Zip As String
    Public Property Account As String
    Public Property Num As String
    Public Property Amount As String
    Public Property Memo As String
    Public Property Cat As String
    Public Property Budget As String
    Public Property NarrowMethod As String

    Public Overrides Function ToString() As String
        Return mOutput
    End Function
End Class
