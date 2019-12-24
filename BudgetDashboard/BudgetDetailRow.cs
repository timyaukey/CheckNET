using System;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.BudgetDashboard
{
    public class BudgetDetailRow : DataRow<BudgetDetailCell>
    {
        public BudgetDetailRow(int periodCount, string key, string label, string sequence)
            : base(periodCount, key, label, sequence)
        {
        }
    }
}
