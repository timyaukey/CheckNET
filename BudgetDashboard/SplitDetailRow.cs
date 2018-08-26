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

        public override DataCell MakeDataCell(SplitCarrier detail)
        {
            return new DataCell(0M, detail.Split.curAmount);
        }
    }
}
