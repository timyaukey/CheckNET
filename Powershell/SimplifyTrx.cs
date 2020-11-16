using System;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{
    [Cmdlet(VerbsCommon.Get, "CheckbookSimpleTrx")]
    public class SimplifyTrx : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Trx Input { get; set; }

        protected override void ProcessRecord()
        {
            ShortTrx shortTrx = new ShortTrx
            {
                Type = Input.GetType().Name,
                Date = Input.datDate,
                Description = Input.strDescription,
                Category = Input.strCategory,
                Amount = Input.curAmount
            };
            WriteObject(shortTrx);
        }

        private class ShortTrx
        {
            public string Type;
            public DateTime Date;
            public string Description;
            public string Category;
            public decimal Amount;
        }
    }
}
