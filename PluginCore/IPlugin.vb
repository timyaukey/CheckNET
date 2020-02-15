Option Strict On
Option Explicit On

''' <summary>
''' All plugins must implement this interface, and contain a constructor that
''' takes a single parameter of type IHostUI.
''' Every class meeting these conditions, if it is also in an assembly marked with the
''' "PluginAssembly" assembly level attribute and in the same folder as the main executing
''' program, will be loaded into the main AppDomain at program startup and its
''' Register() method called.
''' </summary>

Public Interface IPlugin
    ''' <summary>
    ''' Called to let the plugin wire itself into the software.
    ''' </summary>
    Sub Register(ByVal setup As IHostSetup)
End Interface
