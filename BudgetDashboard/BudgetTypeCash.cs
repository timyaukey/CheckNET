using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Willowsoft.CheckBook.Lib;

namespace Willowsoft.CheckBook.BudgetDashboard
{
    public class BudgetTypeCash : BudgetTypeHandler
    {
        public override bool IncludeAccount(Account account)
        {
            return 
                (account.lngSubType == Account.SubType.Asset_CheckingAccount) ||
                (account.lngSubType == Account.SubType.Asset_SavingsAccount);
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
            return true;
        }

        public override string ToString()
        {
            return "Cash";
        }
    }
}
