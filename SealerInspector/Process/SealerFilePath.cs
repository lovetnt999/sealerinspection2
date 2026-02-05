using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvisSealer
{
    public class SealerFilePath
    {
        private static string _RoamingFolder = "";

        public static void InitFolder(string processName)
        {
            _RoamingFolder = string.Format("{0}\\WAMC\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), processName);

            _MakeDirectory(GetLogPath() + "\\");
            _MakeDirectory(GetAlgorithmLogPath() + "\\");
            _MakeDirectory(GetConfigPath() + "\\");
            _MakeDirectory(GetImageFolderPath() + "\\");
            _MakeDirectory(GetTmpFolderPath() + "\\");
        }

        private static void _MakeDirectory(string path)
        {
            bool isEmptyFileName = (path.Substring(path.Length - 1, 1) == "\\");
            string[] split = path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (split != null && split.Length > 0)
            {
                int loopCount = split.Length - 1 + (isEmptyFileName ? 1 : 0);
                string p = string.Empty;
                for (int i = 0; i < loopCount; i++)
                {
                    p += split[i] + "\\";
                    if (!System.IO.Directory.Exists(p))
                    {
                        System.IO.Directory.CreateDirectory(p);
                    }
                }
            }
        }

        public static string GetLogPath()
        {
            return string.Format("{0}\\log", _RoamingFolder);
        }

        public static string GetAlgorithmLogPath()
        {
            return string.Format("{0}\\algorithm_log", _RoamingFolder);
        }

        public static string GetConfigPath()
        {
            return string.Format("{0}\\config", _RoamingFolder);
        }

        public static string GetImageFolderPath()
        {
            return string.Format("{0}\\image", _RoamingFolder);
        }

        public static string GetTmpFolderPath()
        {
            return string.Format("{0}\\tmp", _RoamingFolder);
        }

        public static string GetLiveImagePath(DateTime startTimeUtc)
        {
            string path = string.Format("{0}\\{1}_{2}_{3}\\{4}_live.jpg",
                GetImageFolderPath(),
                startTimeUtc.Year.ToString("0000"), startTimeUtc.Month.ToString("00"), startTimeUtc.Day.ToString("00"),
                startTimeUtc.ToString("HH_mm_ss"));
            _MakeDirectory(path);

            return path;
        }

        public static string GetItemDetailInfoPath(DateTime startTimeUtc, int idx)
        {
            string path = string.Format("{0}\\{1}_{2}_{3}\\{4}_{5}_detail.txt",
                GetImageFolderPath(),
                startTimeUtc.Year.ToString("0000"), startTimeUtc.Month.ToString("00"), startTimeUtc.Day.ToString("00"),
                startTimeUtc.ToString("HH_mm_ss"),
                idx);
            _MakeDirectory(path);

            return path;
        }
    }
}
