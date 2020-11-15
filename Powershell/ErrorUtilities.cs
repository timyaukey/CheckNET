using System;
using System.Management.Automation;

namespace Willowsoft.CheckBook.Powershell
{
    public static class ErrorUtilities
    {
        public static ErrorRecord CreateInvalidOperation(string message, string errorId)
        {
            return new ErrorRecord(
                new InvalidOperationException(message),
                errorId,
                ErrorCategory.InvalidOperation,
                null);
        }
    }
}
