using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using inifiles;
using System.Drawing;

namespace EnglishWords
{
    public class VarsConsts
    {
        static IniFile INI = new IniFile(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MyEngWords\setting.ini");
        public static string wordTranslate { get; set; }
        public static string folderPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\MyEngWords\";
            if (Directory.Exists(path))
                return path;
            else
            {
                Directory.CreateDirectory(path);
                return path;
            }
        }
        public static int interval(string timer)
        {
            if (INI.KeyExists(timer, "Timer"))
            {
                int intv = Convert.ToInt32(INI.ReadINI("Timer", timer ));
                return intv;
            }
            else return 180;
        }
        public static bool toggleToggler (string toggleT)
        {
            if (INI.KeyExists(toggleT, "Toggles"))
            {
                bool st =Convert.ToBoolean(INI.ReadINI("Toggles", toggleT));
                return st;
            }
            else return false;
        }
        public static bool toggleMultiSelection()
        {
            if (INI.KeyExists("MultiSelection", "ListSettings"))
            {
                bool st = Convert.ToBoolean(INI.ReadINI("ListSettings", "MultiSelection"));
                return st;
            }
            else return false;
        }
        public static int fontSizeValue()
        {
            if (INI.KeyExists("FontSize", "ListSettings"))
            {
                int st = Convert.ToInt32(INI.ReadINI("ListSettings", "FontSize"));
                return st;
            }
            else return 12;
        }
        public static bool sortState()
        {
            if (INI.KeyExists("SortState", "ListSettings"))
            {
                bool st = Convert.ToBoolean(INI.ReadINI("ListSettings", "SortState"));
                return st;
            }
            else return false;
        }
        public static Point locations { get; set; }
    }
}
