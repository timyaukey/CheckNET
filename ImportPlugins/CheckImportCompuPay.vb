﻿Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class CheckImportCompuPay
    Inherits CheckImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import CompuPay Payroll Checks"
    End Function

    Public Overrides Function GetMenuTitle() As String
        Return "CompuPay Payroll Clipboard"
    End Function

    Protected Overrides Function GetCheckSpecs() As ReadChecksSpec
        Return New ReadChecksSpec(0, 5, 9, 12, -1)
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Return New ReadChecks(Utilities.objClipboardReader(), "(clipboard)", GetCheckSpecs())
    End Function
End Class
