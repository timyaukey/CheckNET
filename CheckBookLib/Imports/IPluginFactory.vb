Option Strict On
Option Explicit On

''' <summary>
''' Return a list of plugin instances.
''' To dynamically load plugins and attach them to its user
''' interface the main program just has to load all assemblies
''' in a particular folder, scan all the public class types in
''' those assemblies, create an instance of every public
''' class type that implements this interface, and then use
''' this interface to get all the plugins.
''' </summary>

Public Interface IPluginFactory
    Function colGetPlugins(ByVal hostUI_ As IHostUI) As IEnumerable(Of ToolPlugin)
End Interface
