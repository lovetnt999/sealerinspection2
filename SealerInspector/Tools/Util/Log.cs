using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Util.Log
{
    public class CLog
    {
        public static string LOG_DIRECTORY = string.Empty;

        private readonly string _LOG_DIRECTORY_NAME = string.Empty;
        private static string lastLogMessage = string.Empty;
        private static DateTime lastLogSavedTime = DateTime.MinValue;
        private static string lastErrorLogMessage = string.Empty;
        private static DateTime lastErrorLogSavedTime = DateTime.MinValue;
        private static int logSaveInterval = 1;
        private static int logErrSaveInterval = 1;

        private static volatile CLog _instance;
        private static object _syncRoot = new object();

        public static CLog Instance(string processName = "", int saveInerval = 0, int errInterval = 0)
        {
            lock (_syncRoot)
            {
                if (_instance == null)
                {
                    _instance = new CLog(processName);
                    _instance._InitLog();

                    if (saveInerval > 0) CLog.logSaveInterval = saveInerval;
                    if (errInterval > 0) CLog.logErrSaveInterval = errInterval;
                }
            }

            return _instance;
        }

        private CLog(string logDirectoryName)
        {
            this._LOG_DIRECTORY_NAME = logDirectoryName;
        }


        private void _InitLog()
        {
            try
            {
                AjinUtil.Logger.Init("WAMC\\" + _LOG_DIRECTORY_NAME, _LOG_DIRECTORY_NAME);
                LOG_DIRECTORY = AjinUtil.DefaultName.GetAppDataFolderPath("log");
            }
            catch (Exception)
            { }
        }

        public static void Log(string msg)
        {
            if (lastLogMessage != msg || DateTime.Now.Subtract(lastLogSavedTime) > new TimeSpan(0, 0, logSaveInterval))
            {
                string l = msg.Replace("\r", "  /  ");

                AjinUtil.Logger.Get().Log(l);
                lastLogMessage = l;
                lastLogSavedTime = DateTime.Now;
            }
        }

        public static void LogSys(string msg)
        {
            if (lastErrorLogMessage != msg || DateTime.Now.Subtract(lastErrorLogSavedTime) > new TimeSpan(0, 0, 0, 0, 100))
            {
                AjinUtil.Logger.Get().LogSystem(msg);
            }
        }

        public static void LogErr(string msg)
        {
            if (/*lastErrorLogMessage != msg || */DateTime.Now.Subtract(lastErrorLogSavedTime) > new TimeSpan(0, 0, logErrSaveInterval))
            {
                AjinUtil.Logger.Get().LogErr(msg);
                lastErrorLogMessage = msg;
                lastErrorLogSavedTime = DateTime.Now;
            }
        }

        public static void LogErr(System.Exception ex)
        {
            LogErr(string.Format("Exception in '{0}'. ExceptionMessage({1})", ex.Source, ex.Message));
        }

        public static void LogErr(System.Exception ex, string msg)
        {
            LogErr(string.Format("Exception in '{0}'. ExceptionMessage({1}), msg({2})", ex.Source, ex.Message, msg));
        }
    }
}
