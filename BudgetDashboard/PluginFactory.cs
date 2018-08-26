using System;
using System.Collections.Generic;

using CheckBookLib;

[assembly: PluginFactory(typeof(BudgetDashboard.PluginFactory))]

namespace BudgetDashboard
{

    public class PluginFactory : IPluginFactory
    {
        public IEnumerable<ToolPlugin> colGetPlugins(IHostUI hostUI_)
        {
            yield return new DashboardPlugin(hostUI_);
        }
    }
}
