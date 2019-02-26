﻿Option Strict On
Option Explicit On

Imports CheckBookLib
Imports PluginCore

''' <summary>
''' All deposit import plugins must inherit from this.
''' </summary>

Public MustInherit Class DepositImportPlugin
    Inherits TrxImportPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Function GetImportHandler() As IImportHandler
        Return New ImportHandlerDeposits()
    End Function

End Class
