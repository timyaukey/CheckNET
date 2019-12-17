Option Strict On
Option Explicit On


Public Class CheckImportInsight
    Inherits CheckImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register()
        HostUI.objCheckImportMenu.Add(New MenuElementAction("Digital Insight Clipboard", StandardSortCode(), AddressOf ClickHandler, GetPluginPath()))
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import Digital Insight Online Banking Checks"
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadChecks(Utilities.objClipboardReader(), "(clipboard)", New ReadChecksSpec(6, 0, 1, 2, -1))
    End Function
End Class
