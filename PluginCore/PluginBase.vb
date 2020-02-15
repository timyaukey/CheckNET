Option Strict On
Option Explicit On

''' <summary>
''' A convenience implementation of IPlugin.
''' </summary>

Public MustInherit Class PluginBase
    Implements IPlugin
    Protected ReadOnly HostUI As IHostUI

    Public Sub New(ByVal hostUI_ As IHostUI)
        HostUI = hostUI_
    End Sub

    Public MustOverride Sub Register(ByVal setup As IHostSetup) Implements IPlugin.Register

    Protected Function GetPluginPath() As String
        Return System.IO.Path.GetFileName(Me.GetType().Assembly.Location)
    End Function
End Class
