using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Willowsoft.CheckBook.Lib;
using Willowsoft.CheckBook.PluginCore;

namespace Willowsoft.CheckBook.GeneralPlugins
{
    public class RenumberChecksPlugin : PluginBase
    {
        public RenumberChecksPlugin(IHostUI hostUI)
            : base(hostUI)
        {
        }

        public override void Register(IHostSetup setup)
        {
            setup.ToolMenu.Add(new MenuElementRegister(HostUI, "Renumber Checks", 103, ClickHandler));
            MetadataInternal = new PluginMetadata("Renumber Checks", "Willow Creek Software",
                System.Reflection.Assembly.GetExecutingAssembly(), null,
                "", null);
        }

        private void ClickHandler(object sender, RegisterEventArgs e)
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
    }
}
