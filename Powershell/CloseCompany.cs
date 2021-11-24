using System;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{
    [Cmdlet(VerbsCommon.Close, "CheckbookCompany")]
    public class CloseCompany : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public Company Company { get; set; }

        protected override void BeginProcessing()
        {
            CompanySaver.Unload(Company);
        }
    }
}
