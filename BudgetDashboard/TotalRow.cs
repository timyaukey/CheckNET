using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BudgetDashboard
{
    public class TotalRow : DataRow<DataCell>
    {
        public TotalRow(int periodCount, string key, string label)
            : base(periodCount, key, label)
        {
        }
    }
}
