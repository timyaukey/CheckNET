using System;

using CheckBookLib;

namespace BudgetDashboard
{
    public class BudgetDetailRow : DetailRow<BudgetDetailCell, BudgetTrx>
    {
        public bool HasUnalignedPeriods { get; set; }

        public BudgetDetailRow(int periodCount, string key, string label, string sequence)
            : base(periodCount, key, label, sequence)
        {
        }

        protected override BudgetDetailCell MakeDataCell(BudgetTrx detail)
        {
            return new BudgetDetailCell(detail);
        }

        protected override void AddExtraData(BudgetDetailCell accumulator, BudgetDetailCell source)
        {
            accumulator.BudgetApplied += source.BudgetApplied;
            accumulator.BudgetLimit += source.BudgetLimit;
        }
    }
}
