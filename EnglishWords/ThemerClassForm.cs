using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inifiles;
using MetroFramework;
using MetroFramework.Controls;

namespace EnglishWords
{
    public class ThemerClassForm
    {
        IniFile INI;
        public static readonly string[] allColors = new string[] { "Black", "White", "Silver", "Blue", "Green", "Lime", "Teal", "Orange", "Brown", "Pink", "Magenta", "Purple", "Red", "Yellow" };
        public static readonly string[] allThemes = new string[] { "Light", "Dark" };
        public static readonly string[] setThemeKeys = new string[] { "ThemeStyle", "ThemeColor" };
        public static readonly string[] allSettings = new string[] { "Settings" };
        public static readonly Color[] listViewColors = new Color[] { Color.White, Color.Black };
        public static readonly Color[] listViewColors2 = new Color[] { Color.DarkSlateGray, Color.Aqua };
        public ThemerClassForm(IniFile ini)
        { 
            INI = ini;
        }
        public int getThemeFromIniId()
        {
            if (INI.KeyExists(setThemeKeys[0], allSettings[0]))
            {
                string themeIndex = INI.ReadINI(allSettings[0], setThemeKeys[0]);
                int index = 1;
                for (int i = 0; i < allThemes.Length; i++)
                    if (allThemes[i] == themeIndex)
                    {
                        index = i + 1;
                        break;
                    }
                return index;
            }
            else return 1;
        }
        public int getColorFromIniId()
        {
            if (INI.KeyExists(setThemeKeys[1], allSettings[0]))
            {
                string colorIndex = INI.ReadINI(allSettings[0], setThemeKeys[1]);
                int index = 1;
                for (int i = 0; i < allColors.Length; i++)
                    if (allColors[i] == colorIndex)
                    {
                        index = i + 1;
                        break;
                    }
                return index;
            }
            else return 1;
        }
    }
}
