using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BudgetDashboard
{
    public partial class BudgetSpecsForm : Form
    {
        public DateTime StartDate;
        public int PeriodDays;
        public int PeriodCount;

        public BudgetSpecsForm()
        {
            InitializeComponent();
            ctlStartDate.Value = new DateTime(DateTime.Today.Year, 1, 1);
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmdOkay_Click(object sender, EventArgs e)
        {
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
            MessageBox.Show("Invalid budget specs");
        }
    }
}
