﻿using System;
using System.Security.Cryptography;

namespace Willowsoft.CheckBook.BudgetDashboard
{
    // Code generated by TamperProofCoder.CodeFactory
    public class BudgetDashboardLicenseValidator : Willowsoft.TamperProofData.Validator
    {
        protected override RSAParameters GetPublicKey()
        {
            RSAParameters rsap = new RSAParameters();
            rsap.Exponent = new byte[] { 1, 0, 1 };
            rsap.Modulus = new byte[] { 163, 69, 17, 132, 33, 142, 147, 182, 23, 200, 5, 48, 162, 252, 66, 56,
              93, 131, 249, 26, 170, 124, 218, 80, 110, 226, 8, 243, 147, 106, 90, 50,
              98, 0, 61, 249, 60, 143, 252, 104, 27, 49, 154, 70, 32, 220, 45, 224,
              11, 27, 61, 181, 81, 182, 77, 147, 150, 158, 40, 82, 78, 139, 81, 171,
              168, 197, 80, 150, 57, 106, 43, 89, 63, 85, 83, 40, 33, 228, 220, 24,
              219, 168, 12, 26, 85, 185, 131, 143, 252, 242, 5, 95, 6, 249, 36, 197,
              218, 74, 195, 104, 132, 104, 173, 223, 40, 87, 198, 202, 250, 212, 224, 42,
              3, 98, 37, 27, 44, 22, 250, 216, 50, 217, 28, 249, 144, 176, 15, 209 };
            return rsap;
        }
    }
}