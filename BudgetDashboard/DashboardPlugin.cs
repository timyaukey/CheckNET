﻿using System;
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
            using (var specsForm = new BudgetSpecsForm(HostUI))
            {
                if (specsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DashboardData data = new DashboardData(HostUI.objCompany, specsForm.PeriodDays, specsForm.PeriodCount, specsForm.StartDate);
                    var budgetForm = new BudgetDashboardForm();
                    budgetForm.Show(HostUI, data);
                }
            }
        }
    }
}
