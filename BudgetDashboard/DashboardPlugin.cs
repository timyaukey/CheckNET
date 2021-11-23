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
            : base(hostUI)
        {
        }

        public override void Register(IHostSetup setup)
        {
            setup.ReportMenu.Add(new MenuElementAction("Budget Dashboard", 300, ClickHandler));
            setup.HelpMenu.Add(new MenuElementAction("Budget Dashboard", 220, HelpHandler));
            Willowsoft.TamperProofData.IStandardLicense license = new BudgetDashboardLicense();
            license.Load(Company.LicenseFolderPath());
            setup.AddExtraLicense(license);
            MetadataInternal = new PluginMetadata("Budget Dashboard", "Willow Creek Software", 
                System.Reflection.Assembly.GetExecutingAssembly(), null,
                "An easy way to manipulate large numbers of budgets.", license);
        }

        private void ClickHandler(object sender, EventArgs e)
        {
            using (var specsForm = new BudgetSpecsForm(HostUI))
            {
                if (specsForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DashboardData data = new DashboardData(HostUI.Company, specsForm.Handler, specsForm.PeriodDays, specsForm.PeriodCount, specsForm.StartDate);
                    var budgetForm = new BudgetDashboardForm();
                    budgetForm.Show(HostUI, data);
                }
            }
        }

        private void HelpHandler(object sender, EventArgs e)
        {
            HostUI.ShowHelp("BudgetDashboard.html");
        }
    }
}
