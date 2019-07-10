using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CheckBookLib;
using PluginCore;

[assembly: PluginAssembly()]

namespace BudgetDashboard
{
    public class DashboardPlugin : ToolPlugin
    {
        public DashboardPlugin(IHostUI hostUI)
            :base(hostUI)
        {
        }

        public override void Register()
        {
            HostUI.objToolMenu.Add(new MenuElementAction("Budget Dashboard", 101, ClickHandler, GetPluginPath()));
        }

        private void ClickHandler(object sender, EventArgs e)
        {
            Register reg = HostUI.objGetCurrentRegister();
            if (reg == null)
            {
                HostUI.ErrorMessageBox("Select a register window first");
                return;
            }
            using (var specsForm = new BudgetSpecsForm(HostUI))
            {
                if (specsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Account account = reg.objAccount;
                    DashboardData data = new DashboardData(account, specsForm.PeriodDays, specsForm.PeriodCount, specsForm.StartDate);
                    var budgetForm = new BudgetDashboardForm();
                    budgetForm.Show(HostUI, data);
                }
            }
        }
    }
}
