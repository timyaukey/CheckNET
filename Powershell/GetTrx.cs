using System;
using System.Management.Automation;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.Powershell
{
    [Cmdlet(VerbsCommon.Get, "CheckbookTrx")]
    [OutputType(typeof(NormalTrx), typeof(BudgetTrx), typeof(TransferTrx), typeof(ReplicaTrx))]
    public class GetTrx : Cmdlet
    {
        [Parameter(Mandatory = true)]
        public Register Register { get; set; }

        [Parameter(Mandatory = true)]
        public DateTime StartDate { get; set; }

        [Parameter(Mandatory = true)]
        public DateTime EndDate { get; set; }

        [Parameter()]
        [ValidateSet("All", "Normal", "Budget", "Transfer")]
        public string TransType { get; set; }

        protected override void BeginProcessing()
        {
            if (string.IsNullOrEmpty(TransType))
                TransType = "All";
            switch(TransType.ToLower())
            {
                case "all":
                    Execute<Trx>();
                    return;
                case "normal":
                    Execute<NormalTrx>();
                    return;
                case "budget":
                    Execute<BudgetTrx>();
                    return;
                case "transfer":
                    Execute<TransferTrx>();
                    return;
            }
        }

        private void Execute<TTrx>()
            where TTrx : Trx
        {
            foreach(TTrx trx in new RegDateRange<TTrx>(Register, StartDate, EndDate))
            {
                WriteObject(trx);
            }
        }
    }
}
