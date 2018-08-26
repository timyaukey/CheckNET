using System;
using System.Collections.Generic;

namespace BudgetDashboard
{
    public abstract class DetailCell<T> : DataCell
        where T : class
    {
        public readonly List<T> Details = new List<T>();

        public void Add(T detail)
        {
            Details.Add(detail);
        }

        public override string ToString()
        {
            return "(count=" + Details.Count.ToString() + ")" + base.ToString();
        }
    }
}
