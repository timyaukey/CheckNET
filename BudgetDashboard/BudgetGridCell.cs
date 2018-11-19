using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BudgetDashboard
{
    public class BudgetGridCell : DataGridViewTextBoxCell
    {
        private decimal BudgetLimit;
        private decimal BudgetApplied;

        public BudgetGridCell(decimal budgetLimit, decimal budgetApplied)
        {
            BudgetLimit = budgetLimit;
            BudgetApplied = budgetApplied;
        }

        public void UpdateBudgets(decimal budgetLimit, decimal budgetApplied)
        {
            BudgetLimit = budgetLimit;
            BudgetApplied = budgetApplied;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, 
            int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, 
            string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, 
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, 
                rowIndex, cellState, value, formattedValue, 
                errorText, cellStyle, advancedBorderStyle, 
                paintParts);
            // If there is no budget limit then no budget analysis is possible.
            if (BudgetLimit != 0M)
            {
                int barMaxWidth = cellBounds.Width - 3;
                int barWidth;
                Brush barBrush;
                // This tests for applied and limit having opposite signs -
                // which means the amount applied is "less than zero".
                double budgetFraction = (double)BudgetApplied / (double)BudgetLimit;
                if (budgetFraction > 0d)
                {
                    if (budgetFraction <= 1.0d)
                    {
                        barBrush = Brushes.ForestGreen;
                        barWidth = (int)(budgetFraction * (double)barMaxWidth);
                    }
                    else if (budgetFraction <= 2.0d)
                    {
                        barBrush = Brushes.Yellow;
                        barWidth = (int)((budgetFraction - 1.0d) * (double)barMaxWidth);
                    }
                    else
                    {
                        barBrush = Brushes.Red;
                        barWidth = (int)((budgetFraction - 2.0d) * (double)barMaxWidth);
                    }
                    if (barWidth > 0)
                    {
                        if (barWidth > barMaxWidth)
                            barWidth = barMaxWidth;
                        Rectangle newRect = new Rectangle(
                            cellBounds.X + 1,
                            cellBounds.Y + cellBounds.Height - 5,
                            barWidth,
                            3);
                        graphics.FillRectangle(barBrush, newRect);
                    }
                }
            }
        }
    }
}
