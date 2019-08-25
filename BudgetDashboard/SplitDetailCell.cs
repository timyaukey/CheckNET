using System;
using System.Collections.Generic;

using CheckBookLib;

namespace BudgetDashboard
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
