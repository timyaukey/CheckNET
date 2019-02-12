using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CheckBookLib;

namespace GeneralPlugins.IntuitExport
{
    public class ExportEngine
    {
        private IHostUI HostUI;
        private Company Company;
        public DateTime StartDate;
        public DateTime EndDate;
        public Dictionary<string, BalanceSheetMap> BalanceSheetMaps;
        public Dictionary<string, CategoryMap> CategoryMaps;
        private Dictionary<string, PayeeDef> Payees;
        private Dictionary<string, CatDef> Categories;
        CategoryTranslator CatTrans;
        private string OutputPath_;
        private System.IO.TextWriter OutputWriter;

        /*
         * QuickBooks import (IIF) validation notes:
         * A "GENERAL JOURNAL" entry allows all accounts to be used for debits or credits. Total flexibility.
         * A "CHECK" entry must have a debit net amount.
         * A "CHECK" may use a vendor name or other name. Both must obey the debit amount rule.
         * The QuickBooks UI allows "CHECK" with credit amount to be entered directly into register, but it silently
         *   converts that transaction to a "DEPOSIT". The IIF import does not silently convert like this.
         * A "CHECK" entry can be used for ANY debit use in an asset account, for example an ACH transfer or debit card use.
         * A "CHECK" may have a split with a credit amount, so long as the net total of the transaction is a debit.
         * A "CHECK" entry can use Accounts Payable for a split account.
         * A transaction in an A/P account should be "BILL" for a net debit amount, and "BILL REFUND" for a net credit amount.
         * A transaction in an A/P account requires a non-blank vendor name for the "NAME" field.
         * A transaction in an A/P account may have multiple splits, and all of them should have a blank "NAME" field.
         * A transaction in a non-A/P account with a split pointing an A/P account requires a non-blank vendor name for the "NAME" field.
         * A transaction in a non-A/P account may have only one split pointing to an A/P account.
         * A "DEPOSIT" entry must have a credit net amount.
         * A "DEPOSIT" entry may use a vendor name or other name, but the name will be blank in the transaction created.
         * A "DEPOSIT" entry can leave the name field blank.
         */

        public ExportEngine(IHostUI hostUI)
        {
            HostUI = hostUI;
            Company = HostUI.objCompany;
            BalanceSheetMaps = new Dictionary<string, BalanceSheetMap>();
            CategoryMaps = new Dictionary<string, CategoryMap>();
            Payees = new Dictionary<string, PayeeDef>();
            Categories = new Dictionary<string, CatDef>();
            CatTrans = HostUI.objCompany.objCategories;
        }

        public void Run()
        {
            AnalyzeTransactions();
            OutputPath_ = System.IO.Path.Combine(Company.strReportPath(), "IntuitExport.iif");
            using (OutputWriter = new System.IO.StreamWriter(OutputPath_))
            {
                OutputAccounts();
                OutputCategories();
                OutputPayees();
                OutputTransactions();
            }
        }

        public string OutputPath
        {
            get { return OutputPath_; }
        }

        private void AnalyzeTransactions()
        {
            // We collect all trx and sort them in date order before analyzing
            // so that trx payee names and category names are given consistent
            // export names each time we run the software with later and later
            // ending dates. The idea is that the export names are assigned the
            // the first time that name is encountered in date order, and will
            // not be different for different runs of the software unless old
            // transactions are changed (which should never happen).
            List<NormalTrx> normalTrxToAnalyze = new List<NormalTrx>();
            foreach (Account acct in Company.colAccounts)
            {
                if (!SkipAccount(acct))
                {
                    foreach (Register reg in acct.colRegisters)
                    {
                        foreach (Trx trx in reg.colAllTrx())
                        {
                            if (trx.datDate > EndDate)
                                break;
                            NormalTrx normalTrx = trx as NormalTrx;
                            if (normalTrx != null && !normalTrx.blnFake)
                                normalTrxToAnalyze.Add(normalTrx);
                        }
                    }
                }
            }
            // The important part of this sort order is the date - it must be increasing.
            // The only purpose of the rest is to make the order consistent from run to run.
            normalTrxToAnalyze.Sort(
                delegate (NormalTrx trx1, NormalTrx trx2)
                {
                    int result = trx1.datDate.CompareTo(trx2.datDate);
                    if (result != 0)
                        return result;
                    result = trx1.strNumber.CompareTo(trx2.strNumber);
                    if (result != 0)
                        return result;
                    result = trx1.strDescription.CompareTo(trx2.strDescription);
                    if (result != 0)
                        return result;
                    return trx1.curAmount.CompareTo(trx2.curAmount);
                }
                );
            foreach(NormalTrx trx in normalTrxToAnalyze)
            {
                AnalyzeNormalTrx(trx);
            }
        }

        private bool SkipAccount(Account acct)
        {
            // We can't export retained earnings because QuickBooks creates entries in
            // that account from the balances in the income and expense accounts,
            // and there is no way to edit or delete those transactions. Strangely,
            // it is possible to import transactions into that account, but those
            // cannot be edited either. So we need to let QuickBooks manufacture
            // everything in that account instead of importing it.
            return (acct.lngType == Account.AccountType.Personal) ||
                (acct.lngSubType == Account.SubType.Equity_RetainedEarnings);
        }

        private void AnalyzeNormalTrx(NormalTrx trx)
        {
            PayeeDef payee;
            string trimmedPayee = TrimPayeeName(trx.strDescription);
            string normalizedPayee = trimmedPayee.ToLower();
            if (!Payees.TryGetValue(normalizedPayee, out payee))
            {
                payee = new PayeeDef(trimmedPayee, MakeUniquePayeeExportName(trimmedPayee));
                Payees.Add(normalizedPayee, payee);
            }
            if (trx.datDate >= StartDate)
            {
                switch (GetPayeeUsage(trx))
                {
                    case TrxOutputType.JournalEntry:
                        payee.UsedForGeneralJournal = true;
                        break;
                    case TrxOutputType.Check:
                        payee.UsedForCheck = true;
                        break;
                }
            }
            foreach (TrxSplit split in trx.colSplits)
            {
                CatDef cat;
                string catExportKey = GetCatExportKey(split.strCategoryKey);
                if (!Categories.TryGetValue(catExportKey, out cat))
                {
                    string catName = CatTrans.strKeyToValue1(split.strCategoryKey);
                    StringTransElement catElem = this.Company.objCategories.get_objElement(this.Company.objCategories.intLookupKey(split.strCategoryKey));
                    string catType;
                    if (!catElem.colValues.TryGetValue(CategoryTranslator.strTypeKey, out catType))
                        catType = CategoryTranslator.strTypeOfficeExpense;
                    // A null intuitCatType value will cause this category to NOT be output to the IIF file.
                    // This is how we prevent categories that are actually asset, liability and equity accounts
                    // from being output to the IIF as income, expense or COGS account.
                    string intuitCatType = null;
                    if (catType == CategoryTranslator.strTypeCOGS)
                        intuitCatType = "COGS";
                    else if (catName.ToUpper().StartsWith("E:"))
                        intuitCatType = "EXP";
                    else if (catName.ToUpper().StartsWith("I:"))
                        intuitCatType = "INC";
                    cat = new CatDef(catName, MakeCatExportName(split, catName), intuitCatType);
                    Categories.Add(catExportKey, cat);
                }
            }
        }

        private string MakeCatExportName(TrxSplit split, string catName)
        {
            string exportName;
            int periodIndex = split.strCategoryKey.IndexOf('.');
            if (periodIndex < 0)
            {
                exportName = GetPreExistingIntuitCatName(catName);
                if (exportName == null)
                    exportName = MakeUniqueCategoryExportName(catName);
            }
            else
            {
                int acctKey = int.Parse(split.strCategoryKey.Substring(0, periodIndex));
                Account matchingAcct = Company.colAccounts.First(acct => acct.intKey == acctKey);
                exportName = GetPreExistingIntuitAccountName(matchingAcct);
                if (exportName == null)
                    exportName = MakeValidExportAccountName(matchingAcct);
            }
            return exportName;
        }

        private string MakeUniqueCategoryExportName(string rawInputName)
        {
            int maxLength = 40;
            string normalizedInputName = rawInputName.Replace(':', '-');
            if (normalizedInputName.Length > 2)
                normalizedInputName = normalizedInputName.Substring(2);
            string candidateName = normalizedInputName;
            if (candidateName.Length > maxLength)
                candidateName = normalizedInputName.Substring(0, maxLength);
            for (int suffix = 2; ; suffix++)
            {
                bool catFound = false;
                foreach (CatDef cat in Categories.Values)
                {
                    if (cat.ExportName == candidateName)
                    {
                        catFound = true;
                        break;
                    }
                }
                if (!catFound)
                    return candidateName;
                candidateName = normalizedInputName;
                if (candidateName.Length > (maxLength - 3))
                    candidateName = candidateName.Substring(0, (maxLength - 3));
                candidateName = candidateName + suffix.ToString();
            }
        }

        private string GetCatExportKey(string catKey)
        {
            int periodIndex = catKey.IndexOf('.');
            if (periodIndex < 0)
                return catKey;
            return catKey.Substring(0, periodIndex + 1) + "Act";
        }

        private string TrimPayeeName(string originalName)
        {
            // Remove everything inside paired parentheses, and trim leading and trailing whitespace.
            string result = originalName;
            for (; ; )
            {
                if (result.Length <= 15)
                    break;
                int startIndex = result.IndexOf('(', 15);
                if (startIndex < 0)
                    break;
                int endIndex = result.IndexOf(')', startIndex);
                if (endIndex < 0)
                    break;
                if (endIndex == result.Length - 1)
                    result = result.Substring(0, startIndex);
                else
                    result = result.Substring(0, startIndex) + result.Substring(endIndex + 1);
            }
            return result.Trim();
        }

        private string MakeUniquePayeeExportName(string rawInputName)
        {
            int maxLength = 40;
            string normalizedInputName = rawInputName.Replace(':', '-').Replace(",", "");
            string candidateName = normalizedInputName;
            if (candidateName.Length > maxLength)
                candidateName = normalizedInputName.Substring(0, maxLength);
            for (int suffix = 2; ; suffix++)
            {
                bool payeeFound = false;
                foreach (PayeeDef payee in Payees.Values)
                {
                    if (payee.ExportName == candidateName)
                    {
                        payeeFound = true;
                        break;
                    }
                }
                if (!payeeFound)
                    return candidateName;
                candidateName = normalizedInputName;
                if (candidateName.Length > (maxLength - 3))
                    candidateName = candidateName.Substring(0, (maxLength - 3));
                candidateName = candidateName + suffix.ToString();
            }
        }

        private string MakeValidExportAccountName(Account acct)
        {
            return acct.strTitle;
        }

        private void OutputAccounts()
        {
            bool retEarningsCreated = false;
            OutputLine("!ACCNT\tNAME\tACCNTTYPE\tEXTRA");
            foreach (Account acct in Company.colAccounts)
            {
                OutputAccount(acct, ref retEarningsCreated);
            }
        }

        private void OutputAccount(Account acct, ref bool retEarningsCreated)
        {
            if (acct.lngType == Account.AccountType.Personal)
                return;
            if (GetPreExistingIntuitAccountName(acct) != null)
                return;
            switch (acct.lngSubType)
            {
                case Account.SubType.Asset_AccountsReceivable:
                    OutputAccount(acct, "AR");
                    break;
                case Account.SubType.Asset_CheckingAccount:
                    OutputAccount(acct, "BANK");
                    break;
                case Account.SubType.Asset_Inventory:
                    OutputAccount(acct, "OCASSET");
                    break;
                case Account.SubType.Asset_Investment:
                    OutputAccount(acct, "OASSET");
                    break;
                case Account.SubType.Asset_LoanReceivable:
                    OutputAccount(acct, "OASSET");
                    break;
                case Account.SubType.Asset_Other:
                    OutputAccount(acct, "OASSET");
                    break;
                case Account.SubType.Asset_OtherProperty:
                    OutputAccount(acct, "FIXASSET");
                    break;
                case Account.SubType.Asset_RealProperty:
                    OutputAccount(acct, "FIXASSET");
                    break;
                case Account.SubType.Asset_SavingsAccount:
                    OutputAccount(acct, "BANK");
                    break;
                case Account.SubType.Equity_Capital:
                    OutputAccount(acct, "EQUITY");
                    break;
                // I don't think this is actually used, unless we decide to export retained earnings.
                case Account.SubType.Equity_RetainedEarnings:
                    string retEarningsExtra = "";
                    if (!retEarningsCreated)
                    {
                        retEarningsExtra = "RETEARNINGS";
                        retEarningsCreated = true;
                    }
                    OutputAccount(acct, "EQUITY", extra: retEarningsExtra);
                    break;
                case Account.SubType.Equity_Stock:
                    OutputAccount(acct, "EQUITY");
                    break;
                case Account.SubType.Liability_AccountsPayable:
                    OutputAccount(acct, "AP");
                    break;
                case Account.SubType.Liability_LoanPayable:
                    OutputAccount(acct, "LTLIAB");
                    break;
                case Account.SubType.Liability_Other:
                    OutputAccount(acct, "OCLIAB");
                    break;
                case Account.SubType.Liability_Taxes:
                    OutputAccount(acct, "OCLIAB");
                    break;
                default:
                    throw new Exception("Invalid account type");
            }
        }

        private void OutputAccount(Account acct, string acctType, string extra = "")
        {
            OutputLine("ACCNT\t" + MakeValidExportAccountName(acct) + "\t" + acctType);
        }

        private void OutputCategories()
        {
            OutputLine("!ACCNT\tNAME\tACCNTTYPE");
            foreach (CatDef cat in Categories.Values)
            {
                if (GetPreExistingIntuitCatName(cat.Name) != null)
                    continue;
                // Will be null for categories that point to asset, liability or equity accounts.
                if (!string.IsNullOrEmpty(cat.IntuitCatType))
                    OutputLine("ACCNT\t" + cat.ExportName + "\t" + cat.IntuitCatType);
            }
        }

        private string GetPreExistingIntuitAccountName(Account acct)
        {
            if (BalanceSheetMaps.TryGetValue(acct.strFileNameRoot, out BalanceSheetMap balMap))
            {
                return balMap.IntuitName;
            }
            return null;
        }

        private string GetPreExistingIntuitCatName(string catName)
        {
            if (CategoryMaps.TryGetValue(catName, out CategoryMap catMap))
            {
                return catMap.IntuitName;
            }
            return null;
        }

        private void OutputPayees()
        {
            foreach (PayeeDef payee in Payees.Values)
            {
                if (payee.UsedForCheck || payee.UsedForGeneralJournal)
                {
                    OutputLine("!VEND\tNAME");
                    OutputLine("VEND\t" + payee.ExportName);
                }
            }
        }

        private void OutputTransactions()
        {
            OutputLine("!TRNS\tTRNSID\tTRNSTYPE\tDATE\tACCNT\tNAME\tAMOUNT\tDOCNUM\tMEMO\tCLEAR\tTOPRINT");
            OutputLine("!SPL\tSPLID\tTRNSTYPE\tDATE\tACCNT\tNAME\tAMOUNT\tDOCNUM\tMEMO\tCLEAR");
            OutputLine("!ENDTRNS");

            foreach (Account acct in Company.colAccounts)
            {
                if (!SkipAccount(acct))
                {
                    foreach (Register reg in acct.colRegisters)
                    {
                        foreach (Trx trx in reg.colDateRange(StartDate, EndDate))
                        {
                            NormalTrx normalTrx = trx as NormalTrx;
                            if (normalTrx != null && !normalTrx.blnFake)
                                OutputNormalTrx(normalTrx);
                        }
                    }
                }
            }
        }

        private void OutputNormalTrx(NormalTrx trx)
        {
            PayeeDef payee = Payees[TrimPayeeName(trx.strDescription).ToLower()];
            TrxOutputType usage = GetPayeeUsage(trx);
            string registerAccountName = GetPreExistingIntuitAccountName(trx.objReg.objAccount);
            if (registerAccountName == null)
                registerAccountName = trx.objReg.objAccount.strTitle;
            switch (usage)
            {
                case TrxOutputType.Check:
                    if (ContainsPayablesSplit(trx.colSplits))
                        OutputTransactionPerSplit(trx, registerAccountName, payee, "CHECK", usePayeeOnSplit: SplitIsToAccountsPayable);
                    else
                        OutputOneTransaction(trx, registerAccountName, payee, "CHECK", usePayeeOnTrx: true);
                    break;
                case TrxOutputType.Deposit:
                    OutputOneTransaction(trx, registerAccountName, payee, "DEPOSIT", usePayeeOnTrx: false);
                    break;
                case TrxOutputType.JournalEntry:
                    if (ContainsPayablesSplit(trx.colSplits))
                        OutputTransactionPerSplit(trx, registerAccountName, payee, "GENERAL JOURNAL", usePayeeOnSplit: SplitIsToAccountsPayable);
                    else
                        OutputOneTransaction(trx, registerAccountName, payee, "GENERAL JOURNAL", usePayeeOnTrx: true);
                    break;
                case TrxOutputType.Bill:
                    OutputOneTransaction(trx, registerAccountName, payee, "BILL", usePayeeOnTrx: true);
                    break;
                case TrxOutputType.BillCredit:
                    OutputOneTransaction(trx, registerAccountName, payee, "BILL REFUND", usePayeeOnTrx: true);
                    break;
            }
        }

        private void OutputOneTransaction(NormalTrx trx, string registerAccountName, PayeeDef payee, string intuitTrxType, bool usePayeeOnTrx)
        {
            OutputNormalTrx(trx, intuitTrxType, registerAccountName, usePayeeOnTrx ? payee.ExportName : "", trx.curAmount);
            foreach (TrxSplit split in trx.colSplits)
            {
                OutputSplit(split, intuitTrxType, "");
            }
            OutputLine("ENDTRNS");
        }

        private void OutputTransactionPerSplit(NormalTrx trx, string registerAccountName, PayeeDef payee, 
            string intuitTrxType, Func<TrxSplit, bool> usePayeeOnSplit)
        {
            foreach (TrxSplit split in trx.colSplits)
            {
                // Notice we pass the split amount instead of the trx amount, because every split
                // of this transaction type must be a separate transaction in an IIF file.
                OutputNormalTrx(trx, intuitTrxType, registerAccountName, payee.ExportName, split.curAmount);
                OutputSplit(split, intuitTrxType, usePayeeOnSplit(split) ? payee.ExportName : "");
                OutputLine("ENDTRNS");
            }
        }

        private bool ContainsPayablesSplit(IEnumerable<TrxSplit> splits)
        {
            return splits.Any(split => SplitIsToAccountsPayable(split));
        }

        private bool SplitIsToAccountsPayable(TrxSplit split)
        {
            int dotIndex = split.strCategoryKey.IndexOf('.');
            if (dotIndex < 0)
                return false;
            int acctKey = int.Parse(split.strCategoryKey.Substring(0, dotIndex));
            return Company.colAccounts.First(acct => acct.intKey == acctKey).lngSubType == Account.SubType.Liability_AccountsPayable;
        }

        private void OutputNormalTrx(NormalTrx trx, string transType, string registerAccountName, string payeeName, decimal amount)
        {
            OutputLine("TRNS\t\t" + transType + "\t" + trx.datDate.ToString("MM/dd/yyyy") + "\t" + registerAccountName +
                "\t" + payeeName + "\t" + amount.ToString("##############0.00") +
                "\t" + trx.strNumber + "\t" + trx.strMemo + "\tY\tN");
        }

        private void OutputSplit(TrxSplit split, string transType, string payeeName)
        {
            CatDef cat = Categories[GetCatExportKey(split.strCategoryKey)];
            OutputLine("SPL\t\t" + transType + "\t" + split.objParent.datDate.ToString("MM/dd/yyyy") +
                "\t" + cat.ExportName + "\t" + payeeName + "\t" + (-split.curAmount).ToString("##############0.00") +
                "\t\t" + split.strMemo + "\tY");
        }

        private TrxOutputType GetPayeeUsage(NormalTrx trx)
        {
            Account.SubType subType = trx.objReg.objAccount.lngSubType;
            if (subType == Account.SubType.Asset_CheckingAccount ||
                subType == Account.SubType.Asset_SavingsAccount)
            {
                if (trx.curAmount < 0)
                    return TrxOutputType.Check;
                else if (trx.curAmount > 0)
                {
                    if (ContainsPayablesSplit(trx.colSplits))
                        return TrxOutputType.JournalEntry;
                    else
                        return TrxOutputType.Deposit;
                }
            }
            else if (subType == Account.SubType.Liability_AccountsPayable)
            {
                if (trx.curAmount < 0)
                    return TrxOutputType.Bill;
                else
                    return TrxOutputType.BillCredit;
            }
            return TrxOutputType.JournalEntry;
        }

        private void OutputLine(string line)
        {
            OutputWriter.WriteLine(line);
        }

        private class PayeeDef
        {
            public string TrimmedName;
            public string ExportName;
            public bool UsedForCheck;
            public bool UsedForGeneralJournal;

            public PayeeDef(string trimmedName, string exportName)
            {
                TrimmedName = trimmedName;
                ExportName = exportName;
            }
        }

        private class CatDef
        {
            public string Name;
            public string ExportName;
            public string IntuitCatType;

            public CatDef(string name, string exportName, string intuitCatType)
            {
                Name = name;
                ExportName = exportName;
                IntuitCatType = intuitCatType;
            }
        }

        private enum TrxOutputType
        {
            Check = 1,
            Deposit = 2,
            JournalEntry = 3,
            Bill = 4,
            BillCredit = 5
        }

        public class AccountMap
        {
            public string LocalName;
            public string IntuitName;
        }

        public class BalanceSheetMap : AccountMap
        {
        }

        public class CategoryMap : AccountMap
        {
        }
    }
}
