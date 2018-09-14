using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public class SplitDetailRow : DataRow<SplitDetailCell, SplitCarrier>
    {
        public SplitDetailRow(int periodCount, string key, string label)
            : base(periodCount, key, label)
        {
        }

        public override SplitDetailCell MakeDataCell(SplitCarrier detail)
        {
            return new SplitDetailCell(0M, detail.Split.curAmount);
        }
    }
}
