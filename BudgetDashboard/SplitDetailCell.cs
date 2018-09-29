using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public class SplitDetailCell : DetailCell<SplitCarrier>
    {
        public SplitDetailCell()
        {
        }

        public SplitDetailCell(TrxSplit split)
            : base(split.curAmount, 0M, 0M, split.curAmount)
        {
        }
    }
}
