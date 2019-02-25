using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CheckBookLib;
using PluginCore;

[assembly: PluginFactory(typeof(GeneralPlugins.PluginFactory))]

namespace GeneralPlugins
{
    public class PluginFactory : IPluginFactory
    {
        public IEnumerable<IPlugin> colGetPlugins(IHostUI hostUI_)
        {
            yield return new IntuitExport.IntuitExportPlugin(hostUI_);
        }
    }
}
