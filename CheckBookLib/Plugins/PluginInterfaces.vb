Option Strict On
Option Explicit On

''' <summary>
''' All plugins must implement this interface.
''' At the moment this interface is empty, but we
''' may add a member to it to give a name to the plugin
''' in a management user interface.
''' </summary>

Public Interface IPlugin

End Interface

''' <summary>
''' All plugins that are added to the main program user interface
''' must implement this interface or a subclass of this interface.
''' The main program user interface decides which menu to add a plugin 
''' to based on which interface the plugin implements. For example, all
''' check importers must inherit from ICheckImportPlugin to be added
''' to the list of check importers in the UI.
''' </summary>

Public Interface IToolPlugin
    Inherits IPlugin

    Function GetMenuTitle() As String
    Sub ClickHandler(ByVal sender As Object, ByVal e As EventArgs)
    Function SortCode() As Integer
End Interface

Public Interface IReportPlugin
    Inherits IToolPlugin
End Interface

Public Interface IBankImportPlugin
    Inherits IToolPlugin
End Interface

Public Interface ICheckImportPlugin
    Inherits IToolPlugin
End Interface

Public Interface IDepositImportPlugin
    Inherits IToolPlugin
End Interface

Public Interface IInvoiceImportPlugin
    Inherits IToolPlugin
End Interface
