﻿using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public class SplitDetailCell : DetailCell<TrxSplit>
    {
        public SplitDetailCell()
        {
        }

        public SplitDetailCell(TrxSplit split)
            : base(split.curAmount)
        {
        }
    }
}
