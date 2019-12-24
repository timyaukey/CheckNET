using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Willowsoft.CheckBook.BudgetDashboard
{
    public class TotalRow : DataRow<DataCell>
    {
        public TotalRow(int periodCount, string key, string label, string sequence)
            : base(periodCount, key, label, sequence)
        {
        }
    }
}
