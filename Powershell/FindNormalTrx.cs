using System;
using System.Collections.Generic;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{
    [Cmdlet(VerbsCommon.Find, "CheckbookNormalTrx")]
    [OutputType(typeof(BankTrx))]
    public class FindNormalTrx : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public Register Register { get; set; }

        [Parameter]
        public int Number { get; set; }

        [Parameter(Mandatory = true)]
        public DateTime Date { get; set; }

        [Parameter(Mandatory = true)]
        public string Description { get; set; }

        [Parameter]
        public decimal Amount { get; set; }

        [Parameter]
        public decimal MatchMin { get; set; }

        [Parameter]
        public decimal MatchMax { get; set; }

        [Parameter]
        public int DaysBefore { get; set; }

        [Parameter]
        public int DaysAfter { get; set; }

        [Parameter]
        public bool LooseMatch { get; set; }

        protected override void BeginProcessing()
        {
            ICollection<BankTrx> colMatches = null;
            ICollection<BankTrx> colExactMatches = null;
            bool blnExactMatch = false;
            Register.MatchNormalCore(
                lngNumber: Number,
                datDate: Date,
                intDaysBefore: DaysBefore,
                intDaysAfter: DaysAfter,
                strDescription: Description,
                curAmount: Amount,
                curMatchMin: MatchMin,
                curMatchMax: MatchMax,
                blnLooseMatch: LooseMatch,
                colMatches: ref colMatches,
                colExactMatches: ref colExactMatches,
                blnExactMatch: ref blnExactMatch);
            SearchUtilities.PruneToExactMatches(colExactMatches, Date, ref colMatches, ref blnExactMatch);
            foreach(BankTrx objTrx in colMatches)
            {
                WriteObject(objTrx);
            }
        }
    }
}
