using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CheckBookLib;

namespace GeneralPlugins.IntuitExport
{
    public class IntuitExportPlugin : ToolPlugin
    {
        public IntuitExportPlugin(IHostUI hostUI)
            : base(hostUI)
        {
        }

        public override void ClickHandler(object sender, EventArgs e)
        {
            HostUI.InfoMessageBox("Done...");
        }

        public override string GetMenuTitle()
        {
            return "Intuit Export (IIF Format)";
        }

        public override int SortCode()
        {
            return 102;
        }
    }
}
