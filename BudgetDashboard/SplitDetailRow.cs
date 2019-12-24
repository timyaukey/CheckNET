using System;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.BudgetDashboard
{
    public class SplitDetailRow : DataRow<SplitDetailCell>
    {
        public SplitDetailRow(int periodCount, string key, string label, string sequence)
            : base(periodCount, key, label, sequence)
        {
        }
    }
}
