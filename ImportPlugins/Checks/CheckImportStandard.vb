﻿Option Strict On
Option Explicit On


Public Class CheckImportStandard
    Inherits CheckImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register(ByVal setup As IHostSetup)
        setup.CheckImportMenu.Add(New MenuElementAction("Standard Clipboard", 1, AddressOf ClickHandler))
        MetadataInternal = New PluginMetadata("Standard check clipboard import", "Willow Creek Software",
                                    Reflection.Assembly.GetExecutingAssembly(), Nothing, "", Nothing)
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import Standard Checks"
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadChecks(Utilities.GetClipboardReader(), "(clipboard)", New ReadChecksSpec(1, 0, 2, 3, -1))
    End Function
End Class
