﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Willowsoft.CheckBook.LicenseGenerator
{
    public static class UserLicenseBuilder
    {
        public static void Build(string userName, string emailAddress, string serialNumber, System.IO.Stream output)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            values.Add("UserName", userName);
            values.Add("EmailAddress", emailAddress);
            values.Add("SerialNumber", serialNumber);
            Willowsoft.TamperProofData.LicenseWriter.Write(values, new UserLicenseSigner(), output);
        }
    }
}