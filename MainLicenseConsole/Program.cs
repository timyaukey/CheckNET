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
            Console.Write("Name of entity to license (e.g. \"John Smith\" or \"Chess Club\"): ");
            string licensedTo = Console.ReadLine();
            Console.Write("Expiration date (\"mm/dd/yyyy\", blank if never expires): ");
            string expDateText = Console.ReadLine();
            DateTime? expirationDate;
            if (string.IsNullOrEmpty(expDateText))
                expirationDate = null;
            else
            {
                DateTime expDateTemp;
                if (!DateTime.TryParseExact(expDateText, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out expDateTemp))
                {
                    Console.WriteLine("Invalid expiration date.");
                    return;
                }
                expirationDate = expDateTemp;
            }
            Console.Write("Email address of this user: ");
            string emailAddress = Console.ReadLine();
            Console.Write("License serial number (anything will work): ");
            string serialNumber = Console.ReadLine();
            if (string.IsNullOrEmpty(licensedTo) || string.IsNullOrEmpty(emailAddress) || string.IsNullOrEmpty(serialNumber))
            {
                Console.WriteLine("Missing data.");
                return;
            }
            const string userLicenseFileName = "User.lic";
            using (System.IO.Stream output = new System.IO.FileStream(userLicenseFileName, System.IO.FileMode.Create))
            {
                Willowsoft.CheckBook.LicenseGenerator.UserLicenseBuilder.Build(licensedTo, expirationDate, emailAddress, serialNumber, output);
                Console.Write("User license written to " + userLicenseFileName);
            }
        }
    }
}
