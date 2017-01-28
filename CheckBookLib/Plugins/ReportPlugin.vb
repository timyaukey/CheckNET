Option Strict On
Option Explicit On

Public MustInherit Class ReportPlugin
    Inherits ToolPlugin

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

End Class
