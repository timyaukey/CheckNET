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

        [Parameter()]
        public string InvoiceNum { get; set; }

        [Parameter()]
        public string PONumber { get; set; }

        [Parameter()]
        public DateTime InvoiceDate { get; set; }

        [Parameter()]
        public DateTime DueDate { get; set; }

        [Parameter()]
        public string BudgetName { get; set; }

        [Parameter()]
        public string Memo { get; set; }

        [Parameter()]
        public string Terms { get; set; }

        [Parameter(Mandatory = true)]
        public decimal Amount { get; set; }
            
        protected override void BeginProcessing()
        {
            // TO DO: Handle missing args and default values correctly
            SplitContent split = new SplitContent
            {
                strCatKey = Company.objCategories.strKeyToValue1(Category),
                strPONumber = PONumber,
                strInvoiceNum = InvoiceNum,
                datInvoice = InvoiceDate,
                datDue = DueDate,
                curAmount = Amount,
                strBudgetKey=Company.objBudgets.strKeyToValue1(BudgetName), 
                strMemo=Memo, 
                strTerms=Terms
            };
            if (string.IsNullOrEmpty(split.strCatKey))
            {
                WriteError(
                    new ErrorRecord(
                        new InvalidOperationException("Invalid category name"),
                        "CategoryNameFailure",
                        ErrorCategory.InvalidOperation,
                        null)
                    );
                return;
            }
            if (!string.IsNullOrEmpty(BudgetName) & string.IsNullOrEmpty(split.strBudgetKey))
            {
                WriteError(
                    new ErrorRecord(
                        new InvalidOperationException("Invalid budget name"),
                        "BudgetNameFailure",
                        ErrorCategory.InvalidOperation,
                        null)
                    );
                return;
            }
            WriteObject(split);
        }
    }
}
