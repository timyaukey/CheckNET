Option Strict On
Option Explicit On

Public Interface IPluginFactory
    Function colGetPlugins(ByVal hostUI_ As IHostUI) As IEnumerable(Of ToolPlugin)
End Interface
