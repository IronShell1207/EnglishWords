using System;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;


namespace EnglishWords
{
   
    public partial class WinTranslator : Form
    {
        public ChromiumWebBrowser chromiumWeb;
        //public string trword="";
        //public string trword;
        
        public WinTranslator(Form1 f1)
        {
            InitializeComponent();
            InitializeChromium();
        }
        public void InitializeChromium()
        {
            CefSettings setting = new CefSettings();
            setting.BackgroundColor = 000000;
            if (Cef.IsInitialized)
            {

            }
            else
                Cef.Initialize(setting);
            chromiumWeb = new ChromiumWebBrowser("https://translate.google.com/#view=home&op=translate&sl=en&tl=ru&text="+VarsConsts.wordTranslate);
            this.Controls.Add(chromiumWeb);
            chromiumWeb.Dock = DockStyle.Fill;
        }
        private void WinTranslator_Load(object sender, EventArgs e)
        {
            //chromiumWeb.Load("");
        }
        public void loadpage(string page)
        {
            chromiumWeb.Load("https://translate.google.com/#view=home&op=translate&sl=en&tl=ru&text="+page);
        }
        private void WinTranslator_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void WinTranslator_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void WinTranslator_Activated(object sender, EventArgs e)
        {
            NotifyIcon ifs = new NotifyIcon();
            ifs.Visible = false;
        }
    }
}
