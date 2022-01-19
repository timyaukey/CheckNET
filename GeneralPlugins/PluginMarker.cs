using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Willowsoft.CheckBook.Lib;
using Willowsoft.CheckBook.PluginCore;

[assembly: PluginAssembly()]

namespace Willowsoft.CheckBook.GeneralPlugins
{
    public class GeneralPlugins : PluginBase
    {
        public GeneralPlugins(IHostUI hostUI)
            : base(hostUI)
        {
        }

        public override void Register(IHostSetup setup)
        {
            setup.ToolMenu.Add(new MenuElementAction("Intuit Export (IIF Format)", 102, IntuitExportClickHandler));
            setup.ToolMenu.Add(new MenuElementRegister(HostUI, "Renumber Checks", 103, RenumberChecksClickHandler));
            setup.ToolMenu.Add(new MenuElementRegister(HostUI, "Find Missing Checks", 104, MissingChecksClickHandler));
            setup.ReportMenu.Add(new MenuElementAction("Summarize All Accounts", 210, SummarizeAllClickHandler));

            MetadataInternal = new PluginMetadata("External Tools", "Willow Creek Software",
                System.Reflection.Assembly.GetExecutingAssembly(), null,
                "Miscellaneous tools provided by plugin distributed with the software.", null);
        }

        private void IntuitExportClickHandler(object sender, EventArgs e)
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
                if (engine.Run())
                    HostUI.InfoMessageBox("Exported to " + engine.OutputPath);
                else
                    HostUI.ErrorMessageBox("Export canceled.");
            }
            catch (Exception ex)
            {
                ErrorHandling.TopException(ex);
            }
        }

        private void RenumberChecksClickHandler(object sender, RegisterEventArgs e)
        {
            try
            {
                using (RenumberChecksForm frm = new RenumberChecksForm())
                {
                    if (frm.ShowDialog(HostUI) != System.Windows.Forms.DialogResult.OK)
                    {
                        HostUI.InfoMessageBox("Renumber checks canceled.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.TopException(ex);
            }
        }

        private void MissingChecksClickHandler(object sender, RegisterEventArgs e)
        {
            try
            {
                using (MissingChecksForm frm = new MissingChecksForm())
                {
                    frm.ShowDialog(HostUI);
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.TopException(ex);
            }
        }

        private void SummarizeAllClickHandler(object sender, EventArgs e)
        {
            try
            {
                using (SummarizeAllAccountsForm frm = new SummarizeAllAccountsForm())
                {
                    frm.ShowDialog(HostUI);
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.TopException(ex);
            }
        }
    }
}
