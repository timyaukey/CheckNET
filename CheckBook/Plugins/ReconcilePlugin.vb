﻿Option Strict On
Option Explicit On

Imports CheckBookLib

Public Class ReconcilePlugin
    Inherits ToolPlugin

    Public Sub New(hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register()
        HostUI.objToolMenu.Add(New MenuElementAction("Reconcile", 1, AddressOf ClickHandler, GetPluginPath()))
    End Sub

    Private Sub ClickHandler(sender As Object, e As EventArgs)
        Try
            Dim frm As ReconAcctSelectForm = New ReconAcctSelectForm
            frm.Init(HostUI)
            frm.ShowDialog()
            Exit Sub
        Catch ex As Exception
            gTopException(ex)
        End Try
    End Sub
End Class
