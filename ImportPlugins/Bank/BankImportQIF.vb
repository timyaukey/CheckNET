﻿Option Strict On
Option Explicit On

Imports System.IO

Public Class BankImportQIF
    Inherits BankImportBuilder

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Build(ByVal setup As IHostSetup)
        setup.BankImportMenu.Add(New MenuElementAction("QIF File", StandardSortCode(), AddressOf ClickHandler))
    End Sub

    Public Overrides Function GetImportWindowCaption() As String
        Return "Import QIF File From Bank"
    End Function

    Public Overrides Function GetTrxReader() As ITrxReader
        Dim objInput As TrxImportBuilder.InputFile = objAskForFile("Select QIF File From Bank To Import", "QIF", "BankQIFPath")
        If Not objInput Is Nothing Then
            Return New ReadBankQIF(HostUI, objInput.objFile, objInput.strFile)
        End If
        Return Nothing
    End Function

End Class
