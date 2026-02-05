using AvisSealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Util.Locale
{
    public class Locale
    {
        public enum EnumLocaleType
        {
            korea = 0,
            english,
            japan,
            chinese
        }

        public static string FONT_FACE { get { return _FONT_FACE; } }
        private static string _FONT_FACE = "나눔고딕";
        public static string PROCESS_NAME = "UnknownLocale";

        private static ResourceSet myResourceSet = null;
        private static ResourceManager resourceManager = null;

        private static string CurrentLocale
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
                }

                _SetCulture(value);
            }
        }

        public static EnumLocaleType CurrentLocaleType
        {
            get
            {
                string name = CurrentLocale;
                if (name.Contains("ko")) return EnumLocaleType.korea;
                else if (name.Contains("ja")) return EnumLocaleType.japan;
                else if (name.Contains("zh")) return EnumLocaleType.chinese;
                else return EnumLocaleType.english;
            }

            set
            {
                if (value == EnumLocaleType.korea) CurrentLocale = "ko-KR";
                else if (value == EnumLocaleType.japan) CurrentLocale = "ja-JP";
                else if (value == EnumLocaleType.chinese) CurrentLocale = "zh-cn";
                else CurrentLocale = "en-US";
            }
        }

        public static string CurrentLocaleText
        {
            get
            {
                return GetLocalText(CurrentLocaleType);
            }
        }



        public static string GetString(string value)
        {
            if (myResourceSet == null)
            {
                _SetCulture(CurrentLocale);
            }

            return myResourceSet.GetString(value);
        }

        public static string GetLocalText(EnumLocaleType type)
        {
            if (type == EnumLocaleType.korea) return "한글";
            else if (type == EnumLocaleType.japan) return "日本語";
            else if (type == EnumLocaleType.chinese) return "中華門(中华门)";
            else return "English";
        }

        private static bool _SetCulture(string langCode)
        {
            if (langCode == null) langCode = string.Empty;
            if (resourceManager == null)
            {
                resourceManager = new System.Resources.ResourceManager(PROCESS_NAME + ".LangRes.Lang", Assembly.GetExecutingAssembly());
            }

            bool result = false;
            if (langCode.Contains("ko"))
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ko-KR");
                Locale._FONT_FACE = "나눔고딕";

                result = true;
            }
            else if (langCode.Contains("ja"))
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ja-JP");
                Locale._FONT_FACE = "나눔고딕";

                result = true;
            }
            else if (langCode.Contains("zh"))
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-cn");
                Locale._FONT_FACE = "나눔고딕";

                result = true;
            }
            else// if (langCode.Contains("en"))
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Locale._FONT_FACE = "나눔고딕";
                //  fontFace = "Broadway";

                if (langCode.Contains("en-US"))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            myResourceSet = resourceManager.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, false);


            //set Culture of current Thread
            System.Threading.Thread.CurrentThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            //set Culture for further Threads
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            return result;
        }
    }
}
