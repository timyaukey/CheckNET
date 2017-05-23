Option Strict On
Option Explicit On

Imports CheckBookLib

<Assembly: PluginFactory(GetType(ToolPlugins))>

Public Class ToolPlugins
    Implements IPluginFactory

    Public Iterator Function colGetPlugins(hostUI_ As IHostUI) As IEnumerable(Of ToolPlugin) _
        Implements IPluginFactory.colGetPlugins

        Yield New ReconcilePlugin(hostUI_)
        Yield New FindLiveBudgetsPlugin(hostUI_)
        Yield New AdjustBudgetsToCashflowPlugin(hostUI_)
        Yield New CategoryReportPlugin(hostUI_)
        Yield New PayablesReportPlugin(hostUI_)
        Yield New PersonalBusinessPlugin(hostUI_)
    End Function
End Class
