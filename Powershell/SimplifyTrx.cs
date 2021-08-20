using System;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{
    [Cmdlet(VerbsCommon.Get, "CheckbookSimpleTrx")]
    public class SimplifyTrx : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public BaseTrx Input { get; set; }

        protected override void ProcessRecord()
        {
            ShortTrx shortTrx = new ShortTrx
            {
                Type = Input.GetType().Name,
                Date = Utilities.FormatDate(Input.TrxDate),
                Number = Input.Number,
                Description = Input.Description,
                Category = Input.CategoryLabel,
                Amount = Input.Amount
            };
            WriteObject(shortTrx);
        }

        private class ShortTrx
        {
            public string Type;
            public string Date;
            public string Number;
            public string Description;
            public string Category;
            public decimal Amount;
        }
    }
}
