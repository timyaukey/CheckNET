Option Strict On
Option Explicit On

''' <summary>
''' All plugins must implement this interface.
''' </summary>

Public Interface IPlugin
    ''' <summary>
    ''' Called to let the plugin wire itself into the software.
    ''' </summary>
    Sub Register()
End Interface
