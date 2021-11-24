Option Strict On
Option Explicit On

<Assembly: PluginAssembly()>

Public Class PluginMarker
    Inherits PluginBase

    Public Sub New(ByVal hostUI_ As IHostUI)
        MyBase.New(hostUI_)
    End Sub

    Public Overrides Sub Register(setup As IHostSetup)
        setup.SetSearchFormFactory(Function() New SearchForm)
        MetadataInternal = New PluginMetadata("Search Form", "Willow Creek Software",
            Reflection.Assembly.GetExecutingAssembly(), Nothing, "", Nothing)
    End Sub
End Class
