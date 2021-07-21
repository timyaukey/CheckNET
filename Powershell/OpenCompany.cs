using System;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{
    [Cmdlet(VerbsCommon.Open, "CheckbookCompany")]
    [OutputType(typeof(Company))]
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
                ThrowTerminatingError(ErrorUtilities.CreateInvalidOperation(error.strMessage, "CompanyLoadFailure"));
            WriteObject(company);
        }

        private CompanyLoadError authenticate(Company company)
        {
            if (company.SecData.blnNoFile)
                return null;
            if (company.SecData.blnAuthenticate(UserName, Password))
                return null;
            return new CompanyLoadNotAuthorized();
        }
    }
}
