using System;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{
    [Cmdlet(VerbsCommon.New, "CheckbookSplit")]
    [OutputType(typeof(SplitContent))]
    public class NewSplit : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public Company Company { get; set; }

        [Parameter(Mandatory = true)]
        public string Category { get; set; }

        [Parameter]
        public string InvoiceNum { get; set; }

        [Parameter]
        public string PONumber { get; set; }

        [Parameter]
        public DateTime InvoiceDate { get; set; }

        [Parameter]
        public DateTime DueDate { get; set; }

        [Parameter]
        public string Budget { get; set; }

        [Parameter]
        public string Memo { get; set; }

        [Parameter]
        public string Terms { get; set; }

        [Parameter(Mandatory = true)]
        public decimal Amount { get; set; }
            
        protected override void BeginProcessing()
        {
            int catIndex = Company.Categories.FindIndexOfValue1(Category);
            if (catIndex == 0)
                ThrowTerminatingError(ErrorUtilities.CreateInvalidOperation("Invalid category name [" + Category + "]", "CategoryNameFailure"));
            string catKey = Company.Categories.get_GetKey(catIndex);
            string budgetKey;
            if (!string.IsNullOrEmpty(Budget))
            {
                int budIndex = Company.Budgets.FindIndexOfValue1(Budget);
                if (budIndex == 0)
                    ThrowTerminatingError(ErrorUtilities.CreateInvalidOperation("Invalid budget name [" + Budget + "]", "BudgetNameFailure"));
                budgetKey = Company.Budgets.get_GetKey(budIndex);
            }
            else
                budgetKey = "";
            SplitContent split = new SplitContent
            {
                strCatKey = catKey,
                strPONumber = PONumber,
                strInvoiceNum = InvoiceNum,
                datInvoice = InvoiceDate,
                datDue = DueDate,
                curAmount = Amount,
                strBudgetKey = budgetKey,
                strMemo = Memo,
                strTerms = Terms
            };
            WriteObject(split);
        }
    }
}
