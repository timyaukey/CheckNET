﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Willowsoft.CheckBook.Lib;
using Willowsoft.CheckBook.PluginCore;

namespace Willowsoft.CheckBook.GeneralPlugins
{
    public class IntuitExportPlugin : PluginBase
    {
        public IntuitExportPlugin(IHostUI hostUI)
            : base(hostUI)
        {
        }

        public override void Register(IHostSetup setup)
        {
            setup.objToolMenu.Add(new MenuElementAction("Intuit Export (IIF Format)", 102, ClickHandler, GetPluginPath()));
        }

        private void ClickHandler(object sender, EventArgs e)
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
                ErrorHandling.gTopException(ex);
            }
        }
    }
}
