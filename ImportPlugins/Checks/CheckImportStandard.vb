﻿Option Strict On
Option Explicit On

Imports CheckBookLib

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

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadChecks(Utilities.objClipboardReader(), "(clipboard)", New ReadChecksSpec(1, 0, 2, 3, -1))
    End Function
End Class