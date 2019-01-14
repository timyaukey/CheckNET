using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BudgetDashboard
{
    /// <summary>
    /// A DataGridViewTextBoxCell that displays the CellAmount property of the DataCell
    /// passed to the constructor, instead of the Value property of the control.
    /// If will show the current value of CellAmount every time the control is redrawn.
    /// </summary>
    /// <typeparam name="TCell"></typeparam>
    public class DataCellGridCell<TCell> : DataGridViewTextBoxCell
        where TCell : DataCell
    {
        protected TCell mDataCell;

        public DataCellGridCell(TCell dataCell)
        {
            mDataCell = dataCell;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds,
        int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue,
        string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
        DataGridViewPaintParts paintParts)
        {
            // Passes mDataCell.CellAmount to the base class, not the "value" parameter passed in here.
            this.Value = mDataCell.CellAmount.ToString("F2");
            base.Paint(graphics, clipBounds, cellBounds,
                rowIndex, cellState, this.Value, formattedValue,
                errorText, cellStyle, advancedBorderStyle,
                paintParts);
        }
    }
}
