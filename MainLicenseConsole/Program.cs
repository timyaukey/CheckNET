using System;

namespace LicenseConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            new Willowsoft.TamperProofConsole.StandardLicenseConsole
                <Willowsoft.CheckBook.LicenseGenerator.MainLicenseSigner>
                ("Willow Creek Checkbook2", "User.lic", 1)
                .Run();
        }
    }
}
