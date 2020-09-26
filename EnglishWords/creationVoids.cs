using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using EnglishWords;
using System.Windows.Forms;


namespace EnglishWords
{
    public class creationVoids
    {
        public static int intervals { get; set; }
        public static List<ListViewItem> cycleadd()
        {
            List<ListViewItem> listItems = new List<ListViewItem> { };
            string fileNamePath = VarsConsts.folderPath() + "MyWords.txt";
            if (File.Exists(fileNamePath))
            {
                string[] Mass = File.ReadAllLines(fileNamePath, System.Text.Encoding.UTF8);
                for (int i = 0; i < Mass.Count(); i++)
                {
                    Label label = new Label();
                    label.Name = "label" + i.ToString();
                    label.Text = Mass[i];
                    string engWord = label.Text;
                    string lastword = engWord.Substring(engWord.LastIndexOf('/') + 1);
                    engWord = engWord.Substring(0, engWord.LastIndexOf('/'));
                    listItems.Add(newItem(engWord, lastword));
                }
                return listItems;
            }
            else //if (!File.Exists(fileNamePath) || File.ReadAllText(fileNamePath).Length < 10)
            {
                listItems.Add(newItem("Add something here first", "Сначала что-нибудь добавь"));
                return listItems;
            }
        }
        public static ListViewItem newItem(string _item, string _subItem)
        {
            ListViewItem newWord = new ListViewItem(_item);
            newWord.SubItems.Add(_subItem);
            return newWord;
        }
    }
}
