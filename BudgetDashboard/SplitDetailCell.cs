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

        public override void SetAmountsFromDetail()
        {
            this.ClearAmounts();
            List<BankTrx> parentsAddedToGenerated = new List<BankTrx>();
            foreach (TrxSplit split in this.Splits)
            {
                if (split.Budget == null)
                {
                    this.CellAmount += split.Amount;
                    // Because a BankTrx may have several splits that are
                    // added to this cell, and we only want to add the trx
                    // to the generated total once.
                    if (!parentsAddedToGenerated.Contains(split.Parent))
                    {
                        this.GeneratedAmount += split.Parent.GeneratedAmount;
                        parentsAddedToGenerated.Add(split.Parent);
                    }
                }
            }
        }
    }
}
