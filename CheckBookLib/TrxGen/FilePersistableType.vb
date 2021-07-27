Option Explicit On
Option Strict On

Public Class FilePersistableType

    Sub New(ByVal strName_ As String, ByVal strType_ As String)
        Name = strName_
        PersistType = strType_
    End Sub

    Public Property Name As String
    Public Property PersistType As String

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class
