using System;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{
    [Cmdlet(VerbsCommon.Get, "CheckbookRegister")]
    [OutputType(typeof(Register))]
    public class GetRegister : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public Account Account { get; set; }

        [Parameter(Mandatory = true)]
        public string RegisterName { get; set; }

        protected override void BeginProcessing()
        {
            Register regMatch = null;
            foreach (Register regTest in Account.colRegisters)
            {
                if (regTest.strTitle.Equals(RegisterName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (regMatch == null)
                        regMatch = regTest;
                    else
                    {
                        WriteError(
                            new ErrorRecord(
                                new InvalidOperationException("Multiple registers match register name"),
                                "RegisterSearchFailure",
                                ErrorCategory.InvalidOperation,
                                null)
                            );
                        return;
                    }
                }
            }
            if (regMatch == null)
            {
                WriteError(
                    new ErrorRecord(
                        new InvalidOperationException("Register name not found in account"),
                        "RegisterSearchFailure",
                        ErrorCategory.InvalidOperation,
                        null)
                    );
                return;
            }
            WriteObject(regMatch);
        }
    }
}
