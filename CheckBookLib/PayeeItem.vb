Option Strict On
Option Explicit On

Imports System.Xml.Serialization

Public Class PayeeItem
    Public Sub New()
        'Setting default values causes these to be serialized
        'as empty strings if no other value is set for them.
        mOutput = ""
        Input = ""
        'Min = ""     Not serialized if empty
        'Max = ""     Not serialized if empty
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
    <XmlAttribute>
    Public Property Min As String
    <XmlAttribute>
    Public Property Max As String
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
    Public Property AllowAutoBatchNew As String
    Public Property AllowAutoBatchUpdate As String

    Public ReadOnly Property IsAllowAutoBatchNew() As Boolean
        Get
            If Not String.IsNullOrEmpty(AllowAutoBatchNew) Then
                If AllowAutoBatchNew.ToLower() = PayeeItem.Yes Then
                    Return True
                End If
            End If
            Return False
        End Get
    End Property

    Public ReadOnly Property IsAllowAutoBatchUpdate() As Boolean
        Get
            If Not String.IsNullOrEmpty(AllowAutoBatchUpdate) Then
                If AllowAutoBatchUpdate.ToLower() = PayeeItem.Yes Then
                    Return True
                End If
            End If
            Return False
        End Get
    End Property

    Public Const Yes As String = "yes"

    Public Overrides Function ToString() As String
        Return mOutput
    End Function
End Class
