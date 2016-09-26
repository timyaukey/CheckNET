Option Explicit On
Option Strict On

Public Class FilePersistableType

    Sub New(ByVal strName_ As String, ByVal strType_ As String)
        strName = strName_
        strType = strType_
    End Sub

    Public Property strName As String
    Public Property strType As String

    Public Overrides Function ToString() As String
        Return strName
    End Function
End Class
