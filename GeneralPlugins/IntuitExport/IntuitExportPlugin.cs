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
            DateTime startDate = new DateTime(2018, 1, 1);
            DateTime endDate = new DateTime(2018, 1, 31);
            ExportEngine engine = new ExportEngine(HostUI, startDate, endDate);
            engine.Run();
            HostUI.InfoMessageBox("Exported to " + engine.OutputPath);
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
