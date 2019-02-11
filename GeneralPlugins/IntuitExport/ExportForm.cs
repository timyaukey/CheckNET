using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CheckBookLib;

namespace GeneralPlugins.IntuitExport
{
    public partial class ExportForm : Form
    {
        private ExportEngine ExportEngine;
        private IHostUI HostUI;
        private string BalSheetTranslatorFileName;
        private Dictionary<string, ExportEngine.BalanceSheetMap> BalanceSheetMaps;
        private string CatTranslatorFileName;
        private Dictionary<string, ExportEngine.CategoryMap> CategoryMaps;

        public ExportForm()
        {
            InitializeComponent();
        }

        public DialogResult ShowDialog(ExportEngine exportEngine, IHostUI hostUI)
        {
            ExportEngine = exportEngine;
            HostUI = hostUI;
            ctlStartDate.Value = new DateTime(1980, 1, 1);
            ctlEndDate.Value = DateTime.Today;
            DialogResult result = this.ShowDialog();
            if (result == DialogResult.OK)
            {
                // Set ExportEngine properties
                ExportEngine.StartDate = ctlStartDate.Value;
                ExportEngine.EndDate = ctlEndDate.Value;
                ExportEngine.BalanceSheetMaps = BalanceSheetMaps;
                ExportEngine.CategoryMaps = CategoryMaps;
            }
            return result;
        }

        private void btnChooseBalSheetTranslator_Click(object sender, EventArgs e)
        {
            DialogResult result = ctlChooseBalSheetTranslator.ShowDialog();
            if (result != DialogResult.OK)
                return;
            BalSheetTranslatorFileName = ctlChooseBalSheetTranslator.FileName;
            lblBalSheetTranslatorFile.Text = BalSheetTranslatorFileName;
        }

        private void btnBalSheetTranslatorHelp_Click(object sender, EventArgs e)
        {
            HostUI.InfoMessageBox("A new balance sheet account will be be created in QuickBooks " +
                "for every balance sheet account in " + HostUI.strSoftwareName + 
                ", unless you use a balance sheet translation file to specify " +
                "existing QuickBooks accounts to use instead " +
                "for some of your accounts." +
                Environment.NewLine + Environment.NewLine +
                "The balance sheet translation file is a text file that contains one " +
                "line for each account you want to use an existing QuickBooks account for. " +
                "You can create this file with Windows Notepad, or any other text editor. " +
                "You cannot use Microsoft Word, or any other word processing software, " + 
                "because these do not create a simple text file! " +
                Environment.NewLine + Environment.NewLine +
                "Each line starts with the " + HostUI.strSoftwareName + " account file name (e.g. \"Checking.act\"), " +
                "then a tab, and ends with the equivalent QuickBooks account name (e.g. \"Checking\"). " +
                "Two names, separated by a tab, no quotes. " +
                "You do not have to add a line for every account - any accounts you do not " +
                "mention will simply be created as new accounts in QuickBooks.");
        }

        private void btnChooseCatTranslator_Click(object sender, EventArgs e)
        {
            DialogResult result = ctlChooseCatTranslator.ShowDialog();
            if (result != DialogResult.OK)
                return;
            CatTranslatorFileName = ctlChooseCatTranslator.FileName;
            lblCatTranslatorFile.Text = CatTranslatorFileName;
        }

        private void btnCatTranslatorHelp_Click(object sender, EventArgs e)
        {
            HostUI.InfoMessageBox("A new income or expense account will be created in QuickBooks " +
                "for every income and expense category in " + HostUI.strSoftwareName +
                ", unless you use a category translation file to specify " +
                "existing QuickBooks income or expense accounts to use instead " + 
                "for some of your categories. " +
                Environment.NewLine + Environment.NewLine +
                "The category translation file is a text file that contains one " +
                "line for each category you want to use an existing QuickBooks account for. " +
                "You can create this file with Windows Notepad, or any other text editor. " +
                "You cannot use Microsoft Word, or any other word processing software, " +
                "because these do not create a simple text file! " +
                Environment.NewLine + Environment.NewLine +
                "Each line starts with the " + HostUI.strSoftwareName + " category name (e.g. \"E:Advertising\"), " +
                "then a tab, and ends with the QuickBooks income or expense account name (e.g. \"Advertising\"). " +
                "Two names, separated by a tab, no quotes. " +
                "You do not have to add a line for every category - any categories you do not " +
                "mention will simply be created as new income/expense accounts in QuickBooks.");
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            if (!TryLoadFile(BalSheetTranslatorFileName, out BalanceSheetMaps))
                return;
            if (!TryLoadFile(CatTranslatorFileName, out CategoryMaps))
                return;
            this.Close();
        }

        private bool TryLoadFile<TElement>(string fileName, out Dictionary<string, TElement> elements)
            where TElement : ExportEngine.AccountMap, new()
        {
            elements = new Dictionary<string, TElement>();
            if (!string.IsNullOrEmpty(fileName))
            {
                using (System.IO.TextReader reader = new System.IO.StreamReader(fileName))
                {
                    for (; ; )
                    {
                        string line = reader.ReadLine();
                        if (line == null)
                            break;
                        line = line.Trim();
                        int tabIndex = line.IndexOf('\t');
                        if (tabIndex < 0)
                        {
                            HostUI.ErrorMessageBox("Line in translation file does not contain a tab.");
                            return false;
                        }
                        if (tabIndex == 0)
                        {
                            HostUI.ErrorMessageBox(HostUI.strSoftwareName + " account name is empty in translation file line.");
                            return false;
                        }
                        if (tabIndex == line.Length - 1)
                        {
                            HostUI.ErrorMessageBox(HostUI.strSoftwareName + " account name is empty in translation file line.");
                            return false;
                        }
                        TElement elm = new TElement();
                        elm.LocalName = line.Substring(0, tabIndex);
                        elm.IntuitName = line.Substring(tabIndex + 1);
                        elements.Add(elm.LocalName, elm);
                    }
                }
            }
            return true;
        }
    }
}
