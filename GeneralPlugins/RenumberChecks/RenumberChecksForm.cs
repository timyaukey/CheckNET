using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Willowsoft.CheckBook.Lib;
using Willowsoft.CheckBook.PluginCore;

namespace Willowsoft.CheckBook.GeneralPlugins
{
    public partial class RenumberChecksForm : Form
    {
        private IHostUI HostUI;

        public RenumberChecksForm()
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

        private void btnRenumber_Click(object sender, EventArgs e)
        {
            List<NormalTrx> objToChange = new List<NormalTrx>();
            if (!Int32.TryParse(txtStartNumber.Text, out int startNumber))
            {
                HostUI.ErrorMessageBox("Invalid starting check number.");
                return;
            }
            if (!Int32.TryParse(txtEndNumber.Text, out int endNumber))
            {
                HostUI.ErrorMessageBox("Invalid ending check number.");
                return;
            }
            if (!Int32.TryParse(txtAddNumber.Text, out int addNumber))
            {
                HostUI.ErrorMessageBox("Invalid amount to add to each check number.");
                return;
            }
            Register reg = HostUI.objGetCurrentRegister();
            foreach (var objNormal in reg.colDateRange<NormalTrx>(ctlStartDate.Value, ctlEndDate.Value))
            {
                if (Int32.TryParse(objNormal.strNumber, out int checkNumber))
                {
                    if (checkNumber >= startNumber && checkNumber <= endNumber)
                    {
                        objToChange.Add(objNormal);
                        //HostUI.InfoMessageBox("Renumbering " + objNormal.ToString());
                    }
                }
            }
            // Cannot change number during initial scan, because this changes the register order.
            if (HostUI.OkCancelMessageBox("Found " + objToChange.Count + " checks to renumber. Continue?") == DialogResult.OK)
            {
                foreach (var objNormal in objToChange)
                {
                    NormalTrxManager objMgr = objNormal.objGetTrxManager();
                    int checkNumber = Int32.Parse(objNormal.strNumber);
                    objMgr.UpdateStart();
                    string newNumber = (checkNumber + addNumber).ToString();
                    lblProgress.Text = "Changing #" + objMgr.objTrx.strNumber + " to #" + newNumber;
                    lblProgress.Refresh();
                    objMgr.objTrx.strNumber = newNumber;
                    objMgr.UpdateEnd(new LogChange(), "RenumberChecksForm.ChangeNumber");
                }
                HostUI.InfoMessageBox("Renumbered " + objToChange.Count + " checks.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
