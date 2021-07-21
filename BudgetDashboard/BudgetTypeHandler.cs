using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.BudgetDashboard
{
    public abstract class BudgetTypeHandler
    {
        public abstract bool IncludeAccount(Account account);
        public abstract bool IncludeNormalTrx(BankTrx normalTrx);
        public abstract bool IncludeBudgetTrx(BudgetTrx budgetTrx);
        public abstract bool IncludeSplit(TrxSplit split);
    }
}
