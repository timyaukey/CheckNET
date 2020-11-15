using System;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{
    [Cmdlet(VerbsCommon.Remove, "CheckbookTrx")]
    public class RemoveTrx : Cmdlet
    {
        [Parameter(Mandatory = true, ValueFromPipeline = true)]
        public Trx Input { get; set; }

        protected override void ProcessRecord()
        {
            Input.Delete(new LogDelete(), "PowershellDeleteTrx");
        }
    }
}
