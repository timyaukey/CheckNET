using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create License File For Willow Creek Checkbook");
            Console.Write("Name of user to license (e.g. \"John Smith\"): ");
            string userName = Console.ReadLine();
            Console.Write("Email address of this user: ");
            string emailAddress = Console.ReadLine();
            Console.Write("License serial number (anything will work): ");
            string serialNumber = Console.ReadLine();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(serialNumber))
            {
                Console.WriteLine("Missing data.");
                return;
            }
            const string userLicenseFileName = "User.lic";
            using (System.IO.Stream output = new System.IO.FileStream(userLicenseFileName, System.IO.FileMode.Create))
            {
                Willowsoft.CheckBook.LicenseGenerator.UserLicenseBuilder.Build(userName, emailAddress, serialNumber, output);
                Console.Write("User license written to " + userLicenseFileName);
            }
        }
    }
}
