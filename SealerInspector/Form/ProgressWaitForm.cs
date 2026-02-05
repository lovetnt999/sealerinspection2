using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraWaitForm;
using DevExpress.XtraSplashScreen;

namespace AvisSealer
{
    public partial class ProgressWaitForm : WaitForm
    {
        public ProgressWaitForm()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {
        }

        public static void ShowForm(System.Windows.Forms.Form form, string caption, string descript)
        {
            SplashScreenManager.ShowForm(form, typeof(ProgressWaitForm), true, true, false);
            SplashScreenManager.Default.SetWaitFormCaption(caption);
            SetDescript(descript);
        }

        public static void HideForm()
        {
            SplashScreenManager.CloseForm();
        }

        public static void SetDescript(string descript)
        {
            if (SplashScreenManager.Default != null)
            {
                SplashScreenManager.Default.SetWaitFormDescription(descript);
            }
        }
    }
}