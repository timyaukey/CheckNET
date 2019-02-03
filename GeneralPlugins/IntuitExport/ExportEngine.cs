﻿using System;
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
        public Dictionary<string, BalanceSheetMap> BalanceSheetTranslator;
        public Dictionary<string, CategoryMap> CategoryTranslator;
        private Dictionary<string, PayeeDef> Payees;
        private Dictionary<string, CatDef> Categories;
        private string OutputPath_;
        private System.IO.TextWriter OutputWriter;

        public ExportEngine(IHostUI hostUI)
        {
            HostUI = hostUI;
            Company = HostUI.objCompany;
            BalanceSheetTranslator = new Dictionary<string, BalanceSheetMap>();
            CategoryTranslator = new Dictionary<string, CategoryMap>();
            Payees = new Dictionary<string, PayeeDef>();
            Categories = new Dictionary<string, CatDef>();
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
            foreach (Account acct in Company.colAccounts)
            {
                if (acct.lngType != Account.AccountType.Personal)
                {
                    foreach (Register reg in acct.colRegisters)
                    {
                        foreach (Trx trx in reg.colDateRange(StartDate, EndDate))
                        {
                            NormalTrx normalTrx = trx as NormalTrx;
                            if (normalTrx != null && !normalTrx.blnFake)
                                AnalyzeNormalTrx(normalTrx);
                        }
                    }
                }
            }
        }

        private void AnalyzeNormalTrx(NormalTrx trx)
        {
            CategoryTranslator catTrans = HostUI.objCompany.objCategories;
            PayeeDef payee;
            if (!Payees.TryGetValue(trx.strDescription, out payee))
            {
                payee = new PayeeDef(trx.strDescription, GetValidPayeeExportBase(trx.strDescription));
                Payees.Add(trx.strDescription, payee);
            }
            switch (GetPayeeUsage(trx))
            {
                case PayeeUsage.Expense:
                    payee.PaymentCount++;
                    break;
                case PayeeUsage.Income:
                    payee.DepositCount++;
                    break;
                case PayeeUsage.Other:
                    payee.OtherCount++;
                    break;
            }
            foreach (TrxSplit split in trx.colSplits)
            {
                CatDef cat;
                if (!Categories.TryGetValue(split.strCategoryKey, out cat))
                {
                    string catName = catTrans.strKeyToValue1(split.strCategoryKey);
                    string exportName = GetBuiltinIntuitCatName(catName);
                    if (exportName == null)
                    {
                        exportName = GetValidCategoryExport(catName);
                    }
                    cat = new CatDef(split.strCategoryKey, catName, exportName);
                    Categories.Add(cat.Key, cat);
                }
            }
        }

        private string GetValidPayeeExportBase(string rawInputName)
        {
            int maxLength = 40;
            string normalizedInputName = rawInputName.Replace(':', '-');
            string candidateName = normalizedInputName;
            if (candidateName.Length > maxLength)
                candidateName = normalizedInputName.Substring(0, maxLength);
            for (int suffix = 2; ; suffix++)
            {
                bool payeeFound = false;
                foreach (PayeeDef payee in Payees.Values)
                {
                    if (payee.ExportBase == candidateName)
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

        private string GetValidCategoryExport(string rawInputName)
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
            if (GetBuiltinIntuitAccountName(acct) != null)
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
            OutputLine("ACCNT\t" + acct.strTitle + "\t" + acctType);
        }

        private void OutputCategories()
        {
            OutputLine("!ACCNT\tNAME\tACCNTTYPE");
            foreach (CatDef cat in Categories.Values)
            {
                if (GetBuiltinIntuitCatName(cat.Name) != null)
                    continue;
                if (cat.IsIncome)
                    OutputCategory(cat.ExportName, "INC");
                if (cat.IsExpense)
                    OutputCategory(cat.ExportName, "EXP");
            }
        }

        private void OutputCategory(string name, string catType)
        {
            OutputLine("ACCNT\t" + name + "\t" + catType);
        }

        private string GetBuiltinIntuitAccountName(Account acct)
        {
            if (BalanceSheetTranslator.TryGetValue(acct.strFileNameRoot, out BalanceSheetMap balMap))
            {
                return balMap.IntuitName;
            }
            return null;
        }

        private string GetBuiltinIntuitCatName(string catName)
        {
            if (CategoryTranslator.TryGetValue(catName, out CategoryMap catMap))
            {
                return catMap.IntuitName;
            }
            return null;
        }

        private void OutputPayees()
        {
            foreach (PayeeDef payee in Payees.Values)
            {
                OutputPayee(payee);
            }
        }

        private void OutputPayee(PayeeDef payee)
        {
            if (payee.PaymentCount > 0)
                OutputPayee(payee.ExportName(PayeeUsage.Expense), "VEND");
            if (payee.DepositCount > 0)
                OutputPayee(payee.ExportName(PayeeUsage.Income), "CUST");
            if (payee.OtherCount > 0)
                OutputPayee(payee.ExportName(PayeeUsage.Other), "OTHERNAME");
        }

        private void OutputPayee(string payeeName, string tag)
        {
            OutputLine("!" + tag + "\tNAME");
            OutputLine(tag + "\t" + payeeName);
        }

        private void OutputTransactions()
        {
            OutputLine("!TRNS\tTRNSID\tTRNSTYPE\tDATE\tACCNT\tNAME\tAMOUNT\tDOCNUM\tMEMO\tCLEAR\tTOPRINT");
            OutputLine("!SPL\tSPLID\tTRNSTYPE\tDATE\tACCNT\tNAME\tAMOUNT\tDOCNUM\tMEMO\tCLEAR");
            OutputLine("!ENDTRNS");

            foreach (Account acct in Company.colAccounts)
            {
                if (acct.lngType != Account.AccountType.Personal)
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
            PayeeDef payee = Payees[trx.strDescription];
            PayeeUsage usage = GetPayeeUsage(trx);
            string payeeName = payee.ExportName(usage);
            string registerAccountName = GetBuiltinIntuitAccountName(trx.objReg.objAccount);
            if (registerAccountName == null)
                registerAccountName = trx.objReg.objAccount.strTitle;
            switch (usage)
            {
                case PayeeUsage.Expense:
                    OutputNormalTrx(trx, "CHECK", registerAccountName, payeeName);
                    foreach (TrxSplit split in trx.colSplits)
                        OutputSplit(split, "CHECK");
                    OutputLine("ENDTRNS");
                    break;
                case PayeeUsage.Income:
                    OutputNormalTrx(trx, "DEPOSIT", registerAccountName, "");
                    foreach (TrxSplit split in trx.colSplits)
                        OutputSplit(split, "DEPOSIT");
                    OutputLine("ENDTRNS");
                    break;
                case PayeeUsage.Other:
                    OutputNormalTrx(trx, "GENERAL JOURNAL", registerAccountName, payeeName);
                    foreach (TrxSplit split in trx.colSplits)
                        OutputSplit(split, "GENERAL JOURNAL");
                    OutputLine("ENDTRNS");
                    break;
            }
        }

        private void OutputNormalTrx(NormalTrx trx, string transType, string registerAccountName, string payeeName)
        {
            OutputLine("TRNS\t\t" + transType + "\t" + trx.datDate.ToString("MM/dd/yyyy") + "\t" + registerAccountName +
                "\t" + payeeName + "\t" + trx.curAmount.ToString("##############0.00") +
                "\t" + trx.strNumber + "\t" + trx.strMemo + "\tY\tN");
        }

        private void OutputSplit(TrxSplit split, string transType)
        {
            CatDef cat = Categories[split.strCategoryKey];
            OutputLine("SPL\t\t" + transType + "\t" + split.objParent.datDate.ToString("MM/dd/yyyy") +
                "\t" + cat.ExportName + "\t\t" + (-split.curAmount).ToString("##############0.00") +
                "\t\t" + split.strMemo + "\tY");
        }

        private PayeeUsage GetPayeeUsage(NormalTrx trx)
        {
            if (trx.objReg.objAccount.lngSubType == Account.SubType.Asset_CheckingAccount ||
                trx.objReg.objAccount.lngSubType == Account.SubType.Asset_SavingsAccount)
            {
                int incCount = 0;
                int expCount = 0;
                foreach (TrxSplit split in trx.colSplits)
                {
                    if (split.curAmount < 0)
                        expCount++;
                    else if (split.curAmount > 0)
                        incCount++;
                }
                if (expCount > 0 && incCount == 0)
                    return PayeeUsage.Expense;
                else if (expCount == 0 && incCount > 0)
                    return PayeeUsage.Income;
            }
            // TO DO: Check account and splits
            // Payment: Bank account, all splits expenses
            // Deposit: Bank account, all splits income
            // Other: Everything else
            return PayeeUsage.Other;
        }

        private void OutputLine(string line)
        {
            OutputWriter.WriteLine(line);
        }

        private class PayeeDef
        {
            public string Name;
            public string ExportBase;
            public int PaymentCount = 0;
            public int DepositCount = 0;
            public int OtherCount = 0;

            public PayeeDef(string name, string exportBase)
            {
                Name = name;
                ExportBase = exportBase;
            }

            public string ExportName(PayeeUsage usage)
            {
                switch (usage)
                {
                    case PayeeUsage.Expense:
                        return ExportBase;
                    case PayeeUsage.Income:
                        return ExportBase + "(I)";
                    case PayeeUsage.Other:
                        return ExportBase + "(O)";
                    default:
                        throw new ArgumentOutOfRangeException("Invalid PayeeUsage");
                }
            }
        }

        private class CatDef
        {
            public string Key;
            public string Name;
            public string ExportName;

            public CatDef(string key, string name, string exportName)
            {
                Key = key;
                Name = name;
                ExportName = exportName;
            }

            public bool IsIncome
            {
                get { return Name.StartsWith("I:"); }
            }

            public bool IsExpense
            {
                get { return Name.StartsWith("E:"); }
            }
        }

        private enum PayeeUsage
        {
            Expense = 1,
            Income = 2,
            Other = 3
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
