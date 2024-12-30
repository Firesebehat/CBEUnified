using System;
using System.Configuration;

namespace eTax
{
    public static class AppStart
    {
        public static bool isDireDawaEnabled { get; set; }
        public static bool isSomaliEnabled { get; set; }
        public static bool isAddisEnabled { get; set; }
        public static string appHeaderAmh { get; set; }
        public static string appHeaderEng { get; set; }

        public static string appHeaderAmh1 { get; set; }
        public static string appHeaderAmh2 { get; set; }
        public static string appHeaderEnglish1 { get; set; }
        public static string appHeaderEnglish2 { get; set; }
        public static string logoURL { get; set; }
        public static string logoHeaderURL { get; set; }

        public static void setAppName()
        {
            isDireDawaEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["DireDawaEnabled"]);
            isSomaliEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["SomaliEnabled"]);
            isAddisEnabled = Convert.ToBoolean(ConfigurationManager.AppSettings["AddisEnabled"]);

            if (isDireDawaEnabled)
            {
                //diredawa
                appHeaderAmh = ConfigurationManager.AppSettings["AppHeaderAmhDireDawa"];
                appHeaderEng = ConfigurationManager.AppSettings["AppHeaderEnglishDireDawa"];
                logoURL = ConfigurationManager.AppSettings["logoURLDireDawa"];
                appHeaderEnglish2 = ConfigurationManager.AppSettings["AppHeaderEnglishDireDawa1"];


            }
            else if (isSomaliEnabled)
            {
                //Addis Ababa
                appHeaderAmh = ConfigurationManager.AppSettings["AppHeaderAmhSomali"];
                appHeaderEng = ConfigurationManager.AppSettings["AppHeaderEnglishSomali"];
                logoURL = ConfigurationManager.AppSettings["logoURLSomali"];
                appHeaderEnglish2 = ConfigurationManager.AppSettings["AppHeaderEnglishSomali1"];
            }
            else if (isAddisEnabled)
            {
                //Addis Ababa
                appHeaderAmh = ConfigurationManager.AppSettings["AppHeaderAmhAA"];
                appHeaderEng = ConfigurationManager.AppSettings["AppHeaderEnglishAA"];
                logoURL = ConfigurationManager.AppSettings["logoURLAA"];
                appHeaderEnglish2 = ConfigurationManager.AppSettings["AppHeaderEnglishAA1"];
            }

        }
    }
}