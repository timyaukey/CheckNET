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
            try
            {
                ExportEngine engine = new ExportEngine(HostUI);
                using (ExportForm frm = new ExportForm())
                {
                    if (frm.ShowDialog(engine, HostUI) != System.Windows.Forms.DialogResult.OK)
                    {
                        HostUI.InfoMessageBox("Export canceled.");
                        return;
                    }
                }
                engine.Run();
                HostUI.InfoMessageBox("Exported to " + engine.OutputPath);
            }
            catch (Exception ex)
            {
                ErrorHandling.gTopException(ex);
            }
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
