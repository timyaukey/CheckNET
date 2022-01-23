using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Willowsoft.CheckBook.Lib;
using Willowsoft.CheckBook.PluginCore;

namespace Willowsoft.CheckBook.GeneralPlugins
{
    public partial class SummarizeAllAccountsForm : Form
    {
        private IHostUI HostUI;

        public SummarizeAllAccountsForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IHostUI hostUI)
        {
            HostUI = hostUI;
            ctlEndDate.Value = DateTime.Today;
            return this.ShowDialog();
        }

        private void btnSummarize_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime endDate = ctlEndDate.Value.Date;
                bool includeFake = chkFakeInBalance.Checked;
                List<AccountSummary> summaries = new List<AccountSummary>();
                // Load non-personal accounts.
                foreach(Account acct in HostUI.Company.Accounts)
                {
                    if (acct.AcctType != Account.AccountType.Personal)
                    {
                        AccountSummary summary = new AccountSummary();
                        summary.RegularAcct = acct;
                        summary.Name = acct.Title;
                        summaries.Add(summary);
                        foreach(Register reg in acct.Registers)
                        {
                            ScanRegister(reg, endDate, includeFake, out decimal balance, out bool anyFake);
                            summary.RegularBalance += balance;
                            if (anyFake)
                                summary.AnyFakeTrx = true;
                        }
                    }
                }
                // Load personal accounts.
                foreach(Account acct in HostUI.Company.Accounts)
                {
                    if (acct.AcctType == Account.AccountType.Personal)
                    {
                        AccountSummary matchingSummary = null;
                        foreach (AccountSummary existingSummary in summaries)
                        {
                            if (existingSummary.RegularAcct.RelatedAcct1 == acct)
                            {
                                matchingSummary = existingSummary;
                                break;
                            }
                        }
                        if (matchingSummary == null)
                        {
                            matchingSummary = new AccountSummary();
                            matchingSummary.Name = acct.Title;
                            summaries.Add(matchingSummary);
                        }
                        matchingSummary.PersonalAccount = acct;
                        foreach (Register reg in acct.Registers)
                        {
                            ScanRegister(reg, endDate, includeFake, out decimal balance, out bool anyFake);
                            matchingSummary.PersonalBalance += balance;
                            if (anyFake)
                                matchingSummary.AnyFakeTrx = true;
                        }
                    }
                }
                // Sort list by name
                summaries.Sort((s1, s2) => s1.Name.CompareTo(s2.Name));
                // Build list view
                lvwAccounts.Items.Clear();
                foreach(AccountSummary summary in summaries)
                {
                    string regularBal = "";
                    if (summary.RegularBalance != 0M)
                        regularBal = Utilities.FormatCurrency(summary.RegularBalance);
                    string personalBal = "";
                    if (summary.PersonalBalance != 0M)
                        personalBal = Utilities.FormatCurrency(summary.PersonalBalance);
                    string anyFake = "";
                    if (summary.AnyFakeTrx)
                        anyFake = "Yes";
                    ListViewItem itm = new ListViewItem(new string[] {
                        summary.Name, 
                        regularBal, 
                        personalBal,
                        Utilities.FormatCurrency(summary.TotalBalance),
                        anyFake});
                    lvwAccounts.Items.Add(itm);
                }
            }
            catch(Exception ex)
            {
                ErrorHandling.TopException(ex);
            }
        }

        private void ScanRegister(Register reg, DateTime endDate, bool includeFake, out decimal balance, out bool anyFake)
        {
            balance = 0M;
            anyFake = false;
            RegIterator<BaseTrx> scanner = new RegDateRange<BaseTrx>(reg, new DateTime(1800, 1, 1), endDate);
            foreach (BaseTrx baseTrx in scanner)
            {
                if ((baseTrx is BankTrx) || (baseTrx is ReplicaTrx))
                {
                    if (baseTrx.IsFake)
                    {
                        anyFake = true;
                        if (includeFake)
                            balance += baseTrx.Amount;
                    }
                    else
                        balance += baseTrx.Amount;
                }
            }
        }

        private class AccountSummary
        {
            public string Name;
            public Account RegularAcct;
            public Account PersonalAccount;
            public decimal RegularBalance;
            public decimal PersonalBalance;
            public bool AnyFakeTrx;

            public decimal TotalBalance
            {
                get
                {
                    return RegularBalance + PersonalBalance;
                }
            }
        }
    }
}
