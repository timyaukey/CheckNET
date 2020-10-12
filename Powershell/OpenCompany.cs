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
    [Cmdlet(VerbsCommon.Open, "CheckbookCompany")]
    public class OpenCompany : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public string Path { get; set; }

        [Parameter(Mandatory = false)]
        public string UserName { get; set; }

        [Parameter(Mandatory = false)]
        public string Password { get; set; }

        protected override void BeginProcessing()
        {
            Company company = new Company(Path);
            var error = CompanyLoader.objLoad(company, (account) => { }, authenticate);
            if (error != null)
            {
                WriteError(
                    new ErrorRecord(
                        new InvalidOperationException(error.strMessage),
                        "CompanyLoadFailure", 
                        ErrorCategory.InvalidOperation, 
                        null)
                    );
                return;
            }
            WriteObject(company);
        }

        private CompanyLoadError authenticate(Company company)
        {
            if (company.objSecurity.blnNoFile)
                return null;
            if (company.objSecurity.blnAuthenticate(UserName, Password))
                return null;
            return new CompanyLoadNotAuthorized();
        }
    }
}
