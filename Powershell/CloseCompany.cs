using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;
using Willowsoft.CheckBook.PersistTools;

namespace Willowsoft.CheckBook.Powershell
{
    [Cmdlet(VerbsCommon.Close, "CheckbookCompany")]
    [OutputType(typeof(Company))]
    public class CloseCompany : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public Company Company { get; set; }

        protected override void BeginProcessing()
        {
            CompanySaver.SaveChangedAccounts(Company);
            Company.Teardown();
            Company.UnlockCompany();
        }
    }
}
