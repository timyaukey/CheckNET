using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CheckBookLib;

namespace BudgetDashboard
{
    public abstract class BudgetTypeHandler
    {
        public abstract bool IncludeAccount(Account account);
        public abstract bool IncludeNormalTrx(NormalTrx normalTrx);
        public abstract bool IncludeBudgetTrx(BudgetTrx budgetTrx);
        public abstract bool IncludeSplit(TrxSplit split);
    }
}
