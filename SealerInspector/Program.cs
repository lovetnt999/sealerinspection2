using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using System.ComponentModel;
using Tools.Util.Log;

namespace AvisSealer
{
    static class Program
    {
        public static string PROCESS_NAME = "AVIS_SEALER";

        public static CLog log = null;

        public static readonly DateTime startTime = DateTime.Now;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SealerFilePath.InitFolder(PROCESS_NAME);

            log = CLog.Instance(PROCESS_NAME);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            //            UserLookAndFeel.Default.SetSkinStyle("DevExpress Style");

            System.Reflection.Assembly asm = typeof(DevExpress.UserSkins.AIES).Assembly;
            DevExpress.Skins.SkinManager.Default.RegisterAssembly(asm);
            
            Application.Run(new MainForm());
        }
    }
}

public class SkinRegistration : Component
{
    public SkinRegistration()
    {
        DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.AIES).Assembly);
    }
}
