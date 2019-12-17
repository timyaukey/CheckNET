using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Willowsoft.CheckBook.Lib;
using Willowsoft.CheckBook.PluginCore;

namespace GeneralPlugins.RenumberChecks
{
    public class RenumberChecksPlugin : ToolPlugin
    {
        public RenumberChecksPlugin(IHostUI hostUI)
            : base(hostUI)
        {
        }

        public override void Register()
        {
            HostUI.objToolMenu.Add(new MenuElementAction("Renumber Checks", 103, ClickHandler, GetPluginPath()));
        }

        private void ClickHandler(object sender, EventArgs e)
        {
            try
            {
                if (HostUI.objGetCurrentRegister() == null)
                {
                    HostUI.ErrorMessageBox("Please click on the register window containing the checks you wish to renumber.");
                    return;
                }
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
                ErrorHandling.gTopException(ex);
            }
        }
    }
}
