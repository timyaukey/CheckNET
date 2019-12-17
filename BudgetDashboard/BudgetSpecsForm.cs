using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Willowsoft.CheckBook.Lib;
using Willowsoft.CheckBook.PluginCore;

namespace BudgetDashboard
{
    public partial class BudgetSpecsForm : Form
    {
        private IHostUI HostUI;
        public DateTime StartDate;
        public int PeriodDays;
        public int PeriodCount;
        public BudgetTypeHandler Handler;

        public BudgetSpecsForm(IHostUI hostUI)
        {
            HostUI = hostUI;
            InitializeComponent();
            ctlStartDate.Value = new DateTime(DateTime.Today.Year, 1, 1);
            cboBudgetType.Items.Add(new BudgetTypeCash());
            cboBudgetType.Items.Add(new BudgetTypeIncExp());
            cboBudgetType.SelectedIndex = 0;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdOkay_Click(object sender, EventArgs e)
        {
            Handler = (BudgetTypeHandler)cboBudgetType.SelectedItem;
            StartDate = ctlStartDate.Value.Date;
            if (int.TryParse(txtPeriodDays.Text, out PeriodDays))
            {
                if (int.TryParse(txtPeriodCount.Text, out PeriodCount))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    return;
                }
            }
            HostUI.ErrorMessageBox("Invalid budget specs");
        }
    }
}
