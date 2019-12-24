using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.BudgetDashboard
{
    public class BudgetTypeIncExp : BudgetTypeHandler
    {
        public override bool IncludeAccount(Account account)
        {
            return true;
        }

        public override bool IncludeBudgetTrx(BudgetTrx budgetTrx)
        {
            return true;
        }

        public override bool IncludeNormalTrx(NormalTrx normalTrx)
        {
            return true;
        }

        public override bool IncludeSplit(TrxSplit split)
        {
            return split.strCategoryKey.IndexOf('.') <= 0;
        }

        public override string ToString()
        {
            return "Income & Expense";
        }
    }
}
