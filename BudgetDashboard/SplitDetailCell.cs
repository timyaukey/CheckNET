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
            List<NormalTrx> parentsAddedToGenerated = new List<NormalTrx>();
            foreach (TrxSplit split in this.Splits)
            {
                if (split.objBudget == null)
                {
                    this.CellAmount += split.curAmount;
                    // Because a NormalTrx may have several splits that are
                    // added to this cell, and we only want to add the trx
                    // to the generated total once.
                    if (!parentsAddedToGenerated.Contains(split.objParent))
                    {
                        this.GeneratedAmount += split.objParent.curGeneratedAmount;
                        parentsAddedToGenerated.Add(split.objParent);
                    }
                }
            }
        }
    }
}
