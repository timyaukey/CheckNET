Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class CheckImportInsight
    Inherits CheckImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import Digital Insight Online Banking Checks"
    End Function

    Public Overrides Function GetMenuTitle() As String
        Return "Digital Insight Clipboard"
    End Function

    Protected Overrides Function GetCheckSpecs() As ReadChecksSpec
        Return New ReadChecksSpec(6, 0, 1, 2, -1)
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadChecks(Utilities.objClipboardReader(), "(clipboard)", GetCheckSpecs())
    End Function
End Class
