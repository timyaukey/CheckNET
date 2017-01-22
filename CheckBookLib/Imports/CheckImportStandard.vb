Option Strict On
Option Explicit On

Public Class CheckImportStandard
    Inherits CheckImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
        SortCode = 1
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import Standard Checks"
    End Function

    Public Overrides Function GetMenuTitle() As String
        Return "Standard Clipboard"
    End Function

    Protected Overrides Function GetCheckSpecs() As ReadChecksSpec
        Return New ReadChecksSpec(1, 0, 2, 3, -1)
    End Function
End Class
