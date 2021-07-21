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
            const string errorId = "RegisterSearchFailure";
            Register regMatch = null;
            foreach (Register regTest in Account.Registers)
            {
                if (regTest.Title.Equals(RegisterName, StringComparison.InvariantCultureIgnoreCase))
                {
                    if (regMatch == null)
                        regMatch = regTest;
                    else
                        ThrowTerminatingError(ErrorUtilities.CreateInvalidOperation("Multiple registers match register name", errorId));
                }
            }
            if (regMatch == null)
                ThrowTerminatingError(ErrorUtilities.CreateInvalidOperation("Register name not found in account", errorId));
            WriteObject(regMatch);
        }
    }
}
