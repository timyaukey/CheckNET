using System;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{
    [Cmdlet(VerbsData.Save, "CheckbookCompany")]
    public class SaveCompany : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public Company Company { get; set; }

        protected override void BeginProcessing()
        {
            CompanySaver.SaveChangedAccounts(Company);
        }
    }
}
