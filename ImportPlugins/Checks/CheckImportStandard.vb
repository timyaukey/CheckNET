Option Strict On
Option Explicit On


Public Class CheckImportStandard
    Inherits CheckImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register()
        HostUI.objCheckImportMenu.Add(New MenuElementAction("Standard Clipboard", 1, AddressOf ClickHandler, GetPluginPath()))
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import Standard Checks"
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadChecks(Utilities.objClipboardReader(), "(clipboard)", New ReadChecksSpec(1, 0, 2, 3, -1))
    End Function
End Class
