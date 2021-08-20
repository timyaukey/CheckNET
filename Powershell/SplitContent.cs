using System;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{
    public class SplitContent
    {
        public string strCatKey;
        public string strInvoiceNum;
        public string strPONumber;
        public DateTime datInvoice;
        public DateTime datDue;
        public string strTerms;
        public string strMemo;
        public string strBudgetKey;
        public decimal curAmount;

        public void AddToNormalTrx(BankTrx trx)
        {
            trx.AddSplit(
                strCategoryKey_: strCatKey, 
                strPONumber_: strPONumber ?? "", 
                strInvoiceNum_: strInvoiceNum ?? "",
                datInvoiceDate_: (datInvoice <= Utilities.EmptyDate) ? Utilities.EmptyDate : datInvoice,
                datDueDate_: (datDue <= Utilities.EmptyDate) ? Utilities.EmptyDate : datDue,
                strTerms_: strTerms ?? "", 
                strBudgetKey_: strBudgetKey ?? "", 
                strMemo_: strMemo ?? "",
                curAmount_: curAmount);
        }
    }
}
