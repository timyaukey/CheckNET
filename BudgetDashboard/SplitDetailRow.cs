using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public class SplitDetailRow : DetailRow<SplitDetailCell, TrxSplit>
    {
        public SplitDetailRow(int periodCount, string key, string label, string sequence)
            : base(periodCount, key, label, sequence)
        {
        }

        protected override SplitDetailCell MakeDataCell(TrxSplit detail)
        {
            return new SplitDetailCell(detail);
        }

        protected override void AddExtraData(SplitDetailCell accumulator, SplitDetailCell source)
        {
        }
    }
}
