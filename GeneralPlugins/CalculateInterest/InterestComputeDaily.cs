using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.GeneralPlugins.CalculateInterest
{
    public class InterestComputeDaily : IInterestCalculator
    {
        private string Label;
        private int DaysPerYear;
        private bool CompoundDaily;

        public InterestComputeDaily(int daysPerYear, bool compoundDaily)
        {
            Label = (compoundDaily ? "Computed and compounded daily" : "Computed daily") +
                ", " + daysPerYear.ToString() + " year";
            DaysPerYear = daysPerYear;
            CompoundDaily = compoundDaily;
        }

        public override string ToString()
        {
            return Label;
        }

        public string Description
        {
            get
            {
                return "Calculate interest daily, " +
                    (CompoundDaily ? "compound daily" : "no compounding") +
                    ", based on " + DaysPerYear + " day year.";
            }
        }

        public string Memo(decimal annualRate, decimal avgDailyBal)
        {
            return (annualRate * 100M).ToString("F2") + "% APR, " +
                Utilities.FormatCurrency(avgDailyBal) + " avg daily bal, " +
                (CompoundDaily ? "compounded daily" : "not compounded") +
                ", " + DaysPerYear.ToString() + " days in year";
        }

        public decimal Calculate(DateTime startDate, decimal[] dailyBalances, decimal annualRate)
        {
            decimal dailyRate = annualRate / DaysPerYear;
            decimal totalInterest = 0;
            foreach(decimal dailyBalance in dailyBalances)
            {
                decimal dailyInterest = (dailyBalance + (CompoundDaily ? totalInterest : 0M)) * dailyRate;
                totalInterest += dailyInterest;
            }
            return totalInterest;
        }
    }
}
