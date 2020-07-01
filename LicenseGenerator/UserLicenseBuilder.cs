using System;
using System.Collections.Generic;
using System.Text;

namespace Willowsoft.CheckBook.LicenseGenerator
{
    public static class UserLicenseBuilder
    {
        public static void Build(string licensedTo, DateTime? expirationDate, string emailAddress, string serialNumber, System.IO.Stream output)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("LicensedTo", licensedTo);
            if (expirationDate.HasValue)
                values.Add("ExpirationDate", expirationDate.Value.ToShortDateString());
            values.Add("EmailAddress", emailAddress);
            values.Add("SerialNumber", serialNumber);
            Willowsoft.TamperProofData.LicenseWriter.Write(values, new UserLicenseSigner(), output);
        }
    }
}
