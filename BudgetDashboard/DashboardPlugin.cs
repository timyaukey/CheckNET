using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CheckBookLib;

namespace BudgetDashboard
{
    public class DashboardPlugin : ToolPlugin
    {
        public DashboardPlugin(IHostUI hostUI)
            :base(hostUI)
        {
            SortCode = 101;
        }

        public override void ClickHandler(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("budget dashboard");
            Register reg = HostUI.objGetCurrentRegister();
            if (reg != null)
            {
                Account account = reg.objAccount;
                DashboardData data = new DashboardData(account, 14, 26, new DateTime(2015, 1, 1));
                data.Load();
            }
        }

        public override string GetMenuTitle()
        {
            return "Budget Dashboard";
        }
    }
}
