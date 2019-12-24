using System;
using System.Collections.Generic;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.BudgetDashboard
{
    public class SplitDetailCell : DataCell
    {
        public List<TrxSplit> Splits;

        public SplitDetailCell()
        {
            Splits = new List<TrxSplit>();
        }
    }
}
