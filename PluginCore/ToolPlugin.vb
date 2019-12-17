Option Strict On
Option Explicit On


''' <summary>
''' A convenience implementation of IToolPlugin.
''' </summary>

Public MustInherit Class ToolPlugin
    Implements IPlugin
    Protected ReadOnly HostUI As IHostUI

    Public Sub New(ByVal hostUI_ As IHostUI)
        HostUI = hostUI_
    End Sub

    Public MustOverride Sub Register() Implements IPlugin.Register

    Protected Function GetPluginPath() As String
        Return System.IO.Path.GetFileName(Me.GetType().Assembly.Location)
    End Function
End Class
