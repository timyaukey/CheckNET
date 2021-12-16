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

namespace Willowsoft.CheckBook.GeneralPlugins
{
    public partial class MissingChecksForm : Form
    {
        private IHostUI HostUI;

        public MissingChecksForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IHostUI hostUI)
        {
            HostUI = hostUI;
            ctlStartDate.Value = DateTime.Today.AddYears(-1);
            ctlEndDate.Value = DateTime.Today;
            return this.ShowDialog();
        }

        private class CheckNumUsage
        {
            public int Number;
            public DateTime TrxDate;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            DateTime startDate = ctlStartDate.Value;
            DateTime endDate = ctlEndDate.Value;
            List<CheckNumUsage> usages = new List<CheckNumUsage>();
            foreach(Register reg in HostUI.GetCurrentRegister().Account.Registers)
            {
                foreach(BankTrx trx in new RegDateRange<BankTrx>(reg, startDate, endDate))
                {
                    int currentNum;
                    if (Int32.TryParse(trx.Number, out currentNum))
                    {
                        CheckNumUsage usage = new CheckNumUsage() { Number = currentNum, TrxDate = trx.TrxDate };
                        usages.Add(usage);
                    }
                }
            }
            usages.Sort((u1, u2) => u1.Number.CompareTo(u2.Number));
            int previousNum = 0;
            lstMissing.Items.Clear();
            foreach(CheckNumUsage usage in usages)
            {
                int gap = usage.Number - (previousNum + 1);
                if (gap > 0 && gap < 50 && previousNum > 0)
                {
                    string gapDetails;
                    if (gap == 1)
                        gapDetails = "#" + (previousNum + 1).ToString();
                    else
                        gapDetails = "#" + (previousNum + 1).ToString() + "-#" + (usage.Number -1).ToString();
                    lstMissing.Items.Add(gapDetails + "  (" + usage.TrxDate.ToShortDateString() +")");
                }
                previousNum = usage.Number;
            }
        }
    }
}
