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
            Account acctMatch = null;
            foreach(Account acctTest in Company.colAccounts)
            {
                if (acctTest.strTitle.Equals(AccountName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (acctMatch == null)
                        acctMatch = acctTest;
                    else
                    {
                        WriteError(
                            new ErrorRecord(
                                new InvalidOperationException("Multiple accounts match account name"),
                                "AccountSearchFailure",
                                ErrorCategory.InvalidOperation,
                                null)
                            );
                        return;
                    }
                }
            }
            if (acctMatch == null)
            {
                WriteError(
                    new ErrorRecord(
                        new InvalidOperationException("Account name not found in company"),
                        "AccountSearchFailure",
                        ErrorCategory.InvalidOperation,
                        null)
                    );
                return;
            }
            WriteObject(acctMatch);
        }
    }
}
