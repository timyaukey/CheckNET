using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public class SplitCarrier
    {
        public readonly TrxSplit Split;
        public readonly NormalTrx Trx;

        public SplitCarrier(TrxSplit split, NormalTrx trx)
        {
            this.Split = split;
            this.Trx = trx;
        }
    }
}
