using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Willowsoft.CheckBook.GeneralPlugins.CalculateInterest
{
    public interface IInterestCalculator
    {
        // Calculate interest amount.
        decimal Calculate(DateTime startDate, decimal[] dailyBalances, decimal annualRate);

        // Description of what this calculator does, to show in the UI.
        string Description { get; }

        // Transaction memo to use for this calculated interest.
        string Memo(decimal annualRate, decimal avgDailyBal);
    }
}
