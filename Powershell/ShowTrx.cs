using System;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{
    [Cmdlet(VerbsCommon.Show, "CheckbookTrx")]
    public class ShowTrx : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Trx Input { get; set; }

        protected override void ProcessRecord()
        {
            ShortTrx shortTrx = new ShortTrx
            {
                Type = Input.GetType().Name,
                Date = Input.datDate.ToShortDateString(),
                Description = Input.strDescription,
                Category = Input.strCategory,
                Amount = Input.curAmount
            };
            WriteObject(shortTrx);
        }

        private class ShortTrx
        {
            public string Type;
            public string Date;
            public string Description;
            public string Category;
            public decimal Amount;
        }
    }
}
