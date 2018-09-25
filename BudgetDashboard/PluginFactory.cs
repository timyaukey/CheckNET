using System;
using System.Collections.Generic;

using CheckBookLib;

[assembly: PluginFactory(typeof(BudgetDashboard.PluginFactory))]

namespace BudgetDashboard
{

    public class PluginFactory : IPluginFactory
    {
        public IEnumerable<IPlugin> colGetPlugins(IHostUI hostUI_)
        {
            yield return new DashboardPlugin(hostUI_);
        }
    }
}
