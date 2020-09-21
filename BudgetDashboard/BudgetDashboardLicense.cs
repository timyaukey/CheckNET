﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willowsoft.CheckBook.BudgetDashboard
{
    public class BudgetDashboardLicense : Willowsoft.TamperProofData.StandardLicenseBase<BudgetDashboardLicenseValidator>
    {
        public override string BaseFileName => "Willowsoft.Checkbook.BudgetDashboard.lic";

        public override string LicenseTitle => "Willow Creek Budget Dashboard License";

        public override string AttributeSummary => "Unrestricted";
    }
}