using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Willowsoft.CheckBook.Lib;
using Willowsoft.CheckBook.PluginCore;

namespace Willowsoft.CheckBook.GeneralPlugins.CalculateInterest
{
    public partial class CalculateInterestForm : Form
    {
        private IHostUI HostUI;
        private bool PersonalAcctExists;

        public CalculateInterestForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IHostUI hostUI)
        {
            HostUI = hostUI;
            ctlStartDate.Value = new DateTime(1900, 1, 1);
            ctlEndDate.Value = DateTime.Today;
            LoadRegisterList();
            UITools.LoadComboFromStringTranslator(cboInterestCategory, HostUI.Company.Categories, false);
            LoadInterestTypes();
            return this.ShowDialog();
        }

        private void LoadRegisterList()
        {
            Account currentAcct = HostUI.GetCurrentRegister().Account;
            lvwRegisters.Items.Clear();
            PersonalAcctExists = false;
            foreach(Register reg in currentAcct.Registers)
            {
                ListViewItem itm = new ListViewItem(reg.Title);
                itm.Tag = reg;
                itm.Checked = true;
                lvwRegisters.Items.Add(itm);
            }
            if (currentAcct.RelatedAcct1 != null)
            {
                PersonalAcctExists = true;
                foreach(Register reg in currentAcct.RelatedAcct1.Registers)
                {
                    ListViewItem itm = new ListViewItem(reg.Title + " (personal)");
                    itm.Tag = reg;
                    itm.Checked = true;
                    lvwRegisters.Items.Add(itm);
                }
            }
        }

        private void LoadInterestTypes()
        {
            cboInterestType.Items.Add(new InterestComputeDaily(360, false));
            cboInterestType.Items.Add(new InterestComputeDaily(360, true));
            cboInterestType.Items.Add(new InterestComputeDaily(365, false));
            cboInterestType.Items.Add(new InterestComputeDaily(365, true));
            cboInterestType.Items.Add(new InterestComputeDaily(366, false));
            cboInterestType.Items.Add(new InterestComputeDaily(366, true));
        }

        private string GetInterestCategoryKey()
        {
            CBListBoxItem itm = (CBListBoxItem)cboInterestCategory.SelectedItem;
            if (itm != null)
                return HostUI.Company.Categories.get_GetKey(itm.LBValue);
            return null;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = ctlStartDate.Value.Date;
                DateTime endDate = ctlEndDate.Value.Date;
                int interestDays = endDate.Subtract(startDate).Days + 1;
                if (interestDays < 1)
                {
                    HostUI.ErrorMessageBox("Start date must be before end date.");
                    return;
                }
                IInterestCalculator calculator = (IInterestCalculator)cboInterestType.SelectedItem;
                if (calculator == null)
                {
                    HostUI.ErrorMessageBox("Please select an interest type.");
                    return;
                }
                string interestCatKey = GetInterestCategoryKey();
                if (interestCatKey == null)
                {
                    HostUI.ErrorMessageBox("Please select an interest category.");
                    return;
                }
                decimal annualRate;
                if (!decimal.TryParse(txtAnnualRate.Text, out annualRate))
                {
                    HostUI.ErrorMessageBox("Please enter a valid annual interest rate, like \"9.5\" for 9.5%.");
                    return;
                }

                // Compute daily balances from all selected registers
                decimal startingBalance = 0M;
                decimal[] dailyTotals = new decimal[interestDays];
                foreach (ListViewItem itm in lvwRegisters.CheckedItems)
                {
                    Register reg = (Register)itm.Tag;
                    foreach (BaseTrx baseTrx in new RegIterator<BaseTrx>(reg))
                    {
                        if (!baseTrx.IsFake && ((baseTrx is BankTrx) || (baseTrx is ReplicaTrx)))
                        {
                            if (baseTrx.TrxDate < startDate)
                            {
                                startingBalance += baseTrx.Amount;
                            }
                            else if (baseTrx.TrxDate <= endDate)
                            {
                                dailyTotals[baseTrx.TrxDate.Subtract(startDate).Days] += baseTrx.Amount;
                            }
                            else
                                break;
                        }
                    }
                }
                decimal[] dailyBalances = new decimal[interestDays];
                decimal prevBalance = startingBalance;
                decimal sumDailyBalances = 0M;
                for(int i=0; i<interestDays; i++)
                {
                    dailyBalances[i] = prevBalance + dailyTotals[i];
                    sumDailyBalances += dailyBalances[i];
                    prevBalance = dailyBalances[i];
                }

                // Calculate interest
                decimal annualRateFraction = annualRate / 100M;
                decimal avgDailyBal = Math.Round(sumDailyBalances / interestDays, 2);
                decimal totalInterest = Math.Round(calculator.Calculate(startDate, dailyBalances, annualRateFraction), 2);
                string memo = calculator.Memo(annualRateFraction, avgDailyBal);
                
                // Create bank trx in current register
                BankTrx interestTrx = new BankTrx(HostUI.GetCurrentRegister());
                DateTime dummy = DateTime.MinValue;
                string trxDescription = PersonalAcctExists ? "Interest:DIVIDE" : "Interest";
                interestTrx.NewStartNormal(true, "Inv", endDate, trxDescription, memo, BaseTrx.TrxStatus.Unreconciled,
                    false, 0M, false, false, 0, "", "");
                interestTrx.AddSplit("", interestCatKey, "", "", Utilities.EmptyDate, Utilities.EmptyDate, "", "", totalInterest);
                if (!HostUI.AddNormalTrx(interestTrx, ref dummy, false, "Calculate Interest"))
                    this.Close();
            }
            catch(Exception ex)
            {
                ErrorHandling.TopException(ex);
            }
        }

        private void cboInterestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox interestType = (ComboBox)sender;
            lblExplanation.Text = ((IInterestCalculator)(interestType.SelectedItem)).Description +
                " Only non-fake bank transactions and replica transactions are included in balance calculations.";
        }

        private void btnSearchStarting_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime? lastInterestDate = null;
                string interestKey = GetInterestCategoryKey();
                if (interestKey == null)
                {
                    HostUI.ErrorMessageBox("Please select an interest category first.");
                    return;
                }
                foreach (ListViewItem itm in lvwRegisters.CheckedItems)
                {
                    Register reg = (Register)itm.Tag;
                    foreach (BankTrx bankTrx in new RegIterator<BankTrx>(reg))
                    {
                        if (!bankTrx.IsFake)
                        {
                            foreach(TrxSplit spl in bankTrx.Splits)
                            {
                                if (spl.CategoryKey == interestKey)
                                {
                                    if (!lastInterestDate.HasValue)
                                        lastInterestDate = bankTrx.TrxDate;
                                    else if (bankTrx.TrxDate > lastInterestDate)
                                        lastInterestDate = bankTrx.TrxDate;
                                }
                            }
                        }
                    }
                }
                if (lastInterestDate.HasValue)
                {
                    HostUI.InfoMessageBox("Last bank trx with the specified interest category is dated " +
                        Utilities.FormatDate(lastInterestDate.Value) + ". Setting start date to the day after.");
                    ctlStartDate.Value = lastInterestDate.Value.Date.AddDays(1D);
                }
                else
                    HostUI.InfoMessageBox("Could not find interest trx with that category.");
            }
            catch (Exception ex)
            {
                ErrorHandling.TopException(ex);
            }
        }
    }
}
