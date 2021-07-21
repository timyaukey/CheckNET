using System;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{
    [Cmdlet(VerbsCommon.Get, "CheckbookAccount")]
    [OutputType(typeof(Account))]
    public class GetAccount : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public Company Company { get; set; }

        [Parameter(Mandatory = true)]
        public string AccountName { get; set; }

        protected override void BeginProcessing()
        {
            const string errorId = "AccountSearchFailure";
            Account acctMatch = null;
            foreach(Account acctTest in Company.Accounts)
            {
                if (acctTest.Title.Equals(AccountName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (acctMatch == null)
                        acctMatch = acctTest;
                    else
                        ThrowTerminatingError(ErrorUtilities.CreateInvalidOperation("Multiple accounts match account name", errorId));
                }
            }
            if (acctMatch == null)
                ThrowTerminatingError(ErrorUtilities.CreateInvalidOperation("Account name not found in company", errorId));
            WriteObject(acctMatch);
        }
    }
}
