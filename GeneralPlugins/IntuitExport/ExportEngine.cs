using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Willowsoft.CheckBook.Lib;
using Willowsoft.CheckBook.PluginCore;

namespace Willowsoft.CheckBook.GeneralPlugins
{
    /// <summary>
    /// Generate an Intuit IIF file containing all account definitions,
    /// payee definitions, and transaction information for all transcations
    /// in all accounts in this software between a pair of dates. Skips
    /// retained earnings accounts and personal accounts.
    /// The generated account and payee names will automatically be modified
    /// as needed to fit within the limitations of Quickbooks and avoid
    /// duplicate names during that adjustment. Mostly this means that
    /// names will be shortened, and where this would create duplicate names
    /// it replaces the end of the name with a number. The target Quickbooks
    /// database may optionally already have some account definitions you
    /// wish to use instead of creating new ones. This can be accomodated
    /// by passing appropriately populated BalanceSheetMaps and CategoryMaps
    /// objects. These "maps" are dictionaries that supply the name mapping
    /// between this software and Quickbooks.
    /// </summary>
    public class ExportEngine
    {
        private IHostUI HostUI;
        private Company Company;
        public DateTime StartDate;
        public DateTime EndDate;
        // The balance sheet map assigned by the caller.
        // ExportEngine does not change the contents of this dictionary.
        public Dictionary<string, BalanceSheetMap> BalanceSheetMaps;
        // The category map assigned by the caller. 
        // ExportEngine does not change the contents of this dictionary.
        public Dictionary<string, CategoryMap> CategoryMaps;
        // Payee (customer, vendor and other) list, keyed by normalized payee name.
        private Dictionary<string, PayeeDef> Payees;
        // Category list, keyed by category key (not name).
        // Includes balance sheet accounts.
        private Dictionary<string, CatDef> Categories;
        // Income, expense, and balance sheet accounts.
        private CategoryTranslator CatTrans;
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
            BalanceSheetMaps = null;
            CategoryMaps = null;
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

        /// <summary>
        /// Scan all transactions before generating any output. Used to assemble
        /// the list of account and payee names, and resolve things like
        /// duplicate names.
        /// </summary>
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

        /// <summary>
        /// Determine all names associated with the transaction, and add any
        /// new ones to the list that must be defined in the IIF file.
        /// Does not actually output anything to the IIF file here.
        /// </summary>
        /// <param name="trx"></param>
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
                // Create CatDef objects for balance sheet accounts as well as income/expense accounts.
                string catExportKey = GetCatExportKey(split.strCategoryKey);
                if (!Categories.TryGetValue(catExportKey, out cat))
                {
                    string catName = CatTrans.strKeyToValue1(split.strCategoryKey);
                    // objCategories includes balance sheet accounts
                    StringTransElement catElem = this.Company.objCategories.get_objElement(this.Company.objCategories.intLookupKey(split.strCategoryKey));
                    // A null intuitCatType value will cause this category to NOT be output to the IIF file.
                    // This is how we prevent categories that are actually asset, liability and equity accounts
                    // from being output to the IIF as income, expense or COGS account.
                    string intuitCatType;
                    if (split.strCategoryKey.IndexOf('.') >= 0)
                    {
                        intuitCatType = null;
                    }
                    else
                    {
                        string catType;
                        intuitCatType = null;
                        if (!catElem.colValues.TryGetValue(CategoryTranslator.strTypeKey, out catType))
                            catType = CategoryTranslator.strTypeOfficeExpense;
                        if (catType == CategoryTranslator.strTypeCOGS)
                            intuitCatType = "COGS";
                        else if (catType == CategoryTranslator.strTypeOtherIncome)
                            intuitCatType = "EXINC";
                        else if (catType == CategoryTranslator.strTypeOtherExpense)
                            intuitCatType = "EXEXP";
                        else if (catType == CategoryTranslator.strTypeTaxes)
                            intuitCatType = "EXEXP";
                        else if (catName.ToUpper().StartsWith("E:"))
                            intuitCatType = "EXP";
                        else if (catName.ToUpper().StartsWith("I:"))
                            intuitCatType = "INC";
                    }
                    cat = new CatDef(catName, MakeCatExportName(split, catName), intuitCatType);
                    Categories.Add(catExportKey, cat);
                }
            }
        }

        /// <summary>
        /// Construct the account name to write to the export file for the specified split.
        /// Split category may be an income/expense account, or a balance sheet account.
        /// The resulting name is guaranteed to not exist in this.Categories, and the result
        /// is generally added to this.Categories by the caller.
        /// This is how unique names are constructed, FROM A SPLIT, to add to this.Categories.
        /// </summary>
        /// <param name="split"></param>
        /// <param name="catName"></param>
        /// <returns></returns>
        private string MakeCatExportName(TrxSplit split, string catName)
        {
            string exportName;
            int periodIndex = split.strCategoryKey.IndexOf('.');
            if (periodIndex < 0)
            {
                exportName = GetPreExistingIntuitCatName(catName);
                if (exportName == null)
                    exportName = MakeUniqueAccountExportName(catName.Length > 2 ? catName.Substring(2) : catName);
            }
            else
            {
                int acctKey = int.Parse(split.strCategoryKey.Substring(0, periodIndex));
                Account matchingAcct = Company.colAccounts.First(acct => acct.intKey == acctKey);
                exportName = MakeBalanceSheetExportName(matchingAcct);
            }
            return exportName;
        }

        /// <summary>
        /// Similar to MakeCatExportName(), but for balance sheet accounts ONLY.
        /// </summary>
        /// <param name="acct"></param>
        /// <returns></returns>
        private string MakeBalanceSheetExportName(Account acct)
        {
            string exportName = GetPreExistingIntuitAccountName(acct);
            if (exportName == null)
                exportName = MakeUniqueAccountExportName(acct.strTitle);
            return exportName;
        }

        /// <summary>
        /// Deep down, this is how a unique export name is obtained to add to this.Categories.
        /// Always returns a name short enough to be a valid Quickbooks account name.
        /// Truncates rawInputName as necessary, and adds a sequence number as necessary, 
        /// in order to construct a valid Quickbooks name that is also unique.
        /// </summary>
        /// <param name="rawInputName">
        /// The input account name, like "Checking" or "Auto:Gas".
        /// Names from the category list must have any single letter prefix and
        /// a colon removed before passing, like "E:".
        /// </param>
        /// <returns></returns>
        private string MakeUniqueAccountExportName(string rawInputName)
        {
            int maxQBAccountNameLen = 31;
            string normalizedInputName = rawInputName.Replace(':', '-');
            string candidateName = normalizedInputName;
            if (candidateName.Length > maxQBAccountNameLen)
                candidateName = normalizedInputName.Substring(0, maxQBAccountNameLen);
            for (int suffix = 2; ; suffix++)
            {
                bool acctFound = false;
                foreach (CatDef cat in Categories.Values)
                {
                    if (cat.ExportName == candidateName)
                    {
                        acctFound = true;
                        break;
                    }
                }
                if (!acctFound)
                    return candidateName;
                candidateName = normalizedInputName;
                if (candidateName.Length > (maxQBAccountNameLen - 3))
                    candidateName = candidateName.Substring(0, (maxQBAccountNameLen - 3));
                candidateName = candidateName + suffix.ToString();
            }
        }

        private string GetCatExportKey(string catKey)
        {
            int periodIndex = catKey.IndexOf('.');
            if (periodIndex < 0)
                return catKey;
            return GetBalanceSheetExportKey(catKey.Substring(0, periodIndex));
        }

        private string GetBalanceSheetExportKey(string acctKey)
        {
            return acctKey + ".Act";
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
            int maxQBOtherNameLen = 41;
            string normalizedInputName = rawInputName.Replace(':', '-').Replace(",", "");
            string candidateName = normalizedInputName;
            if (candidateName.Length > maxQBOtherNameLen)
                candidateName = normalizedInputName.Substring(0, maxQBOtherNameLen);
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
                if (candidateName.Length > (maxQBOtherNameLen - 3))
                    candidateName = candidateName.Substring(0, (maxQBOtherNameLen - 3));
                candidateName = candidateName + suffix.ToString();
            }
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
            CatDef cat;
            string balExportKey = GetBalanceSheetExportKey(acct.intKey.ToString());
            if (!Categories.TryGetValue(balExportKey, out cat))
            {
                // We probably don't need to add this CatDef to Categories, because
                // nothing will search for at after this point in the export, but
                // we add it for consistency.
                cat = new CatDef(acct.strTitle, MakeBalanceSheetExportName(acct), null);
                Categories.Add(balExportKey, cat);
            }
            OutputLine("ACCNT\t" + cat.ExportName + "\t" + acctType);
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
            string exportAccountName = Categories[GetBalanceSheetExportKey(trx.objReg.objAccount.intKey.ToString())].ExportName;
            switch (usage)
            {
                case TrxOutputType.Check:
                    if (ContainsPayablesSplit(trx.colSplits))
                        OutputTransactionPerSplit(trx, exportAccountName, payee, "CHECK", usePayeeOnSplit: SplitIsToAccountsPayable);
                    else
                        OutputOneTransaction(trx, exportAccountName, payee, "CHECK", usePayeeOnTrx: true);
                    break;
                case TrxOutputType.Deposit:
                    OutputOneTransaction(trx, exportAccountName, payee, "DEPOSIT", usePayeeOnTrx: false);
                    break;
                case TrxOutputType.JournalEntry:
                    if (ContainsPayablesSplit(trx.colSplits))
                        OutputTransactionPerSplit(trx, exportAccountName, payee, "GENERAL JOURNAL", usePayeeOnSplit: SplitIsToAccountsPayable);
                    else
                        OutputOneTransaction(trx, exportAccountName, payee, "GENERAL JOURNAL", usePayeeOnTrx: true);
                    break;
                case TrxOutputType.Bill:
                    OutputOneTransaction(trx, exportAccountName, payee, "BILL", usePayeeOnTrx: true);
                    break;
                case TrxOutputType.BillCredit:
                    OutputOneTransaction(trx, exportAccountName, payee, "BILL REFUND", usePayeeOnTrx: true);
                    break;
            }
        }

        private void OutputOneTransaction(NormalTrx trx, string exportAccountName, PayeeDef payee, string intuitTrxType, bool usePayeeOnTrx)
        {
            OutputNormalTrx(trx, intuitTrxType, exportAccountName, usePayeeOnTrx ? payee.ExportName : "", trx.curAmount);
            foreach (TrxSplit split in trx.colSplits)
            {
                OutputSplit(split, intuitTrxType, "");
            }
            OutputLine("ENDTRNS");
        }

        private void OutputTransactionPerSplit(NormalTrx trx, string exportAccountName, PayeeDef payee, 
            string intuitTrxType, Func<TrxSplit, bool> usePayeeOnSplit)
        {
            foreach (TrxSplit split in trx.colSplits)
            {
                // Notice we pass the split amount instead of the trx amount, because every split
                // of this transaction type must be a separate transaction in an IIF file.
                OutputNormalTrx(trx, intuitTrxType, exportAccountName, payee.ExportName, split.curAmount);
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

        private void OutputNormalTrx(NormalTrx trx, string transType, string exportAccountName, string payeeName, decimal amount)
        {
            OutputLine("TRNS\t\t" + transType + "\t" + trx.datDate.ToString("MM/dd/yyyy") + "\t" + exportAccountName +
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
            // Null for balance sheet accounts
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
