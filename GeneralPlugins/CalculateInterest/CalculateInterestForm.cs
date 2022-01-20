using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Willowsoft.CheckBook.Lib;
using Willowsoft.CheckBook.PluginCore;

namespace Willowsoft.CheckBook.GeneralPlugins
{
    public partial class CalculateInterestForm : Form
    {
        private IHostUI HostUI;

        public CalculateInterestForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(IHostUI hostUI)
        {
            HostUI = hostUI;
            ctlStartDate.Value = new DateTime(1900, 1, 1);
            ctlEndDate.Value = new DateTime(1900, 1, 1);
            return this.ShowDialog();
        }
    }
}
