using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework;
using System.IO;
using inifiles;
using EnglishWords;
namespace EnglishWords
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        IniFile INI = new IniFile(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MyEngWords\setting.ini");
        public Panel usingPanel;
        public string wtf, rustrans;
        WinTranslator formTranslatorChrome;
        int rN;
        bool statusmove = false;
        Point lp;

        public Form1()
        {
            InitializeComponent();
            this.StyleManager = mSM1;
            creationVoids.cycleadd();
            mListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        void randitem()
        {
            rN = new Random().Next(mListView1.Items.Count);
        }
        //void cycleadd()
        //{
        //    if (File.Exists(VarsConsts.folderPath() + "MyWords.txt"))
        //    {
        //        string[] Mass = File.ReadAllLines(@VarsConsts.folderPath() + "MyWords.txt", System.Text.Encoding.UTF8);
        //        for (int i = 0; i < Mass.Count(); i++)
        //        {
        //            Label label = new Label();
        //            label.Name = "label" + i.ToString();
        //            label.Text = Mass[i];
        //            string akks = label.Text;
        //            string lastword = akks.Substring(akks.LastIndexOf('/') + 1);
        //            akks = akks.Substring(0, akks.LastIndexOf('/'));
        //            newItem(akks, lastword);
        //        }
        //    }
        //    else if (!File.Exists(VarsConsts.folderPath() + "MyWords.txt"))
        //        mListView1.Items.Add("Add something here first").SubItems.Add("Сначала что-нибудь добавь");
        //}
        void removeitem(bool learnedToggle)
        {
            int count = mListView1.SelectedItems.Count;
            if (count > 0)
            {
                List<ListViewItem> itemsToRemove = new List<ListViewItem> { };
                for (int i = 0; i < count; i++)
                {
                    string tsxt = mListView1.SelectedItems[i].Text;
                    if (learnedToggle)
                        File.AppendAllText(VarsConsts.folderPath() + "LearnedWords.txt", tsxt + "\r\n");
                    itemsToRemove.Add(mListView1.SelectedItems[i]);
                }
                foreach (ListViewItem itm in itemsToRemove)
                    mListView1.Items.Remove(itm);
                using (StreamWriter sw = new StreamWriter(VarsConsts.folderPath() + "MyWords.txt"))
                {
                    foreach (ListViewItem item in mListView1.Items)
                        sw.WriteLine(item.Text + "/" + item.SubItems[1].Text);
                    sw.Close();
                }
                if (learnedToggle)
                {
                    panelShoweer(mPanelRemove);
                    new Thread(mPanelRemoverHidder).Start();
                }
            }
        }
        void addItemsInListView()
        {
            foreach (ListViewItem listViewItem in EnglishWords.creationVoids.cycleadd())
                mListView1.Items.Add(listViewItem);
        }
        private void MetroButton1_Click(object sender, EventArgs e)
        {
            if (mListView1.SelectedItems.Count <= 1)
                removeitem(true);
            else
            {

                if (MetroMessageBox.Show(this, "You realy want to remove this items?", "Attention", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    removeitem(true);
                }
            }
        }
        void mPanelRemoverHidder()
        {
            Thread.Sleep(1300);
            mPanelRemove.Invoke(new Action(() =>
            {
                mPanelRemove.Visible = false;
            }));
            elementDisabler(true);
        }

        void testx(Control control, string tochange, bool hiding)
        {
            string ststring = control.Text;
            control.Invoke(new Action(() => { control.Text = tochange; }));
            Thread.Sleep(1700);
            control.Invoke(new Action(() => { control.Text = ststring; }));
            if (hiding)
                control.Invoke(new Action(() => { mPanelEdit.Visible = false; }));
            /*BeginInvoke(new Action(() =>
            {
                mButtonAdd.Text = "Added!";
                Thread.Sleep(1700);
                mButtonAdd.Text = "Add";
            }));*/
        }
        void newItemInList(string _item, string _subItem)
        {
            ListViewItem newWord = new ListViewItem(_item);
            newWord.SubItems.Add(_subItem);
            mListView1.Items.Add(newWord);
        }
        void adderNew()
        {
            if (mTextBoxEng.Text != "" && mTextBoxRus.Text != "")
            {
                var t = new Thread(() => testx(mButtonAdd, "Added!", false));
                t.Start();
                if (!File.Exists(VarsConsts.folderPath() + "MyWords.txt"))
                    mListView1.Items.Clear();
                newItemInList(mTextBoxEng.Text, mTextBoxRus.Text);
                File.AppendAllText(VarsConsts.folderPath() + "MyWords.txt", mTextBoxEng.Text + "/" + mTextBoxRus.Text + "\r\n");
                buttonClear.PerformClick();
            }
            else MetroMessageBox.Show(this, "The fields must not be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void MetroButton2_Click(object sender, EventArgs e)
        {
            adderNew();
            panelCloser(panelAdder);
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            switch (toggleNotify.Checked)
            {
                case (true):
                    timer1.Interval = Convert.ToInt16(numUPNotifyInt.Value) * 1000;
                    randitem();
                    notifyI1.ShowBalloonTip(5000, mListView1.Items[rN].SubItems[0].Text, mListView1.Items[rN].SubItems[1].Text, ToolTipIcon.Info);
                    break;
                case (false):
                    break;
            }
            //switch (togglePanels.Checked)
            //{
            //    case (true):
            //        toggleNotify.Checked = false;
            //        timer1.Interval = Convert.ToInt16(numUDTestInt.Value) * 1000;
            //        randitem();
            //        panelLearnerCreation();
            //        break;
            //    case (false):
            //        break;
            //}
        }
        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {

                this.ShowInTaskbar = false;

            }
        }

        private void NotifyI1_Click(object sender, EventArgs e)
        {

        }

        private void MetroFind_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(VarsConsts.folderPath() + "MyWords.txt");
            // System.Diagnostics.Process.Start(@"C:\Program Files\Sublime Text 3\sublime_text.exe", VarsConsts.folderPath() + "MyWords.txt");
        }

        private void MetroToggle1_CheckedChanged(object sender, EventArgs e)
        {
            if (toggleNotify.Checked == true)
            {
                INI.Write("Toggles", "toggleNotify", "true");
            }
            if (toggleNotify.Checked == false)
                INI.Write("Toggles", "toggleNotify", "false");
        }
        private void NumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt16(numUPNotifyInt.Value) * 1000;
            if (numUPNotifyInt.Value > 10)
                INI.Write("Timer", "NotifyInterval", numUPNotifyInt.Value.ToString());
        }
        // 
        private void Form1_Load(object sender, EventArgs e)
        {
            toggleNotify.Checked = VarsConsts.toggleToggler("toggleNotify");
            toggleSort.Checked = VarsConsts.sortState();
            checkBoxMultiSel.Checked = VarsConsts.toggleMultiSelection();
            numUDTestInt.Value = VarsConsts.interval("TestInterval");
            numUPNotifyInt.Value = VarsConsts.interval("NotifyInterval");
            numUPFont.Value = VarsConsts.fontSizeValue();
            timer1.Interval = (int)numUDTestInt.Value * 3000;
            this.Size = new Size(530, 584);
            mListView1.Anchor = (AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom);
            randitem();
            addItemsInListView();
            mListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            exitor();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //mListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            if (this.Location == VarsConsts.locations)
                mListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);


        }

        private void MListView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {

        }

        private void MListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = mListView1.SelectedItems[0];
            wtf = item.SubItems[0].Text;
            translatorFormCreator(wtf);
            //forsa.loadpage(wtf);
        }

        private void MListView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (mListView1.FocusedItem.Bounds.Contains(e.Location))
                {
                    contextMenuStrip1.Show(Cursor.Position);
                }
            }
        }
        void translatorFormCreator(string itemtoTranslate)
        {
            VarsConsts.wordTranslate = itemtoTranslate;
            if (formTranslatorChrome == null || formTranslatorChrome.IsDisposed)
            {
                formTranslatorChrome = new WinTranslator(this);
                formTranslatorChrome.Show();
            }
            else
            {
                formTranslatorChrome.Activate();
                formTranslatorChrome.loadpage(VarsConsts.wordTranslate);
            }
        }
        private void TSMITranslate_Click(object sender, EventArgs e)
        {
            ListViewItem item = mListView1.SelectedItems[0];
            wtf = item.SubItems[0].Text;
            translatorFormCreator(wtf);

        }

        private void MetroButton2_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(VarsConsts.folderPath() + "LearnedWords.txt");
            //System.Diagnostics.Process.Start(@"C:\Program Files\Sublime Text 3\sublime_text.exe", VarsConsts.folderPath() + "LearnedWords.txt");
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
        }
        void panelLearnerCreation()
        {
            metroLabel7.Text = "Show me translation!";
            metroLabel8.Text = mListView1.Items.Count.ToString() + " words left!";
            mPanelLearner.Visible = true;
            timer1.Enabled = false;
            ListViewItem item = mListView1.Items[rN];
            panelShoweer(mPanelLearner);
            metroLabel5.Text = item.SubItems[0].Text;
            rustrans = item.SubItems[1].Text;
        }
        void panelShoweer(Panel panel)
        {
            timer1.Enabled = false;
            usingPanel = panel;
            int xx = (mListView1.Size.Width / 2) + mListView1.Location.X - (panel.Size.Width / 2);
            int yy = (mListView1.Size.Height / 2) + mListView1.Location.Y - (panel.Size.Height / 2);
            panel.Location = new Point(xx, yy);
            panel.Visible = true;
            elementDisabler(false);
            panel.MouseDown += new MouseEventHandler(panelMouseDown);
            panel.MouseMove += new MouseEventHandler(panelMouseMove);
            panel.MouseUp += new MouseEventHandler(panelMouseUp);
        }
        void panelHidder(Panel panel)
        {
            panel.Visible = false;
            elementDisabler(true);
            timer1.Enabled = true;
        }
        void panelEditCreation()
        {
            timer1.Enabled = false;
            ListViewItem item = mListView1.SelectedItems[0];
            rN = item.Index;
            panelShoweer(mPanelEdit);
            mTbEditEng.Text = item.SubItems[0].Text;
            labelEditl.Text = "Edit — " + item.SubItems[0].Text;
            mTbEditRu.Text = item.SubItems[1].Text;
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelEditCreation();

        }
        void panelCloser(Panel panel)
        {
            panel.Visible = false;
            elementDisabler(true);
            timer1.Enabled = true;
        }
        private void MetroButtonEditCancel_Click(object sender, EventArgs e)
        {
            panelCloser(mPanelEdit);

        }
        void elementDisabler(bool stateCtrl)
        {
            List<Control> allControls = new List<Control> { buttonTest, buttonSet, buttonRemoveIt, buttonTranslate, mListView1, buttonAdder, buttonClose };
            foreach (Control allctrl in allControls)
            {
                //foreach (Control ctrlexc in exceptions)
                // if (ctrlexc == allctrl) allControls.Remove(ctrlexc);
                allctrl.Invoke(new Action(() => { allctrl.Enabled = stateCtrl; }));
            }
        }
        private void MetroButtonEditOK_Click(object sender, EventArgs e)
        {
            if (mTbEditEng.Text != "" && mTbEditRu.Text != "")
            {
                removeitem(false);
                newItemInList(mTbEditEng.Text, mTbEditRu.Text);
                File.AppendAllText(VarsConsts.folderPath() + "MyWords.txt", mTbEditEng.Text + "/" + mTbEditRu.Text + "\r\n");
                var t = new Thread(() => testx(metroButtonEditOK, "Saved!!", true));
                t.Start();
                panelCloser(mPanelEdit);
            }
            else
                MetroMessageBox.Show(this, "The fields must not be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #region panel mover
        void panelMouseDown(object sender, MouseEventArgs e)
        {
            statusmove = true;
            lp = Cursor.Position;
        }
        void panelMouseMove(object sender, MouseEventArgs e)
        {
            if (statusmove == true) //Если "перемещение включено"
            {
                int diff_x = Cursor.Position.X - lp.X;
                int diff_y = Cursor.Position.Y - lp.Y;
                Rectangle new_bounds = new Rectangle(usingPanel.Location.X + diff_x,
                usingPanel.Location.Y + diff_y, usingPanel.Width, usingPanel.Height);
                if (this.ClientRectangle.Contains(new_bounds))
                {
                    usingPanel.Location = new_bounds.Location;
                }
            }
            lp = Cursor.Position;
        }
        void panelMouseUp(object sender, MouseEventArgs e)
        {
            statusmove = false;
        }

        #endregion
        private void buttonClearText(object sender, EventArgs e)
        {
            mTextBoxEng.Text = "";
            mTextBoxRus.Text = "";
        }

        private void MTextBoxRus_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                mButtonAdd.PerformClick();
            }
        }

        private void MTextBoxEng_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                mButtonAdd.PerformClick();
            }
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem item = mListView1.SelectedItems[0];
            wtf = item.SubItems[0].Text;
            Clipboard.SetText(wtf);
        }

        private void notifyI1_BalloonTipClicked(object sender, EventArgs e)
        {
            mListView1.Items[rN].Selected = true;
            mListView1.EnsureVisible(rN);
        }

        private void buttontestOK(object sender, EventArgs e)
        {
            removeitem(true);
            var r4 = new Thread(() => ButtonTextChanger());
            r4.Start();
            randitem();
            panelLearnerCreation();
        }
        void ButtonTextChanger()
        {
            metroLabel4.Invoke(new Action(() =>
            {
                metroLabel4.Text = "Removed from list";
            }));
            Thread.Sleep(1100);
            metroLabel4.Invoke(new Action(() =>
            {
                metroLabel4.Text = "That's a new WORD";
            }));
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {

        }

        private void metroButton6_Click(object sender, EventArgs e)
        {
            elementDisabler(true);
            mPanelLearner.Visible = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyI1.Visible = false;
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            panelShoweer(panelSetting);
        }

        private void buttonAdder_Click(object sender, EventArgs e)
        {
            panelShoweer(panelAdder);
            mTextBoxEng.Focus();
        }

        private void metroButton2_Click_2(object sender, EventArgs e)
        {
            panelHidder(panelAdder);
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            panelHidder(panelSetting);
        }

        private void numUDTestInt_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = Convert.ToInt16(numUDTestInt.Value) * 1000;
            if (numUPNotifyInt.Value > 10)
                INI.Write("Timer", "TestInterval", numUDTestInt.Value.ToString());
        }
        void exitor()
        {
            notifyI1.Visible = false;
            if (formTranslatorChrome == null || formTranslatorChrome.IsDisposed)
            {

            }
            else { formTranslatorChrome.Close(); }
            Application.Exit();
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            exitor();
        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            translatorFormCreator(metroLabel5.Text);
        }

        private void notifyI1_DoubleClick(object sender, EventArgs e)
        {

            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
        }

        private void buttonShowWord_Click(object sender, EventArgs e)
        {
            int id = mListView1.FindItemWithText(metroLabel5.Text).Index;
            mListView1.Items[id].Selected = true;
            mListView1.EnsureVisible(id);
            panelHidder(mPanelLearner);
        }

        private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            switch (checkBoxMultiSel.Checked)
            {
                case (true):
                    INI.Write("ListSettings", "MultiSelection", "true");
                    mListView1.MultiSelect = true;
                    break;
                case (false):
                    INI.Write("ListSettings", "MultiSelection", "false");
                    mListView1.MultiSelect = false;
                    break;
            }
        }

        private void mListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonRemoveIt_Click(object sender, EventArgs e)
        {
            if (mListView1.SelectedItems.Count <= 1)
                removeitem(true);
            else
                if (MetroMessageBox.Show(this, "You realy want to remove this items?", "Attention", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                removeitem(true);
        }

        private void numericUpDown1_ValueChanged_1(object sender, EventArgs e)
        {
            mListView1.Font = new Font(mListView1.Font.FontFamily, (int)numUPFont.Value);
            INI.Write("ListSettings", "FontSize", Convert.ToString(numUPFont.Value));
        }

        private void metroButton7_Click(object sender, EventArgs e)
        {
            if (mListView1.SelectedItems.Count > 0)
                translatorFormCreator(mListView1.SelectedItems[0].Text);
            else translatorFormCreator("");
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            VarsConsts.locations = this.Location;
        }

        private void textBoxSearching_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int id = mListView1.FindItemWithText(textBoxSearching.Text).Index;
                mListView1.Items[id].Selected = true;
                mListView1.EnsureVisible(id);
            }
            catch (NullReferenceException d)
            {
                for (int i = 0; i < this.mListView1.SelectedIndices.Count; i++)
                {
                    mListView1.Items[mListView1.SelectedIndices[i]].Selected = false;
                }
            }
        }
        private void mListView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            switch (mListView1.Sorting)
            {
                case (SortOrder.Ascending):
                    mListView1.Sorting = SortOrder.Descending;
                    break;
                case (SortOrder.Descending):
                    mListView1.Sorting = SortOrder.Ascending;
                    break;
                default:
                    mListView1.Sorting = SortOrder.Ascending;
                    break;
            }

        }

        private void toggleSort_CheckedChanged(object sender, EventArgs e)
        {
            switch (toggleSort.Checked)
            {
                case (true):
                    mListView1.Sorting = SortOrder.Ascending;
                    INI.Write("ListSettings", "SortState", "true");
                    break;
                case (false):
                    INI.Write("ListSettings", "SortState", "false");
                    break;
            }
        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            adderNew();
        }

        private void metroLabel12_Click(object sender, EventArgs e)
        {
            randitem();
            mListView1.Items[rN].Selected = true;
            mListView1.EnsureVisible(rN);
        }

        private void CopySelectedValuesToClipboard()
        {
            var builder = new StringBuilder();
            foreach (ListViewItem item in mListView1.SelectedItems)
                builder.AppendLine(item.SubItems[0].Text);

            Clipboard.SetText(builder.ToString());
            sendCtrl = false;
        }
        bool sendCtrl = false;
        private void mListView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender != mListView1) return;

            if (e.KeyCode == Keys.C)
                CopySelectedValuesToClipboard();
            if (e.KeyCode == Keys.Delete)
                removeitem(true);

        }

        private void mListView1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void mListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control) sendCtrl = true;
        }

        private void metroButton7_Click_1(object sender, EventArgs e)
        {
            randitem();
            panelLearnerCreation();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            elementDisabler(false);
            randitem();
            panelLearnerCreation();
        }


        private void metroLabel7_Click(object sender, EventArgs e)
        {
            metroLabel7.Text = rustrans;
        }
    }
}
