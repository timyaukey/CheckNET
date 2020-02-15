using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Willowsoft.CheckBook.Lib;
using Willowsoft.CheckBook.PluginCore;

[assembly: PluginAssembly()]

namespace Willowsoft.CheckBook.BudgetDashboard
{
    public class DashboardPlugin : PluginBase
    {
        public DashboardPlugin(IHostUI hostUI)
            :base(hostUI)
        {
        }

        public override void Register(IHostSetup setup)
        {
            setup.objToolMenu.Add(new MenuElementAction("Budget Dashboard", 101, ClickHandler, GetPluginPath()));
        }

        private void ClickHandler(object sender, EventArgs e)
        {
            using (var specsForm = new BudgetSpecsForm(HostUI))
            {
                if (specsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DashboardData data = new DashboardData(HostUI.objCompany, specsForm.Handler, specsForm.PeriodDays, specsForm.PeriodCount, specsForm.StartDate);
                    var budgetForm = new BudgetDashboardForm();
                    budgetForm.Show(HostUI, data);
                }
            }
        }
    }
}
